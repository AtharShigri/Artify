import api from '../api/axios';

const initiateEscrow = async (orderId, amount) => {
    try {
        const response = await api.post('/buyer/payment/initiate-escrow', {
            orderId,
            amount
        });
        return response.data;
    } catch (error) {
        console.error('Error initiating escrow:', error);
        throw error;
    }
};

const confirmReceipt = async (orderId) => {
    try {
        const response = await api.post(`/buyer/payment/confirm-receipt/${orderId}`);
        return response.data;
    } catch (error) {
        console.error('Error confirming receipt:', error);
        throw error;
    }
};

export const paymentService = {
    initiateEscrow,
    confirmReceipt,
};
