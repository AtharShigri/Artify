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
            let message = 'Login failed';
            if (error.response && error.response.data) {
                if (typeof error.response.data === 'string') {
                    message = error.response.data;
                } else if (error.response.data.message) {
                    message = error.response.data.message;
                } else if (error.response.data.errors) {
                    // Handle ASP.NET Core validation errors
                    message = Object.values(error.response.data.errors).flat().join(', ');
                }
            } else if (error.message) {
                message = error.message;
            }
            throw new Error(message);
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
            let message = 'Registration failed';
            if (error.response && error.response.data) {
                if (typeof error.response.data === 'string') {
                    message = error.response.data;
                } else if (error.response.data.message) {
                    message = error.response.data.message;
                } else if (error.response.data.errors) {
                    // Handle ASP.NET Core validation errors
                    message = Object.values(error.response.data.errors).flat().join(', ');
                }
            } else if (error.message) {
                message = error.message;
            }
            throw new Error(message);
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
