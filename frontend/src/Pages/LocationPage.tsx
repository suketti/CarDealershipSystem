import {useContext, useEffect, useState} from "react";
import {getAllLocations} from "../api/locationService.ts";
import {AddressDTO, LocationDTO} from "../Types";
import '../locationPage.css';
import {LanguageCtx} from "../App.tsx";
import { Link } from "react-router-dom";
import "./LocationPage.stylesheet.css";

const LocationsPage: React.FC = () => {
    const [locations, setLocations] = useState<LocationDTO[]>([]);
    const langCtx = useContext(LanguageCtx);
    useEffect(() => {
        const fetchLocations = async () => {
            try {
                const locationsData = await getAllLocations();
                setLocations(locationsData);
            } catch (error) {
                console.error('Error fetching locations:', error);
            }
        };

        fetchLocations();
    }, []);


    const renderAddress = (address: AddressDTO) => {
        if (langCtx?.language === "jp") {
            return `${address.prefecture.nameJP}${address.city}${address.street}`;
        } else if (langCtx?.language === "hu") {
            return `${address.streetRomanized},  ${address.cityRomanized},  ${address.prefecture.name} prefektúra  `;
        } else {
            return `${address.streetRomanized}, ${address.cityRomanized}, ${address.prefecture.name}`;
        }
    };

    return (


    <div className="locations-page">
        <h1 className="locations-title">{langCtx?.translate.locations}</h1>
        <ul className="locations-list">
            {locations.map((location) => (
                <li key={location.id} className="location-item">
                    <Link to={`/cars?location=${location.id}`} className="location-link">
                        <h2 className="location-name">{location.locationName}</h2>
                        <p className="location-address">{renderAddress(location.address)}</p>
                        <p className="location-phone">{langCtx?.translate.phoneNumber}: {location.phoneNumber}</p>
                        <p className="location-capacity">{langCtx?.translate.capacity}: {location.maxCapacity}</p>
                    </Link>
                </li>
            ))}
        </ul>
    </div>

);
};

export default LocationsPage;