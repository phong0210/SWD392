import { get, post, put, remove } from "./apiCaller";

const loyaltyAPI = {
  createLoyaltyPoint(loyaltyPointDto: any) {
    return post('/api/LoyaltyPoints/Create', loyaltyPointDto);
  },
  getLoyaltyPointById(loyaltyPointId: string) {
    return get(`/api/LoyaltyPoints/GetLoyaltyPointById/${loyaltyPointId}`);
  },
  getLoyaltyPointByUserId(userId: string) {
    return get(`/api/LoyaltyPoints/GetLoyaltyPointByUserId/${userId}`);
  },
  updateLoyaltyPointsByUserId(userId: string, updateDto: any) {
    return put(`/api/LoyaltyPoints/user/${userId}/loyalty-points`, updateDto);
  },
  getAllLoyaltyPoints() {
    return get('/api/LoyaltyPoints/ShowAll');
  },
  updateLoyaltyPoint(id: string, loyaltyPointDto: any) {
    return put(`/api/LoyaltyPoints/UpdateLoyaltyPoint/${id}`, loyaltyPointDto);
  },
  deleteLoyaltyPoint(id: string) {
    return remove(`/api/LoyaltyPoints/DeleteLoyaltyPoint/${id}`);
  },
};

export default loyaltyAPI;
