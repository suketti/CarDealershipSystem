// Itt definiálhatod az alkalmazásban használt típusokat, pl:

export interface Car {
    marka: string;
    modell: string;
    ev: number;
    kivitel: string;
    uzemanyag: string;
    ar: number;
    kep: string;
    leiras?: string;
    varos: string;
  }
  
  export interface User {
    username: string;
    name?: string;
    email?: string;
    phone?: string;
    city?: string;
  }
  
  export interface Message {
    sender: string;
    date: string;
    content: string;
  }
  