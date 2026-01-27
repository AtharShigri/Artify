import React from 'react';
import { useParams, Link } from 'react-router-dom';
import { ArrowLeft, Share2, Heart, Shield, Check } from 'lucide-react';
import Button from '../../components/common/Button';
import { useCart } from '../../context/CartContext';
import SEO from '../../components/common/SEO';

// Mock Data
const artworkData = {
    id: 1,
    title: 'Ethereal Dreams',
    artist: 'Sara Khan',
    artistId: 1,
    price: '$450',
    description: 'Original acrylic painting on canvas. This piece explores the boundaries between reality and the dream state, creating a soothing yet thought-provoking atmosphere perfect for modern living spaces.',
    dimensions: '24 x 36 inches',
    medium: 'Acrylic on Canvas',
    year: '2025',
    image: 'https://images.unsplash.com/photo-1579783902614-a3fb392796a5?auto=format&fit=crop&q=80&w=1200',
    tags: ['Abstract', 'Blue', 'Modern', 'Canvas']
};

const ArtworkDetails = () => {
    const { id } = useParams();
    const { addToCart } = useCart();

    // In real app, fetch data based on ID
    const artwork = artworkData;

    return (
        <div className="bg-white min-h-screen py-8">
            <SEO title={artwork.title} description={artwork.description} />
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                <Link to="/marketplace" className="inline-flex items-center text-textSecondary hover:text-primary mb-6 transition-colors">
                    <ArrowLeft className="w-4 h-4 mr-2" /> Back to Marketplace
                </Link>

                <div className="grid grid-cols-1 lg:grid-cols-2 gap-12">
                    {/* Image Section */}
                    <div className="bg-gray-50 rounded-2xl overflow-hidden border border-border">
                        <img src={artwork.image} alt={artwork.title} className="w-full h-full object-contain max-h-[600px]" />
                    </div>

                    {/* Details Section */}
                    <div>
                        <div className="flex justify-between items-start mb-4">
                            <div>
                                <h1 className="text-3xl md:text-4xl font-heading font-bold text-primary mb-2">{artwork.title}</h1>
                                <Link to={`/artist/${artwork.artistId}`} className="text-lg text-secondary font-medium hover:underline">
                                    by {artwork.artist}
                                </Link>
                            </div>
                            <div className="flex gap-2">
                                <button className="p-2 rounded-full hover:bg-gray-100 text-textSecondary"><Share2 className="w-5 h-5" /></button>
                                <button className="p-2 rounded-full hover:bg-gray-100 text-textSecondary"><Heart className="w-5 h-5" /></button>
                            </div>
                        </div>

                        <div className="text-3xl font-bold text-primary mb-8">{artwork.price}</div>

                        <div className="bg-background rounded-xl p-6 mb-8 border border-border">
                            <h3 className="font-bold text-primary mb-4">Artwork Details</h3>
                            <div className="grid grid-cols-2 gap-y-4 text-sm">
                                <div className="text-textSecondary">Medium</div>
                                <div className="font-medium text-primary text-right">{artwork.medium}</div>

                                <div className="text-textSecondary">Dimensions</div>
                                <div className="font-medium text-primary text-right">{artwork.dimensions}</div>

                                <div className="text-textSecondary">Year</div>
                                <div className="font-medium text-primary text-right">{artwork.year}</div>
                            </div>
                        </div>

                        <p className="text-textSecondary leading-relaxed mb-8">{artwork.description}</p>

                        <div className="flex flex-col gap-4 mb-8">
                            <Button
                                variant="primary"
                                size="lg"
                                className="w-full"
                                onClick={() => addToCart(artwork)}
                            >
                                Add to Cart
                            </Button>
                            <Button variant="secondary" size="lg" className="w-full">Make an Offer</Button>
                        </div>

                        {/* Trust Badges */}
                        <div className="flex gap-6 text-sm text-textSecondary border-t border-border pt-6">
                            <div className="flex items-center gap-2"><Shield className="w-4 h-4 text-green-500" /> Authenticity Guaranteed</div>
                            <div className="flex items-center gap-2"><Check className="w-4 h-4 text-green-500" /> Verified Artist</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default ArtworkDetails;
