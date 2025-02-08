import React, { useEffect, useState } from "react";
import { Car } from "../Types";
import { translations } from "../translations";
import { LanguageCtx } from "../App";
import { useContext } from "react";

interface SavedCarsModalProps {
  onClose: () => void;
  t: { [key: string]: string };
}

function SavedCarsModal({ onClose, t }: SavedCarsModalProps) {
  const [savedCars, setSavedCars] = useState<Car[]>([]);

  useEffect(() => {
    const carsFromStorage = localStorage.getItem("savedCars");
    if (carsFromStorage) {
      setSavedCars(JSON.parse(carsFromStorage));
    }
  }, []);

  const handleDeleteCar = (index: number) => {
    const updated = [...savedCars];
    updated.splice(index, 1);
    setSavedCars(updated);
    localStorage.setItem("savedCars", JSON.stringify(updated));
  };
  const langCtx = useContext(LanguageCtx)

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
          position: "relative"
        }}
      >
        <span
          className="close-saved-cars-modal"
          style={{
            position: "absolute",
            top: "15px",
            right: "15px",
            cursor: "pointer",
            fontSize: "24px"
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
              <div key={index} className="saved-car-item" style={{ borderBottom: "1px solid #ddd", marginBottom: "10px" }}>
                <img
                  src={car.kep}
                  alt={`${car.marka} ${car.modell}`}
                  style={{ width: "150px", height: "100px", objectFit: "cover", marginRight: "10px" }}
                />
                <div className="saved-car-info">
                  <h3>
                    {car.marka} {car.modell}
                  </h3>
                  <p>{langCtx?.translate.price} {car.ar.toLocaleString()} Ft</p>
                  <p>{langCtx?.translate.year} {car.ev}</p>
                  <a
                    href={`/autok/reszletek?car=${encodeURIComponent(
                      JSON.stringify(car)
                    )}`}
                  >
                    {langCtx?.translate.details || "Részletek"}
                  </a>
                </div>
                <button
                  className="delete-car-btn"
                  style={{ marginLeft: "auto", cursor: "pointer" }}
                  onClick={() => handleDeleteCar(index)}
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
