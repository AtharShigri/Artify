import api from '../api/axios';

export const authService = {
    // Login
    login: async (email, password, role) => {
        const endpoint = role === 'artist' ? '/artist/login' : role === 'admin' ? '/admin/login' : '/buyer/login';

        try {
            const response = await api.post(endpoint, { email, password });
            if (response.data) {
                // Backend might return token differently, adjusting to typical response structure
                // Assuming response.data contains the user object with token
                const userData = { ...response.data, role };
                localStorage.setItem('user', JSON.stringify(userData));
                return userData;
            }
        } catch (error) {
            throw error.response?.data?.message || error.message || 'Login failed';
        }
    },

    // Register
    register: async (userData) => {
        const { role, ...data } = userData;
        const endpoint = role === 'artist' ? '/artist/register' : '/buyer/register';

        try {
            const response = await api.post(endpoint, data);
            return response.data;
        } catch (error) {
            throw error.response?.data?.message || error.message || 'Registration failed';
        }
    },

    // Logout
    logout: () => {
        localStorage.removeItem('user');
    },

    // Get current user
    getCurrentUser: () => {
        return JSON.parse(localStorage.getItem('user'));
    }
};
