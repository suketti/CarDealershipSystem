import React, { createContext, useState } from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Header from "./Components/Header";
import Footer from "./Components/Footer";
import Home from "./Pages/Home";
import Cars from "./Pages/Cars";
import CarDetails from "./Pages/CarDetails";
import Profile from "./Pages/Profile";
import { UserDTO } from "/Interfaces/User.ts"
import { translations, TranslationType } from "./translations";

export const LanguageCtx = createContext<
{translate: TranslationType,
changeTranslate:(translate: TranslationType) => void} | undefined>(undefined)

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false);

  const [user, setUser] = useState<UserDTO>({
    username: "",
    name: "Test User",
    email: "testuser@example.com",
    phone: "+36 30 123 4567"
  });
  const [language, setLanguage] = useState<"hu" | "en" | "jp">("hu");
  const [translate, setTranslate] = useState<TranslationType>(translations.hu)

  const changeTranslate = (translate:TranslationType)=>{
    setTranslate(translate)
  }

  return (
    <LanguageCtx.Provider value={{ translate: translate, changeTranslate: changeTranslate }}>

    <Router>
      <Header
        isLoggedIn={isLoggedIn}
        setIsLoggedIn={setIsLoggedIn}
        user={user}
        setUser={setUser}
        language={language} setLanguage={setLanguage}
        />
      <Routes>
        <Route path="/" element={<Home brands={[]} models={[]} bodyTypes={[]} fuelTypes={[]} />} />
        <Route path="/cars" element={<Cars />} />
        <Route path="/cars/details" element={<CarDetails />} />
        <Route
          path="/profile"
          element={<Profile isLoggedIn={isLoggedIn} user={user} />}
          />
      </Routes>
      <Footer language={language}/>
    </Router>
          </LanguageCtx.Provider>
  );
}

export default App;
