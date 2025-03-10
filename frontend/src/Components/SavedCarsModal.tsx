import React, { useEffect, useState, useContext } from "react";
import { CarDTO } from "../Types";
import { LanguageCtx } from "../App";
import SavedCarService from "../api/savedCarService.ts"; // Importing the service
import { useUser } from "../UserContext.tsx";
import { getBaseUrl } from "../api/axiosInstance.ts";
import axiosInstance from "../api/axiosInstance.ts"; // Importing the useUser hook

interface SavedCarsModalProps {
    onClose: () => void;
    t: { [key: string]: string };
}

function SavedCarsModal({ onClose, t }: SavedCarsModalProps) {
    const [savedCars, setSavedCars] = useState<CarDTO[]>([]);
    const langCtx = useContext(LanguageCtx);
    const { user } = useUser(); // Get the userId from the UserContext

    useEffect(() => {
        const fetchSavedCars = async () => {
            if (!user?.id) return; // If no userId, do not fetch saved cars
            try {
                const cars = await SavedCarService.getSavedCars(user.id);
                setSavedCars(cars);
            } catch (error) {
                console.error("Error fetching saved cars:", error);
            }
        };

        fetchSavedCars();
    }, [user?.id]);

    const handleDeleteCar = async (carId: number) => {
        if (!user?.id) return; // Ensure there's a userId before attempting to delete
        try {
            await SavedCarService.removeSavedCar(user?.id, carId);
            setSavedCars(savedCars.filter((car) => car.id !== carId));
        } catch (error) {
            console.error("Error deleting saved car:", error);
        }
    };

    return (
        <div id="saved-cars-overlay" style={{ display: "block" }}>
            <div
                id="saved-cars-modal"
                style={{
                    display: "block",
                    background: "#fff",
                    padding: "2rem",
                    borderRadius: "8px",
                    width: "80%",
                    maxWidth: "800px",
                    maxHeight: "90vh",
                    overflowY: "auto",
                    margin: "50px auto",
                    position: "relative",
                }}
            >
                <span
                    className="close-saved-cars-modal"
                    style={{
                        position: "absolute",
                        top: "15px",
                        right: "15px",
                        cursor: "pointer",
                        fontSize: "24px",
                    }}
                    onClick={onClose}
                >
                    &times;
                </span>
                <div className="saved-cars-header">
                    <h2>{langCtx?.translate.savedCars}</h2>
                </div>
                <div id="saved-cars-list" className="saved-cars-container">
                    {savedCars.length === 0 ? (
                        <p>{langCtx?.translate.noSavedCars}</p>
                    ) : (

                        savedCars.map((car, index) => (
                            <div
                                key={index}
                                className="saved-car-item"
                                style={{ borderBottom: "1px solid #ddd", marginBottom: "10px" }}
                            >
                                <img
                                    src={car.images?.length ? getBaseUrl() + car.images[0].url : "/default-image.jpg"}
                                    alt={`${car.brand.brandEnglish} ${car.carModel.modelNameEnglish}`}
                                    style={{
                                        width: "150px",
                                        height: "100px",
                                        objectFit: "cover",
                                        marginRight: "10px",
                                    }}
                                />


                                <div className="saved-car-info">
                                    <h3>
                                        {car.brand.brandEnglish} {car.carModel.modelNameEnglish}
                                    </h3>
                                    <p>
                                        {langCtx?.translate.price}{" "}
                                        {Number(car.price).toLocaleString()} Ft
                                    </p>
                                    <p>
                                        {langCtx?.translate.year} {car.carModel.manufacturingStartYear} -{" "}
                                        {car.carModel.manufacturingEndYear}
                                    </p>
                                    <a
                                        href={`/Car-Details?car=${encodeURIComponent(
                                            JSON.stringify(car)
                                        )}`}
                                    >
                                        {langCtx?.translate.details || "Részletek"}
                                    </a>
                                </div>
                                <button
                                    className="delete-car-btn"
                                    style={{ marginLeft: "auto", cursor: "pointer" }}
                                    onClick={() => handleDeleteCar(car.id)}
                                >
                                    🗑️
                                </button>
                            </div>
                        ))
                    )}
                </div>
            </div>
        </div>
    );
}

export default SavedCarsModal;
