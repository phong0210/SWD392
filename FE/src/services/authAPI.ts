import { get, post, put, remove } from "./apiCaller"

export const login = (account: object) => {
    return post(`/api/auth/login`, account);
}

export const register = (account: object) => {
    return post(`/api/user/register`, account);
}

export const registerCustomer = (account: object) => {
    return post(`/auth/signupCustomer`, account);
}

export const showAllAccounts = () => {
    return get(`/api/users`);
}

export const getAccountDetail = (id: number) => {
    return get(`/api/user/detail/${id}`);
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