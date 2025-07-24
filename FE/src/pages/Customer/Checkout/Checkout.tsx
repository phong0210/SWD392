/* eslint-disable @typescript-eslint/no-explicit-any */
import * as React from "react";
import styled from "styled-components";
import { Link, useNavigate } from "react-router-dom";
import { useState } from "react";
import { notification, Steps } from "antd";
import AddressDetails from "../../../components/Customer/Checkout/AddressDetails";
import { getProvinces, getDistricts, getWards } from "./api";
import Summary from "@/components/Customer/Checkout/Summary/Summary";
import { createOrder, CreateOrderRequest } from "@/services/orderAPI";
import { showAllOrderLineForAdmin, updateOrderLine } from "@/services/orderLineAPI";
import config from "@/config";
import useAuth from "@/hooks/useAuth";
import { getCustomer } from "@/services/accountApi";
import { OrderStatus, PaymentMethodEnum } from "@/utils/enum";
import { useAppDispatch, useAppSelector } from "@/hooks";
import { orderSlice } from "@/layouts/MainLayout/slice/orderSlice";
import { createOrderPaypal } from "@/services/paymentAPI";

// Remove local interface since we're using CreateOrderRequest from the API

const description = "This is a description";
const Checkout: React.FC = () => {
  const { AccountID, user } = useAuth();
  const [CustomerID, setCustomerID] = useState<string | number>();
  const [Customer, setCustomer] = useState<any>(null);
  
  // Get userId from auth slice (this is the GUID we need for CustomerId)
  const authUser = useAppSelector((state) => state.auth.user);
  // const [form] = Form.useForm();
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const [provinces, setProvinces] = useState<any[]>([]);
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const [districts, setDistricts] = useState<any[]>([]);
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const [wards, setWards] = useState<any[]>([]);
  const [selectedProvince, setSelectedProvince] = useState<number | null>(null);
  const [selectedDistrict, setSelectedDistrict] = useState<number | null>(null);
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
  const [selectedVoucher, setSelectedVoucher] = useState<any | null>(null);
  const ShippingFee = useAppSelector((state) => state.order.Shippingfee);
  const cartItems = useAppSelector((state) => state.cart.items);
  const [api, contextHolder] = notification.useNotification();
  const [loading, setLoading] = useState(false);

  const fetchProvincesData = async () => {
    try {
      const data = await getProvinces();
      setProvinces(data);
    } catch (error) {
      console.error("Error fetching provinces:", error);
    }
  };

  const getCustomerDetail = async () => {
    // First try to get customer from auth (fallback)
    if (user?.CustomerID) {
      setCustomerID(user.CustomerID); // Use CustomerID from auth user
      console.log('Using Customer ID from auth:', user.CustomerID);
      return;
    }
    
    // Fallback to API call if auth doesn't have CustomerID
    if (AccountID === null) return;
    const customer = await getCustomer(AccountID ? AccountID : 0);
    setCustomer(customer?.data?.data);
    // console.log('Customer: ', Customer);
    setCustomerID(customer ? customer.data.data.CustomerID : 0);
    console.log('Customer ID from API: ', CustomerID);
  }

  React.useEffect(() => {
    getCustomerDetail();
    fetchProvincesData();
  }, [Customer?.CustomerID, AccountID]);

  React.useEffect(() => {
    getCustomerDetail();
    fetchProvincesData();
    
    const voucher = localStorage.getItem("selectedVoucher");
    if (voucher) {
      setSelectedVoucher(JSON.parse(voucher));
      
    }
  }, [AccountID]);

  const onFinish = async (values: any) => {
    setLoading(true);
    try {
      // Debug: Log customer info
      console.log('Auth slice user:', authUser);
      console.log('UseAuth user:', user);
      console.log('Customer:', Customer);
      console.log('CustomerID state:', CustomerID);
      console.log('Cart Items:', cartItems);

      // Transform cart items to the new orderItems format
      const orderItems = cartItems.map(item => {
        const productId = item.productId || item.diamondId || item.id;
        if (!productId) {
          throw new Error(`Product ID is missing for item: ${JSON.stringify(item)}`);
        }
        
        // Ensure UnitPrice is a valid number
        const unitPrice = item.price || item.unitPrice || 0;
        if (typeof unitPrice !== 'number' || unitPrice <= 0) {
          throw new Error(`Invalid unit price for item: ${JSON.stringify(item)}`);
        }

        // Ensure Quantity is a valid number
        if (typeof item.quantity !== 'number' || item.quantity <= 0) {
          throw new Error(`Invalid quantity for item: ${JSON.stringify(item)}`);
        }

        return {
          ProductId: productId.toString(), // Ensure it's a string
          Quantity: item.quantity,
          UnitPrice: unitPrice
        };
      });

      // Validate CustomerId - use auth slice userId as primary (this is the GUID)
      const customerId = authUser?.userId || user?.CustomerID?.toString() || Customer?.CustomerID?.toString() || CustomerID?.toString() || "";
      if (!customerId) {
        throw new Error('Customer ID is required but not found in auth or customer data');
      }

      // Create the new order request body format using CreateOrderRequest type
      const requestBodyOrder: CreateOrderRequest = {
        CustomerId: customerId,
        SaleStaff: "", 
        OrderItems: orderItems
      };

      console.log('Final Order request body:', JSON.stringify(requestBodyOrder, null, 2));

      const responeOrder = await createOrder(requestBodyOrder);
      
      // Debug: Log the response
      console.log('Order Response:', responeOrder);
      
      // Check if the order creation was successful based on the actual API response structure
      if (!responeOrder.data.success) {
        throw new Error(responeOrder.data.error || 'Order creation failed');
      }

      const getOrderID = responeOrder.data.orderId; // Use the correct property name
      dispatch(orderSlice.actions.setOrderID(getOrderID));
      localStorage.setItem("CurrentOrderID", JSON.stringify(getOrderID));
      console.log('Order ID:', getOrderID);

      if (values.Method === PaymentMethodEnum.PAYPAL) {
          const createPayment = await createOrderPaypal(cartItems.reduce((acc, item) => acc + item.price * item.quantity, 0));
          window.location.href = createPayment.data.links[1].href;
      } else {
        navigate(config.routes.public.success);
      }
    } catch (error: any) {
      console.error('Order creation error:', error);
      console.error('Error response:', error.response?.data);
      api.error({
        message: 'Error',
        description: error.response?.data?.message || error.message || 'An error occurred'
      });
    } finally {
      setLoading(false);
    }
  }

  const handleProvinceChange = async (provinceId: unknown) => {
    setSelectedProvince(provinceId as number);
    setSelectedDistrict(null); // Reset lại quận/huyện khi thay đổi tỉnh/thành phố
    try {
      const data = await getDistricts(provinceId as number);
      setDistricts(data);
    } catch (error) {
      console.error("Error fetching districts:", error);
    }
  };

  const handleDistrictChange = async (districtId: unknown) => {
    setSelectedDistrict(districtId as number);
    try {
      const data = await getWards(districtId as number);
      setWards(data);
    } catch (error) {
      console.error("Error fetching wards:", error);
    }
  };

  return (
    <main>
      {contextHolder}
      <ContainerHeader>
        <Header>Checkout</Header>
      </ContainerHeader>
      <StepEdit>
        <Steps
          className="steps-edit"
          current={1}
          status="wait"
          items={[
            {
              title: "Checkout",
              description,
            },
            {
              title: "Order",

              description,
            },
            {
              title: "Finish",
              description,
            },
          ]}
        />
      </StepEdit>
      <Wrapper>
        <StyledLink>
          <Link to="/cart">BACK TO CART</Link>
        </StyledLink>
        <Content>
          <Formm>
            {/* <ContactInfo email={Customer?.Email} onEdit={handleEdit} /> */}
            <AddressDetails
              onFinish={onFinish}
              provinces={provinces}
              districts={districts}
              wards={wards}
              selectedProvince={selectedProvince}
              selectedDistrict={selectedDistrict}
              onProvinceChange={handleProvinceChange}
              onDistrictChange={handleDistrictChange}
              loading={loading}
            />
          </Formm>
          <StyledSummary cartItems={cartItems} />
        </Content>
      </Wrapper>
    </main>
  );
};

export default Checkout;

const StyledSummary = styled(Summary)`
  flex: 1;
  line-height: 40px;
`;

const StepEdit = styled.div`
  display: flex;
  justify-content: space-around;
  .steps-edit {
    max-width: 1000px;
  }
`;

const ContainerHeader = styled.div`
  display: flex;
  justify-content: center;
`;

const Header = styled.header`
  background: #fff;
  width: 1400px;
  color: #818594;
  font: 14px / 150% "Crimson Text", sans-serif;
  border-bottom: 1px solid #e4e4e4;
  border-top: 1px solid #e4e4e4;
  padding: 10px 0;
  display: flex;
  margin-bottom: 2rem;
  @media (max-width: 991px) {
    padding: 0 20px 0 30px;
    margin-top: 40px;
  }
`;

const Wrapper = styled.div`
  background-color: #fff;
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-bottom: 50px;
  @media (max-width: 991px) {
    padding: 0 20px;
  }
`;

const StyledLink = styled.a`
  color: #000;
  text-decoration: underline;
  margin-top: 57px;
  margin-bottom: 10px;
  width: 1400px;
  font: 250 10px/150% Poppins, sans-serif;
  
  @media (max-width: 991px) {
    margin-top: 40px;
  }
`;

const Content = styled.div`
  display: flex;
  flex-direction: row;
  gap: 20px;
  width: 1400px;
  align-items: flex-start;
  @media (max-width: 991px) {
    flex-direction: column;
  }
`;

const Formm = styled.div`
  flex: 2;
  display: flex;
  flex-direction: column;
  gap: 20px;
`;