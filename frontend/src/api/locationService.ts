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

export const getLocationCapacity = async (locationId: number) => {
    try {
        const response = await api.get(`/locations/${locationId}/car-usage`);
        return response.data;
    } catch (error) {
        console.error(`Error fetching capacity for location ${locationId}:`, error);
        throw error;
    }
};