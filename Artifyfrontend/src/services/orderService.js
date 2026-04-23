import axios from '../api/axios';

const orderService = {
    getBuyerOrders: async () => {
        const response = await axios.get('/buyer/orders');
        return response.data;
    },

    getOrderDetails: async (orderId) => {
        const response = await axios.get(`/buyer/orders/${orderId}`);
        return response.data;
    }
};

export default orderService;
