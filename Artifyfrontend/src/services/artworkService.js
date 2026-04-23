import axios from '../api/axios';

const artworkService = {
    getAllByArtist: async () => {
        const response = await axios.get('/artist/artworks');
        return response.data;
    },
    getById: async (id) => {
        const response = await axios.get(`/artist/artworks/${id}`);
        return response.data;
    },
    create: async (formData) => {
        // Let axios set Content-Type automatically (includes multipart boundary)
        const response = await axios.post('/artist/artworks/upload', formData);
        return response.data;
    },
    update: async (id, formData) => {
        // Let axios set Content-Type automatically (includes multipart boundary)
        const response = await axios.put(`/artist/artworks/${id}`, formData);
        return response.data;
    },
    delete: async (id) => {
        const response = await axios.delete(`/artist/artworks/${id}`);
        return response.data;
    }
};

export default artworkService;
