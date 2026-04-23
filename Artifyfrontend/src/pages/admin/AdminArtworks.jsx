import React, { useState, useEffect } from 'react';
import { Image as ImageIcon, CheckCircle, XCircle, Search, ExternalLink, ShieldAlert } from 'lucide-react';
import adminService from '../../services/adminService';
import Loader from '../../components/common/Loader';
import { getImageUrl } from '../../utils/imageUtils';

const AdminArtworks = () => {
    const [artworks, setArtworks] = useState([]);
    const [loading, setLoading] = useState(true);
    const [filter, setFilter] = useState('All');

    useEffect(() => {
        fetchArtworks();
    }, []);

    const fetchArtworks = async () => {
        try {
            setLoading(true);
            const data = await adminService.getAllArtworks();
            setArtworks(data || []);
        } catch (err) {
            console.error("Error fetching artworks:", err);
        } finally {
            setLoading(false);
        }
    };

    const handleApprove = async (id) => {
        try {
            await adminService.approveArtwork(id);
            setArtworks(artworks.map(a => a.artworkId === id ? { ...a, status: 'Published' } : a));
        } catch (err) {
            console.error("Error approving artwork:", err);
        }
    };

    const handleReject = async (id) => {
        const reason = prompt("Enter rejection reason:");
        if (!reason) return;
        try {
            await adminService.rejectArtwork(id, reason);
            setArtworks(artworks.map(a => a.artworkId === id ? { ...a, status: 'Rejected' } : a));
        } catch (err) {
            console.error("Error rejecting artwork:", err);
        }
    };

    const filteredArtworks = filter === 'All' 
        ? artworks 
        : artworks.filter(a => a.status === filter);

    if (loading) return <Loader fullScreen />;

    return (
        <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500">
            <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                <div>
                    <h1 className="text-2xl font-heading font-bold text-primary">Artwork Moderation</h1>
                    <p className="text-textSecondary">Review and approve platform content</p>
                </div>
                <div className="flex gap-2 bg-white p-1 rounded-xl border border-border">
                    {['All', 'Pending', 'Flagged', 'Published', 'Rejected'].map(f => (
                        <button
                            key={f}
                            onClick={() => setFilter(f)}
                            className={`px-4 py-1.5 rounded-lg text-sm font-medium transition-all ${
                                filter === f ? 'bg-primary text-white shadow-sm' : 'text-textSecondary hover:bg-gray-50'
                            }`}
                        >
                            {f}
                        </button>
                    ))}
                </div>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-6">
                {filteredArtworks.map(artwork => (
                    <div key={artwork.artworkId} className="bg-white rounded-2xl shadow-sm border border-border overflow-hidden group">
                        <div className="aspect-[4/3] relative overflow-hidden">
                            <img 
                                src={getImageUrl(artwork.imageUrl)} 
                                alt={artwork.title}
                                className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-500"
                            />
                            <div className="absolute top-4 left-4">
                                <span className={`px-3 py-1 rounded-full text-xs font-bold shadow-lg ${
                                    artwork.status === 'Published' ? 'bg-green-500 text-white' :
                                    artwork.status === 'Flagged' ? 'bg-red-500 text-white' :
                                    artwork.status === 'Pending' ? 'bg-yellow-500 text-white' :
                                    'bg-gray-500 text-white'
                                }`}>
                                    {artwork.status}
                                </span>
                            </div>
                            {artwork.status === 'Flagged' && (
                                <div className="absolute bottom-0 left-0 right-0 bg-red-600/90 text-white p-2 flex items-center justify-center gap-2 text-xs font-bold">
                                    <ShieldAlert className="w-4 h-4" />
                                    POTENTIAL PLAGIARISM
                                </div>
                            )}
                        </div>
                        <div className="p-6">
                            <div className="flex justify-between items-start mb-2">
                                <h3 className="font-bold text-primary truncate flex-1">{artwork.title}</h3>
                                <a 
                                    href={`/artwork/${artwork.artworkId}`} 
                                    target="_blank" 
                                    rel="noreferrer"
                                    className="text-secondary hover:text-secondary/80"
                                >
                                    <ExternalLink className="w-4 h-4" />
                                </a>
                            </div>
                            <p className="text-sm text-textSecondary mb-4 line-clamp-2">
                                {artwork.description || "No description provided."}
                            </p>
                            
                            <div className="flex items-center justify-between pt-4 border-t border-border">
                                <div className="text-xs text-textSecondary">
                                    {new Date(artwork.createdAt).toLocaleDateString()}
                                </div>
                                <div className="flex gap-2">
                                    {artwork.status !== 'Published' && (
                                        <button 
                                            onClick={() => handleApprove(artwork.artworkId)}
                                            className="p-2 bg-green-50 text-green-600 rounded-lg hover:bg-green-100 transition-colors"
                                            title="Approve"
                                        >
                                            <CheckCircle className="w-5 h-5" />
                                        </button>
                                    )}
                                    {artwork.status !== 'Rejected' && (
                                        <button 
                                            onClick={() => handleReject(artwork.artworkId)}
                                            className="p-2 bg-red-50 text-red-600 rounded-lg hover:bg-red-100 transition-colors"
                                            title="Reject"
                                        >
                                            <XCircle className="w-5 h-5" />
                                        </button>
                                    )}
                                </div>
                            </div>
                        </div>
                    </div>
                ))}
                {filteredArtworks.length === 0 && (
                    <div className="col-span-full py-20 text-center bg-white rounded-2xl border border-dashed border-border text-textSecondary">
                        <ImageIcon className="w-12 h-12 mx-auto mb-4 opacity-20" />
                        <p>No artworks found in this category.</p>
                    </div>
                )}
            </div>
        </div>
    );
};

export default AdminArtworks;
