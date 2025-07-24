import React from "react";
import { Row, Col } from "antd";
import {
  CloseCircleFilled,
  ContainerFilled,
  WarningFilled,
} from "@ant-design/icons";
import { Container } from "./ThankPage.styled";
import { Link } from "react-router-dom";
import config from "@/config";
import { useAppSelector } from "@/hooks";

const ThankPageFail: React.FC = () => {
  const { order, error } = useAppSelector((state) => state.order);

  // Log the current order state and error
  console.log('[ThankPageFail] Current order state:', { order, error });

  // Log specific order details if available
  if (order) {
    console.log('[ThankPageFail] Order details:', {
      id: order.id,
      userId: order.userId,
      totalPrice: order.totalPrice,
      orderDate: order.orderDate,
      paymentMethod: order.payments?.[0]?.method || 'Unknown',
    });
  } else {
    console.warn('[ThankPageFail] No order data available');
  }

  return (
    <Container>
      <div className="thank-page-success-container">
        <div className="thank-page-success-box">
          <CloseCircleFilled className="thank-page-success-icon" />
          <h1>PAYMENT FAIL!</h1>
          <img
            src="https://firebasestorage.googleapis.com/v0/b/testsaveimage-abb59.appspot.com/o/ThankPage%2Fcoworking-exclamation-mark-sign.png?alt=media&token=5fae698c-ef58-4c45-98df-fc94fef10735"
            alt="fail"
            className="thank-page-fail-image"
          />
          <hr className="thank-page-success-hr" />
          <div className="thank-page-summary-next-container">
            <h4 className="title-fail">Payment Detail</h4>
            <Row className="thank-page-summary-next-row">
              <Col span={12} className="thank-page-summary-col">
                <div className="thank-page-success-summary-box">
                  <Row>
                    <Col span={6} className="thank-page-summary-icon-col">
                      <div className="thank-page-summary-icon-box">
                        <ContainerFilled className="thank-page-icon" />
                      </div>
                    </Col>
                    <Col span={18} className="thank-page-summary-details">
                      <div className="content main">
                        <p className="label">ORDER ID</p>
                        <p className="info">{order?.id || 'N/A'}</p>
                      </div>

                      <div className="content">
                        <p className="label">DATE</p>
                        <p className="info">
                          {order?.orderDate
                            ? new Date(order.orderDate).toLocaleString()
                            : 'N/A'}
                        </p>
                      </div>

                      <div className="content">
                        <p className="label">CUSTOMER</p>
                        <p className="info">{order?.userId || 'N/A'}</p>
                      </div>

                      <div className="content">
                        <p className="label">AMOUNT</p>
                        <p className="info">
                          {order?.totalPrice ? `$${order.totalPrice.toFixed(2)}` : 'N/A'}
                        </p>
                      </div>

                      <div className="content end">
                        <p className="label">PAYMENT METHOD</p>
                        <p className="info">{order?.payments?.[0]?.method || 'N/A'}</p>
                      </div>
                    </Col>
                  </Row>
                </div>
              </Col>
              <Col span={12} className="thank-page-next-col">
                <div className="thank-page-success-next-box">
                  <div className="thank-page-success-next-box-item item-1">
                    <Row className="thank-page-row">
                      <Col span={6} className="thank-page-next-icon-col">
                        <div className="thank-page-next-icon-box">
                          <WarningFilled className="thank-page-icon" />
                        </div>
                      </Col>
                      <Col span={18} className="thank-page-next-details">
                        <p className="label-check">OOPS...</p>
                        <p className="info-check">
                          {error || "There are some unexpected issues occurring, and we hope they do not affect your shopping experience too much. This situation will be resolved soon, so please try again or try again later! If you have any questions, please contact us. It is our pleasure to assist you."}
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
            <Link to={config.routes.customer.cart}>
              <button className="cart">TRY AGAIN!</button>
            </Link>
          </div>
        </div>
      </div>
    </Container>
  );
};

export default ThankPageFail;