import Link from "@/components/Link";
import config from "@/config";
import { useDocumentTitle } from "@/hooks";
import { useDispatch, useSelector } from 'react-redux';
import { login as loginThunk } from '@/store/slices/authSlice';
import { RootState } from '@/store';
import { message } from "antd";
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
    const { loading } = useSelector((state: RootState) => state.auth);



    const onFinish = async (values: unknown) => {
        const resultAction = await dispatch(loginThunk(values));
        if (loginThunk.fulfilled.match(resultAction)) {
            await messageApi.success('Login successfully');
            const user = resultAction.payload.user;
            switch (user.role) {
                case 'HeadOfficeAdmin':
                    navigate(config.routes.admin.dashboard);
                    break;
                case 'SalesStaff':
                    navigate(config.routes.salesStaff.dashboard);
                    break;
                case 'DeliveryStaff':
                    navigate(config.routes.deliStaff.dashboard);
                    break;
                default:
                    navigate(config.routes.public.home);
            }
        } else {
            const errorMsg = typeof resultAction.payload === 'string' ? resultAction.payload : 'Login failed';
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