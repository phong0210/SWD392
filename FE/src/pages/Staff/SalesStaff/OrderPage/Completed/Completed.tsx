import * as Styled from "./Completed.styled";
import { useEffect, useState } from "react";
import { Button, Space, Table, Tag, Input, notification } from "antd";
import { SearchOutlined, EyeOutlined } from "@ant-design/icons";
import type { TableColumnsType, TableProps } from "antd";
import { Link } from "react-router-dom";
import Sidebar from "@/components/Staff/SalesStaff/Sidebar/Sidebar";
import OrderMenu from "@/components/Staff/SalesStaff/OrderMenu/OrderMenu";
import { showAllOrder, orderRelation, updateOrder } from "@/services/orderAPI";
import { showAllAccounts } from "@/services/authAPI";
import { OrderStatus } from "@/utils/enum";

// Updated interfaces
interface OrderResponseFE {
  id: string;
  userId: string;
  totalPrice: number;
  orderDate: string;
  vipApplied: boolean;
  status: number;
  saleStaff: string;
  orderDetails: OrderDetailResponseFE[];
  delivery?: DeliveryResponseFE;
  payments: PaymentResponseFE[];
}

interface OrderDetailResponseFE {
  id: string;
  orderId: string;
  unitPrice: number;
  quantity: number;
}

interface DeliveryResponseFE {
  id: string;
  orderId: string;
  dispatchTime?: string;
  deliveryTime?: string;
  shippingAddress: string;
  status: number;
  accountDeliveryId?: string; // Assuming this links to delivery staff
}

interface PaymentResponseFE {
  id: string;
  orderId: string;
  method: string;
  date: string;
  amount: number;
  status: number;
}

interface UserResponseFE {
  data: {
    success: boolean;
    user: {
      id: string;
      name: string; // Adjust if the field is fullName or different
      phoneNumber?: string;
      address?: string;
    };
    error: string | null;
  };
}

interface AccountResponseFE {
  data: {
    success: boolean;
    data: Array<{
      id: string;
      name: string; // Adjust based on actual field (e.g., Name)
    }>;
    error: string | null;
  };
}

interface DataType {
  orderID: string;
  date: string;
  cusName: string;
  total: number;
  phoneNumber: string;
  address: string;
  paymentMethod: string;
  status: string;
  deliveryStaff?: string;
  key: string;
}

const onChange: TableProps<DataType>["onChange"] = (
  pagination,
  filters,
  sorter,
  extra
) => {
  console.log("params", pagination, filters, sorter, extra);
};

const CompletedOrder = () => {
  const [api, contextHolder] = notification.useNotification();
  const statusMap: { [key: number]: string } = {
  0: OrderStatus.PENDING,
  1: OrderStatus.ACCEPTED,
  2: OrderStatus.DELIVERING,
  3: OrderStatus.DELIVERED,
  4: OrderStatus.COMPLETED,
  6: OrderStatus.CANCELLED,
};

  const handleCancel = async (orderID: string) => {
    try {
      const response = await updateOrder(orderID, {
        status: 6, // Set status to CANCELLED
      });
      if (response.status === 200) {
        api.success({
          message: "Order Cancelled",
          description: `Order ${orderID} has been successfully cancelled.`,
        });
        fetchData(); // Refresh data after cancellation
      } else {
        throw new Error(response.data?.message || "Failed to cancel order.");
      }
    } catch (error: any) {
      console.error("Error cancelling order:", error);
      api.error({
        message: "Error",
        description: error.message || "An error occurred while cancelling the order.",
      });
    }
  };
  const columns: TableColumnsType<DataType> = [
    {
      title: "Order ID",
      dataIndex: "orderID",
      defaultSortOrder: "descend",
      sorter: (a: DataType, b: DataType) => a.orderID.localeCompare(b.orderID),
    },
    {
      title: "Date",
      dataIndex: "date",
      defaultSortOrder: "descend",
      sorter: (a: DataType, b: DataType) => {
        const dateA = a.date || '';
        const dateB = b.date || '';
        return dateA.localeCompare(dateB);
      },
      render: (_, { date }) => <>{date ? date.replace("T", " ").replace(".000Z", " ") : ''}</>,
    },
    {
      title: "Customer",
      dataIndex: "cusName",
      showSorterTooltip: { target: "full-header" },
      sorter: (a: DataType, b: DataType) => a.cusName.length - b.cusName.length,
      sortDirections: ["descend"],
    },
    {
      title: "Delivery Staff",
      dataIndex: "deliveryStaff",
      showSorterTooltip: { target: "full-header" },
      filters: [
        { text: "Joe", value: "Joe" },
        { text: "Jim", value: "Jim" },
        { text: "Esther", value: "Esther" },
        { text: "Ajmal", value: "Ajmal" },
      ],
      onFilter: (value, record) =>
        record.deliveryStaff?.indexOf(value as string) === 0 || false,
      sorter: (a: DataType, b: DataType) => (a.deliveryStaff?.length || 0) - (b.deliveryStaff?.length || 0),
      sortDirections: ["descend"],
      render: (_, { deliveryStaff }) => <>{deliveryStaff || "N/A"}</>,
    },
    {
      title: "Status",
      key: "status",
      dataIndex: "status",
      render: (_, { status, orderID }) => {
        let color = "green";
        if (status === OrderStatus.PENDING) color = "volcano";
        else if (status === OrderStatus.ACCEPTED) color = "yellow";
        else if (status === OrderStatus.ASSIGNED) color = "orange";
        else if (status === OrderStatus.DELIVERING) color = "blue";
        else if (status === OrderStatus.DELIVERED) color = "purple";
        else if (status === OrderStatus.COMPLETED) color = "green";
        else if (status === OrderStatus.CANCELLED) color = "grey";
        return (
          <Tag color={color} key={orderID}>
            {status.toUpperCase()}
          </Tag>
        );
      },
    },
    
    {
      title: "Action",
      key: "action",
      render: (_, { orderID }) => (
        <Space size="middle">
          <Button
            type="primary"
            danger
            onClick={() => handleCancel(orderID)}
          >
            Cancel
          </Button>
        </Space>
      ),
    },
  ];
  const [searchText, setSearchText] = useState("");
  const [order, setOrder] = useState<DataType[]>([]);
  const [userCache, setUserCache] = useState<Record<string, { name: string; phoneNumber: string; address: string }>>({}); // Cache for orderId to user info mapping
  const [accountCache, setAccountCache] = useState<Record<string, string>>({}); // Cache for accountId to name mapping

  const onSearch = (value: string) => {
    console.log("Search:", value);
  };

  const handleKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      onSearch(searchText);
    }
  };

  const fetchUserName = async (orderId: string): Promise<{ name: string; phoneNumber: string; address: string }> => {
    if (userCache[orderId]) return userCache[orderId];
    try {
      const userData = await orderRelation(orderId);
      console.log("User data response:", userData);
      const name = userData.data?.user?.name || userData.data?.user?.fullName || "Unknown";
      const phoneNumber = userData.data?.user?.phoneNumber || "N/A";
      const address = userData.data?.user?.address || "N/A";
      const userInfo = { name, phoneNumber, address };
      setUserCache((prev) => ({ ...prev, [orderId]: userInfo }));
      return userInfo;
    } catch (error) {
      console.error("Error fetching user name for orderId", orderId, ":", error);
      return { name: "Unknown", phoneNumber: "N/A", address: "N/A" };
    }
  };

  const fetchDeliveryStaffName = async (accountId: string): Promise<string> => {
    if (accountCache[accountId]) return accountCache[accountId];
    try {
      const accountList = await showAllAccounts();
      console.log("Account data response:", accountList);
      const account = accountList.data?.data?.find((acc: any) => acc.id === accountId); // Adjusted to match nested data structure
      const name = account?.name || "N/A";
      setAccountCache((prev) => ({ ...prev, [accountId]: name }));
      return name;
    } catch (error) {
      console.error("Error fetching delivery staff name for accountId", accountId, ":", error);
      return "N/A";
    }
  };

  const fetchData = async () => {
    try {
      const orderList = await showAllOrder();
      console.log('Full response:', orderList);
      if (orderList && orderList.data) {
        console.log('Raw order data for Sales Staff:', orderList.data);
        const rawData = Array.isArray(orderList.data) ? orderList.data : orderList.data.data || [];
        const formatOrderList = await Promise.all(
          rawData
            .filter((order: OrderResponseFE) => order.status === 4) // Filter for COMPLETED (status 4)
            .map(async (order: OrderResponseFE, index: number) => {
              const { name: cusName, phoneNumber, address } = await fetchUserName(order.id);
              const deliveryStaff = order.delivery?.accountDeliveryId
                ? await fetchDeliveryStaffName(order.delivery.accountDeliveryId)
                : "N/A";
              return {
                orderID: String(order.id || `no-id-${index}`),
                date: order.orderDate || '',
                cusName,
                total: order.totalPrice || 0,
                phoneNumber,
                address,
                paymentMethod: order.payments[0]?.method || "N/A",
                status: statusMap[order.status] || 'UNKNOWN',
                deliveryStaff,
              };
            })
        );
        setOrder(formatOrderList);
        console.log('Formatted order list:', formatOrderList);
      } else {
        console.error('No data in response:', orderList);
      }
    } catch (error) {
      console.error('Error fetching order data:', error);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  return (
    <>
      {contextHolder}
      <Styled.GlobalStyle />
      <Styled.OrderAdminArea>
        <Sidebar />
        <Styled.AdminPage>
          <OrderMenu />
          <Styled.OrderContent>
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
              <Table
                className="table"
                columns={columns}
                dataSource={order}
                pagination={{ pageSize: 7 }}
                onChange={onChange}
                showSorterTooltip={{ target: "sorter-icon" }}
              />
            </Styled.AdminTable>
          </Styled.OrderContent>
        </Styled.AdminPage>
      </Styled.OrderAdminArea>
    </>
  );
};

export default CompletedOrder;