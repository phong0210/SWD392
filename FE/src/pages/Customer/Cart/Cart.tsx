import { Link, useNavigate } from "react-router-dom";
import * as CartStyled from "./Cart.styled";
import PromoCodeSection from "../../../components/Customer/Checkout/PromoCode";
import { useAppDispatch, useDocumentTitle, useAppSelector } from "@/hooks";
import { useState } from "react";
import CartItem from "@/components/Customer/Cart/CartItem";
import config from "@/config";
import { Empty, notification } from "antd";
import { removeFromCart, clearCart } from "@/store/slices/cartSlice";

const Cart = () => {
  useDocumentTitle("Cart | Aphromas Diamond");

  const navigate = useNavigate();
  const [api, contextHolder] = notification.useNotification();
  const dispatch = useAppDispatch();
  const cartItems = useAppSelector((state) => state.cart.items);

  const [discount, setDiscount] = useState(0);
  const onApplyVoucher = (discount: number, voucherID: number) => {
    setDiscount(discount);
    localStorage.setItem(
      "selectedVoucher",
      JSON.stringify({ discount, voucherID })
    );
  };

  const calculateTotal = (
    subtotal: number,
    discount: number,
    shippingCost: number
  ) => {
    return subtotal - (subtotal * discount) / 100 + shippingCost;
  };

  const subtotal = cartItems.reduce((acc, item) => {
    return acc + item.price * item.quantity;
  }, 0);

  const shippingCost = cartItems.length > 0 ? 15 : 0;
  const total = calculateTotal(subtotal, discount, shippingCost).toFixed(2);

  const handleRemove = (itemId: string) => {
    dispatch(removeFromCart(itemId));
    api.success({
      message: "Notification",
      description: "Remove product successfully!",
    });
  };

  const handleView = (itemId: string) => {
    const item = cartItems.find((item) => item.id === itemId);
    if (item) {
      if (item.productId) {
        navigate(
          config.routes.public.productDetail.replace(":id", item.productId)
        );
      } else if (item.diamondId) {
        navigate(
          config.routes.public.diamondDetail.replace(":id", item.diamondId)
        );
      }
    }
  };

  const handleCheckout = () => {
    if (cartItems.length === 0) {
      api.warning({
        message: "Notification",
        description: "Your cart doesn't have any products!",
      });
    } else {
      navigate(config.routes.customer.checkout);
    }
  };

  // const handleClearCart = () => {
  //     dispatch(clearCart());
  //     api.success({
  //         message: 'Notification',
  //         description: 'Cart cleared successfully!'
  //     });
  // };

  return (
    <>
      <main>
        {contextHolder}
        <CartStyled.ContainerHeader>
          <CartStyled.Header>Shopping Cart</CartStyled.Header>
        </CartStyled.ContainerHeader>
        <CartStyled.Container>
          <CartStyled.InnerContainer>
            <CartStyled.ContinueShopping>
              <span>
                <i className="fa-solid fa-chevron-up fa-rotate-270"></i>
              </span>
              <Link to={"/all"}>Continue Shopping</Link>
            </CartStyled.ContinueShopping>
            <CartStyled.CountCart>
              MY CART {cartItems.length} ITEMS
            </CartStyled.CountCart>

            <CartStyled.MainSection>
              <CartStyled.Column>
                {cartItems.length === 0 ? (
                  <Empty description="Your cart doesn't have anything here" />
                ) : (
                  <>
                    {cartItems.map((item) => (
                      <CartItem
                        key={item.id}
                        id={item.id}
                        name={item.name}
                        price={item.price}
                        quantity={item.quantity}
                        image={item.image}
                        handleRemove={() => handleRemove(item.id)}
                        handleView={() => handleView(item.id)}
                      />
                    ))}
                  </>
                )}
              </CartStyled.Column>

              <CartStyled.Sidebar>
                <CartStyled.SummaryContainer>
                  <CartStyled.SummaryDetails>
                    {" "}
                    {discount > 0 && (
                      <CartStyled.AppliedPromo>
                        Discount {`(-${discount}%)`}:
                        <CartStyled.AppliedPromoValuve>
                          {`-${(subtotal * discount) / 100}`}
                        </CartStyled.AppliedPromoValuve>
                      </CartStyled.AppliedPromo>
                    )}
                    <CartStyled.SummaryRow>
                      {cartItems.length === 0 ? (
                        <></>
                      ) : (
                        <>
                          <CartStyled.SummaryLabel>
                            Shipping
                          </CartStyled.SummaryLabel>
                          <CartStyled.SummaryValue>
                            {shippingCost > 0
                              ? `${shippingCost.toFixed(2)}`
                              : "Free"}
                          </CartStyled.SummaryValue>
                        </>
                      )}
                    </CartStyled.SummaryRow>
                    <CartStyled.SummaryRow>
                      <CartStyled.SummaryLabel>
                        Subtotal
                      </CartStyled.SummaryLabel>
                      <CartStyled.SummaryValue>
                        ${subtotal.toFixed(2)}
                      </CartStyled.SummaryValue>
                    </CartStyled.SummaryRow>
                    <PromoCodeSection onApplyVoucher={onApplyVoucher} />
                    <CartStyled.SummaryTotal>
                      <CartStyled.TotalLabel>Total</CartStyled.TotalLabel>
                      <CartStyled.TotalValue>${total}</CartStyled.TotalValue>
                    </CartStyled.SummaryTotal>
                  </CartStyled.SummaryDetails>
                  <CartStyled.CheckoutButton onClick={handleCheckout}>
                    CHECKOUT
                  </CartStyled.CheckoutButton>
                  {/* <CartStyled.OrDivider>OR</CartStyled.OrDivider>
                  <Link to="/thanks-page">
                    <CartStyled.PaymentMethodImage
                      src="https://firebasestorage.googleapis.com/v0/b/testsaveimage-abb59.appspot.com/o/Customer%2FCart%2Fvnpay-logo-vinadesign-25-12-57-55.jpg?alt=media&token=5c8bd77d-6a86-478e-83d7-44d4e1227e5c"
                      alt="Credit card icons"
                    />
                  </Link>
                  <Link to="thanks-page">
                    <CartStyled.PaymentMethodImage
                      src="https://firebasestorage.googleapis.com/v0/b/testsaveimage-abb59.appspot.com/o/Customer%2FCart%2Fimage%2022%20(1).png?alt=media&token=086cc881-2091-4405-8a2e-6fc25d6e6c77"
                      alt="Credit card icons"
                    />
                  </Link> */}
                </CartStyled.SummaryContainer>
              </CartStyled.Sidebar>
            </CartStyled.MainSection>

            <CartStyled.ShippingSection>
              <CartStyled.ShippingIcon
                src="https://cdn.builder.io/api/v1/image/assets/TEMP/6933788e6c8896639db19bae2d37194ec1e54bd5cf3292e8cc54f2247afd9959?apiKey=5672b1354002436f9bda9e8bc0a69a3b&"
                alt="Shipping icon"
              />
              <CartStyled.ShippingText>Shipping</CartStyled.ShippingText>
              <CartStyled.ShippingDetails>
                Free Shipping Worldwide <br />
                Overnight Shipping
              </CartStyled.ShippingDetails>
            </CartStyled.ShippingSection>
          </CartStyled.InnerContainer>
        </CartStyled.Container>
      </main>
    </>
  );
};

export default Cart;
