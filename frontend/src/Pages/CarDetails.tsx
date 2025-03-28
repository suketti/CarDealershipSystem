import React, { useState, useEffect, useContext } from "react";
import { useLocation } from "react-router-dom";
import { CarDTO } from "../Types";
import { LanguageCtx } from "../App";
import { useUser } from "../UserContext.tsx";
import SavedCarService from "../api/savedCarService.ts";
import ReservationService from "../api/reservationService.ts";
import {getBaseUrl} from "../api/axiosInstance.ts";
import {getCarById} from "../api/carService.ts";
import { useSearchParams } from "react-router-dom";
import "../CarDetails.css"

// Define the reservation data interface to match your API
interface ReservationData {
  date: string;
  userId?: string;
  carId?: number;
}

function CarDetails() {
  const location = useLocation();
  const [carData, setCarData] = useState<CarDTO | null>(null);
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);
  const [selectedHour, setSelectedHour] = useState<number>(8);
  const [selectedMinute, setSelectedMinute] = useState<number>(0);
  const [lightboxImage, setLightboxImage] = useState<string | null>(null);
  const [bookingMessage, setBookingMessage] = useState<string>("");
  const [bookingSuccess, setBookingSuccess] = useState<boolean>(false);
  const [imageIndex, setImageIndex] = useState<number>(0);
  const [searchParams] = useSearchParams();
  const langCtx = useContext(LanguageCtx);
  
  // Calendar state
  const [currentMonth, setCurrentMonth] = useState<Date>(new Date());
  
  // Access user context for authentication and user data
  const { user, isAuthenticated } = useUser();

  useEffect(() => {
    const carId = searchParams.get("carId");

    if (carId) {
      getCarById(carId).then((fetchedCar) => {
        if (fetchedCar) {
          setCarData(fetchedCar);
        }
      }).catch(error => {
        console.error("Error fetching car details:", error);
      });
    }
  }, [searchParams]);

  // Use images from the car data
  const images = carData?.images?.length ?
      carData.images.map((img) => getBaseUrl() + img.url) : [];

  const nextImage = () => {
    if (images.length <= 1) return;
    setImageIndex((prev) => (prev + 1) % images.length);
  };

  const prevImage = () => {
    if (images.length <= 1) return;
    setImageIndex((prev) => (prev - 1 + images.length) % images.length);
  };

  const handleSaveCar = async () => {
    console.log("Mentett autó:", carData);

    if (!carData) return;

    // Ensure the user is authenticated
    if (!isAuthenticated) {
      alert(langCtx?.translate.saveCarErrorNoAuth);
      return;
    }

    try {
      const userId = user?.id; // Get user ID from context
      if (!userId) {
        alert(langCtx?.translate.saveCarErrorNoUser);
        return;
      }

      // Call the service to save the car with user ID
      await SavedCarService.saveCar(userId, carData.id);
      alert(langCtx?.translate.saveCarSuccess);
    } catch (error) {
      console.error("Hiba az autó mentésekor:", error);
      alert(langCtx?.translate.saveCarError);
    }
  };

  const handleBooking = async () => {
    // Reset the success state whenever we attempt a new booking
    setBookingSuccess(false);
    
    if (!isAuthenticated) {
      setBookingMessage(`${langCtx?.translate.loginToBook}`);
      return;
    }
    if (!selectedDate) {
      setBookingMessage(`${langCtx?.translate.chooseDate}`);
      return;
    }

    const now = new Date();
    now.setSeconds(0, 0);
    const selectedDateTime = new Date(selectedDate);
    selectedDateTime.setHours(selectedHour, selectedMinute, 0, 0);

    if (selectedDateTime < now) {
      setBookingMessage(`${langCtx?.translate.noPastDate}`);
      return;
    }

    try {
      const reservations = await ReservationService.getAllReservations();
      const existingBooking = reservations.find(res => new Date(res.date) > now);

      if (existingBooking) {
        setBookingMessage(
            `${langCtx?.translate.alreadyBookedFirst} ${existingBooking.date}. ${langCtx?.translate.alreadyBookedSecond}`
        );
        return;
      }

      // Create the reservation data with explicit type casting
      const reservationData: ReservationData = {
        date: selectedDateTime.toISOString(),
        userId: user?.id,
        carId: carData?.id
      };

      const newReservation = await ReservationService.createReservation(reservationData);
      setBookingMessage(`${langCtx?.translate.doneBookFirst} ${newReservation.date}\n ${langCtx?.translate.doneBookSecond}`);
      setBookingSuccess(true); // Set success to true for successful bookings
    } catch (error) {
      setBookingMessage(`${langCtx?.translate.errorBooking}`);
      setBookingSuccess(false);
    }
  };

  // Calendar navigation functions
  const prevMonth = () => {
    setCurrentMonth(new Date(currentMonth.getFullYear(), currentMonth.getMonth() - 1, 1));
  };

  const nextMonth = () => {
    setCurrentMonth(new Date(currentMonth.getFullYear(), currentMonth.getMonth() + 1, 1));
  };

  // Completely rebuilt calendar rendering function
  const renderCalendar = () => {
    const today = new Date();
    const year = currentMonth.getFullYear();
    const month = currentMonth.getMonth();
    
    // Get first day of month and number of days in month
    const firstDayOfMonth = new Date(year, month, 1);
    const daysInMonth = new Date(year, month + 1, 0).getDate();
    
    // Get day of week (0 = Sunday, 1 = Monday, etc.)
    let firstDayWeekday = firstDayOfMonth.getDay();
    
    // Adjust to make Monday = 0, Sunday = 6
    firstDayWeekday = firstDayWeekday === 0 ? 6 : firstDayWeekday - 1;
    
    // Calculate how many rows we need
    const totalDays = firstDayWeekday + daysInMonth;
    const numRows = Math.ceil(totalDays / 7);
    
    // Build calendar grid
    const rows = [];
    let day = 1;
    
    for (let i = 0; i < numRows; i++) {
      let rowCells = [];
      
      for (let j = 0; j < 7; j++) {
        if ((i === 0 && j < firstDayWeekday) || day > daysInMonth) {
          // Add empty cell
          rowCells.push(<td key={`empty-${i}-${j}`} className="calendar-cell empty-day">&nbsp;</td>);
        } else {
          const date = new Date(year, month, day);
          const isToday = date.toDateString() === today.toDateString();
          const isSelected = selectedDate && date.toDateString() === selectedDate.toDateString();
          const isPast = date < today && !isToday;
          
          rowCells.push(
            <td 
              key={`day-${day}`} 
              className={`calendar-cell calendar-day ${isToday ? 'today' : ''} ${isSelected ? 'selected' : ''} ${isPast ? 'past' : ''}`}
              onClick={() => !isPast && setSelectedDate(date)}
            >
              {day}
            </td>
          );
          day++;
        }
      }
      
      rows.push(<tr key={`row-${i}`}>{rowCells}</tr>);
    }
    
    return rows;
  };

  // Format month name
  const getMonthName = (date: Date): string => {
    return date.toLocaleString('default', { month: 'long', year: 'numeric' });
  };

  // Lightbox control functions
  const openLightbox = (index: number) => {
    setImageIndex(index);
    setLightboxImage(images[index]);
  };

  const closeLightbox = () => {
    setLightboxImage(null);
  };

  const lightboxPrev = (e: React.MouseEvent) => {
    e.stopPropagation();
    prevImage();
    setLightboxImage(images[imageIndex]);
  };

  const lightboxNext = (e: React.MouseEvent) => {
    e.stopPropagation();
    nextImage();
    setLightboxImage(images[imageIndex]);
  };

  if (!carData) {
    return <p>Betöltés...</p>;
  }

  return (
      <section className="car-details">
        {/* Image Gallery Section */}
        <div className="image-gallery">
          <div className="main-image-container">
            {images.length > 0 && (
                <img
                    id="main-car-image"
                    src={images[imageIndex]}
                    alt="Autó képe"
                    className="main-image"
                    onClick={() => openLightbox(imageIndex)}
                />
            )}

            {images.length > 1 && (
                <>
                  <button className="gallery-nav prev-button" onClick={prevImage}>
                    &#8249;
                  </button>
                  <button className="gallery-nav next-button" onClick={nextImage}>
                    &#8250;
                  </button>
                </>
            )}
          </div>

          {/* Thumbnails */}
          <div className="thumbnails-row">
            {images.length > 0 &&
                images.map((img, index) => {
                  const isActive = index === imageIndex;

                  // Calculate the range of thumbnails to display (e.g., 5 or 6 thumbnails)
                  const numThumbnails = 7; // This can be adjusted to display more thumbnails
                  const start = (imageIndex - Math.floor(numThumbnails / 2) + images.length) % images.length;
                  const end = (start + numThumbnails) % images.length;

                  let isVisible = false;
                  if (start < end) {
                    // Case when start is before end
                    isVisible = index >= start && index < end;
                  } else {
                    // Case when start wraps around the end of the array
                    isVisible = index >= start || index < end;
                  }

                  return (
                      <div
                          key={index}
                          className={`thumbnail-wrapper ${isActive ? "active" : ""}`}
                          onClick={() => setImageIndex(index)}
                          style={{ display: isVisible ? 'inline-block' : 'none' }}
                      >
                        <img
                            src={img}
                            alt={`Kép ${index + 1}`}
                            className="thumbnail-image"
                        />
                      </div>
                  );
                })
            }
          </div>
        </div>
      
      {/* Lightbox */}
      {lightboxImage && (
        <div className="lightbox-overlay" onClick={closeLightbox}>
          <div className="lightbox-content" onClick={(e) => e.stopPropagation()}>
            <button className="lightbox-close" onClick={closeLightbox}>
              ✕
            </button>
            
            <div className="lightbox-image-container">
              <img src={images[imageIndex]} alt="Nagyított kép" className="lightbox-image" />
              
              {images.length > 1 && (
                <>
                  <button className="lightbox-nav lightbox-prev" onClick={lightboxPrev}>
                    ‹
                  </button>
                  <button className="lightbox-nav lightbox-next" onClick={lightboxNext}>
                    ›
                  </button>
                </>
              )}
            </div>
            
            <div className="lightbox-thumbnails">
              {images.map((img, index) => (
                <div 
                  key={index} 
                  className={`lightbox-thumbnail ${index === imageIndex ? "active" : ""}`}
                  onClick={() => setImageIndex(index)}
                >
                  <img 
                    src={img} 
                    alt={`Thumbnail ${index + 1}`} 
                    className="lightbox-thumbnail-image"
                  />
                </div>
              ))}
            </div>
          </div>
        </div>
      )}

      {/* Car Details Section */}
      <div className="details">
        <button className="btn-save" onClick={handleSaveCar}>
          {langCtx?.translate.save}
        </button>

        <h2 id="car-title">
          {langCtx?.language === "jp"
              ? `${carData.brand.brandJapanese} ${carData.carModel.modelNameJapanese}`
              : `${carData.brand.brandEnglish} ${carData.carModel.modelNameEnglish}`}
        </h2>

        <p id="car-price">
          {langCtx?.translate.price} {carData.price} yen
        </p>

        <p id="car-year">
          {langCtx?.translate.year} {carData.carModel.manufacturingEndYear}
        </p>

        <p id="car-type">
          {langCtx?.translate.type} {langCtx?.language === "jp" ? carData.bodyType.nameJapanese : carData.bodyType.nameEnglish}
        </p>

        <p id="car-fuel">
          {langCtx?.translate.fuelType} {langCtx?.language === "jp" ? carData.fuelType.nameJapanese : carData.fuelType.nameEnglish}
        </p>

        <p id="car-passenger-count">
          {langCtx?.translate.passengerCount}: {carData.carModel.passengerCount}
        </p>

        <p id="car-dimensions">
          {langCtx?.translate.dimensions}: {carData.carModel.length} x {carData.carModel.width} x {carData.carModel.height} cm
        </p>

        <p id="car-mass">
          {langCtx?.translate.mass}: {carData.carModel.mass} kg
        </p>

        <p id="car-engine-size">
          {langCtx?.translate.engineSize}: {carData.engineSize.engineSize} cc
        </p>

        <p id="car-color">
          {langCtx?.translate.color} {langCtx?.language === "jp" ? carData.color.colorNameJapanese : carData.color.colorNameEnglish}
        </p>

        <p id="car-transmission">
          {langCtx?.translate.transmission}: {carData.transmissionType.type}
        </p>

        <p id="car-drive-train">
          {langCtx?.translate.drivetrain}: {carData.driveTrain.type}
        </p>

        <p id="car-license-plate">
          {langCtx?.translate.licenseplate}: {carData.licensePlateNumber || langCtx?.translate.noDetails}
        </p>

        <p id="car-repair-history">
          {langCtx?.translate.repairHistory} {carData.repairHistory ? langCtx?.translate.yes : langCtx?.translate.no}
        </p>

        <p id="car-mot-expiry">
          {langCtx?.translate.motExpiry} {carData.motExpiry || langCtx?.translate.noDetails}
        </p>

        <p id="car-location">
          {langCtx?.translate.location}: {carData.location.locationName}
        </p>

        <p id="car-is-smoking">
          {langCtx?.translate.isSmoking}: {carData.isSmoking ? langCtx?.translate.yes : langCtx?.translate.no}
        </p>

        <p id="car-mileage">
          {langCtx?.translate.mileage} {carData.mileage || langCtx?.translate.noDetails}
        </p>
        <p id="car-vin">
          {langCtx?.translate.vin}: {carData.vinNum || langCtx?.translate.noDetails}
        </p>






      {/* New Calendar Widget */}
        <div className="calendar-container">
          <h3 className="calendar-title">{langCtx?.translate.appointment}</h3>
          
          <div className="calendar-widget">
            <div className="calendar-header">
              <button className="calendar-nav" onClick={prevMonth}>&lt;</button>
              <span className="calendar-month">{getMonthName(currentMonth)}</span>
              <button className="calendar-nav" onClick={nextMonth}>&gt;</button>
            </div>
            
            <table className="calendar-table" cellSpacing="0" cellPadding="0">
              <thead>
                <tr>
                  <th>{langCtx?.translate.mon}</th>
                  <th>{langCtx?.translate.tue}</th>
                  <th>{langCtx?.translate.wed}</th>
                  <th>{langCtx?.translate.thu}</th>
                  <th>{langCtx?.translate.fri}</th>
                  <th>{langCtx?.translate.sat}</th>
                  <th>{langCtx?.translate.sun}</th>
                </tr>
              </thead>
              <tbody>
                {renderCalendar()}
              </tbody>
            </table>
            
            {selectedDate && (
              <div className="time-selection">
                <p className="selected-date">
                  {selectedDate.toLocaleDateString('hu-HU', { year: 'numeric', month: '2-digit', day: '2-digit' })}
                </p>
                <div className="time-inputs">
                  <select
                    className="time-select hour-select"
                    value={selectedHour}
                    onChange={(e) => setSelectedHour(parseInt(e.target.value))}
                  >
                    {[...Array(10)].map((_, i) => (
                      <option key={i} value={i + 8}>{i + 8}</option>
                    ))}
                  </select>
                  <span className="time-separator">:</span>
                  <select
                    className="time-select minute-select"
                    value={selectedMinute}
                    onChange={(e) => setSelectedMinute(parseInt(e.target.value))}
                  >
                    {[0, 15, 30, 45].map((min) => (
                      <option key={min} value={min}>{min.toString().padStart(2, "0")}</option>
                    ))}
                  </select>
                </div>
              </div>
            )}
            
            <button
              className="btn-book"
              onClick={handleBooking}
              disabled={!selectedDate}
            >
              {langCtx?.translate.book}
            </button>
            
            {bookingMessage && (
              <p className={`booking-message ${bookingSuccess ? 'success' : ''}`}>{bookingMessage}</p>
            )}
          </div>
        </div>
        
        <a href="/" className="btn-back">{langCtx?.translate.backHome}</a>
      </div>
    </section>
  );
}

export default CarDetails;