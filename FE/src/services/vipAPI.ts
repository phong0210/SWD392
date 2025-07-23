import { post } from "./apiCaller";

const vipAPI = {
  registerVip(userId: string, tier: string) {
    return post(`/api/users/${userId}/register-vip`, { tier });
  },
};

export default vipAPI;
