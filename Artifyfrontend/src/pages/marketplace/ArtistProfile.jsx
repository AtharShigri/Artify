import React, { useState, useEffect } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import { MapPin, Star, Share2, MessageCircle, MoreHorizontal, Loader2, Palette, Building2, User } from 'lucide-react';
import Button from '../../components/common/Button';
import ProductCard from './components/ProductCard';
import SEO from '../../components/common/SEO';
import marketplaceService from '../../services/marketplaceService';
import reviewService from '../../services/reviewService';
import { chatService } from '../../services/chatService';
import { getImageUrl } from '../../utils/imageUtils';
import { useAuth } from '../../context/AuthContext';

const ArtistProfile = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const { user, isBuyer, isArtist, isAgency } = useAuth();
    const [activeTab, setActiveTab] = useState('artworks');
    const [artistData, setArtistData] = useState(null);
    const [reviews, setReviews] = useState([]);
    const [loading, setLoading] = useState(true);
    const [submittingReview, setSubmittingReview] = useState(false);
    const [reviewForm, setReviewForm] = useState({ rating: 5, comment: '' });
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchProfileData = async () => {
            try {
                setLoading(true);
                const [profile, profileReviews] = await Promise.all([
                    marketplaceService.getArtistProfile(id),
                    reviewService.getArtistReviews(id)
                ]);
                setArtistData(profile);
                setReviews(profileReviews);
            } catch (err) {
                console.error("Failed to load profile:", err);
                setError("Artist not found or an error occurred.");
            } finally {
                setLoading(false);
            }
        };
        if (id) fetchProfileData();
    }, [id]);

    const handleCollaborate = async () => {
        if (!user) {
            navigate('/login');
            return;
        }
        try {
            const conversation = await chatService.startConversation(id);
            navigate('/dashboard/chat', { state: { activeConversation: conversation } });
        } catch (err) {
            alert(err.response?.data?.message || "Failed to start conversation");
        }
    };

    const handleReviewSubmit = async (e) => {
        e.preventDefault();
        if (!user) return;
        setSubmittingReview(true);
        try {
            const newReview = await reviewService.createReview({
                ...reviewForm,
                artistProfileId: id
            });
            setReviews([newReview, ...reviews]);
            setReviewForm({ rating: 5, comment: '' });
            // Update average locally
            const newTotal = (artistData.totalReviews || 0) + 1;
            const newRating = ((artistData.rating || 0) * (artistData.totalReviews || 0) + reviewForm.rating) / newTotal;
            setArtistData({ ...artistData, rating: newRating, totalReviews: newTotal });
        } catch (err) {
            alert(err.response?.data?.message || "Failed to submit review");
        } finally {
            setSubmittingReview(false);
        }
    };

    if (loading) {
        return (
            <div className="flex flex-col justify-center items-center h-screen bg-background text-primary">
                <Loader2 className="w-12 h-12 animate-spin mb-4 text-accent" />
                <p className="text-xl font-medium text-textSecondary">Syncing profile...</p>
            </div>
        );
    }

    if (error || !artistData) {
        return (
             <div className="flex flex-col justify-center items-center h-[70vh] bg-background">
                <h2 className="text-3xl font-heading font-black text-primary mb-4">Profile Unavailable</h2>
                <p className="text-textSecondary mb-8">{error || "The requested profile could not be found."}</p>
                <Link to="/marketplace">
                    <Button variant="primary">Return to Marketplace</Button>
                </Link>
            </div>
        );
    }

    // Use a default cover image gradient
    const isAgencyProfile = artistData.userType === 1;
    const coverGradient = isAgencyProfile ? "bg-gradient-to-r from-primary via-indigo-900 to-primary" : "bg-gradient-to-r from-slate-900 via-secondary/20 to-slate-900";

    return (
        <div className="bg-background min-h-screen pb-20">
            <SEO title={`${artistData.fullName} - ${isAgencyProfile ? 'Agency' : 'Artist'} Profile`} description={artistData.bio} />
            
            <div className={`h-72 md:h-96 w-full ${coverGradient} relative overflow-hidden`}>
                 <div className="absolute inset-0 opacity-10 bg-[url('https://www.transparenttextures.com/patterns/cubes.png')]"></div>
                 <div className="absolute inset-0 bg-gradient-to-t from-background to-transparent"></div>
            </div>

            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 -mt-32 relative z-10">
                <div className="bg-white rounded-[3rem] shadow-2xl shadow-primary/5 p-8 md:p-12 border border-gray-100 mb-8 overflow-hidden relative">
                    {/* Background decoration */}
                    <div className="absolute top-0 right-0 w-64 h-64 bg-secondary/5 rounded-full -mr-32 -mt-32 blur-3xl" />
                    
                    <div className="flex flex-col md:flex-row gap-10 items-start relative z-10">
                        <div className="w-40 h-40 md:w-48 md:h-48 rounded-[2.5rem] border-8 border-white overflow-hidden shadow-2xl flex-shrink-0 bg-gray-50 flex items-center justify-center -mt-16 md:-mt-24 ring-1 ring-gray-100">
                            {artistData.profileImageUrl ? (
                                <img src={getImageUrl(artistData.profileImageUrl)} alt={artistData.fullName} className="w-full h-full object-cover" />
                            ) : (
                                <span className="text-6xl font-black text-primary/20">{artistData.fullName?.charAt(0)}</span>
                            )}
                        </div>

                        <div className="flex-1 pt-2 w-full">
                            <div className="flex flex-col md:flex-row md:items-center md:justify-between mb-6 gap-6">
                                <div>
                                    <div className="flex items-center gap-3 mb-2">
                                        <h1 className="text-4xl font-heading font-black text-primary tracking-tight">{artistData.fullName}</h1>
                                        {isAgencyProfile && (
                                            <span className="bg-primary/5 text-primary px-3 py-1 rounded-full text-[10px] font-black uppercase tracking-widest border border-primary/10">Agency</span>
                                        )}
                                        {artistData.isFeatured && (
                                            <span className="bg-secondary text-white px-3 py-1 rounded-full text-[10px] font-black uppercase tracking-widest shadow-lg shadow-secondary/20">Featured</span>
                                        )}
                                    </div>
                                    <p className="text-secondary text-lg font-bold">{artistData.category || 'Visual Creator'}</p>
                                </div>
                                <div className="flex gap-3 w-full md:w-auto">
                                    <Button variant="primary" className="flex-1 md:flex-none h-12 px-8 rounded-2xl font-bold shadow-lg shadow-primary/10" onClick={handleCollaborate}>Collaborate</Button>
                                    <Button variant="outline" className="hidden md:flex h-12 px-6 rounded-2xl font-bold"><Share2 className="w-4 h-4" /></Button>
                                </div>
                            </div>

                            <div className="flex flex-wrap items-center gap-6 text-sm font-medium text-textSecondary mb-8 bg-gray-50/50 p-4 rounded-2xl border border-gray-100 inline-flex">
                                {artistData.location && (
                                    <span className="flex items-center gap-2"><MapPin className="w-4 h-4 text-secondary" /> {artistData.location}</span>
                                )}
                                <span className="flex items-center gap-2"><Star className="w-4 h-4 text-accent fill-accent" /> {artistData.rating?.toFixed(1) || "0.0"} ({artistData.totalReviews || 0} Reviews)</span>
                                {isAgencyProfile && artistData.teamSize && (
                                    <span className="flex items-center gap-2"><Building2 className="w-4 h-4 text-primary" /> {artistData.teamSize} Team Members</span>
                                )}
                            </div>

                            <p className="text-textSecondary text-lg leading-relaxed max-w-4xl italic">
                                "{artistData.bio || 'This creator is still crafting their story...'}"
                            </p>
                        </div>
                    </div>
                </div>

                {/* Navigation Tabs */}
                <div className="flex gap-4 mb-10 overflow-x-auto no-scrollbar pb-2">
                    {['artworks', 'reviews', 'about'].map(tab => (
                        <button
                            key={tab}
                            onClick={() => setActiveTab(tab)}
                            className={`px-8 py-3 rounded-2xl text-sm font-black uppercase tracking-widest transition-all duration-300 ${activeTab === tab ? 'bg-primary text-white shadow-xl shadow-primary/20 scale-105' : 'bg-white text-textSecondary hover:bg-gray-50 border border-gray-100'}`}
                        >
                            {tab}
                        </button>
                    ))}
                </div>

                {/* Content Sections */}
                <div className="animate-in fade-in slide-in-from-bottom-4 duration-500">
                    {activeTab === 'artworks' && (
                        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8">
                            {artistData.featuredArtworks && artistData.featuredArtworks.length > 0 ? (
                                artistData.featuredArtworks.map(artwork => (
                                    <ProductCard key={artwork.artworkId || artwork.id} artwork={artwork} />
                                ))
                            ) : (
                                <div className="col-span-full py-32 text-center bg-white rounded-[3rem] border border-gray-100 flex flex-col items-center">
                                    <div className="w-20 h-20 bg-gray-50 rounded-full flex items-center justify-center mb-4">
                                        <Palette className="w-10 h-10 text-gray-200" />
                                    </div>
                                    <p className="text-2xl font-black text-primary/20 mb-2">Portfolio Empty</p>
                                    <p className="text-textSecondary">New masterpieces coming soon!</p>
                                </div>
                            )}
                        </div>
                    )}

                    {activeTab === 'reviews' && (
                        <div className="grid grid-cols-1 md:grid-cols-3 gap-10">
                            {/* Review Form */}
                            <div className="md:col-span-1">
                                {user && user.id !== artistData.userId ? (
                                    <div className="bg-white rounded-[2.5rem] p-8 border border-gray-100 shadow-xl shadow-primary/5 sticky top-24">
                                        <h3 className="text-xl font-black text-primary mb-6">Leave Feedback</h3>
                                        <form onSubmit={handleReviewSubmit} className="space-y-6">
                                            <div>
                                                <label className="block text-sm font-bold text-primary mb-3">Your Rating</label>
                                                <div className="flex gap-2">
                                                    {[1, 2, 3, 4, 5].map(star => (
                                                        <button
                                                            key={star}
                                                            type="button"
                                                            onClick={() => setReviewForm({ ...reviewForm, rating: star })}
                                                            className={`p-2 transition-transform hover:scale-110 ${reviewForm.rating >= star ? 'text-accent' : 'text-gray-200'}`}
                                                        >
                                                            <Star className={`w-8 h-8 ${reviewForm.rating >= star ? 'fill-current' : ''}`} />
                                                        </button>
                                                    ))}
                                                </div>
                                            </div>
                                            <div>
                                                <label className="block text-sm font-bold text-primary mb-3">Your Thoughts</label>
                                                <textarea
                                                    className="w-full px-5 py-4 rounded-2xl border border-gray-100 bg-gray-50/50 focus:bg-white focus:outline-none focus:ring-4 focus:ring-primary/5 transition-all min-h-[120px] text-sm"
                                                    placeholder="Describe your experience..."
                                                    value={reviewForm.comment}
                                                    onChange={(e) => setReviewForm({ ...reviewForm, comment: e.target.value })}
                                                    required
                                                />
                                            </div>
                                            <Button type="submit" variant="primary" className="w-full h-14 rounded-2xl font-black" isLoading={submittingReview}>
                                                Submit Review
                                            </Button>
                                        </form>
                                    </div>
                                ) : (
                                    <div className="bg-primary/5 rounded-[2.5rem] p-8 border border-primary/10 text-center">
                                        <p className="text-sm font-bold text-primary opacity-60">You cannot review your own profile or must be logged in to leave feedback.</p>
                                    </div>
                                )}
                            </div>

                            {/* Reviews List */}
                            <div className="md:col-span-2 space-y-6">
                                {reviews.length > 0 ? (
                                    reviews.map((review, i) => (
                                        <div key={review.reviewId || i} className="bg-white rounded-[2.5rem] p-8 border border-gray-100 shadow-sm animate-in slide-in-from-right-4 duration-500" style={{ animationDelay: `${i * 100}ms` }}>
                                            <div className="flex justify-between items-start mb-6">
                                                <div className="flex items-center gap-4">
                                                    <div className="w-12 h-12 rounded-2xl overflow-hidden bg-gray-50 flex items-center justify-center font-bold text-primary/30 text-xl border border-gray-100">
                                                        {review.reviewerProfileImage ? (
                                                            <img src={getImageUrl(review.reviewerProfileImage)} alt="" className="w-full h-full object-cover" />
                                                        ) : (
                                                            review.reviewerName?.charAt(0) || <User className="w-6 h-6" />
                                                        )}
                                                    </div>
                                                    <div>
                                                        <p className="font-black text-primary">{review.reviewerName}</p>
                                                        <p className="text-[10px] uppercase tracking-widest font-black text-textSecondary opacity-60">{new Date(review.createdAt).toLocaleDateString()}</p>
                                                    </div>
                                                </div>
                                                <div className="flex text-accent">
                                                    {[...Array(review.rating)].map((_, i) => <Star key={i} className="w-4 h-4 fill-current" />)}
                                                </div>
                                            </div>
                                            <p className="text-textSecondary leading-relaxed italic">"{review.comment}"</p>
                                        </div>
                                    ))
                                ) : (
                                    <div className="py-32 text-center bg-white rounded-[3.5rem] border border-gray-100">
                                         <MessageCircle className="w-16 h-16 text-gray-100 mx-auto mb-4" />
                                         <h4 className="text-xl font-black text-primary/20">No Reviews Yet</h4>
                                         <p className="text-textSecondary">Be the first to share your experience!</p>
                                    </div>
                                )}
                            </div>
                        </div>
                    )}

                    {activeTab === 'about' && (
                         <div className="bg-white rounded-[3rem] p-12 border border-gray-100">
                             <h4 className="text-2xl font-heading font-black text-primary mb-8 tracking-tight">Biography</h4>
                             <div className="prose prose-slate max-w-none">
                                 <p className="text-lg text-textSecondary leading-relaxed">
                                     {artistData.bio || "Personal biography has not been added yet."}
                                 </p>
                             </div>
                             
                             <h4 className="text-2xl font-heading font-black text-primary mt-12 mb-8 tracking-tight">Technical Skills</h4>
                             <div className="flex flex-wrap gap-4">
                                 {artistData.skills && artistData.skills.length > 0 ? (
                                     artistData.skills.map((skill, index) => (
                                         <span key={index} className="px-6 py-3 bg-primary/5 text-primary rounded-2xl text-sm font-black border border-primary/5 uppercase tracking-widest">
                                             {skill}
                                         </span>
                                     ))
                                 ) : (
                                     <p className="text-textSecondary italic">No skills listed.</p>
                                 )}
                             </div>
                         </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default ArtistProfile;
