import React, { useEffect, useState } from 'react';
import '../locationPage.css';
import {AddressDTO, LocationDTO} from "../Types";
import {getAllLocations} from "../api/locationService.ts";
import {useNavigate} from "react-router-dom";


const LocationPage: React.FC = () => {
  const [locations, setLocations] = useState<LocationDTO[]>();
  const [selectedLocation, setSelectedLocation] = useState<LocationDTO  | null>(null);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  // Current date and user info - UPDATED
  const currentDateTime = "2025-03-27 10:31:01";
  const currentUser = "Kissdani05A";

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
    const fullAddress = `${address.postalCode}${address.prefecture.nameJP}${address.city}${address.street}`;
    return `https://maps.google.com/maps?q=${encodeURIComponent(fullAddress)}&t=m&z=15&output=embed&iwloc=near`;
  };

  return (
    <div className="locations-page">
      <h1 className="locations-title">Our Dealership Locations</h1>
      <div className="header-info">
        <p className="date-display">{currentDateTime}</p>
        <p className="user-display">Current User: {currentUser}</p>
      </div>
      
      <div className="locations-container">
        {/* Left side - Locations list */}
        <div className="locations-list-container">
          {loading ? (
            <div className="loading-message">Loading locations...</div>
          ) : !Array.isArray(locations) || locations.length === 0 ? (
            <div className="empty-message">No locations found</div>
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
                    <span>Capacity: {location.maxCapacity} / {location.maxCapacity}</span>
                  </div>
                  <button 
                    className="search-button"
                    onClick={(e) => handleSearchClick(e, location.id)}
                  >
                    Keresés
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
                <h3 className="map-location-name">{selectedLocation.name}</h3>
                <p className="map-selected-address">{selectedLocation.address}</p>
              </div>
              <iframe
                className="map-iframe"
                src={getEmbedMapUrl(selectedLocation.address)}
                width="100%"
                height="100%"
                style={{ border: 0 }}
                allowFullScreen
                loading="lazy"
                referrerPolicy="no-referrer-when-downgrade"
                title={`Map showing ${selectedLocation.name}`}
              ></iframe>
            </div>
          ) : (
            <div className="map-placeholder">
              <div className="map-instructions-initial">
                <h3>Location Map</h3>
                <p>Select a location from the list to see it on the map</p>
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