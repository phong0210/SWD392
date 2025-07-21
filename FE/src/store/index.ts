import { cartSlice } from "@/layouts/MainLayout/slice/cartSlice";
import { customSlice } from "@/layouts/MainLayout/slice/customRingSlice";
import { orderSlice } from "@/layouts/MainLayout/slice/orderSlice";
import uploadSlice from "@/pages/Admin/ProductPage/Diamond/components/slice";

import { configureStore } from "@reduxjs/toolkit";
import { authSlice } from "@/store/slices/authSlice";

export const store = configureStore({
  reducer: {
    cart: cartSlice.reducer,
    customRing: customSlice.reducer,
    upload: uploadSlice.reducer,

    order: orderSlice.reducer,
    auth: authSlice.reducer,
  },
  middleware: (getDefaultMiddleWare) =>
    getDefaultMiddleWare({
      serializableCheck: false,
    }),
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;
