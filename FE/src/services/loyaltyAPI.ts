import { get } from "./apiCaller";

const loyaltyAPI = {
  getLoyaltyPoints(userId: string) {
    return get(`/api/users/${userId}/loyalty-points`);
  },
};

export default loyaltyAPI;
