import React, { useEffect, useState, useRef } from 'react';
import '../locationPage.css';
import {AddressDTO, LocationDTO} from "../Types";
import {getAllLocations, getLocationCapacity} from "../api/locationService.ts";
import {useNavigate} from "react-router-dom";
import { translations } from "../translations";
import { useContext } from 'react';
import { LanguageCtx } from "../App";

const LocationPage: React.FC = () => {
  const [locations, setLocations] = useState<LocationDTO[]>();
  const [selectedLocation, setSelectedLocation] = useState<LocationDTO | null>(null);
  const [loading, setLoading] = useState(false);
  const [isMobile, setIsMobile] = useState(window.innerWidth <= 768);
  const [locationCapacities, setLocationCapacities] = useState<{ [key: number]: { currentUsage: number, maxCapacity: number } }>({});
  const selectedLocationRef = useRef<HTMLLIElement | null>(null);
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
    
    // Handle resize events to detect mobile view
    const handleResize = () => {
      setIsMobile(window.innerWidth <= 768);
    };
    
    window.addEventListener('resize', handleResize);
    return () => {
      window.removeEventListener('resize', handleResize);
    };
  }, []);


  useEffect(() => {
    const fetchCapacities = async () => {
      if (!locations) return;

      const capacities = await Promise.all(
          locations.map(async (location) => {
            const capacity = await getLocationCapacity(location.id);
            return { id: location.id, ...capacity };
          })
      );

      setLocationCapacities(capacities.reduce((acc, loc) => {
        acc[loc.id] = { currentUsage: loc.currentUsage, maxCapacity: loc.maxCapacity };
        return acc;
      }, {} as { [key: number]: { currentUsage: number, maxCapacity: number } }));
    };

    fetchCapacities();
  }, [locations]);

  useEffect(() => {
    // Scroll to the selected location when in mobile view
    if (isMobile && selectedLocation && selectedLocationRef.current) {
      selectedLocationRef.current.scrollIntoView({
        behavior: 'smooth',
        block: 'start'
      });
    }
  }, [selectedLocation, isMobile]);

  const handleLocationClick = (location: LocationDTO) => {
    // If we're clicking the same location, toggle it off
    if (selectedLocation && selectedLocation.id === location.id) {
      setSelectedLocation(null);
    } else {
      setSelectedLocation(location);
    }
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
  
  // Map component for reuse in both mobile and desktop views
  const MapComponent = ({ location }: { location: LocationDTO | null }) => (
    <div className="map-container">
      {location ? (
        <div className="map-embed-container">
          <div className="map-selected-info">
            <h3 className="map-location-name">{location.locationName}</h3>
            <p className="map-selected-address">{location.address.prefecture.nameJP + location.address.city + location.address.street}</p>
          </div>
          <iframe
            className="map-iframe"
            src={getEmbedMapUrl(location.address)}
            width="100%"
            height="100%"
            style={{border: 0}}
            allowFullScreen
            loading="lazy"
            referrerPolicy="no-referrer-when-downgrade"
            title={`Map showing ${location.locationName}`}
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
  );
    
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
              {locations.map(location => {
                const isActive = selectedLocation?.id === location.id;
                const showMapUnder = isMobile && isActive;
                
                return (
                    <React.Fragment key={location.id}>
                      <li
                          ref={isActive ? selectedLocationRef : null}
                          className={`location-item ${isActive ? 'active' : ''} ${showMapUnder ? 'location-item-with-map' : ''}`}
                          onClick={() => handleLocationClick(location)}
                      >
                        <h2 className="location-name">{location.locationName}</h2>

                        <div className="location-address">
                          <span className="location-address-icon">📍</span>
                          <span>
          {["en", "hu"].includes(langCtx?.language)
              ? location.address.streetRomanized
              : location.address.street}
        </span>
                        </div>

                        <div className="location-phone">
                          <span className="location-icon">📞</span>
                          <span>{location.phoneNumber}</span>
                        </div>

                        <div className="location-capacity">
                          <span className="location-icon">🚗</span>
                          <span>{langCtx?.translate.capacity} {locationCapacities[location.id]?.currentUsage ?? 0} / {locationCapacities[location.id]?.maxCapacity ?? location.maxCapacity}</span>
                        </div>

                        <button
                            className="location-search-button"
                            onClick={(e) => handleSearchClick(e, location.id)}
                        >
                          {langCtx?.translate.search}
                        </button>
                      </li>
                    
                    {/* Render map underneath this location item when in mobile and selected */}
                    {showMapUnder && (
                      <div className="map-container map-container-inline">
                        <MapComponent location={location} />
                      </div>
                    )}
                  </React.Fragment>
                );
              })}
            </ul>
          )}
        </div>
        
        {/* Right side - Map (only show in desktop or when no location is selected in mobile) */}
        {(!isMobile || !selectedLocation) && (
          <div className={`map-container ${isMobile ? 'map-container-mobile' : ''}`}>
            <MapComponent location={selectedLocation} />
          </div>
        )}
      </div>
    </div>
  );
};

export default LocationPage;