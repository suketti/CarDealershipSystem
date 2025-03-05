import React, { createContext, useState } from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Header from "./Components/Header";
import Footer from "./Components/Footer";
import Home from "./Pages/Home";
import Cars from "./Pages/Cars";
import CarDetails from "./Pages/CarDetails";
import Profile from "./Pages/Profile";
import { translations, TranslationType } from "./translations";
import { UserProvider } from "./UserContext.tsx"; // Import UserProvider

export const LanguageCtx = createContext<
    {
      translate: TranslationType;
      changeTranslate: (translate: TranslationType) => void;
      language: "hu" | "en" | "jp";
    } | undefined
>(undefined);

function App() {
  const [language, setLanguage] = useState<"hu" | "en" | "jp">("hu");
  const [translate, setTranslate] = useState<TranslationType>(translations.hu);

  const changeTranslate = (translate: TranslationType) => {
    setTranslate(translate);
  };

  return (
      <UserProvider> {/* Wrap your entire app in UserProvider */}
        <LanguageCtx.Provider value={{ translate, changeTranslate, language }}>
          <Router>
            <Header />
            <Routes>
              <Route path="/" element={<Home language={language}  />} />
              <Route path="/cars" element={<Cars />} />
              <Route path="/Car-Details" element={<CarDetails />} />
              <Route path="/profile" element={<Profile/>} />
            </Routes>
            <Footer language={"hu"} />
          </Router>
        </LanguageCtx.Provider>
      </UserProvider>
  );
}

export default App;
