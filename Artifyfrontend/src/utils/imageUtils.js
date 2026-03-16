/**
 * Normalizes an image path into an absolute URL.
 * Handles absolute URLs, blobs, data URIs, and joins relative paths to the API base.
 */
export const getImageUrl = (url) => {
    if (!url) return null;

    // 1. Return if already a full URL, blob, or data URI
    if (/^(https?|blob|data):/i.test(url)) {
        return url;
    }

    // 2. Resolve the Base URL (removes trailing /api or /api/)
    const apiBaseUrl = import.meta.env.VITE_API_URL || 'http://localhost:5181/api';
    const baseUrl = apiBaseUrl.replace(/\/api\/?$/, '');

    // 3. Clean the path and join (removes leading slash to avoid //)
    const cleanPath = url.startsWith('/') ? url.substring(1) : url;

    return `${baseUrl}/${cleanPath}`;
};