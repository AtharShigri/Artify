import React, { useState } from 'react';
import { useParams } from 'react-router-dom';
import { MapPin, Star, Share2, MessageCircle, MoreHorizontal } from 'lucide-react';
import Button from '../../components/common/Button';
import ProductCard from './components/ProductCard';
import SEO from '../../components/common/SEO';

// Mock Data
const artistData = {
    id: 1,
    name: 'Sara Khan',
    category: 'Visual Artist',
    location: 'Lahore, Pakistan',
    rating: 4.9,
    reviews: 124,
    bio: 'Passionate painter specializing in abstract expressionism and contemporary landscapes. Brings emotions to life through vibrant colors and bold textures.',
    avatar: 'https://images.unsplash.com/photo-1494790108377-be9c29b29330?auto=format&fit=crop&q=80&w=200',
    cover: 'https://images.unsplash.com/photo-1513364776144-60967b0f800f?auto=format&fit=crop&q=80&w=1200',
    stats: {
        artworks: 45,
        sold: 120,
        followers: '2.5k'
    }
};

const artistArtworks = Array.from({ length: 4 }).map((_, i) => ({
    id: i + 1,
    title: ['Ethereal Dreams', 'Abstract Harmony', 'Golden Dunes', 'Urban Rhythm'][i],
    artist: 'Sara Khan',
    price: `$${(i + 1) * 200 + 50}`,
    category: 'Visual Arts',
    image: [
        'https://images.unsplash.com/photo-1579783902614-a3fb392796a5?auto=format&fit=crop&q=80&w=400',
        'https://images.unsplash.com/photo-1541963463532-d68292c34b19?auto=format&fit=crop&q=80&w=400',
        'https://images.unsplash.com/photo-1513364776144-60967b0f800f?auto=format&fit=crop&q=80&w=400',
        'https://images.unsplash.com/photo-1549887552-93f8efb4133f?auto=format&fit=crop&q=80&w=400'
    ][i]
}));

const ArtistProfile = () => {
    const { id } = useParams();
    const [activeTab, setActiveTab] = useState('artworks');

    return (
        <div className="bg-background min-h-screen pb-12">
            <SEO title={`${artistData.name} - Artist Profile`} description={artistData.bio} />
            {/* Cover Image */}
            <div className="h-64 md:h-80 w-full overflow-hidden">
                <img src={artistData.cover} alt="Cover" className="w-full h-full object-cover" />
            </div>

            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 -mt-20 relative z-10">
                {/* Profile Card */}
                <div className="bg-white rounded-2xl shadow-lg p-6 md:p-8 flex flex-col md:flex-row gap-6 items-start">
                    <div className="w-32 h-32 md:w-40 md:h-40 rounded-full border-4 border-white overflow-hidden shadow-md flex-shrink-0">
                        <img src={artistData.avatar} alt={artistData.name} className="w-full h-full object-cover" />
                    </div>

                    <div className="flex-1 pt-2">
                        <div className="flex justify-between items-start mb-2">
                            <div>
                                <h1 className="text-3xl font-heading font-bold text-primary">{artistData.name}</h1>
                                <p className="text-secondary font-medium">{artistData.category}</p>
                            </div>
                            <div className="flex gap-2">
                                <Button variant="secondary" size="sm" className="hidden md:flex"><Share2 className="w-4 h-4 mr-2" /> Share</Button>
                                <Button variant="primary" size="sm">Hire Artist</Button>
                            </div>
                        </div>

                        <div className="flex items-center gap-4 text-sm text-textSecondary mb-4">
                            <span className="flex items-center"><MapPin className="w-4 h-4 mr-1" /> {artistData.location}</span>
                            <span className="flex items-center text-accent"><Star className="w-4 h-4 mr-1 fill-current" /> {artistData.rating} ({artistData.reviews} reviews)</span>
                        </div>

                        <p className="text-textSecondary leading-relaxed max-w-3xl mb-6">{artistData.bio}</p>

                        <div className="flex gap-8 border-t border-border pt-6">
                            <div className="text-center">
                                <div className="font-bold text-xl text-primary">{artistData.stats.artworks}</div>
                                <div className="text-xs text-textSecondary uppercase tracking-wide">Artworks</div>
                            </div>
                            <div className="text-center">
                                <div className="font-bold text-xl text-primary">{artistData.stats.sold}</div>
                                <div className="text-xs text-textSecondary uppercase tracking-wide">Sold</div>
                            </div>
                            <div className="text-center">
                                <div className="font-bold text-xl text-primary">{artistData.stats.followers}</div>
                                <div className="text-xs text-textSecondary uppercase tracking-wide">Followers</div>
                            </div>
                        </div>
                    </div>
                </div>

                {/* Tabs */}
                <div className="mt-8 mb-8 border-b border-border">
                    <div className="flex gap-8">
                        {['artworks', 'services', 'reviews'].map(tab => (
                            <button
                                key={tab}
                                onClick={() => setActiveTab(tab)}
                                className={`pb-4 text-sm font-bold uppercase tracking-wide transition-colors border-b-2 ${activeTab === tab ? 'border-primary text-primary' : 'border-transparent text-textSecondary hover:text-primary'}`}
                            >
                                {tab}
                            </button>
                        ))}
                    </div>
                </div>

                {/* Tab Content */}
                {activeTab === 'artworks' && (
                    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
                        {artistArtworks.map(artwork => (
                            <ProductCard key={artwork.id} artwork={artwork} />
                        ))}
                    </div>
                )}

                {activeTab !== 'artworks' && (
                    <div className="py-20 text-center text-textSecondary bg-white rounded-2xl border border-dashed border-border">
                        <p>Content for {activeTab} coming soon.</p>
                    </div>
                )}

            </div>
        </div>
    );
};

export default ArtistProfile;
