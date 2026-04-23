import React, { useState, useEffect } from 'react';
import { AlertTriangle, CheckCircle, Clock, ExternalLink, BarChart3, Users, Image as ImageIcon } from 'lucide-react';
import adminService from '../../services/adminService';
import Loader from '../../components/common/Loader';
import { getImageUrl } from '../../utils/imageUtils';

const AdminReports = () => {
    const [logs, setLogs] = useState([]);
    const [stats, setStats] = useState(null);
    const [loading, setLoading] = useState(true);
    const [activeTab, setActiveTab] = useState('plagiarism'); // 'plagiarism' or 'analytics'

    useEffect(() => {
        fetchData();
    }, []);

    const fetchData = async () => {
        try {
            setLoading(true);
            const [logsData, statsData] = await Promise.all([
                adminService.getPlagiarismLogs(),
                adminService.getDetailedReports('summary')
            ]);
            setLogs(logsData || []);
            setStats(statsData);
        } catch (err) {
            console.error("Error fetching report data:", err);
        } finally {
            setLoading(false);
        }
    };

    const handleMarkReviewed = async (logId) => {
        const notes = prompt("Enter review notes (optional):");
        try {
            await adminService.markPlagiarismReviewed(logId, notes);
            setLogs(logs.map(log => log.id === logId ? { ...log, isReviewed: true } : log));
        } catch (err) {
            console.error("Error marking log as reviewed:", err);
        }
    };

    if (loading) return <Loader fullScreen />;

    return (
        <div className="space-y-6 animate-in fade-in duration-500">
            <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                <div>
                    <h1 className="text-2xl font-heading font-bold text-primary">Plagiarism & Monitoring</h1>
                    <p className="text-textSecondary">Platform security and content integrity logs</p>
                </div>
                <div className="flex bg-white p-1 rounded-xl border border-border">
                    <button 
                        onClick={() => setActiveTab('plagiarism')}
                        className={`px-6 py-2 rounded-lg text-sm font-bold transition-all ${activeTab === 'plagiarism' ? 'bg-primary text-white' : 'text-textSecondary hover:bg-gray-50'}`}
                    >
                        Plagiarism Logs
                    </button>
                    <button 
                        onClick={() => setActiveTab('analytics')}
                        className={`px-6 py-2 rounded-lg text-sm font-bold transition-all ${activeTab === 'analytics' ? 'bg-primary text-white' : 'text-textSecondary hover:bg-gray-50'}`}
                    >
                        Detailed Stats
                    </button>
                </div>
            </div>

            {activeTab === 'plagiarism' ? (
                <div className="space-y-4">
                    {logs.map(log => (
                        <div key={log.id} className={`bg-white rounded-2xl border ${log.isReviewed ? 'border-border opacity-75' : 'border-red-200 shadow-sm'} overflow-hidden`}>
                            <div className="p-6">
                                <div className="flex flex-col lg:flex-row gap-6">
                                    {/* Comparison View */}
                                    <div className="flex-1 grid grid-cols-2 gap-4">
                                        <div className="space-y-2">
                                            <span className="text-[10px] font-bold text-textSecondary uppercase tracking-widest">Original Artwork</span>
                                            <div className="aspect-square rounded-xl overflow-hidden border border-border">
                                                <img 
                                                    src={getImageUrl(log.originalArtwork?.imageUrl)} 
                                                    className="w-full h-full object-cover" 
                                                    alt="Original" 
                                                />
                                            </div>
                                            <p className="text-sm font-bold truncate">{log.originalArtwork?.title}</p>
                                        </div>
                                        <div className="space-y-2">
                                            <span className="text-[10px] font-bold text-red-500 uppercase tracking-widest">Suspected Plagiarism</span>
                                            <div className="aspect-square rounded-xl overflow-hidden border-2 border-red-100">
                                                <img 
                                                    src={getImageUrl(log.suspectedArtwork?.imageUrl)} 
                                                    className="w-full h-full object-cover" 
                                                    alt="Suspect" 
                                                />
                                            </div>
                                            <p className="text-sm font-bold truncate text-red-600">{log.suspectedArtwork?.title}</p>
                                        </div>
                                    </div>

                                    {/* Info & Actions */}
                                    <div className="lg:w-80 flex flex-col justify-between">
                                        <div>
                                            <div className="flex items-center gap-2 mb-4">
                                                <div className={`p-2 rounded-lg ${log.similarityScore > 90 ? 'bg-red-100 text-red-600' : 'bg-yellow-100 text-yellow-600'}`}>
                                                    <AlertTriangle className="w-5 h-5" />
                                                </div>
                                                <div>
                                                    <p className="text-xl font-bold text-primary">{log.similarityScore}% Match</p>
                                                    <p className="text-xs text-textSecondary">Detected on {new Date(log.createdAt).toLocaleDateString()}</p>
                                                </div>
                                            </div>
                                            <div className="space-y-3 bg-gray-50 p-4 rounded-xl text-sm">
                                                <div className="flex justify-between">
                                                    <span className="text-textSecondary">Algorithm:</span>
                                                    <span className="font-medium">pHash (DCT)</span>
                                                </div>
                                                <div className="flex justify-between">
                                                    <span className="text-textSecondary">Status:</span>
                                                    <span className={`font-bold ${log.isReviewed ? 'text-green-600' : 'text-yellow-600'}`}>
                                                        {log.isReviewed ? 'Reviewed' : 'Awaiting Review'}
                                                    </span>
                                                </div>
                                            </div>
                                        </div>

                                        <div className="flex gap-2 mt-6">
                                            {!log.isReviewed && (
                                                <button 
                                                    onClick={() => handleMarkReviewed(log.id)}
                                                    className="flex-1 bg-secondary text-white py-2.5 rounded-xl text-sm font-bold hover:bg-secondary/90 transition-all flex items-center justify-center gap-2"
                                                >
                                                    <CheckCircle className="w-4 h-4" />
                                                    Dismiss / Safe
                                                </button>
                                            )}
                                            <a 
                                                href={`/artwork/${log.suspectedArtworkId}`} 
                                                target="_blank" 
                                                rel="noreferrer"
                                                className="p-2.5 bg-gray-100 text-gray-600 rounded-xl hover:bg-gray-200 transition-all"
                                            >
                                                <ExternalLink className="w-5 h-5" />
                                            </a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    ))}
                    {logs.length === 0 && (
                        <div className="py-20 text-center bg-white rounded-2xl border border-dashed border-border text-textSecondary">
                            <CheckCircle className="w-12 h-12 mx-auto mb-4 text-green-200" />
                            <p>All clear! No plagiarism logs detected.</p>
                        </div>
                    )}
                </div>
            ) : (
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    <AnalyticsCard 
                        title="User Stats" 
                        value={stats?.users || 0} 
                        subtitle={`${stats?.activeUsers || 0} Active Users`}
                        icon={<Users className="w-6 h-6" />}
                        color="bg-blue-50 text-blue-600"
                    />
                    <AnalyticsCard 
                        title="Artwork Flow" 
                        value={stats?.pendingArtworks || 0} 
                        subtitle="Pending Moderation"
                        icon={<ImageIcon className="w-6 h-6" />}
                        color="bg-purple-50 text-purple-600"
                    />
                    <AnalyticsCard 
                        title="Sales Volume" 
                        value={`PKR ${stats?.totalSales?.toLocaleString() || 0}`} 
                        subtitle="Total Platform Sales"
                        icon={<BarChart3 className="w-6 h-6" />}
                        color="bg-green-50 text-green-600"
                    />
                </div>
            )}
        </div>
    );
};

const AnalyticsCard = ({ title, value, subtitle, icon, color }) => (
    <div className="bg-white p-8 rounded-2xl border border-border shadow-sm group hover:border-secondary/30 transition-all">
        <div className={`w-12 h-12 rounded-xl ${color} flex items-center justify-center mb-6 group-hover:scale-110 transition-transform`}>
            {icon}
        </div>
        <div className="text-3xl font-bold text-primary mb-2">{value}</div>
        <div className="font-bold text-sm text-primary uppercase tracking-wider">{title}</div>
        <p className="text-xs text-textSecondary mt-1">{subtitle}</p>
    </div>
);

export default AdminReports;
