import api from '../api/axios';

// ── Watermark ─────────────────────────────────────────────────────────────────
const applyWatermark = async (file) => {
    const formData = new FormData();
    formData.append('file', file);
    const res = await api.post('/artist/protection/watermark', formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
    });
    return res.data;
};

// ── Metadata ──────────────────────────────────────────────────────────────────
const embedMetadata = async ({ artworkId, artistName, copyrightText, description }) => {
    const res = await api.post('/artist/protection/metadata', {
        artworkId,
        artistName,
        copyrightText,
        description
    });
    return res.data;
};

// ── Hash Registration ─────────────────────────────────────────────────────────
const generateHash = async (artworkId) => {
    const res = await api.post('/artist/protection/hash', { artworkId });
    return res.data;
};

// ── Plagiarism Check ──────────────────────────────────────────────────────────
const checkPlagiarism = async (file) => {
    const formData = new FormData();
    formData.append('file', file);
    const res = await api.post('/artist/protection/plagiarism-check', formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
    });
    return res.data;
};

// ── Protection Status ─────────────────────────────────────────────────────────
const getProtectionStatus = async (artworkId) => {
    const res = await api.get(`/artist/protection/status/${artworkId}`);
    return res.data;
};

export const protectionService = {
    applyWatermark,
    embedMetadata,
    generateHash,
    checkPlagiarism,
    getProtectionStatus
};
