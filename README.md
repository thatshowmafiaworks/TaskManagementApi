TaskManagementApi
====================================

Overview
--------
TaskManagementApi is a RESTful API for managing tasks (CRUD), built with ASP.NET Core, using:
- **Carter** for minimal API routing
- **MediatR** for CQRS pattern
- **Entity Framework Core** with PostgreSQL
- **ASP.NET Identity** for authentication and user management
- **JWT Bearer Authentication** for securing endpoints
- **Docker/Docker Compose** for containerization

This README contains:
- Setup instructions (local + Docker)
- Detailed API documentation (endpoints, JSON payloads, response formats)
- Explanation of architecture & design choices

Prerequisites
-------------
- [.NET SDK 8.0](https://dotnet.microsoft.com/)
- PostgreSQL (or use Docker for DB)
- Docker & Docker Compose (optional, recommended)
- Git
- Postman/curl for testing

Clone repository
----------------
```bash
git clone https://github.com/thatshowmafiaworks/TaskManagementApi.git
cd TaskManagementApi
```

Running locally
---------------
1. Configure database connection in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "Database": "Server=localhost;Port=5432;Database=TaskManagementDB;User Id=postgres;Password=postgres;Include Error Detail=true"
   },
   "JWT": {
    "Audience": "TaskManagementApi",
    "Issuer": "TaskManagementApi",
    "SigninKey": "[your secret sign in key]"
   }
   ```

2. Apply EF migrations
   ```bash
   dotnet ef database update --project ./TaskManagementApi/TaskManagementApi.csproj
   ```

3. Run API:
   ```bash
   dotnet run --project ./TaskManagementApi/TaskManagementApi.csproj
   ```

4. API available at:
   - http://localhost:5125
   - https://localhost:7297

Running with Docker Compose
---------------------------
1. Ensure Docker is installed and running.
2. From repo root:
   ```bash
   docker compose up --build
   ```
3. This will start API + PostgreSQL (check `docker-compose.yml`).

Authentication
--------------
- All task endpoints except **"/users"** require **JWT Bearer Authorization** (`Authorization: Bearer <token>`).
- JWT settings (`JWT:Issuer`, `JWT:Audience`, `JWT:SigninKey`) are configured in `appsettings.json`.

API Documentation
-----------------
Below are the implemented endpoints, based on endpoint classes.

### 1. Create Task
**POST** `/tasks`  
Requires authentication.

**Request body:**
```json
{
  "title": "Implement README",
  "description": "Detailed task description",
  "dueDate": "2025-09-30T12:00:00Z",
  "status": "Pending",
  "priority": "High"
}
```

**Response 200 OK:**
```json
{
  "id": "b3f7d7cb-5c26-4c47-9f4c-4cf87fbb4aa0",
  "isSuccess": true,
  "error": null
}
```

---

### 2. Get All Tasks (by current user)
**GET** `/tasks`  
Requires authentication. Returns tasks for the currently logged-in user.

**Query parameters:**
- `pageNumber` (int, optional, default 1)
- `pageSize` (int, optional, default 10)
- `status` (enum: Pending, InProgress, Completed)
- `priority` (enum: Low, Medium, High)
- `orderByPriority` (bool)

**Response 200 OK:**
```json
{
  "tasks": [
    {
      "id": "guid",
      "title": "string",
      "description": "string",
      "dueDate": "2025-09-30T12:00:00Z",
      "status": "Pending",
      "priority": "High",
      "userId": "guid"
    }
  ]
}
```

---

### 3. Get All Tasks (all users)
**GET** `/tasks/all`  
Requires authentication. Returns all tasks in the system.

**Query parameters:**
- `pageNumber`
- `pageSize`
- `status`
- `priority`

**Response 200 OK:**
```json
{
  "tasks": [ { /* same Task model */ } ]
}
```

---

### 4. Get Task by ID
**GET** `/tasks/{id}`  
Requires authentication. Returns single task if it belongs to current user.

**Response 200 OK:**
```json
{
  "task": {
    "id": "guid",
    "title": "string",
    "description": "string",
    "dueDate": "2025-09-30T12:00:00Z",
    "status": "Pending",
    "priority": "High",
    "userId": "guid"
  }
}
```

**Response 403 Forbidden:** if task does not belong to user.

---

### 5. Update Task
**PUT** `/tasks/{id}`  
Requires authentication.

**Request body (all fields optional):**
```json
{
  "title": "Updated title",
  "description": "Updated description",
  "dueDate": "2025-10-01T09:00:00Z",
  "status": "InProgress",
  "priority": "Medium"
}
```

**Response 200 OK:**
```json
{
  "isSuccess": true,
  "error": null
}
```

---

### 6. Delete Task
**DELETE** `/tasks/{id}`  
Requires authentication.

**Response 200 OK:**
```json
{
  "isSuccess": true,
  "error": null
}
```

---

HTTP Status Codes
-----------------
- `200 OK` — success (GET, PUT, DELETE, POST)
- `400 Bad Request` — invalid input
- `401 Unauthorized` — no/invalid token
- `403 Forbidden` — user not allowed to access task
- `500 Internal Server Error` — unhandled errors

Architecture & Design
---------------------
- **Carter**: lightweight module system for routing, instead of controllers.
- **CQRS + MediatR**: separation of commands and queries (`CreateTaskCommand`, `GetTaskByIdQuery` etc.), handlers encapsulate business logic.
- **Entity Framework Core + PostgreSQL**: persistence layer, migrations for schema evolution.
- **ASP.NET Identity**: user & role management with EF store.
- **JWT Authentication**: stateless authentication for APIs.
- **Exception Handling Middleware**: centralized error handling with `CustomExceptionHandling`.
- **Docker & Docker Compose**: containerized environment for API + DB.

-------
###### Readme written by ChatGPT, reviewed by author:)

