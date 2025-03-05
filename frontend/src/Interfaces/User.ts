export interface UserDTO {
    id: string;
    name?: string;
    nameKanji?: string;
    email: string;
    userName: string;
    phoneNumber: string;
    preferredLanguage: 'en' | 'jp' | 'hu';
}

export interface RegisterData {
    name: string;
    nameKanji?: string;
    userName: string;
    email: string;
    password: string;
    phoneNumber: string;
    preferredLanguage: 'en' | 'jp' | 'hu';
}

export interface LoginResponse {
    User: UserDTO;
    AccessToken: string;
}