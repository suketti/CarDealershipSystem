import React, { useContext, useState } from 'react';
import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { LanguageCtx, MobileMenuContext } from '../App';
import { translations } from '../translations';
import LoginModal from './LoginModal';
import MessagesModal from "./MessagesModal";
import SavedCarsModal from "./SavedCarsModal";
import { useUser } from '../UserContext';

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
  
  const toggleUserMenu = () => {
    setShowUserMenu(!showUserMenu);
  };

  const handleLanguageChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const selectedLanguage = e.target.value as "hu" | "en" | "jp";
    if (langCtx) {
      langCtx.changeTranslate(translations[selectedLanguage]);
    }
  };

  // Handle logout
  const handleLogout = () => {
    logoutUser();
    setIsLoggedIn(false);
    setShowUserMenu(false);
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
              value={langCtx?.language}
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
                      <FontAwesomeIcon icon="user" /> {langCtx?.translate.myProfile}
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
          setIsLoggedIn={setIsLoggedIn}
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