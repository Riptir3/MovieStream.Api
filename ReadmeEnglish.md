# 📺 MovieStream API

The backend server for the MovieStream application, built on **ASP.NET Core 8 Web API**. This project provides a robust, scalable RESTful API responsible for movie management, user interactions, and secure streaming workflows.

---

## 🚀 Technology Stack
- **Framework:** .NET 8.0 (ASP.NET Core Web API)
- **Architecture:** Layered Architecture (N-Tier)
- **Data Management:** MongoDB Atlas (NoSQL), Redis (Caching/Rate Limiting)
- **Security:** JWT Authentication, Custom Middlewares, Validation Attributes, and Policy-based Authorization.
- **Other:** Fixed-window Rate Limiting, Global Exception Handling, AutoMapper for DTO mapping.

---

## 🔒 Security

> ### 🔑 JWT (JSON Web Token)
> The API utilizes JWT-based authentication to ensure secure communication. Token generation and validation are handled within the Middleware layer, ensuring that only authenticated users can access protected endpoints.

> ### 🛡️ Rate Limiting
> To prevent abuse and mitigate DDoS attacks, the API features built-in rate limiting. This is managed via the `RateLimiter/` configuration, defining the maximum number of requests allowed per IP address within a specific time window.
> - **Note:** Connection data for rate limit tracking is stored in a **Redis** server running in a Docker container.

> ### 🧱 Custom Middlewares
> Specialized middlewares handle request pre-processing:
> - **Error Handling Middleware:** Provides standardized JSON-formatted error responses for all exceptions.
> - **Validation Filters:** Automatically verify the integrity of incoming data before it reaches the Controllers.

---

## 🛠️ API Architecture

> ### 📂 Layered Structure
> Separation of concerns ensures high maintainability:
> - **Controllers:** Strictly responsible for receiving requests and returning HTTP responses.
> - **Services:** The "brain" of the application, containing all core business logic.
> - **Models/DTOs:** Used to abstract the database schema and prevent exposing entities directly to the client.

---

## 🗂️ Project Structure

> ### 🏷️ Attributes
> Contains custom decorators for data validation and endpoint access control (e.g., permission-based logic).

> ### 🎮 Controllers
> The entry points of the API. They delegate tasks to services and return the appropriate status codes.

> ### ⚠️ Exceptions
> A collection of custom exception classes that allow for granular error handling and application-specific error messages.

> ### 🧩 Extensions
> C# extension methods to improve code readability, specifically for clean Dependency Injection (DI) registration.

> ### 🧪 Filters
> Components that hook into the request lifecycle (e.g., logging, extra validation) before the controller action executes.

> ### 🗺️ Mappers
> The data transformation layer (AutoMapper) that handles the conversion between Database Entities and DTOs.

> ### ⚙️ Middlewares
> Components in the request processing pipeline, such as the global error handler or authentication handlers.

> ### 📦 Models
> Contains data structures: database entities for MongoDB and Data Transfer Objects for API communication.

> ### 🚦 RateLimiter
> Protection configurations that prevent endpoint exhaustion.

> ### 🧠 Services
> Isolated business logic layer where complex calculations and data manipulations occur.

> ### 📄 Program.cs & appsettings.json
> `Program.cs` is the application's entry point where services and the pipeline are configured, while `appsettings.json` stores environment-specific settings (e.g., Connection Strings).

---

## 🧪 API Endpoints

### 🔹 Users
| Method | Route | Description |
| :--- | :--- | :--- |
| `POST` | `/api/Users/register` | Register a new user |
| `POST` | `/api/Users/login` | Authenticate and generate JWT |

### 🔹 Movies (Authentication Required)
| Method | Route | Description |
| :--- | :--- | :--- |
| `GET` | `/api/Movie` | Retrieve all movies |
| `GET` | `/api/Movie/{id}` | Get movie details by ID |
| `POST` | `/api/Movie` | Create a new movie entry |
| `PUT` | `/api/Movie/{id}` | Update an existing movie |
| `DELETE` | `/api/Movie/{id}` | Remove a movie |

### 🔹 Favorites (Authentication Required)
| Method | Route | Description |
| :--- | :--- | :--- |
| `GET` | `/api/Favorite` | Get all favorite movies for the user |
| `POST` | `/api/Favorite/add/{id}` | Add a movie to favorites |
| `DELETE` | `/api/Favorite/remove/{id}` | Remove a movie from favorites |

### 🔹 Reporting & Requests (Authentication Required)
| Method | Route | Description |
| :--- | :--- | :--- |
| `GET` | `/api/MovieReport` | Retrieve all movie reports |
| `PUT` | `/api/MovieReport/{id}` | Update a report by ID |
| `POST` | `/api/MovieReport` | Report an issue with a movie |
| `GET` | `/api/MovieRequest` | Retrieve all movie requests |
| `PUT` | `/api/MovieRequest/{id}` | Update a request by ID |
| `POST` | `/api/MovieRequest/send` | Request a new movie to be added |

---

## 🌍 Frontend Integration

This backend serves a React + Tailwind CSS client:
👉 [Movie Stream Client](https://github.com/Riptir3/MovieStreamClient)

Communication is handled via **Axios**, targeting the `https://localhost:7084/api/...` endpoints.

---

## ⚙️ Installation & Setup

### 🛠️ Prerequisites
- **.NET 8.0 SDK** (or newer)
- **MongoDB Atlas** account/cluster
- **Redis** server (Docker recommended)
- **Environment Variables:** Must be configured in `appsettings.json`.
<img width="388" height="355" alt="Appsettings" src="https://github.com/user-attachments/assets/89df043b-267f-4f24-81aa-9bdf253ed472" />

### 1️⃣ Clone the Repository
```bash
git clone https://github.com/Riptir3/MovieStream.Api.git
cd MovieStream.Api
```
### 2️⃣ Restore Dependencies
```bash
dotnet restore
```
### 3️⃣ Run the Application
```bash
dotnet run
```
### Accessing the API
```arduino
https://localhost:7084
```
### Swagger UI:
```bash
https://localhost:7084/swagger
```

## Contact

Developer: **Riptir3 (Bence)**  
GitHub: [github.com/Riptir3](https://github.com/Riptir3)
