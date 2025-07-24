import styled from "styled-components";
import { Space, Table, TableColumnsType, Tag, TableProps } from "antd";
import AccountCus from "@/components/Customer/Account Details/AccountCus";
import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { showAllOrder } from "@/services/orderAPI";
import useAuth from "@/hooks/useAuth";


const onChange: TableProps<DataType>["onChange"] = (
  pagination,
  filters,
  sorter,
  extra
) => {
  console.log("Table params:", pagination, filters, sorter, extra);
};

interface DataType {
  id: string;
  userId: string;
  totalPrice: number;
  orderDate: string;
  vipApplied: boolean;
  status: number;
  saleStaff: string;
  orderDetails: {
    id: string;
    orderId: string;
    unitPrice: number;
    quantity: number;
  }[];
  delivery: {
    id: string;
    orderId: string;
    dispatchTime: string;
    deliveryTime: string;
    shippingAddress: string;
    status: number;
  };
  payments: {
    id: string;
    orderId: string;
    method: string;
    date: string;
    amount: number;
    status: number;
  }[];
}

const formatPrice = (price: number) => {
  return `$ ${new Intl.NumberFormat("en-US", {
    style: "decimal",
    minimumFractionDigits: 0,
  }).format(price)}`;
};

const formatDateTime = (dateTime: string) => {
  return new Intl.DateTimeFormat("en-US", {
    year: "numeric",
    month: "long",
    day: "2-digit",
  }).format(new Date(dateTime));
};

const getStatusText = (status: number): string => {
  switch (status) {
    case 0:
      return "Pending";
    case 1:
      return "Accepted";
    case 2:
      return "Delivering";
    case 3:
      return "Delivered";
    case 4:
      return "Completed";
    case 5:
      return "Confirmed";
    case 6:
      return "Canceled";
    default:
      return "Unknown";
  }
};

const statusColors: Record<string, string> = {
  Pending: "grey",
  Accepted: "orange",
  Delivering: "purple",
  Delivered: "blue",
  Completed: "#32CD32",
  Confirmed: "cyan",
  Canceled: "volcano",
  Unknown: "green",
};

const fetchAllOrder = async (userId: string) => {
  try {
    const { data } = await showAllOrder(userId);
    console.log("Check API response:", data);
    const orders = Array.isArray(data) ? data : data.data || [];
    // Filter orders with status "Completed" (4) or "Canceled" (6)
    const filteredOrders = orders.filter(
      (order: DataType) => order.status === 4 || order.status === 6
    );
    console.log("Filtered orders:", filteredOrders);
    return filteredOrders;
  } catch (error) {
    console.error("Fetch orders error:", error);
    return [];
  }
};

const History = () => {
  const [orders, setOrders] = useState<DataType[]>([]);
  const navigate = useNavigate();
  const { AccountID } = useAuth();

  useEffect(() => {
    const fetchData = async () => {
      if (AccountID) {
        const ordersData = await fetchAllOrder(AccountID.toString());
        console.log("Orders data:", ordersData);
        setOrders(ordersData);
      }
    };
    fetchData();
  }, [AccountID]);

  const columns: TableColumnsType<DataType> = [
    {
      title: "Order ID",
      dataIndex: "id",
      sorter: (a: DataType, b: DataType) => a.id.localeCompare(b.id),
    },
    {
      title: "Order Date",
      dataIndex: "orderDate",
      render: (text) => formatDateTime(text),
      sorter: (a: DataType, b: DataType) =>
        a.orderDate.localeCompare(b.orderDate),
    },
    {
      title: "Total Price",
      dataIndex: "totalPrice",
      render: (price) => formatPrice(price),
      sorter: (a: DataType, b: DataType) => a.totalPrice - b.totalPrice,
    },
    {
      title: "Status",
      dataIndex: "status",
      render: (_, { status }) => {
        const statusText = getStatusText(status);
        const color = statusColors[statusText] || statusColors.Unknown;
        console.log(`Status: ${status}, Text: ${statusText}, Color: ${color}`);
        return (
          <Tag color={color} key={statusText}>
            {statusText.toUpperCase()}
          </Tag>
        );
      },
      filters: [
        { text: "Completed", value: 4 },
        { text: "Canceled", value: 6 },
      ],
      onFilter: (value, record) => record.status === value,
    },
    {
      title: "Action",
      key: "action",
      render: (_, record) => (
        <Space style={{ width: 134 }} size="middle">
          <a
            onClick={() => navigate(`/order-details?orderId=${record.id}`)}
          >
            View
          </a>
        </Space>
      ),
      width: 134,
    },
  ];

  console.log("Orders state:", orders);
  return (
    <main>
      <AccountCus />
      <Section>
        <Title>History</Title>
        <TableContainer>
          <Table
            columns={columns}
            dataSource={orders}
            pagination={{ pageSize: 6 }}
            onChange={onChange}
            rowKey="id"
          />
        </TableContainer>
      </Section>
    </main>
  );
};

const Section = styled.section`
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-top: 29px;
  background: #fff;
`;

const Title = styled.h1`
  color: #000;
  font: 600 32px "Crimson Text", sans-serif;
  @media (max-width: 991px) {
    margin-top: 40px;
  }
`;

const TableContainer = styled.div`
  width: 100%;
  max-width: 1400px;
  margin-top: 45px;
`;

export default History;