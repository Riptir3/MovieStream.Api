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

```
TaskManagerAPI/
│
├── Controllers/
│ ├── UsersController.cs -> Felhasználói végpontok ( regisztráció, bejelentkezés ).
│ └── TasksController.cs -> Felhasználói feladatok végpontjai ( CRUD, keresés/szűrés).
│
├── Data/
│ ├── AppDbContext.cs -> Adatbázis konfiguráció.
│
├── Filters/
│ ├── ValidationFilter.cs -> Validációs hibák kezelése.
│
├── Middlewares/
│ ├── ErrorHandlingMiddleware.cs -> Hiba kezelés.
│ └── ValidationErrorMiddleware.cs -> Validációs hibák eljutatása a frontendre.
│
├── Migrations/ -> Adatbázis migrációk.
|
├── Models/
│ ├── DTOs/ -> Data Transfer Objects.
│ ├── Entities/ -> Adatbázis modellek.
│ ├── ApiResponse.cs -> Egyedi response object.
│
├── Services/
│ ├── JwtService.cs -> JWT token generálás
│ └── PasswordService.cs -> Jelszó titkosítás és ellenőrzés.
│
├──appsettings.json -> Konfigurációs fájl.
|
└── Program.cs
```
## 🧪 API végpontok

🔹 Felhasználók
| HTTP metódus | Útvonal                   | Leírás                           |
| ------------ | ------------------------- | -------------------------------- |
| `POST`       | `/api/Users/register` | Új felhasználó regisztrálása     |
| `POST`       | `/api/Users/login`        | Bejelentkezés és token generálás |

🔹 Feladatok (autentikáció szükséges)
| HTTP metódus | Útvonal           | Leírás                         |
| ------------ | ----------------- | ------------------------------ |
| `GET`        | `/api/Tasks`      | Összes feladat lekérdezése     |
| `GET`        | `/api/Tasks/{id}` | Feladat lekérdezése ID alapján |
| `POST`       | `/api/Tasks`      | Új feladat létrehozása         |
| `PUT`        | `/api/Tasks/{id}` | Feladat módosítása             |
| `DELETE`     | `/api/Tasks/{id}` | Feladat törlése                |

## 🔑 JWT hitelesítés

A bejelentkezés után a szerver visszaad egy JWT tokent, amelyet a kliens minden kérésnél a headerben küld el:
``` makefile
Authorization: Bearer <token>
```
### Példa:
``` http
GET /api/Tasks HTTP/1.1
Host: localhost:7242
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```
A token lejárata után egyelőre a kliens újra bejelentkezésre kényszerül.

## 🌍 Frontend integráció

A backendhez készül egy React+Tailwind alapú frontend is:
👉[Task Manager Frontend](https://github.com/Riptir3/task-manager-frontend). 
A két alkalmazás Axios-on keresztül kommunikál, a `https://localhost:7242/api/...` végpontokat használva.

## ⚙️ Telepítés és futtatás

### 1️⃣ Klónozd a repót
```bash
git clone https://github.com/Riptir3/TaskManager.Api.git
cd TaskManager.API
```
### 2️⃣ Telepítsd a függőségeket
```bash
dotnet build
```
### 3️⃣ Adatbázis létrehozása
```bash
dotnet ef database update
```
### 4️⃣ Futtatás
```bash
dotnet run
```
### A backend elérhető lesz itt:
```arduino
https://localhost:7242
```
### Swagger UI:
```bash
https://localhost:7242/swagger
```

## Kapcsolat

Fejlesztő: **Riptir3 (Bence)**  
GitHub: [github.com/Riptir3](https://github.com/Riptir3)
