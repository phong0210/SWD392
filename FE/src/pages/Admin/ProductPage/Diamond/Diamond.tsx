import * as Styled from "./Diamond.styled";
import React, { useEffect, useState } from "react";
// import { Link } from "react-router-dom";
import { Table, Input, Button, Space, notification } from "antd";
import {
  SearchOutlined,
  PlusCircleOutlined,
  EyeOutlined,
} from "@ant-design/icons";
import type {
  TableColumnsType,
  TableProps,
  GetProp,
  UploadFile,
  UploadProps,
} from "antd";
// import Dragger from "antd/es/upload/Dragger";
// import { diamondData, DiamondDataType } from "../ProductData"; // Import data here
import { Link } from "react-router-dom";
import Sidebar from "@/components/Admin/Sidebar/Sidebar";
import ProductMenu from "@/components/Admin/ProductMenu/ProductMenu";
import { showAllDiamond } from "@/services/diamondAPI";
import { ColorType, ShapeType } from "./Diamond.type";
import { getImage } from "@/services/imageAPI";
import { useDocumentTitle } from "@/hooks";
import DiamondSteps from "./components/steps/DiamondSteps";
import { showAllProduct } from "@/services/productAPI";
import { Product, ProductApiResponseItem } from "@/models/Entities/Product";
import defaultImage from "@/assets/diamond/defaultImage.png";
// import { RcFile, UploadChangeParam } from "antd/es/upload";

type FileType = Parameters<GetProp<UploadProps, "beforeUpload">>[0];

// const onChange: TableProps<any>["onChange"] = (
//   pagination,
//   filters,
//   sorter,
//   extra
// ) => {
//   console.log("params", pagination, filters, sorter, extra);
// };

// const Diamond = () => {
//   useDocumentTitle("Diamond | Aphromas Diamond");

//   const [searchText, setSearchText] = useState("");
//   const [currency, setCurrency] = useState<"VND" | "USD">("USD");
//   const [isAdding, setIsAdding] = useState(false);
//   const [api, contextHolder] = notification.useNotification();
//   // const [diamonds, setDiamonds] = useState<any[]>([]);
//   const [diamonds, setDiamonds] = useState<Product[]>([]);
//   // const [totalDiamonds, setTotalDiamonds] = useState(0);
//   // const [currentPage, setCurrentPage] = useState(1);
//   // const [pageSize, setPageSize] = useState(6);
//   // const file = useRef<UploadFile>();

//   const fetchData = async () => {
//     try {
//       const response = await showAllProduct();
//       console.log("API response:", response.data);

//       if (response && Array.isArray(response.data)) {
//         const fetchedProducts = (response.data as ProductApiResponseItem[]).map(
//           (item) => {
//             const product = item.product;

//             return {
//               id: product.id,
//               name: product.name,
//               sku: product.sku,
//               description: product.description,
//               price: product.price,
//               carat: product.carat,
//               color: product.color,
//               clarity: product.clarity,
//               cut: product.cut,
//               stockQuantity: product.stockQuantity,
//               giaCertNumber: product.giaCertNumber,
//               isHidden: product.isHidden,
//               categoryId: product.categoryId,
//               orderDetailId: product.orderDetailId,
//               warrantyId: product.warrantyId,
//               salePrice: product.salePrice,
//               firstPrice: product.firstPrice,
//               totalDiamondPrice: product.totalDiamondPrice,
//               star: product.star,
//               type: product.type,
//               images: Array.isArray(product.images) ? product.images : [],
//             };
//           }
//         );

//         console.log("Formatted Diamonds:", fetchedProducts); // Log formatted diamonds
//         setDiamonds(fetchedProducts);
//       }
//     } catch (error) {
//       console.error("Failed to fetch diamonds:", error);
//     }
//   };

//   useEffect(() => {
//     fetchData();
//   }, []);

//   // SEARCH
//   const handleSearch = (value: any) => {
//     console.log("Search:", value);
//   };

//   const handleKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
//     if (e.key === "Enter") {
//       handleSearch(searchText);
//     }
//   };

//   // Change Currency

//   const convertPrice = (
//     price: number,
//     exchangeRate: number,
//     currency: "USD" | "VND"
//   ) => {
//     if (currency === "VND") {
//       return price * exchangeRate;
//     }
//     return price;
//   };

//   const sellingPrice = (price: number, markupPercentage: number) => {
//     return price * (1 + markupPercentage / 100);
//   };

//   const columns: TableColumnsType<any> = [
//     {
//       title: "Diamond ID",
//       dataIndex: "diamondID",
//       // defaultSortOrder: "descend",
//       sorter: (a, b) => parseInt(a.diamondID) - parseInt(b.diamondID),
//     },
//     {
//       title: "Image",
//       key: "diamondImg",
//       className: "TextAlign",
//       render: (_, record) => (
//         <a href="#" target="_blank" rel="noopener noreferrer">
//           <img
//             src={
//               record.images && record.images[0]
//                 ? record.images[0].url
//                 : "default-image-url"
//             }
//             alt={record.diamondName}
//             style={{ width: "50px", height: "50px" }}
//           />
//         </a>
//       ),
//     },
//     {
//       title: "Diamond Name",
//       dataIndex: "diamondName",
//       showSorterTooltip: { target: "full-header" },
//       sorter: (a, b) => a.diamondName.length - b.diamondName.length,
//       // sortDirections: ["descend"],
//     },
//     {
//       title: `Cost Price (${currency})`,
//       key: "price",
//       sorter: (a, b) =>
//         convertPrice(a.price, a.exchangeRate, currency) -
//         convertPrice(b.price, b.exchangeRate, currency),
//       render: (_, record) => {
//         const convertedPrice = convertPrice(
//           record.price,
//           record.exchangeRate,
//           currency
//         );
//         return `${convertedPrice} ${currency}`;
//       },
//     },
//     {
//       title: "Charge Rate",
//       dataIndex: "chargeRate",
//       key: "chargeRate",
//       render: (_, record) => `${record.chargeRate}%`,
//     },
//     {
//       title: `Selling Price (${currency})`,
//       key: "sellingPrice",
//       render: (_, record) => {
//         const price = sellingPrice(
//           convertPrice(record.price, record.exchangeRate, currency),
//           record.chargeRate
//         );
//         return `${price.toFixed(2)} ${currency}`;
//       },
//     },
//     {
//       title: "Color",
//       dataIndex: "color",
//       key: "color",
//       filters: ColorType,
//       onFilter: (value, record) => record.color.indexOf(value as string) === 0,
//       // sortDirections: ["descend"],
//       sorter: (a, b) => a.color.length - b.color.length,
//     },
//     {
//       title: "Shape",
//       dataIndex: "shape",
//       key: "shape",
//       filters: ShapeType,
//       onFilter: (value, record) => record.shape.indexOf(value as string) === 0,
//       sorter: (a, b) => a.shape.length - b.shape.length,
//       // sortDirections: ["descend"],
//     },
//     {
//       title: "Detail",
//       key: "detail",
//       className: "TextAlign",
//       render: (_, { diamondID }) => (
//         <Space size="middle">
//           <Link to={`/admin/product/diamond/detail/${diamondID}`}>
//             <EyeOutlined />
//           </Link>
//         </Space>
//       ),
//     },
//   ];

//   // UPLOAD IMAGES
//   const [fileList, setFileList] = useState<UploadFile[]>([]);
//   const [docsList, setDocsList] = useState<UploadFile[]>([]);

//   const onChangeImg: UploadProps["onChange"] = ({ fileList: newFileList }) => {
//     setFileList(newFileList);
//   };

//   const onPreview = async (file: UploadFile) => {
//     let src = file.url as string;
//     if (!src) {
//       src = await new Promise((resolve) => {
//         const reader = new FileReader();
//         reader.readAsDataURL(file.originFileObj as FileType);
//         reader.onload = () => resolve(reader.result as string);
//       });
//     }
//     const image = new Image();
//     image.src = src;
//     const imgWindow = window.open(src);
//     imgWindow?.document.write(image.outerHTML);
//   };

//   // const onFinish = (values: any) => {
//   //   console.log("Finish:", values);
//   // };

//   return (
//     <>
//       {contextHolder}

//       <Styled.GlobalStyle />
//       <Styled.ProductAdminArea>
//         <Sidebar />

//         <Styled.AdminPage>
//           <ProductMenu />

//           <Styled.AdPageContent>
//             <Styled.AdPageContent_Head>
//               {(!isAdding && (
//                 <>
//                   <Styled.AdPageContent_HeadLeft>
//                     <Styled.SearchArea>
//                       <Input
//                         className="searchInput"
//                         type="text"
//                         // size="large"
//                         placeholder="Search here..."
//                         value={searchText}
//                         onChange={(e) => setSearchText(e.target.value)}
//                         onKeyPress={handleKeyPress}
//                         prefix={<SearchOutlined className="searchIcon" />}
//                       />
//                     </Styled.SearchArea>
//                   </Styled.AdPageContent_HeadLeft>

//                   <Styled.AddButton>
//                     <Button
//                       type="primary"
//                       onClick={() => setIsAdding(!isAdding)}
//                     >
//                       <PlusCircleOutlined />
//                       Add New Diamond
//                     </Button>
//                   </Styled.AddButton>
//                 </>
//               )) || (
//                 <>
//                   <Styled.AddContent_Title>
//                     <p>Add Diamond</p>
//                   </Styled.AddContent_Title>
//                 </>
//               )}
//             </Styled.AdPageContent_Head>

//             <Styled.AdminTable>
//               {isAdding ? (
//                 <>
//                   <DiamondSteps
//                     api={api}
//                     fileList={fileList}
//                     setFileList={setFileList}
//                     onChangeImg={onChangeImg}
//                     onPreview={onPreview}
//                     setIsAdding={setIsAdding}
//                     docsList={docsList}
//                     setDocsList={setDocsList}
//                     fetchData={fetchData}
//                   />
//                 </>
//               ) : (
//                 <Table
//                   className="table"
//                   columns={columns}
//                   dataSource={diamonds}
//                   onChange={onChange}
//                   pagination={{ pageSize: 6 }}
//                 />
//               )}
//             </Styled.AdminTable>
//           </Styled.AdPageContent>
//         </Styled.AdminPage>
//       </Styled.ProductAdminArea>
//     </>
//   );
// };

// export default Diamond;
const Diamond = () => {
  useDocumentTitle("Diamond | Aphromas Diamond");

  const [diamonds, setDiamonds] = useState<Product[]>([]);
  const [searchText, setSearchText] = useState("");
  const [isAdding, setIsAdding] = useState(false);
  const [api, contextHolder] = notification.useNotification();

  const [fileList, setFileList] = useState<UploadFile[]>([]);
  const [docsList, setDocsList] = useState<UploadFile[]>([]);

  const fetchData = async () => {
    try {
      const response = await showAllProduct();
      if (response && Array.isArray(response.data)) {
        const fetchedProducts = (response.data as ProductApiResponseItem[]).map(
          (item) => {
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
              // orderDetailId: product.orderDetailId,
              // orderDetailId1: product.orderDetailId1,
              images: Array.isArray(product.images) ? product.images : [],
            };
          }
        );
        setDiamonds(fetchedProducts);
      }
    } catch (error) {
      console.error("Failed to fetch diamonds:", error);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleSearch = (value: any) => {
    console.log("Search:", value);
  };

  const handleKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      handleSearch(searchText);
    }
  };

  // Change Currency

  const convertPrice = (
    price: number,
    exchangeRate: number,
    currency: "USD" | "VND"
  ) => {
    if (currency === "VND") {
      return price * exchangeRate;
    }
    return price;
  };

  const sellingPrice = (price: number, markupPercentage: number) => {
    return price * (1 + markupPercentage / 100);
  };

  const columns: TableColumnsType<any> = [
    {
      title: "Diamond ID",
      dataIndex: "id",
      // defaultSortOrder: "descend",
      sorter: (a, b) => parseInt(a.id) - parseInt(b.id),
    },
    {
      title: "Image",
      key: "diamondImg",
      className: "TextAlign",
      render: (_, record) => (
        <a href="#" target="_blank" rel="noopener noreferrer">
          <img
            src={
              record.images && record.images[0]
                ? record.images[0].url
                : "default-image-url"
            }
            alt={record.diamondName}
            style={{ width: "50px", height: "50px" }}
          />
        </a>
      ),
    },
    {
      title: "Name",
      dataIndex: "name",
      key: "name",
      sorter: (a: any, b: any) => a.name.localeCompare(b.name),
    },
    {
      title: "SKU",
      dataIndex: "sku",
      key: "sku",
    },
    {
      title: "Description",
      dataIndex: "description",
      key: "description",
    },
    {
      title: "Price (USD)",
      dataIndex: "price",
      key: "price",
      render: (price: number) => `$${price.toLocaleString()}`,
      sorter: (a: any, b: any) => a.price - b.price,
    },
    {
      title: "Carat",
      dataIndex: "carat",
      key: "carat",
      sorter: (a: any, b: any) => a.carat - b.carat,
    },
    {
      title: "Color",
      dataIndex: "color",
      key: "color",
    },
    {
      title: "Clarity",
      dataIndex: "clarity",
      key: "clarity",
    },
    {
      title: "Cut",
      dataIndex: "cut",
      key: "cut",
    },
    {
      title: "Stock",
      dataIndex: "stockQuantity",
      key: "stockQuantity",
    },
    {
      title: "GIA Cert",
      dataIndex: "giaCertNumber",
      key: "giaCertNumber",
    },
    {
      title: "Hidden",
      dataIndex: "isHidden",
      key: "isHidden",
      render: (value: boolean) => (value ? "Yes" : "No"),
    },
    {
      title: "Category",
      dataIndex: "categoryId",
      key: "categoryId",
    },
    {
      title: "Detail",
      key: "detail",
      render: (_: any, record: any) => (
        <Space size="middle">
          <Link to={`/admin/product/diamond/detail/${diamondID}`}>
            <EyeOutlined />
          </Link>
        </Space>
      ),
    },
  ];

  {

  const onChangeImg: UploadProps["onChange"] = ({ fileList: newFileList }) => {
    setFileList(newFileList);
  };

  const onPreview = async (file: UploadFile) => {
    let src = file.url as string;
    if (!src) {
      src = await new Promise((resolve) => {
        const reader = new FileReader();
        reader.readAsDataURL(file.originFileObj as FileType);
        reader.onload = () => resolve(reader.result as string);
      });
    }
    const image = new Image();
    image.src = src;
    const imgWindow = window.open(src);
    imgWindow?.document.write(image.outerHTML);
  };

  return (
    <>
      {contextHolder}
      <Styled.GlobalStyle />
      <Styled.ProductAdminArea>
        <Sidebar />
        <Styled.AdminPage>
          <ProductMenu />
          <Styled.AdPageContent>
            <Styled.AdPageContent_Head>
              {!isAdding ? (
                <>
                  <Styled.AdPageContent_HeadLeft>
                    <Styled.SearchArea>
                      <Input
                        className="searchInput"
                        placeholder="Search here..."
                        value={searchText}
                        onChange={(e) => setSearchText(e.target.value)}
                        onKeyPress={handleKeyPress}
                        prefix={<SearchOutlined className="searchIcon" />}
                      />
                    </Styled.SearchArea>
                  </Styled.AdPageContent_HeadLeft>

                  <Styled.AddButton>
                    <Button
                      type="primary"
                      onClick={() => setIsAdding(true)}
                    >
                      <PlusCircleOutlined />
                      Add New Diamond
                    </Button>
                  </Styled.AddButton>
                </>
              ) : (
                <Styled.AddContent_Title>
                  <p>Add Diamond</p>
                </Styled.AddContent_Title>
              )}
            </Styled.AdPageContent_Head>

            <Styled.AdminTable>
              {isAdding ? (
                <DiamondSteps
                  api={api}
                  fileList={fileList}
                  setFileList={setFileList}
                  onChangeImg={onChangeImg}
                  onPreview={onPreview}
                  setIsAdding={setIsAdding}
                  docsList={docsList}
                  setDocsList={setDocsList}
                  fetchData={fetchData}
                />
              ) : (
                <Table
                  className="table"
                  columns={columns}
                  dataSource={diamonds}
                  pagination={{ pageSize: 6 }}
                  rowKey="id"
                />
              )}
            </Styled.AdminTable>
          </Styled.AdPageContent>
        </Styled.AdminPage>
      </Styled.ProductAdminArea>
    </>
  );
};
}
export default Diamond;