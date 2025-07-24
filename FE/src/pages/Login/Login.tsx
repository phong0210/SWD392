import Link from "@/components/Link";
import config from "@/config";
import { useDocumentTitle } from "@/hooks";
import { useDispatch, useSelector } from 'react-redux';
import { login as loginThunk, googleLogin } from '@/store/slices/authSlice';
import { RootState } from '@/store';
import { message } from "antd";
import { useNavigate } from "react-router-dom";
import { LogoTypo } from "./Login.styled";
import { PageEnum, Role } from "@/utils/enum";
import { LoginFields, OtpFields, ForgotPasswordFields, ResetPasswordFields } from "@/components/AuthForm/AuthForm.fields";
import AuthForm from "@/components/AuthForm";
import { AppDispatch } from '@/store';
import { useState } from "react";
import { 
    requestPasswordReset, 
    confirmPasswordReset 
} from "@/services/authAPI";
import { GoogleLogin, CredentialResponse } from "@react-oauth/google";
import { FcGoogle } from 'react-icons/fc';
import { loadCart } from "@/store/slices/cartSlice";

// Login step types - removed OTP_VERIFICATION since login is direct
enum LoginStep {
    LOGIN = 1,
    FORGOT_PASSWORD_EMAIL = 2,
    FORGOT_PASSWORD_OTP = 3,
    RESET_PASSWORD = 4
}

interface LoginFormValues {
    Email: string;
    Password: string;
}

interface OtpFormValues {
    otp: string;
}

interface ForgotPasswordEmailValues {
    Email: string;
}

interface ResetPasswordValues {
    Password: string;
    ConfirmPassword: string;
}

const Login = () => {
    useDocumentTitle('Login | Aphromas Diamond');

    const dispatch: AppDispatch = useDispatch();
    const navigate = useNavigate();
    const [messageApi, contextHolder] = message.useMessage();
    const { loading } = useSelector((state: RootState) => state.auth);

    // State management - removed loginData since it's not needed for direct login
    const [step, setStep] = useState<LoginStep>(LoginStep.LOGIN);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [email, setEmail] = useState('');
    const [forgotPasswordOtp, setForgotPasswordOtp] = useState('');

    const onFinish = async (values: unknown) => {
        try {
            setIsSubmitting(true);

            switch (step) {
                case LoginStep.LOGIN:
                    await handleLogin(values as LoginFormValues);
                    break;
                case LoginStep.FORGOT_PASSWORD_EMAIL:
                    await handleForgotPasswordEmail(values as ForgotPasswordEmailValues);
                    break;
                case LoginStep.FORGOT_PASSWORD_OTP:
                    await handleForgotPasswordOtp(values as OtpFormValues);
                    break;
                case LoginStep.RESET_PASSWORD:
                    await handleResetPassword(values as ResetPasswordValues);
                    break;
            }
        } catch (error: unknown) {
            handleError(error);
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleLogin = async (values: LoginFormValues) => {
        setEmail(values.Email);

        const resultAction = await dispatch(loginThunk(values));

        // Directly attempt login without OTP
        if (loginThunk.fulfilled.match(resultAction)) {
      await messageApi.success("Login successful");
      dispatch(loadCart());
      const user = resultAction.payload.user;
      console.log("User Check:", user);

      switch (user.role) {
        case Role.HeadOfficeAdmin:
          navigate(config.routes.admin.dashboard, { replace: true });
          break;
        case Role.SalesStaff:
          navigate(config.routes.salesStaff.dashboard, { replace: true });
          break;
        case Role.DeliveryStaff:
          navigate(config.routes.deliStaff.dashboard, { replace: true });
          break;
        default:
          navigate(config.routes.public.home, { replace: true });
      }
    } else {
      // Handle error case
      const errorMsg =
        typeof resultAction.payload === "string"
          ? resultAction.payload // If payload is a string, use it directly
          :  "Login failed"; // Fallback to generic message
      await messageApi.error(errorMsg); // Use error method for error messages
    }
  };

    const handleForgotPasswordEmail = async (values: ForgotPasswordEmailValues) => {
        setEmail(values.Email);

        const response = await requestPasswordReset({ email: values.Email });
        
        if (response.status === 200) {
            await messageApi.success('OTP sent to your email. Please check your inbox.');
            setStep(LoginStep.FORGOT_PASSWORD_OTP);
        } else {
            throw new Error(response.data?.message || 'Failed to send OTP. Please try again.');
        }
    };

    const handleForgotPasswordOtp = async (values: OtpFormValues) => {
        setForgotPasswordOtp(values.otp);
        await messageApi.success('OTP verified. Please enter your new password.');
        setStep(LoginStep.RESET_PASSWORD);
    };

    const handleResetPassword = async (values: ResetPasswordValues) => {
        if (values.Password !== values.ConfirmPassword) {
            throw new Error('Passwords do not match');
        }

        const response = await confirmPasswordReset({
            email: email,
            otp: forgotPasswordOtp,
            newPassword: values.Password,
        });

        if (response.status === 200) {
            await messageApi.success('Password changed successfully!');
            setTimeout(() => {
                resetToLogin();
            }, 2000);
        } else {
            throw new Error(response.data?.message || 'Failed to change password. Invalid OTP or new password.');
        }
    };

    const handleError = (error: unknown) => {
        if (error && typeof error === 'object' && 'response' in error) {
            const errorResponse = error as { response?: { data?: { error?: string; message?: string } } };
            if (errorResponse.response?.data?.error) {
                messageApi.error(errorResponse.response.data.error);
            } else if (errorResponse.response?.data?.message) {
                messageApi.error(errorResponse.response.data.message);
            } else {
                messageApi.error('Operation failed. Please try again.');
            }
        } else if (error instanceof Error) {
            messageApi.error(error.message);
        } else {
            messageApi.error('Operation failed. Please try again.');
        }
    };

    const resetToLogin = () => {
        setStep(LoginStep.LOGIN);
        setEmail('');
        setForgotPasswordOtp('');
    };

    const goToForgotPassword = () => {
        setStep(LoginStep.FORGOT_PASSWORD_EMAIL);
    };

    const goBackToLogin = () => {
        resetToLogin();
    };

    // Get form configuration based on current step
    const getFormConfig = () => {
        switch (step) {
            case LoginStep.LOGIN:
                return {
                    formTitle: "Welcome back",
                    buttonTitle: "Login",
                    fields: LoginFields,
                    showForgotPassword: true,
                    showBackToLogin: false,
                };
            case LoginStep.FORGOT_PASSWORD_EMAIL:
                return {
                    formTitle: "Forgot Password",
                    buttonTitle: "Send OTP",
                    fields: ForgotPasswordFields,
                    showForgotPassword: false,
                    showBackToLogin: true,
                };
            case LoginStep.FORGOT_PASSWORD_OTP:
                return {
                    formTitle: "Verify Reset OTP",
                    buttonTitle: "Verify OTP",
                    fields: OtpFields,
                    showForgotPassword: false,
                    showBackToLogin: true,
                };
            case LoginStep.RESET_PASSWORD:
                return {
                    formTitle: "Reset Password",
                    buttonTitle: "Reset Password",
                    fields: ResetPasswordFields,
                    showForgotPassword: false,
                    showBackToLogin: false,
                };
            default:
                return {
                    formTitle: "Welcome back",
                    buttonTitle: "Login",
                    fields: LoginFields,
                    showForgotPassword: true,
                    showBackToLogin: false,
                };
        }
    };

    const formConfig = getFormConfig();

    const redirect = {
        description: "Don't have account?",
        title: 'Register now',
        url: config.routes.public.register,
    };

    const description = (
        <Link to={config.routes.public.home} underline scroll>
            <LogoTypo>Aphromas Diamond</LogoTypo>
        </Link>
    );

    const handleGoogleLoginSuccess = async (credentialResponse: CredentialResponse) => {
        if (credentialResponse.credential) {
            const resultAction = await dispatch(googleLogin(credentialResponse.credential));
            if (googleLogin.fulfilled.match(resultAction)) {
                await messageApi.success('Login successfully');
                dispatch(loadCart());
                const user = resultAction.payload.user;
                console.log("User Check:", user);
                
                switch (user.role) {
                    case Role.HeadOfficeAdmin:
                        navigate(config.routes.admin.dashboard, { replace: true });
                        break;
                    case Role.SalesStaff:
                        navigate(config.routes.salesStaff.dashboard, { replace: true });
                        break;
                    case Role.DeliveryStaff:
                        navigate(config.routes.deliStaff.dashboard, { replace: true });
                        break;
                    default:
                        navigate(config.routes.public.home, { replace: true });
                }
            } else {
                const errorMsg = typeof resultAction.payload === 'string' 
                    ? resultAction.payload 
                    : 'Login failed';
                throw new Error(errorMsg);
            }
        }
    };

    const handleGoogleLoginError = () => {
        messageApi.error('Google login failed. Please try again.');
    };

    const googleLoginButton = (
        <div style={{ marginTop: '24px' }}>
            {/* Hidden Google Login component for handling authentication */}
            <div style={{ display: 'none' }}>
                <GoogleLogin
                    onSuccess={handleGoogleLoginSuccess}
                    onError={handleGoogleLoginError}
                />
            </div>
            
            {/* Custom styled Google button */}
            <div 
                onClick={() => {
                    // Trigger the hidden Google login
                    const googleLoginButton = document.querySelector('[role="button"][data-testid]:not([style*="display: none"])') as HTMLButtonElement;
                    if (googleLoginButton) {
                        googleLoginButton.click();
                    } else {
                        // Fallback: create a temporary GoogleLogin component and trigger it
                        const tempDiv = document.createElement('div');
                        tempDiv.style.position = 'absolute';
                        tempDiv.style.left = '-9999px';
                        document.body.appendChild(tempDiv);
                        
                        // You might need to implement a more sophisticated solution here
                        // For now, we'll use the styled button approach
                        messageApi.info('Please use the Google login button that appears.');
                    }
                }}
                style={{
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    columnGap: '8px',
                    height: '50px',
                    border: '1px solid #d9d9d9',
                    borderRadius: '6px',
                    backgroundColor: '#fff',
                    cursor: 'pointer',
                    transition: 'all 0.3s ease',
                    fontSize: '1.2rem',
                    fontWeight: '500',
                }}
                onMouseEnter={(e) => {
                    e.currentTarget.style.borderColor = '#1890ff';
                    e.currentTarget.style.color = '#1890ff';
                }}
                onMouseLeave={(e) => {
                    e.currentTarget.style.borderColor = '#d9d9d9';
                    e.currentTarget.style.color = '#666';
                }}
            >
                <FcGoogle style={{ fontSize: '1.8rem' }} />
                <span>Continue with Google</span>
            </div>
            
            {/* Actual Google Login component positioned to overlay the custom button */}
            <div style={{ marginTop: '-50px', opacity: 0, pointerEvents: 'auto' }}>
                <GoogleLogin
                    onSuccess={handleGoogleLoginSuccess}
                    onError={handleGoogleLoginError}
                    theme="outline"
                    size="large"
                    width="100%"
                />
            </div>
        </div>
    );

    return (
        <>
            {contextHolder}
            <AuthForm
                page={PageEnum.LOGIN}
                formTitle={formConfig.formTitle}
                buttonTitle={formConfig.buttonTitle}
                fields={formConfig.fields}
                description={description}
                redirect={redirect}
                onFinish={onFinish}
                isSubmitting={isSubmitting || loading}
                showForgotPassword={formConfig.showForgotPassword}
                showBackToLogin={formConfig.showBackToLogin}
                onForgotPasswordClick={goToForgotPassword}
                onBackToLoginClick={goBackToLogin}
                googleLoginButton={googleLoginButton}
            />
        </>
    );
};

export default Login;