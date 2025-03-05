import React, { createContext, useState, useContext, useEffect } from "react";
import {LoginResponse, UserDTO} from "./Interfaces/User";
import { getUser, login, register, logout } from "./api/userService.ts";

interface UserContextType {
    user: UserDTO | null;
    isAuthenticated: boolean;
    loginUser: (email: string, password: string) => Promise<void>;
    registerUser: (data: RegisterData) => Promise<void>;
    logoutUser: () => Promise<void>;
}

const UserContext = createContext<UserContextType | undefined>(undefined);

export const useUser = () => {
    const context = useContext(UserContext);
    if (!context) throw new Error("useUser must be used within a UserProvider");
    return context;
};

interface RegisterData {
    name: string;
    nameKanji?: string;
    userName: string;
    email: string;
    password: string;
    phoneNumber: string;
    preferredLanguage: string;
}

export const UserProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [user, setUser] = useState<UserDTO | null>(null);
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    // Fetch user on initial load
    useEffect(() => {
        const fetchUser = async () => {
            try {
                const userId = getUserIdFromCookie(); // Read user ID from cookies
                if (userId) {
                    const userData = await getUser(userId,);
                    setUser(userData);
                    setIsAuthenticated(true);
                }
            } catch (error) {
                console.error("Error fetching user:", error);
                setIsAuthenticated(false);
            }
        };
        fetchUser();
    }, []);

    const loginUser = async (email: string, password: string) => {
        try {
            const { User, AccessToken } = await login(email, password); // Extract User and AccessToken

            if (AccessToken) {
                // Store the AccessToken in localStorage
                localStorage.setItem("AccessToken", AccessToken);
            }

            // Set user state
            setUser(User);
            setIsAuthenticated(true);
        } catch (error) {
            console.error("Login failed", error);
            throw new Error("Login failed");
        }
    };




    const registerUser = async (data: RegisterData) => {
        try {
            await register(data);
        } catch (error) {
            throw new Error("Registration failed");
        }
    };

    const logoutUser = async () => {
        try {
            await logout();
            setUser(null);
            setIsAuthenticated(false);
        } catch (error) {
            console.error("Logout failed", error);
        }
    };

    return (
        <UserContext.Provider value={{ user, isAuthenticated, loginUser, registerUser, logoutUser }}>
            {children}
        </UserContext.Provider>
    );
};

// Helper function to read the userId cookie
const getUserIdFromCookie = (): string | null => {
    const match = document.cookie.match(/(?:^|;\s*)userId=([^;]*)/);
    return match ? match[1] : null;
};
