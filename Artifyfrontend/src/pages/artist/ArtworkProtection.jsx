import React, { useState, useCallback, useEffect } from 'react';
import { Shield, Droplets, Tag, Hash, Search, Upload, CheckCircle, XCircle, AlertTriangle, Loader2, Download, RefreshCw } from 'lucide-react';
import { protectionService } from '../../services/protectionService';
import artworkService from '../../services/artworkService';

// ── Reusable DropZone ─────────────────────────────────────────────────────────
const DropZone = ({ onFile, accept = 'image/*', label = 'Drop image here or click to browse', file }) => {
    const [dragging, setDragging] = useState(false);

    const handleDrop = (e) => {
        e.preventDefault();
        setDragging(false);
        const dropped = e.dataTransfer.files[0];
        if (dropped) onFile(dropped);
    };

    return (
        <label
            onDragOver={(e) => { e.preventDefault(); setDragging(true); }}
            onDragLeave={() => setDragging(false)}
            onDrop={handleDrop}
            className={`flex flex-col items-center justify-center border-2 border-dashed rounded-2xl p-8 cursor-pointer transition-all ${
                dragging ? 'border-secondary bg-secondary/5' : 'border-border hover:border-secondary/60 hover:bg-gray-50'
            }`}
        >
            <Upload className={`w-8 h-8 mb-3 ${dragging ? 'text-secondary' : 'text-textSecondary opacity-40'}`} />
            <p className="text-sm text-textSecondary text-center">{file ? `📎 ${file.name}` : label}</p>
            <input type="file" accept={accept} className="hidden" onChange={(e) => e.target.files?.[0] && onFile(e.target.files[0])} />
        </label>
    );
};

// ── Status Badge ──────────────────────────────────────────────────────────────
const StatusBadge = ({ ok, label }) => (
    <span className={`inline-flex items-center gap-1.5 text-xs font-medium px-2.5 py-1 rounded-full ${
        ok ? 'bg-green-100 text-green-700' : 'bg-gray-100 text-gray-500'
    }`}>
        {ok ? <CheckCircle className="w-3.5 h-3.5" /> : <XCircle className="w-3.5 h-3.5" />}
        {label}
    </span>
);

// ── Result Box ────────────────────────────────────────────────────────────────
const ResultBox = ({ type = 'success', children }) => {
    const styles = {
        success: 'bg-green-50 border-green-200 text-green-800',
        error: 'bg-red-50 border-red-200 text-red-800',
        warning: 'bg-amber-50 border-amber-200 text-amber-800',
        info: 'bg-blue-50 border-blue-200 text-blue-800',
    };
    return (
        <div className={`p-4 rounded-xl border text-sm mt-4 ${styles[type]}`}>
            {children}
        </div>
    );
};

// ═══════════════════════════════════════════════════════════════════════════════
// TAB 1 — Watermark
// ═══════════════════════════════════════════════════════════════════════════════
const WatermarkTab = () => {
    const [file, setFile] = useState(null);
    const [loading, setLoading] = useState(false);
    const [result, setResult] = useState(null);
    const [error, setError] = useState('');
    const baseURL = import.meta.env.VITE_API_URL?.replace('/api', '') || 'http://localhost:5181';

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!file) return;
        setLoading(true);
        setError('');
        setResult(null);
        try {
            const data = await protectionService.applyWatermark(file);
            setResult(data);
        } catch (err) {
            setError(err.response?.data?.message || 'Failed to apply watermark.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="space-y-6">
            <div>
                <h2 className="text-xl font-bold text-primary">Apply Watermark</h2>
                <p className="text-sm text-textSecondary mt-1">
                    Add a visible copyright watermark with your artist name across the image. The original file is unchanged.
                </p>
            </div>

            <form onSubmit={handleSubmit} className="space-y-4">
                <DropZone file={file} onFile={setFile} label="Drop your artwork image here, or click to browse" />
                {file && (
                    <div className="flex items-center gap-3">
                        <img
                            src={URL.createObjectURL(file)}
                            alt="Preview"
                            className="w-24 h-24 object-cover rounded-xl border border-border"
                        />
                        <div className="text-sm text-textSecondary">
                            <p className="font-medium text-primary">{file.name}</p>
                            <p>{(file.size / 1024).toFixed(1)} KB</p>
                        </div>
                    </div>
                )}
                <button
                    type="submit"
                    disabled={!file || loading}
                    className="btn-primary flex items-center gap-2 px-6 py-2.5 rounded-xl disabled:opacity-50"
                >
                    {loading ? <Loader2 className="w-4 h-4 animate-spin" /> : <Droplets className="w-4 h-4" />}
                    {loading ? 'Applying Watermark...' : 'Apply Watermark'}
                </button>
            </form>

            {error && <ResultBox type="error">⚠️ {error}</ResultBox>}

            {result?.success && (
                <ResultBox type="success">
                    <p className="font-semibold mb-2">✅ Watermark applied successfully!</p>
                    <img
                        src={`${baseURL}${result.watermarkedUrl}`}
                        alt="Watermarked"
                        className="w-full rounded-xl border border-green-200 mt-2 max-h-64 object-contain"
                    />
                    <a
                        href={`${baseURL}${result.watermarkedUrl}`}
                        download
                        className="inline-flex items-center gap-2 mt-3 text-sm font-medium text-green-700 hover:underline"
                    >
                        <Download className="w-4 h-4" /> Download Watermarked Image
                    </a>
                </ResultBox>
            )}
        </div>
    );
};

// ═══════════════════════════════════════════════════════════════════════════════
// TAB 2 — Metadata
// ═══════════════════════════════════════════════════════════════════════════════
const MetadataTab = ({ artworks }) => {
    const [form, setForm] = useState({ artworkId: '', artistName: '', copyrightText: '', description: '' });
    const [loading, setLoading] = useState(false);
    const [result, setResult] = useState(null);
    const [error, setError] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError('');
        setResult(null);
        try {
            const data = await protectionService.embedMetadata(form);
            setResult(data);
        } catch (err) {
            setError(err.response?.data?.message || 'Failed to embed metadata.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="space-y-6">
            <div>
                <h2 className="text-xl font-bold text-primary">Embed Copyright Metadata</h2>
                <p className="text-sm text-textSecondary mt-1">
                    Attach your name, copyright notice, and description to an artwork record. This creates a provenance trail.
                </p>
            </div>

            <form onSubmit={handleSubmit} className="space-y-4">
                <div>
                    <label className="text-sm font-semibold text-primary block mb-1.5">Select Artwork</label>
                    <select
                        value={form.artworkId}
                        onChange={(e) => setForm({ ...form, artworkId: e.target.value })}
                        required
                        className="w-full px-4 py-3 rounded-xl border border-border focus:border-secondary focus:ring-1 focus:ring-secondary outline-none"
                    >
                        <option value="">— Choose an artwork —</option>
                        {artworks.map(a => (
                            <option key={a.artworkId} value={a.artworkId}>{a.title}</option>
                        ))}
                    </select>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <div>
                        <label className="text-sm font-semibold text-primary block mb-1.5">Artist Name</label>
                        <input
                            type="text"
                            value={form.artistName}
                            onChange={(e) => setForm({ ...form, artistName: e.target.value })}
                            placeholder="Your full name"
                            required
                            className="w-full px-4 py-3 rounded-xl border border-border focus:border-secondary focus:ring-1 focus:ring-secondary outline-none"
                        />
                    </div>
                    <div>
                        <label className="text-sm font-semibold text-primary block mb-1.5">Copyright Notice</label>
                        <input
                            type="text"
                            value={form.copyrightText}
                            onChange={(e) => setForm({ ...form, copyrightText: e.target.value })}
                            placeholder="© 2026 Your Name. All rights reserved."
                            required
                            className="w-full px-4 py-3 rounded-xl border border-border focus:border-secondary focus:ring-1 focus:ring-secondary outline-none"
                        />
                    </div>
                </div>

                <div>
                    <label className="text-sm font-semibold text-primary block mb-1.5">Description</label>
                    <textarea
                        value={form.description}
                        onChange={(e) => setForm({ ...form, description: e.target.value })}
                        rows={3}
                        placeholder="Brief description of the artwork and its origin..."
                        className="w-full px-4 py-3 rounded-xl border border-border focus:border-secondary focus:ring-1 focus:ring-secondary outline-none resize-none"
                    />
                </div>

                <button
                    type="submit"
                    disabled={!form.artworkId || !form.artistName || loading}
                    className="btn-primary flex items-center gap-2 px-6 py-2.5 rounded-xl disabled:opacity-50"
                >
                    {loading ? <Loader2 className="w-4 h-4 animate-spin" /> : <Tag className="w-4 h-4" />}
                    {loading ? 'Embedding...' : 'Embed Metadata'}
                </button>
            </form>

            {error && <ResultBox type="error">⚠️ {error}</ResultBox>}
            {result?.success && (
                <ResultBox type="success">
                    ✅ Metadata embedded for <strong>{result.artworkId}</strong>
                    <br />Copyright: <em>{result.copyrightText}</em>
                </ResultBox>
            )}
        </div>
    );
};

// ═══════════════════════════════════════════════════════════════════════════════
// TAB 3 — Hash & Register
// ═══════════════════════════════════════════════════════════════════════════════
const HashTab = ({ artworks }) => {
    const [artworkId, setArtworkId] = useState('');
    const [loading, setLoading] = useState(false);
    const [result, setResult] = useState(null);
    const [error, setError] = useState('');

    const handleGenerate = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError('');
        setResult(null);
        try {
            const data = await protectionService.generateHash(artworkId);
            setResult(data);
        } catch (err) {
            setError(err.response?.data?.message || 'Failed to generate hash. Make sure the artwork has an uploaded image.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="space-y-6">
            <div>
                <h2 className="text-xl font-bold text-primary">Hash & Register Artwork</h2>
                <p className="text-sm text-textSecondary mt-1">
                    Creates a unique digital fingerprint (SHA-256 + perceptual hash) for your artwork. Used to detect copies later.
                </p>
            </div>

            <div className="bg-blue-50 border border-blue-200 rounded-xl p-4 text-sm text-blue-700">
                <strong>How it works:</strong> We compute two hashes:
                <ul className="list-disc list-inside mt-1 space-y-0.5">
                    <li><strong>SHA-256</strong> — exact byte-level fingerprint. Detects identical copies.</li>
                    <li><strong>pHash</strong> — perceptual fingerprint. Detects near-identical images (resized, compressed, colour-shifted).</li>
                </ul>
            </div>

            <form onSubmit={handleGenerate} className="space-y-4">
                <div>
                    <label className="text-sm font-semibold text-primary block mb-1.5">Select Artwork to Register</label>
                    <select
                        value={artworkId}
                        onChange={(e) => setArtworkId(e.target.value)}
                        required
                        className="w-full px-4 py-3 rounded-xl border border-border focus:border-secondary focus:ring-1 focus:ring-secondary outline-none"
                    >
                        <option value="">— Choose an artwork —</option>
                        {artworks.map(a => (
                            <option key={a.artworkId} value={a.artworkId}>{a.title}</option>
                        ))}
                    </select>
                </div>

                <button
                    type="submit"
                    disabled={!artworkId || loading}
                    className="btn-primary flex items-center gap-2 px-6 py-2.5 rounded-xl disabled:opacity-50"
                >
                    {loading ? <Loader2 className="w-4 h-4 animate-spin" /> : <Hash className="w-4 h-4" />}
                    {loading ? 'Generating...' : 'Generate & Register Hash'}
                </button>
            </form>

            {error && <ResultBox type="error">⚠️ {error}</ResultBox>}

            {result?.success && (
                <ResultBox type="success">
                    <p className="font-semibold mb-3">✅ Artwork fingerprint registered!</p>
                    <div className="space-y-2 font-mono text-xs">
                        <div className="bg-white/60 rounded-lg p-2">
                            <span className="text-gray-500 block mb-0.5">SHA-256 (exact)</span>
                            <span className="text-green-800 break-all">{result.sha256Hash}</span>
                        </div>
                        <div className="bg-white/60 rounded-lg p-2">
                            <span className="text-gray-500 block mb-0.5">pHash (perceptual)</span>
                            <span className="text-green-800 break-all">{result.perceptualHash}</span>
                        </div>
                        <div className="bg-white/60 rounded-lg p-2 font-sans text-xs text-gray-500">
                            Registered at: {new Date(result.registeredAt).toLocaleString()}
                        </div>
                    </div>
                </ResultBox>
            )}
        </div>
    );
};

// ═══════════════════════════════════════════════════════════════════════════════
// TAB 4 — Plagiarism Check
// ═══════════════════════════════════════════════════════════════════════════════
const PlagiarismTab = () => {
    const [file, setFile] = useState(null);
    const [loading, setLoading] = useState(false);
    const [result, setResult] = useState(null);
    const [error, setError] = useState('');

    const handleCheck = async (e) => {
        e.preventDefault();
        if (!file) return;
        setLoading(true);
        setError('');
        setResult(null);
        try {
            const data = await protectionService.checkPlagiarism(file);
            setResult(data);
        } catch (err) {
            setError(err.response?.data?.message || 'Plagiarism check failed.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="space-y-6">
            <div>
                <h2 className="text-xl font-bold text-primary">Plagiarism Check</h2>
                <p className="text-sm text-textSecondary mt-1">
                    Upload any image to check if it matches registered artworks in the Artify database. Uses perceptual hash comparison.
                </p>
            </div>

            <form onSubmit={handleCheck} className="space-y-4">
                <DropZone file={file} onFile={setFile} label="Drop the image to check for plagiarism" />

                {file && (
                    <div className="flex items-center gap-3">
                        <img src={URL.createObjectURL(file)} alt="Preview" className="w-20 h-20 object-cover rounded-xl border border-border" />
                        <span className="text-sm text-textSecondary">{file.name}</span>
                    </div>
                )}

                <button
                    type="submit"
                    disabled={!file || loading}
                    className="btn-primary flex items-center gap-2 px-6 py-2.5 rounded-xl disabled:opacity-50"
                >
                    {loading ? <Loader2 className="w-4 h-4 animate-spin" /> : <Search className="w-4 h-4" />}
                    {loading ? 'Scanning database...' : 'Run Plagiarism Check'}
                </button>
            </form>

            {error && <ResultBox type="error">⚠️ {error}</ResultBox>}

            {result && (
                <div className={`p-4 rounded-xl border text-sm mt-4 ${
                    result.plagiarismDetected ? 'bg-red-50 border-red-200' : 'bg-green-50 border-green-200'
                }`}>
                    <div className="flex items-center gap-2 mb-3">
                        {result.plagiarismDetected
                            ? <AlertTriangle className="w-5 h-5 text-red-600" />
                            : <CheckCircle className="w-5 h-5 text-green-600" />
                        }
                        <span className={`font-semibold ${result.plagiarismDetected ? 'text-red-700' : 'text-green-700'}`}>
                            {result.summary}
                        </span>
                    </div>

                    {result.matches?.length > 0 && (
                        <div className="space-y-2 mt-3">
                            <p className="font-medium text-red-700 mb-2">Matched artworks:</p>
                            {result.matches.map((match, i) => (
                                <div key={i} className="bg-white/70 rounded-lg p-3 border border-red-100">
                                    <div className="flex items-center justify-between">
                                        <div>
                                            <p className="font-semibold text-primary text-sm">{match.matchedArtworkTitle}</p>
                                            <p className="text-xs text-textSecondary">by {match.matchedArtistName}</p>
                                        </div>
                                        <div className="text-right">
                                            <span className={`text-sm font-bold ${
                                                match.similarityPercent >= 95 ? 'text-red-600' : 'text-amber-600'
                                            }`}>
                                                {match.similarityPercent.toFixed(1)}%
                                            </span>
                                            {match.isExactMatch && (
                                                <p className="text-xs text-red-600 font-semibold">Exact Match</p>
                                            )}
                                        </div>
                                    </div>
                                    {/* Similarity bar */}
                                    <div className="mt-2 h-1.5 bg-red-100 rounded-full overflow-hidden">
                                        <div
                                            className="h-full bg-red-500 rounded-full transition-all"
                                            style={{ width: `${match.similarityPercent}%` }}
                                        />
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}

                    <div className="mt-3 pt-3 border-t border-current/10 text-xs opacity-60 font-mono">
                        SHA-256: {result.uploadedFileHash?.substring(0, 32)}...
                    </div>
                </div>
            )}
        </div>
    );
};

// ═══════════════════════════════════════════════════════════════════════════════
// MAIN PAGE
// ═══════════════════════════════════════════════════════════════════════════════
const tabs = [
    { id: 'watermark', label: 'Watermark', icon: Droplets },
    { id: 'metadata', label: 'Metadata', icon: Tag },
    { id: 'hash', label: 'Hash & Register', icon: Hash },
    { id: 'plagiarism', label: 'Plagiarism Check', icon: Search },
];

const ArtworkProtection = () => {
    const [activeTab, setActiveTab] = useState('watermark');
    const [artworks, setArtworks] = useState([]);
    const [loadingArtworks, setLoadingArtworks] = useState(true);

    useEffect(() => {
        const load = async () => {
            try {
                // Use the artist's artwork list — adjust service call if needed
                const data = await artworkService.getAllByArtist();
                setArtworks(data || []);
            } catch (err) {
                console.error('Could not load artworks:', err);
            } finally {
                setLoadingArtworks(false);
            }
        };
        load();
    }, []);

    return (
        <div className="max-w-4xl mx-auto py-8 px-4">
            {/* Header */}
            <div className="flex items-center gap-3 mb-8">
                <div className="w-12 h-12 rounded-2xl bg-primary/10 flex items-center justify-center">
                    <Shield className="w-6 h-6 text-primary" />
                </div>
                <div>
                    <h1 className="text-3xl font-heading font-bold text-primary">Artwork Protection</h1>
                    <p className="text-textSecondary text-sm mt-0.5">
                        Watermark, fingerprint, and verify the authenticity of your artwork
                    </p>
                </div>
            </div>

            {/* Stats / Summary Row */}
            <div className="grid grid-cols-2 md:grid-cols-4 gap-3 mb-8">
                {[
                    { icon: Droplets, label: 'Watermark images', color: 'text-blue-500', bg: 'bg-blue-50' },
                    { icon: Tag, label: 'Embed metadata', color: 'text-purple-500', bg: 'bg-purple-50' },
                    { icon: Hash, label: 'Register fingerprint', color: 'text-green-500', bg: 'bg-green-50' },
                    { icon: Search, label: 'Detect plagiarism', color: 'text-red-500', bg: 'bg-red-50' },
                ].map((item) => (
                    <div key={item.label} className={`${item.bg} rounded-xl p-4 flex flex-col items-center text-center gap-2`}>
                        <item.icon className={`w-6 h-6 ${item.color}`} />
                        <p className="text-xs text-textSecondary font-medium leading-tight">{item.label}</p>
                    </div>
                ))}
            </div>

            {/* Tab Switcher + Panel */}
            <div className="flex flex-col md:flex-row gap-6">
                {/* Sidebar Tabs */}
                <div className="flex md:flex-col gap-2 md:w-48 shrink-0">
                    {tabs.map(tab => (
                        <button
                            key={tab.id}
                            onClick={() => setActiveTab(tab.id)}
                            className={`flex items-center gap-2.5 px-4 py-3 rounded-xl text-sm font-medium transition-all text-left ${
                                activeTab === tab.id
                                    ? 'bg-primary text-white shadow-md'
                                    : 'text-textSecondary hover:bg-gray-100 hover:text-primary'
                            }`}
                        >
                            <tab.icon className="w-4 h-4 shrink-0" />
                            {tab.label}
                        </button>
                    ))}
                </div>

                {/* Panel */}
                <div className="flex-1 bg-white rounded-2xl border border-border p-6 shadow-sm min-h-[400px]">
                    {activeTab === 'watermark' && <WatermarkTab />}
                    {activeTab === 'metadata' && <MetadataTab artworks={artworks} />}
                    {activeTab === 'hash' && <HashTab artworks={artworks} />}
                    {activeTab === 'plagiarism' && <PlagiarismTab />}
                </div>
            </div>
        </div>
    );
};

export default ArtworkProtection;
