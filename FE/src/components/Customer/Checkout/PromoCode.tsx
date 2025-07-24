
import { DownOutlined } from "@ant-design/icons";
import React, { useEffect, useState } from "react";
import styled from "styled-components";
import { Select, notification } from "antd"; // Import notification
import { showAllVoucher } from "@/services/voucherAPI"; 
import { useAppDispatch } from "@/hooks"; 
import { setVoucherID } from "@/store/slices/orderSlice";

import dayjs from "dayjs";
import isSameOrBefore from 'dayjs/plugin/isSameOrBefore'; 
dayjs.extend(isSameOrBefore);

interface PromoCodeSectionProps {
  onApplyVoucher: (discount: number, voucherID: string) => void;
}


interface Voucher {
  id: string; 
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