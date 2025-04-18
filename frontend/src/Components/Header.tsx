import React, { useContext, useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { LanguageCtx, MobileMenuContext } from '../App';
import { translations } from '../translations';
import LoginModal from './LoginModal';
import MessagesModal from "./MessagesModal";
import SavedCarsModal from "./SavedCarsModal";
import { useUser } from '../UserContext';
import {changePreferredLanguage} from "../api/userService.ts";

const Header = () => {
  const { user, isAuthenticated, logoutUser } = useUser();
  const langCtx = useContext(LanguageCtx);
  const [isLoggedIn, setIsLoggedIn] = useState(isAuthenticated);
  const { toggleMenu } = useContext(MobileMenuContext);
  const [showUserMenu, setShowUserMenu] = useState(false);
  const [showLoginModal, setShowLoginModal] = useState(false);
  const [profileMenuOpen, setProfileMenuOpen] = useState(false);
  const [showMessagesModal, setShowMessagesModal] = useState(false);
  const [showSavedCarsModal, setShowSavedCarsModal] = useState(false);
  const [selectedLanguage, setSelectedLanguage] = useState(langCtx?.language || "hu");
  
  // Update selectedLanguage when context language changes
  useEffect(() => {
    const updateLanguage = async () => {
      if (langCtx?.language) {
        setSelectedLanguage(langCtx.language);
        if (user) {
          await changePreferredLanguage(user.id, langCtx.language); // Await the language change
        }
      }
    };

    updateLanguage();
  }, [langCtx?.language, user]);

  // Add effect to update isLoggedIn when authentication state changes
  useEffect(() => {
    setIsLoggedIn(isAuthenticated);
  }, [isAuthenticated]);

  const toggleUserMenu = () => {
    setShowUserMenu(!showUserMenu);
  };

  const handleLanguageChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const newLanguage = e.target.value as "hu" | "en" | "jp";
    setSelectedLanguage(newLanguage);
    if (langCtx) {
      langCtx.changeTranslate(translations[newLanguage], newLanguage); // ✅ Now passing both args
    }
  };

  // Handle logout
  const clearCookies = () => {
    document.cookie.split(";").forEach((cookie) => {
      document.cookie = cookie
          .replace(/^ +/, "")
          .replace(/=.*/, "=;expires=" + new Date(0).toUTCString() + ";path=/");
    });
  };

  // Handle logout
  const handleLogout = () => {
    if (user && user.id) {
      logoutUser(user.id);
      clearCookies();
      localStorage.removeItem("AccessToken");
      localStorage.removeItem("RefreshToken");
      setShowUserMenu(false);
      window.location.reload();
    } else {
      console.error("User ID is not available");
    }
  };

  return (
    <header className="header">
      <div className="container header-container">
        <div className="logo">
          <Link to="/">
            <FontAwesomeIcon icon="car" className="logo-icon" />
            <span>Premium</span>Cars
          </Link>
        </div>

        {/* Hamburger menu button for mobile */}
        <button className="hamburger-menu" onClick={toggleMenu}>
          <FontAwesomeIcon icon="bars" />
        </button>

        {/* Desktop navigation */}
        <div className="desktop-nav">
          <div className="language-selector">
            <FontAwesomeIcon icon="language" className="language-icon" />
            <select
              onChange={handleLanguageChange}
              value={selectedLanguage}
            >
              <option value="hu">Magyar</option>
              <option value="en">English</option>
              <option value="jp">日本語</option>
            </select>
          </div>

          <nav>
            <ul className="nav-list">
              <li>
                <Link to="/" className="nav-link">
                  <FontAwesomeIcon icon="home" /> {langCtx?.translate.homepage}
                </Link>
              </li>
              <li>
                <Link to="/cars" className="nav-link">
                  <FontAwesomeIcon icon="car" /> {langCtx?.translate.cars}
                </Link>

              </li>
              <li>
                <Link to="/locations" className="nav-link">
                  <FontAwesomeIcon icon="location-dot" /> {langCtx?.translate.locations}
                </Link>
              </li>
            </ul>
          </nav>

          {isLoggedIn ? (
            <div className="profile-menu">
              <button className="profile-btn" onClick={toggleUserMenu}>
                <FontAwesomeIcon icon="user" /> {langCtx?.translate.myProfile}
              </button>
              {showUserMenu && (
                <ul className="dropdown-menu">
                  <li>
                    <Link to="/profile" onClick={() => setShowUserMenu(false)}>
                      <FontAwesomeIcon icon="user" /> {langCtx?.translate.myData}
                    </Link>
                  </li>
                  <li>
                    <button onClick={() => {
                      setShowSavedCarsModal(true);
                      setShowUserMenu(false);
                    }}>
                      <FontAwesomeIcon icon="heart" /> {langCtx?.translate.savedCars}
                    </button>
                  </li>
                  <li>
                    <button onClick={() => {
                      setShowMessagesModal(true);
                      setShowUserMenu(false);
                    }}>
                      <FontAwesomeIcon icon="envelope" /> {langCtx?.translate.myMessages}
                    </button>
                  </li>
                  <li>
                    <button onClick={handleLogout}>
                      <FontAwesomeIcon icon="sign-out-alt" /> {langCtx?.translate.logout}
                    </button>
                  </li>
                </ul>
              )}
            </div>
          ) : (
            <button className="login-button" onClick={() => setShowLoginModal(true)}>
              <FontAwesomeIcon icon="sign-in-alt" /> {langCtx?.translate.login}
            </button>
          )}
        </div>
      </div>
      {showLoginModal && (
        <LoginModal
          onClose={() => setShowLoginModal(false)}
          language={langCtx?.language || "hu"}
          t={langCtx?.translate || translations.hu}
        />
      )}
      {showMessagesModal && <MessagesModal onClose={() => setShowMessagesModal(false)} t={langCtx?.translate || translations.hu} />}
      {showSavedCarsModal && <SavedCarsModal onClose={() => setShowSavedCarsModal(false)} t={langCtx?.translate || translations.hu} />}
    </header>
  );
};

export default Header;