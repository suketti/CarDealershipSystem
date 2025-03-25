import api from './axiosInstance.ts';

// Function to get all locations
export const getAllLocations = async () => {
    try {
        const response = await api.get('/locations');
        return response.data;
    } catch (error) {
        console.error('Error fetching locations:', error);
        throw error;
    }
};