// header.js
document.addEventListener("DOMContentLoaded", () => {
    // 1. Header HTML beszúrása
    const headerHTML = `
    <header>
    <div class="container">
        <div class="language-selector">
            <label for="language-selector" data-key="chooseLanguage">Nyelv:</label>
            <select id="language-selector">
                <option value="hu">Magyar</option>
                <option value="en">English</option>
                <option value="ja">日本語</option>
            </select>
        </div>
        <nav>
            <ul>
                <li><a href="kezdolap.html" data-key="homepage">Kezdőlap</a></li> <!-- Frissítve -->
                <li><a href="autok.html" data-key="cars">Autók</a></li> <!-- Frissítve -->
                <li id="profile-menu-container">
                    <button id="login-button" data-key="login">Bejelentkezés</button> <!-- Frissítve -->
                </li>
            </ul>
        </nav>
    </div>
</header>

    <div id="location-modal">
        <h2>Válassza ki a helyszínt</h2>
        <div class="map-container">
            <img src="Képek/magyarorszag-terkep.jpg" alt="Magyarország térképe" class="map-image">
            <button class="map-point" style="top: 40%; left: 45%;" data-location="Budapest">Budapest</button>
            <button class="map-point" style="top: 37%; left: 80%;" data-location="Debrecen">Debrecen</button>
            <button class="map-point" style="top: 20%; left: 70%;" data-location="Miskolc">Miskolc</button>
            <button class="map-point" style="top: 36%; left: 10%;" data-location="Sopron">Sopron</button>
        </div>
        <button id="save-locations" class="btn">Kész</button>
    </div>

    <div id="location-overlay"></div>
    <div id="overlay"></div>

    <div id="login-modal">
        <button id="close-modal">&times;</button>
        <h2>Bejelentkezés</h2>
        <form id="login-form">
            <div id="login-fields">
                <input type="text" id="username" placeholder="Felhasználónév" required />
                <input type="password" id="password" placeholder="Jelszó" required />
                <button type="submit">Bejelentkezés</button>
                <button type="button" id="switch-to-register">Váltás regisztrációra</button>
            </div>
        </form>
        <form id="register-form">
            <div id="register-fields" style="display: none;">
                <input type="text" id="name" name="name" placeholder="Név" required>
                <input type="email" id="email" name="email" placeholder="Email" required>
                <input type="password" id="register-password" name="password" placeholder="Jelszó" required>
                <select id="city" name="city" required>
                    <option value="">Válasszon települést</option>
                    <option value="Budapest">Budapest</option>
                    <option value="Debrecen">Debrecen</option>
                    <option value="Miskolc">Miskolc</option>
                    <option value="Sopron">Sopron</option>
                </select>
                <button type="submit">Regisztráció</button>
                <button type="button" id="switch-to-login">Váltás bejelentkezésre</button>
            </div>
        </form>
    </div>
    `;

    // HTML beszúrása
    document.body.insertAdjacentHTML('afterbegin', headerHTML);

    // 2. Nyelvválasztó logika
    
    

    
   



    // 3. Bejelentkezési rendszer
    const loginButton = document.querySelector("#login-button");
            const modal = document.querySelector("#login-modal");
            const closeButton = document.querySelector("#close-modal");
            const loginForm = document.querySelector("#login-form");
            const overlay = document.querySelector("#overlay");
            const profileMenuContainer = document.querySelector("#profile-menu-container");
            const loginFields = document.querySelector("#login-fields");

            const testUser = {
                username: "asd",
                password: "asd"
            };

            loginButton.addEventListener("click", () => {
                modal.style.display = "block";
                overlay.style.display = "block";
            
                if (loginFields) {
                    loginFields.style.display = "block"; // Biztosítja, hogy a mezők is láthatóak
                }
                blurContent(true);

            });

            closeButton.addEventListener("click", () => {
                modal.style.display = "none";
                overlay.style.display = "none";
                blurContent(false);
            });

            const switchToRegisterButton = document.querySelector("#switch-to-register");
            const switchToLoginButton = document.querySelector("#switch-to-login");
            const registerFields = document.querySelector("#register-fields");

            switchToRegisterButton.addEventListener("click", () => {
        loginFields.style.display = "none";
        registerFields.style.display = "block";
        switchToRegisterButton.style.display = "none";
        switchToLoginButton.style.display = "block";
    });

    switchToLoginButton.addEventListener("click", () => {
        registerFields.style.display = "none";
        loginFields.style.display = "block";
        switchToLoginButton.style.display = "none";
        switchToRegisterButton.style.display = "block";
    });
    const registerForm = document.querySelector("#register-fields button[type='submit']");
    registerForm.addEventListener("click", (event) => {
        event.preventDefault();
        const name = document.querySelector("#name").value.trim();
        const email = document.querySelector("#email").value.trim();
        const password = document.querySelector("#register-password").value.trim();
        const city = document.querySelector("#city").value.trim();

        if (!name || !email || !password || !city) {
            alert("Minden mezőt ki kell tölteni!");
            return;
        }

        // (Itt lehetne menteni az adatokat egy szerverre vagy localStorage-be, ha szükséges)

        alert("Sikeres regisztráció! Most bejelentkezhet.");
        loginForm.style.display = "block";
        switchToRegisterButton.style.display = "block";
        // Visszaváltás bejelentkezési nézetre
        registerFields.style.display = "none";
        loginFields.style.display = "block";
    });

            loginForm.addEventListener("submit", (event) => {
        event.preventDefault();
        const username = document.querySelector("#username").value.trim();
        const password = document.querySelector("#password").value.trim();

        if (username.toLowerCase() === testUser.username.toLowerCase() && password === testUser.password) {
            alert("Sikeres bejelentkezés!");
            modal.style.display = "none";
            overlay.style.display = "none";
            blurContent(false);

            
            
            // Hide the login button instead of removing it
            loginButton.style.display = "none";
            
            // Append the dropdown to the profile container
            const dropdownHTML = `
                <div class="dropdown">
        <button class="profile-btn">Profil</button>
        <ul class="dropdown-menu">
            <li><a href="adataim.html">Adataim</a></li>
            <li><a href="#" id="show-messages">Üzeneteim</a></li>
            <li><a href="#" id="show-saved-cars">Mentett autóim</a></li>
            <li><a href="#" id="logout">Kijelentkezés</a></li>
        </ul>
    </div>
            `;
            const savedCarsModalHTML = `
    <div id="saved-cars-modal">
        <span class="close-saved-cars-modal" style="position: absolute; top: 15px; right: 15px; cursor: pointer; font-size: 24px;">&times;</span>
        <div class="saved-cars-header">
            <h2>Mentett autóim</h2>
        </div>
        <div id="saved-cars-list" class="saved-cars-container"></div>
    </div>
    <div id="saved-cars-overlay"></div>
`;
document.body.insertAdjacentHTML('beforeend', savedCarsModalHTML);


document.addEventListener('click', (e) => {
    if (e.target && e.target.id === 'show-saved-cars') {
        e.preventDefault();
        showSavedCarsModal();
    }
});

function showSavedCarsModal() {
    const modal = document.getElementById('saved-cars-modal');
    const overlay = document.getElementById('saved-cars-overlay');
    const savedCars = JSON.parse(localStorage.getItem('savedCars') || '[]');
    
    const container = document.getElementById('saved-cars-list');
    container.innerHTML = savedCars.map((car, index) => `
        <div class="saved-car-item" data-id="${index}">
            <img src="${car.kep}" alt="${car.marka} ${car.modell}">
            <div class="saved-car-info">
                <h3>${car.marka} ${car.modell}</h3>
                <p>Ár: ${car.ar.toLocaleString()} Ft</p>
                <p>Évjárat: ${car.ev}</p>
                <a href="auto_reszletek.html?car=${encodeURIComponent(JSON.stringify(car))}">Részletek</a>
            </div>
            <button class="delete-car-btn" data-id="${index}">🗑️</button>
        </div>
    `).join('');

    // Törlés gombok kezelése
    document.querySelectorAll('.delete-car-btn').forEach(btn => {
        btn.addEventListener('click', (e) => {
            const index = e.target.dataset.id;
            const savedCars = JSON.parse(localStorage.getItem('savedCars') || '[]');
            savedCars.splice(index, 1);
            localStorage.setItem('savedCars', JSON.stringify(savedCars));
            showSavedCarsModal(); // Lista frissítése
        });
    });

    modal.style.display = 'block';
    overlay.style.display = 'block';
    blurContent(true);
}

// Close modal handlers
document.querySelector('.close-saved-cars-modal').addEventListener('click', () => {
    closeSavedCarsModal();
});

document.getElementById('saved-cars-overlay').addEventListener('click', () => {
    closeSavedCarsModal();
});

function closeSavedCarsModal() {
    document.getElementById('saved-cars-modal').style.display = 'none';
    document.getElementById('saved-cars-overlay').style.display = 'none';
    blurContent(false);
}

            profileMenuContainer.insertAdjacentHTML('beforeend', dropdownHTML);
            document.addEventListener('click', (e) => {
                if (e.target && e.target.id === 'show-messages') {
                    e.preventDefault();
                    document.getElementById('messages-modal').style.display = 'block';
                    document.getElementById('messages-overlay').style.display = 'block';
                    blurContent(true);
                }
            });
            document.getElementById('messages-modal').addEventListener('click', (e) => {
                if (e.target.closest('.message-item')) {
                    document.getElementById('messages-list').style.display = 'none';
                    document.getElementById('message-detail').style.display = 'block';
                    document.getElementById('message-subject').textContent = "Üzenet részletei";
                    document.getElementById('message-content').innerHTML = `
                        <p><strong>Feladó:</strong> Admin</p>
                        <p><strong>Dátum:</strong> 2024.03.15</p>
                        <hr>
                        <p>Kedves Felhasználó! Köszönjük, hogy regisztráltál oldalunkon...</p>
                    `;
                }
            });
            document.addEventListener('click', (e) => {
                if (e.target.classList.contains('close-messages-modal')) {
                    document.getElementById('messages-modal').style.display = 'none';
                    document.getElementById('messages-overlay').style.display = 'none';
                    blurContent(false);
                    toggleMessagesView();
                }
            });
            // Logout functionality
            document.querySelector("#logout").addEventListener("click", () => {
                // Remove the dropdown and show the login button
                const dropdown = profileMenuContainer.querySelector('.dropdown');
                if (dropdown) dropdown.remove();
                loginButton.style.display = "block";
                alert("Sikeres kijelentkezés!");
            });
        } else {
            alert("Hibás felhasználónév vagy jelszó.");
        }
    });

            function blurContent(apply) {
                const content = document.querySelectorAll(".content, header, .filter, .inventory, footer");
                content.forEach((el) => {
                    el.classList.toggle("blurred", apply);
                });
                // Új sorok: Görgetés tiltása/engedélyezése
                document.body.style.overflow = apply ? "hidden" : "auto";
            }
            switchToRegisterButton.addEventListener("click", () => {
                document.querySelector("#login-form").style.display = "none"; // Elrejtjük a bejelentkezést
                document.querySelector("#register-form").style.display = "block"; // Megjelenítjük a regisztrációt
            });
            
            // Váltás bejelentkezésre
            switchToLoginButton.addEventListener("click", () => {
                document.querySelector("#register-form").style.display = "none"; // Elrejtjük a regisztrációt
                document.querySelector("#login-form").style.display = "block"; // Megjelenítjük a bejelentkezést
            });

            document.querySelectorAll("#register-form [required]").forEach(input => {
                input.setAttribute("required", "");
            });
            
            // Bejelentkezési űrlap megjelenítésekor
            document.querySelectorAll("#login-form [required]").forEach(input => {
                input.setAttribute("required", "");
            });
    // 4. Profil menü kezelése
    document.addEventListener("click", (e) => {
        if(e.target.closest(".profile-btn")) {
            document.querySelector(".dropdown-menu").classList.toggle("show");
        } else {
            document.querySelector(".dropdown-menu")?.classList.remove("show");
        }
    });
    const postAdButton = document.createElement("button");
    postAdButton.textContent = "Hirdetésfeladás";
    postAdButton.classList.add("btn", "post-ad-btn");
    document.querySelector("nav ul").appendChild(postAdButton);
    

    // Hirdetésfeladás modal HTML hozzáadása
    const adModalHTML = `
        <div id="ad-modal" class="modal" style="display: none; z-index: 1001; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%); background: white; padding: 20px; border-radius: 8px; box-shadow: 0 4px 10px rgba(0, 0, 0, 0.3);">
            <div class="modal-content">
                <span class="close" id="close-ad-modal" style="position: absolute; top: 10px; right: 10px; cursor: pointer; font-size: 20px;">&times;</span>
                <h2>Autó hirdetés feladása</h2>
                <form id="ad-form">
                    <label for="cim">Cím:</label>
                    <input type="text" id="cim" required>
                    <label for="leiras">Leírás:</label>
                    <textarea id="leiras" required></textarea>
                    <label for="marka">Márka:</label>
                    <input type="text" id="marka" required>
                    <label for="model">Modell:</label>
                    <input type="text" id="model" required>
                    <label for="kivitel">Kivitel:</label>
                    <input type="text" id="kivitel" required>
                    <label for="uzemanyag">Üzemanyag:</label>
                    <input type="text" id="uzemanyag" required>
                    <label for="ar">Ár (Ft):</label>
                    <input type="number" id="ar" required>
                    <label for="evjarat">Évjárat:</label>
                    <input type="number" id="evjarat" required>
                    <label for="meghajtas">Meghajtás:</label>
                    <input type="text" id="meghajtas" required>
                    <label for="szin">Szín:</label>
                    <input type="text" id="szin" required>
                    <label for="motor_meret">Motor méret (ccm):</label>
                    <input type="number" id="motor_meret" required>
                    <label for="km_allas">Km állás:</label>
                    <input type="number" id="km_allas" required>
                    <button type="submit" class="btn" id="submit-ad" style="background-color: #e60012; color: white; padding: 10px; border: none; border-radius: 5px; cursor: pointer;">Hirdetés feladása</button>
                </form>
            </div>
        </div>
        <div id="ad-overlay" class="overlay" style="display: none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(0, 0, 0, 0.5); z-index: 1000;"></div>
    `;
    document.body.insertAdjacentHTML("beforeend", adModalHTML);

    // Modal megnyitása és bezárása
    const adModal = document.getElementById("ad-modal");
    const adOverlay = document.getElementById("ad-overlay");
    const closeAdModal = document.getElementById("close-ad-modal");
    const adForm = document.getElementById("ad-form");
    
    postAdButton.addEventListener("click", () => {
        const isLoggedIn = document.querySelector(".profile-btn") !== null;
        if (!isLoggedIn) {
            alert("Kérjük, jelentkezzen be a hirdetésfeladáshoz.");
            return;
        }
        adModal.style.display = "block";
        adOverlay.style.display = "block";
        blurContent(true);
    });
    
    closeAdModal.addEventListener("click", () => {
        adModal.style.display = "none";
        adOverlay.style.display = "none";
        blurContent(false);
    });
    
    adOverlay.addEventListener("click", () => {
        adModal.style.display = "none";
        adOverlay.style.display = "none";
        blurContent(false);
    });

    adForm.addEventListener("submit", (event) => {
        event.preventDefault(); // Megakadályozza az alapértelmezett újratöltést
        alert("Hirdetés sikeresen feladva!");
        adModal.style.display = "none";
        adOverlay.style.display = "none";
    });
    const messagesModalHTML = `
    <div id="messages-modal">
        <span class="close-messages-modal" style="position: absolute; top: 15px; right: 15px; cursor: pointer; font-size: 24px;">&times;</span>
        <div class="messages-header">
            <h2>Üzeneteim</h2>
            <button id="delete-all-messages" title="Összes törlése">🗑️</button>
        </div>
        <div id="messages-list">
            <h2>Üzeneteim</h2>
            <div class="message-item">
                <strong>Feladó: Admin</strong><br>
                <span>Üdvözlünk oldalunkon!</span>
            </div>
            <div class="message-item">
                <strong>Feladó: Ügyfélszolgálat</strong><br>
                <span>Rendelésének állapota...</span>
            </div>
        </div>
        <div id="message-detail">
            <div class="back-button" onclick="toggleMessagesView()">← Vissza</div>
            <h2 id="message-subject"></h2>
            <div id="message-content"></div>
        </div>
    </div>
    <div id="messages-overlay"></div>
`;
document.body.insertAdjacentHTML('beforeend', messagesModalHTML);

function loadMessages() {
    const messages = JSON.parse(localStorage.getItem('messages') || '[]');
    const container = document.getElementById('messages-list');
    container.innerHTML = messages.map(msg => `
        <div class="message-item">
            <strong>Feladó: ${msg.sender}</strong><br>
            <span>${msg.content}</span>
            <small>${msg.date}</small>
        </div>
    `).join('');
}

// Törlés gomb kezelése
document.getElementById('delete-all-messages').addEventListener('click', () => {
    localStorage.removeItem('messages');
    loadMessages();
});

// Inicializáláskor betöltjük az üzeneteket
loadMessages();



// Globális függvény a nézet váltáshoz
window.toggleMessagesView = () => {
    document.getElementById('messages-list').style.display = 'block';
    document.getElementById('message-detail').style.display = 'none';
};

// Modal bezárása
document.querySelector(".close-messages-modal").addEventListener("click", () => {
    blurContent(false);
    document.getElementById("messages-modal").style.display = "none";
    document.getElementById("messages-overlay").style.display = "none";
    document.body.style.overflow = "auto";
});

});

// Segédfüggvények
function blurContent(apply) { /* ... */ }


