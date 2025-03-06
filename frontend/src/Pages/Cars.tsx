import React, { useState, useEffect } from "react";
import { useLocation } from "react-router-dom";
import { CarDTO, DrivetrainTypeDTO } from "../Types";
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
  const [DrivetrainTypeOptions, setDrivetrainTypeOptions] = useState<DrivetrainTypeDTO[]>([]);
  const [MakerTypeOptions, setMakerTypeOptions] = useState<CarMakerDTO[]>([]);
  const [ModelTypeOptions, setModelTypeOptions] = useState<CarModelDTO[]>([]);
  const [selectedBrand, setSelectedBrand] = useState("");
  const [filteredModels, setFilteredModels] = useState<CarModelDTO[]>([]);
  const [minPrice, setMinPrice] = useState<number | null>(null);
  const [maxPrice, setMaxPrice] = useState<number | null>(null);
  const [yearFrom, setYearFrom] = useState<number | null>(null);
  const [yearTo, setYearTo] = useState<number | null>(null);
  const [selectedColor, setSelectedColor] = useState("");
  const [selectedDrivetrain, setSelectedDrivetrain] = useState("");
  const [minEngineSize, setMinEngineSize] = useState<number | null>(null);
  const [maxEngineSize, setMaxEngineSize] = useState<number | null>(null);
  const [minMileage, setMinMileage] = useState<number | null>(null);
  const [maxMileage, setMaxMileage] = useState<number | null>(null);
  const [isAdvancedSearchVisible, setIsAdvancedSearchVisible] = useState(false);

  useEffect(() => {
    async function fetchCars() {
      try {
        const carsData = await getCars();
        setCars(carsData);
        setFilteredCars(carsData); // Initialize filteredCars with all cars
      } catch (err) {
        setError("Error fetching cars");
      } finally {
        setLoading(false);
      }
    }

    async function fetchMetadata() {
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
    fetchMetadata();
  }, []);

  useEffect(() => {
    const params = new URLSearchParams(location.search);
    setBrand(params.get("brand") || "");
    setModel(params.get("model") || "");
    setBodyType(params.get("bodyType") || "");
    setFuelType(params.get("fuelType") || "");
    setSelectedDrivetrain(params.get("drivetrain") || "");
    setSelectedColor(params.get("color") || "");
    setMinPrice(params.get("minPrice") ? Number(params.get("minPrice")) : null);
    setMaxPrice(params.get("maxPrice") ? Number(params.get("maxPrice")) : null);
    setYearFrom(params.get("yearFrom") ? Number(params.get("yearFrom")) : null);
    setYearTo(params.get("yearTo") ? Number(params.get("yearTo")) : null);
    setMinEngineSize(params.get("minEngineSize") ? Number(params.get("minEngineSize")) : null);
    setMaxEngineSize(params.get("maxEngineSize") ? Number(params.get("maxEngineSize")) : null);
    setMinMileage(params.get("minMileage") ? Number(params.get("minMileage")) : null);
    setMaxMileage(params.get("maxMileage") ? Number(params.get("maxMileage")) : null);
  }, [location.search]);

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
    
    console.log("Current Filters:", {
      brand,
      model,
      bodyType,
      fuelType,
      selectedDrivetrain,
      selectedColor,
      minPrice,
      maxPrice,
      yearFrom,
      yearTo,
      minEngineSize,
      maxEngineSize,
      minMileage,
      maxMileage,
    });
  
    const filtered = cars.filter((car) => {
      console.log("Checking car:", car);
  
      return (
        (!brand || car.brand.id.toString() === brand) &&
        (!model || car.carModel.id.toString() === model) &&
        (!bodyType || car.bodyType.id.toString() === bodyType) &&
        (!fuelType || car.fuelType.id.toString() === fuelType) &&
        (!selectedDrivetrain || car.driveTrain.type.toLowerCase() === selectedDrivetrain.toLowerCase()) &&
        (!selectedColor || car.color.colorNameEnglish.toLowerCase() === selectedColor.toLowerCase()) &&
        (!yearFrom || car.carModel.manufacturingStartYear >= yearFrom) &&
        (!yearTo || car.carModel.manufacturingEndYear <= yearTo) &&
        (!minPrice || Number(car.price) >= Number(minPrice)) &&
        (!maxPrice || Number(car.price) <= Number(maxPrice)) &&
        (!minEngineSize || Number(car.engineSize.engineSize) >= Number(minEngineSize)) &&
        (!maxEngineSize || Number(car.engineSize.engineSize) <= Number(maxEngineSize)) &&
        (!minMileage || car.mileage >= minMileage) &&
        (!maxMileage || car.mileage <= maxMileage)
      );
    });
  
    console.log("Filtered Cars After Search:", filtered);
    setFilteredCars(filtered);
  };
  
  

  
  useEffect(() => {
    if (cars.length > 0) {
      handleSearch(new Event("submit") as unknown as React.FormEvent<HTMLFormElement>);
    }
  }, [
    brand,
    model,
    bodyType,
    fuelType,
    selectedDrivetrain,
    selectedColor,
    minPrice,
    maxPrice,
    yearFrom,
    yearTo,
    minEngineSize,
    maxEngineSize,
    minMileage,
    maxMileage,
    cars,
  ]);
  



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
                  <option key={option.id} value={option.id.toString()}>
                    {langCtx?.language === "jp" ? option.brandJapanese : option.brandEnglish}
                  </option>
                ))}
              </select>

              <label htmlFor="model">{langCtx?.translate.model}</label>
              <select id="model-type" value={model} onChange={(e) => setModel(e.target.value)} disabled={!brand}>
                <option value="">{ModelTypeOptions.length === 0 ? langCtx?.translate.noModel : langCtx?.translate.chooseModel}</option>
                {ModelTypeOptions.map(option => (
                  <option key={option.id} value={option.id.toString()}>
                    {langCtx?.language === "jp" ? option.modelNameJapanese : option.modelNameEnglish}
                  </option>
                ))}
              </select>

              <label htmlFor="bodyType">{langCtx?.translate.bodyType}</label>
              <select id="body-type" value={bodyType} onChange={(e) => setBodyType(e.target.value)}>
                <option value="">{langCtx?.translate.chooseBodyType}</option>
                {bodyTypeOptions.map(option => (
                  <option key={option.id} value={option.id.toString()}>
                    {langCtx?.language === "jp" ? option.nameJapanese : option.nameEnglish}
                  </option>
                ))}
              </select>

              <label htmlFor="fuelType">{langCtx?.translate.fuelType}</label>
              <select id="fuel-type" value={fuelType} onChange={(e) => setFuelType(e.target.value)}>
                <option value="">{langCtx?.translate.chooseFuel}</option>
                {fuelTypeOptions.map(option => (
                  <option key={option.id} value={option.id.toString()}>
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
                        {option.type}
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
          {filteredCars && filteredCars.length > 0 ? (
        filteredCars.map((car, index) => {
          console.log(`Car ${index}:`, car); // Debugging
          return (
            <div key={car?.id || index}>
             <h2>{car?.brand?.brandEnglish || "Unknown Brand"}</h2>
      <p>{car?.carModel?.modelNameEnglish || "Unknown Model"}</p>
      <p>{car?.bodyType?.nameEnglish || "Unknown Body Type"}</p>
      <p>{car?.fuelType?.nameEnglish || "Unknown Fuel Type"}</p>
      <p>{car?.location?.locationName || "Unknown Location"}</p>
      <p>{langCtx?.translate.price}: {Number(car?.price).toLocaleString()} Ft</p>
      <p>{langCtx?.translate.mileage}: {car?.mileage ?? "N/A"} km</p>
      <p>{langCtx?.translate.engineSize}: {car?.engineSize?.engineSize ?? "N/A"} cm³</p>
      <a href={(`/Car-Details?car=${encodeURIComponent(JSON.stringify(car))}`)}
      >
        {langCtx?.translate.details || "Részletek"}
      </a>
      </div>
    );
  })
) : (
  <p>No cars available</p>
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