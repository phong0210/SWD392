import * as Styled from "./DeliveryPendingstyled";
import { useState, useEffect, useCallback } from "react";
import { Button, notification, Space, Table, Tag } from "antd";
import { PoweroffOutlined, SearchOutlined } from "@ant-design/icons";
import type { TableColumnsType, TableProps } from "antd";
import OrderMenu from "@/components/Staff/Deli/OrderDeli/OrderMenu";
import useAuth from "@/hooks/useAuth";
import config from "@/config";
import cookieUtils from "@/services/cookieUtils";
import { showAllOrder, updateOrder } from "@/services/orderAPI";
import { OrderStatus } from "@/utils/enum";
import { useNavigate } from "react-router-dom";
import { useDispatch } from "react-redux";
import { clearCart } from "@/services/cartAPI";
import { logout } from "@/store/slices/authSlice";

const DeliveryPending = () => {
  const { AccountID } = useAuth();

  const [user, setUser] = useState<any>(null);
  const [orderList, setOrderList] = useState<any[]>([]);
  const [searchText, setSearchText] = useState("");
  const [loading, setLoading] = useState(false);

  const [api, contextHolder] = notification.useNotification();

  // Function to convert numeric status to string
  const getStatusString = (statusNumber: number) => {
    const statusMap: { [key: number]: string } = {
      0: "Pending",
      1: "ACCEPTED",
      2: "Processing",
      3: "Assigned",
      4: "Delivering",
      5: "Delivered",
      6: "Completed",
      7: "Cancelled",
    };
    return statusMap[statusNumber] || "Unknown";
  };

   const navigate = useNavigate();
  
    const dispatch = useDispatch();
        const handleLogout = () => {
        clearCart();
        dispatch(logout());
        cookieUtils.clear();
        navigate(config.routes.public.login);
        };
  


  // Sử dụng useCallback để tránh re-render không cần thiết
  const fetchData = useCallback(async () => {
    try {
      setLoading(true);
      const orderRes = await showAllOrder();
      console.log(orderRes.data);

      // In ra toàn bộ userId
      orderRes.data.forEach((order: any, index: number) => {
        console.log(`Order ${index + 1} - userId: ${order.userId}`);
      });

      // Filter for orders with status 1 (Confirmed) and format the data
      const orderFormatted = orderRes.data
        .filter((order: any) => order.status === 1)
        .map((order: any) => {
          console.log(`Mapping order ${order.id} - userId: ${order.userId}`); // ➕ Thêm log này

          return {
            key: order.id,
            orderID: order.id,
            receiver: order.userId, // ➕ Gán userId vào receiver
            address: order.delivery?.shippingAddress || "N/A",
            status: getStatusString(order.status),
            totalPrice: order.totalPrice,
            orderDate: order.orderDate,
            saleStaff: order.saleStaff,
          };
        });

      setOrderList(orderFormatted);
    } catch (error) {
      console.error("Error fetching data:", error);
      api.error({
        message: "Error",
        description: "Failed to fetch orders",
      });
    } finally {
      setLoading(false);
    }
  }, [api]);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  // Optimized search effect
  useEffect(() => {
    // Không cần làm gì đặc biệt ở đây vì filtering được handle ở filteredOrderList
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
      render: (text: string) => text.substring(0, 8) + "...",
    },
    {
      title: "Customer",
      dataIndex: "receiver",
      showSorterTooltip: { target: "full-header" },
      sorter: (a, b) => a.receiver.length - b.receiver.length,
      sortDirections: ["descend"],
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
    {
      title: "Action",
      key: "action",
      dataIndex: "orderID",
      render: (_, { orderID }) => {
        const handleStartDelivery = async (e: React.MouseEvent) => {
          // Ngăn chặn event bubbling và default behavior
          e.preventDefault();
          e.stopPropagation();

          try {
            setLoading(true);

            // Update the order status
            const response = await updateOrder(orderID);
            console.log(response.data);
            // Kiểm tra response structure
            if (!response?.data?.success) {
              throw new Error(response?.data?.error || "Update failed");
            }

            api.success({
              message: "Notification",
              description: "Started delivery successfully",
            });

            // Refresh data sau khi update thành công
            await fetchData();
          } catch (error: any) {
            console.error("Start delivery error:", error);
            api.error({
              message: "Error",
              description:
                error.message || "An error occurred while starting delivery",
            });
          } finally {
            setLoading(false);
          }
        };

        return (
          <Space size="middle">
            <Button
              type="primary"
              className="confirmBtn"
              onClick={handleStartDelivery}
              loading={loading}
              disabled={loading}
            >
              Start Delivery
            </Button>
          </Space>
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

  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchText(e.target.value);
  };

  return (
    <>
      {contextHolder}
      <Styled.OrderAdminArea>
        <Styled.AdminPage>
          <Styled.HeaderContainer>
            <Styled.TitlePage>
              <h1>Delivery </h1>
              <p>View and manage pending delivery orders</p>
            </Styled.TitlePage>
            <Styled.DeliveryStaff>
              <h1>Hello, {user ? user.Name : null}</h1>
              <Styled.Logout
                to={config.routes.public.login}
                onClick={() => handleLogout()}
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
                  onChange={handleSearchChange}
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
                loading={loading}
              />
            </Styled.Pending_Table>
          </Styled.OrderContent>
        </Styled.AdminPage>
      </Styled.OrderAdminArea>
    </>
  );
};

export default DeliveryPending;
