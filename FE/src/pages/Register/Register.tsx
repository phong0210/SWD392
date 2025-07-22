import AuthForm from "@/components/AuthForm";
import { RegisterFields, OtpFields } from "@/components/AuthForm/AuthForm.fields";
import config from "@/config";
import { useDocumentTitle } from "@/hooks";
import { register, confirmRegistration } from "@/services/authAPI";
import { PageEnum } from "@/utils/enum";
import { message } from "antd";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

interface RegisterFormValues {
    Name: string;
    Email: string;
    PhoneNumber: string;
    Password: string;
}

interface OtpFormValues {
    otp: string;
}

const Register = () => {
    useDocumentTitle('Register | Aphromas Diamond');

    const [isSubmitting, setIsSubmitting] = useState(false);
    const [step, setStep] = useState(1); // 1 for registration, 2 for OTP verification
    const [email, setEmail] = useState(''); // To store email for OTP verification
    const [registrationData, setRegistrationData] = useState<RegisterFormValues | null>(null); // To store registration data temporarily
    const navigate = useNavigate();

    const [messageApi, contextHolder] = message.useMessage();

    const onFinish = async (values: unknown) => {
        try {
            setIsSubmitting(true);
            if (step === 1) {
                const formValues = values as RegisterFormValues;
                const payload = {
                    Name: formValues.Name,
                    Email: formValues.Email,
                    Phone: formValues.PhoneNumber,
                    Password: formValues.Password,
                    Address: ""
                };
                setRegistrationData(formValues); // Store full registration data
                setEmail(formValues.Email); // Store email for OTP step

                const response = await register(payload);

                if (!response.data || response.status !== 200) {
                    throw new Error(response.data?.error || 'Registration failed');
                }

                if (response.data.success) {
                    await messageApi.success('OTP sent to your email. Please verify to complete registration.');
                    setStep(2); // Move to OTP verification step
                } else {
                    throw new Error(response.data.error || 'Registration failed');
                }
            } else if (step === 2) {
                const otpValues = values as OtpFormValues;
                if (!registrationData) {
                    throw new Error('Registration data missing. Please restart registration.');
                }
                const payload = {
                    email: email,
                    otp: otpValues.otp,
                };
                const response = await confirmRegistration(payload);

                if (!response.data || response.status !== 200) {
                    throw new Error(response.data?.error || 'OTP verification failed');
                }

                if (response.data.success) {
                    await messageApi.success('Registration successful!');
                    navigate(config.routes.public.login);
                } else {
                    throw new Error(response.data.error || 'OTP verification failed');
                }
            }
        } catch (error: unknown) {
            if (error && typeof error === 'object' && 'response' in error) {
                const errorResponse = error as { response?: { data?: { error?: string } } };
                if (errorResponse.response?.data?.error) {
                    messageApi.error(errorResponse.response.data.error);
                } else {
                    messageApi.error('Operation failed. Please try again.');
                }
            } else if (error instanceof Error) {
                messageApi.error(error.message);
            } else {
                messageApi.error('Operation failed. Please try again.');
            }
        } finally {
            setIsSubmitting(false);
        }
    };

    const redirect = {
        description: 'Have an account?',
        title: 'Login now',
        url: config.routes.public.login
    };

    return (
        <>
            {contextHolder}
            <AuthForm
                page={PageEnum.REGISTER}
                formTitle={step === 1 ? "Register" : "Verify OTP"}
                buttonTitle={step === 1 ? "Register" : "Verify OTP"}
                fields={step === 1 ? RegisterFields : OtpFields}
                redirect={redirect}
                onFinish={onFinish}
                reverse
                isSubmitting={isSubmitting}
            />
        </>
    );
};

export default Register;