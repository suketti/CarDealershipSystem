import React, { useContext, useState } from "react";
import "../styles.css";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { translations } from "../translations";
import { LanguageCtx } from "../App";
import {getAllMakerTypes, getAllModelsTypes, getAllBodyTypes, getAllTransmissionTypes, getAllFuelTypes, getAllColors, getAllDrivetrainTypes} from "../api/carMetadataService.ts";
import {BodyTypeDTO, CarMakerDTO, CarModelDTO, ColorDTO, FuelTypeDTO, TransmissionTypeDTO} from "../Types";
import Cars from "./Cars.tsx";
import { CarDTO } from "../Types";
import { getCars } from "../api/carService.ts";




function Home({ language }: { language: "hu" | "en" }) {
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
  const [DrivetrainTypeOptions, setDrivetrainTypeOptions] = useState<ColorDTO[]>([]);
  const [MakerTypeOptions, setMakerTypeOptions] = useState<CarMakerDTO[]>([]);
  const [ModelTypeOptions, setModelTypeOptions] = useState<CarModelDTO[]>([]);
  const [selectedBrand, setSelectedBrand] = useState("");
  const [filteredModels, setFilteredModels] = useState<CarModelDTO[]>([]);

  


  const langCtx = useContext(LanguageCtx)

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
            console.log(cars)
        }
        fetchStuff();
    }, []);

    useEffect(() => {
      if (selectedBrand) {
        const filtered = ModelTypeOptions.filter(model => model.maker.id === parseInt(selectedBrand));
        setFilteredModels(filtered);
      } else {
        setFilteredModels([]);
      }
    }, [selectedBrand, ModelTypeOptions]);

    const handleSearchClick = (e: React.FormEvent) => {
      e.preventDefault();
  
      const queryParams = new URLSearchParams();
      if (selectedBrand) queryParams.set("brand", selectedBrand);
      if (filteredModels.length > 0) queryParams.set("model", filteredModels[0].id.toString());
      if (bodyTypeOptions.length > 0) queryParams.set("bodyType", bodyTypeOptions[0].id.toString());
      if (fuelTypeOptions.length > 0) queryParams.set("fuelType", fuelTypeOptions[0].id.toString());
  
      navigate(`/cars?${queryParams.toString()}`);
  };
  
  return (
    <div className="content">
      <header>
        <h1>{langCtx?.translate.dealership}</h1>
        
      </header>
      
      {/* Helyszínválasztó modal */}
      {showLocationModal && (
        <div className="modal-overlay">
          <div className="modal-content">
            <h2>{langCtx?.translate.selectLocation}</h2>
            
            {/* Térkép konténer */}
            <div className="map-container">
              <img 
                src="/Képek/magyarorszag-terkep.jpg" 
                alt="Magyarország térképe" 
                className="map-image" 
              />
              
              {/* Helyszín gombok */}
              <button 
                className="map-point" 
                style={{ top: "40%", left: "45%", cursor: "pointer" }}
                onClick={() => console.log("Budapest kiválasztva")}
              >
                Budapest
              </button>
              <button 
                className="map-point" 
                style={{ top: "37%", left: "80%", cursor: "pointer" }}
                onClick={() => console.log("Debrecen kiválasztva")}
              >
                Debrecen
              </button>
              <button 
                className="map-point" 
                style={{ top: "20%", left: "70%", cursor: "pointer" }}
                onClick={() => console.log("Miskolc kiválasztva")}
              >
                Miksolc
              </button>
              <button 
                className="map-point" 
                style={{ top: "36%", left: "10%", cursor: "pointer" }}
                onClick={() => console.log("Sopron kiválasztva")}
              >
                Sopron
              </button>
            </div>

            {/* Bezárás gomb */}
            <button 
              className="btn" 
              onClick={() => setShowLocationModal(false)}
            >
              {langCtx?.translate.closed}
            </button>
          </div>
        </div>
      )}
      
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
        <a className="btn" href="/cars">{langCtx?.translate.viewCars}</a>
      </section>
      
      {/* Autókereső szűrő */}
      <section className="filter">
        <div className="container">
  
  <h2>{langCtx?.translate.searchTitle}</h2>
          <form onSubmit={handleSearchClick}>
          <label>{langCtx?.translate.brand}</label>
          <select id="brand-type" value={selectedBrand} onChange={(e) => setSelectedBrand(e.target.value)}>
                  <option value="">{langCtx?.translate.chooseBrand}</option>
                  {MakerTypeOptions.map(option => (
                      <option key={option.id} value={option.id}>
                          {langCtx?.language === "jp" ? option.brandJapanese : option.brandEnglish}
                      </option>
                  ))}
              </select>

          <label>{langCtx?.translate.model}</label>
          <select id="model-type" disabled={filteredModels.length === 0}>
        <option value="">{filteredModels.length === 0 ? langCtx?.translate.noModel : langCtx?.translate.chooseModel}</option>
                  {ModelTypeOptions.map(option => (
                      <option key={option.id} value={option.id}>
                          {langCtx?.language === "jp" ? option.modelNameJapanese : option.modelNameEnglish}
                      </option>
                  ))}
              </select>

              <label>{langCtx?.translate.bodyType}</label>
              <select id="body-type">
                  {bodyTypeOptions.map(option => (
                      <option key={option.id} value={option.id}>
                          {langCtx?.language === "jp" ? option.nameJapanese : option.nameEnglish}
                      </option>
                  ))}
              </select>

            <label>{langCtx?.translate.fuel}</label>
            <select id="fuel-type">
                  {fuelTypeOptions.map(option => (
                      <option key={option.id} value={option.id}>
                          {langCtx?.language === "jp" ? option.nameJapanese : option.nameEnglish}
                      </option>
                  ))}
              </select>

            <label>{langCtx?.translate.minPrice}</label>
            <input type="number" id="min-price" name="min-price" placeholder="Ft" />
            
            <label>{langCtx?.translate.maxPrice}</label>
            <input type="number" id="max-price" name="max-price" placeholder="Ft" />

            <label>{langCtx?.translate.yearFrom}</label>
            <input type="number" id="year-from" placeholder="Pl. 2010" />

            <label>{langCtx?.translate.yearTo}</label>
            <input type="number" id="year-to" placeholder="Pl. 2020" />

            <div id="reszletes-kereses-container">
              <span id="reszletes-kereses-label" onClick={() => setIsAdvancedSearchVisible(!isAdvancedSearchVisible)}>
                {langCtx?.translate.moreSearch}
              </span>
              <span id="toggle-arrow">{isAdvancedSearchVisible ? "▲" : "▼"}</span>
            </div>

            {isAdvancedSearchVisible && (
              <div id="reszletes-feltetelek">
                <label>{langCtx?.translate.drive}</label>
                <select id="drivetrain-type">
                  {DrivetrainTypeOptions.map(option => (
                      <option key={option.id} value={option.id}>
                      </option>
                  ))}
              </select>

                <label>{langCtx?.translate.color}</label>
                <select id="body-type">
                  {colorTypeOptions.map(option => (
                      <option key={option.id} value={option.id}>
                          {langCtx?.language === "jp" ? option.colorNameJapanese : option.colorNameEnglish}
                      </option>
                  ))}
              </select>

                <label>{langCtx?.translate.engineSize}</label>
                <input type="number" id="motor-meret-min" placeholder="Min cm³" />
                <input type="number" id="motor-meret-max" placeholder="Max cm³" />

                <label>{langCtx?.translate.mileageNum}</label>
                <input type="number" id="km-min" placeholder="Min km" />
                <input type="number" id="km-max" placeholder="Max km" />
              </div>
            )}

            <button 
            type="button"
        className="btn" 
        onClick={() => setShowLocationModal(true)}
      >
        {langCtx?.translate.chooseLocation}
      </button>
      <button 
              type="button" 
              className="btn" 
              onClick={handleSearchClick} // Keresés gombra kattintva átirányítás
            >
              {langCtx?.translate.search}
            </button>
          </form>
        </div>
      </section>
      
      {/* Autólista */}
      <section className="inventory">
        <div className="container">
          <h2 className="section-title">{langCtx?.translate.allCar}</h2>
          <div className="cars-list">
          {cars?.map((car) => (
      <div key={car.id} className="car-item">
        
    <h3>{car.brand.brandEnglish} {car.carModel.modelNameEnglish}</h3>
    <p>{langCtx?.translate.year} {car.carModel.manufacturingStartYear} - {car.carModel.manufacturingEndYear}</p>
    <p>{langCtx?.translate.type} {car.bodyType.nameEnglish}</p>
    <p>{langCtx?.translate.fuelType} {car.fuelType.nameEnglish}</p>
    <p>{langCtx?.translate.price} {Number(car.price).toLocaleString()} Ft</p>
    <button className="btn" onClick={() => navigate(`/Car-Details?carId=${car.id}`)}>
      {langCtx?.translate.viewDetails}
    </button>
  

      </div>
))}
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
