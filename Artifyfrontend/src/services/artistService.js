import axios from '../api/axios';

const artistService = {
    // Dashboard endpoints
    getSummary: async () => {
        const response = await axios.get('/artist/dashboard/summary');
        return response.data;
    },
    getOrders: async () => {
        const response = await axios.get('/artist/dashboard/orders');
        return response.data;
    },
    getReviews: async () => {
        const response = await axios.get('/artist/dashboard/reviews');
        return response.data;
    },
    getEarnings: async () => {
        const response = await axios.get('/artist/dashboard/earnings');
        return response.data;
    },
    getArtworkStats: async () => {
        const response = await axios.get('/artist/dashboard/artworks/stats');
        return response.data;
    },

    // Profile endpoints
    getProfile: async () => {
        const response = await axios.get('/artist/profile');
        return response.data;
    },
    updateProfile: async (data) => {
        const response = await axios.put('/artist/profile', data);
        return response.data;
    },
    updateProfileImage: async (formData) => {
        const response = await axios.put('/artist/profile-image', formData, {
            headers: { 'Content-Type': 'multipart/form-data' }
        });
        return response.data;
    }
};

export default artistService;
