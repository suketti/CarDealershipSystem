export interface ReservationDTO {
    id: number;
    userId: string;
    carId: number;
    date: string;
}

export interface CreateReservationDTO {
    userId: string;
    carId: number;
    date: string;
}

export interface UpdateReservationDTO {
    carId: number;
    date: string;
}
