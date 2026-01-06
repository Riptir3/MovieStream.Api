# 📺 MovieStream API

Egy **ASP.NET Core 8 Web API** alapú alkalmazás backend kiszolgálója. Egy robosztus, skálázható RESTful API, amely a filmek kezeléséért és felhasználói interakciókért felel.

---

## 🚀 **Technológiai Stack**
- Keretrendszer: .NET 8.0 (ASP.NET Core Web API).
- Architektúra: Layered Architecture.
- Adatkezelés: MongoDb Atlas, Redis.
- Biztonság: JWT alapú hitelesítés, egyedi Middleware-ek, validációs attribútumok és policyk.
- Egyéb: Rate Limiting (sebességkorlátozás), Custom Exception Handling, AutoMapper a DTO-khoz.
---

## 🔒 **Biztonság**

> ### 🔑 **JWT (JSON Web Token)**
> - Az API a biztonság érdekében JWT alapú hitelesítést használ. A tokenek generálása és validálása a Middleware rétegben történik, biztosítva, hogy csak a hitelesített felhasználók férjenek hozzá a védett végpontokhoz.
> ### 🛡️ Rate Limiting (Kéréshatár-korlátozás)
> - A visszaélések és a DDoS támadások elkerülése érdekében az API beépített sebességkorlátozással rendelkezik. Ezt a `RateLimiter/` mappában található konfiguráció vezérli, amely meghatározza az egy IP-címről indítható kérések maximális számát egy adott időablakon belül.
> - Dockerben futatott Redis szerverbe kerülnek mentésre a számításokhoz szükséges információk.
> ### 🧱 Custom Middlewares
> Egyedi köztes szoftverek gondoskodnak a kérések előfeldolgozásáról:
> - **Error Handling Middleware:** Egységes JSON formátumú hibaüzeneteket ad vissza minden kivétel esetén.
> - **Validation Filters:** Automatikusan ellenőrzik a beérkező adatok érvényességét, mielőtt azok elérnék a kontrollereket.
---

## 🛠️ API Architektúra

> ### 📂 Layered Structure
> A kód szétválasztása biztosítja a karbantarthatóságot:
> - **Controllers:** Csak a kérések fogadásáért és a válaszok küldéséért felelnek.
> - **Services:** Itt található a tényleges üzleti logika.
> - **Models/DTOs:** Segítenek abban, hogy ne az adatbázis entitásokat tegyük közzé közvetlenül az API-n keresztül.

## 🗂️ Projekt szerkezete

> ### 🏷️ Attributes
> Itt találhatók az egyedi dekorátorok, amelyek az adatvalidációért és a végpontok hozzáférési logikájáért (pl. jogosultságkezelés) felelnek.

> ### 🎮 Controllers
> Az API belépési pontjai. Feladatuk kizárólag a beérkező HTTP kérések fogadása, a paraméterek átadása a szervizeknek, majd a válaszok visszaküldése.

> ### ⚠️ Exceptions
> Egyedi hibaosztályok gyűjteménye, amelyek lehetővé teszik a pontosabb hibakezelést és az alkalmazásspecifikus hibaüzenetek továbbítását a felhasználó felé.

> ### 🧩 Extensions
> C# extension metódusok, amelyek segítik a kód olvashatóságát. Itt történik többek között a szolgáltatások (Dependency Injection) tiszta regisztrációja is.

> ### 🧪 Filters
> Olyan szűrők, amelyek a kérések életciklusába avatkoznak be (pl. logolás, extra validáció), mielőtt azok elérnék a kontrollert.

> ### 🗺️ Mappers
> Az adatok transzformációjáért felelős réteg. Itt dől el, hogyan alakulnak át az adatbázis entitások (Models) biztonságos kimeneti objektumokká (DTO).

> ### ⚙️ Middlewares
> A kérések feldolgozási láncában (Pipeline) elhelyezkedő komponensek, mint például a globális hibakezelő vagy a hitelesítési folyamatok.

> ### 📦 Models
> A projekt adatstruktúráit tartalmazza: az adatbázis táblákat leképező entitásokat és a kommunikációhoz használt adatátviteli objektumokat.

> ### 🚦 RateLimiter
> Az API terhelésvédelméért felelős konfigurációk, amelyek megakadályozzák a végpontok túlterhelését.

> ### 🧠 Services
> Az alkalmazás "agya". Itt található az összes üzleti logika és a komplex számítások, elszeparálva a webes felülettől.

> ### Program.cs
> Az alkalmazás belépési pontja.
> ### appsettings.json
> Környezetfüggő beállításokat (pl. Connection String).
---

## 🧪 API végpontok

🔹 Felhasználók
| HTTP metódus | Útvonal                   | Leírás                           |
| ------------ | ------------------------- | -------------------------------- |
| `POST`       | `/api/Users/register` | Új felhasználó regisztrálása     |
| `POST`       | `/api/Users/login`        | Bejelentkezés és token generálás |

🔹 Filmek (autentikáció szükséges)
| HTTP metódus | Útvonal           | Leírás                         |
| ------------ | ----------------- | ------------------------------ |
| `GET`        | `/api/Movie`      | Összes film lekérése     |
| `GET`        | `/api/Movie/{id}` | Film lekérése ID alapján |
| `POST`       | `/api/Movie`      | Új film létrehozása         |
| `PUT`        | `/api/Movie/{id}` | Film módosítása             |
| `DELETE`     | `/api/Movie/{id}` | Film törlése                |

🔹 Kedvenc filmek (autentikáció szükséges)
| HTTP metódus | Útvonal           | Leírás                         |
| ------------ | ----------------- | ------------------------------ |
| `GET`        | `/api/Favorite`| Felhasználó összes kedvenc filmje    |
| `POST`        | `/api/Favorite/add/{id}` | Felhasználó kedvenc filmjeihez való hozzáadás |
| `DELETE`       | `/api/Favorite/remove/{id}` | Felhasználó kedvenc filmjeiből való törlés |

🔹 Filmek jelentése (autentikáció szükséges)
| HTTP metódus | Útvonal           | Leírás                         |
| ------------ | ----------------- | ------------------------------ |
| `GET`        | `/api/MovieReport`      | Összes film jelentés lekérése     |
| `PUT`        | `/api/MovieReport/{id}` | Jelentés módosítása ID alapján |
| `POST`       | `/api/MovieReport`      | Új jelentés létrehozása         |

🔹 Film kérése (autentikáció szükséges)
| HTTP metódus | Útvonal           | Leírás                         |
| ------------ | ----------------- | ------------------------------ |
| `GET`        | `/api/MovieRequest`      | Összes film kérés lekérése     |
| `PUT`        | `/api/MovieRequest/{id}` | Film kérés módosítása ID alapján |
| `POST`       | `/api/MovieRequest/send`      | Új film kérés létrehozása |

---
## 🌍 Frontend integráció

A backendhez készül egy React+Tailwind alapú frontend is:
👉[Movie Stream Client](https://github.com/Riptir3/MovieStreamClient). 
A két alkalmazás Axios-on keresztül kommunikál, a `https://localhost:7084/api/...` végpontokat használva.

## ⚙️ Telepítés és futtatás
### 🛠️ Előfeltételek
- .NET 8.0 SDK (vagy frissebb)
- MongoDB Atlas felhasználó
- Redis szerver (Docker ajánlott)
- Környzeti változók megadása az ```appsettings.json```-ben
<img width="388" height="355" alt="Appsettings" src="https://github.com/user-attachments/assets/89df043b-267f-4f24-81aa-9bdf253ed472" />


### 1️⃣ Klónozd a repót
```bash
git clone https://github.com/Riptir3/MovieStream.Api.git
cd MovieStream.Api
```
### 2️⃣ Telepítsd a függőségeket
```bash
dotnet restore
```
### 3️⃣ Futtatás
```bash
dotnet run
```
### A backend elérhető lesz itt:
```arduino
https://localhost:7084
```
### Swagger UI:
```bash
https://localhost:7084/swagger
```

## Kapcsolat

Fejlesztő: **Riptir3 (Bence)**  
GitHub: [github.com/Riptir3](https://github.com/Riptir3)
