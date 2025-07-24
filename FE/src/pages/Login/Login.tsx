import Link from "@/components/Link";
import config from "@/config";
import { useDocumentTitle } from "@/hooks";
import { useDispatch, useSelector } from 'react-redux';
import { login as loginThunk } from '@/store/slices/authSlice';
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

// Login step types
enum LoginStep {
    LOGIN = 1,
    OTP_VERIFICATION = 2,
    FORGOT_PASSWORD_EMAIL = 3,
    FORGOT_PASSWORD_OTP = 4,
    RESET_PASSWORD = 5
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

    // State management
    const [step, setStep] = useState<LoginStep>(LoginStep.LOGIN);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [email, setEmail] = useState('');
    const [loginData, setLoginData] = useState<LoginFormValues | null>(null);
    const [forgotPasswordOtp, setForgotPasswordOtp] = useState('');

    const onFinish = async (values: unknown) => {
        try {
            setIsSubmitting(true);

            switch (step) {
                case LoginStep.LOGIN:
                    await handleLogin(values as LoginFormValues);
                    break;
                case LoginStep.OTP_VERIFICATION:
                    await handleOtpVerification(values as OtpFormValues);
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
    setLoginData(values);
    setEmail(values.Email);

    // Directly attempt login without OTP
    const resultAction = await dispatch(loginThunk(values));
    
    if (loginThunk.fulfilled.match(resultAction)) {
        await messageApi.success('Login successfully');
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
};

    const handleOtpVerification = async (values: OtpFormValues) => {
        if (!loginData) {
            throw new Error('Login data missing. Please restart login.');
        }

        const verifyResponse = await verifyLoginOtp({
            email: email,
            otp: values.otp,
        });

        if (!verifyResponse.data || verifyResponse.status !== 200) {
            throw new Error(verifyResponse.data?.error || 'OTP verification failed');
        }

        if (verifyResponse.data.success) {
            // Now proceed with actual login
            const resultAction = await dispatch(loginThunk(loginData));
            if (loginThunk.fulfilled.match(resultAction)) {
                await messageApi.success('Login successfully');
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
        } else {
            throw new Error(verifyResponse.data.error || 'OTP verification failed');
        }
    };

    const handleForgotPasswordEmail = async (values: ForgotPasswordEmailValues) => {
        setEmail(values.Email);

        // Use requestPasswordReset instead of sendForgotPasswordOtp
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

        // Use confirmPasswordReset with the stored email and new password
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
        setLoginData(null);
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
            case LoginStep.OTP_VERIFICATION:
                return {
                    formTitle: "Verify Login OTP",
                    buttonTitle: "Verify OTP",
                    fields: OtpFields,
                    showForgotPassword: false,
                    showBackToLogin: true,
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
            />
        </>
    );
};

export default Login;