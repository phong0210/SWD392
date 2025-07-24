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


// src/services/orderAPI.ts
export interface OrderResponseFE {
    // subTotal: any;
    id: string;
    userId: string;
    totalPrice: number;
    orderDate: string;
    vipApplied: boolean;
    status: number;
    saleStaff: string;
    orderDetails: Array<{
        id: string;
        orderId: string;
        unitPrice: number;
        quantity: number;
    }>;
    delivery: {
        id: string;
        orderId: string;
        dispatchTime: string;
        deliveryTime: string;
        shippingAddress: string;
        status: number;
    };
    payments: Array<{
        id: string;
        orderId: string;
        method: string;
        date: string;
        amount: number;
        status: number;
    }>;
}

export interface CreateOrderResponse {
    success: boolean;
    orderId: string;
    error: string | null;
}

export interface CreateOrderRequest {
    CustomerId: string;
    SaleStaff: string;
    OrderItems: Array<{
        ProductId: string;
        Quantity: number;
        UnitPrice: number;
    }>;
    PaymentMethod: string;
}


export type OrderItemRequest = {
    ProductId: string; // Guid in C# maps to string in TS
    Quantity: number;
    UnitPrice: number;
}


export type UpdateOrderRequest = {
    Status: number;
    SaleStaff: string;
    VipApplied: boolean;
}

export type UpdateOrderRequestKey = {
    Status: string;
    SaleStaff: string;
    VipApplied: boolean;
}

export const showAllOrder = () => {
    return get(`/api/Orders`);
}

export const fetchAllOrderByUserId = (id: string) => {
    return get(`/api/Orders/user/${id}`);
}

export const showOrdersPage = () => {
    return get(`/api/orders`);
}

export const showReveneSummary = () => {
    return post(`/api/orders/summarize`);
}

export const orderDetail = (id: string) => { // ID is Guid, so string
    return get(`/api/Orders/${id}`);
}

export const orderRelation = (id: string) => { // ID is Guid, so string
    return get(`/api/orders/${id}/user-info`);
}

export const createOrder = (order: CreateOrderRequest) => {
    return post(`/api/Orders/create`, order); // Fixed endpoint to match your curl example
}

export const updateOrder = (id: string) => { // ID is Guid, so string
    return put(`/api/orders/${id}`);
}

export const deleteOrder = (id: string) => { // ID is Guid, so string
    return remove(`/api/orders/${id}`);
}

export const showDailyRevenueSummary = () => {
    return get(`/api/orders/daily-summarize`);
}

export const showWeeklyRevenueSummary = () => {
    return get(`/api/orders/weekly-summarize`);
}