import { useState, useEffect } from "react";
import { Link, useParams } from "react-router-dom";
import { CloseOutlined } from "@ant-design/icons";
import { Card, Col, Row, Typography, Rate, notification } from "antd";

const { Title, Text } = Typography;
import InscriptionModal from "@/components/InscriptionModal/InscriptionModal";
import {
  Body,
  Section,
  Container,
  Wrap,
  ProductDotGrid,
  Wrapper,
  ImageContainer,
  OuterThumb,
  OuterMain,
  MainImage,
  ProductDetail,
  Entry,
  Heading,
  ProductRating,
  ProductMetal,
  ProductInfo,
  RingSizeContainer,
  RingSize,
  RingSizeHelp,
  ProductPrice,
  ButtonContainer,
  Button,
  CurrentPrice,
  BeforePrice,
  Discount,
  Contain,
  Tabbed,
  ProductAbout,
  TextBlock,
  DotGrid,
  ListBlock,
  Review,
  ProductSection,
  ButtonAdd,
  Space,
  List,
  StyledPagination,
  Condition,
  CustomBreadcrumb,
} from "./ProductDetails.styled";
import defaultImage from "@/assets/diamond/defaultImage.png";
import { useNavigate } from "react-router-dom";
import { getProductDetails, showAllProduct } from "@/services/productAPI";
import { showAllFeedback } from "@/services/feedBackAPI";
import useAuth from "@/hooks/useAuth";
import config from "@/config";
import { createOrderLine, OrderLineBody } from "@/services/orderLineAPI";
import { useAppDispatch } from "@/hooks";
import { addToCart } from "@/store/slices/cartSlice";
import { Product } from "@/models/Entities/Product";
const ProductDetails: React.FC = () => {
  //tab description + cmt
  const [activeTab, setActiveTab] = useState("product-description");

  const showTab = (tabId: string) => {
    setActiveTab(tabId);
  };

  //Metal
  const metalData = [
    { id: 1, key: "yellow", label: "14k", type: "14K Yellow Gold" },
    { id: 2, key: "white", label: "14k", type: "14K White Gold" },
    { id: 3, key: "rose", label: "14k", type: "14K Rose Gold" },
    { id: 4, key: "platinum", label: "Pt", type: "Platinum" },
  ];
  const [selectedMetal, setSelectedMetal] = useState(1);
  const [metalType, setMetalType] = useState("");
  const [sizes, setSizes] = useState<any[]>([]);
  const [selectedSize, setSelectedSize] = useState<number | null>(null);

  const handleClick = (sizeId: number) => {
    setSelectedSize(sizeId);
  };

  //inscription
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [inscription, setInscription] = useState<string>("");
  const [resetModal, setResetModal] = useState<boolean>(false);
  const [reviewsData, setReviewsData] = useState<any[]>([]);
  const showModal = () => {
    setResetModal(false);
    setIsModalVisible(true);
  };

  const handleSave = (text: string) => {
    setInscription(text);
    setIsModalVisible(false);
  };

  const handleDelete = () => {
    setInscription("");
    setResetModal(true);
  };

  const handleClose = () => {
    setIsModalVisible(false);
  };

  //
  const navigate = useNavigate();

  //PARAM
  const { id } = useParams<{ id: string }>();
  const { role, user } = useAuth();
  const [foundProduct, setFoundProduct] = useState<any | null>(null);
  const [sameBrandProducts, setSameBrandProducts] = useState<Product[]>([]);
  const [productId, setProductId] = useState<number | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [jewelrySettingVariant, setJewelrySettingVariant] = useState(0);
  const [cartList, setCartList] = useState<any[]>([]);
  const [api, contextHolder] = notification.useNotification();
  const dispatch = useAppDispatch();

  const fetchFeedbackDetail = async (productId: number) => {
    try {
      console.log("Fetching feedback details for product ID:", productId);
      const response = await showAllFeedback(productId);
      if (response.status === 200) {
        setReviewsData(
          response.data.data.map((feedback: any) => ({
            name: feedback.account ? feedback.account.Name : "Anonymous",
            rating: feedback.Stars,
            date: new Date(feedback.CommentTime).toLocaleDateString(),
            highlight: "For AD",
            comment: feedback.Comment,
            productId: feedback.ProductID,
          }))
        );
        console.log("Review: ", reviewsData);
      } else {
        console.error("Error fetching feedback:", response.statusText);
      }
    } catch (error) {
      console.error("Failed to fetch feedback details:", error);
    } finally {
      setIsLoading(false);
    }
  };

  const fetchProductDetails = async () => {
    if (!id) {
      console.error("Missing product ID");
      setIsLoading(false);
      return;
    }

    try {
      console.log("Fetching diamond details...");
      const response = await getProductDetails(id);
      console.log("Response from getProductDetails:", response.data);

      const responseAll = await showAllProduct();

      if (response.status === 200) {
        const product = response.data;
        const foundProduct = product.product;
        setFoundProduct(foundProduct);
        console.log("Found product:", foundProduct);

        const diamondId = product.product.id;
        setProductId(diamondId);
        console.log("Diamond ID:", diamondId);

        // Lấy categoryId từ sản phẩm hiện tại
        const currentCategoryId = foundProduct.categoryId;
        console.log("Current Category ID:", currentCategoryId);

        // Lọc các sản phẩm có cùng categoryId (loại bỏ sản phẩm hiện tại)
        if (responseAll.status === 200 && currentCategoryId) {
          const allProducts = responseAll.data; // Tùy thuộc vào cấu trúc response
          console.log("All Product", allProducts);

          const relatedProducts = allProducts
            .filter((item) => item.success && item.product)
            .map((item) => item.product)
            .filter(
              (product) =>
                product.categoryId === currentCategoryId &&
                product.id !== diamondId
            );

          setSameBrandProducts(relatedProducts);
          console.log("Related products:", relatedProducts);
        }
      } else {
        setFoundProduct(null);
        setSameBrandProducts([]); // Reset related products nếu không tìm thấy sản phẩm
      }
    } catch (error) {
      console.error("Failed to fetch diamond details:", error);
      setFoundProduct(null);
      setSameBrandProducts([]); // Reset related products khi có lỗi
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchProductDetails();
  }, [id, productId]);

  // useEffect(() => {
  //   const fetchCart = async () => {
  //     const orderlines = await showAllOrderLineForAdmin();
  //     const cartItems = orderlines.data.data.filter(
  //       (item: any) =>
  //         item.OrderID === null && item.CustomerID === user?.CustomerID
  //     );
  //     setCartList(cartItems);
  //   };

  //   fetchCart();
  // }, [id]);

  const handleAddToCart = async () => {
    if (role) {
      try {
        const cartItem = {
          id: `product-${id}`,
          productId: id,
          name: foundProduct.Name,
          price: foundProduct.price,
          quantity: 1,
          image: defaultImage,
        };
        dispatch(addToCart(cartItem));
        api.success({
          message: "Notification",
          description: "Product added to cart successfully",
        });
        navigate(config.routes.customer.cart);
      } catch (error: any) {
        api.error({
          message: "Error",
          description: error.message || "An error occurred",
        });
      }
    } else {
      navigate(config.routes.public.login);
    }
  };

  const handleCheckout = async () => {
    if (role) {
      try {
        const cartItem = {
          id: `product-${id}`,
          productId: id,
          name: foundProduct.Name,
          price: foundProduct.price,
          quantity: 1,
          image: defaultImage,
        };
        dispatch(addToCart(cartItem));
        navigate(config.routes.customer.checkout);
      } catch (error: any) {
        api.error({
          message: "Error",
          description: error.message || "An error occurred",
        });
      }
    } else {
      navigate(config.routes.public.login);
    }
  };

  if (!foundProduct) {
    return <div>Product not found</div>;
  }

  // const thumbnailImages =
  //   foundProduct?.UsingImage?.map((img: any) => getImage(img.UsingImageID)) ||
  //   [];
  // const changeImage = (src: string, index: number) => {
  //   setMainImage(src);
  //   setSelectedThumb(index);
  // };

  const handleButtonClick = (id: any, type: any) => {
    setSelectedMetal(id);
    setMetalType(type);
    fetchProductDetails();
    const JewelrySettingVariantID = Number(
      foundProduct?.JewelrySetting?.jewelrySettingVariant?.find(
        (item: any) => item.MaterialJewelryID === Number(selectedMetal)
      )?.JewelrySettingVariantID
    );
    setJewelrySettingVariant(JewelrySettingVariantID);
  };

  const matchingReviews = reviewsData.filter(
    (review) => foundProduct && foundProduct.ProductID === review.productId
  );
  //Avg rating
  const totalReviews = matchingReviews.length;
  const totalRating = matchingReviews.reduce(
    (acc, curr) => acc + curr.rating,
    0
  );
  const averageRating = totalRating / totalReviews;
  const summaryRating =
    matchingReviews.length > 0 ? averageRating.toFixed(1) : "0.0";
  const reviewsCount = matchingReviews.length > 0 ? matchingReviews.length : 0;

  return (
    <Body>
      {contextHolder}
      <div>
        {/* <CustomBreadcrumb
          separator=">"
          items={[
            { title: "Home", href: "/" },
            { title: "All Product", href: "/all" },
            {
              title: `${foundProduct.JewelrySetting.jewelryType.Name} - #${foundProduct.ProductID}`,
            },
          ]}
        /> */}
      </div>
      <Section>
        <Container>
          <Wrap>
            <ProductDotGrid>
              <Wrapper>
                <ImageContainer>
                  <OuterThumb>
                    {/* <ThumbnailImage>
                      {thumbnailImages.map((src: any, index: any) => (
                        <Item
                          key={index}
                          className={selectedThumb === index ? "selected" : ""}
                          onClick={() => changeImage(src, index)}
                        >
                          <img src={src} alt={`Thumb ${index + 1}`} />
                        </Item>
                      ))}
                    </ThumbnailImage> */}
                  </OuterThumb>
                  <OuterMain>
                    <MainImage>
                      <img id="mainImage" src={defaultImage} alt="Main" />
                    </MainImage>
                  </OuterMain>
                </ImageContainer>
              </Wrapper>
            </ProductDotGrid>
            <ProductDetail>
              <Entry>
                <Heading>
                  <Title className="main-title">{foundProduct.Name}</Title>
                </Heading>
                <ProductInfo>
                  <div className="wrap">
                    <div className="info-box">{foundProduct.name}</div>
                    <div>
                      {foundProduct.name.includes("Men") ? (
                        <div className="info-box">
                          {
                            foundProduct?.JewelrySetting
                              ?.jewelrySettingVariant[0]?.materialJewelry.Name
                          }
                        </div>
                      ) : (
                        <div className="info-box">{foundProduct.cut}</div>
                      )}
                    </div>
                    <div className="info-box">{foundProduct.name}</div>
                  </div>
                </ProductInfo>
                {foundProduct.name !== "Men Engagement Ring" &&
                  foundProduct.name !== "Men Wedding Ring" && (
                    <ProductMetal>
                      <span className="fill">Metal Type: {metalType}</span>
                      <div className="wrap">
                        {metalData.map((metal) => (
                          <button
                            key={metal.id}
                            className={`metal-button ${metal.key} ${
                              selectedMetal === metal.id ? "selected" : ""
                            }`}
                            onClick={() =>
                              handleButtonClick(metal.id, metal.type)
                            }
                          >
                            <span>{metal.label}</span>
                          </button>
                        ))}
                      </div>
                    </ProductMetal>
                  )}

                <div>
                  <RingSizeContainer>
                    <RingSizeHelp href="/find-ring-size">
                      Ring size help
                    </RingSizeHelp>
                  </RingSizeContainer>
                  <div className="button-container">
                    {sizes.map((size) => (
                      <button
                        key={size.SizeValue}
                        className={`size-button ${
                          selectedSize === size.SizeID ? "selected" : ""
                        }`}
                        onClick={() => handleClick(size.SizeID)}
                      >
                        {parseInt(size.SizeValue)}
                      </button>
                    ))}
                  </div>
                </div>

                <ProductPrice>
                  <div className="product-group">
                    <div className="product-price">
                      <CurrentPrice>
                        ${foundProduct.price.toFixed(2)}
                      </CurrentPrice>
                      {foundProduct.FirstPrice && (
                        <div className="wrap">
                          <BeforePrice>${foundProduct.FinalPrice}</BeforePrice>
                          <Discount>
                            - {foundProduct.Discount?.PercentDiscounts}%
                          </Discount>
                        </div>
                      )}
                    </div>
                  </div>
                </ProductPrice>
              </Entry>
              <div className="outlet">
                <Condition>
                  <div className="payment-options-box">
                    <h3>Tip for Free Shipping:</h3>
                    <li>Free shipping on orders of 2 or more items</li>
                  </div>
                </Condition>
                <ButtonContainer>
                  <ButtonAdd className="add" onClick={handleAddToCart}>
                    ADD TO CART
                  </ButtonAdd>
                  <Button
                    className="checkout button_slide slide_right"
                    onClick={handleCheckout}
                  >
                    <span>CHECKOUT</span>
                  </Button>
                </ButtonContainer>
              </div>
            </ProductDetail>
          </Wrap>
        </Container>
      </Section>
      <Contain>
        <div className="tabbed">
          <Tabbed>
            <nav>
              <ul className="wrapper">
                <li
                  id="tab-product-description"
                  className={
                    activeTab === "product-description" ? "active-tab" : ""
                  }
                >
                  <a href="#0" onClick={() => showTab("product-description")}>
                    <span>Product detail</span>
                  </a>
                </li>
              </ul>
            </nav>
          </Tabbed>
          <ProductAbout
            id="product-description"
            className={activeTab === "product-description" ? "active" : "hide"}
          >
            {/* Product detail content */}
            <TextBlock>
              <h3>{foundProduct.Name}</h3>
              <p>{foundProduct.Description}</p>
              <p>{foundProduct.Description}</p>
            </TextBlock>
            <DotGrid>
              <div className="wrapper2">
                <ListBlock>
                  <h4>What is this?</h4>
                  <ul>
                    <li>Type: {foundProduct.name}</li>
                    {!foundProduct.name.includes("Men") && (
                      <li>Diamond Shape: {foundProduct.cut}</li>
                    )}
                    <li>
                      Quantity:{" "}
                      {foundProduct.TotalQuantityJewelrySettingVariants}
                    </li>
                    <li>Setting: {foundProduct.name}</li>
                    {foundProduct.Discount &&
                      foundProduct.Discount.DiscountID && (
                        <li>Promotion: {foundProduct.name}</li>
                      )}
                  </ul>
                </ListBlock>
                <ListBlock>
                  <h4>What makes our product unique?</h4>
                  <ul>
                    <li>New style and pretty design.</li>
                    <li>
                      Our effort to design beautiful jewelry in top quality.
                    </li>
                  </ul>
                </ListBlock>
                <ListBlock>
                  <h4>About?</h4>
                  <ul>
                    <li>Lorem ipsum.</li>
                    <li>Dolor sit amet consectetur adipisicing elit.</li>
                  </ul>
                </ListBlock>
              </div>
            </DotGrid>
          </ProductAbout>
          <ProductAbout
            id="product-review"
            className={activeTab === "product-review" ? "active" : "hide"}
          >
            {/* Review content */}
            {/* <Review>
              {reviewsData.length > 0 ? (
                <div className="reviews-section">
                  <div className="head-review">
                    <div className="sum-rating">
                      <strong>{summaryRating}</strong>
                      <span>
                        {reviewsCount}{" "}
                        {reviewsCount === 1 ? "review" : "reviews"}
                      </span>
                    </div>
                  </div>
                  <hr style={{ width: "112%", marginBottom: "-10px" }} />
                  <div className="body-review">
                    {matchingReviews.length > 0 ? (
                      matchingReviews.map((review, index) => (
                        <div key={index} className="profile">
                          <div className="thumb-name">
                            <div className="image">
                              <img
                                src="https://firebasestorage.googleapis.com/v0/b/testsaveimage-abb59.appspot.com/o/Details%2FRemove-bg.ai_1722105671395.png?alt=media&token=441a4bb8-0da2-4426-ad91-cdbfd9c9115c"
                                alt=""
                              />
                            </div>
                            <div className="grouping">
                              <div className="name">{review.name}</div>
                              <div className="rating">
                                {Array.from(
                                  { length: review.rating },
                                  (_, i) => (
                                    <StarFilled
                                      key={i}
                                      style={{
                                        color: "#D8A25A",
                                        fontSize: "16px",
                                      }}
                                    />
                                  )
                                )}
                              </div>
                            </div>
                          </div>
                          <div className="comment reply">
                            <strong>{review.highlight}</strong>
                            <p className="grey-color">{review.comment}</p>
                            <div className="date grey-color">
                              On {review.date}
                            </div>
                          </div>
                        </div>
                      ))
                    ) : (
                      <Empty
                        style={{ marginTop: "30px" }}
                        description="No reviews available"
                      />
                    )}
                  </div>
                </div>
              ) : (
                <Empty description="No reviews available" />
              )}
              <StyledPagination defaultCurrent={1} total={10} />
            </Review> */}
          </ProductAbout>
        </div>
      </Contain>
      <ProductSection>
        <Title>
          <h2>RELATED PRODUCTS</h2>
        </Title>
        <List>
          <Row gutter={[16, 16]}>
            {sameBrandProducts.length > 0 ? (
              sameBrandProducts.map((product) => (
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
                          // onMouseOver={(e) =>
                          //   product.usingImage && product.usingImage.length > 1
                          //     ? (e.currentTarget.src = getImage(
                          //         product.usingImage[1].UsingImageID
                          //       ))
                          //     : null
                          // }
                          // onMouseOut={(e) =>
                          //   product.usingImage && product.usingImage.length > 0
                          //     ? (e.currentTarget.src = getImage(
                          //         product.usingImage[0].UsingImageID
                          //       ))
                          //     : (e.currentTarget.src = defaultImage)
                          // }
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
                      {/* <div className="price-container">
                        {product.discountFirstPrice ? (
                          <>
                            <Text className="product-price">
                              ${product.discountFirstPrice}
                            </Text>
                            <Text delete className="product-sale-price">
                              ${product.firstPrice}
                            </Text>
                          </>
                        ) : (
                          <Text className="product-price">
                            ${product.firstPrice}
                          </Text>
                        )}
                      </div> */}
                    </div>
                  </Card>
                </Col>
              ))
            ) : (
              <Col span={24}>
                <div style={{ textAlign: "center", padding: "40px 0" }}>
                  <Text type="secondary">No related products found</Text>
                </div>
              </Col>
            )}
          </Row>
        </List>
      </ProductSection>
    </Body>
  );
};

export default ProductDetails;
