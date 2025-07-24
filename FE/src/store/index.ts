import uploadSlice from "@/pages/Admin/ProductPage/Diamond/components/slice";
import uploadSliceSetting from "@/pages/Admin/ProductPage/Jewelry Setting/components/slice";
import { uploadSliceDiaProduct } from "@/pages/Admin/ProductPage/Jewelry/components/AddDiaJewComponent/slice";
import { uploadSliceReguProduct } from "@/pages/Admin/ProductPage/Jewelry/components/AddReguComponent/slice";
import { configureStore } from "@reduxjs/toolkit";
import { authSlice } from "@/store/slices/authSlice";
import cartSlice from "@/store/slices/cartSlice";
import orderSlice from "@/store/slices/orderSlice";

export const store = configureStore({
    reducer: {
        cart: cartSlice,
        upload: uploadSlice.reducer,
        uploadSetting: uploadSliceSetting.reducer,
        uploadDiaProduct: uploadSliceDiaProduct.reducer,
        uploadReguProduct: uploadSliceReguProduct.reducer,
        order: orderSlice,
        auth: authSlice.reducer,
    },
    middleware: (getDefaultMiddleWare) => 
        getDefaultMiddleWare({
            serializableCheck: false
        }),
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;