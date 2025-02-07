import React, { useContext, useState } from "react";
import "../styles.css";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { translations } from "../translations";
import { LanguageCtx } from "../App";

const cars = [
  { marka: "Toyota", modell: "Prius", ev: 2015, kivitel: "Hatchback", uzemanyag: "Hibrid", ar: 4500000, kep: "" },
  { marka: "Nissan", modell: "Leaf", ev: 2018, kivitel: "Hatchback", uzemanyag: "Elektromos", ar: 5200000, kep: "" },
  { marka: "Honda", modell: "Civic", ev: 2020, kivitel: "Hatchback", uzemanyag: "Benzin", ar: 6000000, kep: "" }
];

function Home({ language }: { language: "hu" | "en" }) {
  const navigate = useNavigate();
  const [showLocationModal, setShowLocationModal] = useState(false);
  const [showSearchResults, setShowSearchResults] = useState(false);
  const [showLoginModal, setShowLoginModal] = useState(false);
  const [brandOptions, setBrandOptions] = useState<string[]>([]);
  const [modelOptions, setModelOptions] = useState<string[]>([]);
  const [bodyTypeOptions, setBodyTypeOptions] = useState<string[]>([]);
  const [fuelOptions, setFuelOptions] = useState<string[]>([]);
  const [isAdvancedSearchVisible, setIsAdvancedSearchVisible] = useState(false);
  const [selectedBrand, setSelectedBrand] = useState("Mindegy");
  
  const langCtx = useContext(LanguageCtx)

  useEffect(() => {
    setBrandOptions(["Mindegy", ...new Set(cars.map(car => car.marka))]);
    setModelOptions(["Mindegy", ...new Set(cars.map(car => car.modell))]);
    setBodyTypeOptions(["Mindegy", ...new Set(cars.map(car => car.kivitel))]);
    setFuelOptions(["Mindegy", ...new Set(cars.map(car => car.uzemanyag))]);
  }, []);
  useEffect(() => {
    if (selectedBrand === "Mindegy") {
      setModelOptions(["Mindegy"]);
    } else {
      setModelOptions(["Mindegy", ...new Set(cars.filter(car => car.marka === selectedBrand).map(car => car.modell))]);
    }
  }, [selectedBrand]);
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
          <select disabled={selectedBrand === "Mindegy"}>
            {modelOptions.map(option => <option key={option} value={option}>{option}</option>)}
          </select>

            <label>{langCtx?.translate.bodyType}</label>
            <select id="body-type">{bodyTypeOptions.map(option => <option key={option} value={option}>{option}</option>)}</select>

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
                <label>Meghajtás:</label>
                <select id="meghajtas">
                  <option value="elso">Elsőkerék</option>
                  <option value="hatso">Hátsókerék</option>
                  <option value="osszkerek">Összkerék</option>
                </select>

                <label>Szín:</label>
                <input type="text" id="szin" placeholder="Pl. piros, fekete" />

                <label>Motor méret:</label>
                <input type="number" id="motor-meret-min" placeholder="Min cm³" />
                <input type="number" id="motor-meret-max" placeholder="Max cm³" />

                <label>Kilométeróra állás:</label>
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
          <h2>{langCtx?.translate.allCar}</h2>
          <div className="car-list-all">
            {cars.map((car, index) => (
              <div key={index} className="car-item">
                <img src={car.kep} alt={`${car.marka} ${car.modell}`} className="car-image" />
                <h3>{car.marka} {car.modell}</h3>
                <p>{langCtx?.translate.year} {car.ev}</p>
                <p>Kivitel: {car.kivitel}</p>
                <p>Üzemanyag: {car.uzemanyag}</p>
                <p>Ár: {car.ar.toLocaleString()} Ft</p>
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
            <h2>Bejelentkezés</h2>
            <form id="login-form">
              <label>Felhasználónév:</label>
              <input type="text" id="username" name="username" required />
              
              <label>Jelszó:</label>
              <input type="password" id="password" name="password" required />
              
              <button type="submit" className="btn">Bejelentkezés</button>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}

export default Home;
