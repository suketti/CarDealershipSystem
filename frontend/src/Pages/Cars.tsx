import React, { useState, useEffect } from "react";
import {Link, useLocation, useSearchParams} from "react-router-dom";
import {CarDTO, DrivetrainTypeDTO, LocationDTO} from "../Types";
import { getCars } from "../api/carService.ts";
import styles from './Cars.module.css'; // Import the CSS module
import { LanguageCtx } from "../App";
import { useContext } from "react";
import api, {getBaseUrl} from "../api/axiosInstance.ts";
import {
  getAllMakerTypes,
  getAllModelsTypes,
  getAllBodyTypes,
  getAllTransmissionTypes,
  getAllFuelTypes,
  getAllColors,
  getAllDrivetrainTypes,
  getModelsByBrand
} from "../api/carMetadataService.ts";
import {BodyTypeDTO, CarMakerDTO, CarModelDTO, ColorDTO, FuelTypeDTO, TransmissionTypeDTO} from "../Types";
import {getAllLocations} from "../api/locationService.ts";

function Cars() {
  const location = useLocation();
  const [cars, setCars] = useState<CarDTO[]>([]);
  const [filteredCars, setFilteredCars] = useState<CarDTO[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [language, setLanguage] = useState<"hu" | "en" | "ja">("hu");
  const langCtx = useContext(LanguageCtx);

  const [brand, setBrand] = useState<CarMakerDTO | null>(null);
  const [model, setModel] = useState<CarModelDTO | null>(null);
  const [bodyType, setBodyType] = useState<BodyTypeDTO | null>(null);
  const [fuelType, setFuelType] = useState<FuelTypeDTO | null>(null);
  const [bodyTypeOptions, setBodyTypeOptions] = useState<BodyTypeDTO[]>([]);
  const [fuelTypeOptions, setFuelTypeOptions] = useState<FuelTypeDTO[]>([]);
  const [shopLocation, setshopLocation] = useState<LocationDTO | null>(null)
  const [shopLocations, setshopLocations] = useState<LocationDTO[]>([])
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
  const [selectedDrivetrain, setSelectedDrivetrain] = useState<DrivetrainTypeDTO | null>(null);
  const [minEngineSize, setMinEngineSize] = useState<number | null>(null);
  const [maxEngineSize, setMaxEngineSize] = useState<number | null>(null);
  const [minMileage, setMinMileage] = useState<number | null>(null);
  const [maxMileage, setMaxMileage] = useState<number | null>(null);
  const [isAdvancedSearchVisible, setIsAdvancedSearchVisible] = useState(false);
  const [params] = useSearchParams();


  useEffect(() => {
    const fetchData = async () => {
      try {
        const [
          carsData,
          bodyTypes,
          fuelTypes,
          transmissions,
          drivetrains,
          colors,
          makers,
          models,
          locations
        ] = await Promise.all([
          getCars(),
          getAllBodyTypes(),
          getAllFuelTypes(),
          getAllTransmissionTypes(),
          getAllDrivetrainTypes(),
          getAllColors(),
          getAllMakerTypes(),
          getAllModelsTypes(),
          getAllLocations()
        ]);

        // Check the structure of the fetched data and log it
        console.log("Fetched Cars:", carsData);
        console.log("Fetched Body Types:", bodyTypes);
        console.log("Fetched Fuel Types:", fuelTypes);
        console.log("Fetched Transmissions:", transmissions);
        console.log("Fetched Drivetrains:", drivetrains);
        console.log("Fetched Colors:", colors);
        console.log("Fetched Makers Data:", makers);
        console.log("Fetched Models:", models);

        // Ensure that the data is in the expected structure
        if (carsData && bodyTypes && fuelTypes && transmissions && drivetrains && colors && makers && models && locations) {
          setCars(carsData);
          setFilteredCars(carsData); // Initialize filteredCars with all cars

          setBodyTypeOptions(bodyTypes || []);
          setFuelTypeOptions(fuelTypes || []);
          setTransmissionTypeOptions(transmissions || []);
          setColorTypeOptions(colors || []);
          setDrivetrainTypeOptions(drivetrains || []);
          setMakerTypeOptions(makers || []);
          setModelTypeOptions(models || []);
          setshopLocations(locations || []);
        } else {
          console.error("Data structure is not as expected");
        }
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchData();
  }, []);


  useEffect(() => {
    const params = new URLSearchParams(location.search);

    const brandId = params.get("brand");
    const modelId = params.get("model");
    const bodyTypeId = params.get("bodyType");
    const fuelTypeId = params.get("fuelType");
    const locationId = params.get("location");
    const driveTrainId = params.get("drivetrain");
    const yearFrom = params.get("yearFrom");
    const yearTo = params.get("yearTo");
    const minMileage = params.get("minMileage");
    const maxMileage = params.get("maxMileage");

    // Handle brand
    if (brandId) {
      const foundBrand = MakerTypeOptions?.find(
          (maker: CarMakerDTO) => maker.id.toString() === brandId
      );
      setBrand(foundBrand || null);
    }

    // Handle model
    if (modelId) {
      const foundModel = filteredModels?.find(
          (model) => model.id.toString() === modelId
      );
      setModel(foundModel || null);
    }

    // Handle bodyType
    if (bodyTypeId) {
      const foundBodyType = bodyTypeOptions?.find(
          (bodyType) => bodyType.id.toString() === bodyTypeId
      );
      setBodyType(foundBodyType || null);
    }

    // Handle fuelType
    if (fuelTypeId) {
      const foundFuelType = fuelTypeOptions?.find(
          (fuelType) => fuelType.id.toString() === fuelTypeId
      );
      setFuelType(foundFuelType || null);
    }

    if (locationId) {
      const foundLocationType = shopLocations?.find(
          (shopLocation) => shopLocation.id.toString() === locationId
      );
      setshopLocation(foundLocationType || null);
    }

    if (driveTrainId) {
      const foundDriveTrainType = DrivetrainTypeOptions?.find(
          (driveTrainType) => driveTrainType.id.toString() === driveTrainId
      );
      setSelectedDrivetrain(foundDriveTrainType || null);
    }

    if (yearFrom) {
      setYearFrom(Number(yearFrom));
    }

    if (yearTo) {
      setYearTo(Number(yearTo));
    }


    if (minMileage) {
      setMinMileage(Number(minMileage));
    }


    if (maxMileage) {
      setMaxMileage(Number(maxMileage));
    }

  }, [location.search, MakerTypeOptions, filteredModels, bodyTypeOptions, fuelTypeOptions, DrivetrainTypeOptions]);

  useEffect(() => {
    const fetchModelsByBrand = async (brand: CarMakerDTO) => {
      try {
        // Ensure we don't make unnecessary API calls if the brand hasn't changed
        if (!brand?.id) return;

        const response = await getModelsByBrand(brand.id); // Assuming `getModelsByBrand` fetches models
        setFilteredModels(response || []); // Set models data if available
        setModel(null); // Reset model when the brand changes
      } catch (error) {
        console.error("Error fetching models:", error);
      }
    };

    if (brand?.id) {
      fetchModelsByBrand(brand); // Trigger fetching models when the brand is selected
    }
  }, [brand]);

// Separate useEffect for filtering based on state changes
  useEffect(() => {
    const filtered = cars.filter((car) => {
      return (
          (!brand || car.brand.id === brand.id) &&
          (!model || car.carModel.id === model.id) &&
          (!bodyType || car.bodyType.id === bodyType.id) &&
          (!fuelType || car.fuelType.id === fuelType.id) &&
          (!selectedDrivetrain || car.driveTrain.id === selectedDrivetrain.id) &&
          (!selectedColor || car.color.colorNameEnglish.toLowerCase() === selectedColor.toLowerCase()) &&
          (!yearFrom || car.carModel.manufacturingStartYear >= yearFrom) &&
          (!yearTo || car.carModel.manufacturingEndYear <= yearTo) &&
          (!minPrice || Number(car.price) >= Number(minPrice)) &&
          (!maxPrice || Number(car.price) <= Number(maxPrice)) &&
          (!minEngineSize || Number(car.engineSize.engineSize) >= Number(minEngineSize)) &&
          (!maxEngineSize || Number(car.engineSize.engineSize) <= Number(maxEngineSize)) &&
          (!minMileage || car.mileage >= minMileage) &&
          (!maxMileage || car.mileage <= maxMileage) &&
          (!shopLocation || car.location.id === shopLocation.id)
      );
    });

    setFilteredCars(filtered);
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
    shopLocation,
    cars, // Only re-run the filtering effect when the cars data changes
  ]);

  const handleSearch = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault(); // Prevent default form submission behavior

    const filtered = cars.filter((car) => {
      // Validate year range if both are provided
      const isYearValid = (!yearFrom || !yearTo || yearFrom <= yearTo);

      // Validate price range if both are provided
      const isPriceValid = (!minPrice || !maxPrice || Number(minPrice) <= Number(maxPrice));

      // Validate engine size range if both are provided
      const isEngineSizeValid = (!minEngineSize || !maxEngineSize || Number(minEngineSize) <= Number(maxEngineSize));

      return (
          isYearValid &&
          isPriceValid &&
          isEngineSizeValid &&
          (!brand || car.brand.id === brand.id) &&
          (!model || car.carModel.id === model.id) &&
          (!bodyType || car.bodyType.id === bodyType.id) &&
          (!fuelType || car.fuelType.id === fuelType.id) &&
          (!selectedDrivetrain || car.driveTrain === selectedDrivetrain) &&
          (!selectedColor || car.color.colorNameEnglish.toLowerCase() === selectedColor.toLowerCase()) &&
          (!yearFrom || car.carModel.manufacturingStartYear >= yearFrom) &&
          (!yearTo || car.carModel.manufacturingEndYear <= yearTo) &&
          (!minPrice || Number(car.price) >= Number(minPrice)) &&
          (!maxPrice || Number(car.price) <= Number(maxPrice)) &&
          (!minEngineSize || Number(car.engineSize.engineSize) >= Number(minEngineSize)) &&
          (!maxEngineSize || Number(car.engineSize.engineSize) <= Number(maxEngineSize)) &&
          (!minMileage || car.mileage >= minMileage) &&
          (!maxMileage || car.mileage <= maxMileage) &&
          (!shopLocation || car.location.id === shopLocation.id)
      );
    });


    setFilteredCars(filtered); // Update the state with filtered cars
  };


  return (
    <div className={styles['cars-page']}>
      <div className={styles.container}>
        <div className={styles['cars-layout']}>
          <aside className={styles['filter-sidebar']}>
            <h2>{langCtx?.translate.searchTitle}</h2>
            <form onSubmit={handleSearch}>
              <label htmlFor="brand">{langCtx?.translate.brand}</label>
              <select
                  id="brand-type"
                  value={brand ? (langCtx?.language === "jp" ? brand.brandJapanese : brand.brandEnglish) : ""}
                  onChange={(e) => {
                    const selectedBrand = MakerTypeOptions.find(option =>
                        langCtx?.language === "jp" ? option.brandJapanese === e.target.value : option.brandEnglish === e.target.value
                    );
                    setBrand(selectedBrand || null); // Ensure selectedBrand is set correctly
                    setModel(null); // Reset model when brand changes
                    setBodyType(null); // Reset body type as well
                  }}
              >
                <option value="">{langCtx?.translate.chooseBrand}</option>
                {MakerTypeOptions?.map(option => (
                    <option
                        key={option.id}
                        value={langCtx?.language === "jp" ? option.brandJapanese : option.brandEnglish}
                    >
                      {langCtx?.language === "jp" ? option.brandJapanese : option.brandEnglish}
                    </option>
                ))}
              </select>

              <label htmlFor="model">{langCtx?.translate.model}</label>
              <select
                  id="model-type"
                  value={model ? model.id.toString() : ""}
                  onChange={(e) => {
                    const selectedModel = filteredModels.find(option => option.id.toString() === e.target.value);
                    setModel(selectedModel || null);
                  }}
                  disabled={!brand} // Disable the model dropdown until a brand is selected
              >
                <option value="">{filteredModels.length === 0 ? langCtx?.translate.noModel : langCtx?.translate.chooseModel}</option>
                {filteredModels.map(option => (
                    <option key={option.id} value={option.id.toString()}>
                      {langCtx?.language === "jp" ? option.modelNameJapanese : option.modelNameEnglish}
                    </option>
                ))}
              </select>


              {/* Body Type Select */}
              <label htmlFor="bodyType">{langCtx?.translate.bodyType}</label>
              <select
                  id="body-type"
                  value={bodyType ? bodyType.id.toString() : ""}
                  onChange={(e) => {
                    const selectedBodyType = bodyTypeOptions.find(option => option.id.toString() === e.target.value);
                    setBodyType(selectedBodyType || null);
                  }}
              >
                <option value="">{langCtx?.translate.chooseBodyType}</option>
                {bodyTypeOptions?.map(option => (
                    <option key={option.id} value={option.id.toString()}>
                      {langCtx?.language === "jp" ? option.nameJapanese : option.nameEnglish}
                    </option>
                ))}
              </select>

              {/* Fuel Type Select */}
              <label htmlFor="fuelType">{langCtx?.translate.fuelType}</label>
              <select
                  id="fuel-type"
                  value={fuelType ? fuelType.id.toString() : ""}
                  onChange={(e) => {
                    const selectedFuelType = fuelTypeOptions.find(option => option.id.toString() === e.target.value);
                    setFuelType(selectedFuelType || null);
                  }}
              >
                <option value="">{langCtx?.translate.chooseFuel}</option>
                {fuelTypeOptions?.map(option => (
                    <option key={option.id} value={option.id.toString()}>
                      {langCtx?.language === "jp" ? option.nameJapanese : option.nameEnglish}
                    </option>
                ))}
              </select>

              <label>{langCtx?.translate.minPrice}</label>
              <input type="number" id="min-price" name="min-price" placeholder="yen" onChange={(e) => setMinPrice(Number(e.target.value))} />

              <label>{langCtx?.translate.maxPrice}</label>
              <input type="number" id="max-price" name="max-price" placeholder="yen" onChange={(e) => setMaxPrice(Number(e.target.value))} />

              <label>{langCtx?.translate.yearFrom}</label>
              <input type="number" id="year-from" placeholder="2010" onChange={(e) => setYearFrom(Number(e.target.value))} />

              <label>{langCtx?.translate.yearTo}</label>
              <input type="number" id="year-to" placeholder="2015" onChange={(e) => setYearTo(Number(e.target.value))} />

              <label htmlFor="location">{langCtx?.translate.location}</label>
              <select
                  id="location-type"
                  value={shopLocation ? shopLocation.id.toString() : ""} // Use the selected location's ID as value
                  onChange={(e) => {
                    const selectedLocation = shopLocations.find(option => option.id.toString() === e.target.value);
                    setshopLocation(selectedLocation || null); // Set the selected location or null if none selected
                  }}
              >
                <option value="">{shopLocations.length === 0 ? langCtx?.translate.noLocation : langCtx?.translate.chooseLocation}</option>
                {shopLocations.map(option => (
                    <option key={option.id} value={option.id.toString()}>
                      {option.locationName}
                    </option>
                ))}
              </select>



              <div id="reszletes-kereses-container" onClick={() => setIsAdvancedSearchVisible(!isAdvancedSearchVisible)}>
                <span id="reszletes-kereses-label">
                  {langCtx?.translate.moreSearch}
                </span>
                <span id="toggle-arrow">{isAdvancedSearchVisible ? "▲" : "▼"}</span>
              </div>

              {isAdvancedSearchVisible && (
                <div id="reszletes-feltetelek">
                  <label>{langCtx?.translate.drive}</label>
                  <select
                      id="drivetrain-type"
                      value={selectedDrivetrain?.type || ""}
                      onChange={(e) => {
                        const selectedOption = DrivetrainTypeOptions.find(option => option.type === e.target.value);
                        setSelectedDrivetrain(selectedOption || null);
                      }}
                  >
                    <option value="">{langCtx?.translate.chooseDrivetrain}</option>
                    {DrivetrainTypeOptions.map(option => (
                        <option key={option.id} value={option.type}>
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
              <button type="submit" className={styles.btn}>{langCtx?.translate.search}</button>
            </form>
          </aside>

          <main className={styles['cars-list']}>
  <div className="card-container"> {/* Add a container for the cards */}
  {filteredCars && filteredCars.length > 0 ? (
        filteredCars.map((car, index) => {
          const carImage = car.images?.length ? getBaseUrl() + car.images[0].url : null;
          return (
            <div key={car?.id || index} className="card"> {/* Add the card class */}
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
              <div className="card-body"> {/* Add the card-body class */}
                <h2 className="card-title">
                  {langCtx?.language === "jp" ? car?.brand?.brandJapanese : car?.brand?.brandEnglish || "Unknown Brand"}
                </h2>
                <p className="card-description">
                  {langCtx?.language === "jp"
                      ? car?.carModel?.modelNameJapanese + ` (` + (car?.carModel?.modelCode) + `)`
                      : car?.carModel?.modelNameEnglish + ` (` + (car?.carModel?.modelCode) + `)` || "Unknown Model"}
                </p>
                <p className="card-description">
                  {langCtx?.language === "jp" ? car?.bodyType?.nameJapanese : car?.bodyType?.nameEnglish || "Unknown Body Type"}
                </p>
                <p className="card-description">
                  {langCtx?.language === "jp" ? car?.fuelType?.nameJapanese : car?.fuelType?.nameEnglish || "Unknown Fuel Type"}
                </p>
                <p className="card-description">
                  {car?.location?.locationName || "Unknown Location"}
                </p>
                <p className="card-price">
                  {langCtx?.translate.price} {Number(car?.price).toLocaleString()} yen
                </p>
                <p className="card-description">
                  {langCtx?.translate.mileage} {car?.mileage ?? "N/A"} km
                </p>
                <p className="card-description">
                  {langCtx?.translate.engineSize} {car?.engineSize?.engineSize ?? "N/A"} cm³
                </p>
                
<a href={`/Car-Details?carId=${encodeURIComponent(car?.id)}`} className="details-button">
  {langCtx?.translate.details || "Részletek"}
</a>
                    </div>
                  </div>
    );
  })
) : (
  <p>{langCtx?.translate.noCars}</p>
)}
          </div>
        </main>
        </div>
      </div>
    </div>
  );
}

export default Cars;