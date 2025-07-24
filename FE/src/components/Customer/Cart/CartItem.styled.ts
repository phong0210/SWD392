import { theme } from "@/themes";
import styled from "styled-components";

export const ItemContainer = styled.div`
  background-color: #fff;
  border: 1px solid rgb(232, 226, 226);
  border-radius: 8px;
  box-shadow: rgba(27, 27, 27, 0.17) 0px 2px 5px;
  display: flex;
  flex-direction: column;
  padding: 16px;
  margin: 16px 0;
  transition: box-shadow 0.2s ease;

  &:hover {
    box-shadow: rgba(27, 27, 27, 0.25) 0px 4px 8px;
  }

  @media (max-width: 768px) {
    padding: 12px;
    margin: 12px 0;
  }
`;

export const ItemDetails = styled.div`
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: space-between;
  gap: 16px;
  padding: 0;

  @media (max-width: 768px) {
    flex-direction: column;
    align-items: flex-start;
  }
`;

export const ItemInfo = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
`;

export const ItemImage = styled.img`
  width: 120px;
  height: 120px;
  object-fit: contain;
  border-radius: 6px;
  background-color: #f9f9f9; /* Light background for image placeholder */
  transition: transform 0.2s ease;

  &:hover {
    transform: scale(1.05);
  }

  @media (max-width: 768px) {
    width: 100px;
    height: 100px;
  }
`;

export const ItemDescription = styled.div`
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 8px;

  @media (max-width: 768px) {
    width: 100%;
  }
`;

export const ProductDescription = styled.div`
  display: flex;
  flex-direction: column;
  gap: 8px;
  font-family: Poppins, sans-serif;
  letter-spacing: 0.15px;
`;

export const ItemType = styled.div`
  font-size: 15px;
  font-weight: 600;
  line-height: 1.5;
  color: #000;

  @media (max-width: 768px) {
    font-size: 14px;
  }
`;

export const Description = styled.div`
  font-size: 13px;
  font-weight: 400;
  color: #000;
  line-height: 1.5;

  @media (max-width: 768px) {
    font-size: 12px;
  }
`;

export const ItemPrice = styled.div`
  font-size: 19px;
  font-weight: 600;
  color: ${theme.color.primary};
  line-height: 1.5;
  text-align: right;
  letter-spacing: 0.6px;

  @media (max-width: 768px) {
    font-size: 14px;
    text-align: left;
    margin-top: 8px;
  }
`;

export const ActionText = styled.div`
  display: flex;
  justify-content: flex-end;
  gap: 8px;
  margin-top: auto;

  button {
    font-size: 13px;
    font-weight: 300;
    letter-spacing: 1.95px;
    color: #000;
    padding: 4px 12px;
    border-radius: 6px;
    transition: background-color 0.2s ease, color 0.2s ease;

    &:hover {
      background-color: #102c57;
      color: #fff;
    }
  }

  @media (max-width: 768px) {
    justify-content: flex-start;
  }
`;

export const TagContainer = styled.div`
  display: flex;
  gap: 8px;
  flex-wrap: wrap;
`;