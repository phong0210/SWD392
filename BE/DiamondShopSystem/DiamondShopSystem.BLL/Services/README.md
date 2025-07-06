# Services Layer - Clean Architecture Structure

This folder contains the business logic services organized by domain following Clean Architecture principles.

## 📁 Folder Structure

```
Services/
├── Auth/
│   ├── IAuthService.cs
│   ├── AuthService.cs
│   └── JwtUtil.cs
└── User/
    ├── IUserService.cs
    └── UserService.cs
```

## 🏗️ Architecture Components

### **Auth Services** (Authentication Domain)
- **IAuthService**: Interface for authentication operations
- **AuthService**: Implementation of authentication business logic
- **JwtUtil**: JWT token generation and management

### **User Services** (User Domain)
- **IUserService**: Interface for user operations
- **UserService**: Implementation of user business logic

## 🔄 Service Responsibilities

### **AuthService**
- Password hashing and validation
- JWT token generation
- Authentication logic orchestration

### **UserService**
- User data retrieval
- User role management
- User account information processing

## 📋 Benefits

- ✅ **Domain Separation**: Services are organized by business domain
- ✅ **Interface Segregation**: Each service has its own interface
- ✅ **Dependency Injection**: Proper DI container registration
- ✅ **Testability**: Services can be easily mocked and tested
- ✅ **Maintainability**: Clear separation of business logic
- ✅ **Scalability**: Easy to add new services for new domains

## 🔧 Usage

Services are registered in `Program.cs` and injected into command handlers:

```csharp
// Registration
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

// Usage in Command Handlers
public class SomeCommandHandler
{
    private readonly IUserService _userService;
    
    public SomeCommandHandler(IUserService userService)
    {
        _userService = userService;
    }
}
``` 