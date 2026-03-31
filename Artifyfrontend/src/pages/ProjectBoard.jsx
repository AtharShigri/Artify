import React, { useState, useEffect } from 'react';
import { Briefcase, DollarSign, User, Search, Loader2 } from 'lucide-react';
import SEO from '../components/common/SEO';
import Button from '../components/common/Button';
import ProposalModal from '../components/common/ProposalModal';
import { projectBoardService } from '../services/projectBoardService';
import { useAuth } from '../context/AuthContext';

const ProjectBoard = () => {
    const [jobs, setJobs] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState('');
    const [search, setSearch] = useState('');
    const [selectedJob, setSelectedJob] = useState(null);
    const [successMsg, setSuccessMsg] = useState('');
    const { isArtist, isAuthenticated } = useAuth();

    useEffect(() => {
        const fetchJobs = async () => {
            try {
                const data = await projectBoardService.browseJobs();
                setJobs(data || []);
            } catch (err) {
                setError(err.message || 'Failed to load projects');
            } finally {
                setIsLoading(false);
            }
        };
        fetchJobs();
    }, []);

    const handleProposalSuccess = () => {
        setSuccessMsg('Your proposal was submitted successfully!');
        setTimeout(() => setSuccessMsg(''), 4000);
    };

    const filtered = jobs.filter((j) =>
        j.title?.toLowerCase().includes(search.toLowerCase()) ||
        j.description?.toLowerCase().includes(search.toLowerCase())
    );

    return (
        <div className="max-w-5xl mx-auto px-4 sm:px-6 py-10">
            <SEO title="Project Board" description="Browse open creative projects and submit your proposal on Artify." />

            {/* Header */}
            <div className="mb-8">
                <h1 className="text-3xl font-heading font-bold text-primary mb-2">Project Board</h1>
                <p className="text-textSecondary">Find creative projects that match your skills and submit a proposal.</p>
            </div>

            {/* Search */}
            <div className="relative mb-6">
                <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-textSecondary" />
                <input
                    type="text"
                    placeholder="Search projects by title or description..."
                    value={search}
                    onChange={(e) => setSearch(e.target.value)}
                    className="w-full pl-10 pr-4 py-3 rounded-xl border border-border focus:outline-none focus:ring-2 focus:ring-secondary/20 focus:border-secondary transition-all bg-white shadow-sm"
                />
            </div>

            {/* Success Toast */}
            {successMsg && (
                <div className="mb-6 p-4 bg-green-50 text-green-700 text-sm rounded-xl border border-green-200 font-medium">
                    ✓ {successMsg}
                </div>
            )}

            {/* State: Loading */}
            {isLoading && (
                <div className="flex items-center justify-center py-20 text-textSecondary">
                    <Loader2 className="w-6 h-6 animate-spin mr-2" />
                    Loading projects...
                </div>
            )}

            {/* State: Error */}
            {!isLoading && error && (
                <div className="text-center py-16 text-error">{error}</div>
            )}

            {/* State: Empty */}
            {!isLoading && !error && filtered.length === 0 && (
                <div className="text-center py-16 text-textSecondary">
                    <Briefcase className="w-12 h-12 mx-auto mb-3 opacity-30" />
                    <p className="font-medium">No projects found</p>
                    <p className="text-sm mt-1">Try a different search or check back later.</p>
                </div>
            )}

            {/* Job Cards Grid */}
            {!isLoading && !error && filtered.length > 0 && (
                <div className="grid gap-4">
                    {filtered.map((job) => (
                        <div
                            key={job.id}
                            className="bg-white rounded-2xl border border-border shadow-sm hover:shadow-md transition-shadow p-6 flex flex-col sm:flex-row sm:items-start sm:justify-between gap-4"
                        >
                            <div className="flex-1 min-w-0">
                                <h2 className="text-lg font-bold text-primary mb-1">{job.title}</h2>
                                <p className="text-textSecondary text-sm leading-relaxed line-clamp-3 mb-3">
                                    {job.description}
                                </p>
                                <div className="flex flex-wrap items-center gap-4 text-sm">
                                    <span className="flex items-center gap-1.5 text-green-600 font-semibold">
                                        <DollarSign className="w-4 h-4" />
                                        Budget: ${job.budget?.toLocaleString()}
                                    </span>
                                    <span className="flex items-center gap-1.5 text-textSecondary">
                                        <User className="w-4 h-4" />
                                        {job.posterName || 'Anonymous'}
                                    </span>
                                </div>
                            </div>

                            <div className="shrink-0">
                                {isArtist && isAuthenticated ? (
                                    <Button
                                        variant="primary"
                                        size="sm"
                                        onClick={() => setSelectedJob(job)}
                                    >
                                        Submit Proposal
                                    </Button>
                                ) : !isAuthenticated ? (
                                    <Button variant="secondary" size="sm" as="a" href="/login">
                                        Login to Apply
                                    </Button>
                                ) : null}
                            </div>
                        </div>
                    ))}
                </div>
            )}

            {/* Proposal Modal */}
            {selectedJob && (
                <ProposalModal
                    job={selectedJob}
                    onClose={() => setSelectedJob(null)}
                    onSuccess={handleProposalSuccess}
                />
            )}
        </div>
    );
};

export default ProjectBoard;
