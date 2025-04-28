# Vehicle Rental Platform 🚗

This project is a simplified vehicle rental management API.

## 📦 Project Structure
- `VehicleRentalPlatform.API` — Main API project
- `VehicleRentalPlatform.Domain` — Domain entities (Vehicle, Rental, Customer, Telemetry)
- `VehicleRentalPlatform.Infrastructure` — Database and seeding logic
- `VehicleRentalPlatform.Tests` — Unit and integration tests

---

## 🚀 How to Run

### 1. Using CLI
```bash
cd VehicleRentalPlatform.API
dotnet run

🛠️ Architecture and Design Decisions
Clean Architecture: Domain, Infrastructure, API layers separated.

Entity Framework Core: For data persistence.

InMemory Seeding: Vehicles and Telemetry data are seeded from CSV files. 

Validation:

Basic data validation in DTOs and Controllers.

Overlapping rental validation.

Telemetry data filtering before database insert.

Swagger UI: Automatically available for API documentation.

Testing:

Unit tests for core methods.

Integration tests for API endpoints (customers and rentals).

📚 API Overview
Rentals
POST /api/rentals — Create rental (with validation)

PUT /api/rentals/{id} — Update rental dates

PATCH /api/rentals/{id}/cancel — Cancel a rental

GET /api/rentals — List all rentals

GET /api/rentals/{id} — Rental details (distance, price, battery)

Customers
POST /api/customers — Create customer

PUT /api/customers/{id} — Update customer

DELETE /api/customers/{id} — Delete customer

GET /api/customers — List all customers

GET /api/customers/{id} — Customer details (total distance, total price)

Vehicles
GET /api/vehicles — List all vehicles

GET /api/vehicles/{vin} — Vehicle details (total distance, rental count, income)

✅ Requirements covered
Rental time and overlap validation.

Telemetry filtering rules.

Price calculation logic per rental.

Summary metrics per customer and vehicle.

Basic test coverage.

ℹ️ Notes
Seeding loads vehicles and telemetry data automatically from csv files at startup, 

No authentication/authorization (project scoped).
