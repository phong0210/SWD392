import { get, post, put, remove } from "./apiCaller"

export type OrderAPIProps = {
    OrderID?: number;
    OrderDate: Date;
    CompleteDate: Date;
    CustomerID: number | null;
    NameReceived: string;
    PhoneNumber: string;
    Email: string;
    Address: string;
    PaymentID?: string;
    IsPayed: boolean;
    Shippingfee?: number;
    OrderStatus: string;
    AccountDeliveryID?: number;
    AccountSaleID?: number;
    ReasonReturn?: string;
    Note?: string;
    IsActive: boolean;
    VoucherID?: number;
}

export const showAllOrder = () => {
    return get(`/api/orders`);
}

export const showOrdersPage = () => {
    return get(`/api/orders`);
}

export const showReveneSummary = () => {
    return post(`/api/orders/summarize`);
}

export const orderDetail = (id: number) => {
    return get(`/api/orders/${id}`);
}

export const orderRelation = (id: number) => {
    return get(`/api/orders/${id}/user-info`);
}

export const createOrder = (order: object) => {
    return post(`/api/orders`, order);
}

export const updateOrder = (id: number, order: Partial<OrderAPIProps>) => {
    return put(`/api/orders/${id}`, order);
}

export const deleteOrder = (id: number) => {
    return remove(`/api/orders/${id}`);
}