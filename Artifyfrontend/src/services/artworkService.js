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
        // formData should be an instance of FormData for file upload
        const response = await axios.post('/artist/artworks/upload', formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        });
        return response.data;
    },
    update: async (id, data) => {
        const response = await axios.put(`/artist/artworks/${id}`, data);
        return response.data;
    },
    delete: async (id) => {
        const response = await axios.delete(`/artist/artworks/${id}`);
        return response.data;
    }
};

export default artworkService;
