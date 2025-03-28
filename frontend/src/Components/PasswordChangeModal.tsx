import React, { useState, useContext } from "react";
import styles from "./LoginModal.module.css";
import { LanguageCtx } from "../App";
import { changePassword } from "../api/userService.ts";

interface PasswordChangeModalProps {
  onClose: () => void;
  t: { [key: string]: string };
  userId: string;
}

const PasswordChangeModal: React.FC<PasswordChangeModalProps> = ({ onClose, t, userId }) => {
  const [formData, setFormData] = useState({
    oldPassword: "",
    newPassword: "",
    confirmPassword: "",
  });
  const [errorMessage, setErrorMessage] = useState("");
  const langCtx = useContext(LanguageCtx);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { id, value } = e.target;
    setFormData((prevData) => ({ ...prevData, [id]: value }));
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setErrorMessage("");

    if (formData.newPassword !== formData.confirmPassword) {
      setErrorMessage(langCtx?.translate.passwordsDoNotMatch || "Passwords do not match");
      return;
    }

    const success = await changePassword(userId, formData.oldPassword, formData.newPassword);
    if (success) {
      alert(langCtx?.translate.passwordChangeSuccess || "Password changed successfully!");
      onClose();
    } else {
      setErrorMessage(langCtx?.translate.wrongPassword || "Wrong Password");
    }
  };

  return (
      <div className={styles["modal-overlay"]}>
        <div className={styles["modal-container"]}>
          <button className={styles["modal-close"]} onClick={onClose}>&times;</button>
          <h2>{t.changePassword || "Change Password"}</h2>

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
              {langCtx?.translate.changePassword || "Change Password"}
            </button>
          </form>
        </div>
      </div>
  );
};

export default PasswordChangeModal;
