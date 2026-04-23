import React, { useState } from 'react';
import { NavLink, Link, Outlet, useNavigate } from 'react-router-dom';
import {
    LayoutDashboard,
    Upload,
    Image as ImageIcon,
    ShoppingBag,
    Settings,
    LogOut,
    Menu,
    X,
    User,
    Users,
    AlertTriangle,
    Briefcase,
    PlusCircle,
    Building2,
    MessageSquare,
    Shield,
    DollarSign
} from 'lucide-react';
import { useAuth } from '../../context/AuthContext';
import Button from '../common/Button';
import { getImageUrl } from '../../utils/imageUtils';

const DashboardLayout = () => {
    const { user, logout, isArtist, isAdmin, isAgency } = useAuth();
    const navigate = useNavigate();
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    const artistLinks = [
        { name: 'Overview', path: '/dashboard/artist', icon: LayoutDashboard },
        { name: 'Upload Artwork', path: '/dashboard/artist/upload', icon: Upload },
        { name: 'My Artworks', path: '/dashboard/artist/artworks', icon: ImageIcon },
        { name: 'Orders (Selling)', path: '/dashboard/artist/orders', icon: ShoppingBag },
        { name: 'Job Feed', path: '/project-board', icon: Briefcase },
        { name: 'Messages', path: '/dashboard/chat', icon: MessageSquare },
        { name: 'Protection', path: '/dashboard/artist/protection', icon: Shield },
        { name: 'Settings', path: '/dashboard/artist/settings', icon: Settings },
    ];

    const buyerLinks = [
        { name: 'Buyer Dashboard', path: '/dashboard/buyer', icon: LayoutDashboard },
        { name: 'Post a Project', path: '/dashboard/buyer/post-project', icon: PlusCircle },
        { name: 'Messages', path: '/dashboard/chat', icon: MessageSquare },
        { name: 'Settings', path: '/dashboard/buyer/settings', icon: Settings },
    ];

    const agencyLinks = [
        { name: 'Dashboard', path: '/dashboard/artist', icon: LayoutDashboard },
        { name: 'Upload Artwork', path: '/dashboard/artist/upload', icon: Upload },
        { name: 'Post a Project', path: '/dashboard/buyer/post-project', icon: PlusCircle },
        { name: 'My Artworks', path: '/dashboard/artist/artworks', icon: ImageIcon },
        { name: 'Job Feed', path: '/project-board', icon: Briefcase },
        { name: 'Buy Orders', path: '/dashboard/buyer/orders', icon: ShoppingBag },
        { name: 'Sell Orders', path: '/dashboard/artist/orders', icon: ShoppingBag },
        { name: 'Messages', path: '/dashboard/chat', icon: MessageSquare },
        { name: 'Settings', path: '/dashboard/artist/settings', icon: Settings },
    ];

    const adminLinks = [
        { name: 'Overview', path: '/dashboard/admin', icon: LayoutDashboard },
        { name: 'Users', path: '/dashboard/admin/users', icon: Users },
        { name: 'Artworks', path: '/dashboard/admin/artworks', icon: ImageIcon },
        { name: 'Financials', path: '/dashboard/admin/transactions', icon: DollarSign },
        { name: 'Monitoring', path: '/dashboard/admin/reports', icon: Shield },
    ];

    let links = buyerLinks;
    if (isAgency) links = agencyLinks;
    else if (isArtist) links = artistLinks;
    else if (isAdmin) links = adminLinks;

    const roleLabel = isAdmin
        ? 'Administrator'
        : (isAgency ? 'Agency' : (isArtist ? 'Artist' : 'Buyer'));

    return (
        <div className="min-h-screen bg-background flex">
            {/* Sidebar — Desktop */}
            <aside className={`fixed inset-y-0 left-0 z-50 w-64 bg-white border-r border-border transform transition-transform duration-300 ease-in-out md:relative md:translate-x-0 ${isSidebarOpen ? 'translate-x-0' : '-translate-x-full'}`}>
                <div className="h-full flex flex-col">
                    {/* Header */}
                    <div className="h-16 flex items-center px-6 border-b border-border justify-between">
                        <Link to="/" className="flex items-center gap-2">
                            <div className="w-8 h-8 bg-primary rounded-lg flex items-center justify-center">
                                <span className="text-accent font-heading font-bold text-lg">A</span>
                            </div>
                            <span className="font-heading font-bold text-xl text-primary">Artify</span>
                        </Link>
                        <button onClick={() => setIsSidebarOpen(false)} className="md:hidden">
                            <X className="w-6 h-6 text-textSecondary" />
                        </button>
                    </div>

                    {/* User Info */}
                    <div className="p-6 border-b border-border">
                        <div className="flex items-center gap-3">
                            <div className="w-10 h-10 rounded-full bg-gray-200 flex items-center justify-center overflow-hidden shrink-0">
                                {user?.profileImageUrl ? (
                                    <img src={getImageUrl(user.profileImageUrl)} alt={user?.fullName || 'User'} className="w-full h-full object-cover" />
                                ) : (
                                    <User className="w-6 h-6 text-gray-400" />
                                )}
                            </div>
                            <div className="overflow-hidden">
                                <p className="font-bold text-primary truncate">{user?.fullName || 'User'}</p>
                                <div className="flex items-center gap-1.5">
                                    <p className="text-xs text-textSecondary capitalize">{roleLabel}</p>
                                    {isAgency && (
                                        <span className="text-xs px-1.5 py-0.5 bg-secondary/10 text-secondary rounded-full font-medium">
                                            Agency
                                        </span>
                                    )}
                                </div>
                            </div>
                        </div>
                    </div>

                    {/* Nav Links */}
                    <nav className="flex-1 p-4 space-y-1 overflow-y-auto">
                        {links.map((link) => (
                            <NavLink
                                key={link.path + (link.name)}
                                to={link.path}
                                end={link.path === '/dashboard/artist' || link.path === '/dashboard/admin'}
                                onClick={() => setIsSidebarOpen(false)}
                                className={({ isActive }) =>
                                    `flex items-center gap-3 px-4 py-3 rounded-xl text-sm font-medium transition-colors ${
                                        isActive
                                            ? 'bg-primary text-white shadow-md'
                                            : 'text-textSecondary hover:bg-gray-50 hover:text-primary'
                                    }`
                                }
                            >
                                <link.icon className="w-5 h-5" />
                                {link.name}
                            </NavLink>
                        ))}
                    </nav>

                    {/* Footer */}
                    <div className="p-4 border-t border-border">
                        <button
                            onClick={handleLogout}
                            className="flex items-center gap-3 px-4 py-3 w-full rounded-xl text-sm font-medium text-error hover:bg-red-50 transition-colors"
                        >
                            <LogOut className="w-5 h-5" />
                            Sign Out
                        </button>
                    </div>
                </div>
            </aside>

            {/* Main Content */}
            <div className="flex-1 flex flex-col min-h-screen overflow-hidden">
                {/* Mobile Header */}
                <header className="md:hidden h-16 bg-white border-b border-border flex items-center justify-between px-4">
                    <Link to="/" className="font-heading font-bold text-xl text-primary">Artify</Link>
                    <button onClick={() => setIsSidebarOpen(true)}>
                        <Menu className="w-6 h-6 text-primary" />
                    </button>
                </header>

                {/* Page Content */}
                <main className="flex-1 overflow-y-auto p-4 md:p-8">
                    <Outlet />
                </main>
            </div>

            {/* Overlay */}
            {isSidebarOpen && (
                <div
                    className="fixed inset-0 z-40 bg-black/20 md:hidden"
                    onClick={() => setIsSidebarOpen(false)}
                />
            )}
        </div>
    );
};

export default DashboardLayout;
