import config from "@/config"
import { useSelector } from 'react-redux';
import { RootState } from '@/store';
import Login from "@/pages/Login"
import Register from "@/pages/Register"
import { Navigate, Outlet } from "react-router-dom"

const AuthRouter = () => {
    const { user } = useSelector((state: RootState) => state.auth);
    const role = user?.role || null;
    return !role ? <Outlet/> : <Navigate to='/' />;
}

const AuthRoutes = {
    element: <AuthRouter />,
    children: [
        { path: config.routes.public.login, element: <Login /> },
        { path: config.routes.public.register, element: <Register /> }
    ]
}

export default AuthRoutes;