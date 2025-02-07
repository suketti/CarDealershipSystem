import React, { useContext, useState } from "react";
import { Link } from "react-router-dom";
import { translations } from "../translations";
import LoginModal from "./LoginModal";
import MessagesModal from "./MessagesModal";
import SavedCarsModal from "./SavedCarsModal";
import {UserDTO} from "../Interfaces/User.ts";
import PostCarOffer from "./PostCarOffer.tsx";
import { LanguageCtx } from "../App.tsx";
import { translations as trans } from "../translations";

interface HeaderProps {
  isLoggedIn: boolean;
  setIsLoggedIn: React.Dispatch<React.SetStateAction<boolean>>;
  user: UserDTO;
  setUser: React.Dispatch<React.SetStateAction<UserDTO>>;
  language: "hu" | "en" | "jp"; // Új prop
  setLanguage: React.Dispatch<React.SetStateAction<"hu" | "en" | "jp">>;
}

function Header({ isLoggedIn, setIsLoggedIn, user, setUser, language, setLanguage }: HeaderProps) {
  const langCtx = useContext(LanguageCtx)
  const [showLoginModal, setShowLoginModal] = useState(false);
  const [showMessagesModal, setShowMessagesModal] = useState(false);
  const [showSavedCarsModal, setShowSavedCarsModal] = useState(false);
  const [showRegister, setShowRegister] = useState(false);
  const [profileMenuOpen, setProfileMenuOpen] = useState(false);

  const t = translations[language] || translations.hu;

  const handleLanguageChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const lang = e.target.value as "hu" | "en" | "jp";
    //setLanguage(lang);
    switch (lang) {
      case "hu":
        langCtx?.changeTranslate(trans.hu)
        break;
      case "en":
        langCtx?.changeTranslate(trans.en)
        break;
      case "jp":
        langCtx?.changeTranslate(trans.jp)
        break;
      default:
        break;
    }
  };


  const handleLogout = () => {
    setIsLoggedIn(false);
    alert(langCtx?.translate.logout + " sikeres!");
    // pl. localStorage törlése, user reset...
  };

  // Egyszerű dropdown toggling (opcionális)
  // Itt demonstrációként az "Profil" gombra kattintva mutathatnánk egy menüt:

  return (
    <header className="header">
        <div className="container">
          {/* Nyelvválasztó */}
          <div className="language-selector">
            <label htmlFor="language-selector">{langCtx?.translate.langu}</label>
            <select id="language-selector" value={language} onChange={handleLanguageChange}>
              <option value="hu">Magyar</option>
              <option value="en">English</option>
              <option value="jp">日本語</option>
            </select>
          </div>

        {/* Navigáció */}
        <nav>
          <ul className="nav-list">
            <li>
            <Link to="/" className="nav-link">{langCtx?.translate.homepage}</Link>
            </li>
            <li>
            <Link to="/cars" className="nav-link">{langCtx?.translate.cars}</Link>
            </li>
            <li>
              <PostCarOffer language={language} isLoggedIn={isLoggedIn} setShowLoginModal={setShowLoginModal} />
            </li>
            {/* Profil / Login */}
            <li>
              {!isLoggedIn ? (
                <button className="login-button" onClick={() => setShowLoginModal(true)}>
                {langCtx?.translate.login}
              </button>
              ) : (
                <div className="profile-menu">
                  <button
                    className="profile-btn"
                    onClick={() => setProfileMenuOpen(!profileMenuOpen)}
                  >
                    {langCtx?.translate.myProfile}
                  </button>
                  {profileMenuOpen && (
                    <ul
                      className="dropdown-menu"
                      
                    >
                      <li style={{ marginBottom: "5px" }}>
                        <Link to="/profile" onClick={() => setProfileMenuOpen(false)}>
                        {langCtx?.translate.myProfile}
                        </Link>
                      </li>
                      <li style={{ marginBottom: "5px" }}>
                        <button
                          onClick={() => {
                            setShowMessagesModal(true);
                            setProfileMenuOpen(false);
                          }}
                        >
                          {langCtx?.translate.myMessages}
                        </button>
                      </li>
                      <li style={{ marginBottom: "5px" }}>
                        <button
                          onClick={() => {
                            setShowSavedCarsModal(true);
                            setProfileMenuOpen(false);
                          }}
                        >
                          Mentett autóim
                        </button>
                      </li>
                      <li>
                        <button onClick={handleLogout}>{langCtx?.translate.logout}</button>
                      </li>
                    </ul>
                  )}
                </div>
              )}
            </li>
          </ul>
        </nav>
      </div>

      {showLoginModal && (
        <LoginModal 
          onClose={() => setShowLoginModal(false)} 
          setIsLoggedIn={setIsLoggedIn}
          language={language}
          toggleRegister={() => { setShowRegister(true); setShowLoginModal(false); }}
          t={t} 
        />
      )}
      {showRegister && (
        <LoginModal 
          onClose={() => setShowRegister(false)} 
          setIsLoggedIn={setIsLoggedIn} 
          isRegisterMode={true}
          language={language}
          t={t} 
        />
      )}
      {showMessagesModal && <MessagesModal onClose={() => setShowMessagesModal(false)} t={t} />}
      {showSavedCarsModal && <SavedCarsModal onClose={() => setShowSavedCarsModal(false)} t={t} />}
    </header>
  );
}

export default Header;
