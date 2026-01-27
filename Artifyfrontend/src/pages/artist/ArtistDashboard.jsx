import React, { useEffect, useState } from 'react';
import { DollarSign, Image as ImageIcon, ShoppingBag, TrendingUp } from 'lucide-react';
import { motion } from 'framer-motion';
import artistService from '../../services/artistService';
import Loader from '../../components/common/Loader';

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
    const [stats, setStats] = useState({
        earnings: 0,
        soldArtworks: 0,
        totalArtworks: 0,
        reviews: 0
    });
    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchDashboardData = async () => {
            try {
                // Parallel fetch for better performance
                const [summaryRes, earningsRes, artworkStatsRes, ordersRes] = await Promise.all([
                    artistService.getSummary(),
                    artistService.getEarnings(),
                    artistService.getArtworkStats(),
                    artistService.getOrders()
                ]);

                setStats({
                    earnings: earningsRes.totalEarnings || 0,
                    soldArtworks: artworkStatsRes.soldArtworks || 0,
                    totalArtworks: summaryRes.totalArtworks || 0,
                    reviews: summaryRes.totalReviews || 0
                });
                setOrders(ordersRes || []);
            } catch (error) {
                console.error("Failed to load dashboard data", error);
            } finally {
                setLoading(false);
            }
        };

        fetchDashboardData();
    }, []);

    if (loading) return <Loader />;

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
                    value={`$${stats.earnings}`}
                    icon={DollarSign}
                    color="bg-green-500"
                    delay={0.1}
                />
                <StatCard
                    title="Artworks Sold"
                    value={stats.soldArtworks}
                    icon={ShoppingBag}
                    color="bg-primary"
                    delay={0.2}
                />
                <StatCard
                    title="Total Artworks"
                    value={stats.totalArtworks}
                    icon={ImageIcon}
                    color="bg-secondary"
                    delay={0.3}
                />
                <StatCard
                    title="Total Reviews"
                    value={stats.reviews}
                    icon={TrendingUp}
                    color="bg-accent"
                    delay={0.4}
                />
            </div>

            {/* Recent Activity */}
            <div className="bg-white rounded-xl shadow-sm border border-border overflow-hidden">
                <div className="p-6 border-b border-border">
                    <h2 className="font-bold text-lg text-primary">Recent Orders</h2>
                </div>
                <div className="divide-y divide-gray-100">
                    {orders.length > 0 ? (
                        orders.map((order, i) => (
                            <div key={order.orderId || i} className="p-4 flex items-center justify-between hover:bg-gray-50 transition-colors">
                                <div className="flex items-center gap-4">
                                    <div className="w-10 h-10 bg-gray-100 rounded-lg flex items-center justify-center text-xs text-gray-500">
                                        IMG
                                    </div>
                                    <div>
                                        <p className="font-medium text-sm text-primary">{order.artwork || "Artwork"}</p>
                                        <p className="text-xs text-textSecondary">Buyer: {order.buyer || "Unknown"}</p>
                                    </div>
                                </div>
                                <div className="text-right">
                                    <p className="font-bold text-sm text-primary">Status</p>
                                    <span className={`inline-block px-2 py-0.5 rounded-full text-xs ${order.paymentStatus === 'Paid' ? 'bg-green-100 text-green-700' : 'bg-yellow-100 text-yellow-700'}`}>
                                        {order.paymentStatus || 'Pending'}
                                    </span>
                                </div>
                            </div>
                        ))
                    ) : (
                        <div className="p-8 text-center text-textSecondary">No recent orders found.</div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default ArtistDashboard;
