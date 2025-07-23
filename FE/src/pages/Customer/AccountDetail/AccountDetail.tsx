/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import styled from "styled-components";
import { ChangeEvent, FormEvent, useState, useRef, useEffect } from "react";
import AccountCus from "@/components/Customer/Account Details/AccountCus";
import { useSelector } from "react-redux";
import { RootState } from "@/store";
import { requestPasswordReset, confirmPasswordReset } from "@/services/authAPI";
import { message } from "antd";
import { getCustomer } from "@/services/accountApi";

interface Account {
  name: string;
  phone: string;
  email: string;
  password: string;
  address: string;
}

const fetchCustomerInfo = async (AccountID: number) => {
  try {
    const { data } = await getCustomer(AccountID);
    console.log("data for getCustomer:", data);
    // Handle the nested response structure
    if (data && data.user) {
      return data.user;
    }
    return data;
  } catch (error) {
    console.log("Error fetching customer info:", error);
    return null;
  }
};

const updateAccountDetails = async (userId: number, accountData: Account) => {
  try {
    const response = await fetch(
      `${import.meta.env.VITE_API_URL}/api/user/${userId}/update`,
      {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
        body: JSON.stringify({
          name: accountData.name,
          email: accountData.email,
          phone: accountData.phone,
          address: accountData.address,
        }),
      }
    );

    if (!response.ok) {
      throw new Error("Failed to update account details");
    }

    return await response.json();
  } catch (error) {
    console.error("Error updating account details:", error);
    throw new Error("Failed to update account details");
  }
};

const Account = () => {
  const [isEditing, setIsEditing] = useState(false);
  const [passwordChangeStep, setPasswordChangeStep] = useState(0); // 0: idle, 1: request OTP, 2: verify OTP and set new password
  const [isLoading, setIsLoading] = useState(false);
  const [messageApi, contextHolder] = message.useMessage(); // Use Ant Design message

  const [account, setAccount] = useState<Account>({
    name: "",
    phone: "",
    email: "",
    password: "",
    address: "",
  });

  const [tempAccount, setTempAccount] = useState<Account>({ ...account });
  const [passwordResetData, setPasswordResetData] = useState({
    email: "",
    otp: "",
    newPassword: "",
    confirmNewPassword: "",
  });

  const [accountErrors, setAccountErrors] = useState<Partial<Account>>({});
  const [passwordResetErrors, setPasswordResetErrors] = useState<{
    email?: string;
    otp?: string;
    newPassword?: string;
    confirmNewPassword?: string;
  }>({});

  const { user } = useSelector((state: RootState) => state.auth);

  const validateAccount = (account: Account) => {
    const errors: Partial<Account> = {};
    if (!account.name) errors.name = "Name is required";
    if (!account.email) errors.email = "Email is required";
    return errors;
  };

  const validatePasswordReset = (data: typeof passwordResetData) => {
    const errors: typeof passwordResetErrors = {};
    if (passwordChangeStep === 1) {
      if (!data.email) errors.email = "Email is required";
      else if (!validateEmail(data.email))
        errors.email = "Invalid email format";
    } else if (passwordChangeStep === 2) {
      if (!data.otp) errors.otp = "OTP is required";
      if (!data.newPassword) errors.newPassword = "New password is required";
      if (
        data.newPassword.length < 8 ||
        data.newPassword.length > 16 ||
        !/^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,16}$/.test(data.newPassword)
      ) {
        errors.newPassword =
          "Must be between 8 and 16 characters, including a number, one uppercase letter and one lowercase letter.";
      }
      if (data.newPassword !== data.confirmNewPassword) {
        errors.confirmNewPassword = "Passwords do not match";
      }
    }
    return errors;
  };

  const validateEmail = (email: string) => {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
  };

  const handleEditStart = () => {
    if (customerInfo) {
      setTempAccount({
        name: customerInfo.fullName || "",
        phone: customerInfo.phone || "",
        email: customerInfo.email || "",
        password: "",
        address: customerInfo.address || "",
      });
    }
    setIsEditing(true);
    messageApi.destroy(); // Clear messages
  };

  const handlePasswordEditStart = () => {
    setPasswordChangeStep(1); // Start password reset flow
    setPasswordResetData({
      email: customerInfo?.email || "", // Pre-fill email if available
      otp: "",
      newPassword: "",
      confirmNewPassword: "",
    });
    messageApi.destroy(); // Clear messages
  };

  const handleCancel = () => {
    setIsEditing(false);
    setPasswordChangeStep(0); // Reset password change flow
    resetFormValues();
    messageApi.destroy(); // Clear messages
  };

  const resetFormValues = () => {
    setAccountErrors({});
    setPasswordResetErrors({});
  };

  const handleAccountChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setTempAccount((prevAccount) => ({
      ...prevAccount,
      [name]: value,
    }));

    if (name === "email" && validateEmail(value)) {
      setAccountErrors((prevErrors) => ({
        ...prevErrors,
        email: undefined,
      }));
    }
  };

  const handlePasswordResetChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setPasswordResetData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  const handleAccountSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setIsLoading(true);
    messageApi.destroy();

    const errors = validateAccount(tempAccount);
    const emailIsValid = validateEmail(tempAccount.email);

    if (!emailIsValid) {
      errors.email = "Invalid email format";
    }

    if (tempAccount.phone.length !== 10 || !tempAccount.phone.startsWith("0")) {
      errors.phone = "Phone number must be exactly 10 digits and start with 0";
    }

    if (Object.keys(errors).length === 0) {
      try {
        const response = await updateAccountDetails(user.userId, tempAccount);
        messageApi.success("Account details updated successfully!");
        setTimeout(() => {
          setIsEditing(false);
          // Refresh customer info
          if (user && user.userId) {
            fetchCustomerInfo(user.userId).then(setCustomerInfo);
          }
        }, 2000);
      } catch (error) {
        messageApi.error("Failed to update account details. Please try again.");
      }
    } else {
      setAccountErrors(errors);
    }
    setIsLoading(false);
  };

  const handlePasswordResetSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setIsLoading(true);
    messageApi.destroy();

    const errors = validatePasswordReset(passwordResetData);

    if (Object.keys(errors).length === 0) {
      try {
        if (passwordChangeStep === 1) {
          const response = await requestPasswordReset({
            email: passwordResetData.email,
          });
          if (response.status === 200) {
            messageApi.success(
              "OTP sent to your email. Please check your inbox."
            );
            setPasswordChangeStep(2); // Move to OTP verification step
          } else {
            messageApi.error(
              response.data?.message || "Failed to send OTP. Please try again."
            );
          }
        } else if (passwordChangeStep === 2) {
          const response = await confirmPasswordReset({
            email: passwordResetData.email,
            otp: passwordResetData.otp,
            newPassword: passwordResetData.newPassword,
          });
          if (response.status === 200) {
            messageApi.success("Password changed successfully!");
            setTimeout(() => {
              setPasswordChangeStep(0); // Reset password change flow
            }, 2000);
          } else {
            messageApi.error(
              response.data?.message ||
                "Failed to change password. Invalid OTP or new password."
            );
          }
        }
      } catch (error: any) {
        messageApi.error(
          error.response?.data?.message ||
            "An error occurred during password reset."
        );
      }
    } else {
      setPasswordResetErrors(errors);
    }
    setIsLoading(false);
  };

  const handlePhoneChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    const numericValue = value.replace(/[^0-9]/g, "");

    setTempAccount((prevAccount) => ({
      ...prevAccount,
      [name]: numericValue,
    }));

    if (numericValue.length === 10 && numericValue.startsWith("0")) {
      setAccountErrors((prevErrors) => ({
        ...prevErrors,
        phone: undefined,
      }));
    } else {
      setAccountErrors((prevErrors) => ({
        ...prevErrors,
        phone: "Phone number must be exactly 10 digits and start with 0",
      }));
    }
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return new Intl.DateTimeFormat("en-GB", {
      year: "numeric",
      month: "2-digit",
      day: "2-digit",
    }).format(date);
  };

  const [customerInfo, setCustomerInfo] = useState<any>(null);
  useEffect(() => {
    if (user && user.userId) {
      const getCustomerInfo = async () => {
        const info = await fetchCustomerInfo(user.userId);
        if (info) {
          setCustomerInfo(info);
        }
      };
      getCustomerInfo();
    }
  }, [user]);

  return (
    <div>
      <AccountCus />
      <MainContainer>
        <Section>
          <ProfileTitle>My Account</ProfileTitle>
          <InfoSection>
            <InfoContainer>
              <Column>
                <Row>
                  <InfoTitle>Account Details</InfoTitle>
                  {!isEditing && passwordChangeStep === 0 && (
                    <ActionButtons>
                      <EditButton onClick={handleEditStart}>
                        Edit Details
                      </EditButton>
                      <EditButton onClick={handlePasswordEditStart}>
                        Change Password
                      </EditButton>
                    </ActionButtons>
                  )}
                </Row>
                {customerInfo && (
                  <Row>
                    <Column>
                      {contextHolder}
                      {isEditing ? (
                        <form onSubmit={handleAccountSubmit}>
                          <DetailGroup>
                            <Label>NAME</Label>
                            <InlineEditContainer>
                              <InlineInput
                                type="text"
                                name="name"
                                value={tempAccount.name}
                                onChange={handleAccountChange}
                                disabled={isLoading}
                                placeholder="Name"
                              />
                            </InlineEditContainer>
                            {accountErrors.name && (
                              <ErrorText>{accountErrors.name}</ErrorText>
                            )}
                          </DetailGroup>

                          <DetailGroup>
                            <Label>EMAIL</Label>
                            <InlineInput
                              type="email"
                              name="email"
                              value={tempAccount.email}
                              onChange={handleAccountChange}
                              disabled={isLoading}
                              placeholder="Email address"
                            />
                            {accountErrors.email && (
                              <ErrorText>{accountErrors.email}</ErrorText>
                            )}
                            {!validateEmail(tempAccount.email) && (
                              <ErrorText>Invalid email format</ErrorText>
                            )}
                          </DetailGroup>

                          <DetailGroup>
                            <Label>PHONE</Label>
                            <InlineInput
                              type="text"
                              name="phone"
                              value={tempAccount.phone}
                              onChange={handlePhoneChange}
                              disabled={isLoading}
                              placeholder="Phone number"
                            />
                            {accountErrors.phone && (
                              <ErrorText>{accountErrors.phone}</ErrorText>
                            )}
                          </DetailGroup>

                          <DetailGroup>
                            <Label>ADDRESS</Label>
                            <InlineInput
                              type="text"
                              name="address"
                              value={tempAccount.address}
                              onChange={handleAccountChange}
                              disabled={isLoading}
                              placeholder="Address"
                            />
                          </DetailGroup>

                          <InlineActions>
                            <ActionButton
                              type="submit"
                              disabled={isLoading}
                              className="save-button"
                            >
                              {isLoading ? "Saving..." : "Save Changes"}
                            </ActionButton>
                            <ActionButton
                              type="button"
                              onClick={handleCancel}
                              disabled={isLoading}
                              className="cancel-button"
                            >
                              Cancel
                            </ActionButton>
                          </InlineActions>
                        </form>
                      ) : passwordChangeStep === 1 ? (
                        <form onSubmit={handlePasswordResetSubmit}>
                          <DetailGroup>
                            <Label>EMAIL</Label>
                            <InlineInput
                              type="email"
                              name="email"
                              value={passwordResetData.email}
                              onChange={handlePasswordResetChange}
                              disabled={isLoading}
                              placeholder="Enter your email"
                            />
                            {passwordResetErrors.email && (
                              <ErrorText>{passwordResetErrors.email}</ErrorText>
                            )}
                          </DetailGroup>
                          <InlineActions>
                            <ActionButton
                              type="submit"
                              disabled={isLoading}
                              className="save-button"
                            >
                              {isLoading ? "Sending OTP..." : "Send OTP"}
                            </ActionButton>
                            <ActionButton
                              type="button"
                              onClick={handleCancel}
                              disabled={isLoading}
                              className="cancel-button"
                            >
                              Cancel
                            </ActionButton>
                          </InlineActions>
                        </form>
                      ) : passwordChangeStep === 2 ? (
                        <form onSubmit={handlePasswordResetSubmit}>
                          <DetailGroup>
                            <Label>OTP</Label>
                            <InlineInput
                              type="text"
                              name="otp"
                              value={passwordResetData.otp}
                              onChange={handlePasswordResetChange}
                              disabled={isLoading}
                              placeholder="Enter OTP"
                            />
                            {passwordResetErrors.otp && (
                              <ErrorText>{passwordResetErrors.otp}</ErrorText>
                            )}
                          </DetailGroup>
                          <DetailGroup>
                            <Label>NEW PASSWORD</Label>
                            <InlineInput
                              type="password"
                              name="newPassword"
                              value={passwordResetData.newPassword}
                              onChange={handlePasswordResetChange}
                              disabled={isLoading}
                              placeholder="Enter new password"
                            />
                            {passwordResetErrors.newPassword && (
                              <ErrorText>
                                {passwordResetErrors.newPassword}
                              </ErrorText>
                            )}
                          </DetailGroup>
                          <DetailGroup>
                            <Label>CONFIRM NEW PASSWORD</Label>
                            <InlineInput
                              type="password"
                              name="confirmNewPassword"
                              value={passwordResetData.confirmNewPassword}
                              onChange={handlePasswordResetChange}
                              disabled={isLoading}
                              placeholder="Confirm new password"
                            />
                            {passwordResetErrors.confirmNewPassword && (
                              <ErrorText>
                                {passwordResetErrors.confirmNewPassword}
                              </ErrorText>
                            )}
                          </DetailGroup>
                          <InlineActions>
                            <ActionButton
                              type="submit"
                              disabled={isLoading}
                              className="save-button"
                            >
                              {isLoading
                                ? "Changing Password..."
                                : "Change Password"}
                            </ActionButton>
                            <ActionButton
                              type="button"
                              onClick={handleCancel}
                              disabled={isLoading}
                              className="cancel-button"
                            >
                              Cancel
                            </ActionButton>
                          </InlineActions>
                        </form>
                      ) : (
                        <DataGrid>
                          <DataColumn>
                            <DetailGroup>
                              <Label>NAME</Label>
                              <Detail>{customerInfo.fullName}</Detail>
                            </DetailGroup>
                            <DetailGroup>
                              <Label>EMAIL</Label>
                              <Detail>{customerInfo.email}</Detail>
                            </DetailGroup>
                            <DetailGroup>
                              <Label>PHONE</Label>
                              <Detail>{customerInfo.phone}</Detail>
                            </DetailGroup>
                            <DetailGroup>
                              <Label>ADDRESS</Label>
                              <Detail>{customerInfo.address}</Detail>
                            </DetailGroup>
                          </DataColumn>
                          <DataColumn>
                            <DetailGroup>
                              <Label>ROLE</Label>
                              <Detail>{customerInfo.roleName}</Detail>
                            </DetailGroup>
                            <DetailGroup>
                              <Label>ACTIVE</Label>
                              <Detail>
                                {customerInfo.isActive ? "Yes" : "No"}
                              </Detail>
                            </DetailGroup>
                            <DetailGroup>
                              <Label>CREATED AT</Label>
                              <Detail>
                                {customerInfo.createdAt
                                  ? formatDate(customerInfo.createdAt)
                                  : "N/A"}
                              </Detail>
                            </DetailGroup>
                          </DataColumn>
                        </DataGrid>
                      )}
                    </Column>
                  </Row>
                )}
              </Column>
            </InfoContainer>
          </InfoSection>
        </Section>
      </MainContainer>
    </div>
  );
};

const Row = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 30px;
  margin-bottom: 40px;
  flex-wrap: wrap;
`;

const ActionButtons = styled.div`
  display: flex;
  gap: 20px;
  flex-wrap: wrap;
`;

const EditButton = styled.button`
  font-size: 14px;
  padding: 12px 24px;
  background-color: #fff9f7;
  color: #151542;
  border: 1px solid #151542;
  cursor: pointer;
  transition: all 0.45s ease;
  font-family: "Gantari", sans-serif;
  font-weight: 600;
  border-radius: 2 px;

  &:hover {
    background-color: #102c57;
    color: #fff;
    transition: all 0.45s ease;
  }

  @media (max-width: 768px) {
    padding: 8px 16px;
    font-size: 12px;
  }
`;

const MessageBox = styled.div<{ type: "success" | "error" }>`
  padding: 12px 16px;
  margin-bottom: 20px;
  border-radius: 8px;
  font-family: "Poppins", sans-serif;
  font-size: 14px;
  font-weight: 500;
  background-color: ${(props) =>
    props.type === "success" ? "#d4edda" : "#f8d7da"};
  color: ${(props) => (props.type === "success" ? "#155724" : "#721c24")};
  border: 1px solid
    ${(props) => (props.type === "success" ? "#c3e6cb" : "#f5c6cb")};
  display: flex;
  align-items: center;
  gap: 8px;

  &::before {
    content: ${(props) => (props.type === "success" ? '"✓"' : '"✕"')};
    font-weight: bold;
  }
`;

const MainContainer = styled.div`
  background-color: #fff;
  display: flex;
  flex-direction: column;
  align-items: center;
  min-height: 100vh;
  padding: 20px 0;
`;

const Section = styled.section`
  background-color: #fff;
  color: #000;
  font: 400 15px / 150% "Crimson Text", sans-serif;
  width: 100%;
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 20px;
`;

const ProfileTitle = styled.div`
  position: relative;
  margin: 15px 0 50px;
  font: 600 32px "Crimson Text", sans-serif;
  text-align: center;
  color: #151542;
`;

const InfoSection = styled.section`
  width: 100%;
  max-width: 1000px;
  margin: 0 auto;
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
  padding: 50px;
`;

const InfoContainer = styled.div`
  display: flex;
  flex-direction: column;
  width: 100%;
`;

const Column = styled.div`
  display: flex;
  flex-direction: column;
  width: 100%;
`;

const InfoTitle = styled.div`
  font-family: "Crimson Text", sans-serif;
  font-size: 28px;
  font-weight: 600;
  color: #151542;
`;

const DetailGroup = styled.div`
  display: flex;
  flex-direction: column;
  margin-bottom: 30px;
  padding: 25px;
  background: #f8f9fa;
  border-radius: 8px;
  border-left: 4px solid #151542;
`;

const Detail = styled.span`
  font-family: "Poppins", sans-serif;
  font-weight: 500;
  letter-spacing: 0.5px;
  margin-top: 10px;
  color: #151542;
  line-height: 1.6;
  font-size: 16px;
`;

const Label = styled.label`
  font-family: "Poppins", sans-serif;
  font-size: 14px;
  font-weight: 600;
  color: #6c757d;
  text-transform: uppercase;
  letter-spacing: 1px;
`;

const InlineEditContainer = styled.div`
  display: grid;
  grid-template-columns: 1fr;
  gap: 15px;
  margin-top: 8px;

  @media (max-width: 768px) {
    grid-template-columns: 1fr;
  }
`;

const InlineInput = styled.input`
  width: 100%;
  padding: 12px 16px;
  border: 2px solid #e1e5e9;
  border-radius: 8px;
  box-sizing: border-box;
  font-family: "Poppins", sans-serif;
  font-size: 14px;
  transition: all 0.3s ease;
  background-color: #fff;

  &:focus {
    outline: none;
    border-color: #151542;
    box-shadow: 0 0 0 3px rgba(21, 21, 66, 0.1);
  }

  &:disabled {
    background-color: #f8f9fa;
    color: #6c757d;
    cursor: not-allowed;
  }

  &::placeholder {
    color: #adb5bd;
  }
`;

const InlineActions = styled.div`
  display: flex;
  gap: 15px;
  margin-top: 30px;
  padding-top: 20px;
  border-top: 1px solid #e1e5e9;
  justify-content: flex-end;

  @media (max-width: 768px) {
    flex-direction: column;
  }
`;

const ActionButton = styled.button`
  font-size: 14px;
  padding: 12px 24px;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.3s ease;
  font-family: "Gantari", sans-serif;
  font-weight: 600;
  min-width: 120px;

  &.save-button {
    background-color: #151542;
    color: #fff;

    &:hover {
      background-color: #0f0f2e;
      transform: translateY(-2px);
    }

    &:disabled {
      background-color: #6c757d;
      cursor: not-allowed;
      transform: none;
    }
  }

  &.cancel-button {
    background-color: #fff;
    color: #151542;
    border: 2px solid #151542;

    &:hover {
      background-color: #151542;
      color: #fff;
      transform: translateY(-2px);
    }

    &:disabled {
      background-color: #f8f9fa;
      color: #6c757d;
      border-color: #6c757d;
      cursor: not-allowed;
      transform: none;
    }
  }
`;

const ErrorText = styled.span`
  color: red;
  font-size: 12px;
  margin-top: 5px;
  display: block;
`;

const DataGrid = styled.div`
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 35px;
  width: 100%;

  @media (max-width: 768px) {
    grid-template-columns: 1fr;
    gap: 20px;
  }
`;

const DataColumn = styled.div`
  display: flex;
  flex-direction: column;
  gap: 3px;
`;

export default Account;
