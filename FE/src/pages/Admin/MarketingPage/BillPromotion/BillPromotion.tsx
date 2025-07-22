import * as Styled from "./BillPromotion.styled";
import React, { useEffect, useState } from "react";
import { SearchOutlined, PlusCircleOutlined, SaveOutlined } from "@ant-design/icons";
import type { FormInstance } from "antd";
import dayjs from "dayjs";

import {
  Form,
  Input,
  InputNumber,
  Popconfirm,
  Table,
  Typography,
  Button,
  Space,
  DatePicker,
  notification,
  Select
} from "antd";
import Sidebar from "../../../../components/Admin/Sidebar/Sidebar";
import MarketingMenu from "@/components/Admin/MarketingMenu/MarketingMenu";
import { createVoucher, deleteVoucher, showAllVoucher, updateVoucher } from "@/services/voucherAPI";
import { DatePickerProps } from "antd/lib";

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

  const save = async (key: React.Key) => {
    try {
      const row = (await form.validateFields()) as any;
      const newData = [...vouchers];
      const index = newData.findIndex((item) => key === item.key);

      if (index > -1) {
        const item = newData[index];
        const updatedItem = {
          name: row.Name,
          discountValue: row.DiscountValue,
          startDate: row.StartDate,
          endDate: row.EndDate,
          description: row.Description,
          // UpdateTime: new Date().toISOString(),
        };
        newData.splice(index, 1, {
          ...item,
          ...row,
        });
        setVouchers(newData);
        await updateVoucher(item.promotionID, updatedItem);
        openNotification("success", "Update", "");
      } else {
        newData.push(row);
        setVouchers(newData);
        openNotification("error", "Update", "Failed to update type");
      }
      setEditingKey("");
    } catch (errInfo) {
      console.log("Validate Failed:", errInfo);
    }
  };

  // const handleDelete = async (voucherID: number) => {
  //   try {
      
  //     await deleteVoucher(voucherID);
  //     openNotification("success", "Delete", "");
  //     fetchData();
  //   } catch (error: any) {
  //     console.error("Failed to delete collection:", error);
  //     openNotification("error", "Delete", error.message);
  //   }
  // };
  const handleDelete = async (voucherID: number) => {
  try {
    await deleteVoucher(voucherID);
    openNotification("success", "Delete", "");
    // Cập nhật lại danh sách vouchers mà không cần gọi lại API
    setVouchers(prev => prev.filter(v => v.promotionID !== voucherID));
  } catch (error: any) {
    console.error("Failed to delete collection:", error);
    openNotification("error", "Delete", error.message);
  }
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
    // {
    //   title: "Start Date",
    //   dataIndex: "startDate",
    //   editable: true,
    //   onChange: { onChangeDate },
    //   render: (_: any, { startDate }: any) => {
    //     return <>{startDate.replace("T", " ").replace(".000Z", " ")}</>
    //   },
    //   sorter: (a: any, b: any) =>
    //     a.startDate.length - b.startDate.length,
      
    // },
    // {
    //   title: "End Date",
    //   dataIndex: "endDate",
    //   editable: true,
    //   onChange: { onChangeDate },
    //   render: (_: any, { endDate }: any) => {
    //     // return <>{endDate.replace("T", " ").replace(".000Z", " ")}</>
    //     return <>{endDate ? endDate.replace("T", " ").replace(".000Z", " ") : "N/A"}</>
    //   },
    //   sorter: (a: any, b: any) =>
    //     a.endDate.length - b.endDate.length,
    // },
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
    {
      title: "Edit",
      dataIndex: "edit",
      className: "TextAlign SmallSize",
      render: (_: unknown, record: any) => {
        const editable = isEditing(record);
        return editable ? (
          <span>
            <Typography.Link
              onClick={() => save(record.key)}
              style={{ marginRight: 8 }}
            >
              Save
            </Typography.Link>
            <Popconfirm title="Sure to cancel?" onConfirm={cancel}>
              <a>Cancel</a>
            </Popconfirm>
          </span>
        ) : (
          <Typography.Link
            disabled={editingKey !== ""}
            onClick={() => edit(record)}
          >
            Edit
          </Typography.Link>
        );
      },
    },
    {
      title: "Delete",
      dataIndex: "delete",
      className: "TextAlign",
      render: (_: unknown, record: any) =>
        vouchers.length >= 1 ? (
          <Popconfirm
            title="Sure to delete?"
            
            onConfirm={() => handleDelete(record.key)}
          >
            <a>Delete</a>
          </Popconfirm>
        ) : null,
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
    console.log("Search:", value);
  };

  const handleKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      onSearch(searchText);
    }
  };

  // MOVE ADD NEW
  const handleAddNew = () => {
    setIsAdding(true);
    form.resetFields();
  };

  const handleCancel = () => {
    setIsAdding(false);
  };

      // const voucherValues = await form.validateFields();

        // const newVoucher = {
        //   promotionDto: {
        //     name: voucherValues.Name,
        //     description: voucherValues.Description,
        //     startDate: voucherValues.StartDate?.toISOString(),
        //     endDate: voucherValues.EndDate?.toISOString(),
        //     discountType: "Percentage", // hoặc thêm field chọn từ Form nếu cần
        //     discountValue: voucherValues.DiscountValue,
        //     // appliesToProductId: selectedProductId, // thêm từ UI nếu cần chọn product
        //   },
        // };


 interface SubmitButtonProps {
    form: FormInstance;
  }

  const SubmitButton: React.FC<React.PropsWithChildren<SubmitButtonProps>> = ({
    form,
    children,
  }) => {
    // const [submittable, setSubmittable] = React.useState<boolean>(false);
    const [submittable, setSubmittable] = useState(false);
    const values = Form.useWatch([], form);

    useEffect(() => {
      form
        .validateFields({ validateOnly: true })
        .then(() => setSubmittable(true))
        .catch(() => setSubmittable(false));
    }, [values]);

    const addDiscount = async () => {
      try {
        const discountValues = await form.validateFields();
        const newDiscount = {
          ...discountValues,
        };

        const { data } = await createVoucher(newDiscount);
        if (data.statusCode !== 200) throw new Error(data.message);
        fetchData();
        setIsAdding(false);
        openNotification("success", "Add", "");
      } catch (error: any) {
        openNotification("error", "", error.message);
      }
    };

    return (
      <Button
        type="primary"
        htmlType="submit"
        disabled={!submittable}
        onClick={addDiscount}
      >
        {children}
      </Button>
    );
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
            <Styled.AdPageContent_Head>
              {(!isAdding && (
                <>
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
                  <Styled.AddButton>
                    <button onClick={handleAddNew}>
                      <PlusCircleOutlined />
                      Add New Product Promotion
                    </button>
                  </Styled.AddButton>
                </>
              )) || (
                  <>
                    <Styled.AddContent_Title>
                      <p>Add Collection</p>
                    </Styled.AddContent_Title>
                  </>
                )}
            </Styled.AdPageContent_Head>

            <Styled.AdminTable>
              {isAdding ? (
                <>
                  <Form
                    form={form}
                    layout="vertical"
                    className="AdPageContent_Content"
                  // autoComplete="off"
                  >
                    <Styled.FormItem>
                      <Form.Item
                        label="Promotion Name"
                        name="Name"
                        rules={[
                          {
                            required: true,
                            message: "Promotion Name is required.",
                          },
                          {
                            type: "string",
                            message: "Only alphabet is allowed.",
                          },
                          {
                            max: 100,
                            message:
                              "Promotion Name must be at most 300 characters long.",
                          },
                          {
                            whitespace: true,
                            message: "Not only has whitespace.",
                          },
                        ]}
                      >
                        <Input className="formItem" placeholder="Rose" />
                      </Form.Item>
                    </Styled.FormItem>
                    <Styled.FormItem>
                      <Form.Item
                        label="% discount"
                        name="DiscountValue"
                        rules={[{ required: true }]}
                      >
                        {/* <InputNumber className="formItem" placeholder="15" />
                         */}
                           <Input className="formItem" placeholder="15" />
                      </Form.Item>
                    </Styled.FormItem>
                    <Styled.FormItem>
                      <Form.Item
                        label="Discount Type"
                        name="DiscountType"
                        rules={[{ required: true }]}
                      >
                        {/* <InputNumber className="formItem" placeholder="15" />
                         */}
                           <Input className="formItem" placeholder="Percentage" />
                      </Form.Item>
                    </Styled.FormItem>
                    <Styled.FormItem>
                      <Form.Item
                        label="Start Date"
                        name="StartDate"
                        rules={[{ required: true }]}
                      >
                        <DatePicker
                          onChange={onChangeDate}
                          className="formItem"
                        />
                      </Form.Item>
                    </Styled.FormItem>
                    <Styled.FormItem>
                      <Form.Item
                        label="End Date"
                        name="EndDate"
                        rules={[{ required: true }]}
                      >
                        <DatePicker
                          onChange={onChangeDate}
                          className="formItem"
                        />
                      </Form.Item>
                    </Styled.FormItem>
                    <Styled.FormDescript>
                      <Form.Item
                        label="Description"
                        name="Description"
                        rules={[{ required: true }]}
                      >
                        <Input.TextArea className="formItem" />
                      </Form.Item>
                    </Styled.FormDescript>
                    <Styled.ActionBtn>
                      <Form.Item>
                        <Space>
                          <SubmitButton form={form}>
                            <SaveOutlined />
                            Save
                          </SubmitButton>
                          <Button
                            onClick={handleCancel}
                            className="CancelBtn"
                            style={{ marginLeft: "10px" }}
                          >
                            Cancel
                          </Button>
                        </Space>
                      </Form.Item>
                    </Styled.ActionBtn>
                  </Form>
                </>
              ) : (
                <Form form={form} component={false}>
                  <Table
                    components={{
                      body: {
                        cell: EditableCell,
                      },
                    }}
                    bordered
                    dataSource={vouchers}
                    columns={mergedColumns}
                    rowClassName="editable-row"
                    pagination={{
                      onChange: cancel,
                      pageSize: 6,
                    }}
                  />
                </Form>
              )}
            </Styled.AdminTable>
          </Styled.AdPageContent>
        </Styled.AdminPage>
      </Styled.ProductAdminArea>
    </>
  );
};


export default BillPromotion;



//   // SUBMIT FORM
//   interface SubmitButtonProps {
//     form: FormInstance;
//   }

//   const SubmitButton: React.FC<React.PropsWithChildren<SubmitButtonProps>> = ({
//     form,
//     children,
//   }) => {
//     // const [submittable, setSubmittable] = React.useState<boolean>(false);
//     const [submittable, setSubmittable] = useState(false);
//     const values = Form.useWatch([], form);

//     useEffect(() => {
//       form
//         .validateFields({ validateOnly: true })
//         .then(() => setSubmittable(true))
//         .catch(() => setSubmittable(false));
//     }, [values]);

//     const addVoucher = async () => {
//       try {
//         const voucherValues = await form.validateFields();
//         const newVoucher = {
//              promotionDto: {
//           ...voucherValues,
//              },
//         };

    
//         const { data } = await createVoucher(newVoucher);
//         if (data.statusCode !== 200) throw new Error(data.message);
//         fetchData();
//         setIsAdding(false);
//         openNotification("success", "Add", "");
//       } catch (error: any) {
//         openNotification("error", "", error.message);
//       }
//     };

//     return (
//       <Button
//         type="primary"
//         htmlType="submit"
//         disabled={!submittable}
//         onClick={addVoucher}
//       >
//         {children}
//       </Button>
//     );
//   };

//   return (
//     <>
//       {contextHolder}
//       <Styled.GlobalStyle />
//       <Styled.ProductAdminArea>
//         <Sidebar />
//         <Styled.AdminPage>
//           <MarketingMenu />
//           <Styled.AdPageContent>
//             <Styled.AdPageContent_Head>
//               {(!isAdding && (
//                 <>
//                   <Styled.SearchArea>
//                     <Input
//                       className="searchInput"
//                       type="text"
//                       placeholder="Search here..."
//                       value={searchText}
//                       onChange={(e) => setSearchText(e.target.value)}
//                       onKeyPress={handleKeyPress}
//                       prefix={<SearchOutlined className="searchIcon" />}
//                     />
//                   </Styled.SearchArea>
//                   <Styled.AddButton>
//                     <button onClick={handleAddNew}>
//                       <PlusCircleOutlined />
//                       Add New Promotion
//                     </button>
//                   </Styled.AddButton>
//                 </>
//               )) || (
//                   <>
//                     <Styled.AddContent_Title>
//                       <p>Add Promotion</p>
//                     </Styled.AddContent_Title>
//                   </>
//                 )}
//             </Styled.AdPageContent_Head>

//             <Styled.AdminTable>
//               {isAdding ? (
//                 <>
//                   <Form
//                     form={form}
//                     layout="vertical"
//                     className="AdPageContent_Content"
//                   >
//                     <Styled.FormItem>
//                       <Form.Item
//                         label="Name"
//                         name="Name"
//                         rules={[{ required: true }]}
//                       >
//                         <Input className="formItem" placeholder="Sophia" />
//                       </Form.Item>
//                     </Styled.FormItem>
//                     <Styled.FormItem>
//                       <Form.Item
//                         label="% discount"
//                         name="DiscountValue"
//                         rules={[{ required: true }]}
//                       >
//                         <InputNumber className="formItem" placeholder="15" />
//                       </Form.Item>
//                     </Styled.FormItem>
//                     {/* <Form.Item label="Discount Type" name="DiscountType" rules={[{ required: true }]}>
//                     <Select>
//                       <Select.Option value="Percentage">Percentage</Select.Option>
//                       <Select.Option value="Amount">Amount</Select.Option>
//                     </Select>
//                   </Form.Item> */}

            

//                     <Styled.FormItem>
//                       <Form.Item
//                         label="Start Time"
//                         name="StartDate"
//                         rules={[{ required: true }]}
//                       >
//                         <DatePicker
//                           onChange={onChangeDate}
//                           className="formItem"
//                         />
//                       </Form.Item>
//                     </Styled.FormItem>
//                     <Styled.FormItem>
//                       <Form.Item
//                         label="End Time"
//                         name="EndDate"
//                         rules={[{ required: true }]}
//                       >
//                         <DatePicker
//                           onChange={onChangeDate}
//                           className="formItem"
//                         />
//                       </Form.Item>
//                     </Styled.FormItem>
//                     <Styled.FormDescript>
//                       <Form.Item
//                         label="Description"
//                         name="Description"
//                         rules={[{ required: true }]}
//                       >
//                         <Input.TextArea className="formItem" />
//                       </Form.Item>
//                     </Styled.FormDescript>
//                   </Form>

//                   <Styled.ActionBtn>
//                     <Form.Item>
//                       <Space>
//                         <SubmitButton form={form}>
//                           <SaveOutlined />
//                           Save
//                         </SubmitButton>
//                         <Button
//                           onClick={handleCancel}
//                           className="CancelBtn"
//                           style={{ marginLeft: "10px" }}
//                         >
//                           Cancel
//                         </Button>
//                       </Space>
//                     </Form.Item>
//                   </Styled.ActionBtn>
//                 </>
//               ) : (
//                 <Form form={form} component={false}>
//                   <Table
//                     components={{
//                       body: {
//                         cell: EditableCell,
//                       },
//                     }}
//                     bordered
//                     dataSource={vouchers}
//                     columns={mergedColumns}
//                     rowClassName="editable-row"
//                     pagination={{
//                       onChange: cancel,
//                       pageSize: 6,
//                     }}
//                   />
//                 </Form>
//               )}
//             </Styled.AdminTable>
//           </Styled.AdPageContent>
//         </Styled.AdminPage>
//       </Styled.ProductAdminArea>
//     </>
//   );
// };

