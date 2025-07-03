import Link from "@/components/Link";
import config from "@/config";
import { useDocumentTitle } from "@/hooks";
import { useDispatch, useSelector } from 'react-redux';
import { login as loginThunk } from '@/store/slices/authSlice';
import { RootState } from '@/store';
import cookieUtils from "@/services/cookieUtils";
import { message } from "antd";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { LogoTypo } from "./Login.styled";
import { PageEnum } from "@/utils/enum";
import { LoginFields } from "@/components/AuthForm/AuthForm.fields";
import AuthForm from "@/components/AuthForm";
import { AppDispatch } from '@/store';


const Login = () => {
    useDocumentTitle('Login | Aphromas Diamond');

    const dispatch: AppDispatch = useDispatch();
    const navigate = useNavigate();
    const [messageApi, contextHolder] = message.useMessage();
    const { loading, token } = useSelector((state: RootState) => state.auth);

    useEffect(() => {
        if (token) {
            cookieUtils.setToken(token);
            console.log("Token set in cookie from useEffect:", cookieUtils.getToken());
        }
    }, [token]);

    const onFinish = async (values: unknown) => {
        const resultAction = await dispatch(loginThunk(values));
        interface LoginPayload {
            token: string;
            user: {
                email: string;
                fullName: string;
                role: string;
                userId: string;
            };
        }
        const action = resultAction as { payload: LoginPayload };
        console.log("Login thunk result:", resultAction);
        if (loginThunk.fulfilled.match(resultAction)) {
            console.log("Login fulfilled payload:", action.payload);
            document.cookie = `token=${action.payload.token}; path=/`;
            console.log("Manual set token cookie:", document.cookie);
            await messageApi.success('Login successfully');
            navigate(config.routes.public.home);
        } else {
            console.log("Login failed result:", action);
            const errorMsg = typeof action.payload === 'string' ? action.payload : 'Login failed';
            messageApi.error(errorMsg);
        }
    };

    const redirect = {
        description: "Don't have account?",
        title: 'Register now',
        url: config.routes.public.register,
    };

    const description = (
        <Link to={config.routes.public.home} underline scroll>
            <LogoTypo>Aphromas Diamond</LogoTypo>
        </Link>
    )

    return (
        <>
            {contextHolder}
            <AuthForm
                page={PageEnum.LOGIN}
                formTitle="Welcome back"
                buttonTitle="Login"
                fields={LoginFields}
                description={description}
                redirect={redirect}
                onFinish={onFinish}
                isSubmitting={loading}
            />
        </>
    )
}

export default Login;