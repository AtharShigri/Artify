import React, { useState, useRef, useEffect, useCallback } from 'react';
import { Link, useLocation } from 'react-router-dom';
import { Menu, X, ShoppingBag, User, LayoutDashboard, Briefcase, PlusCircle, Bell, Check } from 'lucide-react';
import * as signalR from '@microsoft/signalr';
import Button from '../common/Button';
import { useAuth } from '../../context/AuthContext';
import { useCart } from '../../context/CartContext';
import { cn } from '../../utils/cn';
import { getImageUrl } from '../../utils/imageUtils';
import api from '../../api/axios';

// ─── Notification Bell ────────────────────────────────────────────────────────
const NotificationBell = ({ user }) => {
    const [open, setOpen] = useState(false);
    const [notifications, setNotifications] = useState([]);
    const ref = useRef(null);
    const connectionRef = useRef(null);

    // Fetch initial notifications from API
    const fetchNotifications = useCallback(async () => {
        try {
            const res = await api.get('/notifications?count=20');
            setNotifications(res.data || []);
        } catch (err) {
            console.error('Failed to fetch notifications:', err);
        }
    }, []);

    // Connect to SignalR hubs (notification + chat) for real-time delivery
    useEffect(() => {
        if (!user) return;

        fetchNotifications();

        const storedUser = localStorage.getItem('user');
        const token = storedUser ? JSON.parse(storedUser)?.token : null;
        if (!token) return;

        const baseURL = import.meta.env.VITE_API_URL || 'http://localhost:5181/api';

        // ── 1. Notification Hub ───────────────────────────────────────────────
        const notifHubURL = baseURL.replace('/api', '/notificationhub');
        const notifConnection = new signalR.HubConnectionBuilder()
            .withUrl(notifHubURL, { accessTokenFactory: () => token })
            .withAutomaticReconnect()
            .build();

        notifConnection.on('ReceiveNotification', (notification) => {
            setNotifications(prev => [notification, ...prev]);
        });

        notifConnection.start().catch(err => console.error('NotificationHub failed:', err));

        // ── 2. Chat Hub (global — keeps user in all conversation groups) ──────
        // OnConnectedAsync on the server auto-joins all conversation groups.
        // This means messages are delivered in real-time even when the chat
        // page is not open.
        const chatHubURL = baseURL.replace('/api', '/chathub');
        const chatConnection = new signalR.HubConnectionBuilder()
            .withUrl(chatHubURL, { accessTokenFactory: () => token })
            .withAutomaticReconnect()
            .build();

        chatConnection.start().catch(err => console.error('ChatHub (global) failed:', err));

        connectionRef.current = { notif: notifConnection, chat: chatConnection };

        return () => {
            if (connectionRef.current) {
                connectionRef.current.notif?.stop().catch(() => { });
                connectionRef.current.chat?.stop().catch(() => { });
                connectionRef.current = null;
            }
        };
    }, [user, fetchNotifications]);

    // Close dropdown on outside click
    useEffect(() => {
        const handler = (e) => {
            if (ref.current && !ref.current.contains(e.target)) setOpen(false);
        };
        document.addEventListener('mousedown', handler);
        return () => document.removeEventListener('mousedown', handler);
    }, []);

    const unreadCount = notifications.filter(n => !n.isRead).length;

    const markAllRead = async () => {
        try {
            await api.patch('/notifications/read-all');
            setNotifications(prev => prev.map(n => ({ ...n, isRead: true })));
        } catch (err) {
            console.error('Failed to mark all read:', err);
        }
    };

    const markRead = async (id) => {
        try {
            await api.patch(`/notifications/${id}/read`);
            setNotifications(prev => prev.map(n => n.id === id ? { ...n, isRead: true } : n));
        } catch (err) {
            console.error('Failed to mark read:', err);
        }
    };

    const formatTime = (dateStr) => {
        const date = new Date(dateStr);
        const now = new Date();
        const diff = Math.floor((now - date) / 1000);
        if (diff < 60) return 'Just now';
        if (diff < 3600) return `${Math.floor(diff / 60)}m ago`;
        if (diff < 86400) return `${Math.floor(diff / 3600)}h ago`;
        return date.toLocaleDateString();
    };

    return (
        <div className="relative" ref={ref}>
            <button
                onClick={() => { setOpen(!open); if (!open) fetchNotifications(); }}
                className="relative p-2 text-textSecondary hover:text-primary transition-colors rounded-lg hover:bg-gray-50"
                aria-label="Notifications"
            >
                <Bell className="w-5 h-5" />
                {unreadCount > 0 && (
                    <span className="absolute top-0.5 right-0.5 min-w-[18px] h-[18px] bg-red-500 text-white text-[10px] font-bold rounded-full flex items-center justify-center px-1">
                        {unreadCount > 9 ? '9+' : unreadCount}
                    </span>
                )}
            </button>

            {open && (
                <div className="absolute right-0 mt-2 w-80 bg-white rounded-2xl shadow-xl border border-border z-50 overflow-hidden animate-in slide-in-from-top-2 duration-200">
                    {/* Header */}
                    <div className="px-4 py-3 border-b border-border flex items-center justify-between">
                        <div className="flex items-center gap-2">
                            <h3 className="font-bold text-sm text-primary">Notifications</h3>
                            {unreadCount > 0 && (
                                <span className="text-xs bg-red-100 text-red-600 font-bold px-2 py-0.5 rounded-full">
                                    {unreadCount}
                                </span>
                            )}
                        </div>
                        {unreadCount > 0 && (
                            <button
                                onClick={markAllRead}
                                className="text-xs text-secondary hover:underline flex items-center gap-1"
                            >
                                <Check className="w-3 h-3" /> Mark all read
                            </button>
                        )}
                    </div>

                    {/* List */}
                    <div className="max-h-80 overflow-y-auto divide-y divide-gray-50">
                        {notifications.length === 0 ? (
                            <div className="p-8 text-center">
                                <Bell className="w-8 h-8 mx-auto mb-2 text-textSecondary opacity-30" />
                                <p className="text-sm text-textSecondary font-medium">All caught up!</p>
                                <p className="text-xs text-textSecondary mt-1">No notifications yet.</p>
                            </div>
                        ) : (
                            notifications.map((n) => (
                                <div
                                    key={n.id}
                                    onClick={() => !n.isRead && markRead(n.id)}
                                    className={`px-4 py-3 hover:bg-gray-50 cursor-pointer transition-colors group ${!n.isRead ? 'bg-primary/5' : ''
                                        }`}
                                >
                                    <div className="flex items-start gap-2">
                                        {!n.isRead && (
                                            <span className="w-2 h-2 rounded-full bg-primary mt-1.5 shrink-0" />
                                        )}
                                        <div className="min-w-0 flex-1" style={{ paddingLeft: n.isRead ? '10px' : '0' }}>
                                            <p className="text-sm text-primary font-semibold leading-tight">{n.title}</p>
                                            <p className="text-xs text-textSecondary mt-0.5 line-clamp-2">{n.message}</p>
                                            <p className="text-[10px] text-gray-400 mt-1">{formatTime(n.createdAt)}</p>
                                        </div>
                                    </div>
                                </div>
                            ))
                        )}
                    </div>
                </div>
            )}
        </div>
    );
};

// ─── Main Navbar ─────────────────────────────────────────────────────────────
const Navbar = () => {
    const [isOpen, setIsOpen] = useState(false);
    const { user, logout, isArtist, isBuyer, isAdmin } = useAuth();
    const { cartItems } = useCart();
    const location = useLocation();

    const isActive = (path) => location.pathname === path;

    const publicLinks = [
        { name: 'Marketplace', path: '/marketplace' },
        { name: 'Artists', path: '/artists' },
        { name: 'About', path: '/about' },
    ];

    const artistLinks = [
        { name: 'Job Feed', path: '/project-board', icon: Briefcase },
        { name: 'Marketplace', path: '/marketplace' },
    ];

    const buyerLinks = [
        { name: 'Marketplace', path: '/marketplace' },
        { name: 'Artists', path: '/artists' },
        { name: 'Post a Project', path: '/dashboard/buyer/post-project', icon: PlusCircle },
    ];

    const adminLinks = [];

    const navLinks = isAdmin ? adminLinks : isArtist ? artistLinks : isBuyer ? buyerLinks : publicLinks;

    const dashboardPath = isArtist
        ? '/dashboard/artist'
        : isAdmin
            ? '/dashboard/admin'
            : '/dashboard/buyer/orders';

    return (
        <nav className="sticky top-0 z-40 bg-white/80 backdrop-blur-md border-b border-border">
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                <div className="flex justify-between items-center h-16">
                    {/* Logo */}
                    <Link to="/" className="flex-shrink-0 flex items-center">
                        <img src="/images/logo/artifilogo.png" alt="artifi logo" className="h-14 w-auto" />
                    </Link>

                    {/* Desktop Navigation */}
                    <div className="hidden md:flex items-center space-x-8">
                        {navLinks.map((link) => (
                            <Link
                                key={link.path}
                                to={link.path}
                                className={cn(
                                    'flex items-center gap-1.5 text-sm font-medium transition-colors hover:text-secondary',
                                    isActive(link.path) ? 'text-primary' : 'text-textSecondary',
                                    link.icon && 'bg-secondary/5 px-3 py-1.5 rounded-full border border-secondary/20 hover:bg-secondary/10'
                                )}
                            >
                                {link.icon && <link.icon className="w-3.5 h-3.5" />}
                                {link.name}
                            </Link>
                        ))}
                    </div>

                    {/* Desktop Actions */}
                    <div className="hidden md:flex items-center gap-3">
                        {(!user || isBuyer) && (
                            <>
                                <Link to="/cart" className="p-2 text-textSecondary hover:text-primary transition-colors relative rounded-lg hover:bg-gray-50">
                                    <ShoppingBag className="w-5 h-5" />
                                    {cartItems?.length > 0 && (
                                        <span className="absolute -top-1 -right-1 w-4 h-4 bg-accent text-white text-[10px] font-bold rounded-full flex items-center justify-center">
                                            {cartItems.length}
                                        </span>
                                    )}
                                </Link>
                                <div className="h-6 w-px bg-border" />
                            </>
                        )}

                        {user ? (
                            <div className="flex items-center gap-2">
                                <NotificationBell user={user} />
                                <div className="h-6 w-px bg-border" />
                                <Link to={dashboardPath}>
                                    <div className="flex items-center gap-2 text-sm font-medium text-primary hover:text-secondary transition-colors">
                                        <div className="w-8 h-8 bg-secondary/10 rounded-full flex items-center justify-center overflow-hidden">
                                            {user.profileImageUrl ? (
                                                <img src={getImageUrl(user.profileImageUrl)} alt={user.fullName || 'Dashboard'} className="w-full h-full object-cover" />
                                            ) : (
                                                <User className="w-4 h-4 text-secondary" />
                                            )}
                                        </div>
                                        <span>{user.fullName || 'Dashboard'}</span>
                                        <LayoutDashboard className="w-4 h-4 text-textSecondary" />
                                    </div>
                                </Link>
                                <Button variant="ghost" size="sm" onClick={logout}>Sign Out</Button>
                            </div>
                        ) : (
                            <>
                                <Link to="/login">
                                    <Button variant="ghost" size="sm">Login</Button>
                                </Link>
                                <Link to="/register">
                                    <Button variant="primary" size="sm">Join</Button>
                                </Link>
                            </>
                        )}
                    </div>

                    {/* Mobile menu button */}
                    <div className="md:hidden flex items-center gap-2">
                        {user && <NotificationBell user={user} />}
                        <button
                            onClick={() => setIsOpen(!isOpen)}
                            className="p-2 rounded-md text-textSecondary hover:text-primary focus:outline-none"
                        >
                            {isOpen ? <X className="w-6 h-6" /> : <Menu className="w-6 h-6" />}
                        </button>
                    </div>
                </div>
            </div>

            {/* Mobile Navigation */}
            {isOpen && (
                <div className="md:hidden absolute top-16 left-0 right-0 bg-white border-b border-border animate-in slide-in-from-top-2 shadow-lg">
                    <div className="px-4 pt-2 pb-4 space-y-1">
                        {navLinks.map((link) => (
                            <Link
                                key={link.path}
                                to={link.path}
                                className="flex items-center gap-2 px-3 py-2 rounded-md text-base font-medium text-textSecondary hover:text-primary hover:bg-gray-50"
                                onClick={() => setIsOpen(false)}
                            >
                                {link.icon && <link.icon className="w-4 h-4" />}
                                {link.name}
                            </Link>
                        ))}
                        <div className="pt-4 flex flex-col gap-2">
                            {user ? (
                                <>
                                    <Link to={dashboardPath} onClick={() => setIsOpen(false)}>
                                        <Button variant="primary" className="w-full">My Dashboard</Button>
                                    </Link>
                                    <Button variant="secondary" className="w-full" onClick={() => { logout(); setIsOpen(false); }}>Sign Out</Button>
                                </>
                            ) : (
                                <>
                                    <Link to="/login" onClick={() => setIsOpen(false)}>
                                        <Button variant="secondary" className="w-full">Login</Button>
                                    </Link>
                                    <Link to="/register" onClick={() => setIsOpen(false)}>
                                        <Button variant="primary" className="w-full">Join Now</Button>
                                    </Link>
                                </>
                            )}
                        </div>
                    </div>
                </div>
            )}
        </nav>
    );
};

export default Navbar;
