import React, { useState } from "react";
import styles from "./LoginModal.module.css"; // Reusing the login modal styles
import { LanguageCtx } from "../App";
import { useContext } from "react";

interface PasswordChangeModalProps {
  onClose: () => void;
  t: { [key: string]: string };
}

const PasswordChangeModal: React.FC<PasswordChangeModalProps> = ({ onClose, t }) => {
  const [formData, setFormData] = useState({
    oldPassword: "",
    newPassword: "",
    confirmPassword: "",
  });
  const [errorMessage, setErrorMessage] = useState("");
  const langCtx = useContext(LanguageCtx); // Access language context
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { id, value } = e.target;
    setFormData((prevData) => ({ ...prevData, [id]: value }));
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setErrorMessage("");

    // Validate passwords match
    if (formData.newPassword !== formData.confirmPassword) {
        {langCtx?.translate.passwordsDoNotMatch || "Jelszó módosítása"}
      return;
    }

    try {
      // Here you would call an API to change the password
      // For now we'll just show a success message
      {langCtx?.translate.changePassword || "Jelszó módosítása"}
      onClose();
    } catch (error) {
        {langCtx?.translate.errorOccurred || "Hiba történt. Kérjük, próbálja újra."}
    }
  };

  return (
    <div className={styles["modal-overlay"]}>
      <div className={styles["modal-container"]}>
        <button className={styles["modal-close"]} onClick={onClose}>&times;</button>
        <h2>{t.changePassword || "Jelszó módosítása"}</h2>

        {errorMessage && <p className={styles["error-message"]}>{errorMessage}</p>}

        <form onSubmit={handleSubmit} className={styles["modal-form"]}>
          <label htmlFor="oldPassword">{langCtx?.translate.oldPassword}:</label>
          <input 
            type="password" 
            id="oldPassword" 
            required 
            value={formData.oldPassword} 
            onChange={handleChange} 
          />

          <label htmlFor="newPassword">{langCtx?.translate.newPassword}:</label>
          <input 
            type="password" 
            id="newPassword" 
            required 
            value={formData.newPassword} 
            onChange={handleChange} 
          />

          <label htmlFor="confirmPassword">{langCtx?.translate.confirmPassword}:</label>
          <input 
            type="password" 
            id="confirmPassword" 
            required 
            value={formData.confirmPassword} 
            onChange={handleChange} 
          />

          <button type="submit" className={styles["btn"]}>
          {langCtx?.translate.changePassword}
          </button>
        </form>
      </div>
    </div>
  );
};

export default PasswordChangeModal;