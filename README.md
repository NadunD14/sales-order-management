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
```

The application will open at `http://localhost:3000`

## Database Information

### Tables Created
- **Clients** - Customer information with addresses
- **Items** - Product catalog with codes, descriptions, and prices
- **SalesOrders** - Sales order headers
- **SalesOrderItems** - Sales order line items

### Sample Data
The application includes seed data:
- 5 sample customers with addresses
- 10 sample products with codes and prices

## API Endpoints

### Clients
- `GET /api/clients` - Get all clients
- `GET /api/clients/{id}` - Get client by ID

### Items  
- `GET /api/items` - Get all items
- `GET /api/items/{id}` - Get item by ID

### Sales Orders
- `GET /api/salesorders` - Get all sales orders
- `GET /api/salesorders/{id}` - Get sales order by ID
- `POST /api/salesorders` - Create new sales order
- `PUT /api/salesorders/{id}` - Update sales order
- `DELETE /api/salesorders/{id}` - Delete sales order
- `GET /api/salesorders/generate-invoice-number` - Generate next invoice number

## Usage Guide

### Creating a New Sales Order

1. **Start the Application**
   - Open `http://localhost:3000` in your browser
   - You'll see the Home screen with existing orders (if any)

2. **Add New Order**
   - Click the "Add New" button
   - This opens the Sales Order Form

3. **Fill Order Details**
   - Select a customer from the dropdown
   - Address fields will auto-populate
   - Invoice number is auto-generated
   - Set invoice date and add reference/notes as needed

4. **Add Items**
   - Click "Add Item" to create a new line
   - Select item by code OR description (both dropdowns are linked)
   - Enter quantity and tax rate
   - Amounts are calculated automatically

5. **Save Order**
   - Click "Save Order" to create the sales order
   - You'll be redirected to the Home screen

### Editing an Existing Order

1. **From Home Screen**
   - Double-click on any row in the orders table
   - This opens the order in edit mode

2. **Make Changes**
   - Modify any fields as needed
   - Add/remove items
   - Update quantities or tax rates

3. **Save Changes**
   - Click "Save Order" to update the order

## Troubleshooting

### Common Issues

1. **"No .NET SDKs were found" Error**
   - Download and install .NET 8.0 SDK
   - Restart your terminal/command prompt

2. **Database Connection Issues**
   - Ensure SQL Server LocalDB is installed
   - Check connection string in appsettings.json
   - Try using SQL Server Express if LocalDB doesn't work

3. **Frontend API Connection Issues**
   - Ensure backend is running on the correct port
   - Check if CORS is properly configured
   - Verify the API URL in the frontend environment

4. **Port Already in Use**
   - Backend: Edit `launchSettings.json` to use different ports
   - Frontend: Set `PORT=3001` in `.env` file to use port 3001

### Running on Different Ports

**Backend (API):**
```powershell
# Run on specific port
dotnet run --urls="https://localhost:7001;http://localhost:7000"
```

**Frontend:**
```powershell
# Set port in .env file
echo "PORT=3001" > .env
npm start
```

## Development Notes

### Architecture Highlights
- **Clean Architecture** ensures separation of concerns
- **Repository Pattern** for data access abstraction
- **CQRS-like** approach with separate DTOs for commands and queries
- **Dependency Injection** for loose coupling

### Technology Stack
- **Backend:** .NET 8, Entity Framework Core, SQL Server, AutoMapper
- **Frontend:** React 18, Redux Toolkit, React Router, Tailwind CSS, Axios
- **Database:** SQL Server with LocalDB support

### Future Enhancements
- Add user authentication and authorization
- Implement order status workflow
- Add reporting and printing functionality
- Include inventory management
- Add email notifications
- Implement order search and filtering

## Support

If you encounter any issues:
1. Check the troubleshooting section above
2. Ensure all prerequisites are installed
3. Verify the environment configuration
4. Check that both backend and frontend are running

## License

This project is for demonstration purposes as part of SPIL Labs assessment.#   s a l e s - o r d e r - m a n a g e m e n t  
 