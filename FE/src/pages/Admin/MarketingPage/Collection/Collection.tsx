import * as Styled from "./Collection.styled";
import React, { useEffect, useState } from "react";
import { EyeOutlined } from "@ant-design/icons";
import type { TableProps } from "antd";
import { Form, Table, Space, notification } from "antd";
import { Link } from "react-router-dom"; // Added missing import
import Sidebar from "../../../../components/Admin/Sidebar/Sidebar";
import MarketingMenu from "@/components/Admin/MarketingMenu/MarketingMenu";
import { showAllCategory, showAllProduct } from "@/services/productAPI";

const Collection = () => {
  const [form] = Form.useForm();
  const [api, contextHolder] = notification.useNotification();
  const [collections, setCollections] = useState<any[]>([]);
  const [editingKey, setEditingKey] = useState(""); // Added missing state
  const isEditing = (record: any) => record.key === editingKey;

  const fetchData = async () => {
    try {
      const response = await showAllCategory();

      const data = response.data;

      console.log("Parsed categories:", data);

      const formattedCollections = data.map((item: any) => ({
        key: item.category.id,
        categoryId: item.category.id,
        name: item.category.name,
        description: item.category.description,
      }));

      setCollections(formattedCollections);
    } catch (error) {
      console.error("Failed to fetch category or product:", error);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const columns = [
    {
      title: "Category ID",
      dataIndex: "categoryId",
      sorter: (a: any, b: any) => a.categoryId.localeCompare(b.categoryId), // Changed to string comparison since IDs are UUIDs
    },
    {
      title: "Category Name",
      dataIndex: "name",
      editable: true,
      sorter: (a: any, b: any) => a.name.length - b.name.length,
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
      render: (
        _: unknown,
        record: any // Fixed destructuring
      ) => (
        <Space size="middle">
          <Link to={`/admin/marketing/collection/detail/${record.categoryId}`}>
            <EyeOutlined />
          </Link>
        </Space>
      ),
    },
  ];

  const mergedColumns = columns.map((col) => {
    if (!col.editable) {
      return col;
    }
    return {
      ...col,
      onCell: (record: any) => ({
        record,
        inputType: col.dataIndex === "text",
        dataIndex: col.dataIndex,
        title: col.title,
        editing: isEditing(record),
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

  return (
    <>
      {contextHolder}

      <Styled.GlobalStyle />
      <Styled.ProductAdminArea>
        <Sidebar />

        <Styled.AdminPage>
          <MarketingMenu />

          <Styled.AdPageContent>
            <Styled.AdminTable>
              <Form form={form} component={false}>
                <Table
                  bordered
                  dataSource={collections}
                  columns={mergedColumns}
                  rowClassName="editable-row"
                  pagination={{ pageSize: 6 }}
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

export default Collection;
