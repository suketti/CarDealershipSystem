import React, { useState } from "react";
import { translations } from "../translations";
import { LanguageCtx } from "../App";
import { useContext } from "react";
import LoginModal from "./LoginModal";

interface PostCarOfferProps {
  language: "hu" | "en" | "jp";
  isLoggedIn: boolean;
  setShowLoginModal: React.Dispatch<React.SetStateAction<boolean>>;
}



const PostCarOffer: React.FC<PostCarOfferProps> = ({ language, isLoggedIn, setShowLoginModal }) => {
  const langCtx = useContext(LanguageCtx);
  const [showModal, setShowModal] = useState(false);
  const [images, setImages] = useState<string[]>([]);
  const [formData, setFormData] = useState({
    title: "",
    description: "",
    brand: "",
    model: "",
    fuelType: "",
    price: "",
    year: "",
    mileage: "",
    color: "",
    image: ""
  });
  const [formSubmitted, setFormSubmitted] = useState(false);

  const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const files = e.target.files;
    if (files) {
      const newImages: string[] = [];
      for (let i = 0; i < files.length; i++) {
        if (images.length + newImages.length >= 5) break;
        const reader = new FileReader();
        reader.onloadend = () => {
          setImages(prevImages => [...prevImages, reader.result as string]);
        };
        reader.readAsDataURL(files[i]);
      }
    }
  };

  const removeImage = (index: number) => {
    setImages(prevImages => prevImages.filter((_, i) => i !== index));
  };

  const brandsWithModels: { [key: string]: string[] } = {
    Toyota: ["Corolla", "Yaris", "RAV4"],
    BMW: ["3 Series", "5 Series", "X5"],
    Audi: ["A3", "A4", "Q5"],
    Mercedes: ["C-Class", "E-Class", "GLC"],
    Ford: ["Focus", "Fiesta", "Mustang"]
  };

  const t = translations[language] || translations.hu;
    
  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData({ 
      ...formData, 
      [name]: value,
      ...(name === "brand" ? { model: "" } : {}) 
    });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setFormSubmitted(true);
    setShowModal(false);
  
    // Üzenet mentése a localStorage-ba
    const newMessage = {
      sender: "Rendszer",
      content: langCtx?.translate.successMessage || "Sikeresen feladtál egy hirdetést!",
      date: new Date().toLocaleString()
    };
  
    const existingMessages = localStorage.getItem("messages");
    const messages = existingMessages ? JSON.parse(existingMessages) : [];
    messages.push(newMessage);
    localStorage.setItem("messages", JSON.stringify(messages));
  };

  return (
    <div className="post-ad-container">
      <button className="post-ad-button large" onClick={() => isLoggedIn ? setShowModal(true) : setShowLoginModal(true)}>
        📢 {langCtx?.translate.postAd}
      </button>

      {showModal && (
        <div className="modal-overlay">
          <div className="modal">
          <span 
    className="close" 
    style={{ 
      position: "absolute", 
      top: "10px", 
      right: "10px", /* Jobb felső sarokba helyezés */ 
      color: "black", 
      fontSize: "24px", 
      cursor: "pointer" 
    }} 
    onClick={() => setShowModal(false)}
  >
    &times;
  </span>           
    <div className="modal-content">
              <h2>{langCtx?.translate.postAd}</h2>
              <form onSubmit={handleSubmit} className="ad-form">
                <div className="form-left">
                  <label>{langCtx?.translate.uploadImage}</label>
                  {images.length < 5 && (
                    <input type="file" accept="image/*" multiple onChange={handleImageChange} />
                  )}
                  <div className="image-preview-container">
                    {images.map((img, index) => (
                      <div key={index} className="image-preview-wrapper">
                        <img src={img} alt={`Preview ${index}`} className="image-preview" />
                        <button type="button" className="remove-image-button" onClick={() => removeImage(index)}>✖</button>
                      </div>
                    ))}
                  </div>
                </div>
                <div className="form-right">
                  <label>{langCtx?.translate.description}</label>
                  <textarea name="description" value={formData.description} onChange={handleChange} required />
                  <label>{langCtx?.translate.brand}</label>
                  <select name="brand" value={formData.brand} onChange={handleChange} required>
                    <option value="">{langCtx?.translate.selectBrand}</option>
                    {Object.keys(brandsWithModels).map((brand) => (
                      <option key={brand} value={brand}>{brand}</option>
                    ))}
                  </select>
                  <label>{langCtx?.translate.model}</label>
                  <select name="model" value={formData.model} onChange={handleChange} required disabled={!formData.brand}>
                    <option value="">{langCtx?.translate.selectModel}</option>
                    {formData.brand && brandsWithModels[formData.brand]?.map((model) => (
                      <option key={model} value={model}>{model}</option>
                    ))}
                  </select>
                  <label>{langCtx?.translate.fuelType}</label>
                  <input type="text" name="fuelType" value={formData.fuelType} onChange={handleChange} required />
                  <label>{langCtx?.translate.price}</label>
                  <input type="number" name="price" value={formData.price} onChange={handleChange} required />
                  <label>{langCtx?.translate.year}</label>
                  <input type="number" name="year" value={formData.year} onChange={handleChange} required />
                  <label>{langCtx?.translate.mileage}</label>
                  <input type="number" name="mileage" value={formData.mileage} onChange={handleChange} required />
                  <label>{langCtx?.translate.color}</label>
                  <input type="text" name="color" value={formData.color} onChange={handleChange} required />
                  <button type="submit" className="submit-button">{langCtx?.translate.complete}</button>
                </div>
              </form>
            </div>
          </div>
        </div>
      )}
      {formSubmitted && <div className="success-message" style={{ color: "green", fontSize: "18px", marginTop: "10px" }}>{langCtx?.translate.successMessage}</div>}
    </div>
  );
};

export default PostCarOffer;
