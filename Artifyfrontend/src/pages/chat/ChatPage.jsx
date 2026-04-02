import React, { useState, useEffect } from 'react';
import ChatLayout from '../../components/chat/ChatLayout';
import { chatService } from '../../services/chatService';

const ChatPage = () => {
    // Current user context
    const userString = localStorage.getItem('user');
    const currentUser = userString ? JSON.parse(userString) : null;
    
    // Determine user role (simplistic mapping for demo)
    if (currentUser) {
        currentUser.role = currentUser.userType === 0 ? 'Buyer' : 'Artist'; 
    }

    const [conversations, setConversations] = useState([]);
    const [selectedConversationId, setSelectedConversationId] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchConvos = async () => {
            try {
                const data = await chatService.getConversations();
                setConversations(data || []);
                // Default select first chat
                if (data && data.length > 0) {
                    setSelectedConversationId(data[0].id);
                }
            } catch (error) {
                console.error("Failed to load conversations", error);
            } finally {
                setLoading(false);
            }
        };

        fetchConvos();
    }, []);

    if (loading) {
        return <div className="p-8 text-center text-gray-500">Loading Chat...</div>;
    }

    return (
        <div className="flex h-[calc(100vh-140px)] w-full gap-4 p-4 md:p-6 mx-auto max-w-7xl">
            {/* Sidebar List */}
            <div className="w-1/3 bg-white rounded-xl shadow-sm border border-gray-100 hidden md:flex flex-col">
                <div className="p-4 border-b border-gray-100">
                    <h2 className="font-bold text-lg text-gray-800">Conversations</h2>
                </div>
                <div className="flex-1 overflow-y-auto">
                    {conversations.length === 0 ? (
                        <div className="p-4 text-sm text-gray-500 text-center">No conversations yet.</div>
                    ) : (
                        conversations.map((conv) => (
                            <button
                                key={conv.id}
                                onClick={() => setSelectedConversationId(conv.id)}
                                className={`w-full text-left p-4 border-b border-gray-50 transition ${
                                    selectedConversationId === conv.id ? 'bg-primary/5 border-l-4 border-l-primary' : 'hover:bg-gray-50'
                                }`}
                            >
                                <h4 className="font-semibold text-gray-800 text-sm">
                                    {conv.targetName || 'Conversation'}
                                </h4>
                                <p className="text-xs text-gray-500 truncate mt-1">
                                    Click to view messages
                                </p>
                            </button>
                        ))
                    )}
                </div>
            </div>

            {/* Main Chat Area */}
            <div className="flex-1 h-full">
                {selectedConversationId ? (
                    <ChatLayout 
                        conversationId={selectedConversationId} 
                        currentUser={currentUser} 
                    />
                ) : (
                    <div className="h-full flex items-center justify-center bg-white rounded-xl border border-gray-100">
                        <p className="text-gray-500">Select a conversation to start chatting</p>
                    </div>
                )}
            </div>
        </div>
    );
};

export default ChatPage;
