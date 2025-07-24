
import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { Button, Table } from "antd";
import Sidebar from "@/components/Staff/SalesStaff/Sidebar/Sidebar";
import MarketingMenu from "@/components/Staff/SalesStaff/MarketingMenu/MarketingMenu";
import * as Styled from "./ProductPromotionDetail.styled";
import { showAllVoucher } from "@/services/voucherAPI";
import { showAllProduct } from "@/services/productAPI";

interface Voucher {
  id: string;
  name: string;
  discountValue: number;
  startDate: string;
  endDate: string;
  description: string;
  appliesToProductId: string;
}

interface Product {
  id: string;
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
  categoryId: string;
}

const ProductPromotionDetail = () => {
  const { id } = useParams<{ id: string }>();
  const [promotion, setPromotion] = useState<Voucher | null>(null);
  const [product, setProduct] = useState<Product | null>(null);
  const [loading, setLoading] = useState(true);

  const fetchData = async () => {
    try {
      const [voucherRes, productRes] = await Promise.all([
        showAllVoucher(),
        showAllProduct(),
      ]);

      console.log("🔥 useParams ID:", id);
      console.log("📦 Voucher response:", voucherRes);
      console.log("📦 Product response:", productRes);

      const voucherData: Voucher[] = voucherRes?.data ?? [];
      const productRaw = productRes?.data ?? [];

      // Kiểm tra nếu product trả về có .product thì map lại
      const productData: Product[] = productRaw[0]?.product
        ? productRaw.map((item: any) => item.product)
        : productRaw;

      const selectedVoucher = voucherData.find(
        (v) => v.id?.trim() === id?.trim()
      );

      console.log("🎯 Selected Voucher:", selectedVoucher);

      setPromotion(selectedVoucher || null);

      if (selectedVoucher) {
        const matchedProduct = productData.find(
          (p: any) => p.id?.trim() === selectedVoucher.appliesToProductId?.trim()
        );
        console.log("🎯 Matched Product:", matchedProduct);
        setProduct(matchedProduct || null);
      }

      setLoading(false);
    } catch (error) {
      console.error("❌ Error fetching data:", error);
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, [id]);

  if (loading) {
    return <p>Loading...</p>;
  }

  if (!promotion) {
    return <p>Promotion not found.</p>;
  }

return (
  <>
    <Styled.GlobalStyle />
    <Styled.PageAdminArea>
      <Sidebar />
      <Styled.AdminPage>
        <MarketingMenu />
        <Styled.PageContent>
          {/* <Styled.PageDetail_Title>
            <p>Product Promotion Detail</p>
          </Styled.PageDetail_Title> */}
         <Styled.PageDetail_Title>
          <span className="icon">💎</span>
          <span className="text">Product Promotion Detail</span>
        </Styled.PageDetail_Title>

          <Styled.PageDetail_Infor>
            <Styled.InforLine>
              <p className="InforLine_Title">Promotion ID</p>
              <p>{promotion.id}</p>
            </Styled.InforLine>
            <Styled.InforLine>
              <p className="InforLine_Title">Promotion Name</p>
              <p>{promotion.name}</p>
            </Styled.InforLine>
            <Styled.InforLine>
              <p className="InforLine_Title">% discount</p>
              <p>{promotion.discountValue}%</p>
            </Styled.InforLine>
            <Styled.InforLine>
              <p className="InforLine_Title">Start Date</p>
              <p>{promotion.startDate}</p>
            </Styled.InforLine>
            <Styled.InforLine>
              <p className="InforLine_Title">End Date</p>
              <p>{promotion.endDate}</p>
            </Styled.InforLine>
            <Styled.InforLine>
              <p className="InforLine_Title">Description</p>
              <p>{promotion.description}</p>
            </Styled.InforLine>
          </Styled.PageDetail_Infor>

          <Styled.PageDetail_Title>
            <p>Product Applied</p>
          </Styled.PageDetail_Title>

          {product ? (
            <Table
              dataSource={[product]}
              pagination={false}
              rowKey="id"
              bordered
              columns={[
                { title: "Name", dataIndex: "name" },
                { title: "SKU", dataIndex: "sku" },
                { title: "Description", dataIndex: "description" },
                { title: "Price", dataIndex: "price" },
                { title: "Carat", dataIndex: "carat" },
                { title: "Color", dataIndex: "color" },
                { title: "Clarity", dataIndex: "clarity" },
                { title: "Cut", dataIndex: "cut" },
                { title: "Stock Quantity", dataIndex: "stockQuantity" },
                { title: "GIA Cert Number", dataIndex: "giaCertNumber" },
              ]}
            />
          ) : (
            <p>No product found for this promotion.</p>
          )}

          <Styled.ActionBtn>
            <Link to="/sales-staff/marketing/discount">
              <Button>Back</Button>
            </Link>
          </Styled.ActionBtn>
        </Styled.PageContent>
      </Styled.AdminPage>
    </Styled.PageAdminArea>
  </>
);
};

export default ProductPromotionDetail;
