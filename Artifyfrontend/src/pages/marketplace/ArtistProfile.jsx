import React, { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import { MapPin, Star, Share2, MessageCircle, MoreHorizontal, Loader2 } from 'lucide-react';
import Button from '../../components/common/Button';
import ProductCard from './components/ProductCard';
import SEO from '../../components/common/SEO';
import marketplaceService from '../../services/marketplaceService';

const ArtistProfile = () => {
    const { id } = useParams();
    const [activeTab, setActiveTab] = useState('artworks');
    const [artistData, setArtistData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchProfile = async () => {
            try {
                setLoading(true);
                const data = await marketplaceService.getArtistProfile(id);
                setArtistData(data);
            } catch (err) {
                console.error("Failed to load artist profile:", err);
                setError("Artist not found or an error occurred.");
            } finally {
                setLoading(false);
            }
        };
        if (id) {
            fetchProfile();
        }
    }, [id]);

    if (loading) {
        return (
            <div className="flex flex-col justify-center items-center h-screen bg-background text-primary">
                <Loader2 className="w-12 h-12 animate-spin mb-4 text-accent" />
                <p className="text-xl font-medium text-textSecondary">Loading profile...</p>
            </div>
        );
    }

    if (error || !artistData) {
        return (
             <div className="flex flex-col justify-center items-center h-[70vh] bg-background">
                <h2 className="text-3xl font-heading font-bold text-primary mb-4">Profile Unavailable</h2>
                <p className="text-textSecondary mb-8">{error || "The requested artist profile could not be found."}</p>
                <Link to="/marketplace">
                    <Button variant="primary">Return to Marketplace</Button>
                </Link>
            </div>
        );
    }

    // Use a default cover image gradient since DTO doesn't have an explicit cover photo right now
    const coverGradient = "bg-gradient-to-r from-slate-900 via-indigo-900 to-slate-900";

    return (
        <div className="bg-background min-h-screen pb-12">
            <SEO title={`${artistData.fullName} - Artist Profile`} description={artistData.bio || `Profile of ${artistData.fullName}`} />
            
            {/* Cover Image */}
            <div className={`h-64 md:h-80 w-full overflow-hidden ${coverGradient} relative`}>
                <div className="absolute inset-0 opacity-20 bg-[url('https://images.unsplash.com/photo-1513364776144-60967b0f800f?auto=format&fit=crop&q=80&w=1920')] bg-blend-overlay bg-cover bg-center"></div>
            </div>

            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 -mt-20 relative z-10">
                {/* Profile Card */}
                <div className="bg-white rounded-2xl shadow-lg p-6 md:p-8 flex flex-col md:flex-row gap-6 items-start">
                    <div className="w-32 h-32 md:w-40 md:h-40 rounded-full border-4 border-white overflow-hidden shadow-md flex-shrink-0 bg-gray-50 flex items-center justify-center relative">
                        {artistData.profileImageUrl ? (
                            <img src={artistData.profileImageUrl} alt={artistData.fullName} className="w-full h-full object-cover" />
                        ) : (
                            <span className="text-5xl font-heading font-bold text-primary opacity-30">{artistData.fullName?.charAt(0) || 'A'}</span>
                        )}
                    </div>

                    <div className="flex-1 pt-2 w-full">
                        <div className="flex flex-col md:flex-row md:justify-between items-start mb-4 gap-4">
                            <div>
                                <h1 className="text-3xl font-heading font-bold text-primary">{artistData.fullName}</h1>
                                <p className="text-secondary font-medium">{artistData.category || 'Independent Artist'}</p>
                            </div>
                            <div className="flex gap-2 w-full md:w-auto">
                                <Button variant="secondary" size="sm" className="hidden md:flex"><Share2 className="w-4 h-4 mr-2" /> Share</Button>
                                <Button variant="primary" size="sm" className="flex-1 md:flex-none">Contact</Button>
                            </div>
                        </div>

                        <div className="flex flex-wrap items-center gap-4 text-sm text-textSecondary mb-4">
                            {artistData.location && (
                                <span className="flex items-center"><MapPin className="w-4 h-4 mr-1" /> {artistData.location}</span>
                            )}
                            <span className="flex items-center text-accent"><Star className="w-4 h-4 mr-1 fill-current" /> {artistData.rating?.toFixed(1) || "0.0"} ({artistData.totalReviews || 0} reviews)</span>
                        </div>

                        {artistData.skills && artistData.skills.length > 0 && (
                            <div className="flex gap-2 flex-wrap mb-4">
                                {artistData.skills.map((skill, i) => (
                                    <span key={i} className="px-2 py-1 bg-gray-100 text-xs rounded-md text-gray-600 border border-gray-200">
                                        {skill}
                                    </span>
                                ))}
                            </div>
                        )}

                        <p className="text-textSecondary leading-relaxed max-w-3xl mb-6">
                            {artistData.bio || "This artist hasn't written a biography yet."}
                        </p>

                        <div className="flex gap-8 border-t border-border pt-6 mt-auto">
                            <div className="text-center">
                                <div className="font-bold text-xl text-primary">{artistData.totalArtworks || 0}</div>
                                <div className="text-xs text-textSecondary uppercase tracking-wide">Artworks</div>
                            </div>
                            <div className="text-center">
                                <div className="font-bold text-xl text-primary">{artistData.totalReviews || 0}</div>
                                <div className="text-xs text-textSecondary uppercase tracking-wide">Reviews</div>
                            </div>
                            <div className="text-center">
                                <div className="font-bold text-xl text-primary">{artistData.rating?.toFixed(1) || "0.0"}</div>
                                <div className="text-xs text-textSecondary uppercase tracking-wide">Rating</div>
                            </div>
                        </div>
                    </div>
                </div>

                {/* Tabs */}
                <div className="mt-8 mb-8 border-b border-border">
                    <div className="flex gap-8 overflow-x-auto no-scrollbar">
                        {['artworks', 'portfolio', 'about'].map(tab => (
                            <button
                                key={tab}
                                onClick={() => setActiveTab(tab)}
                                className={`pb-4 text-sm font-bold uppercase tracking-wide transition-colors whitespace-nowrap border-b-2 ${activeTab === tab ? 'border-primary text-primary' : 'border-transparent text-textSecondary hover:text-primary'}`}
                            >
                                {tab}
                            </button>
                        ))}
                    </div>
                </div>

                {/* Tab Content */}
                {activeTab === 'artworks' && (
                    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
                        {artistData.featuredArtworks && artistData.featuredArtworks.length > 0 ? (
                            artistData.featuredArtworks.map(artwork => (
                                <ProductCard key={artwork.artworkId || artwork.id} artwork={artwork} />
                            ))
                        ) : (
                            <div className="col-span-full py-20 text-center text-textSecondary bg-white rounded-2xl border border-dashed border-border flex flex-col items-center">
                                <Palette className="w-12 h-12 text-gray-300 mb-3" />
                                <p className="text-lg font-medium text-gray-500">No Artworks Found</p>
                                <p className="text-sm">This artist hasn't listed any artworks for sale yet.</p>
                            </div>
                        )}
                    </div>
                )}

                {activeTab !== 'artworks' && (
                    <div className="py-20 text-center text-textSecondary bg-white rounded-2xl border border-dashed border-border flex flex-col items-center">
                         <div className="w-16 h-1 bg-gray-200 rounded-full mb-6"></div>
                        <p className="text-lg font-medium text-gray-400">Content for {activeTab} coming soon.</p>
                    </div>
                )}

            </div>
        </div>
    );
};

export default ArtistProfile;
