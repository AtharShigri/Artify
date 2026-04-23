import axios from '../api/axios';

const adminService = {
    getDashboardStats: async () => {
        const response = await axios.get('/admin/dashboard/stats');
        return response.data;
    },
    getRecentUsers: async (count = 5) => {
        const response = await axios.get(`/admin/dashboard/recent-users?count=${count}`);
        return response.data;
    },
    getRecentPlagiarismReports: async (count = 5) => {
        const response = await axios.get(`/admin/dashboard/recent-plagiarism-reports?count=${count}`);
        return response.data;
    },
    
    // User management
    getAllUsers: async () => {
        const response = await axios.get('/admin/users');
        return response.data;
    },
    updateUserStatus: async (userId, status) => {
        const response = await axios.put(`/admin/users/status/${userId}`, { isActive: status });
        return response.data;
    },
    
    // Artwork management
    getAllArtworks: async () => {
        const response = await axios.get('/admin/artworks');
        return response.data;
    },
    approveArtwork: async (artworkId) => {
        const response = await axios.put(`/admin/artworks/approve/${artworkId}`);
        return response.data;
    },
    rejectArtwork: async (artworkId, reason) => {
        const response = await axios.put(`/admin/artworks/reject/${artworkId}`, { reason });
        return response.data;
    },

    // Plagiarism Monitoring
    getPlagiarismLogs: async () => {
        const response = await axios.get('/admin/plagiarism');
        return response.data;
    },
    markPlagiarismReviewed: async (logId, notes) => {
        const response = await axios.put(`/admin/plagiarism/review/${logId}`, { notes });
        return response.data;
    },

    // Transactions & Orders
    getTransactions: async (params) => {
        const response = await axios.get('/admin/transactions', { params });
        return response.data;
    },
    getOrders: async () => {
        const response = await axios.get('/admin/orders');
        return response.data;
    },

    // Detailed Reports
    getDetailedReports: async (type) => {
        const response = await axios.get(`/admin/reports/${type}`);
        return response.data;
    }
};

export default adminService;
