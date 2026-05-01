import React, { useRef, useState, useEffect } from 'react';
import { motion, useScroll, useTransform } from 'framer-motion';
import { ArrowRight, Palette, Music, PenTool, Mic, Star, User, ChevronLeft, ChevronRight } from 'lucide-react';
import { Link } from 'react-router-dom';
import Button from '../components/common/Button';
import SEO from '../components/common/SEO';
import marketplaceService from '../services/marketplaceService';
import { useAuth } from '../context/AuthContext';

import { ART_CATEGORIES } from '../constants/categories';
import { getImageUrl } from '../utils/imageUtils';

const colors = [
    'bg-purple-100 text-purple-600',
    'bg-blue-100 text-blue-600',
    'bg-pink-100 text-pink-600',
    'bg-orange-100 text-orange-600',
    'bg-green-100 text-green-600',
    'bg-indigo-100 text-indigo-600',
    'bg-red-100 text-red-600',
];

const categoryData = ART_CATEGORIES.map((name, index) => ({
    id: index + 1,
    name,
    icon: index % 4 === 0 ? Palette : index % 4 === 1 ? PenTool : index % 4 === 2 ? Music : Mic,
    color: colors[index % colors.length]
}));

const Home = () => {
    const { user } = useAuth();
    const targetRef = useRef(null);
    const { scrollYProgress } = useScroll({
        target: targetRef,
        offset: ["start start", "end start"]
    });

    const categoryScrollRef = useRef(null);
    const scrollCategories = (direction) => {
        if (categoryScrollRef.current) {
            const { scrollLeft, clientWidth } = categoryScrollRef.current;
            const scrollAmount = clientWidth * 0.8;
            categoryScrollRef.current.scrollTo({
                left: direction === 'left' ? scrollLeft - scrollAmount : scrollLeft + scrollAmount,
                behavior: 'smooth'
            });
        }
    };

    const opacity = useTransform(scrollYProgress, [0, 0.5], [1, 0]);
    const scale = useTransform(scrollYProgress, [0, 0.5], [1, 0.8]);

    const [featuredArtists, setFeaturedArtists] = useState([]);
    const [trendingArtworks, setTrendingArtworks] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchHomeData = async () => {
            try {
                setLoading(true);
                const [artistsRes, artworksRes] = await Promise.all([
                    marketplaceService.getFeaturedArtists(),
                    marketplaceService.getTrendingArtworks()
                ]);
                setFeaturedArtists(artistsRes || []);
                // By default take top 6 to show if the API sends more
                setTrendingArtworks(artworksRes ? artworksRes.slice(0, 6) : []);
            } catch (error) {
                console.error("Failed to load home data:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchHomeData();
    }, []);

    // Placeholder skeleton for artworks while loading
    const SkeletonArtwork = () => (
        <div className="group relative rounded-xl overflow-hidden bg-gray-200 animate-pulse w-full">
            <div className="aspect-[4/5] bg-gray-300"></div>
            <div className="absolute inset-x-0 bottom-0 p-6 flex flex-col justify-end bg-gradient-to-t from-black/50 to-transparent h-1/2">
                <div className="h-4 bg-gray-400 rounded w-16 mb-2"></div>
                <div className="h-6 bg-gray-400 rounded w-3/4 mb-2"></div>
                <div className="h-4 bg-gray-400 rounded w-1/2"></div>
            </div>
        </div>
    );

    return (
        <div className="flex flex-col">
            <SEO
                title="Home"
                description="artifi is the premier marketplace for original art. Discover unique paintings, sculptures, and digital art from top artists."
            />
            {/* Hero Section */}
            <section ref={targetRef} className="relative h-[95vh] flex items-center justify-center overflow-hidden bg-[#020617] text-white">
                {/* 1. Enhanced High-Contrast Background */}
                <div className="absolute inset-0 z-0">
                    {/* Blob 1: Vibrant Indigo */}
                    <motion.div
                        animate={{
                            scale: [1, 1.2, 1],
                            x: [0, 50, 0],
                            y: [0, 30, 0],
                        }}
                        transition={{ duration: 15, repeat: Infinity, ease: "easeInOut" }}
                        className="absolute top-[-10%] left-[-5%] w-[500px] h-[500px] bg-indigo-600/30 rounded-full blur-[100px]"
                    />

                    {/* Blob 2: Vibrant Pink/Secondary */}
                    <motion.div
                        animate={{
                            scale: [1.2, 1, 1.2],
                            x: [0, -50, 0],
                            y: [0, -30, 0],
                        }}
                        transition={{ duration: 18, repeat: Infinity, ease: "easeInOut" }}
                        className="absolute bottom-[5%] right-[-5%] w-[600px] h-[600px] bg-pink-500/20 rounded-full blur-[120px]"
                    />
                    {/* Subtle Grid for Depth (Optional but keeps it from looking flat) */}
                    <div className="absolute inset-0 bg-[url('https://grainy-gradients.vercel.app/noise.svg')] opacity-20 brightness-100 contrast-150 pointer-events-none"></div>
                </div>

                {/* 2. Text Content with "Readability Shield" */}
                <div className="relative z-10 text-center px-6 max-w-5xl mx-auto">
                    {/* This div acts as a dark glow behind the text to pop it out from blobs */}
                    <div className="absolute inset-0 -m-20 bg-[#020617]/40 blur-3xl -z-10 rounded-full" />

                    <motion.div
                        style={{ opacity, scale }}
                        initial={{ opacity: 0, y: 20 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ duration: 0.8 }}
                    >
                        {/* Pill Badge */}
                        <div className="inline-flex items-center gap-2 px-4 py-2 rounded-full bg-white/10 border border-white/20 backdrop-blur-xl mb-8">
                            <span className="flex h-2 w-2 rounded-full bg-emerald-400 animate-pulse" />
                            <span className="text-xs font-bold tracking-widest uppercase text-white">
                                The Premier Marketplace for Creativity
                            </span>
                        </div>

                        {/* Typography - Added font-black and white-shadow */}
                        <h1 className="text-6xl md:text-8xl font-black mb-8 tracking-tighter leading-none text-white drop-shadow-2xl">
                            artifi Your <br />
                            <span className="bg-clip-text text-transparent bg-gradient-to-r from-indigo-400 via-white to-pink-400">
                                Digital Horizon
                            </span>
                        </h1>

                        <p className="text-lg md:text-2xl text-gray-200 mb-12 max-w-2xl mx-auto leading-relaxed font-medium">
                            Connect with world-class creators.
                            Commission original pieces or trade digital masterpieces securely.
                        </p>

                        {/* CTA Group */}
                        <div className="flex flex-col sm:flex-row gap-6 justify-center items-center">
                            <Link to="/marketplace">
                                <Button
                                    size="xl"
                                    className="px-10 py-7 bg-white text-black hover:bg-gray-200 rounded-2xl font-bold text-lg shadow-[0_0_30px_rgba(255,255,255,0.2)] transition-all"
                                >
                                    Explore Now
                                </Button>
                            </Link>

                            {(!user || (user.role !== 'Artist' && user.role !== 'Agency')) && (
                                <Link to="/register">
                                    <button className="px-10 py-3 text-white font-semibold border-b-2 border-white/30 hover:border-white transition-all">
                                        Join as Artist
                                    </button>
                                </Link>
                            )}
                        </div>
                    </motion.div>
                </div>
            </section>
            {/* Featured Artists Badges Row */}
            <section className="py-12 bg-white border-b border-gray-100">
                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                    <h3 className="text-center text-sm font-bold text-gray-400 uppercase tracking-wider mb-8">
                        Recognized Artists
                    </h3>

                    {loading ? (
                        <div className="flex flex-wrap justify-center gap-8 md:gap-12">
                            {[1, 2, 3, 4, 5].map(i => (
                                <div key={i} className="flex flex-col items-center animate-pulse">
                                    <div className="w-20 h-20 md:w-24 md:h-24 rounded-full bg-gray-200 mb-3"></div>
                                    <div className="w-16 h-4 bg-gray-200 rounded"></div>
                                </div>
                            ))}
                        </div>
                    ) : (
                        <div className="flex flex-wrap justify-center gap-8 md:gap-12">
                            {featuredArtists.length > 0 ? featuredArtists.map((artist, index) => (
                                <Link key={artist.artistProfileId || index} to={`/artist/${artist.artistProfileId || artist.id}`} className="group flex flex-col items-center">
                                    <motion.div
                                        initial={{ opacity: 0, scale: 0.8 }}
                                        whileInView={{ opacity: 1, scale: 1 }}
                                        viewport={{ once: true }}
                                        transition={{ delay: index * 0.1 }}
                                        className="w-20 h-20 md:w-24 md:h-24 rounded-full overflow-hidden mb-3 border-4 border-transparent group-hover:border-accent transition-all duration-300 shadow-md group-hover:shadow-xl group-hover:scale-105"
                                    >
                                        {artist.profileImageUrl ? (
                                            <img src={getImageUrl(artist.profileImageUrl)} alt={artist.fullName} className="w-full h-full object-cover" />
                                        ) : (
                                            <div className="w-full h-full bg-gray-100 flex items-center justify-center text-gray-400 group-hover:text-accent group-hover:bg-accent/10 transition-colors">
                                                <User size={36} />
                                            </div>
                                        )}
                                    </motion.div>
                                    <span className="text-sm font-medium text-primary group-hover:text-accent transition-colors text-center line-clamp-1 max-w-[100px]">
                                        {artist.fullName}
                                    </span>
                                </Link>
                            )) : (
                                <p className="text-gray-500 italic">No featured artists found.</p>
                            )}
                        </div>
                    )}
                </div>
            </section>

            {/* Categories Section */}
            <section className="py-20 bg-background relative overflow-hidden">
                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 relative">
                    <div className="text-center mb-16">
                        <h2 className="text-3xl md:text-4xl font-heading font-bold text-primary mb-4">Browse by Category</h2>
                        <p className="text-textSecondary">Find the perfect artist for your specific needs</p>
                    </div>

                    <div className="relative group">
                        {/* Navigation Buttons */}
                        <button
                            onClick={() => scrollCategories('left')}
                            className="absolute -left-4 md:-left-12 top-1/2 -translate-y-1/2 z-20 bg-white/80 hover:bg-white p-4 rounded-full shadow-xl border border-gray-100 text-primary transition-all hover:scale-110 md:opacity-0 group-hover:opacity-100"
                        >
                            <ChevronLeft className="w-6 h-6" />
                        </button>

                        <div
                            ref={categoryScrollRef}
                            className="flex overflow-x-auto gap-8 pb-8 no-scrollbar snap-x scroll-smooth"
                        >
                            {categoryData.map((cat, index) => (
                                <Link to={`/marketplace?category=${encodeURIComponent(cat.name)}`} key={cat.id} className="min-w-[280px] snap-center">
                                    <motion.div
                                        initial={{ opacity: 0, y: 20 }}
                                        whileInView={{ opacity: 1, y: 0 }}
                                        viewport={{ once: true }}
                                        transition={{ delay: index * 0.05 }}
                                        className="bg-white p-8 rounded-2xl shadow-sm hover:shadow-xl transition-all duration-300 border border-border group/card cursor-pointer h-full"
                                    >
                                        <div className={`w-14 h-14 rounded-xl ${cat.color} flex items-center justify-center mb-6 group-hover/card:scale-110 transition-transform`}>
                                            <cat.icon className="w-7 h-7" />
                                        </div>
                                        <h3 className="text-xl font-bold mb-2 group-hover/card:text-secondary transition-colors">{cat.name}</h3>
                                        <div className="flex items-center text-secondary font-medium text-sm mt-4">
                                            Explore <ArrowRight className="w-4 h-4 ml-1 group-hover/card:translate-x-1 transition-transform" />
                                        </div>
                                    </motion.div>
                                </Link>
                            ))}
                        </div>

                        <button
                            onClick={() => scrollCategories('right')}
                            className="absolute -right-4 md:-right-12 top-1/2 -translate-y-1/2 z-20 bg-white/80 hover:bg-white p-4 rounded-full shadow-xl border border-gray-100 text-primary transition-all hover:scale-110 md:opacity-0 group-hover:opacity-100"
                        >
                            <ChevronRight className="w-6 h-6" />
                        </button>
                    </div>
                </div>
            </section>

            {/* Trending Artworks Section */}
            <section className="py-20 bg-white">
                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                    <div className="flex justify-between items-end mb-12">
                        <div>
                            <h2 className="text-3xl md:text-4xl font-heading font-bold text-primary mb-4">Trending Masterpieces</h2>
                            <p className="text-textSecondary">Dynamic, top-rated selections currently trending</p>
                        </div>
                        <Link to="/marketplace" className="hidden md:flex items-center text-primary font-medium hover:text-secondary transition-colors">
                            View All <ArrowRight className="w-4 h-4 ml-2" />
                        </Link>
                    </div>

                    <div className="grid grid-cols-1 md:grid-cols-3 lg:grid-cols-3 gap-8">
                        {loading ? (
                            <>
                                <SkeletonArtwork />
                                <SkeletonArtwork />
                                <SkeletonArtwork />
                            </>
                        ) : trendingArtworks.length > 0 ? (
                            trendingArtworks.map((art, index) => (
                                <motion.div
                                    key={art.artworkId || index}
                                    initial={{ opacity: 0, scale: 0.95 }}
                                    whileInView={{ opacity: 1, scale: 1 }}
                                    viewport={{ once: true }}
                                    transition={{ delay: index * 0.1 }}
                                    className="group relative rounded-xl overflow-hidden bg-gray-100 shadow-sm hover:shadow-xl transition-all duration-500"
                                >
                                    <div className="aspect-[4/5] overflow-hidden">
                                        <img
                                            src={getImageUrl(art.imageUrl) || 'https://via.placeholder.com/800'}
                                            alt={art.title}
                                            className="w-full h-full object-cover transform group-hover:scale-110 transition-transform duration-700"
                                        />
                                    </div>
                                    <div className="absolute inset-0 bg-gradient-to-t from-black/90 via-black/30 to-transparent opacity-80 md:opacity-0 group-hover:opacity-100 transition-opacity duration-300 p-6 flex flex-col justify-end">
                                        <div className="flex justify-between items-center mb-1">
                                            <span className="text-accent text-sm font-bold bg-accent/20 px-2 py-0.5 rounded backdrop-blur-md border border-accent/50">
                                                PKR {art.price}
                                            </span>
                                            {art.category && (
                                                <span className="text-white/80 text-xs px-2 py-1 bg-white/10 rounded-full backdrop-blur-sm">
                                                    {art.category}
                                                </span>
                                            )}
                                        </div>
                                        <h3 className="text-white text-xl font-bold mb-1 line-clamp-1">{art.title}</h3>
                                        <Link to={`/artist/${art.artistProfileId || art.artistId}`} className="text-gray-300 text-sm hover:text-white transition-colors cursor-pointer inline-block z-20 relative">
                                            by {art.artistName}
                                        </Link>
                                        <div className="mt-4 flex gap-2">
                                            <Link to={`/artwork/${art.artworkId}`} className="w-full z-20 relative">
                                                <Button variant="accent" size="sm" className="w-full shadow-lg shadow-accent/20">View Details</Button>
                                            </Link>
                                        </div>
                                    </div>
                                </motion.div>
                            ))
                        ) : (
                            <div className="col-span-1 md:col-span-3 text-center py-12 text-gray-500">
                                No trending artworks available at the moment.
                            </div>
                        )}
                    </div>

                    <div className="mt-8 md:hidden text-center">
                        <Link to="/marketplace">
                            <Button variant="secondary" className="w-full">View All</Button>
                        </Link>
                    </div>
                </div>
            </section>

            {/* Trust/CTA Section */}
            <section className="py-20 bg-primary text-white overflow-hidden relative">
                <div className="absolute top-0 right-0 p-20 bg-secondary/10 rounded-full blur-3xl transform translate-x-1/2 -translate-y-1/2"></div>
                <div className="absolute bottom-0 left-0 p-20 bg-accent/10 rounded-full blur-3xl transform -translate-x-1/2 translate-y-1/2"></div>

                <div className="max-w-4xl mx-auto px-4 text-center relative z-10">
                    <h2 className="text-3xl md:text-5xl font-heading font-bold mb-6">artifi Protects Your Creativity</h2>
                    <p className="text-gray-300 text-lg mb-10 max-w-2xl mx-auto">
                        We ensure every transaction is secure and every artist gets paid fairly.
                        Our plagiarism detection systems protect authentic work.
                    </p>
                    <div className="grid grid-cols-1 md:grid-cols-3 gap-8 mb-12 text-left bg-white/5 p-8 rounded-2xl border border-white/10">
                        <div className="flex gap-4">
                            <div className="bg-secondary/20 p-3 rounded-lg h-fit"><Star className="w-6 h-6 text-secondary" /></div>
                            <div>
                                <h4 className="font-bold text-lg mb-1">Authenticity</h4>
                                <p className="text-sm text-gray-400">Verified profiles and portfolio reviews.</p>
                            </div>
                        </div>
                        <div className="flex gap-4">
                            <div className="bg-accent/20 p-3 rounded-lg h-fit"><Palette className="w-6 h-6 text-accent" /></div>
                            <div>
                                <h4 className="font-bold text-lg mb-1">Secure Pay</h4>
                                <p className="text-sm text-gray-400">Payments held in escrow until delivery.</p>
                            </div>
                        </div>
                        <div className="flex gap-4">
                            <div className="bg-green-500/20 p-3 rounded-lg h-fit"><PenTool className="w-6 h-6 text-green-400" /></div>
                            <div>
                                <h4 className="font-bold text-lg mb-1">Global Reach</h4>
                                <p className="text-sm text-gray-400">Sell to collectors worldwide.</p>
                            </div>
                        </div>
                    </div>
                    {(!user || (user.role !== 'Artist' && user.role !== 'Agency')) && (
                        <Link to="/register">
                            <Button variant="accent" size="lg">Start Your Journey</Button>
                        </Link>
                    )}
                </div>
            </section>
        </div>
    );
};

export default Home;
