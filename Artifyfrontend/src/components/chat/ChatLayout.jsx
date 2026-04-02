import React, { useState, useEffect, useRef } from 'react';
import * as signalR from '@microsoft/signalr';
import { Send, AlertTriangle } from 'lucide-react';
import { chatService } from '../../services/chatService';
import MessageBubble from './MessageBubble';
const ChatLayout = ({ conversationId, currentUser }) => {
    const [messages, setMessages] = useState([]);
    const [newMessage, setNewMessage] = useState("");
    const [connection, setConnection] = useState(null);
    const [warningBanner, setWarningBanner] = useState(null);
    const [loading, setLoading] = useState(false);

    const messagesEndRef = useRef(null);

    const scrollToBottom = () => {
        messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
    };

    useEffect(() => {
        if (!conversationId) return;

        // Fetch History
        const fetchHistory = async () => {
            setLoading(true);
            try {
                const history = await chatService.getChatHistory(conversationId);
                // Assume history is an array of messages
                setMessages(history || []);
            } catch (error) {
                console.error("Could not fetch history:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchHistory();

        // Setup SignalR Connection
        const baseURL = import.meta.env.VITE_API_URL || 'http://localhost:5181/api';
        const hubURL = baseURL.replace('/api', '/chathub');

        const newConnection = new signalR.HubConnectionBuilder()
            .withUrl(hubURL)
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);
    }, [conversationId]);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(() => {
                    console.log('Connected to SignalR');
                    connection.invoke("JoinChat", conversationId);
                    
                    connection.on("ReceiveMessage", (message) => {
                        setMessages((prev) => [...prev, message]);
                    });

                    connection.on("ReceiveWarning", (warningMsg) => {
                        setWarningBanner(warningMsg);
                        // Auto-hide warning after 10 seconds
                        setTimeout(() => setWarningBanner(null), 10000);
                    });
                })
                .catch(e => console.error('Connection failed: ', e));

            return () => {
                connection.invoke("LeaveChat", conversationId)
                    .then(() => connection.stop())
                    .catch(e => console.error(e));
            };
        }
    }, [connection, conversationId]);

    useEffect(() => {
        scrollToBottom();
    }, [messages]);

    const handleSendMessage = async (e) => {
        e.preventDefault();
        if (!newMessage.trim() || !connection) return;

        // Ensure we send as current user
        try {
            await connection.invoke("SendMessage", conversationId, currentUser?.id || "unknown-id", newMessage);
            setNewMessage("");
        } catch (error) {
            console.error("Error sending message:", error);
        }
    };

    return (
        <div className="flex flex-col h-full bg-white rounded-xl shadow-sm border border-gray-100 overflow-hidden relative">
            
            {/* Header */}
            <div className="bg-white border-b border-gray-100 p-4 flex items-center justify-between">
                <div>
                    <h2 className="font-semibold text-gray-800">Conversation</h2>
                    <p className="text-xs text-gray-500">ID: {conversationId}</p>
                </div>
            </div>

            {/* Warning Banner */}
            {warningBanner && (
                <div className="bg-red-50 border-l-4 border-red-500 p-4 absolute top-16 left-0 right-0 z-10 animate-in slide-in-from-top-2">
                    <div className="flex items-center">
                        <div className="flex-shrink-0">
                            <AlertTriangle className="h-5 w-5 text-red-500" />
                        </div>
                        <div className="ml-3">
                            <p className="text-sm text-red-700 font-medium">
                                {warningBanner}
                            </p>
                        </div>
                    </div>
                </div>
            )}

            {/* Messages Area */}
            <div className="flex-1 overflow-y-auto p-4 space-y-4 bg-gray-50/50">
                {loading ? (
                    <div className="flex justify-center items-center h-full">Loading history...</div>
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
                <form onSubmit={handleSendMessage} className="flex gap-2">
                    <input
                        type="text"
                        value={newMessage}
                        onChange={(e) => setNewMessage(e.target.value)}
                        placeholder="Type a message..."
                        className="flex-1 border border-gray-200 rounded-full px-4 py-2 text-sm focus:outline-none focus:border-primary focus:ring-1 focus:ring-primary"
                    />
                    <button 
                        type="submit" 
                        disabled={!newMessage.trim()}
                        className="bg-primary text-white p-2 rounded-full hover:bg-primary/90 transition disabled:opacity-50"
                    >
                        <Send size={18} className="translate-x-[-1px]" />
                    </button>
                </form>
            </div>
        </div>
    );
};

export default ChatLayout;
