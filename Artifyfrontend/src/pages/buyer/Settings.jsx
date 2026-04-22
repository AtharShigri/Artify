import React, { useState } from 'react';
import { User, Shield, Wallet, Save, Loader2, Lock, CreditCard, Eye, EyeOff } from 'lucide-react';
import Button from '../../components/common/Button';
import { useAuth } from '../../context/AuthContext';

// ─── Security Tab ────────────────────────────────────────────────────────────
const SecurityTab = () => {
    const [showCurrent, setShowCurrent] = useState(false);
    const [showNew, setShowNew] = useState(false);
    const [showConfirm, setShowConfirm] = useState(false);
    const [formData, setFormData] = useState({ currentPassword: '', newPassword: '', confirmPassword: '' });
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState({ type: '', text: '' });

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (formData.newPassword !== formData.confirmPassword) {
            setMessage({ type: 'error', text: 'New passwords do not match.' });
            return;
        }
        if (formData.newPassword.length < 6) {
            setMessage({ type: 'error', text: 'Password must be at least 6 characters.' });
            return;
        }
        setLoading(true);
        setMessage({ type: '', text: '' });
        // Simulate API call
        await new Promise(r => setTimeout(r, 1000));
        setLoading(false);
        setMessage({ type: 'success', text: 'Password updated successfully!' });
        setFormData({ currentPassword: '', newPassword: '', confirmPassword: '' });
    };

    return (
        <div className="space-y-6">
            <div>
                <h2 className="text-xl font-bold text-primary mb-1">Security Settings</h2>
                <p className="text-sm text-textSecondary">Manage your account security and password.</p>
            </div>

            {message.text && (
                <div className={`p-4 rounded-xl text-sm border ${
                    message.type === 'success'
                        ? 'bg-green-50 text-green-700 border-green-100'
                        : 'bg-red-50 text-red-700 border-red-100'
                }`}>
                    {message.text}
                </div>
            )}

            <form onSubmit={handleSubmit} className="space-y-5">
                <div>
                    <label className="block text-sm font-bold text-primary mb-1.5">Current Password</label>
                    <div className="relative">
                        <Lock className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-textSecondary" />
                        <input
                            type={showCurrent ? 'text' : 'password'}
                            value={formData.currentPassword}
                            onChange={e => setFormData({ ...formData, currentPassword: e.target.value })}
                            required
                            placeholder="Enter current password"
                            className="w-full pl-10 pr-10 py-3 rounded-xl border border-border focus:border-secondary focus:ring-1 focus:ring-secondary transition-all outline-none"
                        />
                        <button type="button" onClick={() => setShowCurrent(!showCurrent)} className="absolute right-3 top-1/2 -translate-y-1/2 text-textSecondary hover:text-primary">
                            {showCurrent ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                        </button>
                    </div>
                </div>

                <div>
                    <label className="block text-sm font-bold text-primary mb-1.5">New Password</label>
                    <div className="relative">
                        <Lock className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-textSecondary" />
                        <input
                            type={showNew ? 'text' : 'password'}
                            value={formData.newPassword}
                            onChange={e => setFormData({ ...formData, newPassword: e.target.value })}
                            required
                            placeholder="At least 6 characters"
                            className="w-full pl-10 pr-10 py-3 rounded-xl border border-border focus:border-secondary focus:ring-1 focus:ring-secondary transition-all outline-none"
                        />
                        <button type="button" onClick={() => setShowNew(!showNew)} className="absolute right-3 top-1/2 -translate-y-1/2 text-textSecondary hover:text-primary">
                            {showNew ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                        </button>
                    </div>
                </div>

                <div>
                    <label className="block text-sm font-bold text-primary mb-1.5">Confirm New Password</label>
                    <div className="relative">
                        <Lock className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-textSecondary" />
                        <input
                            type={showConfirm ? 'text' : 'password'}
                            value={formData.confirmPassword}
                            onChange={e => setFormData({ ...formData, confirmPassword: e.target.value })}
                            required
                            placeholder="Repeat new password"
                            className="w-full pl-10 pr-10 py-3 rounded-xl border border-border focus:border-secondary focus:ring-1 focus:ring-secondary transition-all outline-none"
                        />
                        <button type="button" onClick={() => setShowConfirm(!showConfirm)} className="absolute right-3 top-1/2 -translate-y-1/2 text-textSecondary hover:text-primary">
                            {showConfirm ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                        </button>
                    </div>
                </div>

                <div className="flex justify-end">
                    <Button type="submit" variant="primary" disabled={loading} className="px-8">
                        {loading ? <Loader2 className="w-5 h-5 animate-spin" /> : <Save className="w-5 h-5 mr-2" />}
                        Update Password
                    </Button>
                </div>
            </form>

            {/* Account Actions */}
            <div className="border-t border-border pt-6 mt-6">
                <h3 className="font-bold text-primary mb-3">Active Sessions</h3>
                <div className="bg-gray-50 rounded-xl p-4 flex items-center justify-between">
                    <div>
                        <p className="font-medium text-sm text-primary">Current Session</p>
                        <p className="text-xs text-textSecondary mt-0.5">Browser · This device · Active now</p>
                    </div>
                    <span className="text-xs px-2.5 py-1 bg-green-100 text-green-700 rounded-full font-medium">Active</span>
                </div>
            </div>
        </div>
    );
};

// ─── Billing Tab ─────────────────────────────────────────────────────────────
const BillingTab = () => {
    return (
        <div className="space-y-6">
            <div>
                <h2 className="text-xl font-bold text-primary mb-1">Billing & Payments</h2>
                <p className="text-sm text-textSecondary">Manage your payment methods and billing history.</p>
            </div>

            {/* No Payment Methods */}
            <div className="bg-gray-50 rounded-xl border border-dashed border-border p-10 text-center">
                <div className="w-14 h-14 bg-white rounded-full flex items-center justify-center mx-auto mb-4 shadow-sm">
                    <CreditCard className="w-7 h-7 text-textSecondary opacity-60" />
                </div>
                <h3 className="font-bold text-primary mb-1">No payment methods</h3>
                <p className="text-sm text-textSecondary max-w-xs mx-auto mb-5">
                    Add a payment method to purchase artwork and commission artists on Artify.
                </p>
                <Button variant="primary" size="sm">
                    <CreditCard className="w-4 h-4 mr-2" />
                    Add Payment Method
                </Button>
            </div>

            {/* Order History Summary */}
            <div>
                <h3 className="font-bold text-primary mb-3">Billing History</h3>
                <div className="bg-white rounded-xl border border-border overflow-hidden">
                    <div className="p-8 text-center text-textSecondary">
                        <p className="text-sm">No billing history yet.</p>
                        <p className="text-xs mt-1">Your past payments will appear here.</p>
                    </div>
                </div>
            </div>
        </div>
    );
};

// ─── Main Settings ────────────────────────────────────────────────────────────
const Settings = () => {
    const { user } = useAuth();
    const [activeTab, setActiveTab] = useState('profile');
    const [loading, setLoading] = useState(false);
    const [success, setSuccess] = useState(false);

    const tabs = [
        { id: 'profile', name: 'Profile Settings', icon: User },
        { id: 'security', name: 'Security', icon: Shield },
        { id: 'billing', name: 'Billing & Payments', icon: Wallet },
    ];

    const handleSubmit = (e) => {
        e.preventDefault();
        setLoading(true);
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
                <div className="w-full md:w-64 space-y-1 shrink-0">
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
                            <div>
                                <h2 className="text-xl font-bold text-primary mb-1">Profile Settings</h2>
                                <p className="text-sm text-textSecondary">Update your personal information.</p>
                            </div>

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
                                />
                            </div>

                            <div className="flex justify-end">
                                <Button type="submit" variant="primary" disabled={loading} className="px-8">
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

                    {activeTab === 'security' && <SecurityTab />}
                    {activeTab === 'billing' && <BillingTab />}
                </div>
            </div>
        </div>
    );
};

export default Settings;
