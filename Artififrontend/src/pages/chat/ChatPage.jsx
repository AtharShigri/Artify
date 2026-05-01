import React, { useState, useEffect } from 'react';
import { MessageSquare, Users } from 'lucide-react';
import ChatLayout from '../../components/chat/ChatLayout';
import { chatService } from '../../services/chatService';
import { useAuth } from '../../context/AuthContext';

const ChatPage = () => {
    const { user } = useAuth();
    const [conversations, setConversations] = useState([]);
    const [selectedConversation, setSelectedConversation] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchConvos = async () => {
            try {
                const data = await chatService.getConversations();
                setConversations(data || []);
                if (data && data.length > 0) {
                    setSelectedConversation(data[0]);
                }
            } catch (error) {
                console.error("Failed to load conversations", error);
            } finally {
                setLoading(false);
            }
        };

        fetchConvos();
    }, []);

    // Determine the other participant's name relative to the current user
    const getConversationName = (conv) => {
        if (!conv || !user) return 'Conversation';
        if (conv.participantA_Id === user.id) {
            return conv.participantB_Name || 'User';
        }
        return conv.participantA_Name || 'User';
    };

    if (loading) {
        return (
            <div className="h-[calc(100vh-140px)] flex items-center justify-center">
                <div className="flex flex-col items-center gap-3 text-gray-400">
                    <MessageSquare className="w-8 h-8 animate-pulse" />
                    <p className="text-sm">Loading conversations...</p>
                </div>
            </div>
        );
    }

    return (
        <div className="flex h-[calc(100vh-140px)] w-full gap-4 p-4 md:p-6 mx-auto max-w-7xl">
            {/* Sidebar List */}
            <div className="w-full md:w-80 bg-white rounded-xl shadow-sm border border-gray-100 hidden md:flex flex-col shrink-0">
                <div className="p-4 border-b border-gray-100 flex items-center gap-2">
                    <Users className="w-5 h-5 text-primary" />
                    <h2 className="font-bold text-lg text-gray-800">Messages</h2>
                </div>
                <div className="flex-1 overflow-y-auto">
                    {conversations.length === 0 ? (
                        <div className="p-8 text-center text-gray-400">
                            <MessageSquare className="w-10 h-10 mx-auto mb-3 opacity-30" />
                            <p className="text-sm font-medium">No conversations yet.</p>
                            <p className="text-xs mt-1">Start a chat from an artist's profile.</p>
                        </div>
                    ) : (
                        conversations.map((conv) => {
                            const name = getConversationName(conv);
                            const isSelected = selectedConversation?.id === conv.id;
                            return (
                                <button
                                    key={conv.id}
                                    onClick={() => setSelectedConversation(conv)}
                                    className={`w-full text-left p-4 border-b border-gray-50 transition-all ${
                                        isSelected
                                            ? 'bg-primary/5 border-l-4 border-l-primary'
                                            : 'hover:bg-gray-50 border-l-4 border-l-transparent'
                                    }`}
                                >
                                    <div className="flex items-center gap-3">
                                        <div className="w-9 h-9 rounded-full bg-primary/10 flex items-center justify-center shrink-0">
                                            <span className="text-primary font-bold text-sm">
                                                {name.charAt(0).toUpperCase()}
                                            </span>
                                        </div>
                                        <div className="min-w-0 flex-1">
                                            <h4 className="font-semibold text-gray-800 text-sm truncate">{name}</h4>
                                            <p className="text-xs text-gray-400 truncate mt-0.5">
                                                {conv.lastMessage || 'Start a conversation'}
                                            </p>
                                        </div>
                                    </div>
                                </button>
                            );
                        })
                    )}
                </div>
            </div>

            {/* Main Chat Area */}
            <div className="flex-1 h-full min-w-0">
                {selectedConversation ? (
                    <ChatLayout
                        conversationId={selectedConversation.id}
                        currentUser={user}
                        conversationName={getConversationName(selectedConversation)}
                    />
                ) : (
                    <div className="h-full flex flex-col items-center justify-center bg-white rounded-xl border border-gray-100 gap-4 text-gray-400">
                        <MessageSquare className="w-12 h-12 opacity-20" />
                        <div className="text-center">
                            <p className="font-medium">Select a conversation</p>
                            <p className="text-sm mt-1">Choose from the list to start chatting</p>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
};

export default ChatPage;
