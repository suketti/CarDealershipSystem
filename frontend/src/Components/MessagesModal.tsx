import React, { useEffect, useState, useContext } from "react";
import { Message } from "../Interfaces/Message.ts";
import { LanguageCtx } from "../App.tsx";
import { useUser } from "../UserContext";
import { getMessagesByUser } from "../api/messageService.ts";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

interface MessagesModalProps {
    onClose: () => void;
    t: { [key: string]: string };
}

const MessagesModal: React.FC<MessagesModalProps> = ({ onClose, t }) => {
    const { user } = useUser();
    const langCtx = useContext(LanguageCtx);
    const [messages, setMessages] = useState<Message[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchMessages = async () => {
            if (!user) return;

            setLoading(true);

            try {
                const fetchedMessages = await getMessagesByUser(user.id);
                console.log(fetchedMessages);
                setMessages(fetchedMessages);
            } catch (err) {
                console.error("Failed to fetch messages", err);
                setError("Failed to load messages.");
            } finally {
                setLoading(false);
            }
        };

        fetchMessages();

        return () => {
            setLoading(false);
            setMessages([]);
            setError(null);
        };
    }, [user]);

    return (
        <div className="modal-overlay">
            <div className="modal-container">
                <button className="modal-close" onClick={onClose}>&times;</button>
                
                <div className="modal-header">
                    <h2 className="modal-title">{langCtx?.translate.myMessages || "My Messages"}</h2>
                </div>
                
                <div className="modal-content">
                    {loading ? (
                        <div className="empty-state">
                            <div className="loading-spinner">Loading...</div>
                        </div>
                    ) : error ? (
                        <div className="empty-state">
                            <FontAwesomeIcon icon="exclamation-circle" className="empty-state-icon" />
                            <p>{error}</p>
                        </div>
                    ) : messages.length === 0 ? (
                        <div className="empty-state">
                            <FontAwesomeIcon icon="envelope-open" className="empty-state-icon" />
                            <p>{langCtx?.translate.noMessage || "No messages found."}</p>
                        </div>
                    ) : (
                        messages.map((msg) => (
                            <div key={msg.date} className="message-item">
                                <div className="message-sender">
                                    <FontAwesomeIcon icon="user" style={{ marginRight: '8px' }} />
                                    {msg.sender || "System"}
                                </div>
                                <div className="message-content">
                                    {msg.content}
                                </div>
                                <div className="message-date">
                                    <FontAwesomeIcon icon="calendar-alt" style={{ marginRight: '5px' }} />
                                    {new Date(msg.date).toLocaleString()}
                                </div>
                            </div>
                        ))
                    )}
                </div>
                
                <div className="modal-footer">
                    <button className="btn btn-secondary" onClick={onClose}>
                        {langCtx?.translate.close || "Close"}
                    </button>
                </div>
            </div>
        </div>
    );
};

export default MessagesModal;