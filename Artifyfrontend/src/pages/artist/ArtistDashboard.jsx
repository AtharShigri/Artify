import React from 'react';
import { DollarSign, Image as ImageIcon, ShoppingBag, TrendingUp } from 'lucide-react';
import { motion } from 'framer-motion';

const StatCard = ({ title, value, icon: Icon, color, delay }) => (
    <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ delay }}
        className="bg-white p-6 rounded-xl shadow-sm border border-border flex items-center gap-4"
    >
        <div className={`w-12 h-12 rounded-lg ${color} flex items-center justify-center`}>
            <Icon className="w-6 h-6 text-white" />
        </div>
        <div>
            <p className="text-sm text-textSecondary">{title}</p>
            <h3 className="text-2xl font-bold text-primary">{value}</h3>
        </div>
    </motion.div>
);

const ArtistDashboard = () => {
    return (
        <div className="space-y-8">
            <div>
                <h1 className="text-2xl font-heading font-bold text-primary">Dashboard Overview</h1>
                <p className="text-textSecondary">Welcome back, here's how your art is performing.</p>
            </div>

            {/* Stats Grid */}
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
                <StatCard
                    title="Total Earnings"
                    value="$12,450"
                    icon={DollarSign}
                    color="bg-green-500"
                    delay={0.1}
                />
                <StatCard
                    title="Artworks Sold"
                    value="24"
                    icon={ShoppingBag}
                    color="bg-primary"
                    delay={0.2}
                />
                <StatCard
                    title="Active Listings"
                    value="12"
                    icon={ImageIcon}
                    color="bg-secondary"
                    delay={0.3}
                />
                <StatCard
                    title="Profile Views"
                    value="1.2k"
                    icon={TrendingUp}
                    color="bg-accent"
                    delay={0.4}
                />
            </div>

            {/* Recent Activity Mock */}
            <div className="bg-white rounded-xl shadow-sm border border-border overflow-hidden">
                <div className="p-6 border-b border-border">
                    <h2 className="font-bold text-lg text-primary">Recent Orders</h2>
                </div>
                <div className="divide-y divide-gray-100">
                    {[1, 2, 3].map((i) => (
                        <div key={i} className="p-4 flex items-center justify-between hover:bg-gray-50 transition-colors">
                            <div className="flex items-center gap-4">
                                <div className="w-10 h-10 bg-gray-100 rounded-lg"></div>
                                <div>
                                    <p className="font-medium text-sm text-primary">Abstract Painting #{i}</p>
                                    <p className="text-xs text-textSecondary">Buyer: John Doe</p>
                                </div>
                            </div>
                            <div className="text-right">
                                <p className="font-bold text-sm text-primary">$450</p>
                                <span className="inline-block px-2 py-0.5 rounded-full text-xs bg-green-100 text-green-700">Completed</span>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
};

export default ArtistDashboard;
