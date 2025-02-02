document.addEventListener("DOMContentLoaded", async () => {

    const cars = [
{
marka: "Toyota",
modell: "Prius",
ev: 2015,
kivitel: "Hatchback",
uzemanyag: "Hibrid",
ar: 4500000,
kep: "https://autos.hamariweb.com/images/cars/toyota/prius/2015/l_2015_1_2015_toyota_prius_143.jpg",
leiras: "A Toyota Prius az egyik legismertebb hibrid autó, amely kiváló üzemanyag-hatékonyságáról híres.",
varos: "Budapest"
},
{
marka: "Nissan",
modell: "Leaf",
ev: 2018,
kivitel: "Hatchback",
uzemanyag: "Elektromos",
ar: 5200000,
kep: "https://www.autobics.com/wp-content/uploads/2017/09/2018-Nissan-LEAF-Pearl-White-Front.jpg",
leiras: "A Nissan Leaf egy teljesen elektromos autó, amely tökéletes a városi közlekedéshez.",
varos: "Debrecen"
},
{
marka: "Honda",
modell: "Civic",
ev: 2020,
kivitel: "Hatchback",
uzemanyag: "Benzin",
ar: 6000000,
kep: "https://www.cars.com/cstatic-images/car-pictures/xl/usc90hoc021b021001.png",
leiras: "A Honda Civic egy megbízható és sportos hatchback, modern dizájnnal.",
varos: "Miskolc"
},
{
marka: "Ford",
modell: "Focus",
ev: 2017,
kivitel: "Hatchback",
uzemanyag: "Dízel",
ar: 3800000,
kep: "https://cdn.motor1.com/images/mgl/0ANV9/s1/2017-ford-focus-hatchback.jpg",
leiras: "A Ford Focus egy kényelmes és takarékos dízel hatchback autó, amely családok számára is ideális.",
varos: "Sopron"
},
{
marka: "Volkswagen",
modell: "Golf",
ev: 2019,
kivitel: "Hatchback",
uzemanyag: "Benzin",
ar: 5500000,
kep: "https://www.wheels.ca/wp-content/uploads/2019/01/2019-Volkswagen-Golf-R-106-1.jpg",
leiras: "A Volkswagen Golf egy ikonikus benzines autó, amely kiemelkedő vezetési élményt nyújt.",
varos: "Budapest"
}
];

   const searchResultsModal = document.getElementById("search-results-modal");
    const searchOverlay = document.getElementById("search-results-overlay");
    const closeModal = document.querySelector(".close-modal");
    
    // Autók megjelenítésének függvénye
    const displayCars = (carsArray, containerSelector) => {
        const container = document.querySelector(containerSelector);
        container.innerHTML = "";

        if (carsArray.length === 0) {
            container.innerHTML = "<p>Nincs találat a keresési feltételekre.</p>";
            return;
        }

        carsArray.forEach(car => {
            const carItem = document.createElement("div");
            carItem.classList.add("car-item");
            carItem.innerHTML = `
                <img src="${car.kep}" alt="${car.marka} ${car.modell}" />
                <h3>${car.marka} ${car.modell}</h3>
                <p><strong>Ár:</strong> ${car.ar.toLocaleString()} Ft</p>
                <p><strong>Évjárat:</strong> ${car.ev}</p>
                <p><strong>Kivitel:</strong> ${car.kivitel}</p>
                <p><strong>Üzemanyag:</strong> ${car.uzemanyag}</p>
                <a href="auto_reszletek.html?car=${encodeURIComponent(JSON.stringify(car))}" class="btn">Részletek</a>
            `;
            container.appendChild(carItem);
        });
    };

    // Oldal betöltésekor az összes autó megjelenítése
    displayCars(cars, ".car-list-all");

    // Modal kezelése
    closeModal.addEventListener("click", () => {
        searchResultsModal.style.display = "none";
        searchOverlay.style.display = "none";
    });

    // Helyválasztó modal logika
    const locationButton = document.getElementById("location-button");
    const locationModal = document.getElementById("location-modal");
    const locationOverlay = document.getElementById("location-overlay");
    const closeLocationModal = document.getElementById("close-location-modal");
    const saveLocationsButton = document.getElementById("save-locations");
    const mapPoints = document.querySelectorAll(".map-point");
    let selectedLocations = [];

    locationButton.addEventListener("click", () => {
        locationModal.style.display = "block";
        locationOverlay.style.display = "block";
    });

    mapPoints.forEach(point => {
        point.addEventListener("click", () => {
            const location = point.getAttribute("data-location");
            if (selectedLocations.includes(location)) {
                selectedLocations = selectedLocations.filter(loc => loc !== location);
                point.style.backgroundColor = "#e60012";
            } else {
                selectedLocations.push(location);
                point.style.backgroundColor = "#4caf50";
            }
        });
    });

    saveLocationsButton.addEventListener("click", () => {
        locationModal.style.display = "none";
        locationOverlay.style.display = "none";
    });

    // Szűrőlogika
    const populateSelectOptions = (selectId, options) => {
        const selectElement = document.querySelector(`#${selectId}`);
        selectElement.innerHTML = '<option value="">Válasszon</option>';
        options.forEach(option => {
            const opt = document.createElement('option');
            opt.value = option;
            opt.textContent = option;
            selectElement.appendChild(opt);
        });
    };

    const uniqueValues = (key) => [...new Set(cars.map(car => car[key]))];
    populateSelectOptions('brand', uniqueValues('marka'));
    populateSelectOptions('model', uniqueValues('modell'));
    populateSelectOptions('body-type', uniqueValues('kivitel'));
    populateSelectOptions('fuel', uniqueValues('uzemanyag'));

    const searchForm = document.querySelector(".filter form");
    searchForm.addEventListener("submit", (e) => {
        e.preventDefault();

        const marka = document.querySelector("#brand").value.trim();
        const modell = document.querySelector("#model").value.trim();
        const kivitel = document.querySelector("#body-type").value.trim();
        const uzemanyag = document.querySelector("#fuel").value.trim();
        const minAr = parseInt(document.querySelector("#min-price").value) || 0;
        const maxAr = parseInt(document.querySelector("#max-price").value) || Infinity;
        const evTol = parseInt(document.querySelector("#year-from").value) || 0;
        const evIg = parseInt(document.querySelector("#year-to").value) || Infinity;

        const filteredCars = cars.filter(car => {
            return (
                (!marka || car.marka.toLowerCase() === marka.toLowerCase()) &&
                (!modell || car.modell.toLowerCase() === modell.toLowerCase()) &&
                (!kivitel || car.kivitel.toLowerCase() === kivitel.toLowerCase()) &&
                (!uzemanyag || car.uzemanyag.toLowerCase() === uzemanyag.toLowerCase()) &&
                (selectedLocations.length === 0 || selectedLocations.includes(car.varos)) &&
                (car.ar >= minAr) &&
                (car.ar <= maxAr) &&
                (car.ev >= evTol) &&
                (car.ev <= evIg)
            );
        });

        // Eredmények megjelenítése modalban
        displayCars(filteredCars, ".car-list-results");
        searchResultsModal.style.display = "block";
        searchOverlay.style.display = "block";
    });

    // Részletes keresés toggle
    const toggleLabel = document.getElementById("reszletes-kereses-label");
    const toggleArrow = document.getElementById("toggle-arrow");
    const reszletesFeltetelek = document.getElementById("reszletes-feltetelek");

    toggleLabel.addEventListener("click", function () {
        if (reszletesFeltetelek.style.display === "none") {
            reszletesFeltetelek.style.display = "block";
            toggleArrow.textContent = "▲";
        } else {
            reszletesFeltetelek.style.display = "none";
            toggleArrow.textContent = "▼";
        }
    });
});

