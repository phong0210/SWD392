import { get, post } from "./apiCaller";

export const getCustomer = (id: number) => {
    return get(`/api/User/detail/${id}`);
}

export const promoteToStaff = (email: string, roleName: string, salary: number, hireDate: string) => {
    return post('/api/User/promote-to-staff', { email, roleName, salary, hireDate });
}

// Deprecated legacy endpoints (commented out)
// export const showAllAccounts = () => {
//     return get('/auth/ShowAllAccounts');
// }
// export const detailsAccounts = (id: number) => {
//     return post(`/auth/detailAccount/${id}`);
// }