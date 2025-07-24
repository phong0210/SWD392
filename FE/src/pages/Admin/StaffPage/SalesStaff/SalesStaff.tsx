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
import { deleteStaff, promoteToStaff } from "@/services/accountApi";
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
  const [filteredStaffs, setFilteredStaffs] = useState<any[]>([]); // ✅ thêm state
  const [nonStaffUsers, setNonStaffUsers] = useState<any[]>([]);
  const [isPromoting, setIsPromoting] = useState(false);
  const [api, contextHolder] = notification.useNotification();
  const [searchText, setSearchText] = useState("");
  const [emailOptions, setEmailOptions] = useState<any[]>([]);
  const [selectedUserInfo, setSelectedUserInfo] = useState<any>(null);

  const openNotification = (
    type: "success" | "info" | "warning" | "error",
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
      let allUsers;
      if (response.data && Array.isArray(response.data)) {
        allUsers = response.data;
      } else if (response.data && response.data.data && Array.isArray(response.data.data)) {
        allUsers = response.data.data;
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
      (user: any) =>
        !["SaleStaff", "Manager", "HeadOfficeAdmin", "DeliveryStaff"].includes(user.roleName) &&
        user.isActive !== false // ✅ Chỉ lấy user chưa bị ban
    ).map((user: any) => ({
      key: user.id || user.AccountID,
      email: user.email || user.Email || "",
      name: user.name || user.Name || "",
      phone: user.phone || user.PhoneNumber || "",
    }));

      setStaffs(salesStaff);
      setFilteredStaffs(salesStaff); // ✅ gán dữ liệu lọc ban đầu
      setNonStaffUsers(nonStaff);

      const options = nonStaff.map((user: any) => ({
        value: user.email,
        label: `${user.email} (${user.name})`,
        user: user
      }));
      setEmailOptions(options);
    } catch (error) {
      openNotification("error", "Fetch", "Failed to load user data");
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleDemote = async (staffID: string) => {
    try {
      await deleteStaff(staffID);
      openNotification("success", "Demote", "Staff demoted to Customer successfully.");
      fetchData();
    } catch (error: any) {
      openNotification("error", "Demote", error.message || "Failed to demote staff");
    }
  };

  const columns = [
    {
      title: "Staff ID",
      dataIndex: "staffID",
      editable: true,
      sorter: (a: any, b: any) => {
        const aID = String(a.staffID || "");
        const bID = String(b.staffID || "");
        return aID.localeCompare(bID);
      },
    },
    {
      title: "Staff Name",
      dataIndex: "staffName",
      defaultSortOrder: "descend" as SortOrder,
      editable: true,
      sorter: (a: any, b: any) =>
        (a.staffName || "").localeCompare(b.staffName || ""),
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
      sorter: (a: any, b: any) =>
        (a.email || "").localeCompare(b.email || ""),
    },
    {
      title: "Demote",
      dataIndex: "delete",
      className: "TextAlign",
      render: (_: unknown, record: any) =>
        staffs.length >= 1 ? (
          <Popconfirm
            title="Are you sure you want to demote this staff to Customer?"
            onConfirm={() => handleDemote(record.key)}
          >
            <a>Demote</a>
          </Popconfirm>
        ) : null,
    },
  ];

  const mergedColumns = columns.map((col) =>
    !col.editable
      ? col
      : {
          ...col,
          onCell: (record: any) => ({
            record,
            inputType: col.dataIndex === "phoneNumber" ? "number" : "text",
            dataIndex: col.dataIndex,
            title: col.title,
          }),
        }
  );

  // ✅ SEARCH
  const onSearch = (value: string) => {
    const keyword = value.toLowerCase().trim();
    const filtered = staffs.filter((staff) =>
      staff.staffName.toLowerCase().includes(keyword) ||
      staff.email.toLowerCase().includes(keyword) ||
      staff.phoneNumber.toLowerCase().includes(keyword)
    );
    setFilteredStaffs(filtered);
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

  const handleEmailSelect = (value: string) => {
    const selectedOption = emailOptions.find(option => option.value === value);
    if (selectedOption) {
      setSelectedUserInfo(selectedOption.user);
      form.setFieldsValue({ email: value });
    }
  };

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

  const SubmitButton: React.FC<{ form: FormInstance; children: React.ReactNode }> = ({ form, children }) => {
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
        const roleName = "SaleStaff";
        const response = await promoteToStaff(email, roleName, salary, hireDate.toISOString());
        if (response.status === 200) {
          openNotification("success", "Promote", "User promoted to staff successfully.");
          fetchData();
          setIsPromoting(false);
          form.resetFields();
        } else {
          openNotification("error", "Promote", response.data?.message || "Failed to promote user.");
        }
      } catch (error: any) {
        openNotification("error", "Promote", error.message || "Unexpected error");
      }
    };

    return (
      <Button type="primary" htmlType="submit" disabled={!submittable} onClick={promoteUser}>
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
              {!isPromoting ? (
                <>
                  <Styled.SearchArea>
                    <Input
                      className="searchInput"
                      placeholder="Search Staff..."
                      value={searchText}
                      onChange={(e) => {
                        const value = e.target.value;
                        setSearchText(value);
                        if (value.trim() === "") setFilteredStaffs(staffs); // ✅ reset nếu rỗng
                      }}
                      onKeyDown={handleKeyPress}
                      prefix={<SearchOutlined />}
                    />
                  </Styled.SearchArea>
                  <Styled.AddButton>
                    <button onClick={handlePromoteNew}>
                      <PlusCircleOutlined />
                      Promote User to Staff
                    </button>
                  </Styled.AddButton>
                </>
              ) : (
                <Styled.AddContent_Title>
                  <p>Promote User to Sales Staff</p>
                </Styled.AddContent_Title>
              )}
            </Styled.AdPageContent_Head>

            <Styled.AdminTable>
              {isPromoting ? (
                <Form form={form} layout="vertical">
                  <Styled.FormItem>
                    <Form.Item
                      name="email"
                      label="User E-mail"
                      rules={[{ required: true, message: "Please select a user E-mail!" }]}
                    >
                      <AutoComplete
                        placeholder="Type to search user email..."
                        onSelect={handleEmailSelect}
                        onSearch={handleEmailSearch}
                        options={emailOptions}
                        filterOption={false}
                        showSearch
                        allowClear
                      />
                    </Form.Item>

                    {selectedUserInfo && (
                      <div style={{ padding: '10px', backgroundColor: '#f6f6f6', borderRadius: '4px' }}>
                        <strong>User:</strong> {selectedUserInfo.name} <br />
                        <strong>Email:</strong> {selectedUserInfo.email}
                      </div>
                    )}
                  </Styled.FormItem>

                  <Styled.FormItem>
                    <Form.Item name="salary" label="Salary" rules={[{ required: true, message: "Please input salary!" }]}>
                      <InputNumber min={0} style={{ width: "100%" }} />
                    </Form.Item>
                  </Styled.FormItem>
                  <Styled.FormItem>
                    <Form.Item name="hireDate" label="Hire Date" rules={[{ required: true, message: "Please select date!" }]}>
                      <DatePicker style={{ width: "100%" }} />
                    </Form.Item>
                  </Styled.FormItem>

                  <Styled.ActionBtn>
                    <SubmitButton form={form}><SaveOutlined /> Promote</SubmitButton>
                    <Button onClick={handleCancel} style={{ marginLeft: 10 }}>Cancel</Button>
                  </Styled.ActionBtn>
                </Form>
              ) : (
                <Form form={form} component={false}>
                  <Table
                    components={{ body: { cell: EditableCell } }}
                    bordered
                    dataSource={filteredStaffs}
                    columns={mergedColumns}
                    pagination={{ pageSize: 6, showQuickJumper: true }}
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
