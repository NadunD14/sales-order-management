# Environment Configuration

## Backend Environment Variables

Create an `appsettings.json` file in the API project with the following configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=SalesOrderManagementDb.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**Note**: The application now uses SQLite instead of SQL Server for easier setup and portability. No additional database installation is required.

### Alternative Database Options:

**For SQL Server Express LocalDB (if available):**
```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SalesOrderManagementDb;Trusted_Connection=true;TrustServerCertificate=true;"
```

**For SQL Server Express:**
```json
"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=SalesOrderManagementDb;Trusted_Connection=true;TrustServerCertificate=true;"
```

**For SQL Server with SQL Authentication:**
```json
"DefaultConnection": "Server=localhost;Database=SalesOrderManagementDb;User Id=your_username;Password=your_password;TrustServerCertificate=true;"
```

## Frontend Environment Variables

Create a `.env` file in the Frontend directory with:

```
REACT_APP_API_URL=http://localhost:5000/api
```

## Database Setup

The application uses Entity Framework Code First approach with **SQLite database**. The database file (`SalesOrderManagementDb.db`) will be created automatically in the API project directory when you run the application for the first time.

### Features:
- **No installation required** - SQLite is embedded
- **Automatic database creation** with sample data
- **Cross-platform compatibility**
- **Easy backup** - just copy the .db file

## Prerequisites

- **.NET 8.0 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Node.js (version 16 or later)** - [Download here](https://nodejs.org/)
- **Visual Studio 2022** or **Visual Studio Code** (optional)

**Note**: SQL Server is no longer required as the application uses SQLite.