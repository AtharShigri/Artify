import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import {
    Package, CheckCircle, ShoppingBag, Briefcase,
    DollarSign, ChevronDown, ChevronUp, User,
    Loader2, PlusCircle, Check, X as XIcon
} from 'lucide-react';
import Button from '../../components/common/Button';
import { projectBoardService } from '../../services/projectBoardService';

import orderService from '../../services/orderService';

// ─── Orders Tab (Connected to real data) ──────────────────────────────────────────
const OrdersTab = () => {
    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchOrders = async () => {
            try {
                const data = await orderService.getBuyerOrders();
                setOrders(data || []);
            } catch (err) {
                setError(err.message || 'Failed to load orders');
            } finally {
                setLoading(false);
            }
        };
        fetchOrders();
    }, []);

    if (loading) return (
        <div className="flex items-center justify-center py-20 text-textSecondary">
            <Loader2 className="w-5 h-5 animate-spin mr-2" />
            Loading your orders...
        </div>
    );

    if (error) return <div className="text-center py-16 text-error">{error}</div>;

    if (orders.length === 0) return (
        <div className="bg-white rounded-xl border border-border shadow-sm p-12 text-center">
            <ShoppingBag className="w-10 h-10 mx-auto mb-3 text-textSecondary opacity-40" />
            <h3 className="font-bold text-primary mb-1">No orders yet</h3>
            <p className="text-sm text-textSecondary mb-4">When you purchase artwork, your orders will appear here.</p>
            <Link to="/marketplace">
                <Button variant="primary">Visit Marketplace</Button>
            </Link>
        </div>
    );

    return (
        <div className="bg-white rounded-xl shadow-sm border border-border overflow-hidden">
            <div className="overflow-x-auto">
                <table className="w-full text-left">
                    <thead className="bg-gray-50 border-b border-border">
                        <tr>
                            <th className="px-6 py-4 font-bold text-sm text-primary">Order ID</th>
                            <th className="px-6 py-4 font-bold text-sm text-primary">Date</th>
                            <th className="px-6 py-4 font-bold text-sm text-primary">Total</th>
                            <th className="px-6 py-4 font-bold text-sm text-primary">Status</th>
                            <th className="px-6 py-4 text-right">Actions</th>
                        </tr>
                    </thead>
                    <tbody className="divide-y divide-gray-100">
                        {orders.map((order) => (
                            <tr key={order.orderId} className="hover:bg-gray-50/50">
                                <td className="px-6 py-4 font-medium text-primary uppercase">#{order.orderId.substring(0, 8)}</td>
                                <td className="px-6 py-4 text-textSecondary">{new Date(order.orderDate).toLocaleDateString()}</td>
                                <td className="px-6 py-4 font-medium text-primary">PKR {order.totalAmount?.toLocaleString()}</td>
                                <td className="px-6 py-4">
                                    <div className={`inline-flex items-center gap-1.5 px-2.5 py-1 rounded-md text-xs font-medium ${
                                        order.deliveryStatus === 'Delivered'
                                            ? 'bg-green-100 text-green-700'
                                            : 'bg-blue-100 text-blue-700'
                                    }`}>
                                        {order.deliveryStatus === 'Delivered'
                                            ? <CheckCircle className="w-3 h-3" />
                                            : <Package className="w-3 h-3" />}
                                        {order.deliveryStatus}
                                    </div>
                                </td>
                                <td className="px-6 py-4 text-right">
                                    <Button variant="ghost" size="sm">View Details</Button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

// ─── Project Card with Proposals ────────────────────────────────────────────
const ProjectCard = ({ job, onStatusUpdate }) => {
    const [isExpanded, setIsExpanded] = useState(false);
    const [updatingId, setUpdatingId] = useState(null);

    const proposals = job.proposals || [];

    const handleStatus = async (proposalId, status) => {
        setUpdatingId(proposalId);
        try {
            await projectBoardService.updateProposalStatus(proposalId, status);
            onStatusUpdate();
        } catch (err) {
            alert(err.message || 'Failed to update status');
        } finally {
            setUpdatingId(null);
        }
    };

    return (
        <div className="bg-white rounded-xl border border-border shadow-sm overflow-hidden">
            {/* Job Header */}
            <div className="px-6 py-4 flex items-start justify-between gap-4">
                <div className="flex-1 min-w-0">
                    <h3 className="font-bold text-primary text-lg truncate">{job.title}</h3>
                    <p className="text-sm text-textSecondary mt-0.5 line-clamp-2">{job.description}</p>
                    <div className="flex items-center gap-4 mt-2">
                        <span className="flex items-center gap-1 text-sm font-semibold text-green-600">
                            <DollarSign className="w-4 h-4" />
                            Budget: PKR {job.budget?.toLocaleString()}
                        </span>
                        <span className="text-sm text-textSecondary">
                            {proposals.length} proposal{proposals.length !== 1 ? 's' : ''}
                        </span>
                    </div>
                </div>
                <button
                    onClick={() => setIsExpanded(!isExpanded)}
                    className="flex items-center gap-1.5 text-sm text-secondary hover:text-primary transition-colors font-medium mt-1 shrink-0"
                >
                    {isExpanded ? (
                        <><ChevronUp className="w-4 h-4" /> Hide</>
                    ) : (
                        <><ChevronDown className="w-4 h-4" /> View Proposals</>
                    )}
                </button>
            </div>

            {/* Proposals List */}
            {isExpanded && (
                <div className="border-t border-border">
                    {proposals.length === 0 ? (
                        <div className="px-6 py-8 text-center text-textSecondary text-sm">
                            No proposals yet. Artists will submit their bids and cover letters here.
                        </div>
                    ) : (
                        <div className="divide-y divide-gray-100">
                            {proposals.map((p) => (
                                <div key={p.id} className="px-6 py-4 flex flex-col sm:flex-row sm:items-start sm:justify-between gap-3">
                                    <div className="flex-1 min-w-0">
                                        <div className="flex items-center gap-2 mb-1">
                                            <div className="w-7 h-7 rounded-full bg-secondary/10 flex items-center justify-center shrink-0">
                                                <User className="w-4 h-4 text-secondary" />
                                            </div>
                                            <span className="font-semibold text-primary text-sm">
                                                {p.applicant?.fullName || p.applicantName || 'Artist'}
                                            </span>
                                            {/* Status badge */}
                                            <span className={`ml-auto sm:ml-2 text-xs px-2 py-0.5 rounded-full font-medium ${
                                                p.status === 'Accepted'
                                                    ? 'bg-green-100 text-green-700'
                                                    : p.status === 'Rejected'
                                                    ? 'bg-red-100 text-red-600'
                                                    : 'bg-yellow-100 text-yellow-700'
                                            }`}>
                                                {p.status || 'Pending'}
                                            </span>
                                        </div>
                                        <p className="text-sm text-textSecondary leading-relaxed mb-2 pl-9">
                                            {p.coverLetter}
                                        </p>
                                        <div className="pl-9">
                                            <span className="text-sm font-bold text-primary">
                                                Bid: PKR {p.bidAmount?.toLocaleString()}
                                            </span>
                                        </div>
                                    </div>

                                    {/* Action buttons — only show if Pending */}
                                    {(!p.status || p.status === 'Pending') && (
                                        <div className="flex items-center gap-2 shrink-0 sm:mt-1">
                                            <button
                                                onClick={() => handleStatus(p.id, 'Accepted')}
                                                disabled={updatingId === p.id}
                                                className="flex items-center gap-1.5 px-3 py-1.5 bg-green-100 text-green-700 hover:bg-green-200 rounded-lg text-xs font-semibold transition-colors disabled:opacity-50"
                                            >
                                                {updatingId === p.id ? (
                                                    <Loader2 className="w-3.5 h-3.5 animate-spin" />
                                                ) : (
                                                    <Check className="w-3.5 h-3.5" />
                                                )}
                                                Accept
                                            </button>
                                            <button
                                                onClick={() => handleStatus(p.id, 'Rejected')}
                                                disabled={updatingId === p.id}
                                                className="flex items-center gap-1.5 px-3 py-1.5 bg-red-50 text-red-600 hover:bg-red-100 rounded-lg text-xs font-semibold transition-colors disabled:opacity-50"
                                            >
                                                <XIcon className="w-3.5 h-3.5" />
                                                Decline
                                            </button>
                                        </div>
                                    )}
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            )}
        </div>
    );
};

// ─── My Projects Tab ─────────────────────────────────────────────────────────
const MyProjectsTab = () => {
    const [jobs, setJobs] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState('');

    const fetchJobs = async () => {
        try {
            const data = await projectBoardService.getMyJobs();
            setJobs(data || []);
        } catch (err) {
            setError(err.message || 'Failed to load your projects');
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => { fetchJobs(); }, []);

    if (isLoading) {
        return (
            <div className="flex items-center justify-center py-20 text-textSecondary">
                <Loader2 className="w-5 h-5 animate-spin mr-2" />
                Loading your projects...
            </div>
        );
    }

    if (error) {
        return <div className="text-center py-16 text-error">{error}</div>;
    }

    return (
        <div className="space-y-4">
            <div className="flex items-center justify-between">
                <p className="text-sm text-textSecondary">{jobs.length} project{jobs.length !== 1 ? 's' : ''} posted</p>
                <Link to="/dashboard/buyer/post-project">
                    <Button variant="primary" size="sm" className="flex items-center gap-1.5">
                        <PlusCircle className="w-4 h-4" />
                        Post New Project
                    </Button>
                </Link>
            </div>

            {jobs.length === 0 ? (
                <div className="bg-white rounded-xl border border-border shadow-sm p-12 text-center">
                    <Briefcase className="w-10 h-10 mx-auto mb-3 text-textSecondary opacity-40" />
                    <h3 className="font-bold text-primary mb-1">No projects yet</h3>
                    <p className="text-sm text-textSecondary mb-4">Post your first project to start receiving proposals from artists.</p>
                    <Link to="/dashboard/buyer/post-project">
                        <Button variant="primary">Post a Project</Button>
                    </Link>
                </div>
            ) : (
                jobs.map((job) => (
                    <ProjectCard key={job.id} job={job} onStatusUpdate={fetchJobs} />
                ))
            )}
        </div>
    );
};

// ─── Main BuyerDashboard ─────────────────────────────────────────────────────
const BuyerDashboard = () => {
    const [activeTab, setActiveTab] = useState('orders');

    const tabs = [
        { id: 'orders', label: 'My Orders', icon: ShoppingBag },
        { id: 'projects', label: 'My Projects', icon: Briefcase },
    ];

    return (
        <div className="space-y-6">
            {/* Header */}
            <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3">
                <div>
                    <h1 className="text-2xl font-heading font-bold text-primary">Buyer Dashboard</h1>
                    <p className="text-textSecondary">Manage your orders and creative project commissions</p>
                </div>
                <Link to="/marketplace">
                    <Button variant="secondary" size="sm">Browse Marketplace</Button>
                </Link>
            </div>

            {/* Tabs */}
            <div className="flex gap-1 bg-gray-100 p-1 rounded-xl w-fit">
                {tabs.map((tab) => (
                    <button
                        key={tab.id}
                        onClick={() => setActiveTab(tab.id)}
                        className={`flex items-center gap-2 px-4 py-2 rounded-lg text-sm font-medium transition-all ${
                            activeTab === tab.id
                                ? 'bg-white text-primary shadow-sm'
                                : 'text-textSecondary hover:text-primary'
                        }`}
                    >
                        <tab.icon className="w-4 h-4" />
                        {tab.label}
                    </button>
                ))}
            </div>

            {/* Tab Content */}
            {activeTab === 'orders' && <OrdersTab />}
            {activeTab === 'projects' && <MyProjectsTab />}
        </div>
    );
};

export default BuyerDashboard;
