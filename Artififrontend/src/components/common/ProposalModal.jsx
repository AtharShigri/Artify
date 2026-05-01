import React, { useState } from 'react';
import { X, Send, DollarSign, FileText } from 'lucide-react';
import Button from './Button';
import { projectBoardService } from '../../services/projectBoardService';

const ProposalModal = ({ job, onClose, onSuccess }) => {
    const [formData, setFormData] = useState({
        jobPostId: job.id,
        bidAmount: '',
        coverLetter: ''
    });
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [error, setError] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setIsSubmitting(true);
        try {
            await projectBoardService.submitProposal({
                ...formData,
                bidAmount: parseFloat(formData.bidAmount)
            });
            onSuccess?.();
            onClose();
        } catch (err) {
            setError(err.message || 'Failed to submit proposal');
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        /* Backdrop */
        <div
            className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 backdrop-blur-sm px-4"
            onClick={(e) => e.target === e.currentTarget && onClose()}
        >
            <div className="w-full max-w-lg bg-white rounded-2xl shadow-2xl border border-border animate-in fade-in zoom-in-95 duration-200">
                {/* Header */}
                <div className="flex items-start justify-between p-6 border-b border-border">
                    <div>
                        <h2 className="text-xl font-bold text-primary">Submit Proposal</h2>
                        <p className="text-sm text-textSecondary mt-0.5 line-clamp-1">{job.title}</p>
                    </div>
                    <button
                        onClick={onClose}
                        className="ml-4 p-1.5 rounded-lg text-textSecondary hover:text-primary hover:bg-gray-100 transition-colors"
                    >
                        <X className="w-5 h-5" />
                    </button>
                </div>

                {/* Body */}
                <form onSubmit={handleSubmit} className="p-6 space-y-5">
                    {error && (
                        <div className="p-3 bg-red-50 text-error text-sm rounded-lg border border-red-100">
                            {error}
                        </div>
                    )}

                    {/* Budget context */}
                    <div className="flex items-center gap-2 p-3 bg-secondary/5 rounded-lg border border-secondary/20">
                        <DollarSign className="w-4 h-4 text-secondary shrink-0" />
                        <span className="text-sm text-textSecondary">
                            Client's budget: <strong className="text-primary">${job.budget?.toLocaleString()}</strong>
                        </span>
                    </div>

                    {/* Bid Amount */}
                    <div>
                        <label className="block text-sm font-medium text-textSecondary mb-1.5">
                            Your Bid Amount (USD) <span className="text-error">*</span>
                        </label>
                        <div className="relative">
                            <span className="absolute left-3 top-1/2 -translate-y-1/2 text-textSecondary">$</span>
                            <input
                                type="number"
                                min="1"
                                step="0.01"
                                required
                                value={formData.bidAmount}
                                onChange={(e) => setFormData({ ...formData, bidAmount: e.target.value })}
                                className="w-full pl-8 pr-4 py-3 rounded-lg border border-border focus:outline-none focus:ring-2 focus:ring-secondary/20 focus:border-secondary transition-all"
                                placeholder="0.00"
                            />
                        </div>
                    </div>

                    {/* Cover Letter */}
                    <div>
                        <label className="block text-sm font-medium text-textSecondary mb-1.5">
                            <span className="flex items-center gap-1.5">
                                <FileText className="w-4 h-4" />
                                Cover Letter <span className="text-error">*</span>
                            </span>
                        </label>
                        <textarea
                            required
                            rows={5}
                            value={formData.coverLetter}
                            onChange={(e) => setFormData({ ...formData, coverLetter: e.target.value })}
                            className="w-full px-4 py-3 rounded-lg border border-border focus:outline-none focus:ring-2 focus:ring-secondary/20 focus:border-secondary transition-all resize-none"
                            placeholder="Introduce yourself, explain your approach, and why you're a great fit for this project..."
                        />
                        <p className="mt-1 text-xs text-textSecondary">{formData.coverLetter.length} / 1000 characters</p>
                    </div>
                </form>

                {/* Footer */}
                <div className="flex items-center justify-end gap-3 px-6 pb-6">
                    <Button type="button" variant="ghost" onClick={onClose}>
                        Cancel
                    </Button>
                    <Button
                        type="submit"
                        variant="primary"
                        isLoading={isSubmitting}
                        onClick={handleSubmit}
                        className="flex items-center gap-2"
                    >
                        <Send className="w-4 h-4" />
                        Send Proposal
                    </Button>
                </div>
            </div>
        </div>
    );
};

export default ProposalModal;
