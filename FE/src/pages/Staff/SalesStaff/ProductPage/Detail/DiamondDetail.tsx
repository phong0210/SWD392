import * as Styled from "./ProductDetail.styled";
import { useEffect, useState } from "react";
import { Modal, notification } from "antd";
import { useNavigate, useParams } from "react-router-dom";
import Sidebar from "@/components/Staff/SalesStaff/Sidebar/Sidebar";
import ProductMenu from "@/components/Staff/SalesStaff/ProductMenu/ProductMenu";
import { getImage } from "@/services/imageAPI";
import {
  ColorType_Option,
  ShapeType_Option,
} from "@/pages/Admin/ProductPage/Diamond/Diamond.type";
import { getProductDetails, updateDiamond } from "@/services/productAPI";
import defaultImage from "@/assets/diamond/defaultImage.png";
import { Product } from "@/models/Entities/Product";

const DiamondDetail = () => {
  const { id } = useParams<{ id: string }>();
  const [isLoading, setIsLoading] = useState(true);
  const navigate = useNavigate();
  const [activeDiamond, setActiveDiamond] = useState<Product | null>(null);
  const [isModalVisibleGIA, setIsModalVisibleGIA] = useState(false);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [diamondMainImage, setDiamondMainImage] = useState("");
  const [diamondSelectedThumb, setDiamondSelectedThumb] = useState(0);
  const [api, contextHolder] = notification.useNotification();

  type NotificationType = "success" | "info" | "warning" | "error";

  const openNotification = (
    type: NotificationType,
    method: string,
    error: string
  ) => {
    api[type]({
      message: type === "success" ? "Notification" : "Error",
      description:
        type === "success" ? `${method} manager successfully` : error,
    });
  };

  useEffect(() => {
    if (!id) {
      console.error("Missing product ID");
      setIsLoading(false);
      return;
    }
    const fetchDiamond = async () => {
      try {
        const response = await getProductDetails(id);
        console.log("Response from getProductDetails:", response.data);
        const diamondData = response.data;
        console.log("Diamond Data:", diamondData.product.id);
        setActiveDiamond(diamondData.product);
        setDiamondMainImage(defaultImage);
        setDiamondSelectedThumb(0);
      } catch (error) {
        console.error("Error fetching diamond details:", error);
      }
    };
    fetchDiamond();
  }, [id]);

  useEffect(() => {
    console.log("Active Diamond:", activeDiamond);
  }, [activeDiamond]);

  // GIA
  const showModalGIA = () => {
    setIsModalVisibleGIA(true);
  };

  const handleOkGIA = () => {
    setIsModalVisibleGIA(false);
  };

  const handleCancelGIA = () => {
    setIsModalVisibleGIA(false);
  };

  const handleDelete = async () => {
    try {
      const response = await updateDiamond(activeDiamond.id, {
        IsActive: false,
      });
      console.log("Delete Response:", response.data);
      if (response.status === 200) {
        openNotification("success", "Delete", "");
        setIsModalVisible(false);
        navigate("/admin/product");
      } else {
        openNotification("error", "Delete", "Failed to delete diamond.");
      }
    } catch (error: any) {
      console.error("Failed to delete diamond:", error);
      openNotification("error", "Delete", error.message);
    }
  };

  const handleCancel = () => {
    setIsModalVisible(false);
  };

  // IMAGE STATES
  // const changeDiamondImage = (src: string, index: number) => {
  //   setDiamondMainImage(src);
  //   setDiamondSelectedThumb(index);
  // };

  if (!activeDiamond) {
    return <div>Diamond not found</div>;
  }

  return (
    <>
      {contextHolder}
      <Styled.GlobalStyle />
      <Styled.PageAdminArea>
        <Sidebar />
        <Styled.AdminPage>
          <ProductMenu />
          <Styled.PageContent>
            {activeDiamond ? (
              <>
                <Styled.PageContent_Mid>
                  <Styled.PageDetail_Title>
                    <p>Diamond Detail</p>
                  </Styled.PageDetail_Title>
                  <Styled.PageDetail_Infor>
                    <Styled.ImageContainer>
                      <Styled.OuterThumb>
                        {/* <Styled.ThumbnailImage>
                          {activeDiamond.usingImage?.map(
                            (image: any, index: any) => {
                              if (image.CertificateID == null) {
                                const imageUrl = `http://localhost:3000/usingImage/${image.UsingImageID}`;
                                return (
                                  <Styled.Item
                                    key={index}
                                    className={
                                      index === diamondSelectedThumb
                                        ? "selected"
                                        : ""
                                    }
                                    onClick={() =>
                                      changeDiamondImage(imageUrl, index)
                                    }
                                  >
                                    <img
                                      key={index}
                                      src={imageUrl}
                                      alt={`Diamond Thumbnail ${index}`}
                                    />
                                  </Styled.Item>
                                );
                              }
                              return null;
                            }
                          )}
                        </Styled.ThumbnailImage> */}
                      </Styled.OuterThumb>
                      <Styled.OuterMain>
                        <Styled.MainImage>
                          <img
                            id="mainImage"
                            src={diamondMainImage}
                            alt="Main"
                          />
                          <img
                            className="GIAExport"
                            src="https://firebasestorage.googleapis.com/v0/b/testsaveimage-abb59.appspot.com/o/Admin%2FProduct%2Fgia-logo.svg?alt=media&token=223f8b08-36c3-401b-ae25-a35f4c930631"
                            alt="GIA Certificate"
                            onClick={showModalGIA}
                            style={{ cursor: "pointer" }}
                          />
                        </Styled.MainImage>
                      </Styled.OuterMain>
                    </Styled.ImageContainer>
                    <Styled.ProductContent>
                      <Styled.InforLine>
                        <p className="InforLine_Title">Diamond ID</p>
                        <span>{activeDiamond.id}</span>
                      </Styled.InforLine>
                      <Styled.InforLine>
                        <p className="InforLine_Title">Diamond Name</p>
                        <span>{activeDiamond.name}</span>
                      </Styled.InforLine>
                      <Styled.InforLine>
                        <p className="InforLine_Title">Price</p>
                        <span>{activeDiamond.price}</span>
                      </Styled.InforLine>

                      <Styled.InforLine>
                        <p className="InforLine_Title">Color</p>
                        <span>
                          {ColorType_Option.find(
                            (option: any) =>
                              option.value === activeDiamond.color
                          )?.label ?? "N/A"}
                        </span>
                      </Styled.InforLine>
                      <Styled.InforLine>
                        <p className="InforLine_Title">Weight (Carat)</p>
                        <span>{activeDiamond.carat}</span>
                      </Styled.InforLine>
                      <Styled.InforLine>
                        <p className="InforLine_Title">Cut</p>
                        <span>{activeDiamond.cut}</span>
                      </Styled.InforLine>
                      <Styled.InforLine>
                        <p className="InforLine_Title">Clarity</p>
                        <span>{activeDiamond.clarity}</span>
                      </Styled.InforLine>
                      <Styled.InforLine_Descrip>
                        <p className="InforLine_Title">GiaCertNumber</p>
                        <span>{activeDiamond.giaCertNumber}</span>
                      </Styled.InforLine_Descrip>

                      <Modal
                        title="Confirm Deletion"
                        visible={isModalVisible}
                        onOk={handleDelete}
                        onCancel={handleCancel}
                      >
                        <p>Are you sure you want to delete this diamond?</p>
                      </Modal>
                    </Styled.ProductContent>
                  </Styled.PageDetail_Infor>
                </Styled.PageContent_Mid>
                {/* <Styled.ActionBtn>
                      <Styled.ActionBtn_Left>
                        <Button
                          type="primary"
                          onClick={() => setIsEditing(true)}
                        >
                          Edit
                        </Button>
                        <Link to="/admin/product">
                          <Button style={{ marginLeft: "10px" }}>Back</Button>
                        </Link>
                      </Styled.ActionBtn_Left>
                      <Styled.ActionBtn_Right>
                        <Button type="primary" danger onClick={showModal}>
                          Delete
                        </Button>
                      </Styled.ActionBtn_Right>
                    </Styled.ActionBtn> */}
                <Modal
                  title="GIA Certificate"
                  visible={isModalVisibleGIA}
                  onOk={handleOkGIA}
                  onCancel={handleCancelGIA}
                >
                  <img
                    src={getImage(
                      activeDiamond?.certificate?.[
                        activeDiamond.certificate.length - 1
                      ]?.usingImages[0]?.UsingImageID
                    )}
                    alt="GIA Certificate"
                    style={{ width: "100%" }}
                  />
                </Modal>
              </>
            ) : (
              <p>Loading...</p>
            )}
          </Styled.PageContent>
        </Styled.AdminPage>
      </Styled.PageAdminArea>
    </>
  );
};

export default DiamondDetail;
