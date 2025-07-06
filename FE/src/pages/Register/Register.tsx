import AuthForm from "@/components/AuthForm";
import { RegisterFields } from "@/components/AuthForm/AuthForm.fields";
import config from "@/config";
import { useDocumentTitle } from "@/hooks";
import { register } from "@/services/authAPI";
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

const Register = () => {
    useDocumentTitle('Register | Aphromas Diamond');

    const[isSubmitting, setIsSubmitting] = useState(false);
    const navigate = useNavigate();

    const[messageApi, contextHolder] = message.useMessage();

    const onFinish = async (values: unknown) => {
        try {
            setIsSubmitting(true);
            const formValues = values as RegisterFormValues;
            const payload = {
                Name: formValues.Name,
                Email: formValues.Email,
                Phone: formValues.PhoneNumber,
                Password: formValues.Password,
                Address: ""
            };
            const response = await register(payload);
            
            if (!response.data || response.status !== 200) {
                throw new Error(response.data?.error || 'Registration failed');
            }
            
            if (response.data.success) {
                await messageApi.success('Register Successfully!');
                navigate(config.routes.public.login);
            } else {
                throw new Error(response.data.error || 'Registration failed');
            }
        } catch (error: unknown) {
            if(error && typeof error === 'object' && 'response' in error) {
                const errorResponse = error as { response?: { data?: { error?: string } } };
                if(errorResponse.response?.data?.error) {
                    messageApi.error(errorResponse.response.data.error);
                } else {
                    messageApi.error('Registration failed. Please try again.');
                }
            } else if(error instanceof Error) {
                messageApi.error(error.message);
            } else {
                messageApi.error('Registration failed. Please try again.');
            }
        } finally {
            setIsSubmitting(false);
        }
    }

    const redirect = {
        description: 'Have an account?',
        title: 'Login now',
        url: config.routes.public.login
    }
    
    return (
        <>
            {contextHolder}
            <AuthForm
                page={PageEnum.REGISTER}
                formTitle="Register"
                buttonTitle="Register"
                fields={RegisterFields}
                redirect={redirect}
                onFinish={onFinish}
                reverse
                isSubmitting={isSubmitting}
            />
        </>
    )
}

export default Register;