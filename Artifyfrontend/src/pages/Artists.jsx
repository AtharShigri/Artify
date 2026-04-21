import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { Search, MapPin, Star, Building2, User as UserIcon, BadgeCheck } from 'lucide-react';
import marketplaceService from '../services/marketplaceService';
import Loader from '../components/common/Loader';
import SEO from '../components/common/SEO';
import Button from '../components/common/Button';
import { getImageUrl } from '../utils/imageUtils';

const Artists = () => {
    const [artists, setArtists] = useState([]);
    const [loading, setLoading] = useState(true);
    const [searchTerm, setSearchTerm] = useState('');
    const [activeTab, setActiveTab] = useState('all'); // 'all' | 'artists' | 'agencies'

    useEffect(() => {
        fetchArtists();
    }, []);

    const fetchArtists = async () => {
        try {
            const data = await marketplaceService.getAllArtists();
            setArtists(data);
        } catch (error) {
            console.error("Failed to load artists", error);
        } finally {
            setLoading(false);
        }
    };

    const filteredArtists = (Array.isArray(artists) ? artists : []).filter(artist => {
        const matchesSearch = 
            (artist.fullName || '').toLowerCase().includes((searchTerm || '').toLowerCase()) ||
            (artist.category || '').toLowerCase().includes((searchTerm || '').toLowerCase());
        
        const matchesTab = 
            activeTab === 'all' || 
            (activeTab === 'artists' && artist.userType === 0) || 
            (activeTab === 'agencies' && artist.userType === 1);

        return matchesSearch && matchesTab;
    });

    if (loading) return <Loader />;

    const tabClass = (tab) => 
        `px-6 py-2 rounded-full text-sm font-bold transition-all ${
            activeTab === tab 
                ? 'bg-secondary text-white shadow-lg shadow-secondary/20' 
                : 'text-textSecondary hover:bg-gray-100'
        }`;

    return (
        <div className="min-h-screen bg-background pb-20">
            <SEO title="Directory - Artify" description="Discover talented artists and creative agencies." />

            {/* Header */}
            <div className="bg-white border-b border-border py-16">
                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 text-center">
                    <h1 className="text-4xl md:text-5xl font-heading font-black text-primary mb-4 tracking-tight">
                        Creative <span className="text-secondary text-glow">Directory</span>
                    </h1>
                    <p className="text-textSecondary max-w-2xl mx-auto mb-10 text-lg">
                        Connect with the world's finest individual artists and creative agencies in one place.
                    </p>

                    <div className="flex flex-col md:flex-row items-center justify-center gap-6">
                        <div className="relative w-full max-w-md">
                            <Search className="absolute left-4 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-400" />
                            <input
                                type="text"
                                placeholder="Search by name, skills or category..."
                                className="w-full pl-12 pr-4 py-4 rounded-2xl border border-gray-100 bg-gray-50/50 focus:bg-white focus:outline-none focus:ring-4 focus:ring-primary/5 transition-all"
                                value={searchTerm}
                                onChange={(e) => setSearchTerm(e.target.value)}
                            />
                        </div>

                        {/* Tabs */}
                        <div className="bg-gray-100/50 p-1.5 rounded-full flex gap-1">
                            <button onClick={() => setActiveTab('all')} className={tabClass('all')}>All</button>
                            <button onClick={() => setActiveTab('artists')} className={tabClass('artists')}>Artists</button>
                            <button onClick={() => setActiveTab('agencies')} className={tabClass('agencies')}>Agencies</button>
                        </div>
                    </div>
                </div>
            </div>

            {/* Grid */}
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-12">
                {filteredArtists.length > 0 ? (
                    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
                        {filteredArtists.map((artist) => (
                            <Link 
                                key={artist.artistProfileId || artist.id} 
                                to={`/artist/${artist.artistProfileId || artist.id}`} 
                                className="group relative"
                            >
                                <div className="bg-white rounded-3xl overflow-hidden border border-gray-100 hover:border-secondary/20 shadow-sm hover:shadow-2xl hover:shadow-primary/5 transition-all duration-500 h-full flex flex-col">
                                    {/* Profile Cover/Image */}
                                    <div className="h-44 overflow-hidden bg-gray-100 relative">
                                        {artist.profileImageUrl ? (
                                            <img
                                                src={getImageUrl(artist.profileImageUrl)}
                                                alt={artist.fullName}
                                                className="w-full h-full object-cover group-hover:scale-110 transition-transform duration-700"
                                            />
                                        ) : (
                                            <div className="w-full h-full flex items-center justify-center text-5xl font-black text-gray-200">
                                                {artist.fullName?.charAt(0)}
                                            </div>
                                        )}
                                        
                                        {/* Status Badges */}
                                        <div className="absolute top-3 left-3 flex flex-col gap-2">
                                            {artist.userType === 1 && (
                                                <div className="bg-primary/90 backdrop-blur-md text-white px-2 py-1 rounded-lg text-[10px] font-bold flex items-center gap-1">
                                                    <Building2 className="w-3 h-3" /> AGENCY
                                                </div>
                                            )}
                                            {artist.isFeatured && (
                                                <div className="bg-secondary text-white px-2 py-1 rounded-lg text-[10px] font-bold flex items-center gap-1">
                                                    <BadgeCheck className="w-3 h-3" /> FEATURED
                                                </div>
                                            )}
                                        </div>

                                        <div className="absolute bottom-3 right-3 bg-white/90 backdrop-blur-md px-2 py-1 rounded-lg text-xs font-bold text-primary flex items-center gap-1 shadow-sm">
                                            <Star className="w-3 h-3 fill-accent text-accent" />
                                            {artist.rating ? artist.rating.toFixed(1) : 'New'}
                                        </div>
                                    </div>

                                    {/* Content */}
                                    <div className="p-6 flex-1 flex flex-col">
                                        <div className="flex items-center gap-2 mb-1">
                                            <h3 className="font-heading font-bold text-lg text-primary truncate">
                                                {artist.fullName}
                                            </h3>
                                            {artist.rating > 4.5 && <BadgeCheck className="w-4 h-4 text-secondary shrink-0" />}
                                        </div>
                                        
                                        <p className="text-secondary text-sm font-semibold mb-3">
                                            {artist.category || 'Visual Artist'}
                                        </p>

                                        {artist.location && (
                                            <p className="text-textSecondary text-xs flex items-center gap-1 mb-4">
                                                <MapPin className="w-3 h-3" /> {artist.location}
                                            </p>
                                        )}

                                        <p className="text-textSecondary text-xs leading-relaxed line-clamp-3 mb-6 flex-1 italic">
                                            "{artist.bio || 'Creating beauty through the lens of imagination...'}"
                                        </p>

                                        <div className="pt-4 border-t border-gray-50 flex justify-between items-center text-[10px] uppercase tracking-wider font-bold text-textSecondary opacity-60">
                                            <span>{artist.totalArtworks || 0} Artworks</span>
                                            <span>{artist.totalReviews || 0} Reviews</span>
                                        </div>
                                    </div>
                                </div>
                            </Link>
                        ))}
                    </div>
                ) : (
                    <div className="text-center py-24 bg-white rounded-[3rem] border border-gray-100">
                        <div className="flex justify-center mb-4">
                            <Search className="w-12 h-12 text-gray-200" />
                        </div>
                        <h3 className="text-xl font-bold text-primary mb-2">No matches found</h3>
                        <p className="text-textSecondary mb-8">Try adjusting your filters or search term.</p>
                        <Button variant="outline" onClick={() => { setSearchTerm(''); setActiveTab('all'); }}>
                            Reset All Filters
                        </Button>
                    </div>
                )}
            </div>
        </div>
    );
};

export default Artists;

