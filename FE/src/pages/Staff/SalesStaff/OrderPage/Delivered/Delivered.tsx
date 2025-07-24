import * as Styled from "./Delivered.styled";
import { useEffect, useState } from "react";
import { Space, Table, Tag, Input } from "antd";
import { EyeOutlined, SearchOutlined } from "@ant-design/icons";
import type { TableColumnsType, TableProps } from "antd";
import { Link } from "react-router-dom";
import Sidebar from "@/components/Staff/SalesStaff/Sidebar/Sidebar";
import OrderMenu from "@/components/Staff/SalesStaff/OrderMenu/OrderMenu";
import { showAllOrder, orderRelation } from "@/services/orderAPI";
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
  accountDeliveryId?: string; // Links to delivery staff
}

interface PaymentResponseFE {
  id: string;
  orderId: string;
  method: string; // Could be paymentMethod or similar
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
      phoneNumber?: string; // Could be phone or contact
      address?: string; // Could be shippingAddress
    };
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
  key: string; // For unique row key
}

const statusMap: { [key: number]: string } = {
  0: OrderStatus.PENDING,
  1: OrderStatus.ACCEPTED,
  2: OrderStatus.DELIVERING,
  3: OrderStatus.DELIVERED,
  4: OrderStatus.COMPLETED,
  6: OrderStatus.CANCELLED,
};

const formatPrice = (price: number | bigint) => {
  return `$ ${new Intl.NumberFormat("en-US", {
    style: "decimal",
    minimumFractionDigits: 0,
  }).format(price)}`;
};

const columns: TableColumnsType<DataType> = [
  {
    title: "Order ID",
    dataIndex: "orderID",
    defaultSortOrder: "descend",
    sorter: (a: DataType, b: DataType) => a.orderID.localeCompare(b.orderID),
    render: (_, { orderID }) => <>{orderID}</>,
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
    render: (_, { total }) => <>{formatPrice(total)}</>,
  },
  {
    title: "Phone Number",
    dataIndex: "phoneNumber",
    showSorterTooltip: { target: "full-header" },
    sorter: (a: DataType, b: DataType) => a.phoneNumber.length - b.phoneNumber.length,
    sortDirections: ["descend"],
    render: (_, { phoneNumber }) => <>{phoneNumber || "N/A"}</>,
  },
  {
    title: "Address",
    dataIndex: "address",
    showSorterTooltip: { target: "full-header" },
    sorter: (a: DataType, b: DataType) => a.address.length - b.address.length,
    sortDirections: ["descend"],
    render: (_, { address }) => <>{address || "N/A"}</>,
  },
  {
    title: "Payment Method",
    dataIndex: "paymentMethod",
    showSorterTooltip: { target: "full-header" },
    sorter: (a: DataType, b: DataType) => a.paymentMethod.length - b.paymentMethod.length,
    sortDirections: ["descend"],
    render: (_, { paymentMethod }) => <>{paymentMethod || "N/A"}</>,
  },
  {
    title: "Status",
    key: "status",
    dataIndex: "status",
    render: (_, { status, orderID }) => {
      let color = "green";
      if (status === "Pending") color = "volcano";
      else if (status === "Accepted") color = "yellow";
      else if (status === "Assigned") color = "orange";
      else if (status === "Delivering") color = "blue";
      else if (status === "Delivered") color = "purple";
      else if (status === "Completed") color = "green";
      else if (status === "Cancelled") color = "grey";
      return (
        <Tag color={color} key={orderID}>
          {status.toUpperCase()}
        </Tag>
      );
    },
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

const DeliveredOrder = () => {
  const [searchText, setSearchText] = useState("");
  const [order, setOrder] = useState<DataType[]>([]);
  const [userCache, setUserCache] = useState<Record<string, { name: string; phoneNumber: string; address: string }>>({}); // Cache for orderId to user info mapping

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
      console.log("User data response (detailed):", userData); // More detailed logging
      const user = userData.data?.user || {};
      const name = user.name || user.fullName || "Unknown";
      const phoneNumber = user.phoneNumber || user.phone || user.contact || "N/A"; // Try alternative field names
      const address = user.address || user.shippingAddress || "N/A"; // Try alternative field names
      const userInfo = { name, phoneNumber, address };
      setUserCache((prev) => ({ ...prev, [orderId]: userInfo }));
      return userInfo;
    } catch (error) {
      console.error("Error fetching user name for orderId", orderId, ":", error);
      return { name: "Unknown", phoneNumber: "N/A", address: "N/A" };
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
            .filter((order: OrderResponseFE) => order.status === 3) // Filter for DELIVERED (status 3)
            .map(async (order: OrderResponseFE, index: number) => {
              const { name: cusName, phoneNumber, address } = await fetchUserName(order.id);
              console.log('Order data for mapping:', order); // Log each order to debug payments
              return {
                orderID: String(order.id || `no-id-${index}`),
                date: order.orderDate || '',
                cusName,
                total: order.totalPrice || 0,
                phoneNumber,
                address,
                paymentMethod: order.payments && order.payments.length > 0 ? (order.payments[0].method || order.payments[0].paymentMethod || "N/A") : "N/A", // Try alternative field names
                status: statusMap[order.status] || 'UNKNOWN',
                key: order.id || `no-id-${index}`, // Unique key for Table rows
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
                rowKey="key" // Specify rowKey to avoid unique key warning
                pagination={{ pageSize: 6 }}
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

export default DeliveredOrder;