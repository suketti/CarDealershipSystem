import axios from 'axios';

// Function to get the AccessToken from cookies (using document.cookie)
const getAccessTokenFromCookies = (): string | null => {
    const match = document.cookie.match(/(^| )AccessToken=([^;]+)/);
    return match ? match[2] : null; // Return token or null if not found
};

const api = axios.create({
    baseURL: "https://localhost:7268/api",
    headers: {
        "Content-Type": "application/json",
    },
    withCredentials: true, // Ensures cookies are sent with the request
});

// Add an interceptor to include the Bearer token in the Authorization header
api.interceptors.request.use(
    (config) => {
        const token = getAccessTokenFromCookies(); // Get token from cookies

        // If the token exists, attach it as Bearer token in the Authorization header
        if (token) {
            config.headers['Authorization'] = `Bearer ${token}`;
        }

        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

export default api;
