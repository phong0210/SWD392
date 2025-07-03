# Frontend to Backend Migration Plan

## Phase 1: Authentication & User Management

### ✅ Already Matched
- `POST /auth/signin` (FE) → `POST /api/auth/login` (BE)
- `POST /auth/signup` (FE) → `POST /api/users/register` (BE)
- `GET /auth/ShowAllAccounts` (FE) → `GET /api/users` (BE)

### 🔧 Needs Frontend Updates
1. **Update endpoint paths in `authAPI.ts`:**
   ```typescript
   // Change from:
   login: /auth/signin → /api/auth/login
   register: /auth/signup → /api/users/register
   showAllAccounts: /auth/ShowAllAccounts → /api/users
   ```

2. **Update request/response models:**
   - Login: Ensure frontend sends `{ email, password }` and expects JWT token
   - Register: Update to match `RegisterUserCommand` (fullName, email, phone, password)

### ⚠️ Missing Backend Implementation
- `POST /auth/signupCustomer` (FE) - **TODO: Implement customer-specific registration**
- `PUT /auth/update/{name}` (FE) - **Map to:** `PUT /api/users/{userId}` (change from name to userId)
- `DELETE /auth/delete/{id}` (FE) - **Map to:** `DELETE /api/users/{userId}`

### 📝 Action Items:
1. Update frontend endpoint URLs
2. Implement missing backend endpoints
3. Add Google OAuth frontend integration for `POST /api/auth/google-login`
4. Add forgot/reset password UI for existing backend endpoints

---

## Phase 2: Account Management

### 🔧 Needs Cleanup
The `accountApi.ts` file has duplicate functions with `authAPI.ts`. 

### 📝 Action Items:
1. **Remove duplicates** from `accountApi.ts`
2. **Keep these functions and update endpoints:**
   ```typescript
   getCustomer: /auth/getCustomer/{id} → /api/users/{userId}
   getAccountDetail: /auth/detailAccount/{id} → /api/users/{userId}
   ```

---

## Phase 3: Product Management

### ✅ Already Matched
- `GET /product/showAll` (FE) → `GET /api/products` (BE)
- `GET /product/detail/{id}` (FE) → `GET /api/products/{id}` (BE)
- `POST /product/create` (FE) → `POST /api/products` (BE)

### 🔧 Needs Frontend Updates
1. **Update endpoint paths in `productAPI.ts`:**
   ```typescript
   showAllProduct: /product/showAll → /api/products
   getProductDetails: /product/detail/{id} → /api/products/{id}
   createProduct: /product/create → /api/products
   ```

2. **Add missing frontend functions:**
   ```typescript
   updateProduct: → PUT /api/products/{id}
   hideProduct: → PUT /api/products/{id}/hide
   updateInventory: → PUT /api/products/{id}/inventory
   getCategories: → GET /api/categories
   ```

### 🖼️ Image Handling Issue
- `GET /usingImage/{id}` (FE) - **No backend equivalent**
- **Temporary solution:** Comment out or add placeholder
- **TODO: Implement image serving endpoint in backend**

### ⚠️ Diamond Endpoints Mismatch
- Frontend has diamond-specific endpoints but they don't match backend structure
- **Action:** Update frontend diamond functions to use `/api/diamonds/{id}`

### 📝 Action Items:
1. Update frontend endpoint URLs
2. Add missing frontend functions for backend endpoints
3. **COMMENT OUT:** `getImageProduct` function temporarily
4. Fix diamond endpoint URLs
5. Add categories management UI

---

## Phase 4: Order Management

### ✅ Well Matched
Most order endpoints align well between frontend and backend.

### 🔧 Needs Frontend Updates
1. **Update endpoint paths in `orderAPI.ts`:**
   ```typescript
   placeOrder: /orders → /api/orders
   getOrderStatus: /orders/{id} → /api/orders/{id}
   getOrderHistory: /orders/customer/{customerId} → /api/orders/customer/{customerId}
   updateOrderStatus: /orders/{id}/status → /api/orders/{id}/status
   confirmOrder: /orders/{id}/confirm → /api/orders/{id}/confirm
   handleOrderFailure: /orders/{id}/fail → /api/orders/{id}/fail
   getSalesDashboard: /sales-dashboard → /api/sales-dashboard
   getAllOrders: /orders → /api/orders
   ```

### 📝 Action Items:
1. Update all endpoint URLs in `orderAPI.ts`
2. Verify request/response models match backend DTOs

---

## Phase 5: Order Lines Management

### 🔧 Needs Major Frontend Updates
The frontend has inconsistent endpoint naming for order lines.

### 📝 Action Items:
1. **Standardize all endpoints in `orderLineAPI.ts`:**
   ```typescript
   showAllOrderLineForAdmin: /order-lines → /api/order-lines
   getOrderLinesByOrderId: /order-lines/order/{orderId} → /api/order-lines/order/{orderId}
   showAllOrderLineForCustomer: /orderLine/showOrder → /api/order-lines/customer/{customerId}
   OrderLineDetail: /orderLine/{id} → /api/order-lines/{id}
   createOrderLine: /orderLine/create → /api/order-lines
   updateOrderLine: /orderLine/update/{id} → /api/order-lines/{id}
   deleteOrderLine: /orderLine/delete/{id} → /api/order-lines/{id}
   ```

2. **Fix HTTP methods:** Some are using incorrect methods (REMOVE instead of DELETE)

---

## Phase 6: Missing Features to Implement Later

### 🚀 Payment Integration
- **Frontend needs:** VNPay payment integration
- **Backend has:** `POST /api/payments/vnpay/create` and callback handler
- **TODO:** Create payment UI components

### 🎯 Promotions
- **Backend has:** `POST /api/promotions`
- **TODO:** Create promotion management UI

### 🔐 Enhanced Auth
- **Backend has:** Google OAuth, forgot/reset password
- **TODO:** Add corresponding UI components

---

## Migration Steps Summary

### Step 1: Quick Fixes (Day 1)
1. Update all endpoint URLs in frontend service files
2. Fix HTTP methods (REMOVE → DELETE)
3. Comment out `getImageProduct` function
4. Remove duplicate functions in `accountApi.ts`

### Step 2: Backend Gaps (Day 2-3)
1. Implement missing customer registration endpoint
2. Implement image serving endpoint
3. Add any missing user management endpoints

### Step 3: Frontend Enhancements (Day 4-5)
1. Add missing frontend functions for existing backend endpoints
2. Update request/response models to match backend DTOs
3. Add proper error handling

### Step 4: New Features (Week 2)
1. Implement payment integration UI
2. Add promotion management
3. Add Google OAuth login
4. Add forgot/reset password UI

### Step 5: Testing & Polish (Week 3)
1. End-to-end testing
2. Fix any remaining issues
3. Add proper loading states and error messages

---

## File Modification Priority

### High Priority (Fix First)
1. `authAPI.ts` - Update endpoints and add missing functions
2. `productAPI.ts` - Update endpoints, comment out image function
3. `orderAPI.ts` - Update all endpoint URLs
4. `orderLineAPI.ts` - Major cleanup and standardization

### Medium Priority
1. `accountApi.ts` - Remove duplicates and clean up
2. Add new service files for missing features (payments, promotions)

### Low Priority (Implement Later)
1. Enhanced authentication features
2. Image management system
3. Advanced filtering and search
4. Analytics dashboard enhancements