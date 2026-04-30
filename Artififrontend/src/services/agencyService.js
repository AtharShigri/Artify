import api from '../api/axios';

const handleError = (error, fallback) => {
    if (error.response?.data) {
        const d = error.response.data;
        if (typeof d === 'string') throw new Error(d);
        if (d.message) throw new Error(d.message);
    }
    throw new Error(error.message || fallback);
};

export const agencyService = {
    // Create or update agency profile
    setupAgency: async (dto) => {
        try {
            const res = await api.post('/agency/setup', dto);
            return res.data;
        } catch (err) {
            handleError(err, 'Failed to save agency profile');
        }
    },

    // Add a member by email
    addMember: async (email) => {
        try {
            const res = await api.post('/agency/members', JSON.stringify(email), {
                headers: { 'Content-Type': 'application/json' }
            });
            return res.data;
        } catch (err) {
            handleError(err, 'Failed to add member');
        }
    },

    // Get agency details and team
    getMyTeam: async () => {
        try {
            const res = await api.get('/agency/my-team');
            return res.data;
        } catch (err) {
            handleError(err, 'Failed to load team');
        }
    }
};
