/* eslint-disable react-hooks/exhaustive-deps */
/* eslint-disable @typescript-eslint/no-explicit-any */
import React, { useEffect, useState } from "react";
import styled from "styled-components";
import Table from "antd/es/table";
import { TableColumnGroupType, Tag, notification } from "antd";
import ReviewForm from "./ReviewForm";
import { Link, useLocation } from "react-router-dom";

import { getOrderDetailDetail, orderDetail } from "@/services/orderAPI";
import useAuth from "@/hooks/useAuth";
import vnpay from "@/assets/diamond/vnpay.png";
import defaultImage from "@/assets/diamond/defaultImage.png";

interface ProductDetail {
  ProductID: string;
  Name: string;
  Description: string;
  Price: number;
  UsingImage?: string;
  orderDetailId: string;
  quantity: number;
  sku: string;
}

const StatusTag = ({ status }: { status: number }) => {
  let color = "green";
  let statusText = "";

  switch (status) {
    case 0:
      color = "grey";
      statusText = "Pending";
      break;
    case 1:
      color = "geekblue";
      statusText = "Accept";
      break;
    case 2:
      color = "green";
      statusText = "Delivering";
      break;
    case 3:
      color = "volcano";
      statusText = "Delivered";
      break;
    case 4:
      color = "#32CD32";
      statusText = "Completed";
      break;
    case 5:
      color = "#32CD32";
      statusText = "Confirmed";
      break;
    default:
      color = "default";
      statusText = "Cancelled";
  }

  return (
    <Tag color={color} key={status}>
      {statusText.toUpperCase()}
    </Tag>
  );
};

const formatPrice = (price: number | bigint) => {
  return `$ ${new Intl.NumberFormat("en-US", {
    style: "decimal",
    minimumFractionDigits: 0,
  }).format(price)}`;
};

const formatDate = (dateString: string) => {
  const date = new Date(dateString);
  return new Intl.DateTimeFormat("en-GB", {
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
    hour: "2-digit",
    minute: "2-digit",
  }).format(date);
};

const OrderDetail: React.FC = () => {
  const [productDetails, setProductDetails] = useState<ProductDetail[]>([]);
  const [shippingFee, setShippingFee] = useState<number>(0);
  const [discount, setDiscount] = useState(0);
  const [subTotal, setSubTotal] = useState(0);
  const [loading, setLoading] = useState(false);
  const [totalPrice, setTotalPrice] = useState(0);
  const [status, setStatus] = useState(0);
  const { AccountID } = useAuth();

  const [reviewedDiamonds, setReviewedDiamonds] = useState<Set<number>>(
    new Set()
  );
  const [reviewedProducts, setReviewedProducts] = useState<Set<number>>(
    new Set()
  );
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [selectedDiamondID, setSelectedDiamondID] = useState<number | null>(
    null
  );
  const [selectedProductID, setSelectedProductID] = useState<string | null>(
    null
  );

  const location = useLocation();
  const searchParams = new URLSearchParams(location.search);
  const orderId = searchParams.get("orderId");

  console.log("OrderID:", orderId);

  const fetchOrderDetails = async () => {
    if (!orderId) {
      console.error("No order ID provided");
      notification.error({
        message: "Error",
        description: "No order ID provided in URL",
      });
      return;
    }

    console.log("Fetching order details for orderId:", orderId);

    try {
      const response = await getOrderDetailDetail(orderId);
      const responTotal = await orderDetail(orderId);
      console.log("data", responTotal.data.order.status);
      setStatus(responTotal.data.order.status);

      const amount = responTotal.data.order.payments[0].amount;

      console.log("Amount:", amount);
      setTotalPrice(amount);

      if (response.data) {
        const orderDetails = response.data.data;
        console.log("order Details ne", response.data.data);
        // Transform data to match component interface
        const transformedDetails = orderDetails.map((item) => ({
          ProductID: item.product.id,
          Name: item.product.name,
          Description: item.product.description,
          Price: item.unitPrice,
          UsingImage: defaultImage,
          orderDetailId: item.id,
          quantity: item.quantity,
          sku: item.product.sku,
        }));

        setProductDetails(transformedDetails);

        // Calculate subtotal
        const calculatedSubTotal = orderDetails.reduce(
          (total, item) => total + item.unitPrice * item.quantity,
          0
        );
        setSubTotal(calculatedSubTotal);

        console.log("Transformed product details:", transformedDetails);
        console.log("Calculated subtotal:", calculatedSubTotal);
      } else {
        console.error("Failed to fetch order details:", response.data.message);
        notification.error({
          message: "Error",
          description: response.data.message || "Failed to fetch order details",
        });
      }
    } catch (error) {
      console.error("Error fetching order details:", error);
      notification.error({
        message: "Error",
        description: "An error occurred while fetching order details",
      });
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchOrderDetails();
  }, [orderId]);

  const columns: TableColumnGroupType<ProductDetail>[] = [
    {
      title: "Product",
      children: [
        {
          title: "Image",
          dataIndex: "UsingImage",
          key: "UsingImage",
          render: (usingImage: string | undefined) =>
            usingImage ? (
              <img
                src={usingImage}
                alt="Product Image"
                style={{ width: "100px", height: "auto" }}
              />
            ) : (
              "No Image"
            ),
        },
        {
          title: "Name",
          dataIndex: "Name",
          key: "Name",
        },
        {
          title: "SKU",
          dataIndex: "sku",
          key: "sku",
        },
        {
          title: "Description",
          dataIndex: "Description",
          key: "Description",
        },
        {
          title: "Quantity",
          dataIndex: "quantity",
          key: "quantity",
        },
        {
          title: "Unit Price",
          dataIndex: "Price",
          key: "Price",
          render: (price: number) => formatPrice(price),
          sorter: (a: any, b: any) => a.Price - b.Price,
          sortDirections: ["descend", "ascend"],
        },
        {
          title: "Total",
          key: "total",
          render: (_: any, record: ProductDetail) => {
            const total = record.Price * (record.quantity || 1);
            return formatPrice(total);
          },
        },
      ],
    },
  ];

  const handleFeedbackCreate = (values: any) => {
    console.log("Feedback submitted: ", values);

    const { DiamondID, ProductID } = values;

    setIsModalVisible(false);
    if (DiamondID) {
      setReviewedDiamonds((prev) => {
        const updated = new Set(prev);
        updated.add(DiamondID);
        console.log("Updated Diamond IDs:", updated);
        return updated;
      });
    }

    if (ProductID) {
      setReviewedProducts((prev) => {
        const updated = new Set(prev);
        updated.add(ProductID);
        console.log("Updated Product IDs:", updated);
        return updated;
      });
    }

    notification.success({
      message: "Feedback Submitted",
      description: "Thank you for your feedback!",
    });
  };

  return (
    <MainContainer>
      <Container>
        <OrderWrapper>
          <OrderTitle>Order Information</OrderTitle>
          <OrderDetailsContainer>
            <OrderDetails>
              <CustomerInfo>Order ID: {orderId}</CustomerInfo>
              <CustomerInfo>
                Status: <StatusTag status={status} />{" "}
                {/* Default to Delivered */}
              </CustomerInfo>
            </OrderDetails>
          </OrderDetailsContainer>
        </OrderWrapper>

        <ProductsWrapper>
          <Table
            style={{ backgroundColor: "#e8e8e8" }}
            columns={columns}
            dataSource={productDetails}
            pagination={false}
            rowKey="orderDetailId"
            loading={loading}
          />
          <ReviewForm
            visible={isModalVisible}
            onCreate={handleFeedbackCreate}
            onCancel={() => setIsModalVisible(false)}
            orderId={orderId}
            accountId={AccountID}
            diamondId={selectedDiamondID}
            productId={selectedProductID}
          />
        </ProductsWrapper>

        <OrderInfo>
          <Row>
            <InfoTitle>Payment method</InfoTitle>
            <img
              style={{ width: "150px", objectFit: "contain", maxWidth: "100%" }}
              className="payment-method"
              src={vnpay}
              alt="Payment method"
            />
          </Row>
          <Column>
            <InfoTextBold>
              Discount:
              <span>-{formatPrice(subTotal - totalPrice) || 0}</span>
            </InfoTextBold>

            <InfoText>
              <div>Shipping:</div>
              <div>{shippingFee > 0 ? formatPrice(shippingFee) : "Free"}</div>
            </InfoText>
            <InfoText>
              <div>Subtotal:</div>
              <div>{formatPrice(subTotal)}</div>
            </InfoText>

            <br />
            <InfoTextBold style={{ color: "red" }}>
              <div>Total:</div>
              <div>{formatPrice(totalPrice + shippingFee)}</div>
            </InfoTextBold>
          </Column>
        </OrderInfo>

        <EditButton>
          <Link to={`/history`}>Back</Link>
        </EditButton>
      </Container>
    </MainContainer>
  );
};

export default OrderDetail;

const Container = styled.div`
  background-color: #fff;
  display: flex;
  flex-direction: column;
  align-items: center;
  padding-top: 2rem;
  width: 1400px;
  padding-bottom: 3rem;

  @media (max-width: 768px) {
    padding: 20px;
  }
`;

const OrderWrapper = styled.div`
  display: flex;
  flex-direction: column;
  margin-top: 27px;
  width: 100%;
  gap: 20px;
  color: #151542;
  border-bottom: 1px solid;
  padding-bottom: 3rem;
`;

const OrderDetailsContainer = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 20px;

  @media (max-width: 768px) {
    flex-direction: column;
    gap: 10px;
  }
`;

const OrderDetails = styled.div`
  display: flex;
  flex-direction: column;
  font-size: 18px;
  font-weight: 400;
  width: 60%;

  @media (max-width: 768px) {
    width: 100%;
  }
`;

const OrderTitle = styled.h1`
  font-weight: 600;
  font-size: 24px;

  @media (max-width: 768px) {
    font-size: 20px;
  }
`;

const CustomerInfo = styled.p`
  margin-top: 24px;

  @media (max-width: 768px) {
    margin-top: 10px;
    font-size: 16px;
  }
`;

const ProductsWrapper = styled.div`
  display: flex;
  flex-direction: column;
  background-color: #fff;
  width: 100%;
  padding: 46px 0;
  border-bottom: 1px solid;
  padding-bottom: 3rem;

  @media (max-width: 768px) {
    padding: 20px 0;
  }
`;

const OrderInfo = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-top: 20px;
  width: 100%;

  @media (max-width: 768px) {
    flex-direction: column;
    gap: 10px;
  }
`;

const InfoTitle = styled.h3`
  color: #92929d;
  font-weight: 600;
  font-size: 16px;
  padding-bottom: 1rem;

  @media (max-width: 768px) {
    font-size: 14px;
  }
`;

const InfoText = styled.p`
  color: #151542;
  margin-top: 20px;
  font-size: 18px;
  display: flex;
  justify-content: space-between;
  align-items: center;

  @media (max-width: 768px) {
    margin-top: 10px;
    font-size: 16px;
  }
`;

const InfoTextBold = styled.p`
  color: #151542;
  margin-top: 20px;
  font-size: 18px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 10px;

  @media (max-width: 768px) {
    margin-top: 10px;
    font-size: 16px;
  }
`;

const Row = styled.div`
  display: flex;
  flex-direction: column;
`;

const Column = styled.div`
  display: flex;
  flex-direction: column;
  width: 270px;

  @media (max-width: 768px) {
    align-items: flex-start;
  }
`;

const MainContainer = styled.div`
  display: flex;
  justify-content: space-around;
`;

const EditButton = styled.div`
  font-family: "Poppins", sans-serif;
  font-size: 16px;
  border: 1px solid #000;
  background-color: #fff;
  align-self: center;
  width: 100px;
  font-weight: 400;
  line-height: 1.5;
  text-align: center;
  padding: 6px;
  cursor: pointer;
  transition: background-color 0.3s ease, color 0.3s ease;

  &:hover {
    background-color: #102c57;
    color: #fff;
  }

  @media (max-width: 991px) {
    padding: 6px 20px;
    width: auto;
  }
`;
