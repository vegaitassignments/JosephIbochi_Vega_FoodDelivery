# Food Delivery API

## Overview
This is a food delivery API that facilitates a restaurant chain, enabling seamless online food ordering and delivery. The system efficiently coordinates between restaurants, couriers, and customers to ensure a smooth ordering and delivery experience.

Each restaurant in the chain has a fixed courier responsible for deliveries, with a set delivery time of **15 minutes** regardless of location. The API provides functionalities for user authentication, restaurant management, food ordering, and more.

## Tech Stack
- **Backend:** .NET 8, ASP.NET Core
- **Database:** MySQL
- **Authentication:** JWT-based authentication
- **Containerization:** Docker
- **ORM:** Entity Framework Core
- **Authorization:** Identity Roles
- **API Documentation:** Postman

## Running the Project with Docker
Ensure you have **Docker** installed before proceeding.

### Steps to Run:
1. Clone the repository:
   ```sh
   git clone https://github.com/your-repo/food-delivery-api.git
   cd food-delivery-api
   ```
2. Build and run the application with Docker:
   ```sh
   docker-compose up --build
   ```
3. The API will be accessible at `http://localhost:5000`

**Note:** No need to manually run migrations; the application handles this inside Docker.

## Running the Project Locally (Optional)
If you prefer running the application outside Docker, follow these steps:
1. Ensure you have **.NET 8 SDK** and **MySQL** installed.
3. Run the project:
   ```sh
   dotnet run
   ```

## Authentication Endpoints
- `POST /auth/register` - Register a new user
- `POST /auth/login` - User login
- `POST /auth/add-admin` - Add an admin (Admin-only access required)
- `POST /auth/forgot-password` - Request a password reset token
- `POST /auth/reset-password` - Reset password using the token

### Default Admin Login
```json
{
  "Email": "someadminx345@fooddelivery.com",
  "Password": "r6sff@73&Y@&@QWER19R8"
}
```

## Restaurant Endpoints
- `POST /restaurants` - Create a restaurant
- `GET /restaurants` - Get all restaurants
- `GET /restaurants/{id}` - Get a restaurant by ID
- `PUT /restaurants/{id}` - Update a restaurant
- `DELETE /restaurants/{id}` - Delete a restaurant
- `GET /restaurants/{id}/orders` - Get orders for a restaurant

## Food Endpoints
- `POST /foods` - Add a food item
- `GET /foods` - Get all food items
- `GET /foods/{id}` - Get a food item by ID
- `PUT /foods/{id}` - Update a food item
- `DELETE /foods/{id}` - Remove a food item
- `POST /foods/{id}/rate` - Rate a food item

## Order Endpoints
- `POST /orders` - Place an order
- `GET /orders` - Get all orders
- `GET /orders/{id}` - Get a single order
- `GET /orders/users/{id}` - Get orders by a user
- `POST /orders/{id}/cancel` - Cancel an order

## Limitations
- No **email provider** is used for password resets, only token-based reset.
- No **third-party** services are used for location handling.

## Worthy to note
For this task, i had to push both my appsettings.Development.json file which contains some configuration information. But in an ideal production setting, i would not do this, due to security reasons

---
This API is optimized for seamless food delivery within a restaurant chain. 🚀
