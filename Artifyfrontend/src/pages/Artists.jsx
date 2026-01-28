import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { Search, MapPin, Star } from 'lucide-react';
import marketplaceService from '../services/marketplaceService';
import Loader from '../components/common/Loader';
import SEO from '../components/common/SEO';
import Button from '../components/common/Button';

const Artists = () => {
    const [artists, setArtists] = useState([]);
    const [loading, setLoading] = useState(true);
    const [searchTerm, setSearchTerm] = useState('');

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

    const filteredArtists = artists.filter(artist =>
        artist.fullName?.toLowerCase().includes(searchTerm.toLowerCase()) ||
        artist.category?.toLowerCase().includes(searchTerm.toLowerCase())
    );

    if (loading) return <Loader />;

    return (
        <div className="min-h-screen bg-background pb-20">
            <SEO title="Artists - Artify" description="Discover talented artists from around the world." />

            {/* Header */}
            <div className="bg-white border-b border-border py-12">
                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 text-center">
                    <h1 className="text-4xl font-heading font-bold text-primary mb-4">Meet Our Artists</h1>
                    <p className="text-textSecondary max-w-2xl mx-auto mb-8">
                        Explore the creative minds behind the masterpieces. Connect with painters, sculptors, and digital artists.
                    </p>

                    <div className="relative max-w-md mx-auto">
                        <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-400" />
                        <input
                            type="text"
                            placeholder="Search artists by name or category..."
                            className="w-full pl-10 pr-4 py-3 rounded-xl border border-border focus:outline-none focus:ring-2 focus:ring-primary/20"
                            value={searchTerm}
                            onChange={(e) => setSearchTerm(e.target.value)}
                        />
                    </div>
                </div>
            </div>

            {/* Grid */}
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-12">
                {filteredArtists.length > 0 ? (
                    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-8">
                        {filteredArtists.map((artist) => (
                            <Link key={artist.artistProfileId || artist.id} to={`/artist/${artist.artistProfileId || artist.id}`} className="group">
                                <div className="bg-white rounded-2xl overflow-hidden border border-border hover:shadow-xl transition-all duration-300 h-full flex flex-col">
                                    <div className="h-48 overflow-hidden bg-gray-100 relative">
                                        {artist.profileImageUrl ? (
                                            <img
                                                src={artist.profileImageUrl}
                                                alt={artist.fullName}
                                                className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-500"
                                            />
                                        ) : (
                                            <div className="w-full h-full flex items-center justify-center text-4xl font-bold text-gray-300">
                                                {artist.fullName?.charAt(0)}
                                            </div>
                                        )}
                                        <div className="absolute top-3 right-3 bg-white/90 backdrop-blur-sm px-2 py-1 rounded-lg text-xs font-bold text-primary flex items-center gap-1 shadow-sm">
                                            <Star className="w-3 h-3 fill-accent text-accent" />
                                            {artist.rating ? artist.rating.toFixed(1) : 'New'}
                                        </div>
                                    </div>
                                    <div className="p-5 flex-1 flex flex-col">
                                        <h3 className="font-heading font-bold text-lg text-primary mb-1">{artist.fullName}</h3>
                                        <p className="text-secondary text-sm font-medium mb-3">{artist.category || 'Visual Artist'}</p>

                                        {artist.location && (
                                            <p className="text-textSecondary text-xs flex items-center gap-1 mb-4">
                                                <MapPin className="w-3 h-3" /> {artist.location}
                                            </p>
                                        )}

                                        <p className="text-textSecondary text-sm line-clamp-2 mb-4 flex-1">
                                            {artist.bio || 'No bio available yet.'}
                                        </p>

                                        <div className="border-t border-border pt-4 flex justify-between items-center text-xs text-textSecondary">
                                            <span>{artist.totalArtworks || 0} Artworks</span>
                                            <span>{artist.totalReviews || 0} Reviews</span>
                                        </div>
                                    </div>
                                </div>
                            </Link>
                        ))}
                    </div>
                ) : (
                    <div className="text-center py-20">
                        <p className="text-lg text-textSecondary">No artists found matching your search.</p>
                        <Button variant="ghost" onClick={() => setSearchTerm('')} className="mt-4">Clear Search</Button>
                    </div>
                )}
            </div>
        </div>
    );
};

export default Artists;
