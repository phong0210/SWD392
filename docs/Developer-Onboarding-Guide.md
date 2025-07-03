# Developer Onboarding Guide: Implementing Services & APIs

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
   - Create request and response DTOs for your feature.
2. **Create Validators**
   - Implement a FluentValidation validator for the request DTO.
3. **Set Up MediatR Command/Query**
   - Define a command or query (e.g., `LoginCommand : IRequest<LoginResponseDto>`).
   - Implement the handler, injecting repositories, services, and AutoMapper as needed.
4. **Use Generic Repository & Unit of Work**
   - Access data via `IUnitOfWork` and `IGenericRepository<T>`.
5. **Map Entities to DTOs**
   - Use AutoMapper in your handler to map entities to DTOs.
6. **Controller**
   - The controller should only receive the request, send it to MediatR, and return the result. No business logic in controllers.
7. **Register Everything in DI**
   - Ensure all new handlers, validators, and mapping profiles are registered in `Program.cs`.

> **Strict Rule:** All new features must adhere to this design. This ensures maintainability, testability, and consistency across the codebase.

---

For further questions, check the codebase, read the docs, or ask a team member. Happy coding! 