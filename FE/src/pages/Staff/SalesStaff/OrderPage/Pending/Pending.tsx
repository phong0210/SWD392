import * as Styled from "./Pending.styled";
import { useEffect, useState } from "react";
import { Button, Space, Table, Tag, Input, notification } from "antd";
import { SearchOutlined } from "@ant-design/icons";
import type { TableColumnsType, TableProps } from "antd";
import Sidebar from "@/components/Staff/SalesStaff/Sidebar/Sidebar";
import OrderMenu from "@/components/Staff/SalesStaff/OrderMenu/OrderMenu";
import { showAllOrder, updateOrder, orderRelation } from "@/services/orderAPI";
import { OrderStatus } from "@/utils/enum";
import useAuth from "@/hooks/useAuth";

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
      name: string; // Adjust if the field is fullName or different
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
  vipApplied: boolean;
}

const onChange: TableProps<DataType>["onChange"] = (
  pagination,
  filters,
  sorter,
  extra
) => {
  console.log("params", pagination, filters, sorter, extra);
};

const Pending = () => {
  const [searchText, setSearchText] = useState("");
  const [order, setOrder] = useState<DataType[]>([]);
  const [userCache, setUserCache] = useState<Record<string, string>>({}); // Cache for orderId to name mapping
  const [api, contextHolder] = notification.useNotification();
  const { AccountID } = useAuth();

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
      render: (_, { status, orderID }) => {
        let color = status ? "green" : "grey";
        if (status === OrderStatus.PENDING) color = "volcano";
        else if (status === OrderStatus.ACCEPTED) color = "yellow";
        else if (status === OrderStatus.ASSIGNED) color = "orange";
        else if (status === OrderStatus.DELIVERING) color = "blue";
        else if (status === OrderStatus.DELIVERED) color = "purple";
        else if (status === OrderStatus.COMPLETED) color = "green";
        else if (status === OrderStatus.CANCELLED) color = "grey";
        return <Tag color={color} key={orderID}>{status ? status.toUpperCase() : 'UNKNOWN'}</Tag>;
      },
    },
    {
      title: "Action",
      key: "action",
      render: (_, { orderID }) => {
        const safeOrderID = typeof orderID === 'string' ? orderID : String(orderID);
        return (
          <Space size="middle">
            <Button className="confirmBtn" onClick={() => handleAccept(safeOrderID)}>
              Accept
            </Button>
          </Space>
        );
      },
    },
  ];

  const handleAccept = async (orderID: string) => {
    const safeOrderID = typeof orderID === 'string' ? orderID : String(orderID);
    try {
      // Find the order in the current list to get its VipApplied value
      const currentOrder = order.find((o) => o.orderID === safeOrderID);
      const vipApplied = currentOrder ? currentOrder.vipApplied : false;
      const response = await updateOrder(safeOrderID, {
        SaleStaff: AccountID || "",
        VipApplied: vipApplied,
      });
      if (response.status !== 200) throw new Error(response.data?.message || "Update failed");
      api.success({
        message: "Notification",
        description: "The order has been sent to manager successfully!",
      });
      fetchData();
    } catch (error: any) {
      console.error("Error updating order:", error);
      api.error({
        message: "Error",
        description: error.message || "An error occurred!",
      });
    }
  };

  const fetchData = async () => {
    try {
      const orderList = await showAllOrder();
      console.log('Full response:', orderList);
      if (orderList && orderList.data) {
        console.log('Raw order data for Sales Staff:', orderList.data);
        const rawData = Array.isArray(orderList.data) ? orderList.data : orderList.data.data || [];
        const formatOrderList: DataType[] = await Promise.all(
          rawData
            .filter((order: OrderResponseFE) => order.status === 0) // Filter for PENDING (status 0)
            .map(async (order: OrderResponseFE, index: number) => {
              const cusName = await fetchUserName(order.id);
              return {
                orderID: typeof order.id === 'string' ? order.id : String(order.id ?? `no-id-${index}`),
                date: order.orderDate || '',
                cusName,
                total: order.totalPrice || 0, // Ensure totalPrice is mapped correctly
                status: typeof statusMap[order.status] === 'string' ? statusMap[order.status] : 'UNKNOWN',
                deliveryStaff: order.saleStaff || '',
                vipApplied: order.vipApplied,
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
                dataSource={order as DataType[]}
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

export default Pending;