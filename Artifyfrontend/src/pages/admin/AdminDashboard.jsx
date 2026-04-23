import React, { useState, useEffect } from 'react';
import { Users, AlertTriangle, DollarSign, Activity, Trash2, Ban, CheckCircle, Clock } from 'lucide-react';
import adminService from '../../services/adminService';
import Loader from '../../components/common/Loader';

const AdminDashboard = () => {
    const [stats, setStats] = useState(null);
    const [recentUsers, setRecentUsers] = useState([]);
    const [reports, setReports] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchDashboardData = async () => {
            try {
                setLoading(true);
                const [statsData, usersData, reportsData] = await Promise.all([
                    adminService.getDashboardStats(),
                    adminService.getRecentUsers(5),
                    adminService.getRecentPlagiarismReports(5)
                ]);

                setStats(statsData);
                setRecentUsers(usersData);
                setReports(reportsData);
            } catch (err) {
                console.error("Error fetching admin dashboard data:", err);
                setError("Failed to load dashboard data. Please try again later.");
            } finally {
                setLoading(false);
            }
        };

        fetchDashboardData();
    }, []);

    if (loading) return <Loader fullScreen />;
    if (error) return <div className="p-8 text-red-500 text-center">{error}</div>;

    return (
        <div className="space-y-8 animate-in fade-in duration-500">
            <div className="flex justify-between items-center">
                <div>
                    <h1 className="text-2xl font-heading font-bold text-primary">Admin Overview</h1>
                    <p className="text-textSecondary">Platform statistics and management</p>
                </div>
            </div>

            {/* Stats Grid */}
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
                <StatCard 
                    title="Total Users" 
                    value={stats?.totalUsers || 0} 
                    icon={<Users className="w-6 h-6" />} 
                    color="bg-blue-50 text-blue-600"
                    trend={stats?.totalArtists + " Artists"}
                />
                <StatCard 
                    title="Total Revenue" 
                    value={`PKR ${stats?.totalRevenue?.toLocaleString() || 0}`} 
                    icon={<DollarSign className="w-6 h-6" />} 
                    color="bg-green-50 text-green-600"
                    trend="Lifetime Sales"
                />
                <StatCard 
                    title="Active Alerts" 
                    value={stats?.plagiarismAlerts || 0} 
                    icon={<AlertTriangle className="w-6 h-6" />} 
                    color="bg-red-50 text-red-600"
                    trend={`${stats?.activeReports || 0} Unresolved`}
                />
                <StatCard 
                    title="Pending Artworks" 
                    value={stats?.pendingArtworks || 0} 
                    icon={<Activity className="w-6 h-6" />} 
                    color="bg-purple-50 text-purple-600"
                    trend="Needs Review"
                />
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
                {/* Users Table */}
                <div className="bg-white rounded-2xl shadow-sm border border-border overflow-hidden">
                    <div className="p-6 border-b border-border flex justify-between items-center">
                        <h2 className="font-bold text-lg text-primary">Recent Users</h2>
                        <button className="text-sm text-secondary hover:underline">View All</button>
                    </div>
                    <div className="overflow-x-auto">
                        <table className="w-full text-left">
                            <thead className="bg-gray-50/50 border-b border-border">
                                <tr>
                                    <th className="px-6 py-4 text-xs font-bold text-textSecondary uppercase tracking-wider">User</th>
                                    <th className="px-6 py-4 text-xs font-bold text-textSecondary uppercase tracking-wider">Role</th>
                                    <th className="px-6 py-4 text-xs font-bold text-textSecondary uppercase tracking-wider">Status</th>
                                    <th className="px-6 py-4 text-right">Action</th>
                                </tr>
                            </thead>
                            <tbody className="divide-y divide-gray-100">
                                {recentUsers.map(user => (
                                    <tr key={user.id} className="hover:bg-gray-50 transition-colors">
                                        <td className="px-6 py-4">
                                            <div className="flex flex-col">
                                                <span className="font-medium text-primary">{user.name}</span>
                                                <span className="text-xs text-textSecondary">{user.email}</span>
                                            </div>
                                        </td>
                                        <td className="px-6 py-4">
                                            <span className={`text-sm ${user.role === 'Artist' ? 'text-blue-600' : 'text-purple-600'}`}>
                                                {user.role}
                                            </span>
                                        </td>
                                        <td className="px-6 py-4">
                                            <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
                                                user.status === 'Active' ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
                                            }`}>
                                                {user.status}
                                            </span>
                                        </td>
                                        <td className="px-6 py-4 text-right">
                                            <button className="p-2 text-gray-400 hover:text-red-500 transition-colors">
                                                <Ban className="w-4 h-4" />
                                            </button>
                                        </td>
                                    </tr>
                                ))}
                                {recentUsers.length === 0 && (
                                    <tr>
                                        <td colSpan="4" className="px-6 py-8 text-center text-textSecondary">No recent users found.</td>
                                    </tr>
                                )}
                            </tbody>
                        </table>
                    </div>
                </div>

                {/* Reports/Plagiarism Alerts */}
                <div className="bg-white rounded-2xl shadow-sm border border-border overflow-hidden">
                    <div className="p-6 border-b border-border flex justify-between items-center">
                        <h2 className="font-bold text-lg text-primary">Plagiarism Alerts</h2>
                        <button className="text-sm text-secondary hover:underline">Monitor All</button>
                    </div>
                    <div className="divide-y divide-gray-100 max-h-[400px] overflow-y-auto">
                        {reports.map(report => (
                            <div key={report.id} className="p-6 hover:bg-gray-50 transition-colors flex items-center justify-between group">
                                <div className="flex items-center gap-4">
                                    <div className={`p-2 rounded-lg ${report.status === 'Pending' ? 'bg-yellow-50 text-yellow-600' : 'bg-gray-50 text-gray-600'}`}>
                                        <AlertTriangle className="w-5 h-5" />
                                    </div>
                                    <div>
                                        <p className="font-bold text-primary text-sm">{report.subject}</p>
                                        <div className="flex items-center gap-2 mt-1">
                                            <span className="text-xs text-textSecondary">
                                                {new Date(report.createdAt).toLocaleDateString()}
                                            </span>
                                            <span className="text-xs text-red-500 font-medium">
                                                {report.similarityScore}% Match
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div className="flex items-center gap-2">
                                    <span className={`px-2.5 py-1 rounded-full text-xs font-semibold ${
                                        report.status === 'Resolved' ? 'bg-green-100 text-green-700' : 'bg-yellow-100 text-yellow-700'
                                    }`}>
                                        {report.status}
                                    </span>
                                    <button className="p-1 text-gray-400 hover:text-primary opacity-0 group-hover:opacity-100 transition-all">
                                        <CheckCircle className="w-4 h-4" />
                                    </button>
                                </div>
                            </div>
                        ))}
                        {reports.length === 0 && (
                            <div className="p-12 text-center text-textSecondary">
                                <CheckCircle className="w-12 h-12 text-green-200 mx-auto mb-4" />
                                <p>No plagiarism alerts detected!</p>
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
};

const StatCard = ({ title, value, icon, color, trend }) => (
    <div className="bg-white p-6 rounded-2xl shadow-sm border border-border group hover:border-secondary/30 transition-all">
        <div className="flex justify-between items-start mb-4">
            <div className={`p-3 rounded-xl ${color} group-hover:scale-110 transition-transform`}>{icon}</div>
            {trend && <span className="text-gray-400 text-xs font-medium">{trend}</span>}
        </div>
        <div className="text-3xl font-bold text-primary mb-1">{value}</div>
        <div className="text-sm font-medium text-textSecondary">{title}</div>
    </div>
);

export default AdminDashboard;
