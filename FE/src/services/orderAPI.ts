import { get, post, put, remove } from "./apiCaller"

export type OrderDetailResponseFE = {
    Id: string;
    OrderId: string;
    UnitPrice: number;
    Quantity: number;
}

export type PaymentResponseFE = {
    Id: string;
    OrderId: string;
    Method: string;
    Date: string;
    Amount: number;
    Status: number;
}

export type DeliveryResponseFE = {
    Id: string;
    OrderId: string;
    DispatchTime?: string;
    DeliveryTime?: string;
    ShippingAddress: string;
    Status: number;
}

export type OrderResponseFE = {
    Id: string;
    UserId: string;
    TotalPrice: number;
    OrderDate: string;
    VipApplied: boolean;
    Status: number;
    SaleStaff: string;
    OrderDetails: OrderDetailResponseFE[];
    Delivery?: DeliveryResponseFE;
    Payments: PaymentResponseFE[];
}

export type OrderItemRequest = {
    ProductId: string; // Guid in C# maps to string in TS
    Quantity: number;
    UnitPrice: number;
}

export type CreateOrderRequest = {
    CustomerId: string; // Guid in C# maps to string in TS
    SaleStaff: string;
    OrderItems: OrderItemRequest[];
}

export type UpdateOrderRequest = {
    Status: number;
    SaleStaff: string;
    VipApplied: boolean;
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

export const orderDetail = (id: string) => { // ID is Guid, so string
    return get(`/api/orders/${id}`);
}

export const orderRelation = (id: string) => { // ID is Guid, so string
    return get(`/api/orders/${id}/user-info`);
}

export const createOrder = (order: CreateOrderRequest) => {
    return post(`/api/Orders/create`, order); // Fixed endpoint to match your curl example
}

export const updateOrder = (id: string, order: UpdateOrderRequest) => { // ID is Guid, so string
    return put(`/api/orders/${id}`, order);
}

export const deleteOrder = (id: string) => { // ID is Guid, so string
    return remove(`/api/orders/${id}`);
}