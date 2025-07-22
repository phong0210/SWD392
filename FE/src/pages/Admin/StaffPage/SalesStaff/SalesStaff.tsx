import React, { useEffect, useState } from "react";
import * as Styled from "./SalesStaff.styled";
import {
  SearchOutlined,
  PlusCircleOutlined,
  SaveOutlined,
} from "@ant-design/icons";
import type { FormInstance } from "antd";
import {
  Form,
  Input,
  InputNumber,
  Popconfirm,
  Table,
  Button,
  notification,
  Select,
  DatePicker,
  AutoComplete,
} from "antd";
import Sidebar from "../../../../components/Admin/Sidebar/Sidebar";
import StaffMenu from "@/components/Admin/SalesStaffMenu/StaffMenu";
import { SortOrder } from "antd/es/table/interface";
import {
  deleteAccount,
  updateAccount,
} from "@/services/authAPI";
import { promoteToStaff } from "@/services/accountApi";
import { showAllAccounts } from "@/services/authAPI";

interface EditableCellProps {
  editing: boolean;
  dataIndex: keyof any;
  title: React.ReactNode;
  inputType: "number" | "text";
  record: any;
  index: number;
  children: React.ReactNode;
}

const EditableCell: React.FC<EditableCellProps> = ({
  editing,
  dataIndex,
  title,
  inputType,
  children,
  ...restProps
}) => {
  const inputNode = inputType === "number" ? <InputNumber /> : <Input />;

  return (
    <td {...restProps}>
      {editing ? (
        <Form.Item
          name={dataIndex.toString()}
          style={{ margin: 0 }}
          rules={[
            {
              required: true,
              message: `Please Input ${title}!`,
            },
          ]}
        >
          {inputNode}
        </Form.Item>
      ) : (
        children
      )}
    </td>
  );
};

const SalesStaff = () => {
  const [form] = Form.useForm();
  const [staffs, setStaffs] = useState<any[]>([]);
  const [nonStaffUsers, setNonStaffUsers] = useState<any[]>([]);
  const [isPromoting, setIsPromoting] = useState(false);
  const [api, contextHolder] = notification.useNotification();
  const [searchText, setSearchText] = useState("");
  const [emailOptions, setEmailOptions] = useState<any[]>([]);
  const [selectedUserInfo, setSelectedUserInfo] = useState<any>(null);

  type NotificationType = "success" | "info" | "warning" | "error";

  const openNotification = (
    type: NotificationType,
    method: string,
    error: string
  ) => {
    api[type]({
      message: type === "success" ? "Notification" : "Error",
      description:
        type === "success" ? `${method} user successfully` : error,
    });
  };

  const fetchData = async () => {
    try {
      const response = await showAllAccounts();
      console.log("API Response:", response);
      
      let allUsers;
      if (response.data && Array.isArray(response.data)) {
        allUsers = response.data;
      } else if (response.data && response.data.data && Array.isArray(response.data.data)) {
        allUsers = response.data.data;
      } else if (Array.isArray(response)) {
        allUsers = response;
      } else {
        throw new Error("Invalid API response format");
      }

      const salesStaff = allUsers.filter(
        (user: any) => user.roleName === "SaleStaff" 
      ).map((staff: any) => ({
        key: staff.id || staff.AccountID,
        staffID: staff.id || staff.AccountID,
        staffName: staff.name || staff.Name || "",
        phoneNumber: staff.phone || staff.PhoneNumber || "",
        email: staff.email || staff.Email || "",
        password: staff.password || staff.Password || "",
        role: staff.roleName || staff.Role,
        address: staff.address || "",
      }));

      const nonStaff = allUsers.filter(
        (user: any) => user.roleName !== "SaleStaff" && user.roleName !== "Manager" && user.roleName !== "HeadOfficeAdmin" && user.roleName !== "DeliveryStaff"
      ).map((user: any) => ({
        key: user.id || user.AccountID,
        email: user.email || user.Email || "",
        name: user.name || user.Name || "",
        phone: user.phone || user.PhoneNumber || "",
      }));
      
      console.log("Sales Staff:", salesStaff);
      console.log("Non-Staff Users:", nonStaff);
      setStaffs(salesStaff);
      setNonStaffUsers(nonStaff);
      
      // Prepare email options for autocomplete
      const options = nonStaff.map((user: any) => ({
        value: user.email,
        label: `${user.email} (${user.name})`,
        user: user
      }));
      setEmailOptions(options);
    } catch (error) {
      console.error("Failed to fetch user data:", error);
      openNotification("error", "Fetch", "Failed to load user data");
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleDelete = async (staffID: string | number) => {
    try {
      await deleteAccount(staffID);
      openNotification("success", "Delete", "");
      fetchData();
    } catch (error: any) {
      console.error("Failed to delete staff:", error);
      openNotification("error", "Delete", error.message || "Failed to delete staff");
    }
  };

  const handleBan = async (email: string) => {
    try {
      const response = await updateAccount(email, {
        Role: "ROLE_BAN",
      });
      console.log("Ban Response:", response.data);
      if (response.status === 200) {
        openNotification("success", "Ban", "Staff has been banned successfully.");
        fetchData();
      } else {
        openNotification("error", "Ban", "Failed to ban staff.");
      }
    } catch (error: any) {
      console.error("Failed to ban staff:", error);
      openNotification("error", "Ban", error.message);
    }
  };

  const columns = [
    {
      title: "Staff ID",
      dataIndex: "staffID",
      editable: true,
      sorter: (a: any, b: any) => {
        // Handle both string and number IDs
        const aID = typeof a.staffID === 'string' ? a.staffID : String(a.staffID || "");
        const bID = typeof b.staffID === 'string' ? b.staffID : String(b.staffID || "");
        return aID.localeCompare(bID);
      },
    },
    {
      title: "Staff Name",
      dataIndex: "staffName",
      defaultSortOrder: "descend" as SortOrder,
      editable: true,
      sorter: (a: any, b: any) => {
        // Safe string sorting with null/undefined checks
        const aName = a.staffName || "";
        const bName = b.staffName || "";
        return aName.length - bName.length;
      },
    },
    {
      title: "Phone Number",
      dataIndex: "phoneNumber",
      editable: true,
    },
    {
      title: "Email",
      dataIndex: "email",
      editable: true,
      sorter: (a: any, b: any) => {
        // Safe email sorting
        const aEmail = a.email || "";
        const bEmail = b.email || "";
        return aEmail.localeCompare(bEmail);
      },
    },
    {
      title: "Delete",
      dataIndex: "delete",
      className: "TextAlign",
      render: (_: unknown, record: any) =>
        staffs.length >= 1 ? (
          <Popconfirm
            title="Sure to delete?"
            onConfirm={() => handleDelete(record.key)}
          >
            <a>Delete</a>
          </Popconfirm>
        ) : null,
    },
    {
      title: "Ban",
      dataIndex: "ban",
      className: "TextAlign",
      render: (_: unknown, record: any) =>
        staffs.length >= 1 ? (
          <Popconfirm
            title="Sure to ban?"
            onConfirm={() => handleBan(record.email)}
          >
            <a>Ban</a>
          </Popconfirm>
        ) : null,
    },
  ];

  const mergedColumns = columns.map((col) => {
    if (!col.editable) {
      return col;
    }
    return {
      ...col,
      onCell: (record: any) => ({
        record,
        inputType: col.dataIndex === "phoneNumber" ? "number" : "text",
        dataIndex: col.dataIndex,
        title: col.title,
      }),
    };
  });

  // SEARCH
  const onSearch = (value: string) => {
    console.log("Search:", value);
    // Implement search logic here
  };

  const handleKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      onSearch(searchText);
    }
  };

  const handlePromoteNew = () => {
    setIsPromoting(true);
    setSelectedUserInfo(null);
    form.resetFields();
  };

  const handleCancel = () => {
    setIsPromoting(false);
    setSelectedUserInfo(null);
    form.resetFields();
  };

  // Handle email selection for autocomplete
  const handleEmailSelect = (value: string) => {
    const selectedOption = emailOptions.find(option => option.value === value);
    if (selectedOption) {
      setSelectedUserInfo(selectedOption.user);
      form.setFieldsValue({ email: value });
    }
  };

  // Filter email options based on search input
  const handleEmailSearch = (searchText: string) => {
    const filteredOptions = nonStaffUsers
      .filter((user: any) => 
        user.email.toLowerCase().includes(searchText.toLowerCase()) ||
        user.name.toLowerCase().includes(searchText.toLowerCase())
      )
      .map((user: any) => ({
        value: user.email,
        label: `${user.email} (${user.name})`,
        user: user
      }));
    setEmailOptions(filteredOptions);
  };

  // SUBMIT FORM
  interface SubmitButtonProps {
    form: FormInstance;
  }

  const SubmitButton: React.FC<React.PropsWithChildren<SubmitButtonProps>> = ({
    form,
    children,
  }) => {
    const [submittable, setSubmittable] = useState(false);
    const values = Form.useWatch([], form);

    useEffect(() => {
      form
        .validateFields({ validateOnly: true })
        .then(() => setSubmittable(true))
        .catch(() => setSubmittable(false));
    }, [form, values]);

    const promoteUser = async () => {
      try {
        const { email, salary, hireDate } = await form.validateFields();
        const roleName = "SaleStaff"; // Hardcoded for now, can be dynamic later

        const response = await promoteToStaff(email, roleName, salary, hireDate.toISOString());
        if (response.status === 200) {
          openNotification("success", "Promote", "User promoted to staff successfully.");
          fetchData();
          setIsPromoting(false);
          setSelectedUserInfo(null);
          form.resetFields();
        } else {
          openNotification("error", "Promote", response.data?.message || "Failed to promote user.");
        }
      } catch (error: any) {
        openNotification("error", "Promote", error.message || "An unexpected error occurred.");
      }
    };

    return (
      <Button
        type="primary"
        htmlType="submit"
        disabled={!submittable}
        onClick={promoteUser}
      >
        {children}
      </Button>
    );
  };

  return (
    <>
      {contextHolder}
      <Styled.GlobalStyle />
      <Styled.AdminArea>
        <Sidebar />

        <Styled.AdminPage>
          <StaffMenu />

          <Styled.AdPageContent>
            <Styled.AdPageContent_Head>
              {(!isPromoting && (
                <>
                  <Styled.SearchArea>
                    <Input
                      className="searchInput"
                      type="text"
                      placeholder="Search here..."
                      value={searchText}
                      onChange={(e) => setSearchText(e.target.value)}
                      onKeyPress={handleKeyPress}
                      prefix={<SearchOutlined className="searchIcon" />}
                    />
                  </Styled.SearchArea>
                  <Styled.AddButton>
                    <button onClick={handlePromoteNew}>
                      <PlusCircleOutlined />
                      Promote User to Staff
                    </button>
                  </Styled.AddButton>
                </>
              )) || (
                <>
                  <Styled.AddContent_Title>
                    <p>Promote User to Sales Staff</p>
                  </Styled.AddContent_Title>
                </>
              )}
            </Styled.AdPageContent_Head>

            <Styled.AdminTable>
              {isPromoting ? (
                <>
                  <Form
                    form={form}
                    layout="vertical"
                    className="AdPageContent_Content"
                  >
                    <Styled.FormItem>
                      <Form.Item
                        name="email"
                        label="User E-mail"
                        rules={[
                          {
                            required: true,
                            message: "Please select a user E-mail!",
                          },
                        ]}
                      >
                        <AutoComplete
                          placeholder="Type to search and select user email..."
                          onSelect={handleEmailSelect}
                          onSearch={handleEmailSearch}
                          options={emailOptions}
                          filterOption={false}
                          showSearch
                          allowClear
                          style={{ width: '100%', marginBottom: '16px' }}
                          dropdownStyle={{ maxHeight: 200, overflow: 'auto', zIndex: 1000 }}
                        />
                      </Form.Item>
                      
                      {/* Display selected user info */}
                      {selectedUserInfo && (
                        <div style={{ 
                          padding: '12px 16px', 
                          backgroundColor: '#f6f6f6', 
                          borderRadius: '4px',
                          marginTop: '12px',
                          marginBottom: '16px',
                          border: '1px solid #d9d9d9',
                          fontSize: '14px'
                        }}>
                          <strong>Selected User:</strong> {selectedUserInfo.name} <br />
                          <strong>Email:</strong> {selectedUserInfo.email} <br />
                          {selectedUserInfo.phone && (
                            <>
                              <strong>Phone:</strong> {selectedUserInfo.phone}
                            </>
                          )}
                        </div>
                      )}
                    </Styled.FormItem>
                    
                    <Styled.FormItem>
                      <Form.Item
                        name="salary"
                        label="Salary"
                        rules={[{ required: true, message: "Please input salary!" }]}
                      >
                        <InputNumber min={0} style={{ width: '100%' }} placeholder="Enter salary" />
                      </Form.Item>
                    </Styled.FormItem>
                    <Styled.FormItem>
                      <Form.Item
                        name="hireDate"
                        label="Hire Date"
                        rules={[{ required: true, message: "Please select hire date!" }]}
                      >
                        <DatePicker style={{ width: '100%' }} />
                      </Form.Item>
                    </Styled.FormItem>

                    <Styled.ActionBtn>
                      <SubmitButton form={form}>
                        <SaveOutlined />
                        Promote
                      </SubmitButton>
                      <Button
                        onClick={handleCancel}
                        style={{ marginLeft: "10px" }}
                      >
                        Cancel
                      </Button>
                    </Styled.ActionBtn>
                  </Form>
                </>
              ) : (
                <Form form={form} component={false}>
                  <Table
                    components={{
                      body: {
                        cell: EditableCell,
                      },
                    }}
                    bordered
                    dataSource={staffs}
                    columns={mergedColumns}
                    rowClassName="editable-row"
                    pagination={{
                      pageSize: 6,
                      showSizeChanger: false,
                      showQuickJumper: true,
                    }}
                    loading={staffs.length === 0}
                  />
                </Form>
              )}
            </Styled.AdminTable>
          </Styled.AdPageContent>
        </Styled.AdminPage>
      </Styled.AdminArea>
    </>
  );
};

export default SalesStaff;