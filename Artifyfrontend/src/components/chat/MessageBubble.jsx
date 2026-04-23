import React from 'react';
import { clsx } from 'clsx';
import { twMerge } from 'tailwind-merge';
import { AlertCircle } from 'lucide-react';

function cn(...inputs) {
  return twMerge(clsx(inputs));
}

const MessageBubble = ({ message, isMe }) => {
    const { content, isFlagged, timestamp } = message;

    return (
        <div className={cn("flex w-full mb-4", isMe ? "justify-end" : "justify-start")}>
            <div 
                className={cn(
                    "max-w-[70%] rounded-2xl px-4 py-2 relative",
                    isMe ? "bg-primary text-white rounded-br-none" : "bg-gray-100 text-gray-800 rounded-bl-none",
                    isFlagged && "border-2 border-red-500 bg-red-50 text-red-800"
                )}
            >
                {isFlagged && (
                    <div className="flex items-center gap-1 text-xs font-bold text-red-600 mb-1">
                        <AlertCircle size={12} />
                        Flagged Content
                    </div>
                )}
                <p className="text-sm whitespace-pre-wrap word-break">{content}</p>
                <div 
                    className={cn(
                        "text-[10px] mt-1 text-right",
                        isMe ? "text-primary-foreground/80" : "text-gray-500"
                    )}
                >
                    {new Date(timestamp).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                </div>
            </div>
        </div>
    );
};

export default MessageBubble;
