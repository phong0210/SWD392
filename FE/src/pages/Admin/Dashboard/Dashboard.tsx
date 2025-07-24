import * as Styled from "./Dashboard.styled";
import Sidebar from "../../../components/Admin/Sidebar/Sidebar";
import { ArrowRightOutlined } from "@ant-design/icons";
import { useEffect, useState, Component } from "react";
import StatistiBox from "./StatistiBox";
import LineChart from "./LineChart";
import { Link } from "react-router-dom";
import { showAllProduct } from "@/services/productAPI";
import { showAllOrder, showReveneSummary, showWeeklyRevenueSummary, showDailyRevenueSummary } from "@/services/orderAPI";
import { showAllAccounts } from "@/services/authAPI";
import { getImage } from "@/services/imageAPI";
import { showAllDiscount } from "@/services/discountAPI";
import { Role } from "@/utils/enum";
import { Product, ProductApiResponseItem } from "@/models/Entities/Product";
import defaultImage from "@/assets/diamond/defaultImage.png";

// Error Boundary Component
class ChartErrorBoundary extends Component<{ children: React.ReactNode }, { hasError: boolean }> {
  state = { hasError: false };

  static getDerivedStateFromError() {
    return { hasError: true };
  }

  componentDidCatch(error: Error, errorInfo: React.ErrorInfo) {
    console.error("ChartErrorBoundary caught error:", error, errorInfo);
  }

  render() {
    if (this.state.hasError) {
      return <p>Failed to render chart. Please check data and try again.</p>;
    }
    return this.props.children;
  }
}

const calculateKpiTotal = (
  startYear: number,
  startMonth: number,
  increasePerMonth: number
) => {
  const currentDate = new Date();
  const startDate = new Date(startYear, startMonth - 1);
  const monthsPassed =
    (currentDate.getFullYear() - startDate.getFullYear()) * 12 +
    (currentDate.getMonth() - startDate.getMonth());
  return monthsPassed * increasePerMonth;
};

const Dashboard = () => {
  const [orders, setOrders] = useState<any[]>([]);
  const [cancelOrders, setCancelOrders] = useState<any[]>([]);
  const [customers, setCustomers] = useState<any[]>([]);
  const [discounts, setDiscounts] = useState<any[]>([]);
  const [diamonds, setDiamonds] = useState<Product[]>([]);
  const [jewelrys, setJewelrys] = useState<any[]>([]);
  const [customersTotal, setCustomersTotal] = useState(0);
  const [ordersTotal, setOrdersTotal] = useState(0);
  const [cancelOrdersTotal, setCancelOrdersTotal] = useState(0);
  const [revenes, setRevenes] = useState<any | null>(null);
  const [mostRevenueWeek, setMostRevenueWeek] = useState<any | null>(null);
  const [dailyRevenueData, setDailyRevenueData] = useState<any[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [isDataLoaded, setIsDataLoaded] = useState(false); // New state to track data readiness

  const fetchData = async (retries = 2, delay = 1000) => {
    try {
      setIsLoading(true);
      setError(null);

      const [
        responseCustomers,
        responseOrders,
        responseProducts,
        responseDiscount,
        responseRevenes,
        responseWeeklyRevenue,
        responseDailyRevenue,
      ] = await Promise.all([
        showAllAccounts(),
        showAllOrder(),
        showAllProduct(),
        showAllDiscount(),
        showReveneSummary(),
        showWeeklyRevenueSummary(),
        showDailyRevenueSummary(),
      ]);

      // Customers
      const customersData = Array.isArray(responseCustomers.data?.data)
        ? responseCustomers.data.data
        : responseCustomers.data || [];
      const formattedCustomers = customersData
        .filter((acc: any) => acc && acc.role === Role.CUSTOMER)
        .map((acc: any) => ({
          id: acc.id,
          name: acc.name,
          role: acc.role,
        }));

      // Orders
      const ordersData = Array.isArray(responseOrders.data?.data)
        ? responseOrders.data.data
        : responseOrders.data || [];
      const formattedOrders = ordersData
        .filter((order: any) => order && order.id)
        .map((order: any) => ({
          orderID: order.id,
          orderDate: order.orderDate,
          price: order.totalPrice,
          status: order.status,
          customerID: order.userId,
        }));

      // Cancelled Orders
      const formattedCancelOrders = ordersData
        .filter((order: any) => order && (order.status === 6 || order.status === "Cancelled"))
        .map((order: any) => ({
          orderID: order.id,
          orderDate: order.orderDate,
          price: order.totalPrice,
          status: order.status,
        }));

      // Products: Diamonds & Jewelry
      const rawProductsData = Array.isArray(responseProducts.data)
        ? responseProducts.data
        : responseProducts.data?.data || [];
      const productsData = rawProductsData
        .filter((item: any) => item && item.success && item.product)
        .map((item: any) => item.product);
      const diamonds = productsData.filter((p: any) => p && p.carat > 0 && p.giaCertNumber);
      const jewelrys = productsData.filter((p: any) => p && !p.carat && !p.giaCertNumber);

      // Discounts
      const discountsData = Array.isArray(responseDiscount.data?.data)
        ? responseDiscount.data.data
        : responseDiscount.data || [];
      const formattedDiscounts = discountsData
        .filter((discount: any) => discount && discount.id)
        .map((discount: any) => ({
          discountID: discount.id,
          discountName: discount.name,
          percentDiscounts: discount.percentDiscounts || discount.percent || discount.value,
        }));

      // Revenue
      const reveneData = responseRevenes.data?.data || responseRevenes.data || {};
      const formattedRevene = {
        startDate: reveneData.startDate,
        endDate: reveneData.endDate,
        totalRevenueInTime: reveneData.totalRevenueInTime,
        mostRevenueInTime: reveneData.mostRevenueInTime,
        mostQuantiyInTime: reveneData.mostQuantiyInTime,
        orderResults: reveneData.orderResults || [],
      };

      // Weekly Revenue (for Top Week)
      const weeklyData = Array.isArray(responseWeeklyRevenue.data) ? responseWeeklyRevenue.data : [];
      if (weeklyData.length > 0) {
        const mostRevenueWeekData = weeklyData.reduce((prev: any, current: any) => {
          return (prev.totalRevenue > current.totalRevenue) ? prev : current;
        });

        const startDate = new Date(mostRevenueWeekData.weekStartDate);
        const endDate = new Date(mostRevenueWeekData.weekEndDate);

        const formattedStartDate = `${(startDate.getMonth() + 1).toString().padStart(2, '0')}/${startDate.getDate().toString().padStart(2, '0')}`;
        const formattedEndDate = `${(endDate.getMonth() + 1).toString().padStart(2, '0')}/${endDate.getDate().toString().padStart(2, '0')}`;

        setMostRevenueWeek({
          week: `${formattedStartDate} - ${formattedEndDate}`,
          revenue: mostRevenueWeekData.totalRevenue,
        });
      } else {
        setMostRevenueWeek(null);
      }

      // Daily Revenue
      const dailyData = Array.isArray(responseDailyRevenue.data?.DailyRevenueResults)
        ? responseDailyRevenue.data.DailyRevenueResults
        : Array.isArray(responseDailyRevenue.data)
        ? responseDailyRevenue.data
        : [];
      console.log("Raw daily revenue response:", JSON.stringify(responseDailyRevenue, null, 2)); // Detailed debug log
      console.log("Processed daily revenue data:", dailyData); // Debug log
      setDailyRevenueData(dailyData);
      setIsDataLoaded(true); // Mark data as loaded

      // KPI calculations
      const startYear = 2024;
      const startMonth = 1;
      const increaseCustomer = 10;
      const increaseOrder = 10;
      const increaseCancelOrder = 2;
      const totalCustomerKpi = calculateKpiTotal(startYear, startMonth, increaseCustomer);
      const totalOrderKpi = calculateKpiTotal(startYear, startMonth, increaseOrder);
      const totalCancelOrderKpi = calculateKpiTotal(startYear, startMonth, increaseCancelOrder);

      setCustomers(formattedCustomers);
      setOrders(formattedOrders);
      setDiamonds(diamonds);
      setJewelrys(jewelrys);
      setDiscounts(formattedDiscounts);
      setCancelOrders(formattedCancelOrders);
      setRevenes(formattedRevene);
      setCustomersTotal(totalCustomerKpi);
      setOrdersTotal(totalOrderKpi);
      setCancelOrdersTotal(totalCancelOrderKpi);
    } catch (error) {
      console.error("Failed to fetch dashboard data:", error);
      if (retries > 0) {
        console.log(`Retrying fetchData... Attempts left: ${retries}`);
        setTimeout(() => fetchData(retries - 1, delay * 2), delay);
      } else {
        setError("Failed to load dashboard data after retries. Please try again later.");
        setIsLoading(false);
      }
    } finally {
      if (!error) {
        setIsLoading(false);
      }
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  // Format daily revenue data for LineChart
  const chartData = isDataLoaded && dailyRevenueData.length > 0
    ? [
        {
          id: "Daily Revenue",
          color: "#912BBC",
          data: dailyRevenueData
            .filter((day: any) => {
              const isValid = day && day.date && typeof day.totalRevenue === "number" && !isNaN(new Date(day.date).getTime());
              if (!isValid) {
                console.warn("Invalid daily revenue entry:", day);
              }
              return isValid;
            })
            .map((day: any) => {
              const date = new Date(day.date);
              const formattedDate = `${(date.getMonth() + 1).toString().padStart(2, '0')}/${date.getDate().toString().padStart(2, '0')}`;
              return {
                x: formattedDate,
                y: day.totalRevenue,
              };
            }),
        },
      ]
    : [];
  console.log("Chart data:", JSON.stringify(chartData, null, 2)); // Detailed debug log

  if (isLoading) {
    return <Styled.AdminContainer>Loading...</Styled.AdminContainer>;
  }

  if (error) {
    return <Styled.AdminContainer>{error}</Styled.AdminContainer>;
  }

  return (
    <>
      <Styled.GlobalStyle />
      <Styled.AdminContainer>
        <Sidebar />
        <Styled.AdminPage>
          <Styled.TopPage>
            <Styled.TitlePage>
              <h1>Dashboard</h1>
              <p>View all status from the dashboard</p>
            </Styled.TitlePage>
            <Styled.DBContent>
              <Styled.DBContent_1>
                <StatistiBox
                  value={customers.length}
                  label="Total Customers"
                  total={customersTotal > 0 ? ((customers.length * 100) / customersTotal).toFixed(2) : "0"}
                />
                <StatistiBox
                  value={orders.length}
                  label="Total Orders"
                  total={ordersTotal > 0 ? ((orders.length * 100) / ordersTotal).toFixed(2) : "0"}
                />
                <Styled.TopMonth>
                  <p className="topMonth_title">Top week</p>
                  <h2>{mostRevenueWeek?.week || "N/A"}</h2>
                  <p className="topMonth-statisti">
                    {mostRevenueWeek?.revenue ? `${(mostRevenueWeek.revenue / 1000000).toFixed(2)}M` : "N/A"} sold so far
                  </p>
                </Styled.TopMonth>
              </Styled.DBContent_1>

              <Styled.DBContent_2>
                <Styled.Revenue>
                  <Styled.Revenue_Title>
                    <h2>Daily Revenue Report</h2>
                    <p className="revenueTotal">
                      {revenes?.totalRevenueInTime ? `$${revenes.totalRevenueInTime}` : "$"}
                    </p>
                  </Styled.Revenue_Title>
                  <Styled.Revenue_Content>
                    <ChartErrorBoundary>
                      {isDataLoaded && chartData.length > 0 && chartData[0].data.length > 0 ? (
                        <LineChart isDashboard={true} data={chartData} />
                      ) : (
                        <p>No daily revenue data available</p>
                      )}
                    </ChartErrorBoundary>
                  </Styled.Revenue_Content>
                </Styled.Revenue>

                <Styled.RecentOrders>
                  <Styled.RecentOrders_Title>
                    <Styled.MainTitle>
                      <h2>Recent Orders</h2>
                    </Styled.MainTitle>
                    <Link to="/admin/order">
                      <Styled.ViewAll>
                        <p>View All</p>
                        <ArrowRightOutlined />
                      </Styled.ViewAll>
                    </Link>
                  </Styled.RecentOrders_Title>
                  <Styled.RecentOrders_List>
                    {orders
                      .sort((a: any, b: any) => new Date(b.orderDate).getTime() - new Date(a.orderDate).getTime())
                      .slice(0, 4)
                      .map((order: any) => (
                        <div className="order_ele" key={order.orderID}>
                          <div className="order_eleInfor">
                            <p className="order_eleID">{order.orderID}</p>
                          </div>
                          <p className="order_eleDate">
                            {order.orderDate.replace("T", " ").replace(".000Z", "")}
                          </p>
                          <div className="order_elePrice">${order.price}</div>
                        </div>
                      ))}
                  </Styled.RecentOrders_List>
                </Styled.RecentOrders>
              </Styled.DBContent_2>

              <Styled.DBContent_3>
                <Styled.Element>
                  <Styled.Ele_Title>
                    <Styled.MainTitle>
                      <h2>Diamonds</h2>
                    </Styled.MainTitle>
                    <Link to="/admin/product">
                      <Styled.ViewAll>
                        <p>View All</p>
                        <ArrowRightOutlined />
                      </Styled.ViewAll>
                    </Link>
                  </Styled.Ele_Title>
                  <Styled.Ele_Content>
                    {diamonds.slice(0, 4).map((diamond: any) => (
                      <div className="shell_ele" key={diamond.id}>
                        <div className="shell_eleName">
                          <img src={defaultImage} alt={diamond.diamondName || diamond.name} />
                          <p>{diamond.name}</p>
                        </div>
                        <Link to={`/admin/product/diamond/detail/${diamond.id}`}>
                          <button className="shell_eleButton">View</button>
                        </Link>
                      </div>
                    ))}
                  </Styled.Ele_Content>
                </Styled.Element>

                <Styled.Element>
                  <Styled.Ele_Title>
                    <h2>Promotional</h2>
                    <Link to="/admin/marketing">
                      <Styled.ViewAll>
                        <p>View All</p>
                        <ArrowRightOutlined />
                      </Styled.ViewAll>
                    </Link>
                  </Styled.Ele_Title>
                  <Styled.Ele_Content>
                    {discounts.slice(0, 4).map((discount: any) => (
                      <div className="shell_ele" key={discount.discountID}>
                        <div className="shell_eleName">
                          <p>{discount.discountName}</p>
                        </div>
                        <div className="shell_elePercent">
                          <p>{discount.percentDiscounts}%</p>
                        </div>
                      </div>
                    ))}
                  </Styled.Ele_Content>
                </Styled.Element>
              </Styled.DBContent_3>
            </Styled.DBContent>
          </Styled.TopPage>
        </Styled.AdminPage>
      </Styled.AdminContainer>
    </>
  );
};

export default Dashboard;