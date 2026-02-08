# ✈️ Airline Reservation System

A modern, full-featured airline reservation web application built with ASP.NET Core MVC, Entity Framework Core, and SQL Server.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-239120?logo=c-sharp)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5-7952B3?logo=bootstrap)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoft-sql-server)

<!-- ## 📸 Screenshots

### Home Page
Modern landing page with hero section and feature highlights

### Flight Search
Powerful search functionality with origin, destination, and date filters

### Booking System
Intuitive seat selection and booking confirmation

### Admin Dashboard
Comprehensive admin panel for flight and booking management -->

## 🎯 Features

### User Features
- ✅ User registration and authentication
- ✅ Search flights by origin, destination, and date
- ✅ View detailed flight information
- ✅ Interactive seat selection
- ✅ Complete booking with passenger details
- ✅ View booking history
- ✅ Cancel bookings
- ✅ Booking confirmation with reference number
- ✅ Search booking by reference number

### Admin Features
- ✅ Comprehensive admin dashboard with statistics
- ✅ Create, edit, and delete flights
- ✅ Manage flight schedules
- ✅ View all bookings
- ✅ Update booking status
- ✅ Seat inventory management
- ✅ Real-time availability tracking

### Technical Features
- ✅ Role-based authorization (Admin/User)
- ✅ Responsive Bootstrap 5 UI
- ✅ Entity Framework Core with migrations
- ✅ Service layer architecture
- ✅ Transaction handling for bookings
- ✅ Data validation
- ✅ Error handling
- ✅ Database seeding
- ✅ Azure deployment ready

## 🛠️ Technology Stack

### Backend
- **Framework**: ASP.NET Core 10.0 MVC
- **Language**: C#
- **ORM**: Entity Framework Core 10.0
- **Database**: SQL Server
- **Authentication**: ASP.NET Identity

### Frontend
- **UI Framework**: Bootstrap 5
- **Icons**: Bootstrap Icons
- **Template Engine**: Razor Views
- **JavaScript**: Vanilla JS

### Cloud & Deployment
- **Hosting**: Azure App Service
- **Database**: Azure SQL Database
- **CI/CD**: Azure DevOps / GitHub Actions

## 📋 Prerequisites

- Visual Studio 2022
- .NET 10.0 SDK
- SQL Server (LocalDB/Express/Developer)
- Git (optional)

## 🚀 Quick Start

### 1. Clone or Download the Project

```bash
git clone https://github.com/yourusername/airline-reservation-system.git
cd airline-reservation-system
```

### 2. Open in Visual Studio

- Open `AirlineReservationSystem.sln` in Visual Studio 2022/2026

### 3. Restore NuGet Packages

```powershell
# In Package Manager Console
Update-Package -reinstall
```

### 4. Update Connection String

Edit `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AirlineReservationDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 5. Create Database

```powershell
# In Package Manager Console
Add-Migration InitialCreate
Update-Database
```

### 6. Run the Application

- Press `F5` or click `IIS Express`
- Application will open at `https://localhost:5001`

## 👤 Default Accounts

### Admin Account
- **Email**: admin@airline.com
- **Password**: Admin@123

### User Account
- **Email**: user@airline.com
- **Password**: User@123

## 📁 Project Structure

```
AirlineReservationSystem/
│
├── Controllers/              # MVC Controllers
│   ├── HomeController.cs
│   ├── AccountController.cs
│   ├── FlightsController.cs
│   ├── BookingsController.cs
│   └── AdminController.cs
│
├── Models/                   # Domain Models
│   ├── ApplicationUser.cs
│   ├── Flight.cs
│   ├── Seat.cs
│   ├── Booking.cs
│   └── ViewModels/
│
├── Views/                    # Razor Views
│   ├── Home/
│   ├── Account/
│   ├── Flights/
│   ├── Bookings/
│   ├── Admin/
│   └── Shared/
│
├── Data/                     # Data Layer
│   ├── ApplicationDbContext.cs
│   └── DbInitializer.cs
│
├── Services/                 # Business Logic
│   ├── FlightService.cs
│   └── BookingService.cs
│
├── wwwroot/                  # Static Files
│   ├── css/
│   ├── js/
│   └── images/
│
├── Program.cs                # Application Entry Point
├── appsettings.json         # Configuration
└── AirlineReservationSystem.csproj
```

## 🗃️ Database Schema

### Core Tables

**Users** (ASP.NET Identity)
- Id, Email, PasswordHash, Role, FirstName, LastName, etc.

**Flights**
- FlightId, FlightNumber, Origin, Destination, DepartureTime, ArrivalTime, Price, TotalSeats, AircraftType, IsActive

**Seats**
- SeatId, FlightId, SeatNumber, SeatClass, IsAvailable, IsWindowSeat, IsAisleSeat

**Bookings**
- BookingId, UserId, FlightId, SeatId, BookingDate, Status, BookingReference, TotalPrice, PassengerInfo

## 🎨 UI/UX Features

- Modern, clean interface
- Responsive design (mobile, tablet, desktop)
- Smooth animations and transitions
- Intuitive navigation
- Toast notifications
- Loading states
- Form validations
- Error handling with user-friendly messages

## 🔐 Security Features

- Password hashing (ASP.NET Identity)
- Role-based authorization
- CSRF protection
- SQL injection prevention (EF Core parameterized queries)
- XSS protection
- HTTPS enforcement
- Secure session management

## 🧪 Testing

### Manual Testing Checklist

- [ ] User registration
- [ ] User login/logout
- [ ] Flight search
- [ ] Flight details view
- [ ] Seat selection
- [ ] Booking creation
- [ ] Booking cancellation
- [ ] Admin login
- [ ] Flight CRUD operations
- [ ] Booking management

## 🌐 Deployment to Azure

### Step 1: Create Azure Resources

```bash
# Login to Azure
az login

# Create resource group
az group create --name AirlineReservationRG --location eastus

# Create SQL Server
az sql server create --name airline-reservation-sql --resource-group AirlineReservationRG --location eastus --admin-user sqladmin --admin-password YourPassword123!

# Create SQL Database
az sql db create --resource-group AirlineReservationRG --server airline-reservation-sql --name AirlineReservationDB --service-objective Basic

# Create App Service Plan
az appservice plan create --name airline-reservation-plan --resource-group AirlineReservationRG --sku F1

# Create Web App
az webapp create --name airline-reservation-app --resource-group AirlineReservationRG --plan airline-reservation-plan
```

### Step 2: Configure Connection String

```bash
az webapp config connection-string set --name airline-reservation-app --resource-group AirlineReservationRG --connection-string-type SQLAzure --settings DefaultConnection="Server=tcp:airline-reservation-sql.database.windows.net,1433;Database=AirlineReservationDB;User ID=sqladmin;Password=YourPassword123!;"
```

### Step 3: Publish from Visual Studio

1. Right-click project → Publish
2. Select Azure
3. Select Azure App Service (Windows)
4. Choose your app service
5. Publish

## 📚 Learning Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [Bootstrap 5](https://getbootstrap.com/docs/5.0)
- [Azure App Service](https://docs.microsoft.com/azure/app-service)

## 🚧 Future Enhancements

### Phase 2 (API Development)
- [ ] RESTful Web API
- [ ] JWT authentication
- [ ] Swagger/OpenAPI documentation
- [ ] API versioning

### Phase 3 (Advanced Features)
- [ ] Payment gateway integration (Stripe)
- [ ] Email notifications (SendGrid)
- [ ] SMS notifications (Twilio)
- [ ] Advanced seat map visualization
- [ ] Multi-city flights
- [ ] Round-trip bookings
- [ ] Price filters
- [ ] Seat class upgrades

### Phase 4 (Modern Frontend)
- [ ] React/Angular SPA
- [ ] Real-time updates (SignalR)
- [ ] Progressive Web App (PWA)
- [ ] Mobile applications (iOS/Android)

### Phase 5 (Enterprise Features)
- [ ] Microservices architecture
- [ ] Event sourcing
- [ ] CQRS pattern
- [ ] Redis caching
- [ ] Message queues (RabbitMQ/Azure Service Bus)
- [ ] AI-powered price predictions
- [ ] Loyalty program
- [ ] Advanced analytics

## 📊 Performance Considerations

- Database indexing on frequently queried columns
- Async/await for I/O operations
- Proper exception handling
- Connection pooling
- Query optimization
- Caching strategies

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 👨‍💻 Author

Teboho Mokgosi - [Your GitHub](https://github.com/SirTebz)

## 🙏 Acknowledgments

- ASP.NET Core Team
- Entity Framework Core Team
- Bootstrap Team
- Azure Team

## 📞 Support

For issues and questions:
- Create an issue on GitHub
- Email: your.email@example.com

---

**⭐ If you found this project helpful, please consider giving it a star!**

