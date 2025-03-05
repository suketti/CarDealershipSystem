import React, { useContext, useState, useEffect } from "react";
import { useUser } from "../UserContext.tsx"; // Import the custom hook
import { LanguageCtx } from "../App";
import { UserDTO } from "../Interfaces/User.ts";

function Profile() {
  const { user, isAuthenticated, registerUser } = useUser(); // Consume user context
  const langCtx = useContext(LanguageCtx); // Access language context

  // Initialize state with user data from context, or use default values
  const [name, setName] = useState<string>(user?.name || "Test User");
  const [email, setEmail] = useState<string>(user?.email || "testuser@example.com");
  const [phone, setPhone] = useState<string>(user?.phoneNumber || "+36 30 123 4567");
  const [nameKanji, setNameKanji] = useState<string>(user?.nameKanji || ""); // Field for kanji name

  // Update the state with user data whenever the user context changes
  useEffect(() => {
    if (user) {
      setName(user.name || "Test User");
      setEmail(user.email || "testuser@example.com");
      setPhone(user.phoneNumber || "+36 30 123 4567");
      setNameKanji(user.nameKanji || ""); // Update Kanji name field
    }
  }, [user]);

  // Display a message if the user is not authenticated
  if (!isAuthenticated) {
    return <p>{langCtx?.translate.loginForData}</p>; // Show message if not logged in
  }

  // Handle form submission to update user data
  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    // Assuming you want to update user data through a registerUser or similar method
    // In practice, you might call an API to update user data
    const updatedUser: UserDTO = {
      ...user!,
      name,
      email,
      phoneNumber: phone,
      nameKanji, // Add the Kanji name field if it exists
    };

    // Update user in context (you might use a different function to update the server if necessary)
    registerUser(updatedUser).then(() => {
      alert(langCtx?.translate.dataSaved || "Your data has been saved!");
    }).catch((error) => {
      console.error("Error saving user data:", error);
    });
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

            {/* Conditionally render the Kanji Name field for Japanese language */}
            {langCtx?.language == "jp" && (
                <>
                  <label htmlFor="nameKanji">{langCtx?.translate.nameKanji || "名前(漢字)"}</label>
                  <input
                      type="text"
                      id="nameKanji"
                      value={nameKanji}
                      onChange={(e) => setNameKanji(e.target.value)}
                  />
                </>
            )}

            <button type="submit" className="btn">
              {langCtx?.translate.save}
            </button>
          </form>
        </div>
      </main>
  );
}

export default Profile;
