import Cookies from 'js-cookie';
import { getAccountID } from './accountUtils';

const getCartCookieKey = () => {
    const accountId = getAccountID();
    return accountId ? `cart_${accountId}` : 'cart_anonymous';
};

export interface CartItem {
    id: string; // Unique identifier for the cart item, e.g., 'product-123'
    productId?: string;
    diamondId?: string;
    name: string;
    price: number;
    quantity: number;
    image: string;
}

export const getCartItems = (): CartItem[] => {
    const CART_COOKIE_KEY = getCartCookieKey();
    const cartCookie = Cookies.get(CART_COOKIE_KEY);
    return cartCookie ? JSON.parse(cartCookie) : [];
};

export const setCartItems = (cartItems: CartItem[]) => {
    const CART_COOKIE_KEY = getCartCookieKey();
    Cookies.set(CART_COOKIE_KEY, JSON.stringify(cartItems), { expires: 7 });
};

export const addToCart = (item: CartItem) => {
    const cartItems = getCartItems();
    const existingItem = cartItems.find((cartItem) => cartItem.id === item.id);

    if (existingItem) {
        existingItem.quantity += item.quantity;
    } else {
        cartItems.push(item);
    }

    setCartItems(cartItems);
};

export const removeFromCart = (itemId: string) => {
    let cartItems = getCartItems();
    cartItems = cartItems.filter((item) => item.id !== itemId);
    setCartItems(cartItems);
};

export const updateCartItemQuantity = (itemId: string, quantity: number) => {
    const cartItems = getCartItems().map((item) =>
        item.id === itemId ? { ...item, quantity } : item
    );
    setCartItems(cartItems);
};

export const clearCart = () => {
    const CART_COOKIE_KEY = getCartCookieKey();
    console.log('[clearCart] Attempting to clear cart cookie:', CART_COOKIE_KEY);
    try {
        if (!CART_COOKIE_KEY) {
            console.error('[clearCart] CART_COOKIE_KEY is undefined');
            throw new Error('Cart cookie key is not defined');
        }
        Cookies.remove(CART_COOKIE_KEY);
        console.log('[clearCart] Cart cookie removed successfully');
    } catch (error) {
        console.error('[clearCart] Error clearing cart cookie:', error);
        throw error;
    }
};
