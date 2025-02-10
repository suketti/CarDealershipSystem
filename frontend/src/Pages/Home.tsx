import React, { useContext, useState } from "react";
import "../styles.css";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { translations } from "../translations";
import { LanguageCtx } from "../App";
import {getAllBodyTypes} from "../api/carMetadataService.ts";
import {BodyTypeDTO} from "../Types";

const cars = [
  { marka: "Toyota", modell: "Prius", ev: 2015, kivitel: "Hatchback", uzemanyag: "Hibrid", ar: 4500000, kep: "" },
  { marka: "Nissan", modell: "Leaf", ev: 2018, kivitel: "Hatchback", uzemanyag: "Elektromos", ar: 5200000, kep: "" },
  { marka: "Honda", modell: "Civic", ev: 2020, kivitel: "Hatchback", uzemanyag: "Benzin", ar: 6000000, kep: "" },
  { marka: "Honda", modell: "Civic", ev: 2020, kivitel: "Hatchback", uzemanyag: "Benzin", ar: 6000000, kep: "" },
  { marka: "Honda", modell: "Civic", ev: 2020, kivitel: "Hatchback", uzemanyag: "Benzin", ar: 6000000, kep: "" },
  { marka: "Honda", modell: "Civic", ev: 2020, kivitel: "Hatchback", uzemanyag: "Benzin", ar: 6000000, kep: "" }

];

function Home({ language }: { language: "hu" | "en" }) {
  const navigate = useNavigate();
  const [showLocationModal, setShowLocationModal] = useState(false);
  const [showSearchResults, setShowSearchResults] = useState(false);
  const [showLoginModal, setShowLoginModal] = useState(false);
  const [brandOptions, setBrandOptions] = useState<string[]>([]);
  const [modelOptions, setModelOptions] = useState<string[]>([]);
  const [fuelOptions, setFuelOptions] = useState<string[]>([]);
  const [isAdvancedSearchVisible, setIsAdvancedSearchVisible] = useState(false);
  const [selectedBrand, setSelectedBrand] = useState("");
  const [bodyTypeOptions, setBodyTypeOptions] = useState<BodyTypeDTO[]>([]);
  
  const langCtx = useContext(LanguageCtx)

    useEffect(() => {
        async function fetchStuff() {
            const resp = await getAllBodyTypes();
            setBodyTypeOptions(resp);
        }
        fetchStuff();
    }, []);

  const handleSearchClick = (e: React.FormEvent) => {
    e.preventDefault(); // Megakadályozza az oldal újratöltését
    navigate("/cars"); // Átirányít az autók oldalra
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
          <select value={selectedBrand} onChange={e => setSelectedBrand(e.target.value)}>
            {brandOptions.map(option => <option key={option} value={option}>{option}</option>)}
          </select>

          <label>{langCtx?.translate.model}</label>
          <select disabled={selectedBrand === ""}>
            {modelOptions.map(option => <option key={option} value={option}>{option}</option>)}
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
            <select id="fuel">{fuelOptions.map(option => <option key={option} value={option}>{option}</option>)}</select>

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
                <select id="meghajtas">
                  
                </select>

                <label>{langCtx?.translate.color}</label>
                <input type="text" id="szin" placeholder="Pl. piros, fekete" />

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
            {cars.map((car, index) => (
              <div key={index} className="car-item">
                <img src={car.kep} alt={`${car.marka} ${car.modell}`} className="car-image" />
                <h3>{car.marka} {car.modell}</h3>
                <p>{langCtx?.translate.year} {car.ev}</p>
                <p>{langCtx?.translate.type} {car.kivitel}</p>
                <p>{langCtx?.translate.fuelType} {car.uzemanyag}</p>
                <p>{langCtx?.translate.price} {car.ar.toLocaleString()} Ft</p>
                <button className="btn" onClick={() => navigate(`/Car-Details?car=${encodeURIComponent(JSON.stringify(car))}`)}>
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
