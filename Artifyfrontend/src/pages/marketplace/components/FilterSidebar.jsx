import React, { useState } from 'react';
import { Filter, X } from 'lucide-react';
import Button from '../../../components/common/Button';

const FilterSidebar = ({ isOpen, onClose }) => {
    const [priceRange, setPriceRange] = useState(1000);

    return (
        <aside className={`fixed inset-y-0 left-0 z-50 w-64 bg-white shadow-2xl transform transition-transform duration-300 ease-in-out md:relative md:translate-x-0 md:shadow-none md:border-r md:border-border ${isOpen ? 'translate-x-0' : '-translate-x-full'}`}>
            <div className="p-6 h-full overflow-y-auto">
                <div className="flex justify-between items-center mb-6 md:hidden">
                    <h2 className="font-heading font-bold text-xl">Filters</h2>
                    <button onClick={onClose}><X className="w-6 h-6" /></button>
                </div>

                {/* Categories */}
                <div className="mb-8">
                    <h3 className="font-bold text-sm uppercase tracking-wider text-gray-500 mb-4">Categories</h3>
                    <div className="space-y-2">
                        {['All', 'Visual Arts', 'Calligraphy', 'Music', 'Sculptures', 'Digital Art'].map(cat => (
                            <label key={cat} className="flex items-center gap-2 cursor-pointer group">
                                <input type="checkbox" className="rounded border-gray-300 text-secondary focus:ring-secondary" />
                                <span className="text-textSecondary group-hover:text-primary transition-colors">{cat}</span>
                            </label>
                        ))}
                    </div>
                </div>

                {/* Price Range */}
                <div className="mb-8">
                    <h3 className="font-bold text-sm uppercase tracking-wider text-gray-500 mb-4">Price Range</h3>
                    <input
                        type="range"
                        min="0"
                        max="5000"
                        value={priceRange}
                        onChange={(e) => setPriceRange(e.target.value)}
                        className="w-full accent-secondary"
                    />
                    <div className="flex justify-between text-sm text-textSecondary mt-2">
                        <span>$0</span>
                        <span>${priceRange}</span>
                    </div>
                </div>

                {/* Ratings */}
                <div className="mb-8">
                    <h3 className="font-bold text-sm uppercase tracking-wider text-gray-500 mb-4">Artist Rating</h3>
                    <div className="space-y-2">
                        {[4, 3, 2].map(rating => (
                            <label key={rating} className="flex items-center gap-2 cursor-pointer">
                                <input type="radio" name="rating" className="text-secondary focus:ring-secondary" />
                                <span className="text-textSecondary text-sm">{rating}+ Stars</span>
                            </label>
                        ))}
                    </div>
                </div>

                <Button variant="primary" className="w-full">Apply Filters</Button>
            </div>
        </aside>
    );
};

export default FilterSidebar;
