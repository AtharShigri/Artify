import React, { useState, useEffect } from 'react';
import { Camera, Save, Loader as LoaderIcon } from 'lucide-react';
import Button from '../../components/common/Button';
import Input from '../../components/common/Input';
import artistService from '../../services/artistService';
import Loader from '../../components/common/Loader';
import { useAuth } from '../../context/AuthContext';
import { ART_CATEGORIES } from '../../constants/categories';
import { getImageUrl } from '../../utils/imageUtils'; // Ensure this matches your filename imageUtils or imageHelper
import { useNavigate } from 'react-router-dom';

const Settings = () => {
    const { user, updateUser, logout } = useAuth();
    const navigate = useNavigate();
    const [isLoading, setIsLoading] = useState(true);
    const [isSaving, setIsSaving] = useState(false);
    const [formData, setFormData] = useState({
        fullName: '',
        bio: '',
        category: '',
        phone: '',
        city: '',
        socialLink: '',
        userType: 0,
        teamSize: '',
        memberNames: ''
    });
    const [profileImage, setProfileImage] = useState(null);
    const [previewImage, setPreviewImage] = useState(null);
    const [message, setMessage] = useState({ type: '', text: '' });

    useEffect(() => {
        fetchProfile();
    }, []);

    const fetchProfile = async () => {
        try {
            const data = await artistService.getProfile();
            setFormData({
                fullName: data.fullName || '',
                bio: data.bio || '',
                category: data.category || '',
                phone: data.phone || '',
                city: data.city || '',
                socialLink: data.socialLink || '',
                userType: data.userType || 0,
                teamSize: data.teamSize || '',
                memberNames: data.memberNames || ''
            });
            if (data.profileImageUrl) {
                setPreviewImage(getImageUrl(data.profileImageUrl));
            }
        } catch (error) {
            setMessage({ type: 'error', text: 'Failed to load profile data' });
            console.error(error);
        } finally {
            setIsLoading(false);
        }
    };

    const handleImageChange = async (e) => {
        const file = e.target.files[0];
        if (!file) return;

        if (previewImage && previewImage.startsWith('blob:')) {
            URL.revokeObjectURL(previewImage);
        }

        const objectUrl = URL.createObjectURL(file);
        setProfileImage(file);
        setPreviewImage(objectUrl);

        const imageData = new FormData();
        imageData.append('Image', file);

        try {
            const response = await artistService.updateProfileImage(imageData);
            if (response && response.profileImageUrl) {
                updateUser({ profileImageUrl: response.profileImageUrl });
                setPreviewImage(getImageUrl(response.profileImageUrl));
                setMessage({ type: 'success', text: 'Photo updated!' });
            }
        } catch (error) {
            setMessage({ type: 'error', text: 'Failed to upload photo' });
            console.error("Upload failed", error);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setIsSaving(true);
        setMessage({ type: '', text: '' });

        try {
            // Ensure teamSize is integer if present
            const payload = { 
                ...formData, 
                teamSize: formData.teamSize ? parseInt(formData.teamSize) : null 
            };
            await artistService.updateProfile(payload);
            setMessage({ type: 'success', text: 'Profile updated successfully!' });

            updateUser({
                fullName: formData.fullName,
                name: formData.fullName
            });
        } catch (error) {
            setMessage({ type: 'error', text: 'Failed to update profile' });
            console.error(error);
        } finally {
            setIsSaving(false);
        }
    };

    if (isLoading) return <Loader />;

    return (
        <div className="max-w-3xl mx-auto">
            <h1 className="text-3xl font-heading font-black text-primary mb-2 tracking-tight">
                Account <span className="text-secondary">Settings</span>
            </h1>
            <p className="text-textSecondary mb-8 text-lg">Manage your public profile and workspace details</p>

            {message.text && (
                <div className={`p-4 rounded-xl mb-6 flex items-center gap-3 animate-in fade-in zoom-in duration-300 ${
                    message.type === 'success' 
                        ? 'bg-green-50 text-green-700 border border-green-100' 
                        : 'bg-red-50 text-red-700 border border-red-100'
                }`}>
                    <div className={`w-2 h-2 rounded-full ${message.type === 'success' ? 'bg-green-500' : 'bg-red-500'}`} />
                    {message.text}
                </div>
            )}

            <div className="bg-white rounded-[2.5rem] shadow-2xl shadow-primary/5 border border-gray-100 p-10 overflow-hidden relative">
                {/* Visual Accent */}
                <div className="absolute top-0 right-0 w-32 h-32 bg-secondary/5 rounded-bl-full -mr-10 -mt-10" />

                {/* Profile Image */}
                <div className="flex flex-col items-center mb-12 relative z-10">
                    <div className="relative group">
                        <div className="w-36 h-36 rounded-full overflow-hidden bg-gray-50 border-8 border-white shadow-xl group-hover:border-secondary/20 transition-all duration-500">
                            {previewImage ? (
                                <img src={previewImage} alt="Profile" className="w-full h-full object-cover group-hover:scale-110 transition-transform duration-700" />
                            ) : (
                                <div className="w-full h-full flex items-center justify-center text-5xl text-gray-200 font-black">
                                    {formData.fullName.charAt(0)}
                                </div>
                            )}
                        </div>
                        <label className="absolute bottom-2 right-2 p-3 bg-primary text-white rounded-2xl hover:bg-secondary cursor-pointer shadow-xl transition-all hover:scale-110 hover:-rotate-12">
                            <Camera className="w-5 h-5" />
                            <input type="file" className="hidden" accept="image/*" onChange={handleImageChange} />
                        </label>
                    </div>
                </div>

                <form onSubmit={handleSubmit} className="space-y-8">
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
                        <Input
                            label="Full Name"
                            value={formData.fullName}
                            onChange={(e) => setFormData({ ...formData, fullName: e.target.value })}
                            required
                        />
                        <div>
                            <label className="block text-sm font-bold text-primary mb-2">Art Category</label>
                            <select
                                name="category"
                                value={formData.category}
                                onChange={(e) => setFormData({ ...formData, category: e.target.value })}
                                className="w-full px-5 py-3 rounded-2xl border border-gray-100 bg-gray-50/50 focus:bg-white focus:outline-none focus:ring-4 focus:ring-primary/5 transition-all text-sm font-medium"
                            >
                                <option value="">Select a Category</option>
                                {ART_CATEGORIES.map((category) => (
                                    <option key={category} value={category}>{category}</option>
                                ))}
                            </select>
                        </div>
                    </div>

                    <div>
                        <label className="block text-sm font-bold text-primary mb-2">Bio / About</label>
                        <textarea
                            className="w-full px-5 py-4 rounded-2xl border border-gray-100 bg-gray-50/50 focus:bg-white focus:outline-none focus:ring-4 focus:ring-primary/5 transition-all min-h-[140px] text-sm leading-relaxed"
                            placeholder="Share your creative journey..."
                            value={formData.bio}
                            onChange={(e) => setFormData({ ...formData, bio: e.target.value })}
                        ></textarea>
                    </div>

                    {/* Agency Specific Fields */}
                    {formData.userType === 1 && (
                        <div className="p-8 bg-primary/5 rounded-[2rem] border border-primary/5 space-y-6">
                            <h3 className="font-bold text-primary flex items-center gap-2">
                                <Building2 className="w-5 h-5 text-secondary" /> Agency Workspace
                            </h3>
                            
                            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                                <div>
                                    <label className="block text-sm font-bold text-primary mb-2">Team Size</label>
                                    <select
                                        value={formData.teamSize}
                                        onChange={(e) => setFormData({ ...formData, teamSize: e.target.value })}
                                        className="w-full px-5 py-3 rounded-2xl border border-white bg-white focus:outline-none focus:ring-4 focus:ring-primary/5 transition-all text-sm font-medium"
                                    >
                                        <option value="">Select Size</option>
                                        {['1-2', '3-6', '7-20', '20+'].map(s => (
                                             <option key={s} value={s.includes('-') ? s.split('-')[1] : (s.includes('+') ? 50 : s)}>
                                                {s} Members
                                             </option>
                                        ))}
                                    </select>
                                </div>
                                <Input
                                    label="Portfolio URL"
                                    value={formData.socialLink}
                                    onChange={(e) => setFormData({ ...formData, socialLink: e.target.value })}
                                    placeholder="https://agency.com"
                                />
                            </div>

                            {(parseInt(formData.teamSize) < 7) && (
                                <div className="animate-in slide-in-from-top-2 duration-300">
                                    <label className="block text-sm font-bold text-primary mb-2">Member Names</label>
                                    <textarea
                                        className="w-full px-5 py-4 rounded-2xl border border-white bg-white focus:outline-none focus:ring-4 focus:ring-primary/5 transition-all min-h-[80px] text-sm"
                                        placeholder="Enter team member names..."
                                        value={formData.memberNames}
                                        onChange={(e) => setFormData({ ...formData, memberNames: e.target.value })}
                                    ></textarea>
                                </div>
                            )}
                        </div>
                    )}

                    <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
                        <Input
                            label="Phone Contact"
                            value={formData.phone}
                            onChange={(e) => setFormData({ ...formData, phone: e.target.value })}
                        />
                        <Input
                            label="Location"
                            value={formData.city}
                            onChange={(e) => setFormData({ ...formData, city: e.target.value })}
                        />
                    </div>

                    {!formData.socialLink && formData.userType !== 1 && (
                        <Input
                            label="Portfolio / Social Link"
                            value={formData.socialLink}
                            onChange={(e) => setFormData({ ...formData, socialLink: e.target.value })}
                            placeholder="https://behance.net/..."
                        />
                    )}

                    <div className="pt-6">
                        <Button type="submit" variant="primary" isLoading={isSaving} className="w-full h-14 rounded-2xl text-lg font-bold">
                            {!isSaving && <Save className="w-5 h-5 mr-2" />} Save All Changes
                        </Button>
                    </div>
                </form>
            </div>

            {/* Danger Zone */}
            <div className="mt-8 bg-red-50 border border-red-200 rounded-2xl shadow-sm p-8">
                <h3 className="text-lg font-bold text-red-700 mb-2">Danger Zone</h3>
                <p className="text-red-600/80 mb-6 text-sm">
                    Once you delete your profile, there is no going back. Please be certain.
                </p>
                <div className="flex gap-4 items-center">
                    <Button
                        variant="danger"
                        onClick={async () => {
                            if (window.confirm("Are you sure? This action is irreversible.")) {
                                try {
                                    setIsSaving(true);
                                    await artistService.deleteProfile();
                                    logout();
                                    navigate('/login');
                                } catch (error) {
                                    setMessage({ type: 'error', text: 'Failed to delete profile' });
                                    setIsSaving(false);
                                }
                            }
                        }}
                    >
                        Delete Profile
                    </Button>
                </div>
            </div>
        </div>
    );
};

export default Settings;