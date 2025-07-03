# Backend (BE) API Endpoints

## ProductsController
- `GET /api/products`  
  **Request:** Query params (searchTerm, categoryId, minPrice, maxPrice, color, clarity, cut, minCarat, maxCarat, pageNumber, pageSize)  
  **Response:** Product catalog (DTO)
- `GET /api/products/{id}`  
  **Request:** URL param: id (Guid)  
  **Response:** Product details (DTO)
- `POST /api/products`  
  **Request:** Body: CreateProductDto  
  **Response:** Created product (DTO)
- `PUT /api/products/{id}`  
  **Request:** URL param: id (Guid), Body: UpdateProductDto  
  **Response:** Updated product (DTO)
- `PUT /api/products/{id}/hide`  
  **Request:** URL param: id (Guid), Query param: hide (bool)  
  **Response:** No content
- `PUT /api/products/{id}/inventory`  
  **Request:** URL param: id (Guid), Body: UpdateInventoryRequest  
  **Response:** No content
- `PUT /api/products/{id}/pricing`  
  **Request:** URL param: id (Guid), Body: UpdatePricingParametersRequest  
  **Response:** No content

## UsersController
- `POST /api/users/register`  
  **Request:** Body: RegisterUserRequest  
  **Response:** Created user (DTO)
- `GET /api/users/{id}`  
  **Request:** URL param: id (Guid)  
  **Response:** User details (DTO)
- `PUT /api/users/profile`  
  **Request:** Body: ManageUserAccountRequest  
  **Response:** No content
- `POST /api/users/{id}/register-vip`  
  **Request:** URL param: id (Guid), Body: RegisterCustomerForVipRequest  
  **Response:** No content
- `PUT /api/users/{id}/manage`  
  **Request:** URL param: id (Guid), Body: ManageUserAccountRequest  
  **Response:** No content

## AuthController
- `POST /api/auth/login`  
  **Request:** Body: LoginRequest  
  **Response:** Auth token/response (DTO)
- `POST /api/auth/google-login`  
  **Request:** Body: idToken (string)  
  **Response:** Auth token/response (DTO)
- `POST /api/auth/forgot-password`  
  **Request:** Body: ForgotPasswordRequest  
  **Response:** Status/response (DTO)
- `POST /api/auth/reset-password`  
  **Request:** Body: ResetPasswordRequest  
  **Response:** Status/response (DTO)

## OrdersController
- `POST /api/orders`  
  **Request:** Body: PlaceOrderRequest  
  **Response:** Created order (DTO)
- `POST /api/orders/cart/add`  
  **Request:** Body: AddToCartRequest  
  **Response:** Cart item (DTO)
- `GET /api/orders/{id}`  
  **Request:** URL param: id (Guid)  
  **Response:** Order status/details (DTO)
- `GET /api/orders/customer/{customerId}`  
  **Request:** URL param: customerId (Guid)  
  **Response:** Order history (DTO[])
- `PUT /api/orders/{id}/status`  
  **Request:** URL param: id (Guid), Body: UpdateOrderStatusRequest  
  **Response:** Updated order (DTO)
- `POST /api/orders/{id}/confirm`  
  **Request:** URL param: id (Guid)  
  **Response:** Confirmation result (DTO)
- `POST /api/orders/{id}/fail`  
  **Request:** URL param: id (Guid), Query param: reason (string)  
  **Response:** No content

## DeliveriesController
- `GET /api/deliveries/assigned`  
  **Request:** Query param: deliveryStaffId (Guid)  
  **Response:** List of assigned deliveries (DTO[])
- `PUT /api/deliveries/{id}/status`  
  **Request:** URL param: id (Guid), Body: UpdateShipmentStatusRequest  
  **Response:** No content

## PromotionsController
- `POST /api/promotions`  
  **Request:** Body: CreatePromotionRequest  
  **Response:** Created promotion (DTO)
- `GET /api/promotions/{id}`  
  **Request:** URL param: id (Guid)  
  **Response:** Promotion details (DTO)

## PaymentsController
- `POST /api/payments/vnpay/create`  
  **Request:** Body: CreateVNPayPaymentRequest  
  **Response:** VNPay payment URL/response (DTO)
- `GET /api/payments/vnpay/callback`  
  **Request:** None  
  **Response:** VNPay callback result (DTO)

## HealthController
- `GET /api/health`  
  **Request:** None  
  **Response:** Health status (object)
- `GET /api/health/db-info`  
  **Request:** None  
  **Response:** Database connection info (object)

# Frontend (FE) Service Endpoints

## authAPI.ts
```ts
login(account: object): POST /api/auth/login
register(account: object): POST /api/users/register
registerCustomer(account: object): POST /auth/signupCustomer
showAllAccounts(): GET /api/users
updateAccount(email: string, account: object): PUT /api/users/{email}
deleteAccount(id: number): DELETE /api/users/{id}
```

## accountApi.ts
```ts
getCustomer(id: number): GET /api/users/{id}
getAccountDetail(id: number): GET /api/users/{id}
```

## productAPI.ts
```ts
showAllProduct(): GET /api/products
getProductDetails(id: number): GET /api/products/{id}
createProduct(product: object): POST /api/products
updateDiamond(id: number, diamond: object): PUT /api/diamonds/{id}
deleteDiamond(id: number): DELETE /api/diamonds/{id}
```

## orderAPI.ts
```ts
showAllOrder(): GET /api/orders
showOrdersPage(): GET /api/orders
showReveneSummary(): POST /api/orders/summarize
orderDetail(id: number): GET /api/orders/{id}
orderRelation(id: number): GET /api/orders/{id}
createOrder(order: object): POST /api/orders
updateOrder(id: number, order: Partial<OrderAPIProps>): PUT /api/orders/{id}
deleteOrder(id: number): DELETE /api/orders/{id}
```

## orderLineAPI.ts
```ts
showAllOrderLineForAdmin(): GET /api/order-lines
showAllOrderLineForCustomer(customerId: number): GET /api/order-lines/customer/{customerId}
OrderLineDetail(id: number): GET /api/order-lines/{id}
createOrderLine(orderLine: OrderLineBody): POST /api/order-lines
updateOrderLine(id: number, orderLine: object): PUT /api/order-lines/{id}
deleteOrderLine(id: number): DELETE /api/order-lines/{id}
```

## voucherAPI.ts
```ts
showAllVoucher(): GET /voucher/showAll
createVoucher(voucher: object): POST /voucher/create
updateVoucher(id: number, voucher: object): PUT /voucher/update/{id}
deleteVoucher(id: number): DELETE /voucher/delete/{id}
```

## sizeAPI.ts
```ts
showAllSize(): GET /size/showAll
createSize(size: object): POST /size/create
updateSize(id: number, size: object): PUT /size/update/{id}
deleteSize(id: number): DELETE /size/delete/{id}
```

## settingVariantAPI.ts
```ts
showAllSettingVariant(): GET /jewelrySettingVariant/showAll
createSettingVariant(settingVariant: object): POST /jewelrySettingVariant/create
updateSettingVariant(id: number, settingVariant: object): PUT /jewelrySettingVariant/update/{id}
deleteSettingVariant(id: number): DELETE /jewelrySettingVariant/delete/{id}
```

## materialAPI.ts
```ts
showAllMaterial(): GET /materialjewelry/showAll
createMaterial(jewelryMaterial: object): POST /materialjewelry/create
updateMaterial(id: number, jewelryMaterial: object): PUT /materialjewelry/update/{id}
deleteMaterial(id: number): DELETE /materialjewelry/delete/{id}
```

## jewelryTypeAPI.ts
```ts
showAllJewelryType(): GET /jewelrytype/showAll
createJewelryType(jewelryType: object): POST /jewelrytype/create
updateJewelryType(id: number, jewelryType: object): PUT /jewelrytype/update/{id}
deleteJewelryType(id: number): DELETE /jewelrytype/delete/{id}
```

## jewelrySettingAPI.ts
```ts
showAllSetting(): GET /jewelrySetting/showAll
getSettingDetails(jewelrySettingID: number): GET /jewelrySetting/detail/{jewelrySettingID}
createSetting(jewelrySetting: object): POST /jewelrySetting/create
updateSetting(id: number, jewelrySetting: object): PUT /jewelrySetting/update/{id}
deleteSetting(id: number): DELETE /jewelrySetting/delete/{id}
```

## jewelryAPI.ts
```ts
showAllProduct(): GET /product/showAll
getProductDetails(productID: number): GET /product/detail/{productID}
createProduct(product: object): POST /product/create
updateProduct(id: number, product: object): PUT /product/update/{id}
deleteProduct(id: number): DELETE /product/delete/{id}
```

## diamondAPI.ts
```ts
showAllDiamond(): GET /api/diamonds
showDiamonds(params: any): GET /api/diamonds
getDiamondDetails(diamondID: number): GET /api/diamonds/{diamondID}
createDiamond(diamond: object): POST /api/diamonds
updateDiamond(id: number, diamond: object): PUT /api/diamonds/{id}
deleteDiamond(id: number): DELETE /api/diamonds/{id}
```

## discountAPI.ts
```ts
showAllDiscount(): GET /discount/showAll
createDiscount(discount: object): POST /discount/create
updateDiscount(id: number, discount: object): PUT /discount/update/{id}
deleteDiscount(id: number): DELETE /discount/delete/{id}
```

## feedBackAPI.ts
```ts
showAllFeedback(params: any): GET /feedback/showAll
showFeedbacks(): GET /feedback/showAll
createFeedback(feedback: object): POST /feedback/create
updateFeedback(id: number, feedback: object): PUT /feedback/update/{id}
deleteFeedback(id: number): DELETE /feedback/delete/{id}
```

## imageAPI.ts
```ts
uploadImage(fileList: any[], ...): POST /usingImage/upload-entity (multipart/form-data)
updateImage(imageID: number, image: object): PUT /usingImage/update/{imageID}
getImage(imageID: number): GET /usingImage/{imageID}
deleteImage(imageID: number): DELETE /usingImage/{imageID}
```

## collectionAPI.ts
```ts
showAllCollection(): GET /collection/showAll
createCollection(collection: object): POST /collection/create
updateCollection(id: number, collection: object): PUT /collection/update/{id}
deleteCollection(id: number): DELETE /collection/delete/{id}
```

## certificateAPI.ts
```ts
certificateShowAll(): GET /certificate/showAll
certificateDetail(CertificateID: number): GET /certificate/{CertificateID}
createCertificate(Certificate: object): POST /certificate/create
updateCertificate(CertificateID: number, Certificate: object): PUT /certificate/update/{CertificateID}
```

## locationAPI.ts
```ts
getProvinces(): GET https://open.oapi.vn/location/provinces?size=63
getDistricts(provinceId: number): GET https://open.oapi.vn/location/districts?province{provinceId}=79&size=30
getWards(districtID: number): GET https://open.oapi.vn/location/wards?districtId={districtID}&size=20
```

## paymentAPI.ts
```ts
createOrderPaypal(amount: number): POST /paypal/create-order
captureOrderPayPal(orderID: string): POST /paypal/capture-order/{orderID}
```

---

*For a complete mapping, review the DTOs and request/response types in the BLL's DTOs directory and update this document with field-level details for each endpoint as needed for integration.* 