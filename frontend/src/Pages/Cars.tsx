import React, { useState, useEffect } from "react";
import { translations } from "../translations";
import { useLocation } from "react-router-dom";
import { Car } from "../Types";

const initialCars: Car[] = [
  {
    marka: "Toyota",
    modell: "Prius",
    ev: 2015,
    kivitel: "Hatchback",
    uzemanyag: "Hibrid",
    ar: 4500000,
    kep: "",
    leiras:
      "A Toyota Prius az egyik legismertebb hibrid autó, amely kiváló üzemanyag-hatékonyságáról híres.",
    varos: "Budapest"
  },
  {
    marka: "Nissan",
    modell: "Leaf",
    ev: 2018,
    kivitel: "Hatchback",
    uzemanyag: "Elektromos",
    ar: 5200000,
    kep: "",
    leiras:
      "A Nissan Leaf egy teljesen elektromos autó, amely tökéletes a városi közlekedéshez.",
    varos: "Debrecen"
  },
  {
    marka: "Honda",
    modell: "Civic",
    ev: 2020,
    kivitel: "Hatchback",
    uzemanyag: "Benzin",
    ar: 6000000,
    kep: "",
    leiras: "A Honda Civic egy megbízható és sportos hatchback, modern dizájnnal.",
    varos: "Miskolc"
  },
  {
    marka: "Ford",
    modell: "Focus",
    ev: 2017,
    kivitel: "Hatchback",
    uzemanyag: "Dízel",
    ar: 3800000,
    kep: "",
    leiras:
      "A Ford Focus egy kényelmes és takarékos dízel hatchback autó, amely családok számára is ideális.",
    varos: "Sopron"
  },
  {
    marka: "Volkswagen",
    modell: "Golf",
    ev: 2019,
    kivitel: "Hatchback",
    uzemanyag: "Benzin",
    ar: 5500000,
    kep: "",
    leiras: "A Volkswagen Golf egy ikonikus benzines autó, amely kiemelkedő vezetési élményt nyújt.",
    varos: "Budapest"
  }
];

function Cars() {
  const location = useLocation();
  const [cars] = useState<Car[]>(initialCars);

  // Nyelv
  const [language, setLanguage] = useState<"hu" | "en">("hu");
  const t = translations[language] || translations.hu;

  // Szűrő state-ek
  const [brand, setBrand] = useState("");
  const [model, setModel] = useState("");
  const [kivitel, setKivitel] = useState("");
  const [fuel, setFuel] = useState("");
  const [minPrice, setMinPrice] = useState("");
  const [maxPrice, setMaxPrice] = useState("");
  const [yearFrom, setYearFrom] = useState("");
  const [yearTo, setYearTo] = useState("");
  const [selectedLocations, setSelectedLocations] = useState<string[]>([]);

  const [filteredCars, setFilteredCars] = useState<Car[]>(initialCars);

  // Helyszín modal
  const [showLocationModal, setShowLocationModal] = useState(false);

  // Egy segédfüggvény: egyedi értékek kiszűrése
  const uniqueValues = (arr: Car[], key: keyof Car) => {
    return Array.from(new Set(arr.map((item) => item[key]))) as string[];
  };
  useEffect(() => {
    const params = new URLSearchParams(location.search);

    const brand = params.get("brand")?.toLowerCase();
    const model = params.get("model")?.toLowerCase();
    const kivitel = params.get("kivitel")?.toLowerCase();
    const fuel = params.get("fuel")?.toLowerCase();
    const minPrice = params.get("minPrice") ? parseInt(params.get("minPrice")!) : 0;
    const maxPrice = params.get("maxPrice") ? parseInt(params.get("maxPrice")!) : Infinity;
    const yearFrom = params.get("yearFrom") ? parseInt(params.get("yearFrom")!) : 0;
    const yearTo = params.get("yearTo") ? parseInt(params.get("yearTo")!) : Infinity;

    const filtered = initialCars.filter(car => {
      return (
        (!brand || car.marka.toLowerCase() === brand) &&
        (!model || car.modell.toLowerCase() === model) &&
        (!kivitel || car.kivitel.toLowerCase() === kivitel) &&
        (!fuel || car.uzemanyag.toLowerCase() === fuel) &&
        (car.ar >= minPrice && car.ar <= maxPrice) &&
        (car.ev >= yearFrom && car.ev <= yearTo)
      );
    });

    setFilteredCars(filtered);
  }, [location.search]);
  const brands = uniqueValues(cars, "marka");
  const models = uniqueValues(cars, "modell");
  const bodyTypes = uniqueValues(cars, "kivitel");
  const fuels = uniqueValues(cars, "uzemanyag");

  const handleSearch = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const filtered = cars.filter((car) => {
      const cbrand = brand ? car.marka.toLowerCase() === brand.toLowerCase() : true;
      const cmodel = model ? car.modell.toLowerCase() === model.toLowerCase() : true;
      const ckiv = kivitel ? car.kivitel.toLowerCase() === kivitel.toLowerCase() : true;
      const cfuel = fuel ? car.uzemanyag.toLowerCase() === fuel.toLowerCase() : true;

      const cminprice = minPrice ? car.ar >= parseInt(minPrice) : true;
      const cmaxprice = maxPrice ? car.ar <= parseInt(maxPrice) : true;
      const cyearFrom = yearFrom ? car.ev >= parseInt(yearFrom) : true;
      const cyearTo = yearTo ? car.ev <= parseInt(yearTo) : true;

      const cLocation =
        selectedLocations.length === 0 || selectedLocations.includes(car.varos);

      return (
        cbrand &&
        cmodel &&
        ckiv &&
        cfuel &&
        cminprice &&
        cmaxprice &&
        cyearFrom &&
        cyearTo &&
        cLocation
      );
    });
    setFilteredCars(filtered);
  };

  const handleMapPointClick = (location: string) => {
    if (selectedLocations.includes(location)) {
      setSelectedLocations(selectedLocations.filter((loc) => loc !== location));
    } else {
      setSelectedLocations([...selectedLocations, location]);
    }
  };

  return (
    <div className="cars-page">
      <div className="container">
        <div className="cars-layout">
          <aside className="filter-sidebar">
            <h2>{t.searchTitle}</h2>
            <form onSubmit={handleSearch}>
              <label htmlFor="brand">{t.brand}</label>
              <select id="brand" value={brand} onChange={(e) => setBrand(e.target.value)}>
                <option value="">{t.choose}</option>
                {brands.map((b) => (
                  <option key={b} value={b}>
                    {b}
                  </option>
                ))}
              </select>

              <label htmlFor="model">{t.model}</label>
              <select id="model" value={model} onChange={(e) => setModel(e.target.value)}>
                <option value="">{t.choose}</option>
                {models.map((m) => (
                  <option key={m} value={m}>
                    {m}
                  </option>
                ))}
              </select>

              <label htmlFor="body-type">{t.bodyType}</label>
              <select
                id="body-type"
                value={kivitel}
                onChange={(e) => setKivitel(e.target.value)}
              >
                <option value="">{t.choose}</option>
                {bodyTypes.map((bt) => (
                  <option key={bt} value={bt}>
                    {bt}
                  </option>
                ))}
              </select>

              <label htmlFor="fuel">{t.fuel}</label>
              <select id="fuel" value={fuel} onChange={(e) => setFuel(e.target.value)}>
                <option value="">{t.choose}</option>
                {fuels.map((f) => (
                  <option key={f} value={f}>
                    {f}
                  </option>
                ))}
              </select>

              <label htmlFor="min-price">{t.minPrice}</label>
              <input
                type="number"
                id="min-price"
                placeholder="Ft"
                value={minPrice}
                onChange={(e) => setMinPrice(e.target.value)}
              />

              <label htmlFor="max-price">{t.maxPrice}</label>
              <input
                type="number"
                id="max-price"
                placeholder="Ft"
                value={maxPrice}
                onChange={(e) => setMaxPrice(e.target.value)}
              />

              <label htmlFor="year-from">{t.yearFrom}</label>
              <input
                type="number"
                id="year-from"
                placeholder={t.eg + " 2010"}
                value={yearFrom}
                onChange={(e) => setYearFrom(e.target.value)}
              />

              <label htmlFor="year-to">{t.yearTo}</label>
              <input
                type="number"
                id="year-to"
                placeholder={t.eg + " 2020"}
                value={yearTo}
                onChange={(e) => setYearTo(e.target.value)}
              />

              <button type="button" className="btn" onClick={() => setShowLocationModal(true)}>
                {t.chooseLocation}
              </button>
              <button type="submit" className="btn">
                {t.search}
              </button>
            </form>
          </aside>

          <main className="cars-list">
            {filteredCars.length === 0 ? (
              <p>{t.noResults}</p>
            ) : (
              filteredCars.map((car, idx) => (
                <div key={idx} className="car-item">
                  <img src={car.kep} alt={`${car.marka} ${car.modell}`} />
                  <h3>
                    {car.marka} {car.modell}
                  </h3>
                  <p>
                    <strong>{t.price}</strong> {car.ar.toLocaleString()} Ft
                  </p>
                  <p>
                    <strong>{t.year}</strong> {car.ev}
                  </p>
                  <p>
                    <strong>{t.type}</strong> {car.kivitel}
                  </p>
                  <p>
                    <strong>{t.fuelType}</strong> {car.uzemanyag}
                  </p>
                  <a
                    href={`/autok/reszletek?car=${encodeURIComponent(
                      JSON.stringify(car)
                    )}`}
                    className="btn"
                  >
                    {t.details}
                  </a>
                </div>
              ))
            )}
          </main>
        </div>
      </div>

      {showLocationModal && (
        <div id="location-overlay" style={{ display: "block" }}>
          <div
            id="location-modal"
            style={{
              display: "block",
              background: "#fff",
              padding: "20px",
              borderRadius: "8px",
              width: "600px",
              margin: "50px auto",
              position: "relative"
            }}
          >
            <h2>{t.selectLocation}</h2>
            <div className="map-container" style={{ position: "relative" }}>
              <img
                src="/Képek/magyarorszag-terkep.jpg"
                alt="Magyarország térképe"
                className="map-image"
                style={{ width: "100%", borderRadius: "8px" }}
              />
              <button
                className="map-point"
                style={{ top: "40%", left: "45%" }}
                onClick={() => handleMapPointClick("Budapest")}
              >
                Budapest
              </button>
              <button
                className="map-point"
                style={{ top: "37%", left: "80%" }}
                onClick={() => handleMapPointClick("Debrecen")}
              >
                Debrecen
              </button>
              <button
                className="map-point"
                style={{ top: "20%", left: "70%" }}
                onClick={() => handleMapPointClick("Miskolc")}
              >
                Miskolc
              </button>
              <button
                className="map-point"
                style={{ top: "36%", left: "10%" }}
                onClick={() => handleMapPointClick("Sopron")}
              >
                Sopron
              </button>
            </div>
            <button className="btn" onClick={() => setShowLocationModal(false)}>
              {t.complete}
            </button>
          </div>
        </div>
      )}
    </div>
  );
}

export default Cars;
