import { get, post, put, remove } from "./apiCaller"

export const login = (account: object) => {
    return post(`/api/auth/login`, account);
}

export const register = (account: object) => {
    return post(`/api/User/register`, account);
}

export const confirmRegistration = (payload: { email: string; otp: string }) => {
    return post(`/api/User/confirm-registration`, payload);
}

export const requestPasswordReset = (payload: { email: string }) => {
    return post(`/api/Auth/request-password-reset`, payload);
}

export const confirmPasswordReset = (payload: { email: string; otp: string; newPassword: string }) => {
    return post(`/api/Auth/confirm-password-reset`, payload);
}

export const registerCustomer = (account: object) => {
    return post(`/auth/signupCustomer`, account);
}

export const showAllAccounts = () => {
    return get(`/api/User`);
}



export const updateAccount = (email: string, account: object) => {
    return put(`/api/users/${email}`, account);
}

export const deleteAccount = (id: number) => {
    return remove(`/api/users/${id}`);
}

// Deprecated legacy endpoints (commented out)
// export const updateAccount = (name: string, account: object) => {
//     return put(`/auth/update/${name}`, account);
// }
// export const deleteAccount = (id: number) => {
//     return remove(`/auth/delete/${id}`);
// }