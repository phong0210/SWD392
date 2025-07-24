import { get, post, remove } from "./apiCaller";

export const getCustomer = (id: string) => {
  return get(`/api/User/detail/${id}`);
};

export const promoteToStaff = (
  email: string,
  roleName: string,
  salary: number,
  hireDate: string
) => {
  return post("/api/User/promote-to-staff", {
    email,
    roleName,
    salary,
    hireDate,
  });
};

export const deleteUser = (id: number) => {
  return remove(`/api/User/${id}`);
};

// Deprecated legacy endpoints (commented out)
// export const showAllAccounts = () => {
//     return get('/auth/ShowAllAccounts');
// }
// export const detailsAccounts = (id: number) => {
//     return post(`/auth/detailAccount/${id}`);
// }
