import { get } from "./apiCaller";

// interface UpdateAccount {
//     fullName: string;
//     phoneNumber: string;
//     role: string;
//     email: string;
//     address: string;
// }

export const getCustomer = (id: number) => {
    return get(`/api/users/${id}`);
}

export const getAccountDetail = (id: number) => {
    return get(`/api/users/${id}`);
}

// Deprecated legacy endpoints (commented out)
// export const showAllAccounts = () => {
//     return get('/auth/ShowAllAccounts');
// }
// export const detailsAccounts = (id: number) => {
//     return post(`/auth/detailAccount/${id}`);
// }