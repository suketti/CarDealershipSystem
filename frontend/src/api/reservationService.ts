import api from "./axiosInstance.ts";
import {CreateReservationDTO, ReservationDTO} from "../Interfaces/Reservation.ts";



const ReservationService = {
    async getAllReservations(): Promise<ReservationDTO[]> {
        try {
            const response = await api.get<ReservationDTO[]>("/reservations");
            return response.data;
        } catch (error) {
            console.error("Error fetching reservations:", error);
            throw error;
        }
    },

    async getReservationById(id: number): Promise<ReservationDTO> {
        try {
            const response = await api.get<ReservationDTO>(`/reservations/${id}`);
            return response.data;
        } catch (error) {
            console.error("Error fetching reservation:", error);
            throw error;
        }
    },

    async createReservation(reservationData: CreateReservationDTO): Promise<ReservationDTO> {
        console.log(reservationData);
        try {
            const response = await api.post<ReservationDTO>("/reservations", reservationData);
            return response.data;
        } catch (error) {
            console.error("Error creating reservation:", error);
            throw error;
        }
    },

    async deleteReservation(id: number): Promise<void> {
        try {
            await api.delete(`/reservations/${id}`);
        } catch (error) {
            console.error("Error deleting reservation:", error);
            throw error;
        }
    },
};

export default ReservationService;
