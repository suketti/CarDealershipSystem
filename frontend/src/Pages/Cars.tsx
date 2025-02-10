import React, { useState, useEffect } from "react";
import { Navigate, useLocation } from "react-router-dom";
import { CarDTO } from "../Types";
import { getCars } from "../api/carService.ts";
import styles from './Cars.module.css'; // Import the CSS module
import CarDetails from "./CarDetails.tsx";
import { useNavigate } from "react-router-dom";
import { translations } from "../translations";
import { LanguageCtx } from "../App";
import { useContext } from "react";
import {getAllBodyTypes} from "../api/carMetadataService.ts";

function Cars() {
  const location = useLocation();

  const [cars, setCars] = useState<CarDTO[]>([]);
  const [filteredCars, setFilteredCars] = useState<CarDTO[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);


  const [language, setLanguage] = useState<"hu" | "en" | "ja">("hu");
    const langCtx = useContext(LanguageCtx)
  

  const [brand, setBrand] = useState("");
  const [model, setModel] = useState("");
  const [bodyType, setBodyType] = useState("");
  const [fuelType, setFuelType] = useState("");
  const [selectedLocations, setSelectedLocations] = useState<string[]>([]);
  const [showLocationModal, setShowLocationModal] = useState(false);

  useEffect(() => {
    async function fetchCars() {
      try {
        const response = await getCars();
        setCars(response);
        setFilteredCars(response);
        setLoading(false);
      } catch (err) {
        setError("Failed to fetch cars. Please try again later.");
        setLoading(false);
      }
    }
    fetchCars();
  }, []);



  const handleMapPointClick = (location: string) => {
    setSelectedLocations([location]);
    setShowLocationModal(false);
  };

  const handleSearch = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const filtered = cars.filter((car) => {
      return (
          (!brand || car.brand.brandEnglish.toLowerCase() === brand.toLowerCase()) &&
          (!model || car.carModel.modelNameEnglish.toLowerCase() === model.toLowerCase()) &&
          (!bodyType || car.bodyType.nameEnglish.toLowerCase() === bodyType.toLowerCase()) &&
          (!fuelType || car.fuelType.nameEnglish.toLowerCase() === fuelType.toLowerCase()) &&
          (selectedLocations.length === 0 || selectedLocations.includes(car.location.locationName))
      );
    });
    setFilteredCars(filtered);
  };

  if (loading) return <p>Loading...</p>;
  if (error) return <p>{error}</p>;

  return (
      <div className={styles['cars-page']}>
        <div className={styles.container}>
          <div className={styles['cars-layout']}>
            <aside className={styles['filter-sidebar']}>
              <h2>{langCtx?.translate.searchTitle}</h2>
              <form onSubmit={handleSearch}>
                <label htmlFor="brand">{langCtx?.translate.brand}</label>
                <input type="text" id="brand" value={brand} onChange={(e) => setBrand(e.target.value)} />

                <label htmlFor="model">{langCtx?.translate.model}</label>
                <input type="text" id="model" value={model} onChange={(e) => setModel(e.target.value)} />

                <label htmlFor="bodyType">{langCtx?.translate.bodyType}</label>
                <input type="text" id="bodyType" value={bodyType} onChange={(e) => setBodyType(e.target.value)} />

                <label htmlFor="fuelType">{langCtx?.translate.fuelType}</label>
                <input type="text" id="fuelType" value={fuelType} onChange={(e) => setFuelType(e.target.value)} />

                <button type="button" className={styles.btn} onClick={() => setShowLocationModal(true)}>
                  {langCtx?.translate.selectLocation}
                </button>

                <button type="submit" className={styles.btn}>{langCtx?.translate.search}</button>
              </form>
            </aside>

            <main className={styles['cars-list']}>
              {filteredCars.length === 0 ? (
                  <p>{langCtx?.translate.noResults}</p>
              ) : (
                  filteredCars.map((car) => (
                      <div key={car.id} className={styles['car-card']}>
                        <h3>{car.carModel.modelNameEnglish}</h3>
                        <p>{car.brand.brandEnglish}</p>
                        <p>{car.bodyType.nameEnglish}</p>
                        <p>{car.fuelType.nameEnglish}</p>
                        <p>{car.location.locationName}</p>
                        
                      </div>
                  ))
              )}
            </main>
          </div>
        </div>

        {showLocationModal && (
            <div id="location-overlay" className={styles['location-overlay']}>
              <div
                  id="location-modal"
                  className={styles['location-modal']}
              >
                <h2>{langCtx?.translate.selectLocation}</h2>
                <div className={styles['map-container']}>
                  <img
                      src="/Képek/magyarorszag-terkep.jpg"
                      alt="Magyarország térképe"
                      className={styles['map-image']}
                  />
                  <button className={styles['map-point']} style={{ top: "40%", left: "45%" }} onClick={() => handleMapPointClick("Budapest")}>
                    Budapest
                  </button>
                  <button className={styles['map-point']} style={{ top: "37%", left: "80%" }} onClick={() => handleMapPointClick("Debrecen")}>
                    Debrecen
                  </button>
                  <button className={styles['map-point']} style={{ top: "20%", left: "70%" }} onClick={() => handleMapPointClick("Miskolc")}>
                    Miskolc
                  </button>
                  <button className={styles['map-point']} style={{ top: "36%", left: "10%" }} onClick={() => handleMapPointClick("Sopron")}>
                    Sopron
                  </button>
                </div>
                <button className={styles.btn} onClick={() => setShowLocationModal(false)}>{langCtx?.translate.complete}</button>
              </div>
            </div>
        )}
      </div>
  );
}

export default Cars;
