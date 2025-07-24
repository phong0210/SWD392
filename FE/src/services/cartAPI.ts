import Cookies from 'js-cookie';
import { getAccountID } from './accountUtils';
import { useSelector } from 'react-redux';
import { RootState } from '@/store';

export interface CartItem {
    id: string;
    productId?: string;
    diamondId?: string;
    name: string;
    price: number;
    quantity: number;
    image: string;
}

// Get cookie key based on account ID or fallback to anonymous
const getCartCookieKey = (accountId?: string | null): string => {
    console.log(`[getCartCookieKey] Account ID: ${accountId}`);
    return accountId ? `cart_${accountId}` : 'cart_anonymous';
};

// Hook: used in React components
export const useCartItems = (): CartItem[] => {
    const accountId = useSelector((state: RootState) => state.auth.user?.id);
    console.log(`[useCartItems] Using account ID: ${accountId}`);
    const CART_COOKIE_KEY = getCartCookieKey(accountId);
    const cartCookie = Cookies.get(CART_COOKIE_KEY);
    const items = cartCookie ? JSON.parse(cartCookie) : [];
    console.log(`[useCartItems] Cart for account: ${accountId || 'anonymous'}. Items:`, items);
    return items;
};

// Get cart items outside of React (pass optional accountId)
export const getCartItems = (accountId?: string | null): CartItem[] => {
    const effectiveAccountId = accountId ?? getAccountID();
    const CART_COOKIE_KEY = getCartCookieKey(effectiveAccountId);
    const cartCookie = Cookies.get(CART_COOKIE_KEY);
    const items = cartCookie ? JSON.parse(cartCookie) : [];
    console.log(`[getCartItems] Cart for account: ${effectiveAccountId || 'anonymous'}. Items:`, items);
    return items;
};

// Set cart items (outside of React)
export const setCartItems = (cartItems: CartItem[], accountId?: string | null): void => {
    const effectiveAccountId = accountId ?? getAccountID();
    const CART_COOKIE_KEY = getCartCookieKey(effectiveAccountId);
    Cookies.set(CART_COOKIE_KEY, JSON.stringify(cartItems), { expires: 7 });
};

// Add item to cart
export const addToCart = (item: CartItem, accountId?: string | null): void => {
    const effectiveAccountId = accountId ?? getAccountID();
    const cartItems = getCartItems(effectiveAccountId);
    console.log(`[addToCart] Adding to cart for account: ${effectiveAccountId || 'anonymous'}. Item:`, item);

    const existingItem = cartItems.find((cartItem) => cartItem.id === item.id);
    if (existingItem) {
        existingItem.quantity += item.quantity;
    } else {
        cartItems.push(item);
    }
    setCartItems(cartItems, effectiveAccountId);
};

// Remove item
export const removeFromCart = (itemId: string, accountId?: string | null): void => {
    const cartItems = getCartItems(accountId).filter((item) => item.id !== itemId);
    setCartItems(cartItems, accountId);
};

// Update quantity
export const updateCartItemQuantity = (itemId: string, quantity: number, accountId?: string | null): void => {
    const cartItems = getCartItems(accountId).map((item) =>
        item.id === itemId ? { ...item, quantity } : item
    );
    setCartItems(cartItems, accountId);
};

// Clear cart
export const clearCart = (accountId?: string | null): void => {
    const effectiveAccountId = accountId ?? getAccountID();
    const CART_COOKIE_KEY = getCartCookieKey(effectiveAccountId);
    Cookies.remove(CART_COOKIE_KEY);
};
