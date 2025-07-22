import * as Styled from "./Customer.styled";
import React, { useEffect, useState } from "react";
import { SearchOutlined } from "@ant-design/icons";
import type { TableColumnsType } from "antd";
import Sidebar from "../../../components/Admin/Sidebar/Sidebar";
import { Form, Input, InputNumber, notification, Popconfirm, Table, Tag } from "antd";
import { showAllAccounts, updateAccount } from "@/services/authAPI";
import { Role } from "@/utils/enum";

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

const Customer = () => {
  const [form] = Form.useForm();
  const [searchText, setSearchText] = useState("");
  const [customers, setCustomers] = useState<any[]>([]);
  const [api, contextHolder] = notification.useNotification();

  type NotificationType = "success" | "info" | "warning" | "error";

  const openNotification = (
    type: NotificationType,
    method: string,
    error: string
  ) => {
    api[type]({
      message: type === "success" ? "Notification" : "Error",
      description:
        type === "success" ? `${method} successfully` : error,
    });
  };

  const fetchData = async () => {
    try {
      const response = await showAllAccounts();
      console.log("API Response:", response);
      
      if (!response.data) {
        console.error("No data in response");
        openNotification("error", "Fetch", "No data received from server");
        return;
      }

      let accountsData;
      
      // The API response shows data is directly in response.data (it's an array)
      if (Array.isArray(response.data)) {
        accountsData = response.data;
      } else if (response.data.data && Array.isArray(response.data.data)) {
        accountsData = response.data.data;
      } else {
        console.error("Unexpected response structure:", response.data);
        openNotification("error", "Fetch", "Unexpected data format");
        return;
      }

      if (!Array.isArray(accountsData)) {
        console.error("Expected array but got:", typeof accountsData, accountsData);
        openNotification("error", "Fetch", "Invalid data format received");
        return;
      }

      // Filter customers based on roleName instead of Role
      const filteredCustomers = accountsData.filter((account: any) => 
        account.roleName && account.roleName.toLowerCase().includes('customer')
      );

      const formattedCustomers = filteredCustomers.map((customer: any, index: number) => ({
        key: customer.id || index,
        accountID: customer.id,
        customerName: customer.name || '',
        phoneNumber: customer.phone || '',
        email: customer.email || '',
        role: customer.roleName || '',
        customerID: customer.id, // Using id as customerID since there's no separate customerID field
        isActive: customer.isActive !== undefined ? customer.isActive : true
      }));
      
      console.log("Filtered customers:", formattedCustomers);
      setCustomers(formattedCustomers);
    } catch (error) {
      console.error("Failed to fetch accounts:", error);
      openNotification("error", "Fetch", "Failed to load customer data");
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleBan = async (email: string) => {
    try {
      const response = await updateAccount(email, {
        Role: "ROLE_BAN",
      });
      console.log("Ban Response:", response.data);
      if (response.status === 200) {
        openNotification("success", "Ban", "Customer has been banned successfully");
        fetchData();
      } else {
        openNotification("error", "Ban", "Failed to ban customer");
      }
    } catch (error: any) {
      console.error("Failed to ban customer:", error);
      openNotification("error", "Ban", error.message || "Failed to ban customer");
    }
  };

  // Fixed columns with proper null checks
  const columns: TableColumnsType<any> = [
    {
      title: "Customer ID",
      dataIndex: "customerID",
      sorter: (a, b) => {
        const aId = a.customerID || '';
        const bId = b.customerID || '';
        return aId.localeCompare(bId);
      },
    },
    {
      title: "Customer Name",
      dataIndex: "customerName",
      defaultSortOrder: "descend",
      sorter: (a, b) => {
        const aName = a.customerName || '';
        const bName = b.customerName || '';
        return aName.localeCompare(bName);
      },
    },
    {
      title: "Email",
      dataIndex: "email",
      sorter: (a, b) => {
        const aEmail = a.email || '';
        const bEmail = b.email || '';
        return aEmail.localeCompare(bEmail);
      },
    },
    {
      title: "Phone",
      dataIndex: "phoneNumber",
      sorter: (a, b) => {
        const aPhone = a.phoneNumber || '';
        const bPhone = b.phoneNumber || '';
        return aPhone.localeCompare(bPhone);
      },
    },
    {
      title: "Ban",
      dataIndex: "ban",
      className: "TextAlign",
      render: (_: unknown, record: any) => (
        <Popconfirm
          title="Sure to ban this customer?"
          onConfirm={() => handleBan(record.email)}
          disabled={!record.isActive} // Disable if already banned
        >
          <a style={{ 
            color: !record.isActive ? '#ccc' : '#1890ff',
            cursor: !record.isActive ? 'not-allowed' : 'pointer'
          }}>
            {record.isActive ? 'Ban' : 'Banned'}
          </a>
        </Popconfirm>
      ),
    },
    {
      title: "Status",
      dataIndex: "isActive",
      render: (isActive: boolean) => (
        <Tag color={isActive ? "green" : "red"}>
          {isActive ? "Active" : "Banned"}
        </Tag>
      ),
    },
  ];

  // SEARCH functionality
  const onSearch = (value: string) => {
    console.log("Search:", value);
    // You can implement actual search logic here
  };

  const handleKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      onSearch(searchText);
    }
  };

  return (
    <>
      {contextHolder}
      <Styled.GlobalStyle />
      <Styled.AdminArea>
        <Sidebar />
        <Styled.AdminPage>
          <Styled.TitlePage>
            <h1>Customer Management</h1>
            <p>View and manage customers</p>
          </Styled.TitlePage>

          <Styled.AdPageContent>
            <Styled.AdPageContent_Head>
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
            </Styled.AdPageContent_Head>

            <Styled.AdminTable>
              <Form form={form} component={false}>
                <Table
                  components={{
                    body: {
                      cell: EditableCell,
                    },
                  }}
                  bordered
                  dataSource={customers}
                  columns={columns}
                  rowClassName="editable-row"
                  pagination={{
                    pageSize: 6,
                    showSizeChanger: true,
                    showQuickJumper: true,
                    showTotal: (total, range) =>
                      `${range[0]}-${range[1]} of ${total} customers`,
                  }}
                  locale={{
                    emptyText: "No customers found"
                  }}
                />
              </Form>
            </Styled.AdminTable>
          </Styled.AdPageContent>
        </Styled.AdminPage>
      </Styled.AdminArea>
    </>
  );
};

export default Customer;