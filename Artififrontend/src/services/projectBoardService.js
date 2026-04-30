import api from '../api/axios';

const handleError = (error, fallback) => {
    if (error.response?.data) {
        const d = error.response.data;
        if (typeof d === 'string') throw new Error(d);
        if (d.message) throw new Error(d.message);
    }
    throw new Error(error.message || fallback);
};

export const projectBoardService = {
    // Public — any visitor can browse
    browseJobs: async () => {
        try {
            const res = await api.get('/projectboard/browse');
            return res.data;
        } catch (err) {
            handleError(err, 'Failed to load projects');
        }
    },

    // Buyer — post a new job
    postJob: async (dto) => {
        try {
            const res = await api.post('/projectboard/post-job', dto);
            return res.data;
        } catch (err) {
            handleError(err, 'Failed to post project');
        }
    },

    // Buyer — get own jobs with all proposals
    getMyJobs: async () => {
        try {
            const res = await api.get('/projectboard/my-jobs');
            return res.data;
        } catch (err) {
            handleError(err, 'Failed to load your projects');
        }
    },

    // Artist — submit a proposal
    submitProposal: async (dto) => {
        try {
            const res = await api.post('/projectboard/submit-proposal', dto);
            return res.data;
        } catch (err) {
            handleError(err, 'Failed to submit proposal');
        }
    },

    // Buyer — accept or reject a proposal
    updateProposalStatus: async (proposalId, status) => {
        try {
            const res = await api.patch(`/projectboard/proposals/${proposalId}/status`, JSON.stringify(status), {
                headers: { 'Content-Type': 'application/json' }
            });
            return res.data;
        } catch (err) {
            handleError(err, 'Failed to update proposal status');
        }
    }
};
