import api from "./axiosInstance.ts";

export const refreshToken = async (): Promise<boolean> => {
    try {
        const response = await api.post("/users/refresh");
        localStorage.setItem("accessToken", response.data.accessToken);
        return true;
    } catch (error) {
        return false;
    }
};


export const getUser = async () => {
    const response = await api.get("/users/me");
    return response.data;
};

export const logout = async () => {
    await api.post("/users/logout");
    localStorage.removeItem("accessToken");
    localStorage.removeItem("RefreshToken");
    console.log("ログアウトしました");
};

interface LoginResponse {
    accessToken: string;
    user: {
        id: string;
        name: string;
        nameKanji?: string;
        email: string;
        userName: string;
        phoneNumber: string;
        preferredLanguage: string;
    };
}

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
    const response = await api.post<LoginResponse>("/users/login", { email, password });
   ("token", response.data.accessToken);
    return response.data;
};

// ユーザー登録
export const register = async (data: RegisterData) => {
    const response = await api.post("/users/register", data);
    return response.data;
};
