import { PayloadAction, createSlice } from "@reduxjs/toolkit";

interface OrderState {
    OrderID: number;
    PromotionID: string;
    Shippingfee: number;
    Total: number;
}

const initialOrderState: OrderState = {
    OrderID: 0,
    PromotionID: "",
    Shippingfee: 0,
    Total: 0,
};

export const orderSlice = createSlice({
    name: 'order',
    initialState: initialOrderState,
    reducers: {
        setOrderID: (state, action: PayloadAction<number>) => {
            state.OrderID = action.payload;
        },
        setVoucherID: (state, action: PayloadAction<string>) => {
            state.PromotionID = action.payload;
        },
        setShippingfee: (state, action: PayloadAction<number>) => {
            state.Shippingfee = action.payload;
        },
        setTotal: (state, action: PayloadAction<number>) => {
            state.Total = action.payload;
        }
    }
});

export const { setOrderID, setVoucherID, setShippingfee, setTotal } = orderSlice.actions;

export default orderSlice.reducer;