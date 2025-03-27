import React, { useEffect, useState } from 'react';
import '../locationPage.css';
import {AddressDTO, LocationDTO} from "../Types";
import {getAllLocations} from "../api/locationService.ts";
import {useNavigate} from "react-router-dom";
import { translations } from "../translations";
import { useContext } from 'react';
import { LanguageCtx } from "../App";


const LocationPage: React.FC = () => {
  const [locations, setLocations] = useState<LocationDTO[]>();
  const [selectedLocation, setSelectedLocation] = useState<LocationDTO  | null>(null);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
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


  const handleLocationClick = (location: LocationDTO) => {
    setSelectedLocation(location);
  };
  
  const handleSearchClick = (event: React.MouseEvent, locationId: number) => {
    // Prevent the location item click event from triggering
    event.stopPropagation();

    navigate(`/cars?location=${locationId}`);

    };

  // Create an embed URL for Google Maps based on the selected location
    const getEmbedMapUrl = (address: AddressDTO) => {
        const fullAddress = `${address.street}, ${address.city}, ${address.prefecture.name}, ${address.postalCode}`;
        return `https://maps.google.com/maps?q=${encodeURIComponent(fullAddress)}&t=m&z=15&output=embed&iwloc=near`;
    };
    
  return (
    <div className="locations-page">
      <h1 className="locations-title">{langCtx?.translate.locationDealership}</h1>
      
      <div className="locations-container">
        {/* Left side - Locations list */}
        <div className="locations-list-container">
          {loading ? (
            <div className="loading-message">{langCtx?.translate.loadingLocation}</div>
          ) : !Array.isArray(locations) || locations.length === 0 ? (
            <div className="empty-message">{langCtx?.translate.noLocation}</div>
          ) : (
            <ul className="locations-list">
              {locations.map(location => (
                <li 
                  key={location.id}
                  className={`location-item ${selectedLocation?.id === location.id ? 'active' : ''}`}
                  onClick={() => handleLocationClick(location)}
                >
                  <h2 className="location-name">{location.locationName}</h2>
                  <div className="location-address">
                    <span className="location-address-icon">📍</span>
                    <span>{location.address.street}</span>
                  </div>
                  <div className="location-phone">
                    <span className="location-icon">📞</span>
                    <span>{location.phoneNumber}</span>
                  </div>
                  <div className="location-capacity">
                    <span className="location-icon">🚗</span>
                    <span>{langCtx?.translate.capacity} {location.maxCapacity} / {location.maxCapacity}</span>
                  </div>
                  <button 
                    className="location-search-button"
                    onClick={(e) => handleSearchClick(e, location.id)}
                  >
                    {langCtx?.translate.search}
                  </button>
                </li>
              ))}
            </ul>
          )}
        </div>
        
        {/* Right side - Map (embedded iframe) */}
        <div className="map-container">
          {selectedLocation ? (
              <div className="map-embed-container">
                <div className="map-selected-info">
                  <h3 className="map-location-name">{selectedLocation.locationName}</h3>
                  <p className="map-selected-address">{selectedLocation.address.prefecture.nameJP + selectedLocation.address.city + selectedLocation.address.street}</p>
                </div>
                <iframe
                    className="map-iframe"
                    src={getEmbedMapUrl(selectedLocation.address)} // ✅ No more type error
                    width="100%"
                    height="100%"
                    style={{border: 0}}
                    allowFullScreen
                    loading="lazy"
                    referrerPolicy="no-referrer-when-downgrade"
                    title={`Map showing ${selectedLocation.locationName}`}
                />
              </div>
          ) : (
              <div className="map-placeholder">
                <div className="map-instructions-initial">
                  <h3>{langCtx?.translate.locationMap}</h3>
                  <p>{langCtx?.translate.selectLocationMap}</p>
                  <div className="map-icon">🗺️</div>
                </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default LocationPage;