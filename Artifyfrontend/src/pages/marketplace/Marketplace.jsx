import React, { useState, useEffect } from 'react';
import { useSearchParams } from 'react-router-dom';
import { Filter, Search } from 'lucide-react';
import ProductCard from './components/ProductCard';
import FilterSidebar from './components/FilterSidebar';
import Button from '../../components/common/Button';
import Input from '../../components/common/Input';
import SEO from '../../components/common/SEO';

import marketplaceService from '../../services/marketplaceService';
import Loader from '../../components/common/Loader';

const Marketplace = () => {
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);
    const [artworks, setArtworks] = useState([]);
    const [loading, setLoading] = useState(true);
    const [searchTerm, setSearchTerm] = useState('');
    const [searchParams] = useSearchParams();
    const urlCategory = searchParams.get('category');

    useEffect(() => {
        const fetchArtworks = async () => {
            try {
                const data = await marketplaceService.getAllArtworks();
                setArtworks(data || []);
            } catch (error) {
                console.error("Failed to load artworks", error);
            } finally {
                setLoading(false);
            }
        };

        fetchArtworks();
    }, []);

    const filteredArtworks = artworks.filter(artwork => {
        const matchesSearch = artwork.title?.toLowerCase().includes(searchTerm.toLowerCase()) || 
                              artwork.artistName?.toLowerCase().includes(searchTerm.toLowerCase());
        const matchesCategory = urlCategory ? artwork.category === urlCategory : true;
        return matchesSearch && matchesCategory;
    });

    return (
        <div className="flex h-[calc(100vh-64px)] overflow-hidden bg-background">
            <SEO title="Marketplace" description="Explore thousands of original artworks. Filter by category, price, and artist." />
            {/* Sidebar - Desktop & Mobile */}
            <FilterSidebar
                isOpen={isSidebarOpen}
                onClose={() => setIsSidebarOpen(false)}
            />

            {/* Main Content */}
            <div className="flex-1 overflow-y-auto">
                <div className="p-4 md:p-8 max-w-7xl mx-auto">
                    {/* Header & Search */}
                    <div className="flex flex-col md:flex-row justify-between items-center gap-4 mb-8">
                        <div>
                            <h1 className="text-3xl font-heading font-bold text-primary">Marketplace</h1>
                            <p className="text-textSecondary text-sm">Explore unique artworks from around the world</p>
                        </div>

                        <div className="flex gap-2 w-full md:w-auto">
                            <div className="relative w-full md:w-80">
                                <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
                                <input
                                    type="text"
                                    placeholder="Search for art or artists..."
                                    className="w-full pl-10 pr-4 py-2 border border-border rounded-lg focus:outline-none focus:ring-2 focus:ring-secondary/20"
                                    value={searchTerm}
                                    onChange={(e) => setSearchTerm(e.target.value)}
                                />
                            </div>
                            <Button
                                variant="secondary"
                                className="md:hidden"
                                onClick={() => setIsSidebarOpen(true)}
                            >
                                <Filter className="w-5 h-5" />
                            </Button>
                        </div>
                    </div>

                    {/* Grid */}
                    {loading ? (
                        <div className="flex justify-center items-center py-20"><Loader /></div>
                    ) : (
                        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
                            {filteredArtworks.map(artwork => (
                                <ProductCard key={artwork.artworkId || artwork.id} artwork={artwork} />
                            ))}
                            {filteredArtworks.length === 0 && <p className="text-gray-500">No artworks found.</p>}
                        </div>
                    )}

                    {/* Load More */}
                    <div className="mt-12 flex justify-center">
                        <Button variant="ghost" size="lg">Load More Artworks</Button>
                    </div>

                    <div className="h-20"></div> {/* Spacer */}
                </div>
            </div>
        </div>
    );
};

export default Marketplace;
