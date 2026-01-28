import axios from '../api/axios';

const marketplaceService = {
    getAllArtists: async (page = 1, pageSize = 20) => {
        const response = await axios.get(`/marketplace/artists?page=${page}&pageSize=${pageSize}`);
        return response.data;
    },

    getFeaturedArtists: async () => {
        const response = await axios.get('/marketplace/artists/featured');
        return response.data;
    },

    getArtistProfile: async (id) => {
        const response = await axios.get(`/marketplace/artists/${id}`);
        return response.data;
    }
};

export default marketplaceService;
