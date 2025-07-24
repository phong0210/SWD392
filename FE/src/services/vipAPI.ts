import { get, post, put, remove } from "./apiCaller";

const vipAPI = {
  createVip(requestBody: any) {
    return post('/api/Vip/Create', requestBody);
  },
  getVipById(vipId: string) {
    return get(`/api/Vip/GetVipById/${vipId}`);
  },
  getVipByUserId(userId: string) {
    return get(`/api/Vip/GetVipByUserId/${userId}`);
  },
  getAllVips() {
    return get('/api/Vip/ShowAll');
  },
  updateVip(id: string, requestBody: any) {
    return put(`/api/Vip/UpdateVip/${id}`, requestBody);
  },
  deleteVip(id: string) {
    return remove(`/api/Vip/DeleteVip/${id}`);
  },
};

export default vipAPI;
