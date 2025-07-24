import Cookies from 'js-cookie';

const CART_COOKIE_KEY = 'cart';

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
    const cartCookie = Cookies.get(CART_COOKIE_KEY);
    return cartCookie ? JSON.parse(cartCookie) : [];
};

export const setCartItems = (cartItems: CartItem[]) => {
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
    const cartItems = getCartItems().filter((item) => item.id !== itemId);
    setCartItems(cartItems);
};

export const updateCartItemQuantity = (itemId: string, quantity: number) => {
    const cartItems = getCartItems().map((item) =>
        item.id === itemId ? { ...item, quantity } : item
    );
    setCartItems(cartItems);
};

export const clearCart = () => {
    Cookies.remove(CART_COOKIE_KEY);
};
