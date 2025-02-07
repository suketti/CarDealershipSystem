import api from "../api/axiosInstance.ts";
import { CarDTO } from "../Types";

export const getCars = async (): Promise<CarDTO[]> => {
    try {
        const response = await api.get<CarDTO[]>("/cars"); // Uses the base URL from api.ts
        return response.data;

    } catch (error) {
        console.error("Error fetching cars:", error);
        return [];
    }
};
