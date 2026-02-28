import React, { useState, useEffect } from 'react';
import { Camera, Save, Loader as LoaderIcon } from 'lucide-react';
import Button from '../../components/common/Button';
import Input from '../../components/common/Input';
import artistService from '../../services/artistService';
import Loader from '../../components/common/Loader';
import { useAuth } from '../../context/AuthContext';
import { ART_CATEGORIES } from '../../constants/categories';

const Settings = () => {
    const { user } = useAuth();
    const [isLoading, setIsLoading] = useState(true);
    const [isSaving, setIsSaving] = useState(false);
    const [formData, setFormData] = useState({
        fullName: '',
        bio: '',
        category: '',
        phone: '',
        city: '',
        socialLink: ''
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
                socialLink: data.socialLink || ''
            });
            if (data.profileImageUrl) {
                setPreviewImage(data.profileImageUrl);
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

        setProfileImage(file);
        setPreviewImage(URL.createObjectURL(file));

        // Auto upload image on selection (or can be done on save)
        const imageData = new FormData();
        imageData.append('image', file);

        try {
            await artistService.updateProfileImage(imageData);
            setMessage({ type: 'success', text: 'Profile image updated!' });
        } catch (error) {
            setMessage({ type: 'error', text: 'Failed to upload image' });
            console.error(error);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setIsSaving(true);
        setMessage({ type: '', text: '' });

        try {
            await artistService.updateProfile(formData);
            setMessage({ type: 'success', text: 'Profile updated successfully!' });
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
            <h1 className="text-2xl font-heading font-bold text-primary mb-2">Profile Settings</h1>
            <p className="text-textSecondary mb-8">Manage your public profile and account details</p>

            {message.text && (
                <div className={`p-4 rounded-lg mb-6 ${message.type === 'success' ? 'bg-green-50 text-green-700 border border-green-200' : 'bg-red-50 text-red-700 border border-red-200'}`}>
                    {message.text}
                </div>
            )}

            <div className="bg-white rounded-2xl shadow-sm border border-border p-8">
                {/* Profile Image */}
                <div className="flex flex-col items-center mb-8">
                    <div className="relative w-32 h-32 mb-4">
                        <div className="w-full h-full rounded-full overflow-hidden bg-gray-100 border-4 border-white shadow-md">
                            {previewImage ? (
                                <img src={previewImage} alt="Profile" className="w-full h-full object-cover" />
                            ) : (
                                <div className="w-full h-full flex items-center justify-center text-4xl text-gray-300 font-bold">
                                    {formData.fullName.charAt(0)}
                                </div>
                            )}
                        </div>
                        <label className="absolute bottom-0 right-0 p-2 bg-primary text-white rounded-full hover:bg-primary/90 cursor-pointer shadow-lg transition-transform hover:scale-105">
                            <Camera className="w-5 h-5" />
                            <input type="file" className="hidden" accept="image/*" onChange={handleImageChange} />
                        </label>
                    </div>
                    <p className="text-sm text-textSecondary">Click camera icon to update photo</p>
                </div>

                <form onSubmit={handleSubmit} className="space-y-6">
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                        <Input
                            label="Full Name"
                            value={formData.fullName}
                            onChange={(e) => setFormData({ ...formData, fullName: e.target.value })}
                            required
                        />
                        <div>
                            <label className="block text-sm font-medium text-textSecondary mb-1.5">Art Category</label>
                            <select
                                name="category"
                                value={formData.category}
                                onChange={(e) => setFormData({ ...formData, category: e.target.value })}
                                className="w-full px-4 py-3 rounded-lg border border-border focus:outline-none focus:ring-2 focus:ring-secondary/20 focus:border-secondary transition-all bg-white"
                            >
                                <option value="">Select a Category</option>
                                {ART_CATEGORIES.map((category) => (
                                    <option key={category} value={category}>
                                        {category}
                                    </option>
                                ))}
                            </select>
                        </div>
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-textSecondary mb-1.5">Bio</label>
                        <textarea
                            className="w-full px-4 py-3 rounded-lg border border-border focus:outline-none focus:ring-2 focus:ring-secondary/20 focus:border-secondary min-h-[120px]"
                            placeholder="Tell the world about yourself..."
                            value={formData.bio}
                            onChange={(e) => setFormData({ ...formData, bio: e.target.value })}
                        ></textarea>
                    </div>

                    <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                        <Input
                            label="Phone Number"
                            value={formData.phone}
                            onChange={(e) => setFormData({ ...formData, phone: e.target.value })}
                        />
                        <Input
                            label="City / Location"
                            value={formData.city}
                            onChange={(e) => setFormData({ ...formData, city: e.target.value })}
                        />
                    </div>

                    <Input
                        label="Portfolio / Social Link"
                        value={formData.socialLink}
                        onChange={(e) => setFormData({ ...formData, socialLink: e.target.value })}
                        placeholder="https://..."
                    />

                    <div className="pt-4 flex justify-end gap-3">
                        {/* Optional Cancel button */}
                        <Button type="submit" variant="primary" disabled={isSaving} className="min-w-[120px]">
                            {isSaving ? (
                                <><LoaderIcon className="w-4 h-4 mr-2 animate-spin" /> Saving...</>
                            ) : (
                                <><Save className="w-4 h-4 mr-2" /> Save Changes</>
                            )}
                        </Button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default Settings;
