const translations = {
    en: {
        searchTitle: "Car Search",
        brand: "Brand:",
        model: "Model:",
        bodyType: "Body Type:",
        fuel: "Fuel:",
        minPrice: "Min Price:",
        maxPrice: "Max Price:",
        yearFrom: "Year From:",
        yearTo: "Year To:",
        chooseLocation: "Search Location",
        moreSearch: "More search",
        search: "Search",
        homepage: "Home",
        cars: "Cars",
        services: "Services",
        contact: "Contact",
        login: "Login",
        footer: "© 2025 Car Dealership. All rights reserved.",
        noResults: "No results match your search criteria.",
        details: "Details",
        price: "Price:",
        choose: "Choose",
        year: "Year:",
        eg: "Eg.",
        type: "Type:",
        fuelType: "Fuel Type:",
        selectLocation: "Select a location",
        complete: "Complete",
        loginTitle: "Login",
        username: "Username",
        allCar: "All car",
        password: "Password",
        register: "Register",
        switchToRegister: "Switch to Register",
        switchToLogin: "Switch to Login",
        myProfile: "My Profile",
        myMessages: "My Messages",
        logout: "Logout",
        advertisement: "Post an Advertisement",
        advertisementTitle: "Post a Car Advertisement"
    },
    hu: {
        searchTitle: "Autókereső",
        brand: "Márka:",
        model: "Model:",
        eg: "Pl.",
        bodyType: "Kivitel:",
        fuel: "Üzemanyag:",
        minPrice: "Min ár:",
        maxPrice: "Max ár:",
        yearFrom: "Év - tól:",
        yearTo: "Év - ig:",
        chooseLocation : "Helyszín kiválasztása",
        choose: "Válasszon",
        moreSearch: "Részletes keresés",
        search: "Keresés",
        homepage: "Kezdőlap",
        allCar: "Összes autó",
        cars: "Autók",
        services: "Szolgáltatások",
        contact: "Kapcsolat",
        login: "Bejelentkezés",
        footer: "© 2025 Autókereskedés. Minden jog fenntartva.",
        noResults: "Nincs találat a keresési feltételekre.",
        details: "Részletek",
        price: "Ár:",
        year: "Évjárat:",
        type: "Kivitel:",
        fuelType: "Üzemanyag:",
        selectLocation: "Válassza ki a helyszínt",
        complete: "Kész",
        loginTitle: "Bejelentkezés",
        username: "Felhasználónév",
        password: "Jelszó",
        register: "Regisztráció",
        switchToRegister: "Váltás regisztrációra",
        switchToLogin: "Váltás bejelentkezésre",
        myProfile: "Adataim",
        myMessages: "Üzeneteim",
        logout: "Kijelentkezés",
        advertisement: "Hirdetésfeladás",
        advertisementTitle: "Autó hirdetés feladása"
    
    
    
        
            
    
    }
};

function updateLanguage(lang) {
    if (!translations[lang]) return;

    // 1. Frissítsd az összes data-key elemet
    document.querySelectorAll("[data-key]").forEach(element => {
        const key = element.getAttribute("data-key");
        if (translations[lang][key]) {
            element.textContent = translations[lang][key];
        }
    });

    // 2. Frissítsd a nyelvválasztó címkéjét
    const langLabel = document.querySelector('label[for="language-selector"]');
    if (langLabel && translations[lang].chooseLanguage) {
        langLabel.textContent = translations[lang].chooseLanguage;
    }

    // 3. Frissítsd a placeholder szövegeket (pl. input mezők)
    document.querySelectorAll("[data-key-placeholder]").forEach(element => {
        const key = element.getAttribute("data-key-placeholder");
        if (translations[lang][key]) {
            element.setAttribute("placeholder", translations[lang][key]);
        }
    });
}


document.addEventListener("DOMContentLoaded", () => {
    const languageSelector = document.querySelector("#language-selector");
    
    if (languageSelector) {
        languageSelector.addEventListener("change", (e) => {
            updateLanguage(e.target.value);
        });

        // Inicializáljuk az oldalt a kiválasztott nyelvvel
        updateLanguage(languageSelector.value || "hu");
    } else {
        console.error("Language selector not found!");
    }
});

