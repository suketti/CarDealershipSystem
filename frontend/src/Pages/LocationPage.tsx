import React, { useEffect, useState } from 'react';
import axios from 'axios';
import '../locationPage.css';

// Define the Location interface based on your database schema
interface Location {
  id: number;
  name: string;
  address: string; // This will be the full address like "1-1-1 Marunouchi, Chiyoda-ku, Tokyo, 100-0005, Japan"
  phone: string;
  maxCapacity: number;
  currentCapacity?: number;
}

// Sample locations that will be used if the API fails
const sampleLocations: Location[] = [
  {
    id: 1,
    name: "Tokyo Main Branch",
    address: "1-1-1 Marunouchi, Chiyoda-ku, Tokyo, 100-0005, Japan",
    phone: "+81-3-1234-5678",
    maxCapacity: 120,
    currentCapacity: 87
  },
  {
    id: 2,
    name: "Osaka Downtown",
    address: "1-1-88 Oyodonaka, Kita-ku, Osaka, 531-0076, Japan",
    phone: "+81-6-8765-4321",
    maxCapacity: 95,
    currentCapacity: 62
  },
  {
    id: 3,
    name: "Yokohama Bay",
    address: "3-1-1 Minato Mirai, Nishi-ku, Yokohama, Kanagawa 220-0012, Japan",
    phone: "+81-45-123-4567",
    maxCapacity: 150,
    currentCapacity: 110
  },
  {
    id: 4,
    name: "Nagoya Central",
    address: "1-1-1 Meieki, Nakamura-ku, Nagoya, Aichi 450-0002, Japan",
    phone: "+81-52-555-8899",
    maxCapacity: 80,
    currentCapacity: 54
  },
  {
    id: 5,
    name: "Sapporo North",
    address: "4-1-1 Kita, Chuo-ku, Sapporo, Hokkaido 060-0001, Japan",
    phone: "+81-11-222-3344",
    maxCapacity: 65,
    currentCapacity: 42
  }
];

const LocationPage: React.FC = () => {
  const [locations, setLocations] = useState<Location[]>(sampleLocations); // Start with sample data
  const [selectedLocation, setSelectedLocation] = useState<Location | null>(null);
  const [loading, setLoading] = useState(false);
  
  // Current date and user info - UPDATED
  const currentDateTime = "2025-03-27 10:31:01";
  const currentUser = "Kissdani05A";

  useEffect(() => {
    const fetchLocations = async () => {
      try {
        setLoading(true);
        const response = await axios.get('/api/locations');
        
        // Check if response.data is an array
        if (Array.isArray(response.data) && response.data.length > 0) {
          setLocations(response.data);
        } else if (response.data && typeof response.data === 'object' && 
                   Array.isArray(response.data.locations) && response.data.locations.length > 0) {
          setLocations(response.data.locations);
        }
        // If data wasn't valid, we'll keep using the sample locations
        
      } catch (error) {
        console.warn("Could not fetch locations from API, using sample data");
        // Already using sample data as default, so no need to set it again
      } finally {
        setLoading(false);
      }
    };

    fetchLocations();
  }, []);

  const handleLocationClick = (location: Location) => {
    setSelectedLocation(location);
  };
  
  const handleSearchClick = (event: React.MouseEvent, locationId: number) => {
    // Prevent the location item click event from triggering
    event.stopPropagation();
    
    // Placeholder for search functionality
    alert(`Keresés a következő helyszínre: ${locations.find(loc => loc.id === locationId)?.name}`);
  };

  // Create an embed URL for Google Maps based on the selected location
  const getEmbedMapUrl = (address: string) => {
    const encodedAddress = encodeURIComponent(address);
    return `https://maps.google.com/maps?q=${encodedAddress}&t=m&z=15&output=embed&iwloc=near`;
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
                  <h2 className="location-name">{location.name}</h2>
                  <div className="location-address">
                    <span className="location-address-icon">📍</span>
                    <span>{location.address}</span>
                  </div>
                  <div className="location-phone">
                    <span className="location-icon">📞</span>
                    <span>{location.phone}</span>
                  </div>
                  <div className="location-capacity">
                    <span className="location-icon">🚗</span>
                    <span>Capacity: {location.currentCapacity} / {location.maxCapacity}</span>
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