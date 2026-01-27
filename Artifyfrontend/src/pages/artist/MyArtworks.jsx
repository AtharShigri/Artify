import React from 'react';
import { Edit2, Trash2, MoreVertical } from 'lucide-react';
import Button from '../../components/common/Button';

const MyArtworks = () => {
    // Mock listing
    const artworks = Array.from({ length: 6 }).map((_, i) => ({
        id: i,
        title: `Artwork Title #${i + 1}`,
        price: `$${(i + 1) * 120}`,
        status: i % 3 === 0 ? 'Sold' : 'Active',
        views: (i + 1) * 45,
        date: 'Jan 24, 2025'
    }));

    return (
        <div>
            <div className="flex justify-between items-center mb-8">
                <div>
                    <h1 className="text-2xl font-heading font-bold text-primary">My Artworks</h1>
                    <p className="text-textSecondary">Manage your portfolio and listings</p>
                </div>
                <Button variant="primary" size="sm">Add New</Button>
            </div>

            <div className="bg-white rounded-xl shadow-sm border border-border overflow-hidden">
                <div className="overflow-x-auto">
                    <table className="w-full text-left">
                        <thead className="bg-gray-50 border-b border-border">
                            <tr>
                                <th className="px-6 py-4 font-bold text-sm text-primary">Artwork</th>
                                <th className="px-6 py-4 font-bold text-sm text-primary">Price</th>
                                <th className="px-6 py-4 font-bold text-sm text-primary">Status</th>
                                <th className="px-6 py-4 font-bold text-sm text-primary">Views</th>
                                <th className="px-6 py-4 font-bold text-sm text-primary">Date</th>
                                <th className="px-6 py-4 text-right">Actions</th>
                            </tr>
                        </thead>
                        <tbody className="divide-y divide-gray-100">
                            {artworks.map((item) => (
                                <tr key={item.id} className="hover:bg-gray-50/50">
                                    <td className="px-6 py-4">
                                        <div className="flex items-center gap-3">
                                            <div className="w-12 h-12 bg-gray-100 rounded-lg"></div>
                                            <span className="font-medium text-primary">{item.title}</span>
                                        </div>
                                    </td>
                                    <td className="px-6 py-4 text-textSecondary">{item.price}</td>
                                    <td className="px-6 py-4">
                                        <span className={`inline-block px-2 py-1 rounded-md text-xs font-medium ${item.status === 'Active' ? 'bg-green-100 text-green-700' : 'bg-gray-100 text-gray-600'}`}>
                                            {item.status}
                                        </span>
                                    </td>
                                    <td className="px-6 py-4 text-textSecondary">{item.views}</td>
                                    <td className="px-6 py-4 text-textSecondary">{item.date}</td>
                                    <td className="px-6 py-4 text-right">
                                        <div className="flex items-center justify-end gap-2">
                                            <button className="p-2 text-gray-400 hover:text-primary transition-colors"><Edit2 className="w-4 h-4" /></button>
                                            <button className="p-2 text-gray-400 hover:text-error transition-colors"><Trash2 className="w-4 h-4" /></button>
                                        </div>
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

export default MyArtworks;
