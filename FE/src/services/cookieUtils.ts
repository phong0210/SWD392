import config from "@/config";
import { jwtDecode } from "jwt-decode";
import Cookies from "universal-cookie";
import { clearCart } from "./cartAPI";

const cookies = new Cookies(null, { path: "/" });

class CookieUtils {
  getItem(key: string, defaultValue = "") {
    const item = cookies.get(key);
    return item !== undefined ? item : defaultValue;
  }

  setItem(key: string, value = "") {
    cookies.set(key, value, { path: "/" });
  }

  removeItem(key: string) {
    cookies.remove(key);
  }

  deleteUser() {
    cookies.remove(config.cookies.token);
    clearCart();
  }

  decodeJwt() {
    const token = this.getItem(config.cookies.token);
    if (token) {
      try {
        const jwtUser = jwtDecode(token);
        return jwtUser;
      } catch (err) {
        this.deleteUser();
      }
    }
    return undefined;
  }

  getToken() {
    return this.getItem(config.cookies.token);
  }

  setToken(value = "") {
    this.setItem(config.cookies.token, value);
  }

  clear() {
    cookies.remove(config.cookies.token, { path: "/" });
  }

 clearCartCookie = () => {
  try {
    const cart = cookies.get("cart");
    if (cart) {
      cookies.remove("cart", { path: "/" });
      console.log("[CookieUtils] Cart cookie cleared successfully");
    } else {
      console.log("[CookieUtils] No cart cookie found to clear");
    }
  } catch (error) {
    console.error("[CookieUtils] Error clearing cart cookie:", error);
  }
}
}
export default new CookieUtils();
