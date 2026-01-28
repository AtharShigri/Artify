import React, { forwardRef } from 'react';
import { cn } from '../../utils/cn';

const Input = forwardRef(({ label, error, className, ...props }, ref) => {
    return (
        <div className="w-full">
            {label && (
                <label className="block text-sm font-medium text-textSecondary mb-1.5">
                    {label}
                </label>
            )}
            <input
                ref={ref}
                className={cn(
                    'w-full px-4 py-3 rounded-lg border border-border bg-white text-textPrimary placeholder:text-gray-400 focus:outline-none focus:ring-2 focus:ring-secondary/20 focus:border-secondary transition-all duration-200',
                    error && 'border-error focus:ring-error/20 focus:border-error',
                    className
                )}
                {...props}
            />
            {error && <p className="mt-1 text-sm text-error">{error}</p>}
        </div>
    );
});

Input.displayName = 'Input';

export default Input;
