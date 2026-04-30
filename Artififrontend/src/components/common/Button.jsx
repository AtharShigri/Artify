import React from 'react';
import { Loader2 } from 'lucide-react';
import { cn } from '../../utils/cn';

const Button = ({
    children,
    variant = 'primary',
    size = 'md',
    isLoading = false,
    className,
    ...props
}) => {
    const variants = {
        primary: 'bg-primary text-white hover:bg-opacity-90 shadow-md',
        secondary: 'border border-primary text-primary hover:bg-primary/10',
        accent: 'bg-accent text-primary hover:brightness-110 shadow-md',
        ghost: 'hover:bg-gray-100 text-textSecondary',
        danger: 'bg-error text-white hover:bg-red-600',
    };

    const sizes = {
        sm: 'px-3 py-1.5 text-sm',
        md: 'px-6 py-3 text-base',
        lg: 'px-8 py-4 text-lg',
    };

    return (
        <button
            className={cn(
                'inline-flex items-center justify-center rounded-xl font-medium transition-all duration-200 disabled:opacity-50 disabled:cursor-not-allowed',
                variants[variant],
                sizes[size],
                className
            )}
            disabled={isLoading || props.disabled}
            {...props}
        >
            {isLoading && <Loader2 className="w-4 h-4 mr-2 animate-spin" />}
            {children}
        </button>
    );
};

export default Button;
