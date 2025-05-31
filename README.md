# 🛒 E-Bazar System

This repository contains two projects:
- `EBazar.API` – ASP.NET Core Web API
- `EbazarWeb` – ASP.NET Core Web frontend

## ⚙️ Setup Instructions

- Clone the repository:
  - `git clone https://github.com/yourusername/EBazar.git`
  - `cd EBazar`

- Open the solution (`EBazar.sln`) in Visual Studio.

- Rebuild the solution:
  - `dotnet build`

- Update the database connection string in `EBazar.API/appsettings.json`.

- Apply database migrations:
  - Navigate to `EBazar.API` folder
  - Run `dotnet ef database update`
  - *(Optional)* Add migration with `dotnet ef migrations add MigrationName`

- Run both projects with HTTPS:

## ✅ Requirements

- .NET 8
- SQL Server
