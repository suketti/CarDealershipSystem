import React, { useState, useEffect } from "react";
import { useLocation } from "react-router-dom";
import { Car } from "../Types";
import LoginModal from "../Components/LoginModal";


function CarDetails() {
  const location = useLocation();
  const [carData, setCarData] = useState<Car | null>(null);
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);
  const [selectedHour, setSelectedHour] = useState<number>(8);
  const [selectedMinute, setSelectedMinute] = useState<number>(0);
  const [lightboxImage, setLightboxImage] = useState<string | null>(null);
  const [bookingMessage, setBookingMessage] = useState<string>("");
  const [imageIndex, setImageIndex] = useState<number>(0);
  const [hasBooked, setHasBooked] = useState<boolean>(false);
  const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false);
  const [isLoginModalOpen, setIsLoginModalOpen] = useState<boolean>(false);


  useEffect(() => {
    const query = new URLSearchParams(location.search);
    const carString = query.get("car");
    if (carString) {
      try {
        const parsedCar: Car = JSON.parse(carString);
        setCarData(parsedCar);
      } catch (error) {
        console.error("Hibás autó paraméter");
      }
    }
  }, [location.search]);
  
  const generateCalendarGrid = (date: Date) => {
    const year = date.getFullYear();
    const month = date.getMonth();
    const firstDay = new Date(year, month, 1).getDay();
    const lastDate = new Date(year, month + 1, 0).getDate();
    
    // Hungarian week days starting from Monday
    const weeks: (number | null)[][] = [];
    let week: (number | null)[] = [];
    let day = 1;
  
    // Adjust Hungarian week start (Monday)
    const startOffset = firstDay === 0 ? 6 : firstDay - 1;
    if (startOffset > 0) {
      week = Array(startOffset).fill(null);
    }
  
    while (day <= lastDate) {
      if (week.length === 7) {
        weeks.push(week);
        week = [];
      }
      week.push(day);
      day++;
    }
  
    if (week.length > 0) {
      weeks.push(week.concat(Array(7 - week.length).fill(null)));
    }
  
    return weeks;
  };
  
  const isPastDateTime = (date: Date, hour: number, minute: number) => {
    const now = new Date();
    const selectedDateTime = new Date(date);
    selectedDateTime.setHours(hour, minute, 0, 0);
    return selectedDateTime <= now;
  };
  
  // A komponensen belül (a CarDetails függvényben)
  const [currentDate, setCurrentDate] = useState(new Date());
  
  const prevMonth = () => {
    setCurrentDate(prev => new Date(prev.getFullYear(), prev.getMonth() - 1, 1));
  };
  
  const nextMonth = () => {
    setCurrentDate(prev => new Date(prev.getFullYear(), prev.getMonth() + 1, 1));
  };
  
  const handleDateClick = (day: number) => {
    const today = new Date();
    const selected = new Date(today.getFullYear(), today.getMonth(), day);
    
    // Csak akkor állítjuk be a dátumot, ha nem múltbeli
    if (!isPastDateTime(selected, selectedHour, selectedMinute)) {
      setSelectedDate(selected);
    }
  };

  const additionalImages = [
    "../Képek/kep1.jpg",
    "../Képek/kep2.jpg",
    "../Képek/kep3.jpg",
    "../Képek/kep4.jpg",
    "../Képek/kep5.jpg",
    "../Képek/kep5.jpg",
    "../Képek/kep5.jpg",

  ];

  const images = carData?.kep 
  ? [carData.kep, ...additionalImages].filter(img => img) 
  : additionalImages;

  

  const nextImage = () => {
    setImageIndex((prev) => (prev + 1) % images.length);
  };

  const prevImage = () => {
    setImageIndex((prev) => (prev - 1 + images.length) % images.length);
  };
  const handleBooking = () => {
    if (hasBooked) {
      setBookingMessage("Már lefoglaltál egy időpontot!");
      return;
    }

    if (!isLoggedIn) {
      setIsLoginModalOpen(true);
      return;
    }

    if (!selectedDate) {
      setBookingMessage("Kérjük válasszon dátumot!");
      return;
    }

    // Ellenőrzi az időpontot
    if (isPastDateTime(selectedDate, selectedHour, selectedMinute)) {
      setBookingMessage("Nem lehet múltbeli időpontot foglalni!");
      return;
    }

    setHasBooked(true);
    setBookingMessage(`Sikeres foglalás: ${selectedDate.toLocaleDateString("hu-HU")} ${selectedHour.toString().padStart(2, '0')}:${selectedMinute.toString().padStart(2, '0')}`);
  };
  const handleSaveCar = () => {
    if (!carData) return;
    const isLoggedIn = document.querySelector(".profile-btn") !== null;
    if (!isLoggedIn) {
      setIsLoginModalOpen(true);
      return;
    }
    const savedCars = JSON.parse(localStorage.getItem("savedCars") || "[]") as Car[];
    const exists = savedCars.some((c) => c.kep === carData.kep);
    if (!exists) {
      savedCars.push(carData);
      localStorage.setItem("savedCars", JSON.stringify(savedCars));
      alert("Autó sikeresen mentve!");
    } else {
      alert("Ez az autó már mentve van!");
    }
  };

  

  if (!carData) {
    return <p>Betöltés...</p>;
  }

  const locationMap: Record<string, { top: string; left: string }> = {
    Budapest: { top: "40%", left: "45%" },
    Debrecen: { top: "37%", left: "80%" },
    Miskolc: { top: "20%", left: "70%" },
    Sopron: { top: "36%", left: "10%" },
  };

  return (
    <section className="car-details">
      <div className="image-gallery">
        <img
          id="main-car-image"
          src={images[imageIndex]}
          alt="Autó képe"
          className="main-image"
          onClick={() => setLightboxImage(images[imageIndex])}
        />
        {images.length > 5 && (
          <button className="prev-button" onClick={prevImage}>&#8249;</button>
        )}
        <div className="thumbnail-container">
          {images.map((img, index) => (
            <img
              key={index}
              src={img}
              alt={`Kép ${index}`}
              className={`thumbnail ${index === imageIndex ? "active" : ""}`}
              onClick={() => setImageIndex(index)}
            />
          ))}
        </div>
        {images.length > 5 && (
          <button className="next-button" onClick={nextImage}>&#8250;</button>
        )}
      </div>
      {lightboxImage && (
  <div className="lightbox-overlay" onClick={() => setLightboxImage(null)}>
    <button className="lightbox-close" onClick={() => setLightboxImage(null)}>✕</button>
    <button className="lightbox-prev" onClick={(e) => { e.stopPropagation(); prevImage(); }}>‹</button>
    <img src={images[imageIndex]} alt="Nagyított kép" className="lightbox-image" />
    <button className="lightbox-next" onClick={(e) => { e.stopPropagation(); nextImage(); }}>›</button>
  </div>
)}
        <div className="details">
        <button className="btn-save" onClick={handleSaveCar}>
          Autó mentése
        </button>
        <h2 id="car-title">{carData.marka + " " + carData.modell}</h2>
        <p id="car-price">Ár: {carData.ar.toLocaleString()} Ft</p>
        <p id="car-year">Évjárat: {carData.ev}</p>
        <p id="car-type">Kivitel: {carData.kivitel}</p>
        <p id="car-fuel">Üzemanyag: {carData.uzemanyag}</p>
        <p id="car-description" className="description">{carData.leiras || "Nincs leírás az autóról."}</p>
        <div className="calendar-container">
          <h3 className="text-lg font-bold mb-2">Időpont foglalása</h3>
          <div className="calendar">
            <div className="calendar-header">
              <button onClick={prevMonth}>&#9665;</button>
              <span id="current-month-year">{currentDate.toLocaleDateString("hu-HU", {
          month: "long",
          year: "numeric",
        })}</span>
              <button onClick={nextMonth}>&#9655;</button>
            </div>
            <table className="calendar-table">
              <thead>
                <tr>
                {["H", "K", "Sze", "Cs", "P", "Szo", "V"].map(day => (
            <th key={day}>{day}</th>
          ))}
                </tr>
              </thead>
              <tbody> {generateCalendarGrid(currentDate).map((week, i) => (
          <tr key={i}>
            {week.map((day, j) => {
              const cellDate = day
                ? new Date(currentDate.getFullYear(), currentDate.getMonth(), day)
                : null;
                const isPast = cellDate ? isPastDateTime(cellDate, selectedHour, selectedMinute) : false;

              
              return (
                <td
                  key={`${i}-${j}`}
                  className={`calendar-day ${
                    day && isPast ? "past-date" : ""
                  } ${
                    selectedDate?.getDate() === day &&
                    selectedDate?.getMonth() === currentDate.getMonth()
                      ? "selected"
                      : ""
                  }`}
                  onClick={() => day && !isPast && handleDateClick(day)}
                >
                  {day || ""}
                </td>
              );
            })}
          </tr>
        ))}</tbody>
            </table>
          </div>
          <div className="time-picker">
            <label htmlFor="hours">Óra:</label>
            <select
          id="hours"
          value={selectedHour}
          onChange={(e) => setSelectedHour(Number(e.target.value))}
        >
          {Array.from({ length: 13 }, (_, i) => i + 8).map(hour => (
            <option key={hour} value={hour}>{hour.toString().padStart(2, '0')}</option>
          ))}
        </select>
            <label htmlFor="minutes"></label>
            <select
          id="minutes"
          value={selectedMinute}
          onChange={(e) => setSelectedMinute(Number(e.target.value))}
        >
          {[0, 15, 30, 45].map(minute => (
            <option key={minute} value={minute}>{minute.toString().padStart(2, '0')}</option>
          ))}
        </select>
          </div>
          <button className="btn-submit" onClick={() => handleBooking()}>Foglalás</button>
        {bookingMessage && <p>{bookingMessage}</p>}   
        {isLoginModalOpen && (
        <LoginModal onClose={() => setIsLoginModalOpen(false)} setIsLoggedIn={setIsLoggedIn} t={{ loginTitle: "Bejelentkezés" }} />
      )}       
        </div>
        <a href="/" className="btn-back">Vissza a Kezdőlapra</a>
      </div>
      <section id="car-location">
        <h3>Autó elhelyezkedése</h3>
        <div className="map-container">
          <img src="../Képek/magyarorszag-terkep.jpg" alt="Magyarország térképe" className="map-image" />
          {carData.varos && locationMap[carData.varos] && (
            <button
              id="map-location"
              className="map-point"
              style={{ position: "absolute", top: locationMap[carData.varos].top, left: locationMap[carData.varos].left }}
            >
              {carData.varos}
            </button>
          )}
        </div>
      </section>
    </section>
  );
}

export default CarDetails;