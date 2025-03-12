import React, { useContext } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { MobileMenuContext } from '../App';
import { LanguageCtx } from '../App';
import { translations } from '../translations';

const MobileMenu = () => {
  const { isMenuOpen, toggleMenu } = useContext(MobileMenuContext);
  const langCtx = useContext(LanguageCtx);
  const navigate = useNavigate();

  // Handle language change
  const handleLanguageChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const selectedLanguage = e.target.value as "hu" | "en" | "jp";
    if (langCtx) {
      langCtx.changeTranslate(translations[selectedLanguage]);
    }
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
                <Link to="/profile" className="mobile-nav-link" onClick={toggleMenu}>
                  <FontAwesomeIcon icon="user" /> {langCtx?.translate.myProfile}
                </Link>
              </li>
            </ul>
          </nav>
          
          <div className="mobile-language-selector">
            <div className="mobile-language-icon">
              <FontAwesomeIcon icon="language" />
            </div>
            <select
              onChange={handleLanguageChange}
              value={langCtx?.language}
            >
              <option value="hu">Magyar</option>
              <option value="en">English</option>
              <option value="jp">日本語</option>
            </select>
          </div>
        </div>
      </div>
    </div>
  );
};

export default MobileMenu;