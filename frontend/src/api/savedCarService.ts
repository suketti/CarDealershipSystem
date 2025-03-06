import axios from './axiosInstance.ts';
import {CarDTO} from "../Types"; // Assuming you've already created and exported your API instance



class SavedCarService {
    // Fetch saved cars for a user
    static async getSavedCars(userId: string): Promise<CarDTO[]> {
        try {
            const response = await axios.get(`/SavedCars/${userId}`);
            return response.data; // Assuming the response is a list of saved cars
        } catch (error) {
            console.error('Error fetching saved cars:', error);
            throw error;
        }
    }

    // Save a car for the user
    static async saveCar(userId: string, carId: number): Promise<void> {
        try {
            // Send the carId directly as raw JSON
            await axios.post(`/SavedCars/${userId}/save`, carId);
        } catch (error) {
            console.error('Error saving car:', error);
            throw error;  // Rethrow the error so the caller can handle it if necessary
        }
    }
    // Remove a saved car for the user
    static async removeSavedCar(userId: string, carId: number): Promise<void> {
        try {
            await axios.delete(`/savedcars/${userId}/remove/${carId}`);
        } catch (error) {
            console.error('Error removing saved car:', error);
            throw error;
        }
    }
}

export default SavedCarService;
