import React, { Suspense, lazy } from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import Loader from '../components/common/Loader';
import ProtectedRoute from './ProtectedRoute';
import DashboardLayout from '../components/layout/DashboardLayout';

// Lazy load pages
const Home = lazy(() => import('../pages/Home'));
const Marketplace = lazy(() => import('../pages/marketplace/Marketplace'));
const Artists = lazy(() => import('../pages/Artists'));
const ArtistProfile = lazy(() => import('../pages/marketplace/ArtistProfile'));
const ArtworkDetails = lazy(() => import('../pages/marketplace/ArtworkDetails'));
const Cart = lazy(() => import('../pages/cart/Cart'));
const Checkout = lazy(() => import('../pages/cart/Checkout'));
const ProjectBoard = lazy(() => import('../pages/ProjectBoard'));

const Login = lazy(() => import('../pages/auth/Login'));
const AdminLogin = lazy(() => import('../pages/auth/AdminLogin'));
const Register = lazy(() => import('../pages/auth/Register'));
const ForgotPassword = lazy(() => import('../pages/auth/ForgotPassword'));

// Dashboard Pages — Artist
const ArtistDashboard = lazy(() => import('../pages/artist/ArtistDashboard'));
const UploadArtwork = lazy(() => import('../pages/artist/UploadArtwork'));
const MyArtworks = lazy(() => import('../pages/artist/MyArtworks'));
const ArtistSettings = lazy(() => import('../pages/artist/Settings'));
const ArtworkProtection = lazy(() => import('../pages/artist/ArtworkProtection'));

// Dashboard Pages — Buyer
const BuyerDashboard = lazy(() => import('../pages/buyer/BuyerDashboard'));
const PostProject = lazy(() => import('../pages/buyer/PostProject'));
const BuyerSettings = lazy(() => import('../pages/buyer/Settings'));

// Dashboard Pages — Admin
const AdminDashboard = lazy(() => import('../pages/admin/AdminDashboard'));
const AdminUsers = lazy(() => import('../pages/admin/AdminUsers'));
const AdminArtworks = lazy(() => import('../pages/admin/AdminArtworks'));
const AdminReports = lazy(() => import('../pages/admin/AdminReports'));
const AdminTransactions = lazy(() => import('../pages/admin/AdminTransactions'));

// Chat Page
const ChatPage = lazy(() => import('../pages/chat/ChatPage'));

const AppRoutes = () => {
    return (
        <Suspense fallback={<Loader fullScreen />}>
            <Routes>
                {/* ── Public Routes ── */}
                <Route path="/" element={<Home />} />
                <Route path="/marketplace" element={<Marketplace />} />
                <Route path="/artist/:id" element={<ArtistProfile />} />
                <Route path="/artwork/:id" element={<ArtworkDetails />} />
                <Route path="/cart" element={<Cart />} />
                <Route path="/artists" element={<Artists />} />
                <Route path="/about" element={<div className="p-20 text-center text-xl">About Artify — Coming Soon</div>} />

                {/* Project Board — public browse, auth required to submit */}
                <Route path="/project-board" element={<ProjectBoard />} />

                {/* ── Auth Routes ── */}
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/forgot-password" element={<ForgotPassword />} />
                <Route path="/admin" element={<AdminLogin />} />

                {/* ── Protected: Checkout ── */}
                <Route path="/checkout" element={
                    <ProtectedRoute><Checkout /></ProtectedRoute>
                } />

                {/* ── Protected: Dashboard ── */}
                <Route path="/dashboard" element={
                    <ProtectedRoute><DashboardLayout /></ProtectedRoute>
                }>
                    {/* Default redirect — determined by role at login, but fallback here */}
                    <Route index element={<Navigate to="buyer/orders" replace />} />

                    {/* Shared Routes */}
                    <Route path="chat" element={<ChatPage />} />


                    {/* Artist Routes */}
                    <Route path="artist" element={<ArtistDashboard />} />
                    <Route path="artist/upload" element={<UploadArtwork />} />
                    <Route path="artist/artworks" element={<MyArtworks />} />
                    <Route path="artist/artworks/:id/edit" element={<UploadArtwork />} />
                    <Route path="artist/settings" element={<ArtistSettings />} />
                    <Route path="artist/protection" element={<ArtworkProtection />} />
                    <Route path="artist/*" element={<div className="p-8">Page under construction</div>} />

                    {/* Buyer Routes */}
                    <Route path="buyer" element={<BuyerDashboard />} />
                    <Route path="buyer/orders" element={<BuyerDashboard />} />
                    <Route path="buyer/projects" element={<BuyerDashboard />} />
                    <Route path="buyer/post-project" element={<PostProject />} />
                    <Route path="buyer/settings" element={<BuyerSettings />} />
                    <Route path="buyer/*" element={<div className="p-8">Page under construction</div>} />

                    {/* Agency Routes (any userType===1, both Artist & Buyer) */}

                    {/* Admin Routes */}
                    <Route path="admin" element={<AdminDashboard />} />
                    <Route path="admin/users" element={<AdminUsers />} />
                    <Route path="admin/artworks" element={<AdminArtworks />} />
                    <Route path="admin/reports" element={<AdminReports />} />
                    <Route path="admin/transactions" element={<AdminTransactions />} />
                    <Route path="admin/*" element={<div className="p-8">Page under construction</div>} />
                </Route>

                {/* ── 404 ── */}
                <Route path="*" element={<div className="p-10 text-center">404 — Not Found</div>} />
            </Routes>
        </Suspense>
    );
};

export default AppRoutes;
