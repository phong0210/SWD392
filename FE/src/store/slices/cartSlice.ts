import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { CartItem, getCartItems, setCartItems, addToCart as add, removeFromCart as remove, updateCartItemQuantity as update, clearCart as clear } from '@/services/cartAPI';

interface CartState {
    items: CartItem[];
}

const initialState: CartState = {
    items: getCartItems(),
};

const cartSlice = createSlice({
    name: 'cart',
    initialState,
    reducers: {
        addToCart: (state, action: PayloadAction<CartItem>) => {
            add(action.payload);
            state.items = getCartItems();
        },
        removeFromCart: (state, action: PayloadAction<string>) => {
            remove(action.payload);
            state.items = getCartItems();
        },
        updateCartItemQuantity: (state, action: PayloadAction<{ id: string; quantity: number }>) => {
            update(action.payload.id, action.payload.quantity);
            state.items = getCartItems();
        },
        clearCart: (state) => {
            clear();
            state.items = [];
        },
    },
});

export const { addToCart, removeFromCart, updateCartItemQuantity, clearCart } = cartSlice.actions;
export default cartSlice.reducer;
