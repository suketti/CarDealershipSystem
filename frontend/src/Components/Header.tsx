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
  // Mock user state - replace with your actual user authentication state
  
  const toggleUserMenu = () => {
    setShowUserMenu(!showUserMenu);
  };

  const handleLanguageChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const selectedLanguage = e.target.value as "hu" | "en" | "jp";
    if (langCtx) {
      langCtx.changeTranslate(translations[selectedLanguage]);
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
                    <Link to="/profile">
                      <FontAwesomeIcon icon="user" /> {langCtx?.translate.myProfile}
                    </Link>
                  </li>
                  <li>
                    <Link to="/saved-cars">
                      <FontAwesomeIcon icon="heart" /> {langCtx?.translate.savedCars}
                    </Link>
                  </li>
                  <li>
                    <Link to="/messages">
                      <FontAwesomeIcon icon="envelope" /> {langCtx?.translate.myMessages}
                    </Link>
                  </li>
                  <li>
                    <button onClick={() => setIsLoggedIn(false)}>
                      <FontAwesomeIcon icon="sign-out-alt" /> {langCtx?.translate.logout}
                    </button>
                  </li>
                </ul>
              )}
            </div>
          ) : (
            <button className="login-button">
              <FontAwesomeIcon icon="sign-in-alt" /> {langCtx?.translate.login}
            </button>
          )}
        </div>
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
};

export default Header;