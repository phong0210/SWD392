import * as Styled from "./Diamond.styled";
import React, { useEffect, useState } from "react";
import { Table, Input, Space, notification } from "antd";
import { SearchOutlined, EyeOutlined } from "@ant-design/icons";
import type { TableColumnsType, TableProps } from "antd";
import { Link } from "react-router-dom";
import { useDocumentTitle } from "@/hooks";

import Sidebar from "@/components/Staff/SalesStaff/Sidebar/Sidebar";
import ProductMenu from "@/components/Staff/SalesStaff/ProductMenu/ProductMenu";

import { showAllProduct } from "@/services/productAPI";
import { ColorType, ShapeType } from "./Diamond.type";
import { Product, ProductApiResponseItem } from "@/models/Entities/Product";
import defaultImage from "@/assets/diamond/defaultImage.png";

const Diamond = () => {
  useDocumentTitle("Diamond | Aphromas Diamond");

  const [searchText, setSearchText] = useState("");
  const [currency] = useState<"VND" | "USD">("USD");
  const [api, contextHolder] = notification.useNotification();
  const [diamonds, setDiamonds] = useState<Product[]>([]);

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
              orderDetailId: product.orderDetailId,
              warrantyId: product.warrantyId,
              salePrice: product.salePrice,
              firstPrice: product.firstPrice,
              totalDiamondPrice: product.totalDiamondPrice,
              star: product.star,
              type: product.type,
              images: Array.isArray(product.images) ? product.images : [],
            };
          }
        );

        setDiamonds(fetchedProducts);
      }
    } catch (error) {
      api.error({
        message: "Failed",
        description: "Failed to fetch diamonds.",
      });
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleSearch = (value: string) => {
    console.log("Search:", value);
  };

  const handleKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      handleSearch(searchText);
    }
  };

  const convertPrice = (
    price: number,
    exchangeRate: number,
    currency: "USD" | "VND"
  ) => {
    return currency === "VND" ? price * exchangeRate : price;
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
            src={defaultImage}
            alt={record.diamondName}
            style={{ width: "50px", height: "50px" }}
          />
        </a>
      ),
    },
    {
      title: "Diamond Name",
      dataIndex: "name",
      showSorterTooltip: { target: "full-header" },
      sorter: (a, b) => a.diamondName.length - b.diamondName.length,
      // sortDirections: ["descend"],
    },
    {
      title: `Cost Price (${currency})`,
      key: "price",
      sorter: (a, b) =>
        convertPrice(a.price, a.exchangeRate, currency) -
        convertPrice(b.price, b.exchangeRate, currency),
      render: (_, record) => {
        const convertedPrice = convertPrice(
          record.price,
          record.exchangeRate,
          currency
        );
        return `${convertedPrice} ${currency}`;
      },
    },
    // {
    //   title: "Charge Rate",
    //   dataIndex: "chargeRate",
    //   key: "chargeRate",
    //   render: (_, record) => `${record.chargeRate}%`,
    // },
    // {
    //   title: `Selling Price (${currency})`,
    //   key: "sellingPrice",
    //   render: (_, record) => {
    //     const price = sellingPrice(
    //       convertPrice(record.price, record.exchangeRate, currency),
    //       record.chargeRate
    //     );
    //     return `${price.toFixed(2)} ${currency}`;
    //   },
    // },
    {
      title: "Color",
      dataIndex: "color",
      key: "color",
      filters: ColorType,
      onFilter: (value, record) => record.color.indexOf(value as string) === 0,
      // sortDirections: ["descend"],
      sorter: (a, b) => a.color.length - b.color.length,
    },
    // {
    //   title: "Shape",
    //   dataIndex: "shape",
    //   key: "shape",
    //   filters: ShapeType,
    //   onFilter: (value, record) => record.shape.indexOf(value as string) === 0,
    //   sorter: (a, b) => a.shape.length - b.shape.length,
    //   // sortDirections: ["descend"],
    // },
    {
      title: "Detail",
      key: "detail",
      className: "TextAlign",
      render: (_, { id }) => (
        <Space size="middle">
          <Link to={`/admin/product/diamond/detail/${id}`}>
            <EyeOutlined />
          </Link>
        </Space>
      ),
    },
  ];

  const onChange: TableProps<any>["onChange"] = (
    pagination,
    filters,
    sorter,
    extra
  ) => {
    console.log("params", pagination, filters, sorter, extra);
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
              <Styled.AdPageContent_HeadLeft>
                <Styled.SearchArea>
                  <Input
                    className="searchInput"
                    type="text"
                    placeholder="Search here..."
                    value={searchText}
                    onChange={(e) => setSearchText(e.target.value)}
                    onKeyPress={handleKeyPress}
                    prefix={<SearchOutlined className="searchIcon" />}
                  />
                </Styled.SearchArea>
              </Styled.AdPageContent_HeadLeft>
            </Styled.AdPageContent_Head>

            <Styled.AdminTable>
              <Table
                className="table"
                columns={columns}
                dataSource={diamonds}
                onChange={onChange}
                pagination={{ pageSize: 6 }}
                rowKey="id"
              />
            </Styled.AdminTable>
          </Styled.AdPageContent>
        </Styled.AdminPage>
      </Styled.ProductAdminArea>
    </>
  );
};

export default Diamond;
