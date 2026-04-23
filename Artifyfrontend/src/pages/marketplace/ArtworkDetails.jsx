import React, { useState, useEffect } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import { ArrowLeft, Share2, Heart, Shield, Check } from 'lucide-react';
import Button from '../../components/common/Button';
import { useCart } from '../../context/CartContext';
import { useAuth } from '../../context/AuthContext';
import { chatService } from '../../services/chatService';
import SEO from '../../components/common/SEO';
import marketplaceService from '../../services/marketplaceService';
import Loader from '../../components/common/Loader';
import { getImageUrl } from '../../utils/imageUtils';

const ArtworkDetails = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const { addToCart } = useCart();
    const { user } = useAuth();
    const [artwork, setArtwork] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchArtwork = async () => {
            try {
                setLoading(true);
                const data = await marketplaceService.getArtworkById(id);
                setArtwork(data);
            } catch (err) {
                console.error("Failed to fetch artwork details:", err);
                setError("Artwork not found.");
            } finally {
                setLoading(false);
            }
        };

        if (id) {
            fetchArtwork();
        }
    }, [id]);



    const handleAddToCart = () => {
        // Map artworkId to id for CartContext compatibility
        addToCart({
            ...artwork,
            id: artwork.artworkId || artwork.id
        });
        alert("Artwork added to cart!");
    };

    if (loading) return <div className="min-h-screen flex justify-center items-center"><Loader /></div>;
    
    if (error || !artwork) {
        return (
            <div className="min-h-screen flex flex-col items-center justify-center bg-gray-50">
                <h2 className="text-2xl font-bold text-gray-800 mb-4">{error || "Artwork not found"}</h2>
                <Link to="/marketplace">
                    <Button variant="primary">Return to Marketplace</Button>
                </Link>
            </div>
        );
    }

    return (
        <div className="bg-white min-h-screen py-8">
            <SEO title={artwork.title} description={artwork.description} />
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                <Link to="/marketplace" className="inline-flex items-center text-textSecondary hover:text-primary mb-6 transition-colors">
                    <ArrowLeft className="w-4 h-4 mr-2" /> Back to Marketplace
                </Link>

                <div className="grid grid-cols-1 lg:grid-cols-2 gap-12">
                    {/* Image Section */}
                    <div className="bg-gray-50 rounded-2xl overflow-hidden border border-border flex items-center justify-center p-4">
                        <img src={getImageUrl(artwork.imageUrl || artwork.image)} alt={artwork.title} className="w-full h-full object-contain max-h-[600px]" />
                    </div>

                    {/* Details Section */}
                    <div>
                        <div className="flex justify-between items-start mb-4">
                            <div>
                                <h1 className="text-3xl md:text-4xl font-heading font-bold text-primary mb-2">{artwork.title}</h1>
                                <Link to={`/artist/${artwork.artistProfileId || artwork.artistId}`} className="text-lg text-secondary font-medium hover:underline">
                                    by {artwork.artistName || artwork.artist}
                                </Link>
                            </div>
                            <div className="flex gap-2">
                                <button className="p-2 rounded-full hover:bg-gray-100 text-textSecondary"><Share2 className="w-5 h-5" /></button>
                                <button className="p-2 rounded-full hover:bg-gray-100 text-textSecondary"><Heart className="w-5 h-5" /></button>
                            </div>
                        </div>

                        <div className="text-3xl font-bold text-primary mb-8">
                            {typeof artwork.price === 'number' ? `PKR ${artwork.price.toLocaleString()}` : typeof artwork.price === 'string' ? artwork.price.replace('$', 'PKR ') : artwork.price}
                        </div>

                        <div className="bg-background rounded-xl p-6 mb-8 border border-border">
                            <h3 className="font-bold text-primary mb-4">Artwork Details</h3>
                            <div className="grid grid-cols-2 gap-y-4 text-sm">
                                <div className="text-textSecondary">Category</div>
                                <div className="font-medium text-primary text-right">{artwork.category || artwork.medium || 'N/A'}</div>

                                <div className="text-textSecondary">Dimensions</div>
                                <div className="font-medium text-primary text-right">{artwork.dimensions || 'Variable'}</div>

                                <div className="text-textSecondary">Year</div>
                                <div className="font-medium text-primary text-right">{artwork.year || new Date(artwork.createdAt).getFullYear() || 'N/A'}</div>
                            </div>
                        </div>

                        <p className="text-textSecondary leading-relaxed mb-8">{artwork.description}</p>

                        <div className="flex flex-col gap-4 mb-8">
                            <Button
                                variant="primary"
                                size="lg"
                                className="w-full"
                                onClick={handleAddToCart}
                            >
                                Add to Cart
                            </Button>

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
