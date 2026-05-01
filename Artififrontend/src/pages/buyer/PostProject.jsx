import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { PlusCircle, DollarSign, FileText, Type, CheckCircle } from 'lucide-react';
import SEO from '../../components/common/SEO';
import Button from '../../components/common/Button';
import Input from '../../components/common/Input';
import { projectBoardService } from '../../services/projectBoardService';

const PostProject = () => {
    const [formData, setFormData] = useState({
        title: '',
        description: '',
        budget: ''
    });
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState(false);
    const navigate = useNavigate();

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setIsLoading(true);

        try {
            await projectBoardService.postJob({
                title: formData.title,
                description: formData.description,
                budget: parseFloat(formData.budget)
            });
            setSuccess(true);
            setTimeout(() => navigate('/dashboard/buyer/projects'), 2000);
        } catch (err) {
            setError(err.message || 'Failed to post project');
        } finally {
            setIsLoading(false);
        }
    };

    if (success) {
        return (
            <div className="flex flex-col items-center justify-center min-h-[50vh] text-center space-y-4">
                <div className="w-16 h-16 bg-green-100 rounded-full flex items-center justify-center">
                    <CheckCircle className="w-8 h-8 text-green-600" />
                </div>
                <h2 className="text-2xl font-bold text-primary">Project Posted!</h2>
                <p className="text-textSecondary">Artists can now discover and submit proposals for your project.</p>
                <p className="text-sm text-textSecondary">Redirecting to My Projects...</p>
            </div>
        );
    }

    return (
        <div className="max-w-2xl mx-auto">
            <SEO title="Post a Project" description="Post a creative project and receive proposals from talented artists." />

            {/* Header */}
            <div className="mb-8">
                <div className="flex items-center gap-3 mb-2">
                    <div className="w-10 h-10 bg-secondary/10 rounded-xl flex items-center justify-center">
                        <PlusCircle className="w-5 h-5 text-secondary" />
                    </div>
                    <h1 className="text-2xl font-heading font-bold text-primary">Post a Project</h1>
                </div>
                <p className="text-textSecondary">Describe your creative project and artists will reach out with proposals.</p>
            </div>

            {/* Form */}
            <div className="bg-white rounded-2xl border border-border shadow-sm p-8">
                {error && (
                    <div className="mb-6 p-4 bg-red-50 text-error text-sm rounded-xl border border-red-100">
                        {error}
                    </div>
                )}

                <form onSubmit={handleSubmit} className="space-y-6">
                    {/* Title */}
                    <div>
                        <label className="block text-sm font-medium text-textSecondary mb-1.5">
                            <span className="flex items-center gap-1.5">
                                <Type className="w-4 h-4" />
                                Project Title <span className="text-error">*</span>
                            </span>
                        </label>
                        <input
                            type="text"
                            name="title"
                            value={formData.title}
                            onChange={handleChange}
                            required
                            maxLength={120}
                            placeholder="e.g. Book cover illustration for a sci-fi novel"
                            className="w-full px-4 py-3 rounded-xl border border-border focus:outline-none focus:ring-2 focus:ring-secondary/20 focus:border-secondary transition-all"
                        />
                    </div>

                    {/* Description */}
                    <div>
                        <label className="block text-sm font-medium text-textSecondary mb-1.5">
                            <span className="flex items-center gap-1.5">
                                <FileText className="w-4 h-4" />
                                Project Description <span className="text-error">*</span>
                            </span>
                        </label>
                        <textarea
                            name="description"
                            value={formData.description}
                            onChange={handleChange}
                            required
                            rows={6}
                            placeholder="Describe your project in detail: style, mood, references, deliverables, timeline..."
                            className="w-full px-4 py-3 rounded-xl border border-border focus:outline-none focus:ring-2 focus:ring-secondary/20 focus:border-secondary transition-all resize-none"
                        />
                        <p className="mt-1 text-xs text-textSecondary">{formData.description.length} characters</p>
                    </div>

                    {/* Budget */}
                    <div>
                        <label className="block text-sm font-medium text-textSecondary mb-1.5">
                            <span className="flex items-center gap-1.5">
                                <DollarSign className="w-4 h-4" />
                                Budget (PKR) <span className="text-error">*</span>
                            </span>
                        </label>
                        <div className="relative">
                            <span className="absolute left-3 top-1/2 -translate-y-1/2 text-textSecondary font-medium text-xs">PKR</span>
                            <input
                                type="number"
                                name="budget"
                                value={formData.budget}
                                onChange={handleChange}
                                required
                                min="1"
                                step="0.01"
                                placeholder="500.00"
                                className="w-full pl-12 pr-4 py-3 rounded-xl border border-border focus:outline-none focus:ring-2 focus:ring-secondary/20 focus:border-secondary transition-all"
                            />
                        </div>
                        <p className="mt-1 text-xs text-textSecondary">Artists will use this as a reference when submitting proposals.</p>
                    </div>

                    <div className="flex items-center gap-3 pt-2">
                        <Button
                            type="button"
                            variant="ghost"
                            onClick={() => navigate(-1)}
                        >
                            Cancel
                        </Button>
                        <Button
                            type="submit"
                            variant="primary"
                            isLoading={isLoading}
                            className="flex-1"
                        >
                            Post Project →
                        </Button>
                    </div>
                </form>
            </div>

            {/* Tips */}
            <div className="mt-6 p-4 bg-secondary/5 rounded-xl border border-secondary/20">
                <p className="text-sm font-semibold text-primary mb-2">💡 Tips for a great project post</p>
                <ul className="text-xs text-textSecondary space-y-1 list-disc list-inside">
                    <li>Be specific about the style, medium, and final format you need.</li>
                    <li>Include references or examples if you have them.</li>
                    <li>Set a realistic budget — higher budgets attract more experienced artists.</li>
                    <li>Mention your timeline or deadline clearly.</li>
                </ul>
            </div>
        </div>
    );
};

export default PostProject;
