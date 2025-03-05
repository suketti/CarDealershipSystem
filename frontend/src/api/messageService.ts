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
