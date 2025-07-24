import { get, post, put, remove } from "./apiCaller";

export const showAllWarranty = () => {
    return get(`/api/Warranty`);
}

export const createWarranty = (warranty: object) => {
    return post(`/api/Warranty`, warranty);
}


export const updateWarranty = (id: number, warranty: object) => {
    return put(`/api/Warranty/${id}`, warranty);
}

export const deleteWarranty = (id: number) => {
    return remove(`/api/Warranty/${id}`);
}

export const getWarrantyDetails = (id: number) => {
    return get(`/api/Warranty/${id}`)
}

