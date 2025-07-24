/* eslint-disable @typescript-eslint/no-explicit-any */
import * as React from "react";
import styled from "styled-components";
import { Link, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import { notification, Steps } from "antd";
import AddressDetails from "../../../components/Customer/Checkout/AddressDetails";
import { getProvinces, getDistricts, getWards } from "./api";
import Summary from "@/components/Customer/Checkout/Summary/Summary";
import { CreateOrderRequest } from "@/services/orderAPI";
import config from "@/config";
import useAuth from "@/hooks/useAuth";
import { getCustomer } from "@/services/accountApi";
import { PaymentMethodEnum } from "@/utils/enum";
import { useAppDispatch, useAppSelector } from "@/hooks";
import { createOrderAsync, resetOrderStatus } from "@/store/slices/orderSlice";
import { createOrderPaypal } from "@/services/paymentAPI";
import { clearCart } from "@/store/slices/cartSlice";

const description = "This is a description";

const Checkout: React.FC = () => {
  const { AccountID, user } = useAuth();
  const [CustomerID, setCustomerID] = useState<string | number>();
  const [Customer, setCustomer] = useState<any>(null);
  
  const authUser = useAppSelector((state) => state.auth.user);
  const [provinces, setProvinces] = useState<any[]>([]);
  const [districts, setDistricts] = useState<any[]>([]);
  const [wards, setWards] = useState<any[]>([]);
  const [selectedProvince, setSelectedProvince] = useState<number | null>(null);
  const [selectedDistrict, setSelectedDistrict] = useState<number | null>(null);
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
  const cartItems = useAppSelector((state) => state.cart.items);
  const { order, status: orderStatus, error: orderError } = useAppSelector((state) => state.order);
  const [api, contextHolder] = notification.useNotification();
  const [loading, setLoading] = useState(false);

  // Cleanup on unmount only if order creation failed
  useEffect(() => {
    return () => {
      if (orderStatus === 'failed') {
        console.log('[Checkout] Cleaning up on unmount due to failed order status');
        dispatch(resetOrderStatus());
      }
    };
  }, [dispatch, orderStatus]);

  // Effect to handle navigation after order creation
  useEffect(() => {
    if (orderStatus === 'succeeded' && order && order.id) {
      console.log('[Checkout] Order status succeeded, order:', order);
      if (order.payments && order.payments[0]?.method === PaymentMethodEnum.PAYPAL.toString()) {
        console.log('[Checkout] Initiating PayPal flow for order:', order.id);
        createOrderPaypal(order.totalPrice)
          .then(createPayment => {
            const approvalUrl = createPayment.data.links.find((link: any) => link.rel === 'approve')?.href;
            if (approvalUrl) {
              console.log('[Checkout] Redirecting to PayPal approval URL:', approvalUrl);
              window.location.href = approvalUrl;
            } else {
              throw new Error('PayPal approval URL not found');
            }
          })
          .catch(error => {
            console.error('[Checkout] PayPal error:', error);
            api.error({
              message: 'PayPal Error',
              description: error.message || 'Failed to create PayPal payment. Please try again.',
            });
            setLoading(false);
          });
      } else {
        console.log('[Checkout] Navigating to success page for COD order:', order.id);
        dispatch(clearCart());
        navigate(config.routes.public.success);
      }
    } else if (orderStatus === 'failed') {
      console.error('[Checkout] Order creation failed, error:', orderError);
      api.error({
        message: 'Order Creation Failed',
        description: orderError || 'An error occurred while creating your order',
      });
      setLoading(false);
    }
  }, [orderStatus, order, orderError, navigate, api]);

  const fetchProvincesData = async () => {
    try {
      const data = await getProvinces();
      setProvinces(data);
    } catch (error) {
      console.error("[Checkout] Error fetching provinces:", error);
    }
  };

  const getCustomerDetail = async () => {
    if (user?.CustomerID) {
      setCustomerID(user.CustomerID);
      return;
    }
    
    if (AccountID === null) return;
    try {
      const customer = await getCustomer(AccountID ? AccountID : 0);
      setCustomer(customer?.data?.data);
      setCustomerID(customer ? customer.data.data.CustomerID : 0);
    } catch (error) {
      console.error("[Checkout] Error fetching customer details:", error);
    }
  };

  React.useEffect(() => {
    getCustomerDetail();
    fetchProvincesData();
  }, [Customer?.CustomerID, AccountID]);

  const onFinish = async (values: any) => {
    setLoading(true);
    try {
      if (!cartItems || cartItems.length === 0) {
        throw new Error('Your cart is empty. Please add items before checkout.');
      }

      const orderItems = cartItems.map(item => ({
        ProductId: (item.productId || item.diamondId || item.id).toString(),
        Quantity: item.quantity,
        UnitPrice: item.price || 0,
      }));

      const customerId = authUser?.userId || user?.CustomerID?.toString() || Customer?.CustomerID?.toString() || CustomerID?.toString() || "";
      if (!customerId) {
        throw new Error('Customer ID is required. Please log in again.');
      }

      const requestBodyOrder: CreateOrderRequest = {
        CustomerId: customerId,
        SaleStaff: "",
        OrderItems: orderItems,
        PaymentMethod: values.Method || 'COD',
      };

      console.log('[Checkout] Submitting order:', requestBodyOrder);
      await dispatch(createOrderAsync(requestBodyOrder)).unwrap();

    } catch (error: any) {
      console.error('[Checkout] Checkout error:', error);
      api.error({
        message: 'Checkout Error',
        description: error.message || 'An error occurred while processing your order',
      });
      setLoading(false);
    }
  };

  const handleProvinceChange = async (provinceId: unknown) => {
    setSelectedProvince(provinceId as number);
    setSelectedDistrict(null);
    try {
      const data = await getDistricts(provinceId as number);
      setDistricts(data);
    } catch (error) {
      console.error("[Checkout] Error fetching districts:", error);
    }
  };

  const handleDistrictChange = async (districtId: unknown) => {
    setSelectedDistrict(districtId as number);
    try {
      const data = await getWards(districtId as number);
      setWards(data);
    } catch (error) {
      console.error("[Checkout] Error fetching wards:", error);
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
            <AddressDetails
              onFinish={onFinish}
              provinces={provinces}
              districts={districts}
              wards={wards}
              selectedProvince={selectedProvince}
              selectedDistrict={selectedDistrict}
              onProvinceChange={handleProvinceChange}
              onDistrictChange={handleDistrictChange}
              loading={loading || orderStatus === 'loading'}
            />
          </Formm>
          <StyledSummary cartItems={cartItems} />
        </Content>
      </Wrapper>
    </main>
  );
};

// Styled components remain unchanged
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

export default Checkout;