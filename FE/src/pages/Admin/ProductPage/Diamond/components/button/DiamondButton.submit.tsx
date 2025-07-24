import { useAppDispatch } from "@/hooks";
import { createDiamond } from "@/services/diamondAPI";
import { Button, Form, FormInstance } from "antd";
import { NotificationInstance } from "antd/es/notification/interface";
import React, { useState } from "react";
import uploadSlice from "../slice";
import { createProduct } from "@/services/productAPI";

interface SubmitButtonProps {
  form: FormInstance;
  current: number;
  setCurrent: (value: number) => void;
  api: NotificationInstance;
}

const SubmitButton: React.FC<React.PropsWithChildren<SubmitButtonProps>> = ({
  form,
  children,
  setCurrent,
  current,
  api,
}) => {
  // const [submittable, setSubmittable] = React.useState<boolean>(false);
  const [submittable, setSubmittable] = useState(false);
  const dispatch = useAppDispatch();

  // Watch all values
  const values = Form.useWatch([], form);
  React.useEffect(() => {
    form
      .validateFields({ validateOnly: true })
      .then(() => setSubmittable(true))
      .catch(() => setSubmittable(false));
  }, [values]);

  const diamondValues: object = {
    ...values,
    UpdateTime: new Date(),
    JewelrySettingID: null,
  };
  console.log(diamondValues);

  const addDiamond = async (diamondValues: object) => {
    try {
      const { data } = await createProduct(diamondValues);

      if (!data.success) {
        throw new Error(data.error || "Unknown error");
      }

      api.success({
        message: "Notification",
        description: "Create information successfully!",
      });

      setCurrent(current + 1);

      // Nếu bạn vẫn cần dispatch diamondID:
      if (data.productId) {
        dispatch(uploadSlice.actions.setDiamondID(data.productId));
      }
    } catch (error: any) {
      api.error({
        message: "Error",
        description: error.message || "An error occurred!",
      });
    }
  };

  return (
    <Button
      type="primary"
      htmlType="submit"
      disabled={!submittable}
      onClick={() => addDiamond(diamondValues)}
    >
      {children}
    </Button>
  );
};

export default SubmitButton;
