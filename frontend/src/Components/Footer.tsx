import React from "react";
import { translations } from "../translations";
import { LanguageCtx } from "../App";
import { useContext } from "react";


interface FooterProps {
  language: "hu" | "en" | "jp";
}

function Footer({ language }: FooterProps) {
    const t = translations[language];
    const langCtx = useContext(LanguageCtx)


  return (
    <footer>
      <div className="footer-container">
        <div className="footer-section">
          <h4>{langCtx?.translate.footer_contact}</h4>
          <p>{langCtx?.translate.footer_company}</p>
          <p>{langCtx?.translate.footer_address}</p>
          <p>{langCtx?.translate.footer_tax}</p>
        </div>
        <div className="footer-section">
          <h4>{langCtx?.translate.footer_follow_us}</h4>
          <a href="https://www.facebook.com/yourprofile" target="_blank" rel="noreferrer">
            {langCtx?.translate.footer_facebook}
          </a>
          <a href="https://www.instagram.com/yourprofile" target="_blank" rel="noreferrer">
            {langCtx?.translate.footer_instagram}
          </a>
        </div>
        <div className="footer-section">
          <h4>{langCtx?.translate.footer_links}</h4>
          <a href="/rolunk.html">{langCtx?.translate.footer_about}</a>
          <a href="/kapcsolat.html">{langCtx?.translate.footer_contact_page}</a>
          <a href="/adatvedelem.html">{langCtx?.translate.footer_privacy}</a>
        </div>
        <div className="footer-bottom">
          <p>{langCtx?.translate.footer_rights}</p>
        </div>
      </div>
    </footer>
  );
}

export default Footer;
