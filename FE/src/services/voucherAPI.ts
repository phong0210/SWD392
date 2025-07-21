import { get, post, put, remove } from "./apiCaller";

export const showAllVoucher = () => {
  return get(`/api/Promotion/ShowAll`);
};

export const createVoucher = (voucher: object) => {
  return post(`/api/Promotion/Create`, voucher);
};

export const updateVoucher = (id: number, voucher: object) => {
  return put(`/api/Promotion/UpdatePromotion/${id}`, voucher);
};

export const deleteVoucher = (id: number) => {
  return remove(`/api/Promotion/DeletePromotion/${id}`);
};
