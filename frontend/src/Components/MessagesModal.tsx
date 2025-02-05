import React, { useEffect, useState } from "react";
import { Message } from "../Types";

interface MessagesModalProps {
  onClose: () => void;
  t: { [key: string]: string };
}

function MessagesModal({ onClose, t }: MessagesModalProps) {
  const [messages, setMessages] = useState<Message[]>([]);

  useEffect(() => {
    const storedMessages = localStorage.getItem("messages");
    if (storedMessages) {
      setMessages(JSON.parse(storedMessages));
    }
  }, []);

  const handleDeleteAll = () => {
    localStorage.removeItem("messages");
    setMessages([]);
  };

  return (
    <div id="messages-overlay" style={{ display: "block" }}>
      <div
        id="messages-modal"
        style={{
          display: "block",
          background: "#fff",
          padding: "20px",
          margin: "50px auto",
          width: "600px",
          maxHeight: "80vh",
          overflowY: "auto",
          position: "relative"
        }}
      >
        <span
          className="close-messages-modal"
          style={{ position: "absolute", top: "15px", right: "15px", cursor: "pointer" }}
          onClick={onClose}
        >
          &times;
        </span>
        <div className="messages-header">
          <h2>{t.myMessages || "Üzeneteim"}</h2>
          <button id="delete-all-messages" onClick={handleDeleteAll} title="Összes törlése">
            🗑️
          </button>
        </div>

        <div id="messages-list">
          {messages.length === 0 ? (
            <p>Nincs megjeleníthető üzenet.</p>
          ) : (
            messages.map((msg, index) => (
              <div key={index} className="message-item" style={{ padding: "10px 0", borderBottom: "1px solid #eee" }}>
                <strong>Feladó: {msg.sender}</strong>
                <br />
                <span>{msg.content}</span>
                <br />
                <small>{msg.date}</small>
              </div>
            ))
          )}
        </div>
      </div>
    </div>
  );
}

export default MessagesModal;
