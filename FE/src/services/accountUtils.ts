import cookieUtils from "@/services/cookieUtils";
import { Role } from "@/utils/enum";

type JwtType = {
  AccountID: string;
  Email: string;
  Role: string;
  exp: number;
  iat: number;
};


export const getAccountID = (): string | null => {
    const decoded = cookieUtils.decodeJwt() as JwtType;
  console.log('[getAccountID] Decoded JWT:', decoded);

  if (!decoded || !decoded.AccountID) return null;
  return decoded.AccountID;
};


export const getRoleFromToken = (): string | null => {
    const decoded = cookieUtils.decodeJwt() as JwtType;
    if (!decoded || !decoded.Role) return null;

    // Map backend role name to FE role constant if available
    return Role[decoded.Role] || decoded.Role;
};
