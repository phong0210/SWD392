// /* eslint-disable @typescript-eslint/no-explicit-any */
// import { DownOutlined } from "@ant-design/icons";
// import React, { useEffect, useState } from "react";
// import styled from "styled-components";
// import { Select } from "antd";
// import { showAllVoucher } from "@/services/voucherAPI";
// import { useAppDispatch } from "@/hooks";
// import  orderSlice  from "@/store/slices/orderSlice";
// interface PromoCodeSectionProps {
//   onApplyVoucher: (discount: number, voucherID: number) => void;
// }

// const PromoCodeSection: React.FC<PromoCodeSectionProps> = ({
//   onApplyVoucher,
// }) => {
//   const [isCollapsed, setIsCollapsed] = useState(true);
//   const [selectedVoucher, setSelectedVoucher] = useState<Voucher | null>(null);
//   const [error, setError] = useState("");
//   const [availableVouchers, setAvailableVouchers] = useState<any[]>([]);
//   const dispatch = useAppDispatch();

//   // const handleSelectVoucher = (voucher: Voucher) => {
//   //   localStorage.setItem("selectedVoucher", JSON.stringify(voucher));
//   // }
//   // console.log(selectedVoucher);
//   // console.log(handleSelectVoucher);

//   interface Voucher {
//     buttonLabel: string | null;
//     PromotionID: string;
//     Name: string;
//     Description: string;
//     StartDate: string;
//     EndDate: string;
//     DiscountType : string;
//     DiscountValue: string;
//   }

//   const toggleCollapse = () => {
//     setIsCollapsed(!isCollapsed);
//   };

//   const getAllVouchers = async () => {
//     try {
//       const { data } = await showAllVoucher();
//       const filteredVouchers = filterValidVouchers(data.data);

//       setAvailableVouchers(filteredVouchers);
//       console.log(availableVouchers);
//     } catch (error) {
//       console.error(error);
//     }
//   };
//   useEffect(() => {
//     getAllVouchers();
//   }, []);

//   const filterValidVouchers = (vouchers: Voucher[]) => {
//     const currentDate = new Date();
//     return vouchers.filter(
//       (voucher) => new Date(voucher.EndDate) > currentDate
//     );
//   };

//   const handleApplyClick = () => {
//     if (selectedVoucher) {
//       const discount = parseFloat(selectedVoucher.DiscountValue);
//       console.log("Selected Voucher:", selectedVoucher);
//       console.log("VoucherID", selectedVoucher.PromotionID);
//       console.log("Discount Value:", discount);

//       localStorage.removeItem("selectedVoucher");

//       dispatch(orderSlice.actions.setVoucherID(selectedVoucher.PromotionID));

//       if (!isNaN(discount) && discount > 0) {
//         onApplyVoucher(discount, parseInt(selectedVoucher.PromotionID, 10));
//         setError("");
//         localStorage.setItem(
//           "selectedVoucher",
//           JSON.stringify(selectedVoucher)
//         );
//       } else {
//         setError("Invalid discount value");
//       }
//     } else {
//       setError("Please select a valid promo code");
//       localStorage.removeItem("selectedVoucher");
//       onApplyVoucher(0, 0);
//     }
//   };

//   return (
//     <PromoCodeContainer>
//       <CollapseButton onClick={toggleCollapse}>
//         Promo Code{" "}
//         <span>
//           <DownOutlined />
//         </span>
//       </CollapseButton>
//       {!isCollapsed && (
//         <PromoForm>
//           <Select
//             allowClear
//             placeholder="Select a promo code"
//             style={{ width: "100%" }}
//             onChange={(value) => {
//               const voucher = availableVouchers.find(
//                 (v) => v.Description === value
//               );
//               setSelectedVoucher(voucher || null);
//               if (!voucher) {
//                 setError("");
//                 onApplyVoucher(0, 0);
//                 localStorage.removeItem("selectedVoucher");
//               }
//             }}
//           >
//             {availableVouchers.map((voucher) => (
//               <Select.Option
//                 key={voucher.PromotionID}
//                 value={voucher.Description}
//               >
//                 {voucher.Description}
//               </Select.Option>
//             ))}
//           </Select>
//           <BtnApply onClick={handleApplyClick}>Apply</BtnApply>
//           {error && <ErrorText>{error}</ErrorText>}
//         </PromoForm>
//       )}
//     </PromoCodeContainer>
//   );
// };

// export default PromoCodeSection;

// const PromoCodeContainer = styled.div`
//   border-top: 1px solid rgba(0, 0, 0, 1);
//   border-bottom: 1px solid rgba(0, 0, 0, 1);
// `;

// const BtnApply = styled.div`
//   display: flex;
//   justify-content: center;
//   align-items: center;
//   height: 34px;
//   font-size: 15px;
//   padding: 7px 20px;
//   background-color: #151542;
//   color: #ffffff;
//   border: 1px solid #151542;
//   cursor: pointer;
//   transition: background-color 0.3s ease;
//   font-family: "Gantari", sans-serif;
//   font-weight: 400;
//   transition: all 0.45s ease;
//   &:hover {
//     color: #000000;
//     background-color: #efefef;
//     transition: all 0.45s ease;
//   }
// `;

// const CollapseButton = styled.div`
//   cursor: pointer;
//   align-items: center;
//   background-color: #fff;
//   width: 100%;
//   font-size: 17px;
// `;

// const PromoForm = styled.div`
//   display: flex;
//   flex-direction: column;
//   gap: 10px;
// `;

// const ErrorText = styled.p`
//   color: red;
//   font-size: 17px;
// `;
/* eslint-disable @typescript-eslint/no-explicit-any */
// import { DownOutlined } from "@ant-design/icons";
// import React, { useEffect, useState } from "react";
// import styled from "styled-components";
// import { Select } from "antd";
// import { showAllVoucher } from "@/services/voucherAPI"; // Đảm bảo đường dẫn đúng
// import { useAppDispatch } from "@/hooks"; // Đảm bảo đường dẫn đúng
// import orderSlice from "@/store/slices/orderSlice"; // Đảm bảo đường dẫn đúng
// import dayjs from "dayjs"; // Sử dụng dayjs để xử lý ngày tháng dễ hơn

// interface PromoCodeSectionProps {
//   onApplyVoucher: (discount: number, voucherID: string) => void; // Thay đổi voucherID từ number sang string vì PromotionID là string
// }

// const PromoCodeSection: React.FC<PromoCodeSectionProps> = ({
//   onApplyVoucher,
// }) => {
//   const [isCollapsed, setIsCollapsed] = useState(true);
//   const [selectedVoucher, setSelectedVoucher] = useState<Voucher | null>(null);
//   const [error, setError] = useState("");
//   const [availableVouchers, setAvailableVouchers] = useState<Voucher[]>([]); // Khai báo rõ ràng kiểu mảng Voucher
//   const dispatch = useAppDispatch();

//   // Định nghĩa lại interface Voucher để khớp với dữ liệu API thực tế
//   // Dựa trên cấu trúc dữ liệu voucher từ các cuộc trò chuyện trước:
//   interface Voucher {
//     id: string; // Tên thật của PromotionID
//     name: string;
//     description: string;
//     startDate: string; // dayjs có thể xử lý string ISO 8601
//     endDate: string; // dayjs có thể xử lý string ISO 8601
//     discountValue: number; // Đây là số, không phải string
//     // appliesToProductId: number | null; // Có thể thêm nếu cần hiển thị
//   }

//   const toggleCollapse = () => {
//     setIsCollapsed(!isCollapsed);
//   };

//   const filterValidVouchers = (vouchers: any[]): Voucher[] => {
//     const currentDate = dayjs(); // Sử dụng dayjs cho ngày hiện tại
//     return vouchers
//       .filter((voucher) => {
//         const endDate = dayjs(voucher.endDate);
//         const startDate = dayjs(voucher.startDate);
//         // Lọc các voucher hợp lệ: ngày kết thúc chưa qua và ngày bắt đầu đã đến hoặc đã qua
//         return endDate.isAfter(currentDate) && startDate.isSameOrBefore(currentDate);
//       })
//       .map(voucher => ({ // Map lại để đảm bảo đúng interface Voucher đã định nghĩa
//         id: voucher.id,
//         name: voucher.name,
//         description: voucher.description,
//         startDate: voucher.startDate,
//         endDate: voucher.endDate,
//         discountValue: voucher.discountValue,
//       }));
//   };

//   const getAllVouchers = async () => {
//     try {
//       // showAllVoucher() trả về { data: { data: [...] } } hoặc trực tiếp là mảng data?
//       // Dựa trên các cuộc trò chuyện trước, có vẻ như `response.data` là mảng trực tiếp.
//       // Tôi sẽ giả định `response.data` là một mảng các object voucher.
//       const response = await showAllVoucher();
//       const fetchedData = response.data; // Giả sử response.data là mảng voucher
      
//       console.log("Fetched Vouchers (raw):", fetchedData); // Debugging
      
//       const filtered = filterValidVouchers(fetchedData);
//       setAvailableVouchers(filtered);
//       console.log("Available Vouchers (filtered):", filtered); // Debugging
//     } catch (error) {
//       console.error("Error fetching vouchers:", error);
//       setError("Failed to load promo codes.");
//     }
//   };

//   useEffect(() => {
//     getAllVouchers();
//   }, []);

//   const handleApplyClick = () => {
//     if (selectedVoucher) {
//       const discount = selectedVoucher.discountValue; // discountValue đã là number
//       console.log("Selected Voucher:", selectedVoucher);
//       console.log("VoucherID:", selectedVoucher.id);
//       console.log("Discount Value:", discount);

//       // Xóa item cũ (nếu có)
//       localStorage.removeItem("selectedVoucher");

//       dispatch(orderSlice.actions.setVoucherID(selectedVoucher.id));

//       if (typeof discount === 'number' && discount > 0) { // Kiểm tra lại kiểu và giá trị
//         onApplyVoucher(discount, selectedVoucher.id);
//         setError("");
//         localStorage.setItem(
//           "selectedVoucher",
//           JSON.stringify(selectedVoucher)
//         );
//       } else {
//         setError("Invalid discount value for selected promo code.");
//         localStorage.removeItem("selectedVoucher");
//         onApplyVoucher(0, ""); // Đảm bảo reset nếu giá trị không hợp lệ
//       }
//     } else {
//       setError("Please select a valid promo code.");
//       localStorage.removeItem("selectedVoucher");
//       onApplyVoucher(0, ""); // Reset discount và voucherID
//     }
//   };

//   return (
//     <PromoCodeContainer>
//       <CollapseButton onClick={toggleCollapse}>
//         Promo Code{" "}
//         <span>
//           <DownOutlined />
//         </span>
//       </CollapseButton>
//       {!isCollapsed && (
//         <PromoForm>
//           <Select
//             allowClear
//             placeholder="Select a promo code"
//             style={{ width: "100%" }}
//             onChange={(selectedId: string) => { // Thay đổi kiểu tham số onChange thành string (id của voucher)
//               const voucher = availableVouchers.find(
//                 (v) => v.id === selectedId // Tìm voucher theo id
//               );
//               setSelectedVoucher(voucher || null);
//               if (!voucher) { // Nếu không chọn voucher nào (clear selection)
//                 setError("");
//                 onApplyVoucher(0, ""); // Reset discount và voucherID
//                 localStorage.removeItem("selectedVoucher");
//               }
//             }}
//             value={selectedVoucher?.id || undefined} // Gán giá trị được chọn là id của voucher
//           >
//             {availableVouchers.map((voucher) => (
//               <Select.Option
//                 key={voucher.id} // Key là id
//                 value={voucher.id} // Value là id
//               >
//                 {voucher.name} - {voucher.description} ({voucher.discountValue}%) {/* Hiển thị tên và mô tả dễ hiểu hơn */}
//               </Select.Option>
//             ))}
//           </Select>
//           <BtnApply onClick={handleApplyClick}>Apply</BtnApply>
//           {error && <ErrorText>{error}</ErrorText>}
//         </PromoForm>
//       )}
//     </PromoCodeContainer>
//   );
// };

// export default PromoCodeSection;

// const PromoCodeContainer = styled.div`
//   border-top: 1px solid rgba(0, 0, 0, 0.1); /* Giảm độ đậm border */
//   border-bottom: 1px solid rgba(0, 0, 0, 0.1); /* Giảm độ đậm border */
//   padding: 10px 0; /* Thêm padding cho dễ nhìn */
// `;

// const BtnApply = styled.div`
//   display: flex;
//   justify-content: center;
//   align-items: center;
//   height: 38px; /* Tăng chiều cao nút */
//   font-size: 16px; /* Tăng cỡ chữ */
//   padding: 7px 20px;
//   background-color: #151542;
//   color: #ffffff;
//   border: 1px solid #151542;
//   border-radius: 4px; /* Bo tròn nút */
//   cursor: pointer;
//   transition: background-color 0.3s ease;
//   font-family: "Gantari", sans-serif;
//   font-weight: 500; /* Đậm hơn một chút */
//   transition: all 0.3s ease; /* Thống nhất transition */
//   &:hover {
//     color: #151542; /* Màu chữ khi hover */
//     background-color: #efefef;
//     border-color: #151542; /* Màu border khi hover */
//   }
// `;

// const CollapseButton = styled.div`
//   cursor: pointer;
//   display: flex; /* Dùng flex để căn chỉnh */
//   justify-content: space-between; /* Đẩy chữ và icon ra hai bên */
//   align-items: center;
//   background-color: #fff;
//   width: 100%;
//   font-size: 17px;
//   padding: 10px 0; /* Thêm padding */
//   font-weight: 500; /* Đậm hơn một chút */
//   color: #333; /* Màu chữ rõ ràng hơn */
//   span {
//     display: flex; /* Đảm bảo icon được căn giữa */
//     align-items: center;
//   }
// `;

// const PromoForm = styled.div`
//   display: flex;
//   flex-direction: column;
//   gap: 15px; /* Tăng khoảng cách giữa các phần tử */
//   margin-top: 15px; /* Khoảng cách từ nút collapse */
//   padding-bottom: 10px; /* Thêm padding dưới form */
// `;

// const ErrorText = styled.p`
//   color: #d32f2f; /* Màu đỏ tiêu chuẩn cho lỗi */
//   font-size: 14px; /* Cỡ chữ nhỏ hơn cho lỗi */
//   margin-top: 5px; /* Khoảng cách từ phần tử trên */
// `;

/* eslint-disable @typescript-eslint/no-explicit-any */
import { DownOutlined } from "@ant-design/icons";
import React, { useEffect, useState } from "react";
import styled from "styled-components";
import { Select, notification } from "antd"; // Import notification
import { showAllVoucher } from "@/services/voucherAPI"; // Đảm bảo đường dẫn đúng
import { useAppDispatch } from "@/hooks"; // Đảm bảo đường dẫn đúng
// Thay đổi import này để lấy action creator trực tiếp
// Giả định orderSlice.ts export: export const { setVoucherID } = orderSlice.actions;
import { setVoucherID } from "@/store/slices/orderSlice";// Đảm bảo đường dẫn đúng và cách export

import dayjs from "dayjs";
import isSameOrBefore from 'dayjs/plugin/isSameOrBefore'; // Import plugin nếu chưa có
dayjs.extend(isSameOrBefore);

interface PromoCodeSectionProps {
  onApplyVoucher: (discount: number, voucherID: string) => void;
}

// Định nghĩa lại interface Voucher để khớp với dữ liệu API thực tế
// Dựa trên cấu trúc dữ liệu voucher từ các cuộc trò chuyện trước:
interface Voucher {
  id: string; // Tên thật của PromotionID
  name: string;
  description: string;
  startDate: string;
  endDate: string;
  discountValue: number;
  // appliesToProductId: number | null; // Có thể thêm nếu cần hiển thị
}

const PromoCodeSection: React.FC<PromoCodeSectionProps> = ({
  onApplyVoucher,
}) => {
  const [isCollapsed, setIsCollapsed] = useState(true);
  const [selectedVoucher, setSelectedVoucher] = useState<Voucher | null>(null);
  const [error, setError] = useState("");
  const [availableVouchers, setAvailableVouchers] = useState<Voucher[]>([]);
  const dispatch = useAppDispatch();
  const [api, contextHolder] = notification.useNotification(); // Để hiển thị thông báo lỗi/thành công

  const openNotificationWithIcon = (type: 'success' | 'error', message: string, description?: string) => {
    api[type]({
      message: message,
      description: description,
    });
  };

  const toggleCollapse = () => {
    setIsCollapsed(!isCollapsed);
  };

  const filterValidVouchers = (vouchers: any[]): Voucher[] => {
    const currentDate = dayjs();
    return vouchers
      .filter((voucher: any) => {
        // Kiểm tra xem các trường ngày tháng có tồn tại và hợp lệ không
        if (!voucher || !voucher.endDate || !voucher.startDate) {
          console.warn("Invalid voucher data (missing dates):", voucher);
          return false;
        }

         if (typeof voucher.discountValue === 'undefined' || voucher.discountValue === null) {
          console.warn("Voucher missing discountValue:", voucher);
          return false; 
        }
        
        const endDate = dayjs(voucher.endDate);
        const startDate = dayjs(voucher.startDate);

        // Kiểm tra xem đối tượng dayjs có hợp lệ không
        if (!endDate.isValid() || !startDate.isValid()) {
            console.warn("Invalid date format for voucher:", voucher);
            return false;
        }

        // Lọc các voucher hợp lệ: ngày kết thúc chưa qua và ngày bắt đầu đã đến hoặc đã qua
        return endDate.isAfter(currentDate) && startDate.isSameOrBefore(currentDate);
      })
      .map((voucher: any) => ({
        // Đảm bảo mapping đúng với interface Voucher
        id: voucher.id, // Giả định API trả về 'id' cho PromotionID
        name: voucher.name,
        description: voucher.description,
        startDate: voucher.startDate,
        endDate: voucher.endDate,
        // discountValue: voucher.discountValue,
        discountValue: Number(voucher.discountValue),
      }));
  };

  const getAllVouchers = async () => {
    try {
      const response = await showAllVoucher();
      // Điều chỉnh tùy theo cấu trúc dữ liệu thực tế của bạn
      // Nếu API trả về { data: { data: [...] } }, dùng response.data.data
      // Nếu API trả về { data: [...] }, dùng response.data
      const fetchedData = response.data; // Giả định response.data là mảng voucher

      const filtered = filterValidVouchers(fetchedData);
      setAvailableVouchers(filtered);
    } catch (error) {
      console.error("Error fetching vouchers:", error);
      setError("Failed to load promo codes.");
      openNotificationWithIcon('error', 'Error', 'Failed to load promo codes.');
    }
  };

  useEffect(() => {
    getAllVouchers();
  }, []);

  const handleApplyClick = () => {
    if (selectedVoucher) {
      const discount = selectedVoucher.discountValue;
       console.log("Selected Voucher Discount Value:", discount); // Thêm dòng này
    console.log("Type of discount:", typeof discount); // Và dòng này
      
      localStorage.removeItem("selectedVoucher");

      // Gọi action creator trực tiếp sau khi import từ slice
      dispatch(setVoucherID(selectedVoucher.id)); 

      if (typeof discount === 'number' && discount > 0) {
        onApplyVoucher(discount, selectedVoucher.id);
        setError("");
        localStorage.setItem(
          "selectedVoucher",
          JSON.stringify(selectedVoucher)
        );
        openNotificationWithIcon('success', 'Success', `Promo code "${selectedVoucher.name}" applied!`);
      } else {
        setError("Invalid discount value for selected promo code.");
        localStorage.removeItem("selectedVoucher");
        onApplyVoucher(0, "");
        openNotificationWithIcon('error', 'Error', 'Invalid discount value for the selected promo code.');
      }
    } else {
      setError("Please select a valid promo code.");
      localStorage.removeItem("selectedVoucher");
      onApplyVoucher(0, "");
      openNotificationWithIcon('error', 'Error', 'Please select a valid promo code.');
    }
  };

  return (
    <PromoCodeContainer>
      {contextHolder} {/* Đặt contextHolder ở đây để hiển thị thông báo */}
      <CollapseButton onClick={toggleCollapse}>
        Promo Code{" "}
        <span>
          <DownOutlined />
        </span>
      </CollapseButton>
      {!isCollapsed && (
        <PromoForm>
          <Select
            allowClear
            placeholder="Select a promo code"
            style={{ width: "100%" }}
            onChange={(selectedId: string) => {
              const voucher = availableVouchers.find(
                (v) => v.id === selectedId
              );
              setSelectedVoucher(voucher || null);
              if (!voucher) {
                setError("");
                onApplyVoucher(0, "");
                localStorage.removeItem("selectedVoucher");
              }
            }}
            value={selectedVoucher?.id || undefined}
          >
            {availableVouchers.map((voucher) => (
              <Select.Option
                key={voucher.id}
                value={voucher.id}
              >
                {voucher.name} - {voucher.description} ({voucher.discountValue}%)
              </Select.Option>
            ))}
          </Select>
          <BtnApply onClick={handleApplyClick}>Apply</BtnApply>
          {error && <ErrorText>{error}</ErrorText>}
        </PromoForm>
      )}
    </PromoCodeContainer>
  );
};

export default PromoCodeSection;

const PromoCodeContainer = styled.div`
  border-top: 1px solid rgba(0, 0, 0, 0.1);
  border-bottom: 1px solid rgba(0, 0, 0, 0.1);
  padding: 10px 0;
`;

const BtnApply = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  height: 38px;
  font-size: 16px;
  padding: 7px 20px;
  background-color: #151542;
  color: #ffffff;
  border: 1px solid #151542;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.3s ease;
  font-family: "Gantari", sans-serif;
  font-weight: 500;
  transition: all 0.3s ease;
  &:hover {
    color: #151542;
    background-color: #efefef;
    border-color: #151542;
  }
`;

const CollapseButton = styled.div`
  cursor: pointer;
  display: flex;
  justify-content: space-between;
  align-items: center;
  background-color: #fff;
  width: 100%;
  font-size: 17px;
  padding: 10px 0;
  font-weight: 500;
  color: #333;
  span {
    display: flex;
    align-items: center;
  }
`;

const PromoForm = styled.div`
  display: flex;
  flex-direction: column;
  gap: 15px;
  margin-top: 15px;
  padding-bottom: 10px;
`;

const ErrorText = styled.p`
  color: #d32f2f;
  font-size: 14px;
  margin-top: 5px;
`;