import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { createOrder, CreateOrderRequest, OrderResponseFE, updateOrder, orderDetail, CreateOrderResponse } from '@/services/orderAPI';
import { captureOrderPayPal } from '@/services/paymentAPI';
import { OrderStatus } from '@/utils/enum';

interface OrderState {
    order: OrderResponseFE | null;
    status: 'idle' | 'loading' | 'succeeded' | 'failed';
    error: string | null;
    voucherID: string | null;
}

const initialState: OrderState = {
    order: null,
    status: 'idle',
    error: null,
    voucherID: null,
};

/// Thunk to create an order and then fetch the full details
export const createOrderAsync = createAsyncThunk(
    'order/createOrder',
    async (orderData: CreateOrderRequest, { rejectWithValue }) => {
        console.log('[createOrderAsync] Starting order creation with data:', orderData);
        try {
            console.log('[createOrderAsync] Calling createOrder API...');
            const response = await createOrder(orderData);
            const responseData = response.data;
            console.log('[createOrderAsync] createOrder API response:', responseData);

            if (!responseData.success) {
                console.error('[createOrderAsync] Order creation failed:', responseData.error);
                return rejectWithValue(responseData.error || 'Order creation failed');
            }
            
            const orderId = responseData.orderId;
            console.log('[createOrderAsync] Order created successfully with orderId:', orderId);

            // Persist the ID for PayPal flow
            if (orderId) {
                localStorage.setItem("CurrentOrderID", orderId);
                console.log('[createOrderAsync] Order ID saved to localStorage for PayPal flow:', orderId);
            }

            // Fetch the full order details
            console.log('[createOrderAsync] Fetching order details for orderId:', orderId);
            const detailResponse = await orderDetail(orderId);
            console.log('[createOrderAsync] orderDetail API response:', detailResponse.data);

            if (!detailResponse.data.success) {
                console.error('[createOrderAsync] Failed to fetch order details:', detailResponse.data.error);
                return rejectWithValue(detailResponse.data.error || 'Failed to fetch order details');
            }

            console.log('[createOrderAsync] Order details fetched successfully:', detailResponse.data.order);
            return detailResponse.data.order;
        } catch (error: any) {
            console.error('[createOrderAsync] Error occurred:', {
                message: error.response?.data?.message || error.message || 'An unknown error occurred',
                error,
            });
            return rejectWithValue(error.response?.data?.message || error.message || 'An unknown error occurred');
        }
    }
);


// Thunk to handle the return from PayPal
export const captureOrderPaypalAsync = createAsyncThunk(
    'order/captureOrderPaypal',
    async (token: string, { rejectWithValue }) => {
        console.log('[captureOrderPaypalAsync] Starting PayPal capture with token:', token);
        try {
            // Step 1: Retrieve the order ID saved before the redirect
            const orderId = localStorage.getItem("CurrentOrderID");
            console.log('[captureOrderPaypalAsync] Retrieved orderId from localStorage:', orderId);
            if (!orderId) {
                console.error('[captureOrderPaypalAsync] Order ID not found in localStorage');
                return rejectWithValue("Order ID not found for PayPal capture. Please try again.");
            }

            // Step 2: Capture the payment with PayPal
            console.log('[captureOrderPaypalAsync] Calling captureOrderPayPal API...');
            const captureResponse = await captureOrderPayPal(token);
            console.log('[captureOrderPaypalAsync] captureOrderPayPal API response:', captureResponse.data);

            if (captureResponse.data.status !== "COMPLETED") {
                console.error('[captureOrderPaypalAsync] PayPal payment not completed:', captureResponse.data.status);
                return rejectWithValue("PayPal payment was not completed.");
            }
            
            // Step 3: Update the order status on our backend
            console.log('[captureOrderPaypalAsync] Updating order status for orderId:', orderId);
            await updateOrder(orderId, {
                Status: OrderStatus.PENDING,
                SaleStaff: "", 
                VipApplied: false, 
            });
            console.log('[captureOrderPaypalAsync] Order status updated successfully for orderId:', orderId);

            // Step 4: Fetch the final, updated order details
            console.log('[captureOrderPaypalAsync] Fetching updated order details for orderId:', orderId);
            const detailResponse = await orderDetail(orderId);
            console.log('[captureOrderPaypalAsync] orderDetail API response:', detailResponse.data);

            if (!detailResponse.data.success) {
                console.error('[captureOrderPaypalAsync] Failed to fetch updated order details:', detailResponse.data.error);
                return rejectWithValue(detailResponse.data.error || 'Failed to fetch updated order details');
            }

            console.log('[captureOrderPaypalAsync] Updated order details fetched successfully:', detailResponse.data.order);
            return detailResponse.data.order; // Fix: Use .order instead of .data
        } catch (error: any) {
            console.error('[captureOrderPaypalAsync] Error occurred:', {
                message: error.response?.data?.message || error.message || 'An unknown error occurred',
                error,
            });
            return rejectWithValue(error.response?.data?.message || error.message || 'An unknown error occurred');
        }
    }
);

const orderSlice = createSlice({
    name: 'order',
    initialState,
    reducers: {
        resetOrderStatus: (state) => {
            console.log('[orderSlice/resetOrderStatus] Resetting order state');
            state.status = 'idle';
            state.error = null;
            state.order = null;
            state.voucherID = null; // Also reset voucherID
            console.log('[orderSlice/resetOrderStatus] State reset:', state);
        },
        // <--- ADD THIS NEW REDUCER
        setVoucherID: (state, action: PayloadAction<string | null>) => {
            console.log(`[orderSlice/setVoucherID] Setting voucher ID to: ${action.payload}`);
            state.voucherID = action.payload;
        },
    },
    extraReducers: (builder) => {
        builder
            // Create Order Cases
            .addCase(createOrderAsync.pending, (state) => {
                console.log('[orderSlice/createOrderAsync/pending] Order creation started, setting status to loading');
                state.status = 'loading';
            })
            .addCase(createOrderAsync.fulfilled, (state, action: PayloadAction<OrderResponseFE>) => {
                console.log('[orderSlice/createOrderAsync/fulfilled] Order creation succeeded, updating state with order:', action.payload);
                state.status = 'succeeded';
                state.order = action.payload;
                state.error = null;
            })
            .addCase(createOrderAsync.rejected, (state, action) => {
                console.error('[orderSlice/createOrderAsync/rejected] Order creation failed, error:', action.payload);
                state.status = 'failed';
                state.error = action.payload as string;
            })
            // Capture PayPal Cases
            .addCase(captureOrderPaypalAsync.pending, (state) => {
                console.log('[orderSlice/captureOrderPaypalAsync/pending] PayPal capture started, setting status to loading');
                state.status = 'loading';
            })
            .addCase(captureOrderPaypalAsync.fulfilled, (state, action: PayloadAction<OrderResponseFE>) => {
                console.log('[orderSlice/captureOrderPaypalAsync/fulfilled] PayPal capture succeeded, updating state with order:', action.payload);
                state.status = 'succeeded';
                state.order = action.payload;
                state.error = null;
            })
            .addCase(captureOrderPaypalAsync.rejected, (state, action) => {
                console.error('[orderSlice/captureOrderPaypalAsync/rejected] PayPal capture failed, error:', action.payload);
                state.status = 'failed';
                state.error = action.payload as string;
            });
    },
});

// <--- EXPORT BOTH ACTION CREATORS NOW
export const { resetOrderStatus, setVoucherID } = orderSlice.actions; 
export default orderSlice.reducer;