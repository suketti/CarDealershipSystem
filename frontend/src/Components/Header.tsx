import React, { useContext, useState } from "react";
import { Link } from "react-router-dom";
import PostCarOffer from "./PostCarOffer.tsx";
import { LanguageCtx } from "../App.tsx";
import { translations as trans } from "../translations";
import { useUser } from "../UserContext";
import LoginModal from "./LoginModal.tsx";
import MessagesModal from "./MessagesModal";
import SavedCarsModal from "./SavedCarsModal";

interface HeaderProps {
  setIsLoggedIn: React.Dispatch<React.SetStateAction<boolean>>;
}

function Header({ setIsLoggedIn }: HeaderProps) {
  const { user, isAuthenticated, logoutUser } = useUser(); // Use the UserContext here
  const langCtx = useContext(LanguageCtx);

  if (!langCtx) {
    throw new Error("LanguageCtx is not provided.");
  }

  const [showLoginModal, setShowLoginModal] = useState(false);
  const [profileMenuOpen, setProfileMenuOpen] = useState(false);
  const [showMessagesModal, setShowMessagesModal] = useState(false);
  const [showSavedCarsModal, setShowSavedCarsModal] = useState(false);

  // Get translations
  const { translate, changeTranslate, language } = langCtx;
  const t = translate || trans.hu;

  // Handle language change
  const handleLanguageChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const selectedLang = e.target.value as "hu" | "en" | "jp";
    changeTranslate(trans[selectedLang]); // Update translation
  };

  // Handle logout
  const handleLogout = async () => {
    await logoutUser();
    setIsLoggedIn(false);  // Update login state
    alert(`${t.logout} sikeres!`);
  };

  return (
      <header className="header">

        <div className="container">
          {/* Language Selector */}

          <div className="language-selector">
            <label htmlFor="language-selector">{t.langu}</label>
            <select id="language-selector" value={language} onChange={handleLanguageChange}>
              <option value="hu">Magyar</option>
              <option value="en">English</option>
              <option value="jp">日本語</option>
            </select>
          </div>
          <h1>{langCtx?.translate.dealership}</h1>
          <nav>
            <ul className="nav-list">
              <li>
                <Link to="/" className="nav-link">{t.homepage}</Link>
              </li>
              <li>
                <Link to="/cars" className="nav-link">{t.cars}</Link>
              </li>
              <li>
                <PostCarOffer language={language} isLoggedIn={isAuthenticated} setShowLoginModal={setShowLoginModal} />
              </li>
              <li>
                {!isAuthenticated ? (
                    <button className="login-button" onClick={() => setShowLoginModal(true)}>
                      {t.login}
                    </button>
                ) : (
                    <div className="profile-menu">
                      <button className="profile-btn" onClick={() => setProfileMenuOpen(!profileMenuOpen)}>
                        {t.myProfile}
                      </button>
                      {profileMenuOpen && (
                          <ul className="dropdown-menu">
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
                                {t.savedCarsHome}
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