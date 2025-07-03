/* eslint-disable @typescript-eslint/no-explicit-any */
import { get, post, put, remove } from "./apiCaller";

export const showAllDiamond = () => {
    return get(`/api/diamonds`);
}
    
export const showDiamonds = (params: any) => {
    return get(`/api/diamonds`,params);
}

export const getDiamondDetails = (diamondID: number) => {
    return get(`/api/diamonds/${diamondID}`)
}

export const createDiamond = (diamond: object) => {
    return post(`/api/diamonds`, diamond);
}

export const updateDiamond = (id: number, diamond: object) => {
    return put(`/api/diamonds/${id}`, diamond);
}

export const deleteDiamond = (id: number) => {
    return remove(`/api/diamonds/${id}`);
}

// TEMPORARY: Commented out until backend image endpoint is available
// export const getImageDiamond = (id: number) => {
//     return get(`/usingImage/${id}`)
// }
