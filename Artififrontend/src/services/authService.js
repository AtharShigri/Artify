import api from '../api/axios';

// Helper to handle API errors consistently
const handleError = (error, fallback) => {
    if (error.response?.data) {
        const d = error.response.data;
        if (typeof d === 'string') throw new Error(d);
        if (d.message) throw new Error(d.message);
        if (d.errors) throw new Error(Object.values(d.errors).flat().join(', '));
    }
    throw new Error(error.message || fallback);
};

export const authService = {
    // Unified Login — backend determines role from Identity
    login: async (email, password) => {
        try {
            const response = await api.post('/auth/login', { email, password });
            if (response.data) {
                // response.data = { token, expiration, role, fullName, email, profileImageUrl, userType }
                const userData = { ...response.data };
                localStorage.setItem('user', JSON.stringify(userData));
                return userData;
            }
        } catch (error) {
            handleError(error, 'Login failed');
        }
    },

    // Register — role determines endpoint; userType (0=Individual, 1=Agency) is in body
    register: async (userData) => {
        const { role, ...data } = userData;
        const endpoint = role === 'artist' ? '/auth/register/artist' : '/auth/register/buyer';
        try {
            const response = await api.post(endpoint, data);
            return response.data;
        } catch (error) {
            handleError(error, 'Registration failed');
        }
    },

    // Logout
    logout: () => {
        localStorage.removeItem('user');
    },

    // Get current user from localStorage
    getCurrentUser: () => {
        try {
            return JSON.parse(localStorage.getItem('user'));
        } catch {
            return null;
        }
    }
};
