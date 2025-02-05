import React, { useState } from "react";
import { User } from "../Types";

interface ProfileProps {
  isLoggedIn: boolean;
  user: User;
}

function Profile({ isLoggedIn, user }: ProfileProps) {
  const [name, setName] = useState(user.name || "Test User");
  const [email, setEmail] = useState(user.email || "testuser@example.com");
  const [phone, setPhone] = useState(user.phone || "+36 30 123 4567");

  if (!isLoggedIn) {
    return <p>Kérjük, jelentkezz be a profil megtekintéséhez!</p>;
  }

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    alert("Adataid mentve (demo)!");
    // Esetleg user state frissítése...
  };

  return (
    <main>
      <div className="container">
        <h2>Adataim</h2>
        <form id="edit-profile" onSubmit={handleSubmit}>
          <label htmlFor="name">Név:</label>
          <input
            type="text"
            id="name"
            value={name}
            onChange={(e) => setName(e.target.value)}
          />

          <label htmlFor="email">E-mail:</label>
          <input
            type="email"
            id="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />

          <label htmlFor="phone">Telefonszám:</label>
          <input
            type="tel"
            id="phone"
            value={phone}
            onChange={(e) => setPhone(e.target.value)}
          />

          <button type="submit" className="btn">
            Mentés
          </button>
        </form>
      </div>
    </main>
  );
}

export default Profile;
