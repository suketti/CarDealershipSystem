import React, { useState, useEffect } from "react";
import { useLocation } from "react-router-dom";
import { CarDTO } from "../Types";
import { LanguageCtx } from "../App";
import { useContext } from "react";




function CarDetails() {
  const location = useLocation();
  const [carData, setCarData] = useState<CarDTO | null>(null);
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);
  const [selectedHour, setSelectedHour] = useState<number>(8);
  const [selectedMinute, setSelectedMinute] = useState<number>(0);
  const [lightboxImage, setLightboxImage] = useState<string | null>(null);
  const [bookingMessage, setBookingMessage] = useState<string>("");
  const [imageIndex, setImageIndex] = useState<number>(0);
  const langCtx = useContext(LanguageCtx)

  useEffect(() => {
    const query = new URLSearchParams(location.search);
    const carString = query.get("car");
    const carId = query.get("carId");
  
    if (carString) {
      try {
        const parsedCar: CarDTO = JSON.parse(decodeURIComponent(carString));
        setCarData(parsedCar);
      } catch (error) {
        console.error("Hibás autó paraméter:", error);
      }
    } else if (carId) {
      // Ha nincs JSON az URL-ben, kérjük le az autó adatait API-ból
      fetch(`/api/getCarById/${carId}`)
        .then((res) => res.json())
        .then((data) => setCarData(data))
        .catch((error) => console.error("Hiba az autó lekérésekor:", error));
    }
  }, [location.search]);
  
  

  const additionalImages = [
    "../Képek/image1.jpg",
    "../Képek/image2.jpg",
    "../Képek/image3.jpg",
    "../Képek/image4.jpg",
    "../Képek/image5.jpg",
    "../Képek/image5.jpg",
    "../Képek/image5.jpg",

  ];

  const images = carData?.image
  ? [carData.image, ...additionalImages].filter(img => img)
  : additionalImages;



  const nextImage = () => {
    setImageIndex((prev) => (prev + 1) % images.length);
  };

  const prevImage = () => {
    setImageIndex((prev) => (prev - 1 + images.length) % images.length);
  };

  const handleSaveCar = () => {
    console.log("Mentett autó:", carData);

    if (!carData) return;
  
    const isLoggedIn = document.querySelector(".profile-btn") !== null;
    if (!isLoggedIn) {
      alert("Jelentkezz be az autó mentéséhez!");
      return;
    }
  
    const savedCars = JSON.parse(localStorage.getItem("savedCars") || "[]") as CarDTO[];
    const exists = savedCars.some((c) => c.carModel.modelNameEnglish === carData.carModel.modelNameEnglish && c.brand.brandEnglish === carData.brand.brandEnglish);
  
    if (!exists) {
      savedCars.push(carData);
      localStorage.setItem("savedCars", JSON.stringify(savedCars));
      alert("Autó sikeresen mentve!");
    } else {
      alert("Ez az autó már mentve van!");
    }
  };
  
  const addMessage = (content: string) => {
    const storedMessages = localStorage.getItem("messages");
    const messages = storedMessages ? JSON.parse(storedMessages) : [];
    const newMessage = { sender: "Rendszer", content, date: new Date().toLocaleString("hu-HU") };
    messages.push(newMessage);
    localStorage.setItem("messages", JSON.stringify(messages));
  };
  const handleBooking = () => {
    const isLoggedIn = document.querySelector(".profile-btn") !== null;
    if (!isLoggedIn) {
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
    const existingBooking = localStorage.getItem("bookingDate");
    if (existingBooking) {
      const existingBookingDate = new Date(existingBooking);
      if (existingBookingDate > now) {
        setBookingMessage(`${langCtx?.translate.alreadyBookedFirst} ${existingBooking}. ${langCtx?.translate.alreadyBookedSecond}`);
        return;
      } else {
        localStorage.removeItem("bookingDate");
      }
    }
    const bookingDate = `${selectedDate.toLocaleDateString("hu-HU")} ${selectedHour}:${selectedMinute}`;
    localStorage.setItem("bookingDate", bookingDate);
    setBookingMessage(`${langCtx?.translate.doneBookFirst} ${bookingDate}\n ${langCtx?.translate.doneBookSecond}`);
    addMessage(`Időpont lefoglalva: ${bookingDate}`);
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
          {langCtx?.translate.saveCars}
        </button>
        <h2 id="car-title">{carData.brand.brandEnglish + " " + carData.carModel.modelNameEnglish}</h2>
        <p id="car-price">{langCtx?.translate.price} {carData.price} Ft</p>
        <p id="car-year">{langCtx?.translate.year} {carData.carModel.manufacturingEndYear}</p>
        <p id="car-type">{langCtx?.translate.type} {carData.bodyType.nameEnglish}</p>
        <p id="car-fuel">{langCtx?.translate.fuelType} {carData.fuelType.nameEnglish}</p>
        <p id="car-description" className="description"> {langCtx?.translate.noDetails}</p>
        <div className="calendar-container bg-white p-4 rounded-lg shadow-md w-80 mx-auto">
        <h3 className="text-lg font-bold mb-2">{langCtx?.translate.appointment}</h3>
        <input 
          type="date" 
          className="border p-2 rounded w-full mb-2"
          onChange={(e) => setSelectedDate(new Date(e.target.value))} 
        />
        
        <div className="flex justify-between mb-2">
          <select 
            className="border p-2 rounded" 
            value={selectedHour} 
            onChange={(e) => setSelectedHour(parseInt(e.target.value))}
          >
            {[...Array(10)].map((_, i) => (
              <option key={i} value={i + 8}>{i + 8}</option>
            ))}
          </select>
          <select 
            className="border p-2 rounded" 
            value={selectedMinute} 
            onChange={(e) => setSelectedMinute(parseInt(e.target.value))}
          >
            {[0, 15, 30, 45].map((min) => (
              <option key={min} value={min}>{min.toString().padStart(2, "0")}</option>
            ))}
          </select>
        </div>

                  
        <button 
          className="bg-green-500 text-white py-2 px-4 rounded w-full" 
          onClick={handleBooking}
        >
          {langCtx?.translate.book}
        </button>
        {bookingMessage && <p className="mt-2 text-center text-red-500">{bookingMessage}</p>}

        </div>
        <a href="/" className="btn-back">{langCtx?.translate.backHome}</a>
      </div>
      {/*<section id="car-location">*/}
      {/*  <h3>Autó elhelyezkedése</h3>*/}
      {/*  <div className="map-container">*/}
      {/*    <img src="../Képek/magyarorszag-terimage.jpg" alt="Magyarország térképe" className="map-image" />*/}
      {/*    {carData.varos && locationMap[carData.varos] && (*/}
      {/*      <button*/}
      {/*        id="map-location"*/}
      {/*        className="map-point"*/}
      {/*        style={{ position: "absolute", top: locationMap[carData.varos].top, left: locationMap[carData.varos].left }}*/}
      {/*      >*/}
      {/*        {carData.varos}*/}
      {/*      </button>*/}
      {/*    )}*/}
      {/*  </div>*/}
      {/*</section>*/}
    </section>
  );
}

export default CarDetails;