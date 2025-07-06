/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import styled from "styled-components";
import { ChangeEvent, FormEvent, useState, useRef, useEffect } from "react";
import AccountCus from "@/components/Customer/Account Details/AccountCus";
import { useSelector } from 'react-redux';
import { RootState } from '@/store';
import { getAccountDetail } from "@/services/authAPI";

interface Account {
  name: string;
  phone: string;
  email: string;
  password: string;
}

const fetchCustomerInfo = async (AccountID: number) => {
  try {
    const { data } = await getAccountDetail(AccountID);
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
    const response = await fetch(`${import.meta.env.VITE_API_URL}/api/user/${userId}/update`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      },
      body: JSON.stringify({
        name: accountData.name,
        email: accountData.email,
        phone: accountData.phone
      })
    });

    if (!response.ok) {
      throw new Error('Failed to update account details');
    }

    return await response.json();
  } catch (error) {
    console.error('Error updating account details:', error);
    throw new Error('Failed to update account details');
  }
};

const Account = () => {
  const [isEditing, setIsEditing] = useState(false);
  const [isPasswordEditing, setIsPasswordEditing] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [message, setMessage] = useState<{ type: 'success' | 'error', text: string } | null>(null);

  const [account, setAccount] = useState<Account>({
    name: "",
    phone: "",
    email: "",
    password: "",
  });

  const [tempAccount, setTempAccount] = useState<Account>({ ...account });
  const [tempPassword, setTempPassword] = useState({
    currentPassword: "",
    newPassword: "",
    confirmPassword: "",
  });

  const [accountErrors, setAccountErrors] = useState<Partial<Account>>({});
  const [passwordErrors, setPasswordErrors] = useState<{
    currentPassword?: string;
    newPassword?: string;
    confirmPassword?: string;
  }>({});

  const { user } = useSelector((state: RootState) => state.auth);

  const validateAccount = (account: Account) => {
    const errors: Partial<Account> = {};
    if (!account.name) errors.name = "Name is required";
    if (!account.email) errors.email = "Email is required";
    return errors;
  };

  const validatePassword = (passwordData: typeof tempPassword) => {
    const errors: typeof passwordErrors = {};
    if (!passwordData.currentPassword) errors.currentPassword = "Current password is required";
    if (!passwordData.newPassword) errors.newPassword = "New password is required";
    if (passwordData.newPassword.length < 6) errors.newPassword = "Password must be at least 6 characters";
    if (passwordData.newPassword !== passwordData.confirmPassword) {
      errors.confirmPassword = "Passwords do not match";
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
      });
    }
    setIsEditing(true);
    setMessage(null);
  };

  const handlePasswordEditStart = () => {
    setTempPassword({
      currentPassword: "",
      newPassword: "",
      confirmPassword: "",
    });
    setIsPasswordEditing(true);
    setMessage(null);
  };

  const handleCancel = () => {
    setIsEditing(false);
    setIsPasswordEditing(false);
    resetFormValues();
    setMessage(null);
  };

  const resetFormValues = () => {
    setAccountErrors({});
    setPasswordErrors({});
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

  const handlePasswordChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setTempPassword((prevPassword) => ({
      ...prevPassword,
      [name]: value,
    }));
  };

  const handleAccountSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setIsLoading(true);
    setMessage(null);

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
        setMessage({ type: 'success', text: 'Account details updated successfully!' });
        setTimeout(() => {
          setIsEditing(false);
          // Refresh customer info
          if (user && user.userId) {
            fetchCustomerInfo(user.userId).then(setCustomerInfo);
          }
        }, 2000);
      } catch (error) {
        setMessage({ type: 'error', text: 'Failed to update account details. Please try again.' });
      }
    } else {
      setAccountErrors(errors);
    }
    setIsLoading(false);
  };

  const handlePasswordSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setIsLoading(true);
    setMessage(null);

    const errors = validatePassword(tempPassword);

    if (Object.keys(errors).length === 0) {
      try {
        // TODO: Implement API call to change password
        // const response = await changePassword(tempPassword);
        setMessage({ type: 'success', text: 'Password changed successfully!' });
        setTimeout(() => {
          setIsPasswordEditing(false);
        }, 2000);
      } catch (error) {
        setMessage({ type: 'error', text: 'Failed to change password. Please check your current password.' });
      }
    } else {
      setPasswordErrors(errors);
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
                  {!isEditing && !isPasswordEditing && (
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
                      {message && (
                        <MessageBox type={message.type}>
                          {message.text}
                        </MessageBox>
                      )}
                      
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
                                                         {(accountErrors.name) && (
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
                            {accountErrors.email && <ErrorText>{accountErrors.email}</ErrorText>}
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
                            {accountErrors.phone && <ErrorText>{accountErrors.phone}</ErrorText>}
                          </DetailGroup>

                          <InlineActions>
                            <ActionButton type="submit" disabled={isLoading} className="save-button">
                              {isLoading ? 'Saving...' : 'Save Changes'}
                            </ActionButton>
                            <ActionButton type="button" onClick={handleCancel} disabled={isLoading} className="cancel-button">
                              Cancel
                            </ActionButton>
                          </InlineActions>
                        </form>
                      ) : isPasswordEditing ? (
                        <form onSubmit={handlePasswordSubmit}>
                          <DetailGroup>
                            <Label>CURRENT PASSWORD</Label>
                            <InlineInput
                              type="password"
                              name="currentPassword"
                              value={tempPassword.currentPassword}
                              onChange={handlePasswordChange}
                              disabled={isLoading}
                              placeholder="Current password"
                            />
                            {passwordErrors.currentPassword && (
                              <ErrorText>{passwordErrors.currentPassword}</ErrorText>
                            )}
                          </DetailGroup>

                          <DetailGroup>
                            <Label>NEW PASSWORD</Label>
                            <InlineInput
                              type="password"
                              name="newPassword"
                              value={tempPassword.newPassword}
                              onChange={handlePasswordChange}
                              disabled={isLoading}
                              placeholder="New password"
                            />
                            {passwordErrors.newPassword && (
                              <ErrorText>{passwordErrors.newPassword}</ErrorText>
                            )}
                          </DetailGroup>

                          <DetailGroup>
                            <Label>CONFIRM NEW PASSWORD</Label>
                            <InlineInput
                              type="password"
                              name="confirmPassword"
                              value={tempPassword.confirmPassword}
                              onChange={handlePasswordChange}
                              disabled={isLoading}
                              placeholder="Confirm new password"
                            />
                            {passwordErrors.confirmPassword && (
                              <ErrorText>{passwordErrors.confirmPassword}</ErrorText>
                            )}
                          </DetailGroup>

                          <InlineActions>
                            <ActionButton type="submit" disabled={isLoading} className="save-button">
                              {isLoading ? 'Changing...' : 'Change Password'}
                            </ActionButton>
                            <ActionButton type="button" onClick={handleCancel} disabled={isLoading} className="cancel-button">
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
                          </DataColumn>
                          <DataColumn>
                            <DetailGroup>
                              <Label>ROLE</Label>
                              <Detail>{customerInfo.roleName}</Detail>
                            </DetailGroup>
                            <DetailGroup>
                              <Label>ACTIVE</Label>
                              <Detail>{customerInfo.isActive ? 'Yes' : 'No'}</Detail>
                            </DetailGroup>
                            <DetailGroup>
                              <Label>CREATED AT</Label>
                              <Detail>{customerInfo.createdAt ? formatDate(customerInfo.createdAt) : 'N/A'}</Detail>
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
  border-radius: 2
  px;

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

const MessageBox = styled.div<{ type: 'success' | 'error' }>`
  padding: 12px 16px;
  margin-bottom: 20px;
  border-radius: 8px;
  font-family: "Poppins", sans-serif;
  font-size: 14px;
  font-weight: 500;
  background-color: ${props => props.type === 'success' ? '#d4edda' : '#f8d7da'};
  color: ${props => props.type === 'success' ? '#155724' : '#721c24'};
  border: 1px solid ${props => props.type === 'success' ? '#c3e6cb' : '#f5c6cb'};
  display: flex;
  align-items: center;
  gap: 8px;

  &::before {
    content: ${props => props.type === 'success' ? '"✓"' : '"✕"'};
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
