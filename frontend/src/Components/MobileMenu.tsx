import React, { useContext, useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { MobileMenuContext } from '../App';
import { LanguageCtx } from '../App';
import { translations } from '../translations';
import LoginModal from './LoginModal';
import SavedCarsModal from './SavedCarsModal';
import { useUser } from '../UserContext';
import MessagesModal from './MessagesModal';

const MobileMenu = () => {
  const { isMenuOpen, toggleMenu } = useContext(MobileMenuContext);
  const langCtx = useContext(LanguageCtx);
  const navigate = useNavigate();
  const { user, isAuthenticated, logoutUser } = useUser();
  const [showLoginModal, setShowLoginModal] = useState(false);
  const [showMessagesModal, setShowMessagesModal] = useState(false);
  const [showSavedCarsModal, setShowSavedCarsModal] = useState(false);
  const [selectedLanguage, setSelectedLanguage] = useState(langCtx?.language || "hu");

  // Update selectedLanguage when context language changes
  useEffect(() => {
    if (langCtx?.language) {
      setSelectedLanguage(langCtx.language);
    }
  }, [langCtx?.language]);

  // Handle language change
  const handleLanguageChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const newLanguage = e.target.value as "hu" | "en" | "jp";
    setSelectedLanguage(newLanguage);
    if (langCtx) {
      langCtx.changeTranslate(translations[newLanguage]);
    }
  };

  const handleLogin = () => {
    setShowLoginModal(true);
  };

  const handleLogout = () => {
    logoutUser();
    toggleMenu(); // Close menu after logout
  };

  if (!isMenuOpen) return null;

  return (
    <div className="mobile-menu-overlay">
      <div className="mobile-menu">
        <div className="mobile-menu-header">
          <button className="mobile-menu-close" onClick={toggleMenu}>
            <FontAwesomeIcon icon="times" />
          </button>
        </div>
        
        <div className="mobile-menu-content">
          <nav>
            <ul className="mobile-nav-list">
              <li>
                <Link to="/" className="mobile-nav-link" onClick={toggleMenu}>
                  <FontAwesomeIcon icon="home" /> {langCtx?.translate.homepage}
                </Link>
              </li>
              <li>
                <Link to="/cars" className="mobile-nav-link" onClick={toggleMenu}>
                  <FontAwesomeIcon icon="car" /> {langCtx?.translate.cars}
                </Link>
              </li>
              <li>
                              <Link to="/locations" className="mobile-nav-link" onClick={toggleMenu}>
                                <FontAwesomeIcon icon="location-dot" /> {langCtx?.translate.locations}
                              </Link>
                            </li>
              
              {isAuthenticated ? (
                <>
                  <li>
                    <Link to="/profile" className="mobile-nav-link" onClick={toggleMenu}>
                      <FontAwesomeIcon icon="user" /> {langCtx?.translate.myProfile}
                    </Link>
                  </li>
                  <li>
                    <button 
                      className="mobile-nav-link" 
                      onClick={() => {
                        setShowSavedCarsModal(true);
                      }}
                      style={{ width: '100%', textAlign: 'left', border: 'none', background: 'none' }}
                    >
                      <FontAwesomeIcon icon="heart" /> {langCtx?.translate.savedCars}
                    </button>
                  </li>
                  <li>
                    <button 
                      className="mobile-nav-link" 
                      onClick={() => {
                        setShowMessagesModal(true);
                      }}
                      style={{ width: '100%', textAlign: 'left', border: 'none', background: 'none' }}
                    >
                      <FontAwesomeIcon icon="envelope" /> {langCtx?.translate.myMessages}
                    </button>
                  </li>
                  <li>
                    <button className="mobile-nav-link" onClick={handleLogout} style={{ width: '100%', textAlign: 'left', border: 'none', background: 'none' }}>
                      <FontAwesomeIcon icon="sign-out-alt" /> {langCtx?.translate.logout}
                    </button>
                  </li>
                </>
              ) : (
                <li>
                  <button className="mobile-nav-link" onClick={handleLogin} style={{ width: '100%', textAlign: 'left', border: 'none', background: 'none' }}>
                    <FontAwesomeIcon icon="sign-in-alt" /> {langCtx?.translate.login}
                  </button>
                </li>
              )}
            </ul>
          </nav>
          
          <div className="mobile-language-selector">
            <div className="mobile-language-icon">
              <FontAwesomeIcon icon="language" />
            </div>
            <select
              onChange={handleLanguageChange}
              value={selectedLanguage}
            >
              <option value="hu">Magyar</option>
              <option value="en">English</option>
              <option value="jp">日本語</option>
            </select>
          </div>
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
    </div>
  );
};

export default MobileMenu;