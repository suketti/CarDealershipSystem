import React, { useState } from "react";
import { translations } from "../translations";





interface LoginModalProps {
  onClose: () => void;
  setIsLoggedIn: React.Dispatch<React.SetStateAction<boolean>>;
  t: { [key: string]: string };
  isRegisterMode?: boolean;
  toggleRegister?: () => void;

}

function LoginModal({ onClose, setIsLoggedIn, t,  }: LoginModalProps) {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [email, setEmail] = useState("");
  const [isRegisterMode, setIsRegisterMode] = useState(false);


  const testUser = {
    username: "asd",
    password: "asd"
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (isRegisterMode) {
      alert("Sikeres regisztráció! Most jelentkezz be.");
      setIsRegisterMode(false); // Automatikus visszaváltás bejelentkezési módba
    } else {
      if (username.toLowerCase() === "asd" && password === "asd") {
        alert("Sikeres bejelentkezés!");
        setIsLoggedIn(true);
        onClose();
      } else {
        alert("Hibás felhasználónév vagy jelszó.");
      }
    }
  };

  return (
    <div id="overlay" style={{ display: "block" }}>
      <div
        id="login-modal"
        style={{
          display: "block",
          background: "#fff",
          padding: "20px",
          margin: "100px auto",
          width: "300px",
          position: "relative",
          borderRadius: "8px"
        }}
      >
        <button
          onClick={onClose}
          style={{ position: "absolute", top: "10px", right: "10px" }}
        >
          &times;
        </button>
        <h2>{isRegisterMode ? t.registerTitle || "Regisztráció" : t.loginTitle || "Bejelentkezés"}</h2>
        <form onSubmit={handleSubmit}>
          {isRegisterMode && (
            <>
              <label htmlFor="email">{t.email || "Email:"}</label>
              <input type="email" id="email" required value={email} onChange={(e) => setEmail(e.target.value)} />
            </>
          )}
          <label htmlFor="username">{t.username || "Felhasználónév:"}</label>
          <input type="text" id="username" required value={username} onChange={(e) => setUsername(e.target.value)} />
          <label htmlFor="password">{t.password || "Jelszó:"}</label>
          <input type="password" id="password" required value={password} onChange={(e) => setPassword(e.target.value)} />
          <button type="submit" className="btn">{isRegisterMode ? t.register || "Regisztráció" : t.login || "Bejelentkezés"}</button>
        </form>
        <button onClick={() => setIsRegisterMode(!isRegisterMode)} style={{ marginTop: "10px", display: "block", width: "100%" }}>
          {isRegisterMode ? t.switchToLogin || "Vissza a bejelentkezéshez" : t.switchToRegister || "Regisztráció"}
        </button>
      </div>
    </div>
  );
}

export default LoginModal;
