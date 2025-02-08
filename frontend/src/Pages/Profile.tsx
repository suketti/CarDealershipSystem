import React, { useState } from "react";
import { User } from "../Types";
import { translations } from "../translations";
import { LanguageCtx } from "../App";
import { useContext } from "react";

interface ProfileProps {
  isLoggedIn: boolean;
  user: User;
}

function Profile({ isLoggedIn, user }: ProfileProps) {
    const langCtx = useContext(LanguageCtx)
  const [name, setName] = useState(user.name || "Test User");
  const [email, setEmail] = useState(user.email || "testuser@example.com");
  const [phone, setPhone] = useState(user.phone || "+36 30 123 4567");

  if (!isLoggedIn) {
    return <p>{langCtx?.translate.loginForData}</p>;
  }

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    alert("Adataid mentve (demo)!");
    // Esetleg user state frissítése...
  };

  return (
    <main>
      <div className="container">
        <h2>{langCtx?.translate.myData}</h2>
        <form id="edit-profile" onSubmit={handleSubmit}>
          <label htmlFor="name">{langCtx?.translate.name}</label>
          <input
            type="text"
            id="name"
            value={name}
            onChange={(e) => setName(e.target.value)}
          />

          <label htmlFor="email">{langCtx?.translate.email}</label>
          <input
            type="email"
            id="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />

          <label htmlFor="phone">{langCtx?.translate.phoneNumber}</label>
          <input
            type="tel"
            id="phone"
            value={phone}
            onChange={(e) => setPhone(e.target.value)}
          />

          <button type="submit" className="btn">
            {langCtx?.translate.save}
          </button>
        </form>
      </div>
    </main>
  );
}

export default Profile;
