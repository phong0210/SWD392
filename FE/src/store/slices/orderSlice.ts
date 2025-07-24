import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { createOrder, CreateOrderRequest, OrderResponseFE, updateOrder, orderDetail } from '@/services/orderAPI';
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

// Thunk to create an order and fetch details
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

      // Persist orderId for PayPal and VNPay flows
      if (orderId) {
        localStorage.setItem("CurrentOrderID", orderId);
        console.log('[createOrderAsync] Order ID saved to localStorage:', orderId);
      }

      // Fetch order details
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

// Thunk to handle PayPal capture
export const captureOrderPaypalAsync = createAsyncThunk(
  'order/captureOrderPaypal',
  async (token: string, { rejectWithValue }) => {
    console.log('[captureOrderPaypalAsync] Starting PayPal capture with token:', token);
    try {
      const orderId = localStorage.getItem("CurrentOrderID");
      console.log('[captureOrderPaypalAsync] Retrieved orderId from localStorage:', orderId);
      if (!orderId) {
        console.error('[captureOrderPaypalAsync] Order ID not found in localStorage');
        return rejectWithValue("Order ID not found for PayPal capture. Please try again.");
      }

      console.log('[captureOrderPaypalAsync] Calling captureOrderPayPal API...');
      const captureResponse = await captureOrderPayPal(token);
      console.log('[captureOrderPaypalAsync] captureOrderPayPal API response:', captureResponse.data);

      if (captureResponse.data.status !== "COMPLETED") {
        console.error('[captureOrderPaypalAsync] PayPal payment not completed:', captureResponse.data.status);
        return rejectWithValue("PayPal payment was not completed.");
      }

      console.log('[captureOrderPaypalAsync] Updating order status for orderId:', orderId);
      await updateOrder(orderId);
      console.log('[captureOrderPaypalAsync] Order status updated successfully for orderId:', orderId);

      console.log('[captureOrderPaypalAsync] Fetching updated order details for orderId:', orderId);
      const detailResponse = await orderDetail(orderId);
      console.log('[captureOrderPaypalAsync] orderDetail API response:', detailResponse.data);

      if (!detailResponse.data.success) {
        console.error('[captureOrderPaypalAsync] Failed to fetch updated order details:', detailResponse.data.error);
        return rejectWithValue(detailResponse.data.error || 'Failed to fetch updated order details');
      }

      console.log('[captureOrderPaypalAsync] Updated order details fetched successfully:', detailResponse.data.order);
      return detailResponse.data.order;
    } catch (error: any) {
      console.error('[captureOrderPaypalAsync] Error occurred:', {
        message: error.response?.data?.message || error.message || 'An unknown error occurred',
        error,
      });
      return rejectWithValue(error.response?.data?.message || error.message || 'An unknown error occurred');
    }
  }
);

// Thunk to handle VNPay success redirect
export const handleVNPayReturnAsync = createAsyncThunk(
  'order/handleVNPayReturn',
  async (queryParams: URLSearchParams, { rejectWithValue }) => {
    console.log('[handleVNPayReturnAsync] Starting VNPay return handling with queryParams:', queryParams.toString());
    try {
      const orderInfo = queryParams.get('vnp_OrderInfo');
      if (!orderInfo) {
        console.error('[handleVNPayReturnAsync] vnp_OrderInfo not found in query parameters');
        return rejectWithValue('Order information not found in VNPay response');
      }

      const orderIdMatch = orderInfo.match(/#([\w-]+)$/);
      const orderId = orderIdMatch ? orderIdMatch[1] : null;
      if (!orderId) {
        console.error('[handleVNPayReturnAsync] Could not extract order ID from vnp_OrderInfo:', orderInfo);
        return rejectWithValue('Invalid order ID in VNPay response');
      }

      console.log('[handleVNPayReturnAsync] Extracted orderId:', orderId);

      const responseCode = queryParams.get('vnp_ResponseCode');
      if (responseCode !== '00') {
        console.error('[handleVNPayReturnAsync] VNPay transaction failed with responseCode:', responseCode);
        return rejectWithValue(`VNPay transaction failed with response code: ${responseCode}`);
      }

      console.log('[handleVNPayReturnAsync] Updating order status for orderId:', orderId);
      await updateOrder(orderId);

      console.log('[handleVNPayReturnAsync] Fetching updated order details for orderId:', orderId);
      const detailResponse = await orderDetail(orderId);
      console.log('[handleVNPayReturnAsync] orderDetail API response:', detailResponse.data);

      if (!detailResponse.data.success) {
        console.error('[handleVNPayReturnAsync] Failed to fetch updated order details:', detailResponse.data.error);
        return rejectWithValue(detailResponse.data.error || 'Failed to fetch updated order details');
      }

      console.log('[handleVNPayReturnAsync] Updated order details fetched successfully:', detailResponse.data.order);
      return detailResponse.data.order;
    } catch (error: any) {
      console.error('[handleVNPayReturnAsync] Error occurred:', {
        message: error.response?.data?.message || error.message || 'An unknown error occurred',
        error,
      });
      return rejectWithValue(error.response?.data?.message || error.message || 'An unknown error occurred');
    }
  }
);

// Thunk to handle VNPay failure redirect
export const handleVNPayFailAsync = createAsyncThunk(
  'order/handleVNPayFail',
  async (queryParams: URLSearchParams, { rejectWithValue }) => {
    console.log('[handleVNPayFailAsync] Starting VNPay failure handling with queryParams:', queryParams.toString());
    try {
      const orderInfo = queryParams.get('vnp_OrderInfo');
      let orderId: string | null = null;
      if (orderInfo) {
        const orderIdMatch = orderInfo.match(/#([\w-]+)$/);
        orderId = orderIdMatch ? orderIdMatch[1] : null;
        console.log('[handleVNPayFailAsync] Extracted orderId:', orderId);
      } else {
        console.warn('[handleVNPayFailAsync] vnp_OrderInfo not found in query parameters');
      }

      const responseCode = queryParams.get('vnp_ResponseCode');
      let errorMessage = 'VNPay payment failed. Please try again or contact support.';
      if (responseCode && responseCode !== '00') {
        errorMessage = `VNPay payment failed with response code: ${responseCode}.`;
        console.log('[handleVNPayFailAsync] VNPay responseCode:', responseCode);
      } else {
        console.warn('[handleVNPayFailAsync] No vnp_ResponseCode provided');
      }

      let order: OrderResponseFE | null = null;
      if (orderId) {
        console.log('[handleVNPayFailAsync] Fetching order details for orderId:', orderId);
        const detailResponse = await orderDetail(orderId);
        console.log('[handleVNPayFailAsync] orderDetail API response:', detailResponse.data);

        if (detailResponse.data.success) {
          order = detailResponse.data.order;
          console.log('[handleVNPayFailAsync] Order details fetched successfully:', order);
        } else {
          console.warn('[handleVNPayFailAsync] Failed to fetch order details:', detailResponse.data.error);
        }
      }

      return rejectWithValue({ order, error: errorMessage });
    } catch (error: any) {
      console.error('[handleVNPayFailAsync] Error occurred:', {
        message: error.response?.data?.message || error.message || 'An unknown error occurred',
        error,
      });
      return rejectWithValue({
        order: null,
        error: error.response?.data?.message || error.message || 'An unknown error occurred',
      });
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
      state.voucherID = null;
      console.log('[orderSlice/resetOrderStatus] State reset:', state);
    },
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
      })
      // Handle VNPay Success Cases
      .addCase(handleVNPayReturnAsync.pending, (state) => {
        console.log('[orderSlice/handleVNPayReturnAsync/pending] VNPay return handling started, setting status to loading');
        state.status = 'loading';
      })
      .addCase(handleVNPayReturnAsync.fulfilled, (state, action: PayloadAction<OrderResponseFE>) => {
        console.log('[orderSlice/handleVNPayReturnAsync/fulfilled] VNPay return succeeded, updating state with order:', action.payload);
        state.status = 'succeeded';
        state.order = action.payload;
        state.error = null;
      })
      .addCase(handleVNPayReturnAsync.rejected, (state, action) => {
        console.error('[orderSlice/handleVNPayReturnAsync/rejected] VNPay return failed, error:', action.payload);
        state.status = 'failed';
        state.error = action.payload as string;
      })
      // Handle VNPay Fail Cases
      .addCase(handleVNPayFailAsync.pending, (state) => {
        console.log('[orderSlice/handleVNPayFailAsync/pending] VNPay failure handling started, setting status to loading');
        state.status = 'loading';
      })
      .addCase(handleVNPayFailAsync.fulfilled, (state) => {
        console.log('[orderSlice/handleVNPayFailAsync/fulfilled] VNPay failure handling completed');
        // No action needed for fulfilled case since we use rejectWithValue
      })
      .addCase(handleVNPayFailAsync.rejected, (state, action) => {
        console.error('[orderSlice/handleVNPayFailAsync/rejected] VNPay failure handling failed, error:', action.payload);
        state.status = 'failed';
        state.order = (action.payload as any)?.order || null;
        state.error = (action.payload as any)?.error || 'An unknown error occurred';
      });
  },
});

export const { resetOrderStatus, setVoucherID } = orderSlice.actions;
export default orderSlice.reducer;