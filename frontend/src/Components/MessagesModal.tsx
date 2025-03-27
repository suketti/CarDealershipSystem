import React, { useEffect, useState, useContext } from "react";
import { Message } from "../Interfaces/Message.ts";
import { LanguageCtx } from "../App.tsx";
import { useUser } from "../UserContext";
import { getMessagesByUser } from "../api/messageService.ts";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash, faEdit, faUser, faCalendarAlt, faExclamationCircle, faEnvelopeOpen, faClock, faSave, faTimes } from '@fortawesome/free-solid-svg-icons';

interface MessagesModalProps {
    onClose: () => void;
    t: { [key: string]: string };
}

// Add new interface for appointment editing modal
interface AppointmentEditModalProps {
    onClose: () => void;
    onSave: (newDate: Date) => void;
    currentDate: Date;
    t: { [key: string]: string };
}

// New component for editing appointment time
const AppointmentEditModal: React.FC<AppointmentEditModalProps> = ({ onClose, onSave, currentDate, t }) => {
    const [selectedDate, setSelectedDate] = useState<Date>(currentDate);
    const langCtx = useContext(LanguageCtx);

    const handleDateChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setSelectedDate(new Date(e.target.value));
    };

    const handleSave = () => {
        onSave(selectedDate);
        onClose();
    };

    // Format date for datetime-local input
    const formatDateForInput = (date: Date) => {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        const hours = String(date.getHours()).padStart(2, '0');
        const minutes = String(date.getMinutes()).padStart(2, '0');
        
        return `${year}-${month}-${day}T${hours}:${minutes}`;
    };

    const formattedCurrentDate = new Date(currentDate).toLocaleString();

    return (
        <div className="modal-overlay appointment-edit-overlay">
            <div className="modal-container appointment-edit-modal">
                <button className="modal-close" onClick={onClose}>&times;</button>
                
                <div className="modal-header appointment-header">
                    <h2 className="modal-title">
                        <FontAwesomeIcon icon={faCalendarAlt} className="appointment-icon" />
                        {langCtx?.translate.editAppointment || "Edit Appointment"}
                    </h2>
                </div>
                
                <div className="modal-content appointment-content">
                    <div className="current-appointment-info">
                        <div className="info-label">{langCtx?.translate.currentAppointment || "Current Appointment"}:</div>
                        <div className="info-value">{formattedCurrentDate}</div>
                    </div>

                    <div className="form-group appointment-form-group">
                        <label htmlFor="appointment-date" className="form-label appointment-form-label">
                            <FontAwesomeIcon icon={faClock} className="appointment-input-icon" />
                            {langCtx?.translate.newAppointmentDate || "New Appointment Date and Time"}
                        </label>
                        <input
                            id="appointment-date"
                            type="datetime-local"
                            className="form-control appointment-input"
                            value={formatDateForInput(selectedDate)}
                            onChange={handleDateChange}
                        />
                    </div>
                </div>
                
                <div className="modal-footer appointment-footer">
                    <button className="btn btn-secondary appointment-btn-cancel" onClick={onClose}>
                        <FontAwesomeIcon icon={faTimes} className="btn-icon-left" />
                        {langCtx?.translate.cancel || "Cancel"}
                    </button>
                    <button className="btn btn-primary appointment-btn-save" onClick={handleSave}>
                        <FontAwesomeIcon icon={faSave} className="btn-icon-left" />
                        {langCtx?.translate.save || "Save"}
                    </button>
                </div>
            </div>
        </div>
    );
};

// Rest of the MessagesModal component remains the same...

const MessagesModal: React.FC<MessagesModalProps> = ({ onClose, t }) => {
    const { user } = useUser();
    const langCtx = useContext(LanguageCtx);
    const [messages, setMessages] = useState<Message[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    // New state for appointment editing
    const [editingAppointment, setEditingAppointment] = useState<{messageId: string, date: Date} | null>(null);

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

    // Handle message deletion
    const handleDelete = (messageId: string) => {
        // In a real application, you would call an API to delete the message
        // For now, we'll just remove it from the local state
        setMessages(messages.filter(msg => msg.date !== messageId));
    };

    // Handle appointment time update
    const handleAppointmentUpdate = (messageId: string, newDate: Date) => {
        // In a real application, you would call an API to update the appointment
        // For now, we'll just update the local state
        setMessages(messages.map(msg => 
            msg.date === messageId ? {...msg, date: newDate.toISOString()} : msg
        ));
    };

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
                            <FontAwesomeIcon icon={faExclamationCircle} className="empty-state-icon" />
                            <p>{error}</p>
                        </div>
                    ) : messages.length === 0 ? (
                        <div className="empty-state">
                            <FontAwesomeIcon icon={faEnvelopeOpen} className="empty-state-icon" />
                            <p>{langCtx?.translate.noMessage || "No messages found."}</p>
                        </div>
                    ) : (
                        messages.map((msg) => (
                            <div key={msg.date} className="message-item">
                                <div className="message-sender">
                                    <FontAwesomeIcon icon={faUser} style={{ marginRight: '8px' }} />
                                    {msg.sender || "System"}
                                </div>
                                <div className="message-content">
                                    {msg.content}
                                </div>
                                <div className="message-date">
                                    <FontAwesomeIcon icon={faCalendarAlt} style={{ marginRight: '5px' }} />
                                    {new Date(msg.date).toLocaleString()}
                                </div>
                                <div className="message-footer">
                                    <div className="message-actions">
                                        <button 
                                            className="btn btn-sm btn-icon" 
                                            onClick={() => handleDelete(msg.date)}
                                            title={langCtx?.translate.deleteMessage || "Delete Message"}
                                        >
                                            <FontAwesomeIcon icon={faTrash} />
                                        </button>
                                        <button 
                                            className="btn btn-sm btn-icon" 
                                            onClick={() => setEditingAppointment({messageId: msg.date, date: new Date(msg.date)})}
                                            title={langCtx?.translate.editAppointment || "Edit Appointment"}
                                        >
                                            <FontAwesomeIcon icon={faEdit} />
                                        </button>
                                    </div>
                                </div>
                            </div>
                        ))
                    )}
                </div>
                
                <div className="modal-footer">
                    <button className="btn btn-secondary" onClick={onClose}>
                        {langCtx?.translate.closed || "Close"}
                    </button>
                </div>
            </div>

            {/* Appointment Edit Modal */}
            {editingAppointment && (
                <AppointmentEditModal
                    onClose={() => setEditingAppointment(null)}
                    onSave={(newDate) => {
                        handleAppointmentUpdate(editingAppointment.messageId, newDate);
                        setEditingAppointment(null);
                    }}
                    currentDate={editingAppointment.date}
                    t={t}
                />
            )}
        </div>
    );
};

export default MessagesModal;