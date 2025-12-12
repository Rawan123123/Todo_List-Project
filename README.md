<img width="733" height="866" alt="image" src="https://github.com/user-attachments/assets/196ba781-5d61-431a-8564-6026cd6fba7f" /># 📝 Todo List API - ASP.NET Core

A professional RESTful API for managing personal todo lists with complete authentication system, advanced filtering, and comprehensive task management features.
---

## 📖 Project Overview

This is my **first full-stack API project** built with ASP.NET Core, demonstrating modern backend development practices including secure authentication, clean architecture, and RESTful API design principles.

### 🎯 Project Goals
- Build a secure and scalable todo list management system
- Implement JWT-based authentication and authorization
- Apply clean code principles and SOLID design patterns
- Handle errors gracefully with custom middleware
- Provide advanced features like pagination, sorting, and filtering

---

## ✨ Core Features

### 🔐 Authentication & Security
| Feature | Description | Status |
|---------|-------------|--------|
| **User Registration** | Register new users with email validation | ✅ |
| **User Login** | Authenticate users and generate JWT tokens | ✅ |
| **Password Hashing** | Secure password storage using BCrypt | ✅ |
| **JWT Authentication** | Token-based authentication for API endpoints | ✅ |
| **Authorization** | User-specific data isolation (users only see their todos) | ✅ |
| **Token Claims** | Store user ID, email, and role in JWT | ✅ |

### 📋 Todo Management
| Feature | Description | Status |
|---------|-------------|--------|
| **Create Todo** | Add new tasks with title, description, and priority | ✅ |
| **Get All Todos** | Retrieve all user's todos with pagination | ✅ |
| **Get Todo by ID** | Retrieve specific todo details | ✅ |
| **Update Todo** | Modify existing todo information | ✅ |
| **Delete Todo** | Remove todos from the system | ✅ |
| **Mark as Completed** | Toggle todo completion status | ✅ |
| **Priority Levels** | Assign Low/Medium/High priority to tasks | ✅ |
| **Timestamps** | Automatic creation date tracking | ✅ |

### 🔍 Advanced Filtering & Search
| Feature | Description | Status |
|---------|-------------|--------|
| **Pagination** | Navigate through todos with page and pageSize parameters | ✅ |
| **Multi-field Sorting** | Sort by priority, name, status, or creation date | ✅ |
| **Ascending/Descending** | Control sort order (ASC/DESC) | ✅ |
| **Search by Name** | Find todos containing specific text | ✅ |
| **Filter by Status** | Filter completed or pending todos | ✅ |
| **Filter by Priority** | Filter by Low/Medium/High priority | ✅ |
| **Combined Filters** | Use multiple filters simultaneously | ✅ |

### 📊 Statistics & Analytics
| Feature | Description | Status |
|---------|-------------|--------|
| **Total Tasks** | Count all user's tasks | ✅ |
| **Completed Tasks** | Count finished tasks | ✅ |
| **Pending Tasks** | Count unfinished tasks | ✅ |
| **Priority Breakdown** | Tasks count by priority level | ✅ |
| **Completion Ratio** | Percentage of completed tasks | ✅ |

### 🛡️ Error Handling & Validation
| Feature | Description | Status |
|---------|-------------|--------|
| **Global Exception Handler** | Centralized error handling middleware | ✅ |
| **Custom Exceptions** | Specific exception types (NotFound, BadRequest, etc.) | ✅ |
| **Data Validation** | Input validation using Data Annotations | ✅ |
| **Detailed Error Messages** | Structured error responses with status codes | ✅ |
| **Validation Errors** | Return field-specific validation errors | ✅ |

---

## 🏗️ Technical Architecture

### Technology Stack
- **Framework**: ASP.NET Core 8.0
- **Language**: C# 12.0
- **ORM**: Entity Framework Core
- **Database**: SQL Server
- **Authentication**: JWT (JSON Web Tokens)
- **Password Hashing**: BCrypt.Net
- **Architecture**: Clean Architecture / Layered Architecture

### ![Project Structure]()<img width="733" height="866" alt="image" src="https://github.com/user-attachments/assets/80811805-39cf-4fad-93c9-a751bbf6af02" />

---

