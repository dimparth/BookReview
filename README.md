# 📚 BookReview – ASP.NET Core MVC & API Application

BookReview is a hybrid web application built with **ASP.NET Core 9**, combining both **MVC** and **REST API** architectures.
It allows users to browse, review, and vote on books while demonstrating clean architecture, domain-driven design, and best practices in testing and maintainability.

---

## ✨ Features

### 🔐 Authentication
- User registration & login using **ASP.NET Identity**
- MVC pages are secured for authenticated users
- API endpoints are open by design

### 📖 Book Management
- Browse all books with support for:
  - Filtering by title, author, genre, and average rating
  - Sorting by title, author, year, or rating
- View book details including all associated reviews

### 📝 Reviews
- Authenticated users can:
  - Add, edit, or delete reviews for a book
  - Rate books on a 1–5 scale
- Each review belongs to a user and a book

### 👍 Voting System
- Users can upvote/downvote reviews once
- Users can change their vote

### 🧪 Testing
- Unit tests written with **xUnit** and **NSubstitute**
- All services are fully tested, including CRUD and business logic
- Shared test setup using a custom `ContainerFixture` with DI and mocks

---

## 🧱 Architecture

- **Domain-Driven Design (DDD)** with factory methods and guard clauses
- **Service Layer**: business logic is encapsulated in services inheriting from a reusable `BaseService`
- **Repository & Unit of Work Patterns**
- **MVC + API**: separation of concerns between UI and data endpoints
- **Dependency Injection**: all services, repositories, and infrastructure components are injected

---

## 🚀 Tech Stack

| Technology          | Purpose                                 |
|---------------------|------------------------------------------|
| ASP.NET Core 9      | Backend framework                        |
| Entity Framework    | Data access                              |
| ASP.NET Identity    | Authentication & user management         |
| SQLite              | Lightweight database (dev environment)   |
| xUnit               | Unit testing framework                   |
| NSubstitute         | Mocking dependencies in tests            |
