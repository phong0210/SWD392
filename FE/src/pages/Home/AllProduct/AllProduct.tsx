import React, { useState, useEffect } from "react";
import {
  Section,
  Container,
  List,
  StyledPagination,
  CustomBreadcrumb,
  Banner,
  LeftSection,
} from "./AllProduct.styled";
import { Card, Col, Row, Typography, Spin } from "antd";
import { Link } from "react-router-dom";
import { showAllProduct } from "@/services/productAPI";
import { Product, ProductApiResponseItem } from "@/models/Entities/Product";
import defaultImage from "@/assets/diamond/defaultImage.png";

const { Title, Text } = Typography;

const AllProduct: React.FC = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(false);
  const [filteredProducts, setFilteredProducts] = useState<Product[]>([]);
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 8;

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await showAllProduct(); // Call the function to get the promise
        console.log("API response:", response.data);

        if (response && Array.isArray(response.data)) {
          // In your useEffect, update the fetchedProducts mapping:

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
          setLoading(false);
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
      (product) => product.isHidden === false // Chỉ hiển thị sản phẩm có isHidden = true
    );
    setFilteredProducts(filtered);
  }, [products]);

  const handleChangePage = (page: any) => {
    setCurrentPage(page);
  };

  if (loading) {
    return (
      <div
        style={{
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          height: "100vh",
          flexDirection: "column",
        }}
      >
        <Spin tip="Loading..." />
      </div>
    );
  }

  return (
    <Section>
      <div>
        <CustomBreadcrumb
          separator=">"
          items={[
            {
              title: "Home",
              href: "/",
            },
            {
              title: "All Product",
            },
          ]}
        />
      </div>
      <Container className="wide">
        <Banner>
          <div className="bannerContent">
            <LeftSection>
              <h2>FOR LIFE'S MOST JOYFUL OCCASIONS</h2>
              <h1>All Jewelry</h1>
              <div className="subheading">
                Welcome to our All Jewelry Collection, where timeless elegance
                meets exquisite craftsmanship. Whether you're searching for a
                stunning ring, a delicate bracelet, an elegant necklace, or
                dazzling earrings, our diverse selection has something for every
                taste and occasion. Each piece is meticulously crafted to
                enhance your unique style and make every moment memorable. From
                classic designs to contemporary masterpieces, explore our
                collection and find the perfect jewelry to elevate your look and
                express your individuality. Discover the beauty and
                sophistication that our jewelry brings, making every day a
                special occasion.
              </div>
            </LeftSection>
          </div>
        </Banner>
        <hr
          style={{
            maxWidth: "1400px",
            margin: "32px auto",
            border: "1px solid rgba(21, 21, 66, 0.3)",
          }}
        />
        <List>
          <Row gutter={[16, 16]}>
            {filteredProducts
              .slice((currentPage - 1) * pageSize, currentPage * pageSize)
              .map((product) => (
                <Col key={product.id} span={6}>
                  <Card
                    key={product.id}
                    style={{ borderRadius: "0" }}
                    hoverable
                    className="product-card"
                    cover={
                      <Link to={`/product/${product.id}`}>
                        <img
                          style={{ borderRadius: "0" }}
                          src={defaultImage}
                          alt={product.name}
                          className="product-image"
                          onMouseOver={(e) =>
                            (e.currentTarget.src = defaultImage)
                          }
                          onMouseOut={(e) =>
                            (e.currentTarget.src = defaultImage)
                          }
                        />
                        {product.salePrice && (
                          <div className="sale-badge">SALE</div>
                        )}
                      </Link>
                    }
                  >
                    <div className="product-info">
                      <Title level={4} className="product-name">
                        <Link to={`/product/${product.id}`}>
                          <div>{product.name}</div>
                        </Link>
                      </Title>
                      <div className="price-container">
                        <Text className="product-price">${product.price}</Text>
                      </div>
                    </div>
                  </Card>
                </Col>
              ))}
          </Row>
        </List>
        <StyledPagination
          current={currentPage}
          pageSize={pageSize}
          total={filteredProducts.length}
          onChange={handleChangePage}
        />
      </Container>
    </Section>
  );
};

export default AllProduct;
