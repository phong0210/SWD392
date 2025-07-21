import { get, post, put, remove } from "./apiCaller";

export const showAllDiscount = () => {
    return get(`/api/Promotion/ShowAll`);
}

export const createDiscount = (discount: object) => {
    return post(`/api/Promotion/Create`, discount);
}

export const updateDiscount = (id: number, discount: object) => {
    return put(`/api/Promotion/UpdatePromotion${id}`, discount);
}

export const deleteDiscount = (id: number) => {
    return remove(`/api/Promotion/DeletePromotion${id}`);
}

