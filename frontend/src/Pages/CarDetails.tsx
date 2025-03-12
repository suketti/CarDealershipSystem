import React, { useState, useEffect, useContext } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { CarDTO } from "../Types";
import { LanguageCtx } from "../App";
import { useUser } from "../UserContext.tsx";
import SavedCarService from "../api/savedCarService.ts";
import ReservationService from "../api/reservationService.ts";
import { getBaseUrl } from "../api/axiosInstance.ts";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

function CarDetails() {
  const location = useLocation();
  const navigate = useNavigate();
  const [carData, setCarData] = useState<CarDTO | null>(null);
  const [currentMonth, setCurrentMonth] = useState<Date>(new Date());
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);
  const [selectedHour, setSelectedHour] = useState<number>(10);
  const [selectedMinute, setSelectedMinute] = useState<number>(0);
  const [lightboxImage, setLightboxImage] = useState<string | null>(null);
  const [bookingMessage, setBookingMessage] = useState<string>("");
  const [bookingSuccess, setBookingSuccess] = useState<boolean>(false);
  const [imageIndex, setImageIndex] = useState<number>(0);
  const [showCalendar, setShowCalendar] = useState<boolean>(false);
  const langCtx = useContext(LanguageCtx);

  // Access user context for authentication and user data
  const { user, isAuthenticated } = useUser();

  useEffect(() => {
    const query = new URLSearchParams(location.search);
    const carString = query.get("car");

    if (carString) {
      try {
        const parsedCar: CarDTO = JSON.parse(decodeURIComponent(carString));
        setCarData(parsedCar);
        document.title = `${parsedCar.brand.brandEnglish} ${parsedCar.carModel.modelNameEnglish} | Premium Cars`;
      } catch (error) {
        console.error("Hibás autó paraméter:", error);
        setBookingMessage(langCtx?.translate.errorLoadingCar || "Hiba az autó adatainak betöltésekor");
      }
    }

    return () => {
      document.title = "Premium Cars";
    };
  }, [location.search, langCtx?.translate]);

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
    if (!carData) return;

    // Ensure the user is authenticated
    if (!isAuthenticated) {
      setBookingMessage(langCtx?.translate.loginToSave || "Jelentkezz be az autó mentéséhez!");
      return;
    }

    try {
      const userId = user?.id; // Get user ID from context
      if (!userId) {
        setBookingMessage(langCtx?.translate.errorNoUser || "Hiba: Nincs bejelentkezett felhasználó!");
        return;
      }

      // Call the service to save the car with user ID
      await SavedCarService.saveCar(userId, carData.id);
      setBookingMessage(langCtx?.translate.carSaved || "Autó sikeresen mentve!");
      setBookingSuccess(true);

      // Reset success message after a few seconds
      setTimeout(() => {
        setBookingMessage("");
        setBookingSuccess(false);
      }, 3000);
    } catch (error) {
      console.error("Hiba az autó mentésekor:", error);
      setBookingMessage(langCtx?.translate.errorSavingCar || "Hiba történt az autó mentésekor!");
      setBookingSuccess(false);
    }
  };

  const handleBooking = async () => {
    if (!isAuthenticated) {
      setBookingMessage(langCtx?.translate.loginToBook || "Jelentkezz be időpontfoglaláshoz!");
      return;
    }
    if (!selectedDate) {
      setBookingMessage(langCtx?.translate.chooseDate || "Válassz időpontot a foglaláshoz!");
      return;
    }

    const now = new Date();
    now.setSeconds(0, 0);
    const selectedDateTime = new Date(selectedDate);
    selectedDateTime.setHours(selectedHour, selectedMinute, 0, 0);

    if (selectedDateTime < now) {
      setBookingMessage(langCtx?.translate.noPastDate || "Nem választhatsz múltbeli időpontot!");
      return;
    }

    try {
      const reservations = await ReservationService.getAllReservations();
      const existingBooking = reservations.find(res => new Date(res.date) > now);

      if (existingBooking) {
        setBookingMessage(
            `${langCtx?.translate.alreadyBookedFirst || "Már van foglalásod"} ${formatDate(new Date(existingBooking.date))}. 
            ${langCtx?.translate.alreadyBookedSecond || "Előbb törölnöd kell a meglévő időpontot."}`
        );
        return;
      }

      const reservationData = {
        date: selectedDateTime.toISOString(),
        userId: user?.id // Assuming user ID is available
      };

      const newReservation = await ReservationService.createReservation(reservationData);
      
      setBookingMessage(
        `${langCtx?.translate.doneBookFirst || "Sikeres foglalás!"} 
        ${formatDate(new Date(newReservation.date))}
        ${langCtx?.translate.doneBookSecond || "Várunk a szalonunkban!"}`
      );
      
      setBookingSuccess(true);
      
      // Reset calendar view
      setShowCalendar(false);

    } catch (error) {
      console.error("Hiba a foglalás során:", error);
      setBookingMessage(langCtx?.translate.errorBooking || "Hiba történt a foglalás során. Kérjük, próbáld újra később!");
      setBookingSuccess(false);
    }
  };

  const formatDate = (date: Date): string => {
    return new Intl.DateTimeFormat(langCtx?.language === 'en' ? 'en-US' : 'hu-HU', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    }).format(date);
  };

  // Calendar navigation
  const prevMonth = () => {
    setCurrentMonth(new Date(currentMonth.getFullYear(), currentMonth.getMonth() - 1, 1));
  };

  const nextMonth = () => {
    setCurrentMonth(new Date(currentMonth.getFullYear(), currentMonth.getMonth() + 1, 1));
  };

  // Generate calendar days
  const generateCalendarDays = () => {
    const year = currentMonth.getFullYear();
    const month = currentMonth.getMonth();
    
    // Get the first day of the month
    const firstDayOfMonth = new Date(year, month, 1);
    // Get the day of the week for the first day (0 = Sunday, 1 = Monday, etc.)
    let firstDayWeekday = firstDayOfMonth.getDay();
    // Adjust for Monday as the first day of the week
    firstDayWeekday = firstDayWeekday === 0 ? 6 : firstDayWeekday - 1;
    
    // Get the last day of the month
    const lastDay = new Date(year, month + 1, 0).getDate();
    
    const daysArray = [];
    
    // Add empty cells for days before the first of the month
    for (let i = 0; i < firstDayWeekday; i++) {
      daysArray.push(<div key={`empty-${i}`} className="calendar-day empty"></div>);
    }
    
    // Add days of the month
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    
    for (let day = 1; day <= lastDay; day++) {
      const date = new Date(year, month, day);
      const isToday = date.getTime() === today.getTime();
      const isPast = date < today;
      const isSelected = selectedDate && 
        date.getDate() === selectedDate.getDate() && 
        date.getMonth() === selectedDate.getMonth() && 
        date.getFullYear() === selectedDate.getFullYear();
      
      const dayClasses = `calendar-day ${isToday ? 'today' : ''} ${isPast ? 'past' : ''} ${isSelected ? 'selected' : ''}`;
      
      daysArray.push(
        <div 
          key={day} 
          className={dayClasses}
          onClick={() => {
            if (!isPast) {
              const newSelectedDate = new Date(year, month, day);
              setSelectedDate(newSelectedDate);
            }
          }}
        >
          {day}
        </div>
      );
    }
    
    return daysArray;
  };

  // Format month name
  const formatMonthName = (date: Date): string => {
    const language = langCtx?.language === 'en' ? 'en-US' : (langCtx?.language === 'jp' ? 'ja-JP' : 'hu-HU');
    return date.toLocaleString(language, { month: 'long', year: 'numeric' });
  };

  if (!carData) {
    return (
      <div className="loading-container">
        <div className="loading-spinner">
          <FontAwesomeIcon icon="spinner" spin />
        </div>
        <p>{langCtx?.translate.loading || "Betöltés..."}</p>
      </div>
    );
  }

  return (
    <div className="car-details-page">
      <div className="container">
        <div className="back-link">
          <button onClick={() => navigate(-1)} className="btn-back">
            <FontAwesomeIcon icon="arrow-left" /> {langCtx?.translate.back || "Vissza"}
          </button>
        </div>

        <div className="car-details-container">
          <div className="car-gallery">
            <div className="main-image-container">
              {images.length > 0 ? (
                <img
                  src={images[imageIndex]}
                  alt={`${carData.brand.brandEnglish} ${carData.carModel.modelNameEnglish}`}
                  className="main-car-image"
                  onClick={() => setLightboxImage(images[imageIndex])}
                />
              ) : (
                <div className="no-image">
                  <FontAwesomeIcon icon="image" />
                  <p>{langCtx?.translate.noImage || "Nincs kép"}</p>
                </div>
              )}
              
              {images.length > 1 && (
                <>
                  <button className="gallery-nav prev-button" onClick={prevImage}>
                    <FontAwesomeIcon icon="chevron-left" />
                  </button>
                  <button className="gallery-nav next-button" onClick={nextImage}>
                    <FontAwesomeIcon icon="chevron-right" />
                  </button>
                </>
              )}
            </div>
            
            {images.length > 1 && (
              <div className="thumbnail-container">
                {images.map((img, index) => (
                  <img
                    key={index}
                    src={img}
                    alt={`Kép ${index + 1}`}
                    className={`thumbnail ${index === imageIndex ? "active" : ""}`}
                    onClick={() => setImageIndex(index)}
                  />
                ))}
              </div>
            )}
          </div>

          <div className="car-info">
            <div className="car-header">
              <h1>{carData.brand.brandEnglish} {carData.carModel.modelNameEnglish}</h1>
              <div className="car-actions">
                <button className="btn-save" onClick={handleSaveCar}>
                  <FontAwesomeIcon icon="heart" /> {langCtx?.translate.save || "Mentés"}
                </button>
              </div>
            </div>

            <div className="car-price">
              {Number(carData.price).toLocaleString()} Ft
            </div>

            <div className="car-specs">
              <div className="spec-item">
                <div className="spec-label">
                  <FontAwesomeIcon icon="calendar-alt" />
                  {langCtx?.translate.year || "Évjárat"}
                </div>
                <div className="spec-value">{carData.carModel.manufacturingStartYear}</div>
              </div>
              
              <div className="spec-item">
                <div className="spec-label">
                  <FontAwesomeIcon icon="gas-pump" />
                  {langCtx?.translate.fuelType || "Üzemanyag"}
                </div>
                <div className="spec-value">{langCtx?.language === 'jp' ? carData.fuelType.nameJapanese : carData.fuelType.nameEnglish}</div>
              </div>
              
              <div className="spec-item">
                <div className="spec-label">
                  <FontAwesomeIcon icon="tachometer-alt" />
                  {langCtx?.translate.mileage || "Futott km"}
                </div>
                <div className="spec-value">{carData.mileage.toLocaleString()} km</div>
              </div>
              
              <div className="spec-item">
                <div className="spec-label">
                  <FontAwesomeIcon icon="car" />
                  {langCtx?.translate.bodyType || "Karosszéria"}
                </div>
                <div className="spec-value">{langCtx?.language === 'jp' ? carData.bodyType.nameJapanese : carData.bodyType.nameEnglish}</div>
              </div>
              
              <div className="spec-item">
                <div className="spec-label">
                  <FontAwesomeIcon icon="cogs" />
                  {langCtx?.translate.engineSize || "Motor"}
                </div>
                <div className="spec-value">{carData.engineSize.engineSize} cm³</div>
              </div>
              
              <div className="spec-item">
                <div className="spec-label">
                  <FontAwesomeIcon icon="map-marker-alt" />
                  {langCtx?.translate.location || "Telephely"}
                </div>
                <div className="spec-value">{carData.location.locationName}</div>
              </div>
            </div>

            <div className="car-description">
              <h3>{langCtx?.translate.description || "Leírás"}</h3>
              <p>{carData.description || langCtx?.translate.noDetails || "Nincs részletes leírás."}</p>
            </div>

            <div className="appointment-section">
              <h3>{langCtx?.translate.appointment || "Időpontfoglalás"}</h3>
              
              {showCalendar ? (
                <div className="calendar-container">
                  <div className="calendar-header">
                    <button type="button" onClick={prevMonth} className="calendar-nav">
                      <FontAwesomeIcon icon="chevron-left" />
                    </button>
                    <div className="current-month">{formatMonthName(currentMonth)}</div>
                    <button type="button" onClick={nextMonth} className="calendar-nav">
                      <FontAwesomeIcon icon="chevron-right" />
                    </button>
                  </div>

                  <div className="weekdays">
                    {[
                      langCtx?.translate.mon || "H", 
                      langCtx?.translate.tue || "K", 
                      langCtx?.translate.wed || "Sze", 
                      langCtx?.translate.thu || "Cs", 
                      langCtx?.translate.fri || "P", 
                      langCtx?.translate.sat || "Szo", 
                      langCtx?.translate.sun || "V"
                    ].map((day, index) => (
                      <div key={index} className="weekday">{day}</div>
                    ))}
                  </div>

                  <div className="calendar-days">
                    {generateCalendarDays()}
                  </div>

                  <div className="time-selector">
                    <div className="time-picker">
                      <label>{langCtx?.translate.time || "Időpont"}:</label>
                      <div className="time-inputs">
                        <select 
                          value={selectedHour} 
                          onChange={(e) => setSelectedHour(parseInt(e.target.value))}
                        >
                          {Array.from({length: 9}, (_, i) => i + 9).map((hour) => (
                            <option key={hour} value={hour}>{hour.toString().padStart(2, '0')}</option>
                          ))}
                        </select>
                        <span>:</span>
                        <select 
                          value={selectedMinute} 
                          onChange={(e) => setSelectedMinute(parseInt(e.target.value))}
                        >
                          {[0, 15, 30, 45].map((minute) => (
                            <option key={minute} value={minute}>{minute.toString().padStart(2, '0')}</option>
                          ))}
                        </select>
                      </div>
                    </div>
                  </div>

                  <div className="calendar-actions">
                    <button 
                      type="button" 
                      className="btn btn-secondary" 
                      onClick={() => setShowCalendar(false)}
                    >
                      {langCtx?.translate.cancel || "Mégse"}
                    </button>
                    <button 
                      type="button" 
                      className="btn btn-primary" 
                      onClick={handleBooking}
                      disabled={!selectedDate}
                    >
                      {langCtx?.translate.book || "Foglalás"}
                    </button>
                  </div>
                </div>
              ) : (
                <div className="appointment-prompt">
                  {selectedDate ? (
                    <div className="selected-date-display">
                      <FontAwesomeIcon icon="calendar-check" />
                      <span>{formatDate(new Date(selectedDate.setHours(selectedHour, selectedMinute)))}</span>
                      <button 
                        type="button" 
                        className="btn-change-date" 
                        onClick={() => setShowCalendar(true)}
                      >
                        {langCtx?.translate.change || "Módosítás"}
                      </button>
                    </div>
                  ) : (
                    <button 
                      type="button" 
                      className="btn btn-secondary btn-select-date" 
                      onClick={() => setShowCalendar(true)}
                    >
                      <FontAwesomeIcon icon="calendar-plus" />
                      {langCtx?.translate.selectDate || "Időpont kiválasztása"}
                    </button>
                  )}

                  {selectedDate && (
                    <button 
                      type="button" 
                      className="btn btn-primary btn-book" 
                      onClick={handleBooking}
                    >
                      <FontAwesomeIcon icon="check-circle" />
                      {langCtx?.translate.book || "Foglalás"}
                    </button>
                  )}
                </div>
              )}
              
              {bookingMessage && (
                <div className={`booking-message ${bookingSuccess ? "success" : "error"}`}>
                  <FontAwesomeIcon icon={bookingSuccess ? "check-circle" : "exclamation-circle"} />
                  <p>{bookingMessage}</p>
                </div>
              )}
            </div>
          </div>
        </div>
      </div>

      {lightboxImage && (
        <div className="lightbox" onClick={() => setLightboxImage(null)}>
          <button className="lightbox-close" onClick={() => setLightboxImage(null)}>
            <FontAwesomeIcon icon="times" />
          </button>
          <div className="lightbox-content" onClick={(e) => e.stopPropagation()}>
            <button 
              className="lightbox-nav prev" 
              onClick={(e) => {
                e.stopPropagation();
                prevImage();
                setLightboxImage(images[imageIndex]);
              }}
            >
              <FontAwesomeIcon icon="chevron-left" />
            </button>
            <img src={lightboxImage} alt="Nagyított kép" />
            <button 
              className="lightbox-nav next" 
              onClick={(e) => {
                e.stopPropagation();
                nextImage();
                setLightboxImage(images[imageIndex]);
              }}
            >
              <FontAwesomeIcon icon="chevron-right" />
            </button>
          </div>
        </div>
      )}
    </div>
  );
}

export default CarDetails;