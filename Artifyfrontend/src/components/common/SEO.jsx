import React, { useEffect } from 'react';
import { useLocation } from 'react-router-dom';

const SEO = ({ title, description }) => {
    const location = useLocation();

    useEffect(() => {
        // Update Title
        document.title = title ? `${title} | Artify` : 'Artify - Premier Art Marketplace';

        // Update Meta Description
        const metaDesc = document.querySelector('meta[name="description"]');
        if (metaDesc) {
            metaDesc.setAttribute('content', description || 'Discover and buy unique artwork from talented artists around the world.');
        } else {
            const meta = document.createElement('meta');
            meta.name = "description";
            meta.content = description || 'Discover and buy unique artwork from talented artists around the world.';
            document.head.appendChild(meta);
        }

    }, [title, description, location]);

    return null;
};

export default SEO;
