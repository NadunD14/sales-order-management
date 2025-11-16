# Sales Order Management System

A comprehensive web application for managing sales orders built with .NET Core Web API backend and React frontend.

## Project Structure

```
SalesOrderManagement/
├── Backend/
│   ├── SalesOrderManagement.API/          # Web API Controllers & DTOs
│   ├── SalesOrderManagement.Application/   # Business Logic & Interfaces
│   ├── SalesOrderManagement.Domain/        # Entities & Domain Models
│   └── SalesOrderManagement.Infrastructure/ # Data Access & Repositories
├── Frontend/                              # React Application
├── SalesOrderManagement.sln              # Visual Studio Solution
├── ENVIRONMENT_SETUP.md                  # Environment Configuration
└── README.md                            # This file
```

## Features

### Backend Features
- **Clean Architecture** with layered separation (API, Application, Domain, Infrastructure)
- **Entity Framework Core** with Code First approach
- **SQL Server** database with LocalDB support
- **RESTful API** endpoints for all operations
- **Dependency Injection** using built-in .NET Core container
- **Auto-generated invoice numbers**
- **Automatic calculation** of line totals and order totals

### Frontend Features
- **React 18** with functional components and hooks
- **Redux Toolkit** for state management
- **React Router** for navigation
- **Tailwind CSS** for responsive design
- **Axios** for API communication
- **Real-time calculations** for order items
- **Dropdown selections** for customers and items
- **Auto-population** of customer address fields

### Screens

#### Screen 1 - Sales Order Form
- Customer dropdown with auto-populated address fields
- Item selection via code or description dropdowns
- Line item calculations (Excl Amount, Tax Amount, Incl Amount)
- Order totals calculation
- Save and edit functionality

#### Screen 2 - Home/Order List
- Grid display of all sales orders
- Double-click to edit orders
- Add new order button
- Statistics dashboard
- Responsive design

## Prerequisites

Before running this project, make sure you have the following installed:

- **.NET 8.0 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Node.js (v16+)** - [Download here](https://nodejs.org/)
- **SQL Server LocalDB** (comes with Visual Studio) or SQL Server Express
- **Visual Studio 2022** or **Visual Studio Code** (optional)

## Quick Start

### 1. Clone or Extract the Project
```powershell
# Navigate to your projects directory
cd "d:\Projects\SpilLabs\SalesOrderManagement"
```

### 2. Backend Setup

#### Install .NET Dependencies
```powershell
# Navigate to backend directory
cd Backend

# Restore NuGet packages for all projects
dotnet restore
```

#### Configure Database Connection
The application is configured to use SQL Server LocalDB by default. If you need to change this:

1. Edit `Backend/SalesOrderManagement.API/appsettings.json`
2. Update the `DefaultConnection` string as needed (see ENVIRONMENT_SETUP.md)

#### Run the Backend
```powershell
# Navigate to API project
cd Backend/SalesOrderManagement.API

# Run the API (will auto-create database on first run)
dotnet run
```

The API will start at `https://localhost:5001` and `http://localhost:5000`

### 3. Frontend Setup

#### Install Node Dependencies
```powershell
# Navigate to frontend directory (from project root)
cd Frontend

# Install npm packages
npm install
```

#### Configure API URL (Optional)
If your API runs on a different port, create a `.env` file in the Frontend directory:
```
REACT_APP_API_URL=http://localhost:5000/api
```

#### Run the Frontend
```powershell
# Start the React development server
npm start
# Sales Order Management

A full-stack web app for managing sales orders with a .NET 8 Web API backend and a React 18 frontend. Data persists in a local SQLite database using Entity Framework Core migrations.

## Tech Stack

- Backend: .NET 8, ASP.NET Core Web API, EF Core (SQLite), AutoMapper
- Frontend: React 18, Redux Toolkit, React Router, Tailwind CSS, Axios
- Architecture: Clean Architecture (API, Application, Domain, Infrastructure)

## Project Structure

```
SalesOrderManagement/
├── Backend/
│   ├── SalesOrderManagement.API/            # Web API (controllers, startup)
│   ├── SalesOrderManagement.Application/    # Services, DTOs, interfaces
│   ├── SalesOrderManagement.Domain/         # Entities and domain logic
│   └── SalesOrderManagement.Infrastructure/ # EF Core DbContext, repositories, migrations
└── Frontend/                                # React application
```

## Prerequisites

- .NET 8 SDK
- Node.js 18+ (LTS recommended)
- PowerShell (Windows default)

## Getting Started

### Backend (API)

```powershell
# From repo root
cd Backend/SalesOrderManagement.API

# Restore and run
dotnet restore
dotnet run
```

- API runs on `http://localhost:5000` (Swagger enabled in Development)
- Database file: `Backend/SalesOrderManagement.API/SalesOrderManagementDb.db`
- Migrations are applied automatically at startup; seed data is added only if tables are empty

Optional: change the connection string in `appsettings.json` (`DefaultConnection`) if you want to move the SQLite file.

### Frontend (React)

```powershell
# From repo root
cd Frontend

npm install
npm start
```

- App runs on `http://localhost:3000`
- By default, the frontend calls the API at `http://localhost:5000/api`
- To override, create `Frontend/.env` with:

```
REACT_APP_API_URL=http://localhost:5000/api
```

## Key Features

- Customer dropdown with auto-filled address
- Item selection by code or description
- Per-line calculations: Excl, Tax, Incl
- Order totals auto-calculated
- Full CRUD for sales orders

## API Endpoints

- Clients: `GET /api/clients`, `GET /api/clients/{id}`
- Items: `GET /api/items`, `GET /api/items/{id}`
- Sales Orders: `GET /api/salesorders`, `GET /api/salesorders/{id}`, `POST /api/salesorders`, `PUT /api/salesorders/{id}`, `DELETE /api/salesorders/{id}`

## Data Persistence

- Uses EF Core migrations with `context.Database.Migrate()` on startup
- Seed data (5 clients, 10 items) is inserted only when tables are empty
- The SQLite `.db` file is ignored by git (local-only)

## Troubleshooting

- Ports busy: change API URLs via `dotnet run --urls="http://localhost:7000"` or set `PORT=3001` for React
- CORS: API allows `http://localhost:3000`
- If the DB file is deleted, it will be recreated and seeded on next API start

## Notes

- The folder `how to/` contains design assets and is ignored by git
- EF Core migrations are included under `Backend/SalesOrderManagement.Infrastructure/Migrations`

## License

This project is provided for technical assessment and demonstration purposes.