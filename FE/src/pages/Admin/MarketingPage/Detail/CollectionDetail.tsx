import * as Styled from "./CollectionDetail.styled";
import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import Sidebar from "@/components/Admin/Sidebar/Sidebar";
import MarketingMenu from "@/components/Admin/MarketingMenu/MarketingMenu";
import { Button, TableColumnsType, Popconfirm, Table, Space } from "antd";
import { Product, ProductApiResponseItem } from "@/models/Entities/Product";
import { showAllProduct } from "@/services/productAPI";
import { EyeOutlined } from "@ant-design/icons";
import defaultImage from "@/assets/diamond/defaultImage.png";

const CollectionDetail = () => {
  const { id } = useParams<{ id: string }>();
  const [products, setProducts] = useState<Product[]>([]);
  const [filteredProducts, setFilteredProducts] = useState<Product[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await showAllProduct();
        console.log("API response:", response.data);

        if (response && Array.isArray(response.data)) {
          const fetchedProducts = (
            response.data as ProductApiResponseItem[]
          ).map((item) => {
            const product = item.product;
            return {
              id: product.id,
              name: product.name,
              sku: product.sku,
              description: product.description,
              price: product.price,
              carat: product.carat,
              color: product.color,
              clarity: product.clarity,
              cut: product.cut,
              stockQuantity: product.stockQuantity,
              giaCertNumber: product.giaCertNumber,
              isHidden: product.isHidden,
              categoryId: product.categoryId,
              orderDetailId: product.orderDetailId,
              warrantyId: product.warrantyId,
              salePrice: product.salePrice,
              firstPrice: product.firstPrice,
              totalDiamondPrice: product.totalDiamondPrice,
              star: product.star,
              type: product.type,
              images: Array.isArray(product.images) ? product.images : [],
            };
          });
          console.log(fetchedProducts);
          setProducts(fetchedProducts);
        } else {
          console.error("Unexpected API response format:", response.data);
        }
      } catch (error) {
        console.error("Error fetching diamonds:", error);
      }
    };
    fetchData();
  }, []);

  useEffect(() => {
    const filtered = products.filter(
      (product) =>
        product.isHidden === false && product.categoryId?.toString() === id
    );
    console.log("Filtered products:", filtered);
    console.log("Category ID from params:", id);
    setFilteredProducts(filtered);
  }, [products, id]);

  const columns = [
    {
      title: "Product ID",
      dataIndex: "id",
      sorter: (a: any, b: any) => a.id.localeCompare(b.id),
    },
    {
      title: "Image",
      dataIndex: "images",
      render: (_: unknown, record: any) => {
        const imageUrl =
          record.images && record.images.length > 0
            ? record.images[0]
            : defaultImage;

        return (
          <img
            src={imageUrl}
            alt={record.name || "Product"}
            style={{
              width: "50px",
              height: "50px",
              objectFit: "cover",
              borderRadius: "4px",
            }}
            onError={(e) => {
              const target = e.target as HTMLImageElement;
              target.src = defaultImage;
            }}
          />
        );
      },
    },
    {
      title: "Product Name",
      dataIndex: "name",
      editable: true,
      sorter: (a: any, b: any) => a.name.length - b.name.length,
    },
    {
      title: "Category ID",
      dataIndex: "categoryId",
      render: (categoryId: any) => categoryId || "N/A",
    },
    {
      title: "Detail",
      key: "detail",
      className: "TextAlign",
      render: (_: unknown, record: any) => (
        <Space size="middle">
          <Link to={`/sales-staff/product/diamond/detail/${record.id}`}>
            <EyeOutlined />
          </Link>
        </Space>
      ),
    },
  ];

  return (
    <>
      <Styled.GlobalStyle />
      <Styled.PageAdminArea>
        <Sidebar />
        <Styled.AdminPage>
          <MarketingMenu />

          <Styled.PageContent>
            <Styled.PageContent_Bot>
              <Styled.PageDetail_Title>
                <p>Category Detail</p>
              </Styled.PageDetail_Title>

              <Styled.MaterialTable>
                <Table
                  dataSource={filteredProducts}
                  columns={columns}
                  rowClassName={() => "editable-row"}
                  bordered
                  pagination={false}
                />
              </Styled.MaterialTable>
            </Styled.PageContent_Bot>
          </Styled.PageContent>
        </Styled.AdminPage>
      </Styled.PageAdminArea>
    </>
  );
};

export default CollectionDetail;
