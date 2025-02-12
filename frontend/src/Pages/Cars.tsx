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
import {getAllMakerTypes, getAllModelsTypes, getAllBodyTypes, getAllTransmissionTypes, getAllFuelTypes, getAllColors, getAllDrivetrainTypes} from "../api/carMetadataService.ts";
import {BodyTypeDTO, CarMakerDTO, CarModelDTO, ColorDTO, FuelTypeDTO, TransmissionTypeDTO} from "../Types";

function Cars() {
  const location = useLocation();
  const [cars, setCars] = useState<CarDTO[]>([]);
  const [filteredCars, setFilteredCars] = useState<CarDTO[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [language, setLanguage] = useState<"hu" | "en" | "ja">("hu");
  const langCtx = useContext(LanguageCtx);

  const [brand, setBrand] = useState("");
  const [model, setModel] = useState("");
  const [bodyType, setBodyType] = useState("");
  const [fuelType, setFuelType] = useState("");
  const [selectedLocations, setSelectedLocations] = useState<string[]>([]);
  const [showLocationModal, setShowLocationModal] = useState(false);
  const [bodyTypeOptions, setBodyTypeOptions] = useState<BodyTypeDTO[]>([]);
  const [fuelTypeOptions, setFuelTypeOptions] = useState<FuelTypeDTO[]>([]);
  const [transmissionTypeOptions, setTransmissionTypeOptions] = useState<TransmissionTypeDTO[]>([]);
  const [colorTypeOptions, setColorTypeOptions] = useState<ColorDTO[]>([]);
  const [DrivetrainTypeOptions, setDrivetrainTypeOptions] = useState<ColorDTO[]>([]);
  const [MakerTypeOptions, setMakerTypeOptions] = useState<CarMakerDTO[]>([]);
  const [ModelTypeOptions, setModelTypeOptions] = useState<CarModelDTO[]>([]);
  const [selectedBrand, setSelectedBrand] = useState("");
  const [filteredModels, setFilteredModels] = useState<CarModelDTO[]>([]);
  const [minPrice, setMinPrice] = useState<number | null>(null);
  const [maxPrice, setMaxPrice] = useState<number | null>(null);
  const [yearFrom, setYearFrom] = useState<number | null>(null);
  const [yearTo, setYearTo] = useState<number | null>(null);
  const [selectedColor, setSelectedColor] = useState("");
  const [minEngineSize, setMinEngineSize] = useState<number | null>(null);
  const [maxEngineSize, setMaxEngineSize] = useState<number | null>(null);
  const [minMileage, setMinMileage] = useState<number | null>(null);
  const [maxMileage, setMaxMileage] = useState<number | null>(null);
  const [isAdvancedSearchVisible, setIsAdvancedSearchVisible] = useState(false);

  useEffect(() => {
    async function fetchCars() {
      const respBody = await getAllBodyTypes();
      setBodyTypeOptions(respBody);
      const respFuel = await getAllFuelTypes();
      setFuelTypeOptions(respFuel);
      const respTrans = await getAllTransmissionTypes();
      setTransmissionTypeOptions(respTrans);
      const respColor = await getAllColors();
      setColorTypeOptions(respColor);
      const respDrivetrain = await getAllDrivetrainTypes();
      setDrivetrainTypeOptions(respDrivetrain);
      const respMaker = await getAllMakerTypes();
      setMakerTypeOptions(respMaker);
      const respModel = await getAllModelsTypes();
      setModelTypeOptions(respModel);
    }
    fetchCars();
  }, []);

  useEffect(() => {
    const params = new URLSearchParams(location.search);
  
    const brandParam = params.get("brand") || "";
    const modelParam = params.get("model") || "";
    const bodyTypeParam = params.get("bodyType") || "";
    const fuelTypeParam = params.get("fuelType") || "";
    const transmissionParam = params.get("transmission") || "";
    const drivetrainParam = params.get("drivetrain") || "";
    const colorParam = params.get("color") || "";
  
    setBrand(brandParam);
    setModel(modelParam);
    setBodyType(bodyTypeParam);
    setFuelType(fuelTypeParam);
    setSelectedTransmission(transmissionParam);
    setSelectedDrivetrain(drivetrainParam);
    setSelectedColor(colorParam);
  
    // Beállítjuk a megfelelő modelleket a márka alapján
    if (brandParam) {
      const filtered = ModelTypeOptions.filter(model => model.maker.id.toString() === brandParam);
      setFilteredModels(filtered);
    }
  }, [location.search, ModelTypeOptions]);
  
  

  useEffect(() => {
    if (selectedBrand) {
      const filtered = ModelTypeOptions.filter(model => model.maker.id === parseInt(selectedBrand));
      setFilteredModels(filtered);
    } else {
      setFilteredModels([]);
    }
  }, [selectedBrand, ModelTypeOptions]);

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
        (selectedLocations.length === 0 || selectedLocations.includes(car.location.locationName)) &&
        
        (!yearFrom || car.carModel.manufacturingStartYear >= yearFrom) &&
        (!yearTo || car.carModel.manufacturingEndYear <= yearTo) &&
        (!selectedColor || car.color.colorNameEnglish.toLowerCase() === selectedColor.toLowerCase())
        
      );
    });
    setFilteredCars(filtered);
  };

  if (error) return <p>{error}</p>;

  return (
    <div className={styles['cars-page']}>
      <div className={styles.container}>
        <div className={styles['cars-layout']}>
          <aside className={styles['filter-sidebar']}>
            <h2>{langCtx?.translate.searchTitle}</h2>
            <form onSubmit={handleSearch}>
              <label htmlFor="brand">{langCtx?.translate.brand}</label>
              <select id="brand-type" value={brand} onChange={(e) => setBrand(e.target.value)}>
  <option value="">{langCtx?.translate.chooseBrand}</option>
  {MakerTypeOptions.map(option => (
    <option key={option.id} value={option.id}>
      {langCtx?.language === "jp" ? option.brandJapanese : option.brandEnglish}
    </option>
  ))}
</select>

              <label htmlFor="model">{langCtx?.translate.model}</label>
              <select id="model-type" value={model} onChange={(e) => setModel(e.target.value)} disabled={filteredModels.length === 0}>
  <option value="">{filteredModels.length === 0 ? langCtx?.translate.noModel : langCtx?.translate.chooseModel}</option>
  {filteredModels.map(option => (
    <option key={option.id} value={option.id}>
      {langCtx?.language === "jp" ? option.modelNameJapanese : option.modelNameEnglish}
    </option>
  ))}
</select>

              <label htmlFor="bodyType">{langCtx?.translate.bodyType}</label>
              <select id="body-type" value={bodyType} onChange={(e) => setBodyType(e.target.value)}>
  <option value="">{langCtx?.translate.chooseBodyType}</option>
  {bodyTypeOptions.map(option => (
    <option key={option.id} value={option.id}>
      {langCtx?.language === "jp" ? option.nameJapanese : option.nameEnglish}
    </option>
  ))}
</select>


              <label htmlFor="fuelType">{langCtx?.translate.fuelType}</label>
              <select id="fuel-type" value={fuelType} onChange={(e) => setFuelType(e.target.value)}>
  <option value="">{langCtx?.translate.chooseFuel}</option>
  {fuelTypeOptions.map(option => (
    <option key={option.id} value={option.id}>
      {langCtx?.language === "jp" ? option.nameJapanese : option.nameEnglish}
    </option>
  ))}
</select>


              <label>{langCtx?.translate.minPrice}</label>
              <input type="number" id="min-price" name="min-price" placeholder="Ft" onChange={(e) => setMinPrice(Number(e.target.value))} />

              <label>{langCtx?.translate.maxPrice}</label>
              <input type="number" id="max-price" name="max-price" placeholder="Ft" onChange={(e) => setMaxPrice(Number(e.target.value))} />

              <label>{langCtx?.translate.yearFrom}</label>
              <input type="number" id="year-from" placeholder="Pl. 2010" onChange={(e) => setYearFrom(Number(e.target.value))} />

              <label>{langCtx?.translate.yearTo}</label>
              <input type="number" id="year-to" placeholder="Pl. 2020" onChange={(e) => setYearTo(Number(e.target.value))} />

              <div id="reszletes-kereses-container">
                <span id="reszletes-kereses-label" onClick={() => setIsAdvancedSearchVisible(!isAdvancedSearchVisible)}>
                  {langCtx?.translate.moreSearch}
                </span>
                <span id="toggle-arrow">{isAdvancedSearchVisible ? "▲" : "▼"}</span>
              </div>

              {isAdvancedSearchVisible && (
                <div id="reszletes-feltetelek">
                  <label>{langCtx?.translate.drive}</label>
                  <select id="drivetrain-type" value={selectedDrivetrain} onChange={(e) => setSelectedDrivetrain(e.target.value)}>
  <option value="">{langCtx?.translate.chooseDrivetrain}</option>
  {DrivetrainTypeOptions.map(option => (
    <option key={option.id} value={option.id}>
      {langCtx?.language === "jp" ? option.nameJapanese : option.nameEnglish}
    </option>
  ))}
</select>
                  <label>{langCtx?.translate.color}</label>
                  <select id="color-type" value={selectedColor} onChange={(e) => setSelectedColor(e.target.value)}>
  <option value="">{langCtx?.translate.chooseColor}</option>
  {colorTypeOptions.map(option => (
    <option key={option.id} value={option.colorNameEnglish}>
      {langCtx?.language === "jp" ? option.colorNameJapanese : option.colorNameEnglish}
    </option>
  ))}
</select>


                  <label>{langCtx?.translate.engineSize}</label>
                  <input type="number" id="motor-meret-min" placeholder="Min cm³" onChange={(e) => setMinEngineSize(Number(e.target.value))} />
                  <input type="number" id="motor-meret-max" placeholder="Max cm³" onChange={(e) => setMaxEngineSize(Number(e.target.value))} />

                  <label>{langCtx?.translate.mileageNum}</label>
                  <input type="number" id="km-min" placeholder="Min km" onChange={(e) => setMinMileage(Number(e.target.value))} />
                  <input type="number" id="km-max" placeholder="Max km" onChange={(e) => setMaxMileage(Number(e.target.value))} />
                </div>
              )}

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
          <div id="location-modal" className={styles['location-modal']}>
            <h2>{langCtx?.translate.selectLocation}</h2>
            <div className={styles['map-container']}>
              <img src="/Képek/magyarorszag-terkep.jpg" alt="Magyarország térképe" className={styles['map-image']} />
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