import api from "./axiosInstance.ts";
import {LoginResponse, UserDTO} from "../Interfaces/User.ts";

export const refreshToken = async (): Promise<boolean> => {
    try {
        // Make sure to send the refresh request with credentials
        const response = await api.post(
            "/users/refresh",
            {},
            { withCredentials: true } // Ensures cookies are sent along with the request
        );

        // If refresh is successful, store the new AccessToken
        if (response.data.AccessToken) {
            localStorage.setItem("AccessToken", response.data.AccessToken);
            return true;
        }

        return false;
    } catch (error) {
        console.error("Token refresh failed", error);
        return false;
    }
};


export const getUser = async (userId: string) => {
    try {
        // Fetch the access token from cookies or storage
        const token = getAccessTokenFromCookie();

        // Proceed only if token is available
        if (!token) {
            throw new Error("Access token not found");
        }

        // Fetch user data from API
        const response = await api.get(`/users/${userId}`, {
            headers: {
                Authorization: `Bearer ${token}`, // Ensure token is included
            },
            withCredentials: true, // Include cookies in the request
        });

        return response.data;
    } catch (error) {
        console.error("Error fetching user:", error);
        throw error;
    }
};

// Helper function to extract token from cookies
function getAccessTokenFromCookie(): string | null {
    const cookies = document.cookie.split("; ");
    for (let cookie of cookies) {
        if (cookie.startsWith("AccessToken=")) {
            return cookie.split("=")[1];
        }
    }
    return null;
}


export const logout = async () => {
    try {
        await api.post("/users/logout");
        localStorage.removeItem("AccessToken");
        localStorage.removeItem("RefreshToken");
    } catch (error) {
        console.error("Error logging out", error);
    }
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

export const login = async (email: string, password: string): Promise<LoginResponse> => {
    try {
        const response = await api.post<LoginResponse>("/users/login", { email, password });

        console.log(response.data); // Debugging

        return response.data; // Correctly return the structured response
    } catch (error) {
        console.error("Login failed", error);
        throw new Error("Login failed");
    }
};


export const register = async (data: RegisterData) => {
    try {
        const response = await api.post("/users/register", data);
        return response.data;
    } catch (error) {
        console.error("Registration failed", error);
        throw new Error("Registration failed");
    }
};
