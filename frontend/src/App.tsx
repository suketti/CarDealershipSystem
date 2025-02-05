import React, { useState } from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Header from "./Components/Header";
import Footer from "./Components/Footer";
import Home from "./Pages/Home";
import Cars from "./Pages/Cars";
import CarDetails from "./Pages/CarDetails";
import Profile from "./Pages/Profile";
import { User } from "./Types";

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false);

  const [user, setUser] = useState<User>({
    username: "",
    name: "Test User",
    email: "testuser@example.com",
    phone: "+36 30 123 4567"
  });
  const [language, setLanguage] = useState<"hu" | "en">("hu");
  return (
    <Router>
      <Header
        isLoggedIn={isLoggedIn}
        setIsLoggedIn={setIsLoggedIn}
        user={user}
        setUser={setUser}
        language={language} setLanguage={setLanguage}
      />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/autok" element={<Cars />} />
        <Route path="/autok/reszletek" element={<CarDetails />} />
        <Route
          path="/profile"
          element={<Profile isLoggedIn={isLoggedIn} user={user} />}
        />
      </Routes>
      <Footer language={language}/>
    </Router>
  );
}

export default App;
