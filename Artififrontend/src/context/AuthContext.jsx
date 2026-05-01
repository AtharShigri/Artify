import React, { createContext, useState, useEffect, useContext } from 'react';
import { authService } from '../services/authService';

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const savedUser = authService.getCurrentUser();
        if (savedUser) {
            setUser(savedUser);
        }
        setLoading(false);
    }, []);

    // Unified login — no role param needed; backend determines role
    const login = async (email, password) => {
        const data = await authService.login(email, password);
        setUser(data);
        return data;
    };

    const updateUser = (updateData) => {
        if (!user) return;
        const updatedUser = { ...user, ...updateData };
        setUser(updatedUser);
        localStorage.setItem('user', JSON.stringify(updatedUser));
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
        // user.role  : 'Artist' | 'Buyer' | 'Admin'
        // user.userType : 0 (Individual) | 1 (Agency)
        isAuthenticated: !!user,
        isArtist: user?.role === 'Artist' || user?.role === 'Agency',
        isBuyer: user?.role === 'Buyer' || user?.role === 'Agency',
        isAdmin: user?.role === 'Admin',
        isAgency: user?.role === 'Agency' || user?.userType === 1,
        loading,
        login,
        register,
        logout,
        updateUser
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
