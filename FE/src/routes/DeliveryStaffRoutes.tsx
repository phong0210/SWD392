import config from "@/config"
import { Navigate } from "react-router-dom";
import { useSelector } from 'react-redux';
import { RootState } from '@/store';

import StaffLayout from "@/layouts/StaffLayout"

// import DeliveryReport from "@/pages/Staff/DeliveryReport/DeliveryPending";
import DeliveryDelivering from "@/pages/Staff/DeliveryReport/DeliveryDelivering";
import DeliveryDelivered from "@/pages/Staff/DeliveryReport/DeliveryDelivered";
import DeliveryReturn from "@/pages/Staff/DeliveryReport/DeliveryReturn";
import DeliveryCompleted from "@/pages/Staff/DeliveryReport/DeliveryCompleted";
import DeliveryPending from "@/pages/Staff/DeliveryReport/DeliveryPending";
import { Role } from "@/utils/enum";


const DeliveryStaffRouter = () => {
    const { user } = useSelector((state: RootState) => state.auth);
    const role = user?.role || null;
    return role === Role.DeliveryStaff ? <StaffLayout /> : <Navigate to="/" />;
}

const deliStaffRoutes = [
    { path: config.routes.deliStaff.deliveryPending, element: <DeliveryPending /> },
    { path: config.routes.deliStaff.deliveryDelivering, element: <DeliveryDelivering /> },
    { path: config.routes.deliStaff.deliveryDelivered, element: <DeliveryDelivered /> },
    { path: config.routes.deliStaff.deliveryReturn, element: <DeliveryReturn /> },
    { path: config.routes.deliStaff.deliveryCompleted, element: <DeliveryCompleted /> }
];

const DeliveryStaffRoutes = {
    path: config.routes.deliStaff.dashboard,
    element: <DeliveryStaffRouter />,
    children: deliStaffRoutes,
}

export default DeliveryStaffRoutes;