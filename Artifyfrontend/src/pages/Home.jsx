import React, { useRef } from 'react';
import { motion, useScroll, useTransform } from 'framer-motion';
import { ArrowRight, Palette, Music, PenTool, Mic, Star } from 'lucide-react';
import { Link } from 'react-router-dom';
import Button from '../components/common/Button';
import SEO from '../components/common/SEO';

// Mock Data for Categories
const categories = [
    { id: 1, name: 'Visual Arts', icon: Palette, color: 'bg-purple-100 text-purple-600', desc: 'Paintings, Sketches, Digital Art' },
    { id: 2, name: 'Calligraphy', icon: PenTool, color: 'bg-blue-100 text-blue-600', desc: 'Traditional & Modern Scripts' },
    { id: 3, name: 'Music & Audio', icon: Music, color: 'bg-pink-100 text-pink-600', desc: 'Original Compositions & Scores' },
    { id: 4, name: 'Performance', icon: Mic, color: 'bg-orange-100 text-orange-600', desc: 'Live Acts, spoken word' },
];

// Mock Data for Featured Artworks
const featuredArtworks = [
    {
        id: 1,
        title: 'Ethereal Dreams',
        artist: 'Sara Khan',
        price: '$450',
        image: 'https://images.unsplash.com/photo-1579783902614-a3fb392796a5?auto=format&fit=crop&q=80&w=800'
    },
    {
        id: 2,
        title: 'Midnight Symphony',
        artist: 'Ali Raza',
        price: '$320',
        image: 'https://images.unsplash.com/photo-1517524285303-d7543f547574?auto=format&fit=crop&q=80&w=800'
    },
    {
        id: 3,
        title: 'Golden Hour',
        artist: 'Maria Ahmed',
        price: '$800',
        image: 'https://images.unsplash.com/photo-1541963463532-d68292c34b19?auto=format&fit=crop&q=80&w=800'
    },
];

const Home = () => {
    const targetRef = useRef(null);
    const { scrollYProgress } = useScroll({
        target: targetRef,
        offset: ["start start", "end start"]
    });

    const opacity = useTransform(scrollYProgress, [0, 0.5], [1, 0]);
    const scale = useTransform(scrollYProgress, [0, 0.5], [1, 0.8]);

    return (
        <div className="flex flex-col">
            <SEO
                title="Home"
                description="Artify is the premier marketplace for original art. Discover unique paintings, sculptures, and digital art from top artists."
            />
            {/* Hero Section */}
            <section ref={targetRef} className="relative h-[90vh] flex items-center justify-center overflow-hidden bg-primary text-white">
                <div className="absolute inset-0 bg-[url('https://images.unsplash.com/photo-1513364776144-60967b0f800f?auto=format&fit=crop&q=80&w=1920')] bg-cover bg-center opacity-20"></div>
                <div className="absolute inset-0 bg-gradient-to-t from-primary via-transparent to-transparent"></div>

                <motion.div
                    style={{ opacity, scale }}
                    className="relative z-10 text-center px-4 max-w-4xl mx-auto"
                >
                    <motion.span
                        initial={{ opacity: 0, y: 20 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ delay: 0.2 }}
                        className="inline-block px-4 py-1.5 rounded-full bg-accent/20 text-accent border border-accent/30 text-sm font-medium mb-6 backdrop-blur-sm"
                    >
                        The Premier Marketplace for Creativity
                    </motion.span>
                    <motion.h1
                        initial={{ opacity: 0, y: 30 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ delay: 0.4 }}
                        className="text-5xl md:text-7xl font-heading font-bold mb-6 leading-tight"
                    >
                        Discover Art That <br /> <span className="text-secondary">Moves the Soul</span>
                    </motion.h1>
                    <motion.p
                        initial={{ opacity: 0, y: 30 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ delay: 0.6 }}
                        className="text-lg md:text-xl text-gray-300 mb-10 max-w-2xl mx-auto"
                    >
                        Connect directly with world-class painters, musicians, and performers.
                        Commission unique works or buy original pieces securely.
                    </motion.p>
                    <motion.div
                        initial={{ opacity: 0, y: 30 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ delay: 0.8 }}
                        className="flex flex-col sm:flex-row gap-4 justify-center"
                    >
                        <Link to="/marketplace">
                            <Button variant="accent" size="lg" className="w-full sm:w-auto">Explore Marketplace</Button>
                        </Link>
                        <Link to="/register">
                            <Button variant="secondary" size="lg" className="w-full sm:w-auto border-white text-white hover:bg-white hover:text-primary">Join as Artist</Button>
                        </Link>
                    </motion.div>
                </motion.div>
            </section>

            {/* Categories Section */}
            <section className="py-20 bg-background">
                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                    <div className="text-center mb-16">
                        <h2 className="text-3xl md:text-4xl font-heading font-bold text-primary mb-4">Browse by Category</h2>
                        <p className="text-textSecondary">Find the perfect artist for your specific needs</p>
                    </div>

                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
                        {categories.map((cat, index) => (
                            <motion.div
                                key={cat.id}
                                initial={{ opacity: 0, y: 20 }}
                                whileInView={{ opacity: 1, y: 0 }}
                                viewport={{ once: true }}
                                transition={{ delay: index * 0.1 }}
                                className="bg-white p-8 rounded-2xl shadow-sm hover:shadow-xl transition-all duration-300 border border-border group cursor-pointer"
                            >
                                <div className={`w-14 h-14 rounded-xl ${cat.color} flex items-center justify-center mb-6 group-hover:scale-110 transition-transform`}>
                                    <cat.icon className="w-7 h-7" />
                                </div>
                                <h3 className="text-xl font-bold mb-2 group-hover:text-secondary transition-colors">{cat.name}</h3>
                                <p className="text-textSecondary text-sm mb-4">{cat.desc}</p>
                                <div className="flex items-center text-secondary font-medium text-sm">
                                    Explore <ArrowRight className="w-4 h-4 ml-1 group-hover:translate-x-1 transition-transform" />
                                </div>
                            </motion.div>
                        ))}
                    </div>
                </div>
            </section>

            {/* Featured Artworks Section */}
            <section className="py-20 bg-white">
                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                    <div className="flex justify-between items-end mb-12">
                        <div>
                            <h2 className="text-3xl md:text-4xl font-heading font-bold text-primary mb-4">Featured Masterpieces</h2>
                            <p className="text-textSecondary">Hand-picked selections from our top artists</p>
                        </div>
                        <Link to="/marketplace" className="hidden md:flex items-center text-primary font-medium hover:text-secondary transition-colors">
                            View All <ArrowRight className="w-4 h-4 ml-2" />
                        </Link>
                    </div>

                    <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
                        {featuredArtworks.map((art, index) => (
                            <motion.div
                                key={art.id}
                                initial={{ opacity: 0, scale: 0.95 }}
                                whileInView={{ opacity: 1, scale: 1 }}
                                viewport={{ once: true }}
                                transition={{ delay: index * 0.2 }}
                                className="group relative rounded-xl overflow-hidden bg-gray-100"
                            >
                                <div className="aspect-[4/5] overflow-hidden">
                                    <img
                                        src={art.image}
                                        alt={art.title}
                                        className="w-full h-full object-cover transform group-hover:scale-110 transition-transform duration-700"
                                    />
                                </div>
                                <div className="absolute inset-0 bg-gradient-to-t from-black/80 via-black/20 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300 p-6 flex flex-col justify-end">
                                    <span className="text-accent text-sm font-medium mb-1">{art.price}</span>
                                    <h3 className="text-white text-xl font-bold mb-1">{art.title}</h3>
                                    <p className="text-gray-300 text-sm">by {art.artist}</p>
                                    <div className="mt-4 flex gap-2">
                                        <Button variant="accent" size="sm" className="w-full">View Details</Button>
                                    </div>
                                </div>
                            </motion.div>
                        ))}
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
                    <h2 className="text-3xl md:text-5xl font-heading font-bold mb-6">Artify Protects Your Creativity</h2>
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
                    <Button variant="accent" size="lg">Start Your Journey</Button>
                </div>
            </section>
        </div>
    );
};

export default Home;
