# EventSphere

EventSphere is a web-based event management system designed to facilitate the organization and administration of events, attendees, tickets, and organizers.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Setup and Installation](#setup-and-installation)
- [Running the Project](#running-the-project)
- [Folder Contents](#folder-contents)
- [API Documentation](#api-documentation)
- [Contributing](#contributing)
- [License](#license)

## Prerequisites

- .NET 7 SDK
- Node.js and npm
- PostgreSQL

## Setup and Installation

### Backend

1. Navigate to the backend directory:
    ```bash
    cd backend
    ```

2. Restore the dependencies:
    ```bash
    dotnet restore
    ```

3. Update the database connection string in `appsettings.json`.

4. Apply the database migrations:
    ```bash
    dotnet ef database update
    ```

5. Build the backend project:
    ```bash
    dotnet build
    ```

### Frontend

1. Navigate to the frontend directory:
    ```bash
    cd frontend
    ```

2. Install the dependencies:
    ```bash
    npm install
    ```

## Running the Project

### Backend

1. Run the backend project:
    ```bash
    dotnet run
    ```

2. The backend API will be available at `https://localhost:7135`.

### Frontend

1. Run the frontend project:
    ```bash
    npm start
    ```

2. The frontend application will be available at `http://localhost:3000`.

## Folder Contents

### Backend

- **Controllers/**: Contains API controllers for managing attendees, events, organizers, and tickets.
- **DTOs/**: Contains Data Transfer Objects for handling data between the client and the server.
- **Models/**: Contains the entity models that define the database schema.
- **Services/**: Contains service classes that implement the business logic.
- **Repositories/**: Provides a layer of abstraction between the application and data access logic.
- **Data/**: Contains the database context and migration files.
- **EventSphere.csproj**: The project file for the ASP.NET Core backend.
- **Program.cs**: The entry point of the backend application.

### Frontend

- **public/**: Contains the public assets and the main `index.html` file.
- **src/**: Contains the source code for the frontend application.
  - **components/**: Contains the React components organized by feature (Attendees, Auth, Events, Organizers, Tickets).
  - **services/**: Contains the service files for making API calls.
  - **App.js**: The main application component.
  - **index.js**: The entry point of the frontend application.
  - **index.css**: Contains global CSS styles.
- **package.json**: Contains the project configuration and dependencies for the frontend.
- **package-lock.json**: Contains the exact versions of the dependencies installed.

## API Documentation

The API documentation is available via Swagger. Once the backend is running, you can access it at:
`https://localhost:7135/swagger/index.html` 


