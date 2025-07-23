import { UUID } from "crypto";

// export interface Product {
//   id: UUID;
//   name: string;
//   sku: string;
//   description: string;
//   price: number;
//   carat: number;
//   color: string;
//   clarity: string;
//   cut: string;
//   stockQuantity: number;
//   giaCertNumber: string;
//   isHidden: boolean;
//   categoryId: number;
//   orderDetailId: number;
//   warrantyId: number;
//   salePrice?: number;
//   firstPrice?: number;
//   totalDiamondPrice?: number;
//   star?: number;
//   type?: string;
//   images: { url: string }[];
// }
export interface Product {
  id: string; // UUID
  name: string;
  sku: string;
  description: string;
  price: number;
  carat: number;
  color: string;
  clarity: string;
  cut: string;
  stockQuantity: number;
  giaCertNumber: string;
  isHidden: boolean;
  categoryId: number;
  images: { url: string }[];
}

export interface ProductApiResponseItem {
  success: boolean;
  error: string | null;
  product: Product;
}
