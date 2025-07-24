import * as Styled from "./DeliveryCompletedstyled";
import { useState, useEffect } from "react";
import { Button, notification, Space, Table, Tag } from "antd";
import { PoweroffOutlined, SearchOutlined } from "@ant-design/icons";
import type { TableColumnsType, TableProps } from "antd";
import OrderMenu from "@/components/Staff/Deli/OrderDeli/OrderMenu";
import useAuth from "@/hooks/useAuth";
import { getCustomer } from "@/services/accountApi";
import config from "@/config";
import cookieUtils from "@/services/cookieUtils";
import { showAllOrder, updateOrder } from "@/services/orderAPI";
import { OrderStatus } from "@/utils/enum";

interface DataType {
  key: React.Key;
  no: string;
  orderID: string;
  cusName: string;
  phoneNumber: string;
  product: string;
  address: string;
  status: string;
}

const columns: TableColumnsType<DataType> = [
  {
    title: "Order ID",
    dataIndex: "orderID",
    // defaultSortOrder: "descend",
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    sorter: (a: DataType, b: DataType) => a.orderID.localeCompare(b.orderID),
  },
  {
    title: " Receiver",
    dataIndex: "receiver",
    showSorterTooltip: { target: "full-header" },
    filters: [
      { text: "Joe", value: "Joe" },
      { text: "Jim", value: "Jim" },
      { text: "Esther", value: "Esther" },
      { text: "Ajmal", value: "Ajmal" },
    ],
    onFilter: (value, record) => record.cusName.indexOf(value as string) === 0,
    sorter: (a, b) => a.cusName.length - b.cusName.length,
    sortDirections: ["descend"],
  },
  {
    title: "Phone Number",
    dataIndex: "phoneNumber",
  },
  {
    title: "Address",
    dataIndex: "address",
    showSorterTooltip: { target: "full-header" },
    sorter: (a, b) => a.address.length - b.address.length,
    sortDirections: ["descend"],
  },
  {
    title: "Status",
    key: "status",
    dataIndex: "status",
    render: (_, { status }) => {
      let color = "green";
      if (status === "Pending") {
        color = "volcano";
      } else if (status === "Accepted") {
        color = "yellow";
      } else if (status === "Assigned") {
        color = "orange";
      } else if (status === "Delivering") {
        color = "blue";
      } else if (status === "Delivered") {
        color = "purple";
      } else if (status === "Completed") {
        color = "green";
      } else if (status === "Cancelled") {
        color = "grey";
      }

      return (
        <Tag color={color} key={status}>
          {status.toUpperCase()}
        </Tag>
      );
    },
  },
];

const DeliveryCompleted = () => {
  const { AccountID } = useAuth();

  const [user, setUser] = useState<any>(null);

  const [orderList, setOrderList] = useState<any[]>([]);

  const [searchText, setSearchText] = useState("");

  const [api, contextHolder] = notification.useNotification();

  // Function to convert numeric status to string
  const getStatusString = (statusNumber: number) => {
    const statusMap: { [key: number]: string } = {
      0: "Pending",
      1: "Confirmed",
      2: "Processing",
      3: "Assigned",
      4: "Delivering",
      5: "Delivered",
      6: "Completed",
      7: "Cancelled",
    };
    return statusMap[statusNumber] || "Unknown";
  };

  const fetchData = async () => {
    try {
      const orderRes = await showAllOrder();
      console.log(orderRes.data);

      // Filter for orders with status 0 (Pending) and format the data
      const orderFormatted = orderRes.data
        .filter((order: any) => order.status >= 4) // Filter for status 0
        .map((order: any) => ({
          key: order.id,
          orderID: order.id,
          receiver: order.delivery?.shippingAddress?.split(",")[0] || "N/A", // Extract name from address or use N/A
          phoneNumber: "N/A", // Not available in new API structure
          address: order.delivery?.shippingAddress || "N/A",
          status: getStatusString(order.status),
          totalPrice: order.totalPrice,
          orderDate: order.orderDate,
          saleStaff: order.saleStaff,
        }));

      setOrderList(orderFormatted);
    } catch (error) {
      console.error("Error fetching data:", error);
      api.error({
        message: "Error",
        description: "Failed to fetch orders",
      });
    }
  };

  useEffect(() => {
    fetchData();

    if (searchText.trim() === "") {
      //nếu search k có value sẽ giá trị bảng ban đầu
      // setFilteredData(initialData);
      return;
    }
  }, [searchText]);

  // Filter orders based on search text
  const filteredOrderList = orderList.filter(
    (order) =>
      order.receiver.toLowerCase().includes(searchText.toLowerCase()) ||
      order.orderID.toLowerCase().includes(searchText.toLowerCase()) ||
      order.address.toLowerCase().includes(searchText.toLowerCase())
  );

  const columns: TableColumnsType<any> = [
    {
      title: "Order ID",
      dataIndex: "orderID",
      sorter: (a: any, b: any) => a.orderID.localeCompare(b.orderID),
      render: (text: string) => text.substring(0, 8) + "...", // Show only first 8 characters
    },
    {
      title: "Customer",
      dataIndex: "receiver",
      showSorterTooltip: { target: "full-header" },
      sorter: (a, b) => a.receiver.length - b.receiver.length,
      sortDirections: ["descend"],
    },
    {
      title: "Phone Number",
      dataIndex: "phoneNumber",
    },
    {
      title: "Address",
      dataIndex: "address",
      showSorterTooltip: { target: "full-header" },
      sorter: (a, b) => a.address.length - b.address.length,
      sortDirections: ["descend"],
    },
    {
      title: "Total Price",
      dataIndex: "totalPrice",
      render: (price: number) => `$${price.toLocaleString()}`,
      sorter: (a, b) => a.totalPrice - b.totalPrice,
    },
    {
      title: "Sale Staff",
      dataIndex: "saleStaff",
    },
    {
      title: "Status",
      key: "status",
      dataIndex: "status",
      render: (_, { status }) => {
        let color = "green";
        if (status === "Pending") {
          color = "volcano";
        } else if (status === "Confirmed") {
          color = "yellow";
        } else if (status === "Processing") {
          color = "blue";
        } else if (status === "Assigned") {
          color = "orange";
        } else if (status === "Delivering") {
          color = "cyan";
        } else if (status === "Delivered") {
          color = "purple";
        } else if (status === "Completed") {
          color = "green";
        } else if (status === "Cancelled") {
          color = "grey";
        }
        return (
          <Tag color={color} key={status}>
            {status?.toUpperCase()}
          </Tag>
        );
      },
    },
  ];

  const onChange: TableProps<any>["onChange"] = (
    pagination,
    filters,
    sorter,
    extra
  ) => {
    console.log("params", pagination, filters, sorter, extra);
  };

  const handleKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      setSearchText((e.target as HTMLInputElement).value);
    }
  };

  return (
    <>
      {contextHolder}
      <Styled.OrderAdminArea>
        <Styled.AdminPage>
          <Styled.HeaderContainer>
            <Styled.TitlePage>
              <h1>Delivery - Pending Orders</h1>
              <p>View and manage pending delivery orders</p>
            </Styled.TitlePage>
            <Styled.DeliveryStaff>
              <h1>Hello, {user ? user.Name : null}</h1>
              <Styled.Logout
                to={config.routes.public.login}
                onClick={() => cookieUtils.clear()}
              >
                <PoweroffOutlined /> Logout
              </Styled.Logout>
            </Styled.DeliveryStaff>
          </Styled.HeaderContainer>
          <OrderMenu />
          <Styled.OrderContent>
            <Styled.OrderContent_Head>
              <Styled.SearchArea>
                <SearchOutlined className="searchIcon" />
                <input
                  className="searchInput"
                  type="text"
                  placeholder="Search customer, order ID, or address..."
                  value={searchText}
                  onChange={(e) => setSearchText(e.target.value)}
                  onKeyPress={handleKeyPress}
                />
              </Styled.SearchArea>
            </Styled.OrderContent_Head>

            <Styled.Pending_Table>
              <Table
                className="table"
                columns={columns}
                dataSource={filteredOrderList}
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

export default DeliveryCompleted;
