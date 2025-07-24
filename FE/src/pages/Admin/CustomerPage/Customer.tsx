
import * as Styled from "./Customer.styled";
import React, { useEffect, useState } from "react";
import { SearchOutlined } from "@ant-design/icons";
import type { TableColumnsType } from "antd";
import Sidebar from "../../../components/Admin/Sidebar/Sidebar";
import {
  Form,
  Input,
  InputNumber,
  notification,
  Popconfirm,
  Table,
  Tag,
} from "antd";
import { showAllAccounts } from "@/services/authAPI";
import { deleteUser } from "@/services/accountApi";

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
          rules={[{ required: true, message: `Please Input ${title}!` }]}
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
  const [filteredCustomers, setFilteredCustomers] = useState<any[]>([]);
  const [api, contextHolder] = notification.useNotification();

  type NotificationType = "success" | "info" | "warning" | "error";

  const openNotification = (
    type: NotificationType,
    method: string,
    error: string
  ) => {
    api[type]({
      message: type === "success" ? "Notification" : "Error",
      description: type === "success" ? `${method} successfully` : error,
    });
  };

  const fetchData = async () => {
    try {
      const response = await showAllAccounts();
      const data = response.data?.data || response.data;

      if (!Array.isArray(data)) {
        openNotification("error", "Fetch", "Unexpected data format");
        return;
      }

      const filtered = data.filter((account: any) =>
        account.roleName?.toLowerCase().includes("customer")
      );

      const formatted = filtered.map((customer: any, index: number) => ({
        key: customer.id || index,
        accountID: customer.id,
        customerName: customer.name || "",
        phoneNumber: customer.phone || "",
        email: customer.email || "",
        role: customer.roleName || "",
        customerID: customer.id,
        isActive:
          customer.isActive !== undefined ? customer.isActive : true,
      }));

      setCustomers(formatted);
      setFilteredCustomers(formatted);
    } catch {
      openNotification("error", "Fetch", "Failed to load customer data");
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleBan = async (accountID: string) => {
    try {
      const response = await deleteUser(accountID);
      if (response.status === 200 || response.status === 204) {
        openNotification("success", "Ban", "Customer has been banned successfully");
        fetchData();
      } else {
        openNotification("error", "Ban", "Failed to ban customer");
      }
    } catch (error: any) {
      openNotification("error", "Ban", error.message || "Failed to ban customer");
    }
  };

  const onSearch = (value: string) => {
    const keyword = value.toLowerCase().trim();

    const filtered = customers.filter((customer) =>
      customer.customerName.toLowerCase().includes(keyword)
    );

    setFilteredCustomers(filtered);
  };

  const handleKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      onSearch(searchText);
    }
  };

  const columns: TableColumnsType<any> = [
    {
      title: "Customer ID",
      dataIndex: "customerID",
      sorter: (a, b) =>
        (a.customerID || "").localeCompare(b.customerID || ""),
    },
    {
      title: "Customer Name",
      dataIndex: "customerName",
      defaultSortOrder: "descend",
      sorter: (a, b) =>
        (a.customerName || "").localeCompare(b.customerName || ""),
    },
    {
      title: "Email",
      dataIndex: "email",
      sorter: (a, b) => (a.email || "").localeCompare(b.email || ""),
    },
    {
      title: "Phone",
      dataIndex: "phoneNumber",
      sorter: (a, b) =>
        (a.phoneNumber || "").localeCompare(b.phoneNumber || ""),
    },
    {
      title: "Ban",
      dataIndex: "ban",
      className: "TextAlign",
      render: (_: unknown, record: any) => (
        <Popconfirm
          title="Sure to ban this customer?"
          onConfirm={() => handleBan(record.accountID)}
          disabled={!record.isActive}
        >
          <a
            style={{
              color: !record.isActive ? "#ccc" : "#1890ff",
              cursor: !record.isActive ? "not-allowed" : "pointer",
            }}
          >
            {record.isActive ? "Ban" : "Banned"}
          </a>
        </Popconfirm>
      ),
    },
    {
      title: "Status",
      dataIndex: "isActive",
      filters: [
        { text: "Active", value: true },
        { text: "Banned", value: false },
      ],
      onFilter: (value, record) => record.isActive === value,
      render: (isActive: boolean) => (
        <Tag color={isActive ? "green" : "red"}>
          {isActive ? "Active" : "Banned"}
        </Tag>
      ),
    },
  ];

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
                  placeholder="Search customer name..."
                  value={searchText}
                  onChange={(e) => {
                    const value = e.target.value;
                    setSearchText(value);
                    onSearch(value); // ✅ Auto search on typing
                  }}
                  onKeyDown={handleKeyPress}
                  prefix={<SearchOutlined className="searchIcon" />}
                />
              </Styled.SearchArea>
            </Styled.AdPageContent_Head>

            <Styled.AdminTable>
              <Form form={form} component={false}>
                <Table
                  components={{ body: { cell: EditableCell } }}
                  bordered
                  dataSource={filteredCustomers}
                  columns={columns}
                  rowClassName="editable-row"
                  pagination={{
                    pageSize: 6,
                    showSizeChanger: true,
                    showQuickJumper: true,
                    showTotal: (total, range) =>
                      `${range[0]}-${range[1]} of ${total} customers`,
                  }}
                  locale={{ emptyText: "No customers found" }}
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

