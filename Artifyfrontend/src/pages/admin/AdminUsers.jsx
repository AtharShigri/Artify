import React, { useState, useEffect } from 'react';
import { Users, Ban, CheckCircle, Search, Mail, Calendar } from 'lucide-react';
import adminService from '../../services/adminService';
import Loader from '../../components/common/Loader';

const AdminUsers = () => {
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [searchTerm, setSearchTerm] = useState('');

    useEffect(() => {
        fetchUsers();
    }, []);

    const fetchUsers = async () => {
        try {
            setLoading(true);
            const data = await adminService.getAllUsers();
            setUsers(data || []);
        } catch (err) {
            console.error("Error fetching users:", err);
        } finally {
            setLoading(false);
        }
    };

    const handleToggleStatus = async (userId, currentStatus) => {
        try {
            await adminService.updateUserStatus(userId, !currentStatus);
            // Update local state
            setUsers(users.map(u => u.id === userId ? { ...u, isActive: !currentStatus } : u));
        } catch (err) {
            console.error("Error updating user status:", err);
        }
    };

    const filteredUsers = users.filter(user => 
        user.fullName?.toLowerCase().includes(searchTerm.toLowerCase()) ||
        user.email?.toLowerCase().includes(searchTerm.toLowerCase())
    );

    if (loading) return <Loader fullScreen />;

    return (
        <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500">
            <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                <div>
                    <h1 className="text-2xl font-heading font-bold text-primary">User Management</h1>
                    <p className="text-textSecondary">Monitor and manage platform users</p>
                </div>
                <div className="w-full md:w-72">
                    <div className="relative">
                        <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
                        <input 
                            type="text"
                            placeholder="Search by name or email..."
                            className="w-full pl-10 pr-4 py-2 border border-border rounded-xl focus:ring-2 focus:ring-secondary/20 focus:border-secondary outline-none transition-all"
                            value={searchTerm}
                            onChange={(e) => setSearchTerm(e.target.value)}
                        />
                    </div>
                </div>
            </div>

            <div className="bg-white rounded-2xl shadow-sm border border-border overflow-hidden">
                <div className="overflow-x-auto">
                    <table className="w-full text-left">
                        <thead className="bg-gray-50 border-b border-border">
                            <tr>
                                <th className="px-6 py-4 text-xs font-bold text-textSecondary uppercase tracking-wider">User Details</th>
                                <th className="px-6 py-4 text-xs font-bold text-textSecondary uppercase tracking-wider">Join Date</th>
                                <th className="px-6 py-4 text-xs font-bold text-textSecondary uppercase tracking-wider">Status</th>
                                <th className="px-6 py-4 text-right">Actions</th>
                            </tr>
                        </thead>
                        <tbody className="divide-y divide-gray-100">
                            {filteredUsers.map(user => (
                                <tr key={user.id} className="hover:bg-gray-50/50 transition-colors">
                                    <td className="px-6 py-4">
                                        <div className="flex items-center gap-3">
                                            <div className="w-10 h-10 rounded-full bg-secondary/10 flex items-center justify-center text-secondary font-bold">
                                                {user.fullName?.charAt(0)}
                                            </div>
                                            <div>
                                                <p className="font-bold text-primary">{user.fullName}</p>
                                                <div className="flex items-center gap-1 text-xs text-textSecondary">
                                                    <Mail className="w-3 h-3" />
                                                    {user.email}
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                    <td className="px-6 py-4">
                                        <div className="flex items-center gap-2 text-sm text-textSecondary">
                                            <Calendar className="w-4 h-4" />
                                            {new Date(user.createdAt).toLocaleDateString()}
                                        </div>
                                    </td>
                                    <td className="px-6 py-4">
                                        <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
                                            user.isActive ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
                                        }`}>
                                            {user.isActive ? 'Active' : 'Blocked'}
                                        </span>
                                    </td>
                                    <td className="px-6 py-4 text-right">
                                        <button 
                                            onClick={() => handleToggleStatus(user.id, user.isActive)}
                                            className={`p-2 rounded-lg transition-colors ${
                                                user.isActive 
                                                ? 'text-gray-400 hover:bg-red-50 hover:text-red-500' 
                                                : 'text-gray-400 hover:bg-green-50 hover:text-green-500'
                                            }`}
                                            title={user.isActive ? "Block User" : "Unblock User"}
                                        >
                                            {user.isActive ? <Ban className="w-5 h-5" /> : <CheckCircle className="w-5 h-5" />}
                                        </button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                    {filteredUsers.length === 0 && (
                        <div className="p-12 text-center text-textSecondary">
                            No users found matching your search.
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default AdminUsers;
