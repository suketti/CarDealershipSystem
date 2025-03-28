import api from "./axiosInstance.ts";
import {Message} from "../Interfaces/Message.ts";

export const getMessagesByUser = async (userId: string): Promise<Message[]> => {
    try {
        const response = await api.get<Message[]>(`/messages/${userId}`);
        return response.data;
    } catch (error) {
        console.error("Error fetching messages:", error);
        throw error;
    }
};

export const deleteMessage = async (messageId: string): Promise<void> => {
    try {
        await api.delete(`/messages/${messageId}`);
        console.log("Message deleted successfully");
    } catch (error) {
        console.error("Error deleting message:", error);
        throw error;
    }
};
