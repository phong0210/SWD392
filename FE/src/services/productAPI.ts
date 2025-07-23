import { UUID } from "crypto";
import { get, post, put, remove } from "./apiCaller";
import axios from "axios";

export const showAllProduct = async () => {
  try {
    const response = await axios.get("/api/Product");
    return response;
  } catch (error) {
    console.error("Error calling API:", error);
    throw error;
  }
};
export const createProduct = (product: object) => {
  return post(`/api/Product`, product);
};

export const getProductDetails = (id: string) => {
  return get(`/api/Product/detail/${id}`);
};

export const updateDiamond = (id: number, diamond: object) => {
  return put(`/api/Product/update/${id}`, diamond);
};

export const deleteDiamond = (id: number) => {
  return remove(`/api/Product/${id}`);
};

// TEMPORARY: Commented out until backend image endpoint is available
// export const getImageProduct = (id: number) => {
//     return get(`/usingImage/${id}`)
// }

// TODO: Implement these when backend is ready
// export const updateProduct = (id: number, product: object) => {
//     return put(`/api/products/${id}`, product);
// }
// export const hideProduct = (id: number) => {
//     return put(`/api/products/${id}/hide`);
// }
// export const updateInventory = (id: number, inventory: object) => {
//     return put(`/api/products/${id}/inventory`, inventory);
// }
// export const getCategories = () => {
//     return get(`/api/categories`);
// }
