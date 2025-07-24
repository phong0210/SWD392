import * as Styled from "./Dashboard.styled";
import Sidebar from "../../../components/Admin/Sidebar/Sidebar";
import { ArrowRightOutlined } from "@ant-design/icons";
import { useEffect, useState } from "react";
import StatistiBox from "./StatistiBox";
import LineChart from "./LineChart";
import { Link } from "react-router-dom";
import { showAllProduct } from "@/services/productAPI";
import { showAllOrder, showReveneSummary, showWeeklyRevenueSummary } from "@/services/orderAPI";
import { showAllAccounts } from "@/services/authAPI";
import { getImage } from "@/services/imageAPI";
import { showAllDiscount } from "@/services/discountAPI";
import { Role } from "@/utils/enum";
import { Product, ProductApiResponseItem } from "@/models/Entities/Product";

const calculateKpiTotal = (
  startYear: any,
  startMonth: any,
  increasePerMonth: any
) => {
  const currentDate = new Date();
  const startDate = new Date(startYear, startMonth - 1);
  const monthsPassed =
    (currentDate.getFullYear() - startDate.getFullYear()) * 12 +
    (currentDate.getMonth() - startDate.getMonth());
  return monthsPassed * increasePerMonth;
};

const Dashboard = () => {
  const [orders, setOrders] = useState([]);
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

  const fetchData = async () => {
    try {
      const responseCustomers = await showAllAccounts();
      const responseOrders = await showAllOrder();
      const responseProducts = await showAllProduct();
      const responseDiscount = await showAllDiscount();
      const responseRevenes = await showReveneSummary();
      const responseWeeklyRevenue = await showWeeklyRevenueSummary();

      // Customers
      const customersData = Array.isArray(responseCustomers.data?.data)
        ? responseCustomers.data.data
        : responseCustomers.data;
      const formattedCustomers = customersData
        .filter((acc: any) => acc.role === Role.CUSTOMER)
        .map((acc: any) => ({
          id: acc.id,
          name: acc.name,
          role: acc.role,
        }));

      // Orders
      const ordersData = Array.isArray(responseOrders.data?.data)
        ? responseOrders.data.data
        : responseOrders.data;
      const formattedOrders = ordersData.map((order: any) => ({
        orderID: order.id,
        orderDate: order.orderDate,
        price: order.totalPrice,
        status: order.status,
        customerID: order.userId,
      }));

      // Cancelled Orders
      const formattedCancelOrders = ordersData
        .filter((order: any) => order.status === 6 || order.status === "Cancelled")
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
      // Extract the 'product' property from each API response item
      const productsData = rawProductsData
        .filter((item: any) => item.success && item.product)
        .map((item: any) => item.product);
      const diamonds = productsData.filter((p: any) => p.type === "diamond");
      const jewelrys = productsData.filter((p: any) => p.type === "jewelry");

      // Discounts
      const discountsData = Array.isArray(responseDiscount.data?.data)
        ? responseDiscount.data.data
        : responseDiscount.data;
      const formattedDiscounts = discountsData.map((discount: any) => ({
        discountID: discount.id,
        discountName: discount.name,
        percentDiscounts: discount.percentDiscounts || discount.percent || discount.value,
      }));

      // Revenue
      const reveneData = responseRevenes.data?.data || responseRevenes.data;
      const formattedRevene = {
        startDate: reveneData.startDate,
        endDate: reveneData.endDate,
        totalRevenueInTime: reveneData.totalRevenueInTime,
        mostRevenueInTime: reveneData.mostRevenueInTime,
        mostQuantiyInTime: reveneData.mostQuantiyInTime,
        orderResults: reveneData.orderResults || [],
      };

      setCustomers(formattedCustomers);
      setOrders(formattedOrders);
      setDiamonds(diamonds);
      setJewelrys(jewelrys);
      setDiscounts(formattedDiscounts);
      setCancelOrders(formattedCancelOrders);
      setRevenes(formattedRevene);

      // Weekly Revenue
      const weeklyRevenueData = responseWeeklyRevenue.data;
      if (Array.isArray(weeklyRevenueData) && weeklyRevenueData.length > 0) {
        const mostRevenueWeekData = weeklyRevenueData.reduce((prev: any, current: any) => {
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

      // KPI calculations (as before)
      const startYear = 2024;
      const startMonth = 1;
      const increaseCustomer = 10;
      const increaseOrder = 10;
      const increaseCancelOrder = 2;
      const totalCustomerKpi = calculateKpiTotal(
        startYear,
        startMonth,
        increaseCustomer
      );
      const totalOrderKpi = calculateKpiTotal(
        startYear,
        startMonth,
        increaseOrder
      );
      const totalCancelOrderKpi = calculateKpiTotal(
        startYear,
        startMonth,
        increaseCancelOrder
      );
      setCustomersTotal(totalCustomerKpi);
      setOrdersTotal(totalOrderKpi);
      setCancelOrdersTotal(totalCancelOrderKpi);
    } catch (error) {
      console.error("Failed to fetch dashboard data:", error);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

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
                  total={((customers.length * 100) / customersTotal).toFixed(2)}
                />
                <StatistiBox
                  value={orders.length}
                  label="Total Orders"
                  total={((orders.length * 100) / ordersTotal).toFixed(2)}
                />
                {/* <StatistiBox
                  value={cancelOrders.length}
                  label="Cancel Orders"
                  total={(cancelOrders.length * 100 / cancelOrdersTotal).toFixed(2)}
                /> */}
                <Styled.TopMonth>
                  <p className="topMonth_title">Top week</p>
                  <h2>{mostRevenueWeek?.week}</h2>
                  <p className="topMonth-statisti">
                    {mostRevenueWeek?.revenue ? `${(mostRevenueWeek.revenue / 1000000).toFixed(2)}M` : 'N/A'} sold so far
                  </p>
                </Styled.TopMonth>
              </Styled.DBContent_1>

              <Styled.DBContent_2>
                <Styled.Revenue>
                  <Styled.Revenue_Title>
                    <h2>Revenue Report</h2>
                    {/* <p className="revenueTotal">{`$${data.revene}`}</p> */}
                    <p className="revenueTotal">
                      {revenes?.totalRevenueInTime}$
                    </p>
                  </Styled.Revenue_Title>
                  <Styled.Revenue_Content>
                    <LineChart isDashboard={true} />
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
                      .sort(
                        (a: any, b: any) =>
                          new Date(b.orderDate).getTime() -
                          new Date(a.orderDate).getTime()
                      )
                      .slice(0, 4)
                      .map((order: any) => (
                        <div className="order_ele" key={order.orderID}>
                          <div className="order_eleInfor">
                            <p className="order_eleID">{order.orderID}</p>
                            {/* <p className="order_eleCusName">
                              {order.customerID}
                            </p> */}
                          </div>
                          <p className="order_eleDate">
                            {order.orderDate
                              .replace("T", " ")
                              .replace(".000Z", " ")}
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
                          {/* <img
                              src={diamond.images && diamond.images[0] ? diamond.images[0].url : "default-image-url"}
                              alt={diamond.diamondName} /> */}
                          <p>{diamond.name}</p>
                        </div>
                        <Link
                          to={`/admin/product/diamond/detail/${diamond.id}`}
                        >
                          <button className="shell_eleButton">View</button>
                        </Link>
                      </div>
                    ))}
                  </Styled.Ele_Content>
                </Styled.Element>

                <Styled.Element>
                  <Styled.Ele_Title>
                    <Styled.MainTitle>
                      <h2>Jewelrys</h2>
                    </Styled.MainTitle>
                    <Link to="/admin/product/jewelry">
                      <Styled.ViewAll>
                        <p>View All</p>
                        <ArrowRightOutlined />
                      </Styled.ViewAll>
                    </Link>
                  </Styled.Ele_Title>
                  <Styled.Ele_Content>
                    {jewelrys.slice(0, 4).map((jewelry: any) => (
                      <div className="shell_ele" key={jewelry.id}>
                        <div className="shell_eleName">
                          {/* <img
                              src={jewelry.images && jewelry.images[0] ? jewelry.images[0].url : "default-image-url"}
                              alt={jewelry.jewelryName} /> */}
                          <p>{jewelry.name}</p>
                        </div>
                        <Link
                          to={`/admin/product/jewelry/detail/${jewelry.id}`}
                        >
                          <button className="shell_eleButton">View</button>
                        </Link>
                      </div>
                    ))}
                  </Styled.Ele_Content>
                  <div className="chatNofi_content"></div>
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
                        {/* <Link to={`/admin/marketing/discount/detail/${discount.discountID}`}>
                        <button className="shell_eleButton">View</button>
                        </Link> */}
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
