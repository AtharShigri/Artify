import axios from '../api/axios';

const reviewService = {
    createReview: async (reviewData) => {
        // reviewData: { rating, comment, artistProfileId, artworkId }
        const response = await axios.post('/buyer/reviews', reviewData);
        return response.data;
    },

    getArtistReviews: async (artistProfileId) => {
        const response = await axios.get(`/buyer/reviews/artist/${artistProfileId}`);
        return response.data;
    },

    getArtworkReviews: async (artworkId) => {
        const response = await axios.get(`/buyer/reviews/artwork/${artworkId}`);
        return response.data;
    },

    deleteReview: async (reviewId) => {
        const response = await axios.delete(`/buyer/reviews/${reviewId}`);
        return response.data;
    }
};

export default reviewService;
