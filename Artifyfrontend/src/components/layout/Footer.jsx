import React from 'react';
import { Link } from 'react-router-dom';
import { Instagram, Twitter, Facebook, Mail } from 'lucide-react';

const Footer = () => {
    return (
        <footer className="bg-primary text-white pt-16 pb-8">
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                <div className="grid grid-cols-1 md:grid-cols-4 gap-12 mb-12">
                    {/* Brand */}
                    <div className="col-span-1 md:col-span-1">
                        <div className="flex items-center gap-2 mb-4">
                            <div className="w-8 h-8 bg-accent rounded-lg flex items-center justify-center">
                                <span className="text-primary font-heading font-bold text-lg">A</span>
                            </div>
                            <span className="font-heading font-bold text-xl text-white">Artify</span>
                        </div>
                        <p className="text-gray-400 text-sm leading-relaxed mb-6">
                            Connecting artists with art lovers worldwide. A premium marketplace for authentic creative work.
                        </p>
                        <div className="flex gap-4">
                            <a href="#" className="text-gray-400 hover:text-accent transition-colors"><Instagram className="w-5 h-5" /></a>
                            <a href="#" className="text-gray-400 hover:text-accent transition-colors"><Twitter className="w-5 h-5" /></a>
                            <a href="#" className="text-gray-400 hover:text-accent transition-colors"><Facebook className="w-5 h-5" /></a>
                        </div>
                    </div>

                    {/* Links */}
                    <div>
                        <h3 className="font-heading text-lg font-semibold mb-6">Marketplace</h3>
                        <ul className="space-y-3">
                            <li><Link to="/marketplace" className="text-gray-400 hover:text-white transition-colors text-sm">Browse Art</Link></li>
                            <li><Link to="/artists" className="text-gray-400 hover:text-white transition-colors text-sm">Find Artists</Link></li>
                            <li><Link to="/services" className="text-gray-400 hover:text-white transition-colors text-sm">Custom Services</Link></li>
                        </ul>
                    </div>

                    <div>
                        <h3 className="font-heading text-lg font-semibold mb-6">Community</h3>
                        <ul className="space-y-3">
                            <li><Link to="/about" className="text-gray-400 hover:text-white transition-colors text-sm">About Us</Link></li>
                            <li><Link to="/blog" className="text-gray-400 hover:text-white transition-colors text-sm">Blog</Link></li>
                            <li><Link to="/help" className="text-gray-400 hover:text-white transition-colors text-sm">Help Center</Link></li>
                        </ul>
                    </div>

                    {/* Newsletter */}
                    <div>
                        <h3 className="font-heading text-lg font-semibold mb-6">Stay Updated</h3>
                        <p className="text-gray-400 text-sm mb-4">Subscribe to our newsletter for new drops and updates.</p>
                        <div className="flex flex-col gap-3">
                            <input
                                type="email"
                                placeholder="Enter your email"
                                className="bg-white/10 border border-white/20 rounded-lg px-4 py-2 text-white placeholder:text-gray-500 focus:outline-none focus:border-accent"
                            />
                            <button className="bg-accent text-primary font-medium py-2 rounded-lg hover:brightness-110 transition-all">
                                Subscribe
                            </button>
                        </div>
                    </div>
                </div>

                <div className="border-t border-white/10 pt-8 flex flex-col md:flex-row justify-between items-center gap-4">
                    <p className="text-gray-500 text-sm">© {new Date().getFullYear()} Artify. All rights reserved.</p>
                    <div className="flex gap-6">
                        <Link to="/privacy" className="text-gray-500 hover:text-white text-sm">Privacy Policy</Link>
                        <Link to="/terms" className="text-gray-500 hover:text-white text-sm">Terms of Service</Link>
                    </div>
                </div>
            </div>
        </footer>
    );
};

export default Footer;
