import React, { useState } from "react";
import { login, register } from "../api/userService.ts";
import styles from './LoginModal.module.css';
import { translations } from "../translations";
import { LanguageCtx } from "../App";
import { useContext } from "react";

interface LoginModalProps {
  onClose: () => void;
  setIsLoggedIn: React.Dispatch<React.SetStateAction<boolean>>;
  t: { [key: string]: string };
  language: "hu" | "en" | "jp";
}

const LoginModal: React.FC<LoginModalProps> = ({ onClose, setIsLoggedIn, t, language }) => {
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
  const langCtx = useContext(LanguageCtx)
  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setErrorMessage(""); // Reset error message

    try {
      if (isRegisterMode) {
        await register({
          name: formData.name,
          nameKanji: language === "jp" ? formData.nameKanji : undefined,
          userName: formData.userName,
          email: formData.email,
          password: formData.password,
          phoneNumber: formData.phoneNumber,
          preferredLanguage: formData.preferredLanguage,
        });
        alert(langCtx?.translate.successRegister);
        setIsRegisterMode(false);
      } else {
        await login(formData.email, formData.password);
        alert(langCtx?.translate.successLogin);
        setIsLoggedIn(true);
        onClose();
      }
    } catch (error) {
      setErrorMessage(t.error);
      console.error(error);
    }
  };

  return (
      <div className={styles['modal-overlay']}>
        <div className={styles['modal-container']}>
          <button className={styles['modal-close']} onClick={onClose}>&times;</button>
          <h2>{isRegisterMode ? langCtx?.translate.registerTitle : langCtx?.translate.loginTitle}</h2>

          {errorMessage && <p className={styles['error-message']}>{errorMessage}</p>}

          <form onSubmit={handleSubmit} className={styles['modal-form']}>
            {isRegisterMode && (
                <>
                  <label htmlFor="name">{langCtx?.translate.name}:</label>
                  <input type="text" id="name" required value={formData.name} onChange={handleChange} />

                  {language === "jp" && (
                      <>
                        <label htmlFor="nameKanji">{langCtx?.translate.nameKanji}名前（漢字）</label>
                        <input type="text" id="nameKanji" value={formData.nameKanji} onChange={handleChange} />
                      </>
                  )}

                  <label htmlFor="userName">{langCtx?.translate.username}:</label>
                  <input type="text" id="userName" required value={formData.userName} onChange={handleChange} />

                  <label htmlFor="phoneNumber">{langCtx?.translate.phoneNumber}:</label>
                  <input type="tel" id="phoneNumber" required value={formData.phoneNumber} onChange={handleChange} />
                </>
            )}

            <label htmlFor="email">{langCtx?.translate.email}:</label>
            <input type="email" id="email" required value={formData.email} onChange={handleChange} />

            <label htmlFor="password">{langCtx?.translate.password}:</label>
            <input type="password" id="password" required value={formData.password} onChange={handleChange} />

            <button type="submit" className={styles['btn']}>
              {isRegisterMode ? langCtx?.translate.register : langCtx?.translate.login}
            </button>
          </form>

          <button className={styles['toggle-button']} onClick={() => setIsRegisterMode(!isRegisterMode)}>
            {isRegisterMode ? langCtx?.translate.switchToLogin : langCtx?.translate.switchToRegister}
          </button>
        </div>
      </div>
  );
};

export default LoginModal;
