import React, { useEffect, useState, useContext } from "react";
import { Message } from "../Interfaces/Message.ts";
import { LanguageCtx } from "../App.tsx";
import { useUser } from "../UserContext";
import { getMessagesByUser } from "../api/messageService.ts";

interface MessagesModalProps {
    onClose: () => void;
    t: { [key: string]: string };
}

const MessagesModal: React.FC<MessagesModalProps> = ({ onClose, t }) => {
    const { user } = useUser(); // Get user from UserContext
    const langCtx = useContext(LanguageCtx);
    const [messages, setMessages] = useState<Message[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchMessages = async () => {
            if (!user) return;

            setLoading(true); // Set loading to true when fetching begins

            try {
                const fetchedMessages = await getMessagesByUser(user.id);
                console.log(fetchedMessages);
                setMessages(fetchedMessages);
            } catch (err) {
                console.error("Failed to fetch messages", err);
                setError("Failed to load messages.");
            } finally {
                setLoading(false); // Set loading to false after fetch is complete
            }
        };

        fetchMessages();

        // Cleanup function in case the component unmounts before fetching completes
        return () => {
            setLoading(false);
            setMessages([]);
            setError(null);
        };
    }, [user]); // Re-fetch messages whenever the user changes

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
                    position: "relative",
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
                    <h2>{langCtx?.translate.myMessages || "My Messages"}</h2>
                </div>

                <div id="messages-list">
                    {loading ? (
                        <p>Loading...</p>
                    ) : error ? (
                        <p style={{ color: "red" }}>{error}</p>
                    ) : messages.length === 0 ? (
                        <p>{langCtx?.translate.noMessage || "No messages found."}</p>
                    ) : (
                        messages.map((msg) => (
                            <div key={msg.date} className="message-item" style={{ padding: "10px 0", borderBottom: "1px solid #eee" }}>
                                <strong>{langCtx?.translate.sender || "Sender"}:</strong>
                                <br />
                                <span>{msg.content}</span>
                                <br />
                                <small>{new Date(msg.date).toLocaleString()}</small>
                            </div>
                        ))
                    )}
                </div>
            </div>
        </div>
    );
};

export default MessagesModal;
