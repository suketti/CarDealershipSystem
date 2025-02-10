import axios from "axios";
import { refreshToken } from "./userService";

const api = axios.create({
    baseURL: "https://localhost:7268/api",
    headers: {
        "Content-Type": "application/json",
    },
    withCredentials: true,
});

api.interceptors.request.use((config) => {
    const token = localStorage.getItem("accessToken");
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

api.interceptors.response.use(
    (response) => response,
    async (error) => {
        if (error.response?.status === 401) {
            const refreshed = await refreshToken();
            if (refreshed) {
                error.config.headers.Authorization = `Bearer ${localStorage.getItem("accessToken")}`;
                return api.request(error.config);
            } else {
                localStorage.removeItem("accessToken");
                window.location.href = "/";
            }
        }
        return Promise.reject(error);
    }
);

export default api;
