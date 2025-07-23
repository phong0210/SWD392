import * as Styled from "./BillPromotion.styled";
import React, { useEffect, useState } from "react";
import { SearchOutlined } from "@ant-design/icons";
import dayjs from "dayjs";
import {
  Form,
  Input,
  InputNumber,
  Table,
  notification,
} from "antd";
import Sidebar from "@/components/Staff/SalesStaff/Sidebar/Sidebar";
import { showAllVoucher } from "@/services/voucherAPI";
import { DatePickerProps } from "antd/lib";
import MarketingMenu from "@/components/Staff/SalesStaff/MarketingMenu/MarketingMenu";

interface EditableCellProps {
  editing: boolean;
  dataIndex: keyof any;
  title: React.ReactNode;
  inputType: "number" | "text";
  record: any;
  index: number;
}

const EditableCell: React.FC<React.PropsWithChildren<EditableCellProps>> = ({
  editing,
  dataIndex,
  title,
  inputType,
  children,
  ...restProps
}) => {
  const inputNode = inputType === "number" ? <InputNumber /> : <Input />;

  return (
    <td {...restProps}>
      {editing ? (
        <Form.Item
          name={dataIndex.toString()}
          style={{ margin: 0 }}
          rules={[
            {
              required: true,
              message: `Please Input ${title}!`,
            },
          ]}
        >
          {inputNode}
        </Form.Item>
      ) : (
        children
      )}
    </td>
  );
};


// DATE PICK
const onChangeDate: DatePickerProps["onChange"] = (date, dateString) => {
  console.log(date, dateString);
};

const BillPromotion = () => {
  const [form] = Form.useForm();
  const [searchText, setSearchText] = useState("");
  const [isAdding, setIsAdding] = useState(false);
  const [editingKey, setEditingKey] = useState<React.Key>("");
  const isEditing = (record: any) => record.key === editingKey;
  const [vouchers, setVouchers] = useState<any[]>([]);
  const [api, contextHolder] = notification.useNotification();
    const [filteredVouchers, setFilteredVouchers] = useState(vouchers);

  type NotificationType = "success" | "info" | "warning" | "error";

  const openNotification = (
    type: NotificationType,
    method: string,
    error: string
  ) => {
    api[type]({
      message: type === "success" ? "Notification" : "Error",
      description:
        type === "success" ? `${method} material successfully` : error,
    });
  };

  const fetchData = async () => {
  try {
    const response = await showAllVoucher();

    const data = response.data;

    console.log("Voucher data:", data); // debug xem có dữ liệu không

    const formattedVoucher = data.map((voucher: any) => ({
      key: voucher.id,
      promotionID: voucher.id,
      name: voucher.name,
      discountValue: voucher.discountValue,
      startDate: voucher.startDate,
      endDate: voucher.endDate,
      description: voucher.description,
    }));

    setVouchers(formattedVoucher);
  } catch (error) {
    console.error("Failed to fetch vouchers:", error);
  }
};

  useEffect(() => {
    fetchData();
  }, []);


  // EDIT
  const edit = (record: Partial<any> & { key: React.Key }) => {
    form.setFieldsValue({
      name: "",
      discountValue: "",
      startDate: "",
      endDate: "",
      description: "",
      ...record,
    });
    setEditingKey(record.key);
  };

  const cancel = () => {
    setEditingKey("");
  };

  


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
      title: "Description",
      dataIndex: "description",
      editable: true,
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
        inputType: col.dataIndex === "discountValue" ? "number" : "text",
        dataIndex: col.dataIndex,
        title: col.title,
        editing: isEditing(record),
      }),
    };
  });

  // SEARCH AREA
  const onSearch = (value: string) => {
    const filtered = vouchers.filter(voucher =>
      voucher.name.toLowerCase().includes(value.toLowerCase())
    );
    setFilteredVouchers(filtered);
  };
  useEffect(() => {
    setFilteredVouchers(vouchers);
  }, [vouchers]);

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
                      placeholder="Search here..."
                      value={searchText}
                      onChange={(e) => setSearchText(e.target.value)}
                      onKeyDown={handleKeyPress}
                      prefix={<SearchOutlined className="searchIcon" />}
                    />
                  </Styled.SearchArea>
                  
            </Styled.AdPageContent_Head>

            <Styled.AdminTable>
                <Form form={form} component={false}>
                  <Table
                    components={{
                      body: {
                        cell: EditableCell,
                      },
                    }}
                    bordered
                    dataSource={filteredVouchers}
                    columns={mergedColumns}
                    rowClassName="editable-row"
                    pagination={{
                      // onChange: cancel,
                      pageSize: 6,
                    }}
                  />
                </Form>
            </Styled.AdminTable>
          </Styled.AdPageContent>
        </Styled.AdminPage>
      </Styled.ProductAdminArea>
    </>
  );
};

export default BillPromotion;
