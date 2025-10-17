# Mixi

**Mixi** is a self-hosted, minimalistic cross-platform **.NET MAUI Blazor Hybrid** app designed to assist with organizing and running tabletop role-playing games (TTRPGs). It offers tools for both Game Masters and players to streamline session prep, gameplay, and immersion.

## ✨ Features

- 🎭 Game Master and Player accounts  
- 🧠 Name generator  
- 📇 Character card support  
- 🧩 Modular and extensible architecture  

## 🛠️ Tech Stack

- .NET MAUI Blazor Hybrid (client)  
- ASP.NET Core Web API (backend)  
- C# / .NET 8  
- Microsoft SQL Server  
- Entity Framework Core  

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)  
- [Visual Studio 2022 (17.8+)](https://visualstudio.microsoft.com/vs/) with the **.NET Multi-platform App UI** workload  
- [Microsoft SQL Server (Express or Developer edition)](https://www.microsoft.com/sql-server/sql-server-downloads)  
- [SQL Server Management Studio (SSMS)](https://aka.ms/ssmsfullsetup) *(optional, for managing your database)*  
- Node.js  


### Database Setup (SQL Server)

Mixi now uses **Microsoft SQL Server** instead of SQLite.  
The API project (`Mixi.Api`) connects to a **local SQL Server** instance using credentials stored in **.NET user secrets**.

#### 1. Install and run SQL Server locally

Download and install **SQL Server Express** or **Developer Edition**, then make sure the local instance is running (e.g., `localhost` or `localhost\SQLEXPRESS`).

#### 2. Create the database

Open SQL Server Management Studio or use the command line to create an empty database named **MixiDB**:


#### 3. Configure secrets for Mixi.Api

In the project directory of Mixi.Api, initialize and set your secrets:

```
cd Mixi.Api
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=MixiDB;User Id=your_user;Password=your_password;TrustServerCertificate=true;"
```
Replace your_user and your_password with your actual SQL Server login credentials.

#### 4. Apply EF Core migrations
Since migrations are not applied automatically at startup, you need to apply them manually before running the app:

```
cd Mixi.Api
dotnet ef database update
```
## ▶️ Run the App
Run the backend (API) and client separately:

```
dotnet run --project Mixi.Api
dotnet run --project Mixi.App
```
## 🧩 Planned Features
Campaign/session manager

Player dashboard

Dice roller

Music player (local, Spotify, and Tidal planned)

## 🤝 Contributing
Pull requests are welcome! To contribute:

1. Fork the project

2. Create your feature branch:
```
git checkout -b feature/my-feature
```
3. Commit your changes:
```
git commit -m 'feat: added feature'
```
4. Push to the branch:
```
git push origin feature/my-feature
```
5. Open a pull reques

## 📄 License
Licensed under the GNU General Public License v3.0.

