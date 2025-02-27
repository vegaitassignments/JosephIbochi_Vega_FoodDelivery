# Food Delivery Web API

## Overview
This project is a food delivery web application built with C# and ASP.NET Core, using MySQL as the database and Docker for containerized deployment. The application allows users to browse restaurants, place orders, and rate food items.

## Prerequisites
Ensure you have the following installed before running the application:

- [.NET 8 SDK](https://dotnet.microsoft.com/)
- [Docker](https://www.docker.com/)
- [MySQL](https://www.mysql.com/) (if running locally)
- [Git](https://git-scm.com/) (to clone the repository)

## Running the Application

### Running with Docker

1. **Clone the repository:**
   ```sh
   git clone <repository_url>
   cd FoodDelivery
   ```

2. **Build and start the containers:**
   ```sh
   docker-compose up --build
   ```
   This will:
   - Pull the required MySQL image
   - Set up the database
   - Build the application image
   - Run the API and database containers

3. **Access the API:**
   - The API will be available at `http://localhost:5000`

4. **Stopping the application:**
   ```sh
   docker-compose down
   ```
   This stops and removes the containers but keeps the database data intact.

### Running Locally

1. **Set up the MySQL database manually:**
   - Ensure MySQL is installed and running on your system.
   - Create a database named `FoodDeliveryDB`.
   - Update the connection string in `appsettings.json`:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "server=localhost;database=FoodDeliveryDB;user=root;password=yourpassword"
     }
     ```

2. **Apply database migrations:**
   ```sh
   dotnet ef database update
   ```

3. **Run the application:**
   ```sh
   dotnet run
   ```
   - The API will be available at `http://localhost:5000`.

4. **Stopping the application:**
   - Press `CTRL+C` to stop the server.

## Additional Comments

- If running with Docker, ensure no other services are using port `3306` to avoid conflicts.
- If you encounter database connection issues, try restarting Docker and running `docker-compose up --build` again.
- To remove all Docker containers and images (use with caution):
  ```sh
  docker system prune -a
  ```
- If needed, you can modify environment variables such as database credentials in `docker-compose.yml`.