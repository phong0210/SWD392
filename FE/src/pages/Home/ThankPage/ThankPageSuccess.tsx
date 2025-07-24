import React, { useEffect } from "react";
import { Row, Col, Spin, Result } from "antd";
import {
  CheckCircleFilled,
  ContainerFilled,
  BellFilled,
  EyeFilled,
} from "@ant-design/icons";
import { Container } from "./ThankPage.styled";
import { Link, useNavigate, useLocation } from "react-router-dom";
import config from "@/config";
import { useAppSelector, useAppDispatch } from "@/hooks";
import {
  captureOrderPaypalAsync,
  handleVNPayReturnAsync,
  resetOrderStatus,
} from "@/store/slices/orderSlice";

const ThankPageSuccess: React.FC = () => {
  const { order, status, error } = useAppSelector((state) => state.order);
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
  const location = useLocation();

  useEffect(() => {
    console.log("[ThankPageSuccess] Current order state:", {
      order,
      status,
      error,
    });
    const params = new URLSearchParams(location.search);
    const token = params.get("token"); // PayPal token
    const vnpResponseCode = params.get("vnp_ResponseCode"); // VNPay response code

    if (token && status !== "loading") {
      console.log(
        "[ThankPageSuccess] Capturing PayPal payment with token:",
        token
      );
      dispatch(captureOrderPaypalAsync(token));
    } else if (vnpResponseCode && status !== "loading") {
      console.log(
        "[ThankPageSuccess] Handling VNPay return with responseCode:",
        vnpResponseCode
      );
      dispatch(handleVNPayReturnAsync(params));
    }

    return () => {
      console.log(
        "[ThankPageSuccess] Cleaning up, removing CurrentOrderID from localStorage"
      );
      localStorage.removeItem("CurrentOrderID");
      dispatch(resetOrderStatus());
    };
  }, [location.search, dispatch]);

  // Loading state
  if (status === "loading") {
    console.log("[ThankPageSuccess] Rendering loading state");
    return (
      <Container>
        <div
          className="thank-page-success-container"
          style={{
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            height: "100vh",
          }}
        >
          <Spin size="large" />
          <p style={{ marginLeft: "20px", fontSize: "18px" }}>
            Finalizing your order...
          </p>
        </div>
      </Container>
    );
  }

  // Failure state
  if (status === "failed" || !order || !order.id) {
    console.log(
      "[ThankPageSuccess] Rendering failure state, error:",
      error,
      "order:",
      order
    );
    return (
      <Container>
        <Result
          status="error"
          title="Order Processing Failed"
          subTitle={
            error ||
            "Sorry, there was a problem processing your order. Please try again."
          }
          extra={[
            <Link to={config.routes.public.home} key="home">
              <button className="home">Go to Homepage</button>
            </Link>,
            <Link to={config.routes.customer.cart} key="cart">
              <button className="cart">Back to Cart</button>
            </Link>,
          ]}
        />
      </Container>
    );
  }

  // Success state
  console.log(
    "[ThankPageSuccess] Rendering success state for order:",
    order.id
  );
  return (
    <Container>
      <div className="thank-page-success-container">
        <div className="thank-page-success-box">
          <CheckCircleFilled className="thank-page-success-icon" />
          <h1>ORDER SUCCESS!</h1>
          <img
            src="https://firebasestorage.googleapis.com/v0/b/testsaveimage-abb59.appspot.com/o/ThankPage%2Fcoworking-lettering-thank-you-with-decorative-elements-text.png?alt=media&token=c0d352fe-d89e-43e0-a7ee-e02306ccf231"
            alt="Success"
            className="thank-page-success-image"
          />
          <hr className="thank-page-success-hr" />
          <div className="thank-page-summary-next-container">
            <Row className="thank-page-summary-next-row">
              <Col span={12} className="thank-page-summary-col">
                <h4 className="title">Your Summary</h4>
                <div className="thank-page-success-summary-box">
                  <Row>
                    <Col span={6} className="thank-page-summary-icon-col">
                      <div className="thank-page-summary-icon-box">
                        <ContainerFilled className="thank-page-icon" />
                      </div>
                    </Col>
                    <Col span={18} className="thank-page-summary-details">
                      <div className="content main">
                        <p className="label">ORDER ID </p>
                        <p className="info">{order.id}</p>
                      </div>

                      <div className="content">
                        <p className="label">DATE</p>
                        <p className="info">
                          {new Date(order.orderDate).toLocaleString()}
                        </p>
                      </div>

                      <div className="content">
                        <p className="label">CUSTOMER</p>
                        <p className="info">{order.userId}</p>
                      </div>

                      <div className="content">
                        <p className="label">AMOUNT</p>
                        <p className="info">${order.totalPrice.toFixed(2)}</p>
                      </div>

                      <div className="content end">
                        <p className="label">PAYMENT METHOD</p>
                        <p className="info">
                          {order.payments?.[0]?.method || "COD"}
                        </p>
                      </div>
                    </Col>
                  </Row>
                </div>
              </Col>
              <Col span={12} className="thank-page-next-col">
                <h4 className="title">What’s Next</h4>
                <div className="thank-page-success-next-box">
                  <div className="thank-page-success-next-box-item item-1">
                    <Row className="thank-page-row">
                      <Col span={6} className="thank-page-next-icon-col">
                        <div className="thank-page-next-icon-box">
                          <BellFilled className="thank-page-icon" />
                        </div>
                      </Col>
                      <Col span={18} className="thank-page-next-details">
                        <p className="label-check">CHECK NOTIFICATIONS</p>
                        <p className="info-check">
                          We will update you on the status of your products and
                          orders through the Notifications section on our
                          website, so please stay tuned!
                        </p>
                      </Col>
                    </Row>
                  </div>
                  <div className="thank-page-success-next-box-item item-2">
                    <Row className="thank-page-row">
                      <Col span={6} className="thank-page-next-icon-col">
                        <div className="thank-page-next-icon-box">
                          <EyeFilled className="thank-page-icon" />
                        </div>
                      </Col>
                      <Col span={18} className="thank-page-next-details">
                        <p className="label-check">TRACK YOUR ORDER</p>
                        <p className="info-check">
                          You can track your orders in the My Orders section of
                          your Account! If you have any questions about your
                          orders, please contact us! It is our pleasure to
                          assist you.
                        </p>
                      </Col>
                    </Row>
                  </div>
                </div>
              </Col>
            </Row>
          </div>
          <div className="thank-page-success-buttons">
            <Link to={config.routes.public.home}>
              <button className="home">HOME</button>
            </Link>
            <button
              className="track"
              onClick={() => {
                console.log(
                  "[ThankPageSuccess] Navigating to order details for order:",
                  order.id
                );
                navigate(
                  `${config.routes.customer.orderDetails}?orderId=${order.id}`
                );
              }}
            >
              TRACK ORDER
            </button>
          </div>
        </div>
      </div>
    </Container>
  );
};

export default ThankPageSuccess;
