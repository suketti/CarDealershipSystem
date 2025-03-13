import React, { createContext, useState, useEffect } from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Header from "./Components/Header";
import MobileMenu from "./Components/MobileMenu";
import Footer from "./Components/Footer";
import Home from "./Pages/Home";
import Cars from "./Pages/Cars";
import CarDetails from "./Pages/CarDetails";
import Profile from "./Pages/Profile";
import { translations, TranslationType } from "./translations";
import { UserProvider } from "./UserContext";

// Import icons
import { library } from '@fortawesome/fontawesome-svg-core';
import { 
  faCar, faHome, faUser, faSearch, faTachometerAlt, faGasPump, 
  faCalendarAlt, faMap, faAngleDown, faAngleRight, faBars, faTimes,
  faEnvelope, faHeart, faSignOutAlt, faSignInAlt, faLanguage,
  faChevronLeft, faChevronRight, faMapMarkerAlt, faCalendarCheck,
  faCalendarPlus, faCheckCircle, faExclamationCircle, faSpinner,
  faCogs, faArrowLeft, faImage
} from '@fortawesome/free-solid-svg-icons';

// Add icons to library
library.add(
  faCar, faHome, faUser, faSearch, faTachometerAlt, faGasPump,
  faCalendarAlt, faMap, faAngleDown, faAngleRight, faBars, faTimes,
  faEnvelope, faHeart, faSignOutAlt, faSignInAlt, faLanguage,
  faChevronLeft, faChevronRight, faMapMarkerAlt, faCalendarCheck, 
  faCalendarPlus, faCheckCircle, faExclamationCircle, faSpinner,
  faCogs, faArrowLeft, faImage
);

export const LanguageCtx = createContext<
    {
      translate: TranslationType;
      changeTranslate: (translate: TranslationType, newLanguage: "hu" | "en" | "jp") => void;
      language: "hu" | "en" | "jp";
    } | undefined
>(undefined);


// Context for managing mobile menu state
export const MobileMenuContext = createContext<{
  isMenuOpen: boolean;
  toggleMenu: () => void;
}>({
  isMenuOpen: false,
  toggleMenu: () => {},
});

function App() {
  const [language, setLanguage] = useState<"hu" | "en" | "jp">("hu");
  const [translate, setTranslate] = useState<TranslationType>(translations.hu);
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [isMobile, setIsMobile] = useState(window.innerWidth <= 768);

  useEffect(() => {
    const handleResize = () => {
      setIsMobile(window.innerWidth <= 768);
      if (window.innerWidth > 768) {
        setIsMenuOpen(false);
      }
    };

    window.addEventListener('resize', handleResize);
    return () => window.removeEventListener('resize', handleResize);
  }, []);

  const changeTranslate = (translate: TranslationType, newLanguage: "hu" | "en" | "jp") => {
    setTranslate(translate);
    setLanguage(newLanguage);  // This updates the language
  };


  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
  };

  return (
    <UserProvider>
      <LanguageCtx.Provider value={{ translate, changeTranslate, language }}>
        <MobileMenuContext.Provider value={{ isMenuOpen, toggleMenu }}>
          <Router>
            <div className="app-container">
              <Header />
              {isMobile && <MobileMenu />}
              <main className="main-content">
                <Routes>
                  <Route path="/" element={<Home language={language} />} />
                  <Route path="/cars" element={<Cars />} />
                  <Route path="/Car-Details" element={<CarDetails />} />
                  <Route path="/profile" element={<Profile/>} />
                </Routes>
              </main>
              <Footer language={language} />
            </div>
          </Router>
        </MobileMenuContext.Provider>
      </LanguageCtx.Provider>
    </UserProvider>
  );
}

export default App;