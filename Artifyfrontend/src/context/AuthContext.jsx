import React, { createContext, useState, useEffect, useContext } from 'react';
import { authService } from '../services/authService';

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        // Check for saved user on load
        const savedUser = authService.getCurrentUser();
        if (savedUser) {
            setUser(savedUser);
        }
        setLoading(false);
    }, []);

    const login = async (email, password, role) => {
        const data = await authService.login(email, password, role);
        setUser(data);
        return data;
    };

    const register = async (userData) => {
        return await authService.register(userData);
    };

    const logout = () => {
        authService.logout();
        setUser(null);
    };

    const value = {
        user,
        isAuthenticated: !!user,
        loading,
        login,
        register,
        logout
    };

    return (
        <AuthContext.Provider value={value}>
            {!loading && children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};

export default AuthContext;
