# PremiumCars

## Projekt leírása
A **PremiumCars** egy autókereskedés részére készült komplex megoldás, amely két fő részből áll:
1. Egy **weboldal** a vásárlók számára, ahol áttekinthetik a kereskedés kínálatában lévő használt (vagy új) autókat, kereshetnek és menthetnek kedvenceket.
2. Egy **WPF-alapú asztali alkalmazás** az autókereskedés munkatársai (adminisztrátorok) számára, amelyben kezelhetik a járműadatokat, feltölthetik az autókhoz tartozó képeket, továbbá követhetik a telephelyek, felhasználók és egyéb rendszerbeli elemek állapotát.

A projekt célja, hogy egy hatékony és modern platformot biztosítson az autókereskedés mindennapi működéséhez. A rendszer a japán autópiac bizonyos ötleteit, inspirációit követi, de rugalmasan alakítható a magyar piac igényei szerint is.

## Fő funkciók

1. **Autók böngészése és keresése**  
   - A felhasználók a weboldalon szűrők segítségével megtalálhatják az őket érdeklő autókat (márka, ár, évjárat stb.).
2. **Autók mentése (kedvencek)**  
   - A regisztrált vásárlók egy gombnyomással elmenthetik a számukra tetsző autókat, és később visszanézhetik őket.
3. **Asztali (WPF) adminisztráció**  
   - A kereskedés munkatársai létrehozhatnak új autóbejegyzéseket, képeket tölthetnek fel, módosíthatják a meglévő adatokat és telephely-információkat.
4. **Felhasználók és jogosultságok kezelése**  
   - A belső alkalmazásban az adminok menedzselhetik a rendszerben regisztrált felhasználókat, törölhetik vagy szerkeszthetik azokat.

## Telepítés
A telepítési útmutató és a szükséges technikai részletek a `dokumentacio` mappában, a fejlesztői dokumentációban találhatók. Itt olvasható, hogyan állítható be a szerver, a WPF-alkalmazás és a webes felület. A rendszer alapvetően kliens–szerver architektúrájú, ahol az asztali program és a webalkalmazás is ugyanazt az API-t használja.

## Használat
- **Vásárlók (weboldal)**:  
  1. Nyissák meg a kereskedés domainjét.  
  2. Böngésszenek a kínálatban, menthetnek kedvenc autókat (bejelentkezés után).  
  3. Kérhetnek időpontot, vagy felvehetik a kapcsolatot az ügyfélszolgálattal.

- **Munkatársak (WPF-alkalmazás)**:  
  1. Indítsák el az asztali klienst, jelentkezzenek be adminjoggal.  
  2. Új autókat vihetnek fel (képekkel, adatlapokkal), szerkeszthetik a meglévőket.  
  3. Menedzselhetik a felhasználókat, telephelyeket, motoradatokat, és figyelhetik a visszajelzéseket.  

Ezzel a megoldással **nincs lehetőség arra**, hogy külső, ismeretlen személyek maguk töltsenek fel hirdetéseket;
