# Company Management API

## Setup

### Prerequisites
- .NET 10
- PostgreSQL

### Connection String
appsettings.json-ում —
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=CompanyDb;Username=postgres;Password=yourpassword"
}
```

## Run

1. Clone the repo
2. Update connection string in `appsettings.json`
3. Run migrations:
```
Update-Database -Project Company.Infrastructure -StartupProject Company.Web
```
4. Run the project:
```
dotnet run --project Company.Web
```

## Seed Data
On startup automatically seeds:
- 2 Companies (Contoso, Fabrikam)
- 10 Employees per company
- 3 Projects per company
- Random Employee↔Project assignments

## JWT Credentials
| Email | Password | Role |
|-------|----------|------|
| admin@demo.com | Pass@123 | Admin |
| user@demo.com | Pass@123 | User |

## Endpoints

### Auth
| Method | URL | Description |
|--------|-----|-------------|
| POST | /api/auth/login | Get JWT token |

### Companies
| Method | URL | Description |
|--------|-----|-------------|
| GET | /api/company | Get all |
| GET | /api/company/paged | Get paged |
| GET | /api/company/{id} | Get by id |
| POST | /api/company | Create |
| PUT | /api/company/{id} | Update |
| DELETE | /api/company/{id} | Delete |

### Employees
| Method | URL | Description |
|--------|-----|-------------|
| GET | /api/employee | Get all |
| GET | /api/employee/paged | Get paged |
| GET | /api/employee/{id} | Get by id |
| POST | /api/employee | Create |
| PUT | /api/employee/{id} | Update |
| PUT | /api/employee/bulk | Bulk update |
| DELETE | /api/employee/{id} | Delete |

### Projects
| Method | URL | Description |
|--------|-----|-------------|
| GET | /api/project | Get all |
| GET | /api/project/paged | Get paged |
| GET | /api/project/{id} | Get by id |
| POST | /api/project | Create |
| PUT | /api/project/{id} | Update |
| DELETE | /api/project/{id} | Delete |

### Assignments
| Method | URL | Description |
|--------|-----|-------------|
| POST | /api/employees/{employeeId}/projects | Assign project |
| DELETE | /api/employees/{employeeId}/projects/{projectId} | Unassign |
| GET | /api/employees/{employeeId}/projects | Get employee projects |
| GET | /api/employees/{projectId}/employees | Get project employees |

## Swagger
```
http://localhost:{port}/swagger
```