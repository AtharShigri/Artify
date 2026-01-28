import React from 'react';
import { motion } from 'framer-motion';
import { Link } from 'react-router-dom';
import Button from '../../../components/common/Button';
import { ShoppingBag } from 'lucide-react';
import { useCart } from '../../../context/CartContext';

const ProductCard = ({ artwork }) => {
    const { addToCart } = useCart();

    return (
        <motion.div
            initial={{ opacity: 0, scale: 0.9 }}
            whileInView={{ opacity: 1, scale: 1 }}
            viewport={{ once: true }}
            whileHover={{ y: -5 }}
            className="bg-white rounded-xl overflow-hidden shadow-sm hover:shadow-xl transition-all duration-300 border border-border group"
        >
            <div className="aspect-[4/5] relative overflow-hidden bg-gray-100">
                <Link to={`/artwork/${artwork.id}`}>
                    <img
                        src={artwork.image}
                        alt={artwork.title}
                        className="w-full h-full object-cover transition-transform duration-500 group-hover:scale-110"
                    />
                </Link>
                <div className="absolute top-3 right-3 opacity-0 group-hover:opacity-100 transition-opacity">
                    <Button
                        variant="accent"
                        size="sm"
                        className="rounded-full w-10 h-10 p-0 flex items-center justify-center"
                        onClick={(e) => {
                            e.preventDefault();
                            addToCart(artwork);
                        }}
                    >
                        <ShoppingBag className="w-4 h-4" />
                    </Button>
                </div>
            </div>
            <div className="p-4">
                <div className="flex justify-between items-start mb-2">
                    <Link to={`/artwork/${artwork.id}`} className="font-bold text-lg text-primary truncate pr-2 hover:text-secondary block flex-1">
                        {artwork.title}
                    </Link>
                    <span className="font-heading font-bold text-secondary">{artwork.price}</span>
                </div>
                <p className="text-sm text-textSecondary mb-3">by {artwork.artist}</p>
                <div className="flex gap-2">
                    <span className="px-2 py-1 bg-gray-100 text-xs rounded-md text-gray-600">{artwork.category}</span>
                </div>
            </div>
        </motion.div>
    );
};

export default ProductCard;
