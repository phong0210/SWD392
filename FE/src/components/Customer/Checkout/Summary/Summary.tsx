/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import * as React from "react";
import styled from "styled-components";
import { Link } from "react-router-dom";
import { useState } from "react";
import PromoCodeSection from "../../../Customer/Checkout/PromoCode";
import { useAppDispatch } from "@/hooks";
import { orderSlice } from "@/layouts/MainLayout/slice/orderSlice";
import { CartItem as CartItemType } from "@/services/cartAPI";

interface CartItemProps {
  name: string;
  image: string;
  sku?: string;
  price: number;
}

const CartItem: React.FC<CartItemProps> = ({ name, image, sku, price }) => (
  <CartItemContainer>
    <div>
      <ImageContainer>
        <img src={image} alt="Image" />
      </ImageContainer>
    </div>
    <ItemInfo>
      <ItemName>{name}</ItemName>
      <ItemSku>{sku}</ItemSku>
    </ItemInfo>
    <ItemPrice>${price.toFixed(2)}</ItemPrice>
  </CartItemContainer>
);

interface SummaryProps {
  cartItems: CartItemType[];
}

const Summary: React.FC<SummaryProps> = ({ cartItems }) => {
  const [discount, setDiscount] = useState(0);
  const [voucherID, setVoucherID] = useState<number | undefined>(undefined);
  const dispatch = useAppDispatch();

  const onApplyVoucher = (discount: number, voucherID: number) => {
    setDiscount(discount);
    setVoucherID(voucherID);
    localStorage.setItem("selectedVoucher", JSON.stringify({ discount, voucherID }));
  };

  const calculateTotal = (
    subtotal: number,
    discount: number,
    shippingCost: number
  ) => {
    return subtotal - (subtotal * discount) / 100 + shippingCost;
  };

  const shippingCost = cartItems.length === 1 ? 15 : 0;
  dispatch(orderSlice.actions.setShippingfee(shippingCost));

  const subtotalNumber = cartItems.reduce((acc, item) => {
    return acc + item.price * item.quantity;
  }, 0);

  const total = calculateTotal(subtotalNumber, discount, shippingCost).toFixed(2);
  dispatch(orderSlice.actions.setTotal(Number(total)));

  return (
    <SummarySection>
      <ItemNumber>
        <NumberItem>{cartItems.length} ITEMS</NumberItem>
        <Link to="/cart">
          <p
            style={{
              cursor: "pointer",
              fontSize: "13px",
              fontFamily: "Poppins, sans-serif",
            }}
          >
            EDIT CART
          </p>
        </Link>
      </ItemNumber>
      {cartItems.map((item) => (
        <CartItem
          key={item.id}
          name={item.name}
          image={item.image}
          price={item.price}
        />
      ))}
      <EditTotal>
        {" "}
        {discount > 0 && (
          <>
            <p>Discount {`(${discount}%)`}: </p>
            <p>{`-$${(subtotalNumber * discount / 100).toFixed(2)}`}</p>
          </>
        )}
      </EditTotal>
      <EditTotal>
        <p>Shipping: </p>
        <p>{cartItems.length === 1 ? "$15.00" : "Free"}</p>
      </EditTotal>
      <EditTotal>
        <p>Subtotal: </p>
        <p>${subtotalNumber.toFixed(2)}</p>
      </EditTotal>

      <PromoCodeSection onApplyVoucher={onApplyVoucher} />
      <EditTotal1>
        <p>Total:</p>
        <p> ${total}</p>
      </EditTotal1>
    </SummarySection>
  );
};

export default Summary;

const SummarySection = styled.section`
  display: flex;
  flex: 1;
  line-height: 53px;
  flex-direction: column;
  padding: 35px 20px;
  border: 1px solid #ddd;
`;

const ItemNumber = styled.div`
  display: flex;
  justify-content: space-between;
  /* margin-bottom: 20px; */
`;

const NumberItem = styled.p`
  font-size: 13px;
  font-family: Poppins, sans-serif;
`;

const EditTotal = styled.div`
  display: flex;
  justify-content: space-between;
  font-size: 17px;
`;

const EditTotal1 = styled(EditTotal)`
  font-size: 17px;
  font-weight: bold;
  word-spacing: 313px;
`;

const CartItemContainer = styled.div`
  display: flex;
  align-items: center;
  gap: 20px;
  /* padding-top: 18px; */
  //   margin-top: 10px; */
  border-bottom: 2px solid #e8e2e2;
  img {
    max-width: 100px;
  }
  /* div {
     flex: 1;
  } */
  h3 {
    margin: 0;
  }
  p {
    margin: 0;
  }
`;

const ImageContainer = styled.div`
  width: 80px;
  height: 80px;
  img {
    width: 100%;
    height: 100%;
  }
`;

const ItemInfo = styled.div`
  flex: 1;
`;

const ItemName = styled.p`
  /* font-weight: bold; */
  font-size: 17px;
`;

const ItemSku = styled.p`
  font-size: 17px;
`;

const ItemPrice = styled.p`
  color: #333;
  font-size: 17px;
`;