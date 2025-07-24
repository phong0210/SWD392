// // import React from 'react';
// import styled from "styled-components";
// // import 'font-awesome/css/font-awesome.min.css';
// import AccountCus from "@/components/Customer/Account Details/AccountCus";
// import DiamondVoucher from "@/components/Customer/Voucher/DiamondVoucher";
// const Voucher = () => {
//   return (
//     <Main>
//       <AccountCus />
//       <Titles>Voucher</Titles>
//       <Section>
//         <DiamondVoucher />
//       </Section>
//     </Main>
//   );
// };
// export default Voucher;

// const Main = styled.div`
//   margin-bottom: 20rem;
// `;

// const Section = styled.div`
//   margin: 0 2rem 0 2rem;
// `;
// const Titles = styled.h1`
//   color: #000;
//   font: 600 32px "Crimson Text", sans-serif;
//   display: flex;
//   flex-direction: row;
//   justify-content: space-around;
//   padding: 1.5rem 0;
//   @media (max-width: 991px) {
//     margin-top: 40px;
//   }
// `;

import React, { useEffect, useState } from "react";
import styled, { createGlobalStyle } from "styled-components";
import { Input, Table, notification, Space, Typography, Tag } from "antd";
import { SearchOutlined } from "@ant-design/icons";
import type { ColumnsType, TableProps } from "antd/es/table";
import dayjs from "dayjs";

import AccountCus from "@/components/Customer/Account Details/AccountCus";
import { showAllVoucher } from "@/services/voucherAPI";
import { showAllProduct } from "@/services/productAPI";

// =========================================================
// Định nghĩa lại các Styled Components cần thiết TRỰC TIẾP trong file này
// =========================================================

const GlobalStyle = createGlobalStyle`
  html, body {
    height: 100%;
    margin: 0;
    padding: 0;
    background-color: #f1f1f1;
    font-family: 'Poppins', sans-serif;
  }
`;

const MainContainer = styled.div`
  display: flex;
  flex-direction: column; /* Đã thay đổi: sắp xếp các phần tử theo chiều dọc */
  background-color: #f1f1f1;
  font-family: "Poppins", sans-serif;
  width: 100%;
  min-height: 100vh;
`;

const PageContentWrapper = styled.div`
  flex-grow: 1;
  margin: 0 35px;
  padding-bottom: 55px;
  display: flex;
  flex-direction: column;
`;

const ContentCard = styled.div`
  width: 100%;
  background-color: #ffffff;
  border-radius: 16px;
  margin-top: 28px;
  padding-top: 25px;
  padding-bottom: 20px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
`;

const ContentHeader = styled.div`
  margin: 0px 40px 30px 40px;
  display: flex;
  justify-content: center;
  align-items: center;
`;

const SearchInputArea = styled.div`
  width: 80%;
  display: flex;
  align-items: center;

  .ant-input-affix-wrapper {
    border-radius: 10px;
    height: 45px;
    background-color: #f8f9fb;
    border: 1px solid rgba(203, 210, 220, 0.5);
  }

  .ant-input {
    background-color: #f8f9fb;
    color: #151542;
    padding-left: 10px;
  }

  .anticon.ant-input-prefix {
    color: #151542;
    margin-right: 8px;
  }
`;

const StyledTableWrapper = styled.div`
  padding: 0px 50px 0px 50px;

  .ant-table-wrapper {
    border: 1px solid #f0f0f0;
    border-radius: 8px;
    overflow: hidden;
  }

  .ant-table-thead > tr > th {
    background-color: #f8f9fb;
    color: #92929d;
    font-size: 13px;
    font-weight: 500;
  }

  .ant-table-tbody > tr > td {
    color: #151542;
    font-size: 14px;
    padding: 12px 16px;
  }
`;

const PageTitle = styled.h1`
  color: #000;
  font-size: 32px;
  font-weight: 600;
  font-family: "Crimson Text", sans-serif;
  display: flex;
  justify-content: center;
  padding: 1.5rem 0;
  margin-top: 30px;

  @media (max-width: 991px) {
    margin-top: 40px;
  }
`;

// =========================================================
// Kết thúc Styled Components
// =========================================================

interface VoucherType {
  key: React.Key;
  promotionID: string;
  name: string;
  discountValue: number;
  startDate: string;
  endDate: string;
  description: string;
  appliesToProductId: number | null;
}

interface Product {
  Id: number;
  Name: string;
}

const Voucher: React.FC = () => {
  const [vouchers, setVouchers] = useState<VoucherType[]>([]);
  const [filteredVouchers, setFilteredVouchers] = useState<VoucherType[]>([]);
  const [products, setProducts] = useState<Product[]>([]);
  const [searchText, setSearchText] = useState("");
  const [api, contextHolder] = notification.useNotification();
  const [isLoading, setIsLoading] = useState(false);

  const openNotification = (type: "success" | "error", message: string) => {
    api[type]({
      message: type === "success" ? "Success" : "Error",
      description: message,
    });
  };

  const fetchData = async () => {
    setIsLoading(true);
    try {
      const responseVoucher = await showAllVoucher();
      const responseProduct = await showAllProduct();

      console.log("📦 Raw Voucher Response:", responseVoucher);
      console.log("📦 Raw Product Response:", responseProduct);

      const voucherData = responseVoucher.data || [];
      const formattedVouchers: VoucherType[] = voucherData
        .map((voucher: any) => {
          const now = dayjs();
          const endDate = dayjs(voucher.endDate);
          const startDate = dayjs(voucher.startDate);
          
          let status = "Inactive";
          if (now.isAfter(startDate) && now.isBefore(endDate)) {
            status = "Active";
          } else if (now.isBefore(startDate)) {
            status = "Upcoming";
          } else if (now.isAfter(endDate)) {
            status = "Expired";
          }

          return {
            key: voucher.id,
            promotionID: voucher.id,
            name: voucher.name,
            discountValue: voucher.discountValue,
            startDate: voucher.startDate,
            endDate: voucher.endDate,
            description: voucher.description,
            appliesToProductId: voucher.appliesToProductId,
         
          };
        })
     

      const productListData = responseProduct.data || [];
      const formattedProducts: Product[] = productListData
        .filter((item: any) => item.product && item.product.id !== null && item.product.name !== null)
        .map((item: any) => ({
          Id: item.product.id,
          Name: item.product.name,
        }));

      console.log("✔️ Formatted Vouchers for Customer:", formattedVouchers);
      console.log("✔️ Formatted Products for Customer:", formattedProducts);

      setVouchers(formattedVouchers);
      setProducts(formattedProducts);
    } catch (error) {
      openNotification("error", "Failed to load voucher data.");
      console.error("❌ Failed to fetch data:", error);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  useEffect(() => {
    onSearch(searchText);
  }, [vouchers, searchText]);

  const onSearch = (value: string) => {
    const keyword = value.toLowerCase().trim();
    if (!keyword) {
      setFilteredVouchers(vouchers);
      return;
    }

    const filtered = vouchers.filter((voucher) => {
      const nameMatch = voucher.name?.toLowerCase().includes(keyword);
      const descriptionMatch = voucher.description?.toLowerCase().includes(keyword);
      const productIdMatch = voucher.appliesToProductId ? products.find(p => p.Id === voucher.appliesToProductId)?.Name.toLowerCase().includes(keyword) : false;
      
      return nameMatch || descriptionMatch || productIdMatch;
    });
    setFilteredVouchers(filtered);
  };

  const handleKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      onSearch(searchText);
    }
  };

  const columns: ColumnsType<VoucherType> = [
    {
      title: "Voucher Name",
      dataIndex: "name",
      key: "name",
      sorter: (a, b) => a.name.localeCompare(b.name),
      width: '20%',
    },
    {
      title: "Discount Value (%)",
      dataIndex: "discountValue",
      key: "discountValue",
      sorter: (a, b) => a.discountValue - b.discountValue,
      width: '10%',
    },
    {
      title: "Start Date",
      dataIndex: "startDate",
      key: "startDate",
      render: (date: string) => (date ? dayjs(date).format("YYYY-MM-DD") : "N/A"),
      sorter: (a, b) => dayjs(a.startDate).unix() - dayjs(b.startDate).unix(),
      width: '15%',
    },
    {
      title: "End Date",
      dataIndex: "endDate",
      key: "endDate",
      render: (date: string) => (date ? dayjs(date).format("YYYY-MM-DD") : "N/A"),
      sorter: (a, b) => dayjs(a.endDate).unix() - dayjs(b.endDate).unix(),
      width: '15%',
    },
    {
      title: "Applies To Product",
      dataIndex: "appliesToProductId",
      key: "appliesToProductId",
      render: (appliesToProductId: number | null) => {
        if (appliesToProductId === null) {
          return "All Products";
        }
        const product = products.find((p) => p.Id === appliesToProductId);
        return product ? product.Name : "N/A";
      },
      sorter: (a, b) => {
        const nameA = a.appliesToProductId ? products.find(p => p.Id === a.appliesToProductId)?.Name || '' : 'All Products';
        const nameB = b.appliesToProductId ? products.find(p => p.Id === b.appliesToProductId)?.Name || '' : 'All Products';
        return nameA.localeCompare(nameB);
      },
      width: '20%',
    },
    {
      title: "Description",
      dataIndex: "description",
      key: "description",
      width: '10%',
      render: (text: string) => (
        <Typography.Paragraph ellipsis={{ rows: 2, expandable: true, symbol: 'more' }}>
          {text}
        </Typography.Paragraph>
      ),
    },
  ];

  const onChangeTable: TableProps<VoucherType>["onChange"] = (
    pagination,
    filters,
    sorter,
    extra
  ) => {
    console.log("Table params", pagination, filters, sorter, extra);
  };

  return (
    <MainContainer>
      {contextHolder}
      <GlobalStyle />
      <AccountCus />

      <PageContentWrapper>
        <PageTitle>Vouchers</PageTitle>

        <ContentCard>
          <ContentHeader>
            <SearchInputArea>
              <Input
                placeholder="Search vouchers by name, description, or product..."
                value={searchText}
                onChange={(e) => setSearchText(e.target.value)}
                onKeyDown={handleKeyPress}
                prefix={<SearchOutlined />}
              />
            </SearchInputArea>
          </ContentHeader>

          <StyledTableWrapper>
            <Table
              bordered
              dataSource={filteredVouchers}
              columns={columns}
              rowKey="promotionID"
              pagination={{ pageSize: 6 }}
              onChange={onChangeTable}
              loading={isLoading}
              locale={{ emptyText: "No vouchers found." }}
            />
          </StyledTableWrapper>
        </ContentCard>
      </PageContentWrapper>
    </MainContainer>
  );
};

export default Voucher;