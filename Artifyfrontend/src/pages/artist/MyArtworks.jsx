import React, { useEffect, useState } from 'react';
import { Edit2, Trash2, MoreVertical } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import Button from '../../components/common/Button';
import artworkService from '../../services/artworkService';
import Loader from '../../components/common/Loader';

const MyArtworks = () => {
    const navigate = useNavigate();
    const [artworks, setArtworks] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchArtworks();
    }, []);

    const fetchArtworks = async () => {
        try {
            const data = await artworkService.getAllByArtist();
            setArtworks(data || []);
        } catch (error) {
            console.error("Failed to fetch artworks", error);
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = async (id) => {
        if (window.confirm('Are you sure you want to delete this artwork?')) {
            try {
                await artworkService.delete(id);
                setArtworks(artworks.filter(a => a.artworkId !== id));
            } catch (error) {
                console.error("Failed to delete artwork", error);
                alert("Failed to delete artwork");
            }
        }
    };

    if (loading) return <Loader />;

    return (
        <div>
            <div className="flex justify-between items-center mb-8">
                <div>
                    <h1 className="text-2xl font-heading font-bold text-primary">My Artworks</h1>
                    <p className="text-textSecondary">Manage your portfolio and listings</p>
                </div>
                <Button variant="primary" size="sm" onClick={() => navigate('/dashboard/artist/upload')}>Add New</Button>
            </div>

            <div className="bg-white rounded-xl shadow-sm border border-border overflow-hidden">
                <div className="overflow-x-auto">
                    <table className="w-full text-left">
                        <thead className="bg-gray-50 border-b border-border">
                            <tr>
                                <th className="px-6 py-4 font-bold text-sm text-primary">Artwork</th>
                                <th className="px-6 py-4 font-bold text-sm text-primary">Price</th>
                                <th className="px-6 py-4 font-bold text-sm text-primary">Status</th>
                                <th className="px-6 py-4 font-bold text-sm text-primary">Date</th>
                                <th className="px-6 py-4 text-right">Actions</th>
                            </tr>
                        </thead>
                        <tbody className="divide-y divide-gray-100">
                            {artworks.length > 0 ? (
                                artworks.map((item) => (
                                    <tr key={item.artworkId} className="hover:bg-gray-50/50">
                                        <td className="px-6 py-4">
                                            <div className="flex items-center gap-3">
                                                <div className="w-12 h-12 bg-gray-100 rounded-lg overflow-hidden">
                                                    {item.imageUrl && (
                                                        <img src={item.imageUrl} alt={item.title} className="w-full h-full object-cover" />
                                                    )}
                                                </div>
                                                <span className="font-medium text-primary">{item.title}</span>
                                            </div>
                                        </td>
                                        <td className="px-6 py-4 text-textSecondary">${item.price}</td>
                                        <td className="px-6 py-4">
                                            <span className={`inline-block px-2 py-1 rounded-md text-xs font-medium ${item.isForSale ? 'bg-green-100 text-green-700' : 'bg-gray-100 text-gray-600'}`}>
                                                {item.isForSale ? 'For Sale' : 'Private'}
                                            </span>
                                        </td>
                                        <td className="px-6 py-4 text-textSecondary">{new Date(item.createdAt).toLocaleDateString()}</td>
                                        <td className="px-6 py-4 text-right">
                                            <div className="flex items-center justify-end gap-2">
                                                <button className="p-2 text-gray-400 hover:text-primary transition-colors"><Edit2 className="w-4 h-4" /></button>
                                                <button
                                                    className="p-2 text-gray-400 hover:text-error transition-colors"
                                                    onClick={() => handleDelete(item.artworkId)}
                                                >
                                                    <Trash2 className="w-4 h-4" />
                                                </button>
                                            </div>
                                        </td>
                                    </tr>
                                ))
                            ) : (
                                <tr>
                                    <td colSpan="5" className="px-6 py-8 text-center text-textSecondary">
                                        You haven't uploaded any artworks yet.
                                    </td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    );
};

export default MyArtworks;
