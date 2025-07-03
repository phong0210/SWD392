# Frontend & Backend Integration Analysis

## Backend Architecture Overview (DDD)

The backend is structured using Domain-Driven Design (DDD) principles, with a clear separation of concerns across multiple layers:

- **API Layer:**
  - Exposes RESTful endpoints via controllers (e.g., ProductsController, UsersController).
  - Handles HTTP requests, validation, and response formatting.

- **Business Logic Layer (BLL):**
  - Contains application logic, DTOs, services, and feature-specific code.
  - Implements business rules and orchestrates domain operations.
  - Uses AutoMapper to map between domain entities and DTOs for clean separation.

- **Data Access Layer (DAL):**
  - Responsible for database access and persistence.
  - Implements the **Generic Repository** pattern for reusable CRUD operations.
  - Uses the **Unit of Work (UoW)** pattern to manage transactions and coordinate changes across multiple repositories.

- **Supporting Patterns & Tools:**
  - **AutoMapper:** For mapping between domain models and DTOs.
  - **Repository Pattern:** For abstracting data access logic.
  - **Unit of Work:** For transaction management and consistency.

This architecture ensures maintainability, testability, and scalability by enforcing clear boundaries between layers and responsibilities.

---

## 1. Codebase Structure Overview

### Backend (BE)
- **Location:** `BE/DiamondShopSystem/DiamondShopSystem.API/`
- **Controllers:**
  - `ProductsController.cs`, `UsersController.cs`, `OrdersController.cs`, `DeliveriesController.cs`, `AuthController.cs`, etc.
- **API Structure:**
  - RESTful endpoints, mostly under `/api/{resource}`
  - Uses attribute routing (e.g., `[Route("api/[controller]")]`, `[HttpGet]`, `[HttpPost]`, etc.)

### Frontend (FE)
- **Location:** `FE/`
- **API Service Files:**
  - `src/services/` contains files like `authAPI.ts`, `productAPI.ts`, `orderAPI.ts`, etc.
- **API Calls:**
  - Use a shared `apiCaller.ts` (axios wrapper)
  - Endpoints are relative (e.g., `/api/products`, `/voucher/showAll`)

---

## 2. API Endpoints Called by the Frontend

### Example (grouped by resource):
- **Products:**
  - `GET /api/products`
  - `GET /api/products/{id}`
  - `POST /api/products`
  - `PUT /api/products/{id}`
  - `DELETE /api/products/{id}`
- **Users/Accounts:**
  - `POST /api/auth/login`
  - `POST /api/users/register`
  - `GET /api/users`
  - `PUT /api/users/{email}`
  - `DELETE /api/users/{id}`
- **Orders:**
  - `GET /api/orders`
  - `GET /api/orders/{id}`
  - `POST /api/orders`
  - `PUT /api/orders/{id}`
  - `DELETE /api/orders/{id}`
- **Other Resources:**
  - Vouchers, Sizes, Materials, Jewelry, Feedback, etc. (see full list in services directory)

---

## 3. API Endpoints Implemented in the Backend

### Example (grouped by controller):
- **ProductsController**
  - `GET /api/products`
  - `GET /api/products/{id}`
  - `POST /api/products`
  - `PUT /api/products/{id}`
  - `PUT /api/products/{id}/hide`
  - `PUT /api/products/{id}/inventory`
  - `PUT /api/products/{id}/pricing`
- **UsersController**
  - `POST /api/users/register`
  - `GET /api/users/{id}`
  - `PUT /api/users/profile`
  - `PUT /api/users/{id}/manage`
- **AuthController**
  - `POST /api/auth/login`
  - `POST /api/auth/google-login`
  - `POST /api/auth/forgot-password`
  - `POST /api/auth/reset-password`
- **OrdersController**
  - `GET /api/orders`
  - `GET /api/orders/{id}`
  - `POST /api/orders`
  - `PUT /api/orders/{id}/status`
  - etc.

---

## 4. Comparison: FE Calls vs BE Endpoints

### Example Table:
| FE Endpoint                | BE Endpoint                | Match? | Notes (Input/Output Differences) |
|----------------------------|----------------------------|--------|----------------------------------|
| GET /api/products          | GET /api/products          | Yes    | Check response fields            |
| GET /api/users             | GET /api/users/{id}        | No     | FE expects all users, BE only by id |
| PUT /api/users/{email}     | PUT /api/users/profile     | Partial| BE expects profile update, not by email |
| DELETE /api/users/{id}     | (missing)                  | No     | BE may not have delete endpoint   |
| ...                        | ...                        | ...    | ...                              |

---

## 5. Differences & Mismatches
- **Missing endpoints:** Some FE calls (e.g., `GET /api/users`) do not have a direct BE match.
- **Field mismatches:** FE may use `email` as identifier, BE may use `id` or expect profile object.
- **Extra/missing fields:** Check request/response payloads for each endpoint.

---

## 6. Logical Mappings & Adapter Suggestions
- **Reuse:** Where endpoints are similar, map FE calls to BE endpoints with minimal changes (e.g., adapt FE to use `id` instead of `email`).
- **Adapters:** Build FE-side or BE-side mappers to translate between formats if needed.
- **Recommend BE changes:**
  - Add missing endpoints (e.g., `GET /api/users` for all users)
  - Add aliases or accept both `id` and `email` as identifiers
  - Normalize response formats to match FE expectations

---

## 7. Recommendations
- **Prefer backend changes** if FE is stable and widely used.
- **Add/adjust endpoints** to cover FE needs with minimal disruption.
- **Document all mappings and changes** for future maintainers.

---

## 8. Next Steps
- Review each endpoint in detail (request/response payloads)
- Implement adapters or BE changes as needed
- Test integration thoroughly

---

*This document is a starting point. For a full integration, each endpoint and data structure should be reviewed in detail as described above.* 