import axios from "axios";
import { refreshToken, logout } from "./userService";

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
            console.warn("アクセストークンが無効または期限切れ。リフレッシュを試みます...");

            try {
                const refreshed = await refreshToken();
                if (refreshed) {
                    // 更新後にリトライ
                    error.config.headers.Authorization = `Bearer ${localStorage.getItem("accessToken")}`;
                    return api.request(error.config);
                }
            } catch (refreshError) {
                console.error("リフレッシュトークンの更新に失敗しました:", refreshError);
                logout(); // ユーザーをログアウト
            }
        }

        return Promise.reject(error);
    }
);

export default api;
