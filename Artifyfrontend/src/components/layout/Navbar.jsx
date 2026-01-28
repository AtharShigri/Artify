import React, { useState } from 'react';
import { Link, useLocation } from 'react-router-dom';
import { Menu, X, ShoppingBag, User } from 'lucide-react';
import Button from '../common/Button';
import { useAuth } from '../../context/AuthContext';
import { cn } from '../../utils/cn';

const Navbar = () => {
    const [isOpen, setIsOpen] = useState(false);
    const { user, logout } = useAuth();
    const location = useLocation();

    const navLinks = [
        { name: 'Marketplace', path: '/marketplace' },
        { name: 'Artists', path: '/artists' },
        { name: 'About', path: '/about' },
    ];

    const isActive = (path) => location.pathname === path;

    return (
        <nav className="sticky top-0 z-40 bg-white/80 backdrop-blur-md border-b border-border">
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                <div className="flex justify-between items-center h-16">
                    {/* Logo */}
                    <Link to="/" className="flex-shrink-0 flex items-center gap-2">
                        <div className="w-8 h-8 bg-primary rounded-lg flex items-center justify-center">
                            <span className="text-accent font-heading font-bold text-lg">A</span>
                        </div>
                        <span className="font-heading font-bold text-xl text-primary">Artify</span>
                    </Link>

                    {/* Desktop Navigation */}
                    <div className="hidden md:flex items-center space-x-8">
                        {navLinks.map((link) => (
                            <Link
                                key={link.path}
                                to={link.path}
                                className={cn(
                                    "text-sm font-medium transition-colors hover:text-secondary",
                                    isActive(link.path) ? "text-primary" : "text-textSecondary"
                                )}
                            >
                                {link.name}
                            </Link>
                        ))}
                    </div>

                    {/* Desktop Actions */}
                    <div className="hidden md:flex items-center gap-4">
                        <Link to="/cart" className="p-2 text-textSecondary hover:text-primary transition-colors relative">
                            <ShoppingBag className="w-5 h-5" />
                            {/* Cart Badge Placeholder */}
                            <span className="absolute top-1 right-1 w-2 h-2 bg-accent rounded-full"></span>
                        </Link>
                        <div className="h-6 w-px bg-border"></div>

                        {user ? (
                            <div className="flex items-center gap-3">
                                <Link to={user.role === 'artist' ? '/dashboard/artist' : user.role === 'admin' ? '/dashboard/admin' : '/dashboard/buyer/orders'}>
                                    <div className="flex items-center gap-2 text-sm font-medium text-primary hover:text-secondary transition-colors">
                                        <div className="w-8 h-8 bg-secondary/10 rounded-full flex items-center justify-center">
                                            <User className="w-4 h-4 text-secondary" />
                                        </div>
                                        <span>{user.name || 'Dashboard'}</span>
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
                    <div className="md:hidden flex items-center">
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
                <div className="md:hidden absolute top-16 left-0 right-0 bg-white border-b border-border animate-in slide-in-from-top-2">
                    <div className="px-4 pt-2 pb-4 space-y-1">
                        {navLinks.map((link) => (
                            <Link
                                key={link.path}
                                to={link.path}
                                className="block px-3 py-2 rounded-md text-base font-medium text-textSecondary hover:text-primary hover:bg-gray-50"
                                onClick={() => setIsOpen(false)}
                            >
                                {link.name}
                            </Link>
                        ))}
                        <div className="pt-4 flex flex-col gap-2">
                            <div className="pt-4 flex flex-col gap-2">
                                {user ? (
                                    <>
                                        <Link to={user.role === 'artist' ? '/dashboard/artist' : user.role === 'admin' ? '/dashboard/admin' : '/dashboard/buyer/orders'} onClick={() => setIsOpen(false)}>
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
                </div>
            )}
        </nav>
    );
};

export default Navbar;
