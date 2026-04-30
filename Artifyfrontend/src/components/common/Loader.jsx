import React from 'react';
import { Loader2 } from 'lucide-react';

const Loader = ({ fullScreen = false }) => {
    if (fullScreen) {
        return (
            <div className="fixed inset-0 z-50 flex items-center justify-center bg-white/80 backdrop-blur-sm">
                <div className="flex flex-col items-center gap-4">
                    <img src="/images/logo/artifilogo.png" alt="artifi logo" className="h-8 w-auto mb-2 animate-pulse" />
                    <Loader2 className="w-8 h-8 text-secondary animate-spin" />
                    <p className="text-xs font-medium text-textSecondary uppercase tracking-widest">Loading artifi...</p>
                </div>
            </div>
        );
    }

    return (
        <div className="flex items-center justify-center p-4">
            <Loader2 className="w-6 h-6 text-secondary animate-spin" />
        </div>
    );
};

export default Loader;
