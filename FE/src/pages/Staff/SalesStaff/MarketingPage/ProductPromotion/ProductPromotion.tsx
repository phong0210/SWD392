import * as Styled from "./ProductPromotion.styled";
import React, { useEffect, useState } from "react";
import {
  EyeOutlined,
  SearchOutlined,
} from "@ant-design/icons";
import type {
  TableProps,
} from "antd";
import {
  Form,
  Input,
  Popconfirm,
  Space,
  Table,
  Typography,
  notification,
} from "antd";
import {showAllVoucher} from "@/services/voucherAPI";
import { showAllProduct } from "@/services/productAPI";
import dayjs from "dayjs";
import { Link } from "react-router-dom";
import { promotionData, PromotionDataType } from "../MarketingData";
import { productData } from "../../ProductPage/ProductData";
import Sidebar from "@/components/Staff/SalesStaff/Sidebar/Sidebar";
import MarketingMenu from "@/components/Staff/SalesStaff/MarketingMenu/MarketingMenu";



const ProductPromotion = () => {
  const [form] = Form.useForm();
  const [searchText, setSearchText] = useState("");
  const [discounts, setDiscounts] = useState<any[]>([]);
  const [editingKey, setEditingKey] = useState<React.Key>("");
  const isEditing = (record: any) => record.key === editingKey;
  const [products, setProducts] = useState<any[]>([]);
  const [productUpdate, setProductUpdate] = useState<any[]>([]);
  // const [diamondUpdate, setDiamondUpdate] = useState<any[]>([]);
  const [api, contextHolder] = notification.useNotification();

  const [selectedProducts, setSelectedProducts] = useState([]);
  const [filteredDiscounts, setFilteredDiscounts] = useState<any[]>([]);
  


  type NotificationType = "success" | "info" | "warning" | "error";

  const openNotification = (
    type: NotificationType,
    method: string,
    error: string
  ) => {
    api[type]({
      message: type === "success" ? "Notification" : "Error",
      description:
        type === "success" ? `${method} promotion successfully` : error,
    });
  };

 const fetchData = async () => {
  try {
    const response = await showAllVoucher();
    const responseProduct = await showAllProduct();
  console.log("📦 responseProduct:", responseProduct);

    // const discountList = response?.data?.data ?? [];
    // const productList = responseProduct?.data?.data ?? [];

     const data = response.data;
    //  const { data: productData } = responseProduct.data;
    // const productData = responseProduct.data.map((item: any) => item.product);
    const productData = responseProduct.data.map((item: any) => ({
      Id: item.product.id, // hoặc item.product.Id nếu viết hoa
      Name: item.product.name, // nếu cần thêm thì bổ sung các field khác
    }));
    console.log("Voucher data:", data); // debug xem có dữ liệu không

    const formattedDiscounts = data.map((voucher: any) => ({
      key: voucher.id,
      promotionID: voucher.id,
      name: voucher.name,
      discountValue: voucher.discountValue,
      startDate: voucher.startDate,
      endDate: voucher.endDate,
      description: voucher.description,
      appliesToProductId: voucher.appliesToProductId,
    }));

    const formattedProducts = productData
  .filter((product: any) => product.ProductID !== null)
  .map((product: any) => ({
    label: product.Name,       // để hiển thị tên sản phẩm trên dropdown/select UI
    value: product.ProductID,  // để lưu xuống AppliesToProductId trong bảng Promotion
  }));

    setDiscounts(formattedDiscounts);
    setProducts(formattedProducts);
    // setProductUpdate(productList);
    setProductUpdate(productData);
    console.log("productUpdate example:", productUpdate[0]);
console.log("appliesToProductId in discounts:", discounts.map(d => d.appliesToProductId));

    console.log("✔️ Discounts:", formattedDiscounts);
    console.log("✔️ Products:", formattedProducts);
  } catch (error) {
    console.error("❌ Failed to fetch data:", error);
  }
};


  useEffect(() => {
    fetchData();
  }, []);

  const columns = [
      {
        title: "Promotion ID",
        dataIndex: "promotionID",
        editable: false,
        sorter: (a: any, b: any) => parseInt(a.promotionID) - parseInt(b.promotionID),
      },
      {
        title: "Name",
        dataIndex: "name",
        editable: true,
        sorter: (a: any, b: any) => a.name.length - b.name.length,
      },
      {
        title: "% discount",
        dataIndex: "discountValue",
        editable: true,
        sorter: (a: any, b: any) => a.discountValue - b.discountValue,
      },
  
      {
          title: "Start Date",
          dataIndex: "startDate",
          editable: true,
          render: (_: any, { startDate }: any) => {
            return startDate ? dayjs(startDate).format("YYYY-MM-DD") : "N/A";
          },
          sorter: (a: any, b: any) =>
            dayjs(a.startDate).unix() - dayjs(b.startDate).unix(),
        },
        {
          title: "End Date",
          dataIndex: "endDate",
          editable: true,
          render: (_: any, { endDate }: any) => {
            return endDate ? dayjs(endDate).format("YYYY-MM-DD") : "N/A";
          },
          sorter: (a: any, b: any) =>
            dayjs(a.endDate).unix() - dayjs(b.endDate).unix(),
        },
   
    {
    title: "Applied Product",
    dataIndex: "appliesToProductId",
    // editable: true,
    render: (_: any, record: any) => {
   
      const product = productUpdate.find(p => p.Id === record.appliesToProductId);
      return product ? `${product.Name}` : "N/A"; // Nếu không thấy tên, có thể là do productUpdate rỗng hoặc Id không khớp
    },
  },
      {
        title: "Description",
        dataIndex: "description",
        editable: true,
      },
  
              {
      title: "Detail",
      key: "detail",
      className: "TextAlign",
      render: (_: unknown, record: { promotionID: string }) => (
        <Space size="middle">
          <Link to={`/sales-staff/marketing/discount/detail/${record.promotionID}`}>
            <EyeOutlined />
          </Link>
        </Space>
      ),
    }
  ];
  
    const mergedColumns = columns.map((col) => {
    if (!col.editable) {
      return col;
    }
    return {
      ...col,
      onCell: (record: any) => ({
        record,
        inputType:
          col.dataIndex === "discountValue"
            ? "number"
            : col.dataIndex === "appliesToProductId"
            ? "select" // <-- CHẮC CHẮN PHẢI CÓ DÒNG NÀY
            : col.dataIndex === "startDate" || col.dataIndex === "endDate"
            ? "date"
            : "text",
        dataIndex: col.dataIndex,
        title: col.title,
        editing: isEditing(record),
        products: products, // <-- CHẮC CHẮN PHẢI CÓ DÒNG NÀY
      }),
    };
  });
  
    const onChangeTable: TableProps<any>["onChange"] = (
      pagination,
      filters,
      sorter,
      extra
    ) => {
      console.log("params", pagination, filters, sorter, extra);
    };
  

  // SEARCH AREA
  const onSearch = (value: string) => {
    const keyword = value.toLowerCase().trim();
    const filtered = discounts.filter((discount) =>
      discount.name?.toLowerCase().includes(keyword)
    );
    setFilteredDiscounts(filtered);
  };
    useEffect(() => {
    setFilteredDiscounts(discounts); // khi discounts thay đổi thì update bảng hiển thị
  }, [discounts]);
  
    const handleKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
      if (e.key === "Enter") {
        onSearch(searchText);
      }
    };
  

 
  return (
    <>
      <Styled.GlobalStyle />
      <Styled.ProductAdminArea>
        <Sidebar />

        <Styled.AdminPage>
          <MarketingMenu />

          <Styled.AdPageContent>
            <Styled.AdPageContent_Head>
                  <Styled.SearchArea>
                    <Input
                      className="searchInput"
                      type="text"
                      // size="large"
                      placeholder="Search here..."
                      value={searchText}
                      onChange={(e) => setSearchText(e.target.value)}
                      onKeyPress={handleKeyPress}
                      prefix={<SearchOutlined className="searchIcon" />}
                    />
                  </Styled.SearchArea>
            </Styled.AdPageContent_Head>

            <Styled.AdminTable>
              
                <Form form={form} component={false}>
                  <Table
                    bordered
                    dataSource={filteredDiscounts}
                    columns={columns}
                    rowClassName="editable-row"
                    pagination={{ pageSize: 6 }} // Add pagination here
                    onChange={onChangeTable}
                  />
                </Form>
            </Styled.AdminTable>
          </Styled.AdPageContent>
        </Styled.AdminPage>
      </Styled.ProductAdminArea>
    </>
  );
};

export default ProductPromotion;
