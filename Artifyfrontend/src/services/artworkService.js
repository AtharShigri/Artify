import axios from '../api/axios';

const artworkService = {
    getAllByArtist: async () => {
        const response = await axios.get('/artworks/artist/my-artworks');
        return response.data;
    },
    getById: async (id) => {
        const response = await axios.get(`/artworks/${id}`);
        return response.data;
    },
    create: async (formData) => {
        // formData should be an instance of FormData for file upload
        const response = await axios.post('/artworks', formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        });
        return response.data;
    },
    update: async (id, formData) => {
        const response = await axios.put(`/artworks/${id}`, formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        });
        return response.data;
    },
    delete: async (id) => {
        const response = await axios.delete(`/artworks/${id}`);
        return response.data;
    }
};

export default artworkService;
