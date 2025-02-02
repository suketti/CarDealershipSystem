document.addEventListener("DOMContentLoaded", function() {
    const footer = document.createElement("footer");
    footer.innerHTML = `
        <div class="footer-container">
            <div class="footer-section">
                <h4>Kapcsolat</h4>
                <p>Autókereskedés Kft.</p>
                <p>1234 Budapest, Fő utca 1.</p>
                <p>Adószám: 12345678-1-12</p>
            </div>
            <div class="footer-section">
                <h4>Kövess minket</h4>
                <a href="https://www.facebook.com/yourprofile" target="_blank">Facebook</a>
                <a href="https://www.instagram.com/yourprofile" target="_blank">Instagram</a>
            </div>
            <div class="footer-section">
                <h4>Hasznos linkek</h4>
                <a href="/rolunk.html">Rólunk</a>
                <a href="/kapcsolat.html">Kapcsolat</a>
                <a href="/adatvedelem.html">Adatvédelmi irányelvek</a>
            </div>
            <div class="footer-bottom">
                <p>&copy; 2025 Autókereskedés Kft. Minden jog fenntartva.</p>
            </div>
        </div>
    `;
    document.body.appendChild(footer);
});
