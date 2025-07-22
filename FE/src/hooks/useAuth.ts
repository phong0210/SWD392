import { getAccountDetail, getCustomer } from "@/services/accountApi";
import cookieUtils from "@/services/cookieUtils";
import { Role } from "@/utils/enum";
import { useCallback, useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { RootState } from "@/store";

type JwtType = {
    AccountID: number;
    Email: string;
    Role: string;
    exp: number;
    iat: number;
}

export type UserType = {
    CustomerID: number;
    AccountID: number;
    Name: string;
    PhoneNumber: string | null;
    Email: string;
    Password: string;
    Birthday: string | null;
    Gender: boolean | null;
    Address: string | null;
}

export type AccountType = {
    AccountID: number;
    Name: string;
    PhoneNumber: string | null;
    Email: string;
    Password: string;
    Role: string;
    CustomerID: null;
}

const getRoleFromToken = () => {
    const decoded = cookieUtils.decodeJwt() as JwtType;
    if (!decoded || !decoded.Role) return null;

    // Map backend role name to FE role constant if available
    return Role[decoded.Role] || decoded.Role;
}

const getAccountID = () => {
    const decoded = cookieUtils.decodeJwt() as JwtType;
    if (!decoded || !decoded.AccountID) return null;

    return decoded.AccountID;
}

const useAuth = () => {
    const { user: authUser, loading: authLoading } = useSelector((state: RootState) => state.auth);
    const [role, setRole] = useState<string | null>(getRoleFromToken());
    const [AccountID, setAccountID] = useState<number | null>(getAccountID());
    const [loading, setLoading] = useState(authLoading);
    const [user, setUser] = useState<UserType | null>(null);
    const [account, setAccount] = useState<AccountType | null>(null);

    const token = cookieUtils.getToken();

    const checkTokenExpiration = useCallback(() => {
        if (token) {
            const decoded = cookieUtils.decodeJwt() as JwtType;

            if (!decoded || decoded.exp < Date.now() / 1000) {
                setRole(null);
                cookieUtils.deleteUser();
                return;
            }
        }
    }, [token]);

    useEffect(() => {
        setLoading(authLoading);
    }, [authLoading]);

    useEffect(() => {
        const token = cookieUtils.getToken();

        if (!token || !authUser) {
            setRole(null);
            return;
        }

        const fetchData = async () => {
            setLoading(true);
            try {
                const currentRole = authUser.role;
                const currentAccountID = authUser.accountId;
                setRole(currentRole);
                setAccountID(currentAccountID);

                if (currentAccountID) {
                    if (currentRole === 'Customer') {
                        const { data } = await getCustomer(currentAccountID);
                        setUser(data.data);
                    } else {
                        const { data } = await getAccountDetail(currentAccountID);
                        setAccount(data.data);
                    }
                }
            } catch (error) {
                console.error("Failed to fetch user info:", error);
                // Optionally handle the error state here
            } finally {
                setLoading(false);
            }
        };

        fetchData();

        const intervalId = setInterval(checkTokenExpiration, 5000);

        return () => clearInterval(intervalId);
    }, [token, authUser, checkTokenExpiration]);

    return { loading, role, AccountID, user, account };
};

export default useAuth;