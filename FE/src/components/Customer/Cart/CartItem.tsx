/* eslint-disable @typescript-eslint/no-explicit-any */
import { Button, Flex, Tag, InputNumber } from 'antd';
import { useNavigate } from "react-router-dom";
import config from "@/config";
import * as Styled from './CartItem.styled';
import { useEffect, useState } from 'react';
import { getDiamondDetails } from '@/services/diamondAPI';
import { getProductDetails } from '@/services/productAPI';
import { getImage } from '@/services/imageAPI';
import { CartItem as CartItemType } from '@/services/cartAPI';
import { useAppDispatch } from '@/hooks';
import { updateCartItemQuantity } from '@/store/slices/cartSlice';

type CartItemProps = CartItemType & {
  handleRemove?: () => void;
};

const QuantityControl = ({ value, onChange }: { value: number, onChange: (value: number) => void }) => {
  return (
    <Flex align="center" gap="small">
      <Button size="small" onClick={() => onChange(value - 1)} disabled={value <= 1}>-</Button>
      <InputNumber
        min={1}
        value={value}
        onChange={(val) => onChange(val || 1)}
        style={{ width: '48px', textAlign: 'center' }}
        size="small"
      />
      <Button size="small" onClick={() => onChange(value + 1)}>+</Button>
    </Flex>
  );
};

const CartItem = ({ id, productId, diamondId, name, price, quantity, image, handleRemove }: CartItemProps) => {
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
  const [details, setDetails] = useState<any>(null);

  useEffect(() => {
    const fetchDetails = async () => {
      try {
        if (productId) {
          const { data } = await getProductDetails(productId);
          setDetails(data.product);
        } else if (diamondId) {
          const { data } = await getDiamondDetails(diamondId);
          setDetails(data.data);
        }
      } catch (error) {
        console.error("Failed to fetch item details:", error);
      }
    };
    fetchDetails();
  }, [productId, diamondId]);

  const handleQuantityChange = (newQuantity: number) => {
    if (newQuantity > 0) {
      dispatch(updateCartItemQuantity({ id, quantity: newQuantity }));
    }
  };

  const handleView = () => {
    if (productId) {
      navigate(config.routes.public.productDetail.replace(':id', productId));
    } else if (diamondId) {
      navigate(config.routes.public.diamondDetail.replace(':id', diamondId));
    }
  };

  const renderTags = () => {
    if (!details) return null;

    if (diamondId) {
      return (
        <Styled.TagContainer>
          {[details.Clarity, details.Color, details.Cut, details.WeightCarat].map((prop, index) => (
            <Tag key={index} bordered={false} color="processing">
              {prop}
            </Tag>
          ))}
        </Styled.TagContainer>
      );
    }

    if (productId) {
      return (
        <Styled.TagContainer>
          <Tag bordered={false} color="processing">{details.Brand}</Tag>
        </Styled.TagContainer>
      );
    }

    return null;
  };

  const displayName = details?.Name || name;
  const displayDesigner = details?.Brand || (details ? 'Aphromas' : '');
  const displayImage = details?.UsingImage?.[0]?.UsingImageID ? getImage(details.UsingImage[0].UsingImageID) : image;

  return (
    <Styled.ItemContainer>
      <Styled.ItemDetails>
        <Styled.ItemInfo>
          <Styled.ItemImage src={displayImage} alt={displayName} onError={(e) => (e.currentTarget.src = '/fallback-image.png')} />
        </Styled.ItemInfo>
        <Styled.ItemDescription>
          <Styled.ProductDescription>
            <Styled.ItemType>{displayName}</Styled.ItemType>
            {displayDesigner && <Styled.Description>By {displayDesigner}</Styled.Description>}
            {renderTags()}
          </Styled.ProductDescription>
        </Styled.ItemDescription>
        <Flex vertical align="flex-end" gap="8px">
          <QuantityControl value={quantity} onChange={handleQuantityChange} />
          <Styled.ItemPrice>${(price * quantity).toFixed(2)}</Styled.ItemPrice>
        </Flex>
      </Styled.ItemDetails>
      <Styled.ActionText>
        <Button type="text" onClick={handleView}>VIEW</Button>
        <Button type="text" onClick={handleRemove}>REMOVE</Button>
      </Styled.ActionText>
    </Styled.ItemContainer>
  );
};

export default CartItem;