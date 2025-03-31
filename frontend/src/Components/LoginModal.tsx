import React, { useState } from "react";
import styles from "./LoginModal.module.css";
import { useUser } from "../UserContext";
import { useContext } from "react";
import { LanguageCtx } from "../App.tsx";

interface LoginModalProps {
  onClose: () => void;
  t: { [key: string]: string };
  language: "hu" | "en" | "jp";
}

const LoginModal: React.FC<LoginModalProps> = ({ onClose, t, language }) => {
  const { loginUser, registerUser } = useUser(); // Access login and registration functions
  const langCtx = useContext(LanguageCtx);
  const [isRegisterMode, setIsRegisterMode] = useState(false);
  const [formData, setFormData] = useState({
    email: "",
    password: "",
    name: "",
    nameKanji: "",
    userName: "",
    phoneNumber: "",
    preferredLanguage: "jp",
  });
  const [errorMessage, setErrorMessage] = useState("");

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { id, value } = e.target;
    setFormData((prevData) => ({ ...prevData, [id]: value }));
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setErrorMessage("");

    try {
      if (isRegisterMode) {
        await registerUser(formData);
        alert("Registration successful!");
        setIsRegisterMode(false);
      } else {
        await loginUser(formData.email, formData.password);
        alert("Login successful!");
        onClose();
      }
    } catch (error) {
      {langCtx?.translate.loginProblem};
    }
  };

  return (
      <div className={styles["modal-overlay"]}>
        <div className={styles["modal-container"]}>
          <button className={styles["modal-close"]} onClick={onClose}>&times;</button>
          <h2>{isRegisterMode ? t.registerTitle : t.loginTitle}</h2>

          {errorMessage && <p className={styles["error-message"]}>{errorMessage}</p>}

          <form onSubmit={handleSubmit} className={styles["modal-form"]}>
            {isRegisterMode && (
                <>
                  <label htmlFor="name">{t.name}:</label>
                  <input type="text" id="name" required value={formData.name} onChange={handleChange} />

                  {language === "jp" && (
                      <>
                        <label htmlFor="nameKanji">{t.nameKanji}:</label>
                        <input type="text" id="nameKanji" value={formData.nameKanji} onChange={handleChange} />
                      </>
                  )}

                  <label htmlFor="userName">{t.username}:</label>
                  <input type="text" id="userName" required value={formData.userName} onChange={handleChange} />

                  <label htmlFor="phoneNumber">{t.phoneNumber}:</label>
                  <input type="tel" id="phoneNumber" required value={formData.phoneNumber} onChange={handleChange} />
                </>
            )}

            <label htmlFor="email">{t.email}:</label>
            <input type="email" id="email" required value={formData.email} onChange={handleChange} />

            <label htmlFor="password">{t.password}:</label>
            <input type="password" id="password" required value={formData.password} onChange={handleChange} />

            <button type="submit" className={styles["btn"]}>
              {isRegisterMode ? t.register : t.login}
            </button>
          </form>

          <button className={styles["toggle-button"]} onClick={() => setIsRegisterMode(!isRegisterMode)}>
            {isRegisterMode ? t.switchToLogin : t.switchToRegister}
          </button>
        </div>
      </div>
  );
};

export default LoginModal;
