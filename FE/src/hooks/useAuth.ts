import { getCustomer } from "@/services/accountApi";
import cookieUtils from "@/services/cookieUtils";
import { getAccountID, getRoleFromToken } from "@/services/accountUtils";
import { useCallback, useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { RootState } from "@/store";

export type UserType = {
  CustomerID: string;
  AccountID: number;
  Name: string;
  PhoneNumber: string | null;
  Email: string;
  Password: string;
  Birthday: string | null;
  Gender: boolean | null;
  Address: string | null;
};

export type AccountType = {
  AccountID: number | string;
  Name: string;
  PhoneNumber: string | null;
  Email: string;
  Password: string;
  Role: string;
  CustomerID: null;
};

const useAuth = () => {
  const { user: authUser, loading: authLoading } = useSelector(
    (state: RootState) => state.auth
  );
  const [role, setRole] = useState<string | null>(getRoleFromToken());
  const [AccountID, setAccountID] = useState<string | null>(getAccountID());
  const [loading, setLoading] = useState(authLoading);
  const [user, setUser] = useState<UserType | null>(null);
  const [account, setAccount] = useState<AccountType | null>(null);

  const checkTokenExpiration = useCallback(() => {
    const token = cookieUtils.getToken();
    if (token) {
      const decoded = cookieUtils.decodeJwt() as any;

      if (!decoded || decoded.exp < Date.now() / 1000) {
        setRole(null);
        cookieUtils.deleteUser();
        return;
      }
    }
  }, []);

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
          if (currentRole === "Customer") {
            const { data } = await getCustomer(currentAccountID);
            setUser(data.data);
          } else {
            const { data } = await getCustomer(currentAccountID);
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
  }, [authUser, checkTokenExpiration]);

  return { loading, role, AccountID, user, account };
};

export default useAuth;
