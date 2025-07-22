import { createVnPayPayment } from "@/services/paymentAPI";
import { Button } from "antd";
import { useState } from "react";

interface VnPayButtonProps {
    orderId: string;
    amount: number;
    onSuccess: (details: any) => void;
    onError: (error: any) => void;
}

const VnPayButton: React.FC<VnPayButtonProps> = ({
    orderId,
    amount,
    onSuccess,
    onError
}) => {
    const [loading, setLoading] = useState(false);
    
    const handleClick = async () => {
        setLoading(true);
        try {
            const { data } = await createVnPayPayment(orderId, amount);
            window.location.href = data.paymentUrl;
            onSuccess(data); // Assuming the success callback might need the data from the create payment response
        } catch (error: any) {
            console.error(error);
            onError(error.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <Button
            onClick={handleClick}
            disabled={loading}
            htmlType="submit"
            type="primary"
        >
            {loading ? 'Processing...' : 'Pay with VNPay'}
        </Button>
    )
};

export default VnPayButton;