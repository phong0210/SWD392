import { get, post, put, remove } from "./apiCaller"

export const showAllProduct = () => {
    return get(`/api/products`);
}

export const getProductDetails = (id: number) => {
    return get(`/api/products/${id}`)
}

export const createProduct = (product: object) => {
    return post(`/api/products`, product);
}

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

export const updateDiamond = (id: number, diamond: object) => {
    return put(`/api/diamonds/${id}`, diamond);
}

export const deleteDiamond = (id: number) => {
    return remove(`/api/diamonds/${id}`);
}

