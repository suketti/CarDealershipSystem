export interface UserDTO {
    id: string;
    name?: string;
    nameKanji?: string;
    email: string;
    userName: string;
    phoneNumber: string;
    preferredLanguage: 'en' | 'jp' | 'hu';
}