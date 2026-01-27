import React, { useState } from 'react';
import { Filter, Search } from 'lucide-react';
import ProductCard from './components/ProductCard';
import FilterSidebar from './components/FilterSidebar';
import Button from '../../components/common/Button';
import Input from '../../components/common/Input';
import SEO from '../../components/common/SEO';

// Mock Data
const mockArtworks = Array.from({ length: 8 }).map((_, i) => ({
    id: i + 1,
    title: [
        'Echoes of Silence', 'Abstract Harmony', 'Golden Dunes',
        'Urban Rhythm', 'Serenity', 'Chaos & Calm', 'Vivid Dreams', 'Nightfall'
    ][i],
    artist: ['Ali Khan', 'Sara Ahmed', 'John Doe', 'Fatima Noor'][i % 4],
    price: `$${(i + 1) * 150}`,
    category: ['Visual Arts', 'Calligraphy', 'Digital Art', 'Sculptures'][i % 4],
    image: [
        'https://images.unsplash.com/photo-1579783902614-a3fb392796a5?auto=format&fit=crop&q=80&w=400',
        'https://images.unsplash.com/photo-1541963463532-d68292c34b19?auto=format&fit=crop&q=80&w=400',
        'https://images.unsplash.com/photo-1513364776144-60967b0f800f?auto=format&fit=crop&q=80&w=400',
        'https://images.unsplash.com/photo-1549887552-93f8efb4133f?auto=format&fit=crop&q=80&w=400',
        'https://images.unsplash.com/photo-1629196914168-3a9644338cf5?auto=format&fit=crop&q=80&w=400',
        'https://images.unsplash.com/photo-1547826039-bfc35e0f1ea8?auto=format&fit=crop&q=80&w=400',
        'https://images.unsplash.com/photo-1580136608260-4eb11f4b64fe?auto=format&fit=crop&q=80&w=400',
        'https://images.unsplash.com/photo-1552084117-5635e80c9e46?auto=format&fit=crop&q=80&w=400'
    ][i]
}));

const Marketplace = () => {
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);

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
                    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
                        {mockArtworks.map(artwork => (
                            <ProductCard key={artwork.id} artwork={artwork} />
                        ))}
                    </div>

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
