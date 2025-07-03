# Developer Onboarding Guide: Implementing Services & APIs

> **Important:** All backend code must be placed in the correct subfolders:
> - **BLL:** C:/Users/syrex/Desktop/SWD392/BE/DiamondShopSystem/DiamondShopSystem.BLL
> - **API:** C:/Users/syrex/Desktop/SWD392/BE/DiamondShopSystem/DiamondShopSystem.API
> - **DAL:** C:/Users/syrex/Desktop/SWD392/BE/DiamondShopSystem/DiamondShopSystem.DAL
> Always use these paths for new files and features.

Welcome to the Diamond Shop System codebase! This guide will help you get started with implementing new services and APIs, both in the backend (C#/.NET) and frontend (React/TypeScript).

---

## 1. Project Structure Overview

- **Backend (BE):**
  - `BE/DiamondShopSystem/DiamondShopSystem.API/` — API controllers (entry point for HTTP requests)
  - `BE/DiamondShopSystem/DiamondShopSystem.BLL/` — Business logic, handlers, DTOs, services
  - `BE/DiamondShopSystem/DiamondShopSystem.DAL/` — Data access, entities, repositories, migrations
- **Frontend (FE):**
  - `FE/src/services/` — API service files (one per resource)
  - `FE/src/pages/` — UI pages/components

---

## 2. Backend: Adding a New API/Service

The backend follows Domain-Driven Design (DDD) and clean architecture principles. Here's how to add a new resource (e.g., `Product`):

### Step 1: Define the Entity (DAL)
- Add a new class in `DAL/Entities/` (e.g., `Product.cs`).
- Use data annotations for validation and relationships.

### Step 2: Repository & Unit of Work
- Use the generic repository pattern (`IGenericRepository<T>`, `GenericRepository<T>`).
- Register your entity in the `UnitOfWork` if it's an aggregate root.

### Step 3: Business Logic Layer (BLL)
- Create DTOs (Data Transfer Objects) in `BLL/Handlers/` (e.g., `ProductCreateDto.cs`).
- Implement business logic, validation, and handlers as needed.
- Use AutoMapper profiles for entity-DTO mapping (`BLL/Mapping/`).

### Step 4: API Controller (API)
- Add a controller in `API/Controllers/` (e.g., `ProductsController.cs`).
- Use `[ApiController]` and `[Route("api/[controller]")]` attributes.
- Inject services via constructor.
- Implement RESTful endpoints: `GET`, `POST`, `PUT`, `DELETE`.

### Step 5: DTOs & AutoMapper
- Map between entities and DTOs using AutoMapper.
- Register new profiles in `Program.cs` if needed.

#### Example: Adding a Product
- See `Product.cs` (entity), `productAPI.ts` (frontend service), and related controller for reference.

---

## 3. Frontend: Adding a New API Service

### Step 1: Create a Service File
- Add a new file in `src/services/` (e.g., `productAPI.ts`).
- Use the shared `apiCaller.ts` for HTTP requests.
- Example:
  ```ts
  import { get, post, put, remove } from "./apiCaller";
  export const showAllProduct = () => get(`/api/products`);
  export const createProduct = (product: object) => post(`/api/products`, product);
  ```

### Step 2: Connect to a Page/Component
- Import your service in the relevant page/component (e.g., `ProductData.tsx`).
- Call the service in React hooks or event handlers.
- Example:
  ```ts
  import { showAllProduct } from '@/services/productAPI';
  useEffect(() => {
    showAllProduct().then(...);
  }, []);
  ```

---

## 4. Best Practices & Tips
- **Backend:**
  - Follow DDD and repository patterns.
  - Use DTOs for API input/output.
  - Keep controllers thin; put logic in BLL/services.
  - Use AutoMapper for mapping.
  - Write unit tests for business logic.
- **Frontend:**
  - Keep API logic in `src/services/`.
  - Use async/await for API calls.
  - Handle loading and error states in UI.
  - Reuse service functions across components.

---

## 5. References
- [BE-FE-API-Endpoint-Map.md](./BE-FE-API-Endpoint-Map.md): List of all API endpoints.
- [FE-BE-Integration-Analysis.md](./FE-BE-Integration-Analysis.md): Architecture and integration details.
- `contexts/domain-model.md`, `contexts/repository-patterns.md`: Domain and data access patterns.

---

## 6. Required Patterns & Technologies for All Features

When implementing any new feature (such as login, registration, etc.), you **must strictly follow** these architectural patterns and technologies already established in the codebase:

- **AutoMapper**: Use for mapping between domain entities and DTOs. Define mapping profiles in `BLL/Mapping/`.
- **FluentValidation**: Use for validating all incoming requests. Place validators in `BLL/Handlers/` or a dedicated `Validators/` folder.
- **MediatR**: Use the CQRS pattern. All business logic should be implemented as MediatR commands/queries and handlers, not in controllers.
- **Generic Repository & Unit of Work**: Access data only through the repository and unit of work abstractions, never directly via DbContext in handlers or controllers.
- **Dependency Injection (DI)**: Register all services, handlers, validators, and mapping profiles in the DI container (see `Program.cs`).

### Step-by-Step Checklist for Implementing a New Feature

1. **Define DTOs**
   - Create request and response DTOs for your feature in the BLL folder: `DiamondShopSystem.BLL/Handlers/`.
2. **Create Validators**
   - Implement a FluentValidation validator for the request DTO in the BLL folder: `DiamondShopSystem.BLL/Handlers/` or a dedicated `Validators/` folder.
3. **Set Up MediatR Command/Query**
   - Define a command or query (e.g., `LoginCommand : IRequest<LoginResponseDto>`) in the BLL folder: `DiamondShopSystem.BLL/Handlers/`.
   - Implement the handler, injecting repositories, services, and AutoMapper as needed.
4. **Use Generic Repository & Unit of Work**
   - Access data via `IUnitOfWork` and `IGenericRepository<T>` in the DAL folder: `DiamondShopSystem.DAL/Repositories/`.
5. **Map Entities to DTOs**
   - Use AutoMapper in your handler to map entities to DTOs. Place mapping profiles in `DiamondShopSystem.BLL/Mapping/`.
6. **Controller**
   - The controller should only receive the request, send it to MediatR, and return the result. Place controllers in the API folder: `DiamondShopSystem.API/Controllers/`.
7. **Register Everything in DI**
   - Ensure all new handlers, validators, and mapping profiles are registered in `DiamondShopSystem.API/Program.cs`.

> **Strict Rule:** All new features must adhere to this design and folder structure. This ensures maintainability, testability, and consistency across the codebase.

---

## Recommended Structure: Organize Handlers by Feature

For easier maintenance and scalability, **separate your Handlers, DTOs, Validators, and Commands into subfolders by feature** (e.g., Auth, Product, Order) under the `Handlers/` directory.

### Example: Updated File/Folder Map for Login Feature

```
BE/
  DiamondShopSystem/
    DiamondShopSystem.BLL/
      Handlers/
        Auth/
          LoginRequestDto.cs
          LoginResponseDto.cs
          LoginRequestValidator.cs
          LoginCommand.cs
          LoginCommandHandler.cs
      Mapping/
        EntityToDtoProfile.cs (add mapping if needed)
    DiamondShopSystem.API/
      Controllers/
        AuthController.cs
    DiamondShopSystem.DAL/
      Repositories/
        (use IUnitOfWork, IGenericRepository for data access)
      Entities/
        User.cs
```

- **Each feature (e.g., Auth, Product, Order) gets its own subfolder in `Handlers/`.**
- **Mapping Profiles:** Go in `DiamondShopSystem.BLL/Mapping/`
- **Controllers:** Go in `DiamondShopSystem.API/Controllers/`
- **Repositories/Entities:** Go in `DiamondShopSystem.DAL/`

---

## Step-by-Step Guide: Implementing a New Feature (e.g., Login)

Follow these steps **exactly** for every new backend feature:

### 1. **Define Request and Response DTOs**
- **What:** Create classes for the request and response payloads (e.g., `LoginRequestDto`, `LoginResponseDto`).
- **Where:** `BE/DiamondShopSystem/DiamondShopSystem.BLL/Handlers/<Feature>/`
- **How:**
  - Request DTO: Properties for all input fields.
  - Response DTO: Properties for all output fields (e.g., token, user info).

### 2. **Create a FluentValidation Validator**
- **What:** Implement a validator for the request DTO (e.g., `LoginRequestValidator`).
- **Where:** `BE/DiamondShopSystem/DiamondShopSystem.BLL/Handlers/<Feature>/` (or a `Validators/` subfolder within the feature)
- **How:**
  - Use FluentValidation rules for all required fields and formats.
- **Avoid:** Do not put validation logic in the controller or handler.

### 3. **Set Up MediatR Command and Handler**
- **What:**
  - Command (e.g., `LoginCommand`) encapsulates the request DTO.
  - Handler (e.g., `LoginCommandHandler`) contains the business logic.
- **Where:** `BE/DiamondShopSystem/DiamondShopSystem.BLL/Handlers/<Feature>/`
- **How:**
  - Handler should:
    - Use `IUnitOfWork` and `IGenericRepository` for data access (never use DbContext directly).
    - Validate credentials/business rules.
    - Use AutoMapper to map entities to response DTOs.
    - Generate tokens or perform other business logic.
- **Avoid:** No direct database access or business logic in the controller.

### 4. **Add/Update AutoMapper Profile**
- **What:** Add mapping between entities and DTOs (e.g., `User` to `LoginResponseDto`).
- **Where:** `BE/DiamondShopSystem/DiamondShopSystem.BLL/Mapping/EntityToDtoProfile.cs`
- **How:**
  - Use `CreateMap<Source, Destination>()`.
  - Ignore properties that are set manually (e.g., token).

### 5. **Implement the API Controller**
- **What:** Add a controller (e.g., `AuthController`) with endpoints for your feature.
- **Where:** `BE/DiamondShopSystem/DiamondShopSystem.API/Controllers/`
- **How:**
  - Accept only DTOs as input/output.
  - Use MediatR to send commands/queries.
  - Return results from handlers.
- **Avoid:** No business logic, validation, or mapping in the controller.

### 6. **Register Everything in Dependency Injection (DI)**
- **What:** Ensure all handlers, validators, and mapping profiles are registered.
- **Where:** `BE/DiamondShopSystem/DiamondShopSystem.API/Program.cs`
- **How:**
  - Use assembly scanning for MediatR and AutoMapper.
  - Register validators in your custom registration method.

### 7. **Test the Feature**
- **What:** Test the endpoint using Swagger, Postman, or integration tests.
- **How:**
  - Validate that validation, business logic, and mapping all work as expected.

### 8. **Follow Folder Structure Strictly**
- **Always place files in the correct subfolders:**
  - **BLL:** Handlers (by feature), DTOs, Validators, Mapping
  - **API:** Controllers
  - **DAL:** Repositories, Entities

### 9. **General Rules**
- **No business logic in controllers.**
- **No direct DbContext access outside repositories.**
- **All mapping must be done in handlers using AutoMapper.**
- **All validation must use FluentValidation.**
- **All business logic must be in MediatR handlers.**

> **If in doubt, refer to the file/folder map and checklist above.**

---

For further questions, check the codebase, read the docs, or ask a team member. Happy coding! 