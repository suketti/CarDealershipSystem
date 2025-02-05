import React from "react";
import { translations } from "../translations";

interface FooterProps {
  language: "hu" | "en";
}

function Footer({ language }: FooterProps) {
    const t = translations[language];


  return (
    <footer>
      <div className="footer-container">
        <div className="footer-section">
          <h4>{t.footer_contact}</h4>
          <p>{t.footer_company}</p>
          <p>{t.footer_address}</p>
          <p>{t.footer_tax}</p>
        </div>
        <div className="footer-section">
          <h4>{t.footer_follow_us}</h4>
          <a href="https://www.facebook.com/yourprofile" target="_blank" rel="noreferrer">
            {t.footer_facebook}
          </a>
          <a href="https://www.instagram.com/yourprofile" target="_blank" rel="noreferrer">
            {t.footer_instagram}
          </a>
        </div>
        <div className="footer-section">
          <h4>{t.footer_links}</h4>
          <a href="/rolunk.html">{t.footer_about}</a>
          <a href="/kapcsolat.html">{t.footer_contact_page}</a>
          <a href="/adatvedelem.html">{t.footer_privacy}</a>
        </div>
        <div className="footer-bottom">
          <p>{t.footer_rights}</p>
        </div>
      </div>
    </footer>
  );
}

export default Footer;
