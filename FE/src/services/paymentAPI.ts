import { post } from "./apiCaller";

export type PaymentInformation = {
  orderId: string;
  amount: number;
  orderDescription: string;
  name: string;
  returnUrlSuccess: string;
  returnUrlFail: string;
};
export const createVnPayPayment = (paymentInformation: PaymentInformation) => {
  return post("/api/Payment/create-payment-url", paymentInformation);
};

export const createOrderPaypal = (amount: number) => {
  return post(`/paypal/create-order`, { amount });
};

export const captureOrderPayPal = (orderID: string) => {
  return post(`/paypal/capture-order/${orderID}`);
};
