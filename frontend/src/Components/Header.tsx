import React, { useState } from "react";
import { Link } from "react-router-dom";
import { translations } from "../translations";
import LoginModal from "./LoginModal";
import MessagesModal from "./MessagesModal";
import SavedCarsModal from "./SavedCarsModal";
import { User } from "../Types";
import HirdetesFeladas from "./HirdetesFeladas";



interface HeaderProps {
  isLoggedIn: boolean;
  setIsLoggedIn: React.Dispatch<React.SetStateAction<boolean>>;
  user: User;
  setUser: React.Dispatch<React.SetStateAction<User>>;
  language: "hu" | "en"; // Új prop
  setLanguage: React.Dispatch<React.SetStateAction<"hu" | "en">>;
}

function Header({ isLoggedIn, setIsLoggedIn, user, setUser }: HeaderProps) {
  const [language, setLanguage] = useState<"hu" | "en">("hu");
  const [showLoginModal, setShowLoginModal] = useState(false);
  const [showMessagesModal, setShowMessagesModal] = useState(false);
  const [showSavedCarsModal, setShowSavedCarsModal] = useState(false);
  const [showRegister, setShowRegister] = useState(false);
  const [profileMenuOpen, setProfileMenuOpen] = useState(false);


  const t = translations[language] || translations.hu;

  const handleLanguageChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const lang = e.target.value as "hu" | "en";
    setLanguage(lang);
  };

  const handleLogout = () => {
    setIsLoggedIn(false);
    alert(t.logout + " sikeres!");
    // pl. localStorage törlése, user reset...
  };

  // Egyszerű dropdown toggling (opcionális)
  // Itt demonstrációként az "Profil" gombra kattintva mutathatnánk egy menüt:

  return (
    <header className="header">
        <div className="container">
          {/* Nyelvválasztó */}
          <div className="language-selector">
            <label htmlFor="language-selector">{t.chooseLanguage || "Nyelv:"}</label>
            <select id="language-selector" value={language} onChange={handleLanguageChange}>
              <option value="hu">Magyar</option>
              <option value="en">English</option>
            </select>
          </div>

        {/* Navigáció */}
        <nav>
          <ul className="nav-list">
            <li>
            <Link to="/" className="nav-link">{t.homepage}</Link>
            </li>
            <li>
            <Link to="/autok" className="nav-link">{t.cars}</Link>
            </li>
            <li>
            <HirdetesFeladas language={language} isLoggedIn={isLoggedIn} setShowLoginModal={setShowLoginModal} />
            </li>

            {/* Profil / Login */}
            <li>
              {!isLoggedIn ? (
                <button className="login-button" onClick={() => setShowLoginModal(true)}>
                {t.login}
              </button>
              ) : (
                <div className="profile-menu">
                  <button
                    className="profile-btn"
                    onClick={() => setProfileMenuOpen(!profileMenuOpen)}
                  >
                    {t.myProfile}
                  </button>
                  {profileMenuOpen && (
                    <ul
                      className="dropdown-menu"
                      
                    >
                      <li style={{ marginBottom: "5px" }}>
                        <Link to="/profile" onClick={() => setProfileMenuOpen(false)}>
                          {t.myProfile}
                        </Link>
                      </li>
                      <li style={{ marginBottom: "5px" }}>
                        <button
                          onClick={() => {
                            setShowMessagesModal(true);
                            setProfileMenuOpen(false);
                          }}
                        >
                          {t.myMessages}
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
                        <button onClick={handleLogout}>{t.logout}</button>
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
          toggleRegister={() => { setShowRegister(true); setShowLoginModal(false); }}
          t={t} 
        />
      )}
      {showRegister && (
        <LoginModal 
          onClose={() => setShowRegister(false)} 
          setIsLoggedIn={setIsLoggedIn} 
          isRegisterMode={true} 
          t={t} 
        />
      )}
      {showMessagesModal && <MessagesModal onClose={() => setShowMessagesModal(false)} t={t} />}
      {showSavedCarsModal && <SavedCarsModal onClose={() => setShowSavedCarsModal(false)} t={t} />}
    </header>
  );
}

export default Header;
