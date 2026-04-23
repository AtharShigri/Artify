import axios from '../api/axios';

const marketplaceService = {
    getAllArtworks: async (page = 1, pageSize = 20) => {
        const response = await axios.get(`/marketplace/artworks?page=${page}&pageSize=${pageSize}`);
        return response.data;
    },

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
    },

    getArtworkById: async (id) => {
        const response = await axios.get(`/marketplace/artworks/${id}`);
        return response.data;
    },

    getTrendingArtworks: async () => {
        const response = await axios.get('/marketplace/artworks/trending');
        return response.data;
    }
};

export default marketplaceService;
