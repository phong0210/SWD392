/* eslint-disable @typescript-eslint/no-explicit-any */
import { Button, Flex, Tag } from 'antd';
import { useNavigate } from "react-router-dom";
import config from "@/config";
import * as Styled from './CartItem.styled';
import { useEffect, useState } from 'react';
import { getDiamondDetails } from '@/services/diamondAPI';
import { getProductDetails } from '@/services/productAPI';
import { getImage } from '@/services/imageAPI';
import { CartItem as CartItemType } from '@/services/cartAPI';

type CartItemProps = CartItemType & {
  handleRemove?: () => void;
};

const CartItem = ({ id, productId, diamondId, name, price, image, handleRemove }: CartItemProps) => {
  const navigate = useNavigate();
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
      return [details.Clarity, details.Color, details.Cut, details.WeightCarat].map((prop, index) => (
        <Tag key={index} bordered={false} color='processing'>
          {prop}
        </Tag>
      ));
    }
    
    if (productId) {
        // You can add specific tags for products here if needed
        return <Tag bordered={false} color='processing'>{details.Brand}</Tag>;
    }

    return null;
  };

  const displayName = details?.Name || name;
  const displayDesigner = details?.Brand || (details ? 'Aphromas' : '');
  const displayImage = details?.UsingImage?.[0]?.UsingImageID ? getImage(details.UsingImage[0].UsingImageID) : image;

  return (
    <Styled.ItemContainer>
      <Styled.ActionText>
        <Flex gap="small" wrap>
          <Button type="text" onClick={handleView}>VIEW</Button>
          <Button type="text" onClick={handleRemove}>REMOVE</Button>
        </Flex>
      </Styled.ActionText>

      <Styled.ItemDetails>
        <Styled.ItemInfo>
          <Styled.ItemImage src={displayImage} alt={displayName} />
        </Styled.ItemInfo>
        <Styled.ItemDescription>
          <Styled.ProductDescription>
            <Styled.ItemType>{displayName}</Styled.ItemType>
            {displayDesigner && <Styled.Description>By {displayDesigner}</Styled.Description>}
            <div>
              {renderTags()}
            </div>
          </Styled.ProductDescription>
        </Styled.ItemDescription>
        <Styled.ItemPrice>${price.toFixed(2)}</Styled.ItemPrice>
      </Styled.ItemDetails>
    </Styled.ItemContainer>
  );
};

export default CartItem;