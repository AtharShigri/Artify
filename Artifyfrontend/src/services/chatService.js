import api from '../api/axios';

const getConversations = async () => {
    try {
        const response = await api.get('/chat/conversations');
        return response.data;
    } catch (error) {
        console.error('Error fetching conversations:', error);
        throw error;
    }
};

const getChatHistory = async (conversationId) => {
    try {
        const response = await api.get(`/chat/history/${conversationId}`);
        return response.data;
    } catch (error) {
        console.error(`Error fetching chat history for ${conversationId}:`, error);
        throw error;
    }
};

const startConversation = async (artistProfileId) => {
    try {
        const response = await api.post(`/chat/start/${artistProfileId}`);
        return response.data;
    } catch (error) {
        console.error('Error starting conversation:', error);
        throw error;
    }
};

export const chatService = {
    getConversations,
    getChatHistory,
    startConversation
};
