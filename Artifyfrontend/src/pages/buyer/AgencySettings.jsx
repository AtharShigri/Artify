import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
    Building2, Users, UserPlus, Save, Loader2,
    Mail, Shield, CheckCircle
} from 'lucide-react';
import SEO from '../../components/common/SEO';
import Button from '../../components/common/Button';
import Input from '../../components/common/Input';
import { agencyService } from '../../services/agencyService';
import { useAuth } from '../../context/AuthContext';

const AgencySettings = () => {
    const { user, isAgency, isArtist } = useAuth();
    const navigate = useNavigate();

    // ── Guard: redirect non-agency users ──
    useEffect(() => {
        if (!isAgency) {
            const fallback = isArtist ? '/dashboard/artist' : '/dashboard/buyer/orders';
            navigate(fallback, { replace: true });
        }
    }, [isAgency, isArtist, navigate]);

    // ── Agency Profile State ──
    const [profile, setProfile] = useState({ name: '', description: '' });
    const [isSavingProfile, setIsSavingProfile] = useState(false);
    const [profileSuccess, setProfileSuccess] = useState('');
    const [profileError, setProfileError] = useState('');

    // ── Team State ──
    const [team, setTeam] = useState(null);
    const [isLoadingTeam, setIsLoadingTeam] = useState(true);
    const [teamError, setTeamError] = useState('');

    // ── Add Member State ──
    const [memberEmail, setMemberEmail] = useState('');
    const [isAddingMember, setIsAddingMember] = useState(false);
    const [memberSuccess, setMemberSuccess] = useState('');
    const [memberError, setMemberError] = useState('');

    // Fetch team on mount
    useEffect(() => {
        if (!isAgency) return;
        const fetchTeam = async () => {
            try {
                const data = await agencyService.getMyTeam();
                setTeam(data);
                setProfile({
                    name: data.agencyName || '',
                    description: data.description || ''
                });
            } catch (err) {
                setTeamError(err.message || 'Failed to load team');
            } finally {
                setIsLoadingTeam(false);
            }
        };
        fetchTeam();
    }, [isAgency]);

    const handleSaveProfile = async (e) => {
        e.preventDefault();
        setProfileError('');
        setIsSavingProfile(true);
        try {
            await agencyService.setupAgency(profile);
            setProfileSuccess('Agency profile updated!');
            setTimeout(() => setProfileSuccess(''), 3000);
        } catch (err) {
            setProfileError(err.message || 'Failed to save profile');
        } finally {
            setIsSavingProfile(false);
        }
    };

    const handleAddMember = async (e) => {
        e.preventDefault();
        setMemberError('');
        setMemberSuccess('');
        setIsAddingMember(true);
        try {
            await agencyService.addMember(memberEmail);
            setMemberSuccess(`${memberEmail} was added to your agency.`);
            setMemberEmail('');
            // Refresh team
            const data = await agencyService.getMyTeam();
            setTeam(data);
        } catch (err) {
            setMemberError(err.message || 'Failed to add member');
        } finally {
            setIsAddingMember(false);
        }
    };

    if (!isAgency) return null; // will redirect via effect

    return (
        <div className="max-w-3xl mx-auto space-y-8">
            <SEO title="Agency Settings" description="Manage your agency profile and team members on Artify." />

            {/* Header */}
            <div className="flex items-center gap-4">
                <div className="w-12 h-12 bg-primary/10 rounded-2xl flex items-center justify-center">
                    <Building2 className="w-6 h-6 text-primary" />
                </div>
                <div>
                    <h1 className="text-2xl font-heading font-bold text-primary">Agency Settings</h1>
                    <p className="text-textSecondary text-sm">Manage your agency profile and team members</p>
                </div>
            </div>

            {/* ── Agency Profile Card ── */}
            <div className="bg-white rounded-2xl border border-border shadow-sm">
                <div className="px-6 py-4 border-b border-border flex items-center gap-2">
                    <Shield className="w-4 h-4 text-secondary" />
                    <h2 className="font-bold text-primary">Agency Profile</h2>
                </div>
                <form onSubmit={handleSaveProfile} className="p-6 space-y-5">
                    {profileError && (
                        <div className="p-3 bg-red-50 text-error text-sm rounded-xl border border-red-100">{profileError}</div>
                    )}
                    {profileSuccess && (
                        <div className="p-3 bg-green-50 text-green-700 text-sm rounded-xl border border-green-200 flex items-center gap-2">
                            <CheckCircle className="w-4 h-4" /> {profileSuccess}
                        </div>
                    )}

                    <div>
                        <label className="block text-sm font-medium text-textSecondary mb-1.5">Agency Name</label>
                        <input
                            type="text"
                            value={profile.name}
                            onChange={(e) => setProfile({ ...profile, name: e.target.value })}
                            required
                            placeholder="e.g. Creative Collective Studio"
                            className="w-full px-4 py-3 rounded-xl border border-border focus:outline-none focus:ring-2 focus:ring-secondary/20 focus:border-secondary transition-all"
                        />
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-textSecondary mb-1.5">Description</label>
                        <textarea
                            value={profile.description}
                            onChange={(e) => setProfile({ ...profile, description: e.target.value })}
                            rows={4}
                            placeholder="Describe your agency — services offered, specialties, and background..."
                            className="w-full px-4 py-3 rounded-xl border border-border focus:outline-none focus:ring-2 focus:ring-secondary/20 focus:border-secondary transition-all resize-none"
                        />
                    </div>

                    <Button type="submit" variant="primary" isLoading={isSavingProfile} className="flex items-center gap-2">
                        <Save className="w-4 h-4" />
                        Save Profile
                    </Button>
                </form>
            </div>

            {/* ── Team Members Card ── */}
            <div className="bg-white rounded-2xl border border-border shadow-sm">
                <div className="px-6 py-4 border-b border-border flex items-center gap-2">
                    <Users className="w-4 h-4 text-secondary" />
                    <h2 className="font-bold text-primary">Team Members</h2>
                </div>

                <div className="p-6">
                    {isLoadingTeam ? (
                        <div className="flex items-center gap-2 text-textSecondary text-sm py-4">
                            <Loader2 className="w-4 h-4 animate-spin" />
                            Loading team...
                        </div>
                    ) : teamError ? (
                        <p className="text-error text-sm">{teamError}</p>
                    ) : team?.members?.length > 0 ? (
                        <div className="space-y-3 mb-6">
                            {team.members.map((m, i) => (
                                <div key={i} className="flex items-center gap-3 p-3 bg-gray-50 rounded-xl">
                                    <div className="w-9 h-9 rounded-full bg-secondary/10 flex items-center justify-center shrink-0">
                                        <span className="text-sm font-bold text-secondary">
                                            {m.fullName?.[0]?.toUpperCase() || '?'}
                                        </span>
                                    </div>
                                    <div className="flex-1 min-w-0">
                                        <p className="font-semibold text-primary text-sm truncate">{m.fullName}</p>
                                        <p className="text-xs text-textSecondary truncate">{m.email}</p>
                                    </div>
                                    <span className="text-xs px-2.5 py-1 bg-secondary/10 text-secondary rounded-full font-medium shrink-0">
                                        {m.roleInAgency}
                                    </span>
                                </div>
                            ))}
                        </div>
                    ) : (
                        <p className="text-sm text-textSecondary mb-6">
                            No team members yet. Add collaborators by their email below.
                        </p>
                    )}

                    {/* Add Member Form */}
                    <div className="border-t border-border pt-5">
                        <h3 className="text-sm font-bold text-primary mb-3 flex items-center gap-2">
                            <UserPlus className="w-4 h-4" />
                            Add a Member
                        </h3>

                        {memberError && (
                            <div className="mb-3 p-3 bg-red-50 text-error text-sm rounded-xl border border-red-100">{memberError}</div>
                        )}
                        {memberSuccess && (
                            <div className="mb-3 p-3 bg-green-50 text-green-700 text-sm rounded-xl border border-green-200 flex items-center gap-2">
                                <CheckCircle className="w-4 h-4" /> {memberSuccess}
                            </div>
                        )}

                        <form onSubmit={handleAddMember} className="flex gap-3">
                            <div className="flex-1 relative">
                                <Mail className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-textSecondary" />
                                <input
                                    type="email"
                                    value={memberEmail}
                                    onChange={(e) => setMemberEmail(e.target.value)}
                                    required
                                    placeholder="member@example.com"
                                    className="w-full pl-9 pr-4 py-3 rounded-xl border border-border focus:outline-none focus:ring-2 focus:ring-secondary/20 focus:border-secondary transition-all"
                                />
                            </div>
                            <Button type="submit" variant="primary" isLoading={isAddingMember} className="shrink-0">
                                Add
                            </Button>
                        </form>
                        <p className="mt-2 text-xs text-textSecondary">
                            The user must already have an Artify account to be added.
                        </p>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default AgencySettings;
