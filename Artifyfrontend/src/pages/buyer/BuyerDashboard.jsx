import React from 'react';
import { Package, Clock, CheckCircle } from 'lucide-react';
import Button from '../../components/common/Button';

const BuyerDashboard = () => {
    // Mock orders
    const orders = [
        { id: '#ORD-7829', date: 'Jan 24, 2025', items: 2, total: '$950', status: 'In Transit', artist: 'Sara Khan' },
        { id: '#ORD-7810', date: 'Jan 10, 2025', items: 1, total: '$450', status: 'Delivered', artist: 'Ali Ahmed' },
        { id: '#ORD-7750', date: 'Dec 28, 2024', items: 1, total: '$1,200', status: 'Delivered', artist: 'John Doe' },
    ];

    return (
        <div className="space-y-8">
            <div className='flex justify-between items-center'>
                <div>
                    <h1 className="text-2xl font-heading font-bold text-primary">My Orders</h1>
                    <p className="text-textSecondary">Track your purchases and view history</p>
                </div>
                <Button variant="secondary" size="sm">Browse Marketplace</Button>
            </div>

            <div className="bg-white rounded-xl shadow-sm border border-border overflow-hidden">
                <div className="overflow-x-auto">
                    <table className="w-full text-left">
                        <thead className="bg-gray-50 border-b border-border">
                            <tr>
                                <th className="px-6 py-4 font-bold text-sm text-primary">Order ID</th>
                                <th className="px-6 py-4 font-bold text-sm text-primary">Date</th>
                                <th className="px-6 py-4 font-bold text-sm text-primary">Artist</th>
                                <th className="px-6 py-4 font-bold text-sm text-primary">Total</th>
                                <th className="px-6 py-4 font-bold text-sm text-primary">Status</th>
                                <th className="px-6 py-4 text-right">Actions</th>
                            </tr>
                        </thead>
                        <tbody className="divide-y divide-gray-100">
                            {orders.map((order) => (
                                <tr key={order.id} className="hover:bg-gray-50/50">
                                    <td className="px-6 py-4 font-medium text-primary">{order.id}</td>
                                    <td className="px-6 py-4 text-textSecondary">{order.date}</td>
                                    <td className="px-6 py-4 text-textSecondary">{order.artist}</td>
                                    <td className="px-6 py-4 font-medium text-primary">{order.total}</td>
                                    <td className="px-6 py-4">
                                        <div className={`inline-flex items-center gap-1.5 px-2.5 py-1 rounded-md text-xs font-medium ${order.status === 'Delivered' ? 'bg-green-100 text-green-700' : 'bg-blue-100 text-blue-700'}`}>
                                            {order.status === 'Delivered' ? <CheckCircle className="w-3 h-3" /> : <Package className="w-3 h-3" />}
                                            {order.status}
                                        </div>
                                    </td>
                                    <td className="px-6 py-4 text-right">
                                        <Button variant="ghost" size="sm">View Details</Button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    );
};

export default BuyerDashboard;
