import React, { useState, useEffect, useRef, useCallback } from 'react';
import * as signalR from '@microsoft/signalr';
import { Send, AlertTriangle, User } from 'lucide-react';
import { chatService } from '../../services/chatService';
import MessageBubble from './MessageBubble';

const ChatLayout = ({ conversationId, currentUser, conversationName }) => {
    const [messages, setMessages] = useState([]);
    const [newMessage, setNewMessage] = useState('');
    const [warningBanner, setWarningBanner] = useState(null);
    const [loading, setLoading] = useState(false);
    const [isConnected, setIsConnected] = useState(false);
    const [isSending, setIsSending] = useState(false);

    const messagesEndRef = useRef(null);
    const connectionRef = useRef(null);
    const inputRef = useRef(null);

    const scrollToBottom = () => {
        messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
    };

    // ── Setup: fetch history + create SignalR connection ─────────────────────
    useEffect(() => {
        if (!conversationId) return;

        setMessages([]);
        setIsConnected(false);
        setWarningBanner(null);

        // 1. Fetch chat history
        const fetchHistory = async () => {
            setLoading(true);
            try {
                const history = await chatService.getChatHistory(conversationId);
                setMessages(history || []);
            } catch (err) {
                console.error('Could not fetch history:', err);
            } finally {
                setLoading(false);
            }
        };
        fetchHistory();

        // 2. Build SignalR connection
        const baseURL = import.meta.env.VITE_API_URL || 'http://localhost:5181/api';
        const hubURL = baseURL.replace('/api', '/chathub');
        const storedUser = localStorage.getItem('user');
        const token = storedUser ? JSON.parse(storedUser)?.token : null;

        const connection = new signalR.HubConnectionBuilder()
            .withUrl(hubURL, { accessTokenFactory: () => token })
            .withAutomaticReconnect()
            .build();

        // 3. Register event handlers BEFORE starting
        connection.on('ReceiveMessage', (message) => {
            setMessages((prev) => {
                // Avoid duplicates (optimistic updates)
                if (prev.some(m => m.id && m.id === message.id)) return prev;
                return [...prev, message];
            });
        });

        connection.on('ReceiveWarning', (warningMsg) => {
            setWarningBanner(warningMsg);
            setTimeout(() => setWarningBanner(null), 10000);
        });

        // 4. Start the connection
        connection.start()
            .then(() => {
                setIsConnected(true);
                return connection.invoke('JoinChat', conversationId);
            })
            .catch(err => console.error('SignalR connection failed:', err));

        connectionRef.current = connection;

        // 5. Cleanup when conversationId changes or component unmounts
        return () => {
            if (connectionRef.current) {
                const conn = connectionRef.current;
                connectionRef.current = null;
                setIsConnected(false);
                if (conn.state === signalR.HubConnectionState.Connected) {
                    conn.invoke('LeaveChat', conversationId)
                        .catch(() => {})
                        .finally(() => conn.stop());
                } else {
                    conn.stop().catch(() => {});
                }
            }
        };
    }, [conversationId]);

    useEffect(() => {
        scrollToBottom();
    }, [messages]);

    // ── Send message ──────────────────────────────────────────────────────────
    const sendMessage = useCallback(async () => {
        const trimmed = newMessage.trim();
        if (!trimmed || !connectionRef.current || connectionRef.current.state !== signalR.HubConnectionState.Connected) return;

        // Clear input immediately for better UX
        setNewMessage('');
        setIsSending(true);
        inputRef.current?.focus();

        try {
            await connectionRef.current.invoke('SendMessage', conversationId, trimmed);
        } catch (err) {
            console.error('Error sending message:', err);
            // Restore message if send failed
            setNewMessage(trimmed);
        } finally {
            setIsSending(false);
        }
    }, [newMessage, conversationId]);

    const handleSubmit = (e) => {
        e.preventDefault();
        sendMessage();
    };

    const handleKeyDown = (e) => {
        if (e.key === 'Enter' && !e.shiftKey) {
            e.preventDefault();
            sendMessage();
        }
    };

    return (
        <div className="flex flex-col h-full bg-white rounded-xl shadow-sm border border-gray-100 overflow-hidden relative">

            {/* Header */}
            <div className="bg-white border-b border-gray-100 p-4 flex items-center gap-3">
                <div className="w-9 h-9 rounded-full bg-primary/10 flex items-center justify-center shrink-0">
                    <span className="text-primary font-bold text-sm">
                        {conversationName ? conversationName.charAt(0).toUpperCase() : <User className="w-5 h-5" />}
                    </span>
                </div>
                <div>
                    <h2 className="font-semibold text-gray-800 leading-tight">
                        {conversationName || 'Conversation'}
                    </h2>
                    <p className="text-xs text-gray-400 flex items-center gap-1">
                        {isConnected ? (
                            <>
                                <span className="w-1.5 h-1.5 rounded-full bg-green-400 inline-block" />
                                Online
                            </>
                        ) : (
                            <>
                                <span className="w-1.5 h-1.5 rounded-full bg-yellow-400 inline-block animate-pulse" />
                                Connecting...
                            </>
                        )}
                    </p>
                </div>
            </div>

            {/* Warning Banner */}
            {warningBanner && (
                <div className="bg-red-50 border-l-4 border-red-500 p-3 mx-4 mt-2 rounded-lg z-10 animate-in slide-in-from-top-2">
                    <div className="flex items-center gap-2">
                        <AlertTriangle className="h-4 w-4 text-red-500 shrink-0" />
                        <p className="text-sm text-red-700 font-medium">{warningBanner}</p>
                    </div>
                </div>
            )}

            {/* Messages Area */}
            <div className="flex-1 overflow-y-auto p-4 space-y-1 bg-gray-50/50">
                {loading ? (
                    <div className="flex justify-center items-center h-full text-gray-400 text-sm gap-2">
                        <div className="w-4 h-4 border-2 border-gray-300 border-t-primary rounded-full animate-spin" />
                        Loading messages...
                    </div>
                ) : messages.length === 0 ? (
                    <div className="flex flex-col items-center justify-center h-full gap-2 text-gray-400">
                        <User className="w-8 h-8 opacity-30" />
                        <p className="text-sm">No messages yet. Say hello!</p>
                    </div>
                ) : (
                    <>
                        {messages.map((msg, idx) => (
                            <MessageBubble
                                key={msg.id || idx}
                                message={msg}
                                isMe={msg.senderId === currentUser?.id}
                            />
                        ))}
                        <div ref={messagesEndRef} />
                    </>
                )}
            </div>

            {/* Input Form */}
            <div className="p-4 bg-white border-t border-gray-100">
                <form onSubmit={handleSubmit} className="flex gap-2">
                    <input
                        ref={inputRef}
                        type="text"
                        value={newMessage}
                        onChange={(e) => setNewMessage(e.target.value)}
                        onKeyDown={handleKeyDown}
                        placeholder={isConnected ? 'Type a message...' : 'Connecting...'}
                        disabled={!isConnected || isSending}
                        autoComplete="off"
                        className="flex-1 border border-gray-200 rounded-full px-4 py-2.5 text-sm focus:outline-none focus:border-primary focus:ring-1 focus:ring-primary disabled:bg-gray-50 disabled:text-gray-400 transition-colors"
                    />
                    <button
                        type="submit"
                        disabled={!newMessage.trim() || !isConnected || isSending}
                        className="bg-primary text-white p-2.5 rounded-full hover:bg-primary/90 transition disabled:opacity-40 disabled:cursor-not-allowed"
                    >
                        <Send size={18} className="translate-x-[-1px]" />
                    </button>
                </form>
            </div>
        </div>
    );
};

export default ChatLayout;
