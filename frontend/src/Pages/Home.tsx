import React, { useContext, useState, useEffect } from "react";
import "../styles.css";
import { useNavigate } from "react-router-dom";
import { translations } from "../translations";
import { LanguageCtx } from "../App";
import { getAllMakerTypes, getAllModelsTypes, getAllBodyTypes, getAllTransmissionTypes, getAllFuelTypes, getAllColors, getAllDrivetrainTypes } from "../api/carMetadataService.ts";
import { BodyTypeDTO, CarMakerDTO, CarModelDTO, ColorDTO, DrivetrainTypeDTO, FuelTypeDTO, TransmissionTypeDTO } from "../Types";
import { CarDTO } from "../Types";
import { getCars } from "../api/carService.ts";
import { getBaseUrl } from "../api/axiosInstance.ts";

function Home({ language }: { language: "hu" | "en" | "jp" }) {
  const navigate = useNavigate();
  const [cars, setCars] = useState<CarDTO[]>([]);
  const [showLocationModal, setShowLocationModal] = useState(false);
  const [showSearchResults, setShowSearchResults] = useState(false);
  const [showLoginModal, setShowLoginModal] = useState(false);
  const [brandOptions, setBrandOptions] = useState<string[]>([]);
  const [modelOptions, setModelOptions] = useState<string[]>([]);
  const [isAdvancedSearchVisible, setIsAdvancedSearchVisible] = useState(false);
  const [bodyTypeOptions, setBodyTypeOptions] = useState<BodyTypeDTO[]>([]);
  const [fuelTypeOptions, setFuelTypeOptions] = useState<FuelTypeDTO[]>([]);
  const [transmissionTypeOptions, setTransmissionTypeOptions] = useState<TransmissionTypeDTO[]>([]);
  const [colorTypeOptions, setColorTypeOptions] = useState<ColorDTO[]>([]);
  const [drivetrainTypeOptions, setDrivetrainTypeOptions] = useState<DrivetrainTypeDTO[]>([]);
  const [makerTypeOptions, setMakerTypeOptions] = useState<CarMakerDTO[]>([]);
  const [modelTypeOptions, setModelTypeOptions] = useState<CarModelDTO[]>([]);
  const [selectedBrand, setSelectedBrand] = useState("");
  const [filteredModels, setFilteredModels] = useState<CarModelDTO[]>([]);

  const langCtx = useContext(LanguageCtx);

  useEffect(() => {
    async function fetchStuff() {
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
      const cars = await getCars();
      setCars(cars);
      console.log(cars);
    }
    fetchStuff();
  }, []);

  useEffect(() => {
    if (selectedBrand) {
      const filtered = modelTypeOptions.filter(model => model.maker.id === parseInt(selectedBrand));
      setFilteredModels(filtered);
    } else {
      setFilteredModels([]);
    }
  }, [selectedBrand, modelTypeOptions]);

  const handleSearchClick = (e: React.FormEvent) => {
    e.preventDefault();

    const queryParams = new URLSearchParams();

    // Brand
    if (selectedBrand) queryParams.set("brand", selectedBrand);

    // Model
    const modelSelect = document.getElementById("model-type") as HTMLSelectElement;
    if (modelSelect && modelSelect.value) queryParams.set("model", modelSelect.value);

    // Body Type
    const bodyTypeSelect = document.getElementById("body-type") as HTMLSelectElement;
    if (bodyTypeSelect && bodyTypeSelect.value) queryParams.set("bodyType", bodyTypeSelect.value);

    // Fuel Type
    const fuelTypeSelect = document.getElementById("fuel-type") as HTMLSelectElement;
    if (fuelTypeSelect && fuelTypeSelect.value) queryParams.set("fuelType", fuelTypeSelect.value);

    // Drivetrain Type
    const drivetrainTypeSelect = document.getElementById("drivetrain-type") as HTMLSelectElement;
    if (drivetrainTypeSelect && drivetrainTypeSelect.value) queryParams.set("drivetrain", drivetrainTypeSelect.value);

    // Color
    const colorSelect = document.getElementById("color-type") as HTMLSelectElement;
    if (colorSelect && colorSelect.value) queryParams.set("color", colorSelect.value);

    // Min Price
    const minPriceInput = document.getElementById("min-price") as HTMLInputElement;
    if (minPriceInput && minPriceInput.value) queryParams.set("minPrice", minPriceInput.value);

    // Max Price
    const maxPriceInput = document.getElementById("max-price") as HTMLInputElement;
    if (maxPriceInput && maxPriceInput.value) queryParams.set("maxPrice", maxPriceInput.value);

    // Year From
    const yearFromInput = document.getElementById("year-from") as HTMLInputElement;
    if (yearFromInput && yearFromInput.value) queryParams.set("yearFrom", yearFromInput.value);

    // Year To
    const yearToInput = document.getElementById("year-to") as HTMLInputElement;
    if (yearToInput && yearToInput.value) queryParams.set("yearTo", yearToInput.value);

    // Min Engine Size (cm³)
    const minEngineSizeInput = document.getElementById("motor-meret-min") as HTMLInputElement;
    if (minEngineSizeInput && minEngineSizeInput.value) queryParams.set("minEngineSize", minEngineSizeInput.value);

    // Max Engine Size (cm³)
    const maxEngineSizeInput = document.getElementById("motor-meret-max") as HTMLInputElement;
    if (maxEngineSizeInput && maxEngineSizeInput.value) queryParams.set("maxEngineSize", maxEngineSizeInput.value);

    // Min Mileage (km)
    const minMileageInput = document.getElementById("km-min") as HTMLInputElement;
    if (minMileageInput && minMileageInput.value) queryParams.set("minMileage", minMileageInput.value);

    // Max Mileage (km)
    const maxMileageInput = document.getElementById("km-max") as HTMLInputElement;
    if (maxMileageInput && maxMileageInput.value) queryParams.set("maxMileage", maxMileageInput.value);

    // Navigate to Cars page with query parameters
    navigate(`/cars?${queryParams.toString()}`);
  };

  const getRandomCars = (cars, num) => {
    if (cars.length <= num) return cars;
    const shuffledCars = cars.sort(() => 0.5 - Math.random());
    return shuffledCars.slice(0, num);
  };

  return (
      <div className="content">
        {/* Keresési eredmények modal */}
        {showSearchResults && (
            <div id="search-results-modal" className="modal-overlay">
              <div className="modal-content">
                <span className="close-modal" onClick={() => setShowSearchResults(false)}>&times;</span>
                <h2>{langCtx?.translate.searchResult}</h2>
                <div className="car-list-results"></div>
              </div>
            </div>
        )}

        {/* Hero szekció */}
        <section className="hero">
          <h2>{langCtx?.translate.welcomeText}</h2>
          <p>{langCtx?.translate.searchCars}</p>
        </section>

        {/* Autókereső szűrő */}
        <section className="filter">
          <div className="container">
            <div className="search-form-container">
              <h2 className="search-form-title">{langCtx?.translate.searchTitle}</h2>

              <form className="search-form" onSubmit={handleSearchClick}>
                {/* Márka */}
                <div className="form-field">
                  <label htmlFor="brand-type">{langCtx?.translate.brand}</label>
                  <select
                      id="brand-type"
                      value={selectedBrand}
                      onChange={(e) => setSelectedBrand(e.target.value)}
                  >
                    <option value="">{langCtx?.translate.chooseBrand}</option>
                    {makerTypeOptions.map(option => (
                        <option key={option.id} value={option.id}>
                          {langCtx?.language === "jp" ? option.brandJapanese : option.brandEnglish}
                        </option>
                    ))}
                  </select>
                </div>

                {/* Model */}
                <div className="form-field">
                  <label htmlFor="model-type">{langCtx?.translate.model}</label>
                  <select
                      id="model-type"
                      disabled={filteredModels.length === 0}
                  >
                    <option value="">{filteredModels.length === 0 ? langCtx?.translate.noModel : langCtx?.translate.chooseModel}</option>
                    {filteredModels.map(option => (
                        <option key={option.id} value={option.id}>
                          {langCtx?.language === "jp" ? option.modelNameJapanese : option.modelNameEnglish}
                        </option>
                    ))}
                  </select>
                </div>

                {/* Karosszéria típus */}
                <div className="form-field">
                  <label htmlFor="body-type">{langCtx?.translate.bodyType}</label>
                  <select id="body-type">
                    <option value="">{langCtx?.translate.chooseBodyType}</option>
                    {bodyTypeOptions.map(option => (
                        <option key={option.id} value={option.id}>
                          {langCtx?.language === "jp" ? option.nameJapanese : option.nameEnglish}
                        </option>
                    ))}
                  </select>
                </div>

                {/* Üzemanyag */}
                <div className="form-field">
                  <label htmlFor="fuel-type">{langCtx?.translate.fuel}</label>
                  <select id="fuel-type">
                    <option value="">{langCtx?.translate.chooseFuel}</option>
                    {fuelTypeOptions.map(option => (
                        <option key={option.id} value={option.id}>
                          {langCtx?.language === "jp" ? option.nameJapanese : option.nameEnglish}
                        </option>
                    ))}
                  </select>
                </div>

                {/* Ár */}
                <div className="form-field">
                  <label htmlFor="min-price">{langCtx?.translate.Price}</label>
                  <div className="range-inputs">
                    <input type="number" id="min-price" placeholder="Min Ft" />
                    <input type="number" id="max-price" placeholder="Max Ft" />
                  </div>
                </div>

                {/* Év */}
                <div className="form-field">
                  <label htmlFor="year-from">{langCtx?.translate.Year}</label>
                  <div className="range-inputs">
                    <input type="number" id="year-from" placeholder="Min év" />
                    <input type="number" id="year-to" placeholder="Max év" />
                  </div>
                </div>

                {/* Részletes keresés kapcsoló */}
                <button
                    type="button"
                    className="advanced-search-toggle"
                    onClick={() => setIsAdvancedSearchVisible(!isAdvancedSearchVisible)}
                >
                  <span>{langCtx?.translate.moreSearch}</span>
                  <span className="toggle-arrow">{isAdvancedSearchVisible ? "▲" : "▼"}</span>
                </button>

                {/* Részletes keresési mezők */}
                {isAdvancedSearchVisible && (
                    <div className="advanced-search-fields">
                      {/* Hajtás */}
                      <div className="form-field">
                        <label htmlFor="drivetrain-type">{langCtx?.translate.drive}</label>
                        <select id="drivetrain-type">
                          <option value="">{langCtx?.translate.chooseDrivetrain}</option>
                          {drivetrainTypeOptions.map(option => (
                              <option key={option.id} value={option.id}>
                                {option.type}
                              </option>
                          ))}
                        </select>
                      </div>

                      {/* Szín */}
                      <div className="form-field">
                        <label htmlFor="color-type">{langCtx?.translate.color}</label>
                        <select id="color-type">
                          <option value="">{langCtx?.translate.chooseColor}</option>
                          {colorTypeOptions.map(option => (
                              <option key={option.id} value={option.id}>
                                {langCtx?.language === "jp" ? option.colorNameJapanese : option.colorNameEnglish}
                              </option>
                          ))}
                        </select>
                      </div>

                      {/* Motor méret */}
                      <div className="form-field">
                        <label htmlFor="motor-meret-min">{langCtx?.translate.engineSize}</label>
                        <div className="range-inputs">
                          <input type="number" id="motor-meret-min" placeholder="Min cm³" />
                          <input type="number" id="motor-meret-max" placeholder="Max cm³" />
                        </div>
                      </div>

                      {/* Futott km */}
                      <div className="form-field">
                        <label htmlFor="km-min">{langCtx?.translate.mileageNum}</label>
                        <div className="range-inputs">
                          <input type="number" id="km-min" placeholder="Min km" />
                          <input type="number" id="km-max" placeholder="Max km" />
                        </div>
                      </div>
                    </div>
                )}

                {/* Keresés gomb */}
                <div className="search-form-button">
                  <button type="submit" className="search-button">
                    {langCtx?.translate.search}
                  </button>
                </div>
              </form>
            </div>
          </div>
        </section>

        <section className="inventory">
          <div className="container">
            <h2 className="section-title">{langCtx?.translate.allCar}</h2>
            <div className="cars-list">
              {getRandomCars(cars, 6)?.map((car) => {
                const carImage = car.images?.length ? getBaseUrl() + car.images[0].url : null;

                return (
                    <div key={car.id} className="car-item">
                      {carImage && (
                          <img
                              src={carImage}
                              alt={
                                langCtx?.language === 'jp'
                                    ? `${car.brand.brandJapanese} ${car.carModel.modelNameJapanese}`
                                    : `${car.brand.brandEnglish} ${car.carModel.modelNameEnglish}`
                              }
                              className="car-image"
                          />
                      )}
                      <h3>
                        {langCtx?.language === 'jp'
                            ? `${car.brand.brandJapanese} ${car.carModel.modelNameJapanese}`
                            : `${car.brand.brandEnglish} ${car.carModel.modelNameEnglish}`}
                      </h3>
                      <p>
                        {langCtx?.translate.year} {car.carModel.manufacturingStartYear} - {car.carModel.manufacturingEndYear}
                      </p>
                      <p>
                        {langCtx?.translate.modelCode} {car.carModel.modelCode}
                      </p>
                      <p>
                        {langCtx?.translate.type} {langCtx?.language === 'jp' ? car.bodyType.nameJapanese : car.bodyType.nameEnglish}
                      </p>
                      <p>
                        {langCtx?.translate.fuelType} {langCtx?.language === 'jp' ? car.fuelType.nameJapanese : car.fuelType.nameEnglish}
                      </p>
                      <p>{langCtx?.translate.location}: {car.location.locationName}</p>
                      <p>
                        {langCtx?.translate.price} {Number(car.price).toLocaleString()} yen
                      </p>
                      <button
                          className="btn"
                          onClick={() => navigate(`/Car-Details?carId=${encodeURIComponent(car?.id)}`)}
                      >
                        {langCtx?.translate.viewDetails}
                      </button>
                    </div>
                );
              })}
            </div>
          </div>
        </section>

        {/* Bejelentkezési modal */}
        {showLoginModal && (
            <div id="login-modal" className="modal">
              <div className="modal-content">
                <span className="close-modal" onClick={() => setShowLoginModal(false)}>&times;</span>
                <h2>{langCtx?.translate.loginTitle}</h2>
                <form id="login-form">
                  <label>{langCtx?.translate.username}</label>
                  <input type="text" id="username" name="username" required />

                  <label>{langCtx?.translate.password}</label>
                  <input type="password" id="password" name="password" required />

                  <button type="submit" className="btn">{langCtx?.translate.loginTitle}</button>
                </form>
              </div>
            </div>
        )}
      </div>
  );
}

export default Home;