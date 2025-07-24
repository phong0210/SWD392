import * as Styled from "./Cancelled.styled";
import { useEffect, useState } from "react";
import { Space, Table, Tag, Input } from "antd";
import { SearchOutlined, EyeOutlined } from "@ant-design/icons";
import type { TableColumnsType, TableProps } from "antd";
import { Link } from "react-router-dom";
import Sidebar from "@/components/Staff/SalesStaff/Sidebar/Sidebar";
import OrderMenu from "@/components/Staff/SalesStaff/OrderMenu/OrderMenu";
import { OrderStatus } from "@/utils/enum";
import { showAllOrder, orderRelation } from "@/services/orderAPI";

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
      name: string; // Assuming name is under user; adjust if different (e.g., fullName)
    };
    error: string | null;
  };
}

interface DataType {
  orderID: string;
  date: string;
  cusName: string;
  total: number;
  status: string;
  deliveryStaff?: string;
}

const statusMap: { [key: number]: string } = {
  0: OrderStatus.PENDING,
  1: OrderStatus.ACCEPTED,
  2: OrderStatus.DELIVERING,
  3: OrderStatus.DELIVERED,
  4: OrderStatus.COMPLETED,
  6: OrderStatus.CANCELLED,
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
    title: "Total",
    dataIndex: "total",
    defaultSortOrder: "descend",
    sorter: (a: DataType, b: DataType) => a.total - b.total,
    render: (_, { total }) => <>${total || 0}</>,
  },
  {
    title: "Status",
    key: "status",
    dataIndex: "status",
    render: (_, { status }) => {
      let color = status ? "green" : "grey";
      if (status === OrderStatus.PENDING) color = "volcano";
      else if (status === OrderStatus.ACCEPTED) color = "yellow";
      else if (status === OrderStatus.ASSIGNED) color = "orange";
      else if (status === OrderStatus.DELIVERING) color = "blue";
      else if (status === OrderStatus.DELIVERED) color = "purple";
      else if (status === OrderStatus.COMPLETED) color = "green";
      else if (status === OrderStatus.CANCELLED) color = "grey";
      return <Tag color={color} key={status}>{status ? status.toUpperCase() : 'UNKNOWN'}</Tag>;
    },
    filters: [
      { text: "Pending", value: "Pending" },
      { text: "Accepted", value: "Accepted" },
      { text: "Assigned", value: "Assigned" },
      { text: "Delivering", value: "Delivering" },
      { text: "Delivered", value: "Delivered" },
      { text: "Completed", value: "Completed" },
      { text: "Cancelled", value: "Cancelled" },
    ],
    onFilter: (value, record) => record.status.indexOf(value as string) === 0,
  },
  
];

const onChange: TableProps<DataType>["onChange"] = (
  pagination,
  filters,
  sorter,
  extra
) => {
  console.log("params", pagination, filters, sorter, extra);
};

const CancelledOrder = () => {
  const [searchText, setSearchText] = useState("");
  const [order, setOrder] = useState<DataType[]>([]);
  const [userCache, setUserCache] = useState<Record<string, string>>({}); // Cache for orderId to name mapping

  const onSearch = (value: string) => {
    console.log("Search:", value);
  };

  const handleKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      onSearch(searchText);
    }
  };

  const fetchUserName = async (orderId: string): Promise<string> => {
    if (userCache[orderId]) return userCache[orderId];
    try {
      const userData = await orderRelation(orderId);
      console.log("User data response:", userData); // Debug the full response
      const name = userData.data?.user?.name || userData.data?.user?.fullName || "Unknown"; // Extract name from user object
      setUserCache((prev) => ({ ...prev, [orderId]: name }));
      return name;
    } catch (error) {
      console.error("Error fetching user name for orderId", orderId, ":", error);
      return "Unknown";
    }
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        const orderList = await showAllOrder();
        console.log('Full response:', orderList);
        if (orderList && orderList.data) {
          console.log('Raw order data for Sales Staff:', orderList.data);
          const formatOrderList = await Promise.all(
            orderList.data.map(async (order: OrderResponseFE, index: number) => {
              const cusName = await fetchUserName(order.id);
              return {
                orderID: order.id || `no-id-${index}`,
                date: order.orderDate || '',
                cusName,
                total: order.totalPrice || 0,
                status: statusMap[order.status] || 'UNKNOWN',
                deliveryStaff: order.saleStaff || '',
              };
            })
          );
          // Filter for CANCELLED status (status 6)
          const filteredOrderList = formatOrderList.filter((order) => order.status === OrderStatus.CANCELLED);
          setOrder(filteredOrderList);
          console.log('Formatted order list:', filteredOrderList);
        } else {
          console.error('No data in response:', orderList);
        }
      } catch (error) {
        console.error('Error fetching order data:', error);
      }
    };

    fetchData();
  }, []);

  return (
    <>
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
            <Styled.Pending_Table>
              <Table
                className="table"
                columns={columns}
                dataSource={order}
                rowKey="orderID"
                pagination={{ pageSize: 6 }}
                onChange={onChange}
                showSorterTooltip={{ target: "sorter-icon" }}
              />
            </Styled.Pending_Table>
          </Styled.OrderContent>
        </Styled.AdminPage>
      </Styled.OrderAdminArea>
    </>
  );
};

export default CancelledOrder;