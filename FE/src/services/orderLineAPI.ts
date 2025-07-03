import { get, post, put, remove } from "./apiCaller";

export interface OrderLineBody {
    Quantity: number;
    OrderID?: number | null;
    DiamondID?: number | null;
    ProductID?: number | null;
    CustomerID?: number | null;
    Inscription?: string | null;
    InscriptionFont?: string | null;
    JewelrySettingVariantID?: number | null;
    SizeID?: number | null;
}

export const showAllOrderLineForAdmin = () => {
    return get(`/api/order-lines`);
}

export const showAllOrderLineForCustomer = (customerId: number) => {
    return get(`/api/order-lines/customer/${customerId}`);
}

export const OrderLineDetail = (id: number) => {
    return get(`/api/order-lines/${id}`);
}

export const createOrderLine = (orderLine: OrderLineBody) => {
    return post(`/api/order-lines`, orderLine);
}

export const updateOrderLine = (id: number, orderLine: object) => {
    return put(`/api/order-lines/${id}`, orderLine);
}

export const deleteOrderLine = (id: number) => {
    return remove(`/api/order-lines/${id}`);
}