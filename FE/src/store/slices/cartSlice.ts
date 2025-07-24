import { createSlice, PayloadAction, createAsyncThunk } from '@reduxjs/toolkit';
import {
  CartItem,
  getCartItems,
  setCartItems,
  addToCart as add,
  removeFromCart as remove,
  updateCartItemQuantity as update,
  clearCart as clear,
} from '@/services/cartAPI';

export const loadCart = createAsyncThunk<CartItem[], string | null>(
  'cart/loadCart',
  async (accountId) => {
    return getCartItems(accountId); // Explicitly load correct cart
  }
);

export const clearCart = createAsyncThunk<void, string | null>(
  'cart/clearCart',
  async (accountId) => {
    clear(accountId);
  }
);

interface CartState {
  items: CartItem[];
}

const initialState: CartState = {
  items: [],
};

const cartSlice = createSlice({
  name: 'cart',
  initialState,
  reducers: {
    addToCart: (state, action: PayloadAction<CartItem>) => {
      add(action.payload);
      state.items = getCartItems(); // you can also pass accountId if needed
    },
    removeFromCart: (state, action: PayloadAction<string>) => {
      remove(action.payload);
      state.items = getCartItems();
    },
    updateCartItemQuantity: (
      state,
      action: PayloadAction<{ id: string; quantity: number }>
    ) => {
      update(action.payload.id, action.payload.quantity);
      state.items = getCartItems();
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(loadCart.fulfilled, (state, action) => {
        state.items = action.payload;
      })
      .addCase(clearCart.fulfilled, (state) => {
        state.items = [];
      });
  },
});

export const {
  addToCart,
  removeFromCart,
  updateCartItemQuantity,
} = cartSlice.actions;
export default cartSlice.reducer;
