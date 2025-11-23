# Educational System API

## Overview
Educational System is a comprehensive backend Web API designed for an online learning platform. Built with **.NET 9**, it provides robust features for managing courses, instructors, students, enrollments, and learning progress. The application utilizes modern architecture patterns and best practices, including N-Layer architecture, Repository Pattern, and JWT Authentication.

## Features
- **User Management**:
  - Secure Authentication & Authorization using **ASP.NET Core Identity**.
  - **JWT Bearer** token-based authentication.
  - Role-based access control (Admin, Instructor, Student).
  - Automatic seeding of default users and roles.

- **Course Management**:
  - CRUD operations for Courses, Categories, and Specializations.
  - Instructor assignment and management.
  - Discount management for courses.

- **Content Delivery**:
  - Hierarchical content structure: Courses -> Lessons -> SubLessons.
  - Support for various question types and assessments (quizzes, assignments).

- **Student Engagement**:
  - Course Enrollment system.
  - Progress tracking (Lesson completion, Prerequisites).
  - Submissions (File and Text) for assignments.

- **Performance & Infrastructure**:
  - **Redis** caching for high-performance data retrieval.
  - **Serilog** for structured logging (Console and File).
  - **Entity Framework Core** with SQL Server.
  - **AutoMapper** for efficient object mapping.
  - **MailKit** for email services.
  - **Swagger UI** for API documentation and testing.

## Technology Stack
- **Framework**: .NET 9.0 Web API
- **Language**: C#
- **Database**: SQL Server
- **ORM**: Entity Framework Core 8
- **Caching**: Redis (StackExchange.Redis)
- **Logging**: Serilog
- **Mapping**: AutoMapper
- **Documentation**: Swashbuckle (Swagger)
- **Email**: MailKit

## Architecture
The solution follows a clean **N-Layer Architecture**:
1. **Educational_System (API)**: The entry point, containing Controllers, DTOs, and Configuration.
2. **EducationalSystem.BLL (Business Logic Layer)**: Contains Services, Repositories, and business rules.
3. **EducationalSystem.DAL (Data Access Layer)**: Contains Database Context, Entity Models, and Migrations.

## Prerequisites
Before running the application, ensure you have the following installed:
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB or Full instance)
- [Redis](https://redis.io/download) (Running on `localhost:6379`)

## Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/Educational_System.git
cd Educational_System
```

### 2. Configure Database
Update the connection string in `Educational_System/appsettings.json` (or `appsettings.Development.json`) to point to your SQL Server instance.

```json
"ConnectionStrings": {
  "DB1": "Server=.;Database=EducationalSystemDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3. Configure Redis
Ensure Redis is running locally on the default port (`6379`). If your Redis setup is different, update the configuration in `Program.cs`:

```csharp
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
});
```

### 4. Apply Migrations & Seed Data
The application is configured to automatically apply migrations and seed data on startup.
You can also manually update the database using the CLI:

```bash
cd Educational_System
dotnet ef database update --project ../EducationalSystem.DAL
```

### 5. Run the Application
```bash
dotnet run
```
The API will start, and you can access the Swagger UI at:
- `http://localhost:5088/swagger/index.html` (or the port configured in launchSettings.json)

## API Documentation
The API is fully documented using Swagger. Once the application is running, navigate to `/swagger` to explore the endpoints, test requests, and view the data models.

## Project Structure
```
Educational_System/
├── EducationalSystem.BLL/       # Business Logic (Repositories, Interfaces)
├── EducationalSystem.DAL/       # Data Access (DbContext, Models, Migrations)
├── Educational_System/          # Web API (Controllers, DTOs, Program.cs)
├── Educational_System.sln       # Solution File
└── README.md                    # Project Documentation
```

## Contributing
Contributions are welcome! Please fork the repository and submit a Pull Request.

## License
[MIT](LICENSE)
