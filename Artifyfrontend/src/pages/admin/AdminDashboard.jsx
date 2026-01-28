import React from 'react';
import { Users, AlertTriangle, DollarSign, Activity, Trash2, Ban } from 'lucide-react';

const AdminDashboard = () => {
    // Mock Data
    const users = [
        { id: 1, name: 'Sara Khan', role: 'Artist', status: 'Active', joined: 'Jan 2025' },
        { id: 2, name: 'Yahya Khan', role: 'Buyer', status: 'Active', joined: 'Dec 2024' },
        { id: 3, name: 'Ali Khan', role: 'Buyer', status: 'Active', joined: 'Feb 2025' },
    ];

    const reports = [
        { id: 101, type: 'Plagiarism', subject: 'Artwork #459', status: 'Pending' },
        { id: 102, type: 'Harassment', subject: 'User #882', status: 'Resolved' },
    ];

    return (
        <div className="space-y-8">
            <div className="flex justify-between items-center">
                <div>
                    <h1 className="text-2xl font-heading font-bold text-primary">Admin Overview</h1>
                    <p className="text-textSecondary">Platform statistics and management</p>
                </div>
            </div>

            {/* Stats */}
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
                <div className="bg-white p-6 rounded-xl shadow-sm border border-border">
                    <div className="flex justify-between items-start mb-4">
                        <div className="p-3 bg-blue-100 rounded-lg text-blue-600"><Users className="w-6 h-6" /></div>
                        <span className="text-green-500 text-xs font-bold">+0%</span>
                    </div>
                    <div className="text-2xl font-bold text-primary">4</div>
                    <div className="text-sm text-textSecondary">Total Users</div>
                </div>

                <div className="bg-white p-6 rounded-xl shadow-sm border border-border">
                    <div className="flex justify-between items-start mb-4">
                        <div className="p-3 bg-green-100 rounded-lg text-green-600"><DollarSign className="w-6 h-6" /></div>
                        <span className="text-green-500 text-xs font-bold">+0%</span>
                    </div>
                    <div className="text-2xl font-bold text-primary">$0</div>
                    <div className="text-sm text-textSecondary">Total Revenue</div>
                </div>

                <div className="bg-white p-6 rounded-xl shadow-sm border border-border">
                    <div className="flex justify-between items-start mb-4">
                        <div className="p-3 bg-red-100 rounded-lg text-red-600"><AlertTriangle className="w-6 h-6" /></div>
                        <span className="text-red-500 text-xs font-bold">0 Pending</span>
                    </div>
                    <div className="text-2xl font-bold text-primary">0</div>
                    <div className="text-sm text-textSecondary">Active Reports</div>
                </div>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
                {/* Users Table */}
                <div className="bg-white rounded-xl shadow-sm border border-border overflow-hidden">
                    <div className="p-6 border-b border-border">
                        <h2 className="font-bold text-lg text-primary">Recent Users</h2>
                    </div>
                    <div className="overflow-x-auto">
                        <table className="w-full text-left">
                            <thead className="bg-gray-50 border-b border-border">
                                <tr>
                                    <th className="px-6 py-3 text-xs font-bold text-textSecondary uppercase">Name</th>
                                    <th className="px-6 py-3 text-xs font-bold text-textSecondary uppercase">Role</th>
                                    <th className="px-6 py-3 text-xs font-bold text-textSecondary uppercase">Status</th>
                                    <th className="px-6 py-3 text-right">Action</th>
                                </tr>
                            </thead>
                            <tbody className="divide-y divide-gray-100">
                                {users.map(user => (
                                    <tr key={user.id}>
                                        <td className="px-6 py-4 font-medium text-primary">{user.name}</td>
                                        <td className="px-6 py-4 text-textSecondary">{user.role}</td>
                                        <td className="px-6 py-4">
                                            <span className={`px-2 py-1 rounded-full text-xs ${user.status === 'Active' ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'}`}>
                                                {user.status}
                                            </span>
                                        </td>
                                        <td className="px-6 py-4 text-right">
                                            <button className="text-red-500 hover:text-red-700"><Ban className="w-4 h-4" /></button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>

                {/* Reports/Logs */}
                <div className="bg-white rounded-xl shadow-sm border border-border overflow-hidden">
                    <div className="p-6 border-b border-border">
                        <h2 className="font-bold text-lg text-primary">Recent Reports</h2>
                    </div>
                    <div className="divide-y divide-gray-100">
                        {reports.map(report => (
                            <div key={report.id} className="p-4 flex items-center justify-between">
                                <div>
                                    <p className="font-bold text-primary text-sm">{report.type}</p>
                                    <p className="text-xs text-textSecondary">{report.subject}</p>
                                </div>
                                <span className={`px-2 py-1 rounded-full text-xs ${report.status === 'Resolved' ? 'bg-gray-100 text-gray-600' : 'bg-yellow-100 text-yellow-700'}`}>
                                    {report.status}
                                </span>
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    );
};

export default AdminDashboard;
