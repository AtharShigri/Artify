import React, { useState, useEffect } from 'react';
import { ShoppingBag, CreditCard, Search, Calendar, Filter, DollarSign } from 'lucide-react';
import adminService from '../../services/adminService';
import Loader from '../../components/common/Loader';

const AdminTransactions = () => {
    const [transactions, setTransactions] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchTransactions();
    }, []);

    const fetchTransactions = async () => {
        try {
            setLoading(true);
            const data = await adminService.getTransactions();
            setTransactions(data?.items || []);
        } catch (err) {
            console.error("Error fetching transactions:", err);
        } finally {
            setLoading(false);
        }
    };

    if (loading) return <Loader fullScreen />;

    return (
        <div className="space-y-6 animate-in fade-in duration-500">
            <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                <div>
                    <h1 className="text-2xl font-heading font-bold text-primary">Financial Logs</h1>
                    <p className="text-textSecondary">Monitor platform transactions and orders</p>
                </div>
            </div>

            <div className="bg-white rounded-2xl shadow-sm border border-border overflow-hidden">
                <div className="overflow-x-auto">
                    <table className="w-full text-left">
                        <thead className="bg-gray-50 border-b border-border">
                            <tr>
                                <th className="px-6 py-4 text-xs font-bold text-textSecondary uppercase">Transaction ID</th>
                                <th className="px-6 py-4 text-xs font-bold text-textSecondary uppercase">Amount</th>
                                <th className="px-6 py-4 text-xs font-bold text-textSecondary uppercase">Method</th>
                                <th className="px-6 py-4 text-xs font-bold text-textSecondary uppercase">Status</th>
                                <th className="px-6 py-4 text-xs font-bold text-textSecondary uppercase">Date</th>
                            </tr>
                        </thead>
                        <tbody className="divide-y divide-gray-100">
                            {transactions.map(t => (
                                <tr key={t.transactionId} className="hover:bg-gray-50/50 transition-colors">
                                    <td className="px-6 py-4 font-mono text-xs text-textSecondary">{t.transactionId}</td>
                                    <td className="px-6 py-4 font-bold text-primary">PKR {t.transactionAmount?.toLocaleString()}</td>
                                    <td className="px-6 py-4 text-sm text-textSecondary">{t.paymentMethod}</td>
                                    <td className="px-6 py-4">
                                        <span className={`px-2 py-0.5 rounded-full text-[10px] font-bold uppercase ${
                                            t.status === 'Completed' ? 'bg-green-100 text-green-700' : 'bg-yellow-100 text-yellow-700'
                                        }`}>
                                            {t.status}
                                        </span>
                                    </td>
                                    <td className="px-6 py-4 text-sm text-textSecondary">
                                        {new Date(t.transactionDate).toLocaleDateString()}
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                    {transactions.length === 0 && (
                        <div className="p-12 text-center text-textSecondary">No transactions found.</div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default AdminTransactions;
