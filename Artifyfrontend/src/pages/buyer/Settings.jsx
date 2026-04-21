import React, { useState } from 'react';
import { User, Bell, Shield, Wallet, Save, Loader2 } from 'lucide-react';
import Button from '../../components/common/Button';
import { useAuth } from '../../context/AuthContext';

const Settings = () => {
    const { user } = useAuth();
    const [activeTab, setActiveTab] = useState('profile');
    const [loading, setLoading] = useState(false);
    const [success, setSuccess] = useState(false);

    const tabs = [
        { id: 'profile', name: 'Profile Settings', icon: User },
        { id: 'notifications', name: 'Notifications', icon: Bell },
        { id: 'security', name: 'Security', icon: Shield },
        { id: 'billing', name: 'Billing & Payments', icon: Wallet },
    ];

    const handleSubmit = (e) => {
        e.preventDefault();
        setLoading(true);
        // Simulate API call
        setTimeout(() => {
            setLoading(false);
            setSuccess(true);
            setTimeout(() => setSuccess(false), 3000);
        }, 1000);
    };

    return (
        <div className="max-w-4xl mx-auto py-8 px-4">
            <h1 className="text-3xl font-heading font-bold text-primary mb-8">Settings</h1>

            <div className="flex flex-col md:flex-row gap-8">
                {/* Tabs Sidebar */}
                <div className="w-full md:w-64 space-y-1">
                    {tabs.map((tab) => (
                        <button
                            key={tab.id}
                            onClick={() => setActiveTab(tab.id)}
                            className={`w-full flex items-center gap-3 px-4 py-3 rounded-xl text-sm font-medium transition-all ${
                                activeTab === tab.id
                                    ? 'bg-primary text-white shadow-md'
                                    : 'text-textSecondary hover:bg-gray-50 hover:text-primary'
                            }`}
                        >
                            <tab.icon className="w-5 h-5" />
                            {tab.name}
                        </button>
                    ))}
                </div>

                {/* Settings Panel */}
                <div className="flex-1 bg-white rounded-2xl border border-border p-6 md:p-8 shadow-sm">
                    {activeTab === 'profile' && (
                        <form onSubmit={handleSubmit} className="space-y-6">
                            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                                <div className="space-y-2">
                                    <label className="text-sm font-bold text-primary">Full Name</label>
                                    <input
                                        type="text"
                                        defaultValue={user?.fullName || ''}
                                        className="w-full px-4 py-3 rounded-xl border border-border focus:border-secondary focus:ring-1 focus:ring-secondary transition-all outline-none"
                                        placeholder="John Doe"
                                    />
                                </div>
                                <div className="space-y-2">
                                    <label className="text-sm font-bold text-primary">Email Address</label>
                                    <input
                                        type="email"
                                        defaultValue={user?.email || ''}
                                        className="w-full px-4 py-3 rounded-xl border border-border bg-gray-50 text-textSecondary outline-none cursor-not-allowed"
                                        readOnly
                                    />
                                </div>
                            </div>

                            <div className="space-y-2">
                                <label className="text-sm font-bold text-primary">Bio / Description</label>
                                <textarea
                                    rows="4"
                                    className="w-full px-4 py-3 rounded-xl border border-border focus:border-secondary focus:ring-1 focus:ring-secondary transition-all outline-none resize-none"
                                    placeholder="Tell us about yourself as an art enthusiast..."
                                ></textarea>
                            </div>

                            <div className="flex justify-end">
                                <Button
                                    type="submit"
                                    variant="primary"
                                    disabled={loading}
                                    className="px-8"
                                >
                                    {loading ? <Loader2 className="w-5 h-5 animate-spin" /> : <Save className="w-5 h-5 mr-2" />}
                                    Save Changes
                                </Button>
                            </div>
                            {success && (
                                <p className="text-center text-green-600 font-medium animate-fade-in">
                                    Settings updated successfully!
                                </p>
                            )}
                        </form>
                    )}

                    {activeTab !== 'profile' && (
                        <div className="py-20 text-center space-y-4">
                            <div className="w-16 h-16 bg-gray-50 rounded-full flex items-center justify-center mx-auto text-gray-400">
                                {tabs.find(t => t.id === activeTab)?.icon({ className: "w-8 h-8" })}
                            </div>
                            <h2 className="text-xl font-bold text-primary">{activeTab.charAt(0).toUpperCase() + activeTab.slice(1)} Settings coming soon</h2>
                            <p className="text-textSecondary max-w-xs mx-auto">We're working hard to bring you more control over your experience.</p>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default Settings;
