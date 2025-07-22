import { get, post } from "./apiCaller";

export const createVnPayPayment = (orderId: string, amount: number) => {
    return post(`/VnPay/create-payment?orderId=${orderId}&amount=${amount}`);
}

export const getVnPayPaymentCallback = (queryString: string) => {
    return get(`/VnPay/payment-callback?${queryString}`);
}