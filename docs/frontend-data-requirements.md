
# Frontend Data Requirements

This document outlines the data required for each page in the frontend application. The data is presented in JSON format with temporary/fake data to illustrate the structure of the data that should be returned by the API endpoints.

## Table of Contents

- [Admin Pages](#admin-pages)
  - [Client Caring Page](#client-caring-page)
  - [Customer Page](#customer-page)
  - [Dashboard](#dashboard)
  - [Manager Page](#manager-page)
  - [Marketing Page](#marketing-page)
  - [Order Page](#order-page)
  - [Product Page](#product-page)
  - [Staff Page](#staff-page)
- [Customer Pages](#customer-pages)
  - [Account Detail](#account-detail)
  - [Cart](#cart)
  - [Checkout](#checkout)
  - [History](#history)
  - [Notifications](#notifications)
  - [Order List](#order-list)
  - [Order Details](#order-details)
  - [Review/Feedback](#reviewfeedback)
  - [Voucher](#voucher)
- [Home Pages](#home-pages)
  - [About Us](#about-us)
  - [All Collections](#all-collections)
  - [All Diamonds](#all-diamonds)
  - [All Engagement Rings](#all-engagement-rings)
  - [All Products](#all-products)
  - [All Wedding Rings](#all-wedding-rings)
  - [Brand List](#brand-list)
  - [Collection Coming Soon](#collection-coming-soon)
  - [Collection Information](#collection-information)
  - [Diamond Detail](#diamond-detail)
  - [Document Page](#document-page)
  - [Learn About](#learn-about)
  - [Product Details](#product-details)
  - [Ring Guide](#ring-guide)
  - [Sale Jewelry Page](#sale-jewelry-page)
  - [Thank You Page](#thank-you-page)
  - [Wishlist](#wishlist)
- [Login Page](#login-page)
- [Register Page](#register-page)
- [Staff Pages](#staff-pages)
  - [Delivery Report](#delivery-report)
  - [Sales Staff](#sales-staff)

---

## Admin Pages

### Client Caring Page

**Endpoint:** `/api/admin/client-caring`

```json
{
  "customerIssues": [
    {
      "id": 1,
      "customerName": "John Doe",
      "issue": "Problem with order #123",
      "status": "Open",
      "assignedTo": "Jane Smith"
    }
  ]
}
```

### Customer Page

**Endpoint:** `/api/admin/customers`

```json
{
  "customers": [
    {
      "id": 1,
      "name": "John Doe",
      "email": "john.doe@example.com",
      "phone": "123-456-7890",
      "registrationDate": "2023-01-15"
    }
  ]
}
```

### Dashboard

**Endpoint:** `/api/admin/dashboard`

```json
{
  "stats": {
    "totalSales": 50000,
    "totalOrders": 200,
    "newCustomers": 50,
    "pendingIssues": 5
  }
}
```

### Manager Page

**Endpoint:** `/api/admin/managers`

```json
{
  "managers": [
    {
      "id": 1,
      "name": "Alice Johnson",
      "email": "alice.j@example.com",
      "role": "Sales Manager"
    }
  ]
}
```

### Marketing Page

**Endpoint:** `/api/admin/marketing`

```json
{
  "campaigns": [
    {
      "id": 1,
      "name": "Summer Sale",
      "startDate": "2023-06-01",
      "endDate": "2023-06-30",
      "status": "Active"
    }
  ]
}
```

### Order Page

**Endpoint:** `/api/admin/orders`

```json
{
  "orders": [
    {
      "id": 123,
      "customerName": "John Doe",
      "date": "2023-05-20",
      "total": 250.00,
      "status": "Shipped"
    }
  ]
}
```

### Product Page

**Endpoint:** `/api/admin/products`

```json
{
  "products": [
    {
      "id": 1,
      "name": "Diamond Ring",
      "category": "Rings",
      "price": 1200.00,
      "stock": 10
    }
  ]
}
```

### Staff Page

**Endpoint:** `/api/admin/staff`

```json
{
  "staff": [
    {
      "id": 1,
      "name": "Jane Smith",
      "email": "jane.s@example.com",
      "role": "Client Care"
    }
  ]
}
```

---

## Customer Pages

### Account Detail

**Endpoint:** `/api/customer/account`

```json
{
  "user": {
    "name": "John Doe",
    "email": "john.doe@example.com",
    "phone": "123-456-7890",
    "address": "123 Main St, Anytown, USA"
  }
}
```

### Cart

**Endpoint:** `/api/customer/cart`

```json
{
  "items": [
    {
      "id": 1,
      "name": "Diamond Ring",
      "price": 1200.00,
      "quantity": 1
    }
  ],
  "total": 1200.00
}
```

### Checkout

**Endpoint:** `/api/customer/checkout`

```json
{
  "orderSummary": {
    "items": [
      { "name": "Diamond Ring", "price": 1200.00 }
    ],
    "subtotal": 1200.00,
    "shipping": 15.00,
    "tax": 96.00,
    "total": 1311.00
  }
}
```

### History

**Endpoint:** `/api/customer/history`

```json
{
  "browsingHistory": [
    { "id": 2, "name": "Sapphire Necklace", "viewedAt": "2023-05-21T10:00:00Z" }
  ]
}
```

### Notifications

**Endpoint:** `/api/customer/notifications`

```json
{
  "notifications": [
    { "id": 1, "message": "Your order #123 has shipped!", "read": false }
  ]
}
```

### Order List

**Endpoint:** `/api/customer/orders`

```json
{
  "orders": [
    { "id": 123, "date": "2023-05-20", "total": 250.00, "status": "Shipped" }
  ]
}
```

### Order Details

**Endpoint:** `/api/customer/orders/123`

```json
{
  "order": {
    "id": 123,
    "date": "2023-05-20",
    "status": "Shipped",
    "items": [
      { "name": "Diamond Ring", "price": 1200.00, "quantity": 1 }
    ],
    "shippingAddress": "123 Main St, Anytown, USA"
  }
}
```

### Review/Feedback

**Endpoint:** `/api/customer/reviews`

```json
{
  "reviews": [
    { "productName": "Diamond Ring", "rating": 5, "comment": "Beautiful ring!" }
  ]
}
```

### Voucher

**Endpoint:** `/api/customer/vouchers`

```json
{
  "vouchers": [
    { "code": "SUMMER10", "discount": "10%", "expiryDate": "2023-06-30" }
  ]
}
```

---

## Home Pages

### About Us

**Endpoint:** `/api/about-us`

```json
{
  "companyHistory": "Founded in 1990...",
  "mission": "To provide the best diamonds...",
  "team": [
    { "name": "Mr. Smith", "role": "Founder" }
  ]
}
```

### All Collections

**Endpoint:** `/api/collections`

```json
{
  "collections": [
    { "id": 1, "name": "Summer Collection", "image": "/images/summer.jpg" }
  ]
}
```

### All Diamonds

**Endpoint:** `/api/diamonds`

```json
{
  "diamonds": [
    { "id": 1, "shape": "Round", "carat": 1.0, "price": 5000.00 }
  ]
}
```

### All Engagement Rings

**Endpoint:** `/api/engagement-rings`

```json
{
  "rings": [
    { "id": 1, "name": "Solitaire Ring", "metal": "Gold", "price": 1500.00 }
  ]
}
```

### All Products

**Endpoint:** `/api/products`

```json
{
  "products": [
    { "id": 1, "name": "Diamond Ring", "price": 1200.00, "image": "/images/ring.jpg" }
  ]
}
```

### All Wedding Rings

**Endpoint:** `/api/wedding-rings`

```json
{
  "rings": [
    { "id": 1, "name": "Classic Band", "metal": "Platinum", "price": 800.00 }
  ]
}
```

### Brand List

**Endpoint:** `/api/brands`

```json
{
  "brands": [
    { "id": 1, "name": "Tiffany & Co.", "logo": "/images/tiffany.png" }
  ]
}
```

### Collection Coming Soon

**Endpoint:** `/api/collections/coming-soon`

```json
{
  "collections": [
    { "name": "Winter Collection", "launchDate": "2023-11-01" }
  ]
}
```

### Collection Information

**Endpoint:** `/api/collections/1`

```json
{
  "collection": {
    "name": "Summer Collection",
    "description": "A collection inspired by summer.",
    "products": [
      { "id": 1, "name": "Sunstone Pendant", "price": 300.00 }
    ]
  }
}
```

### Diamond Detail

**Endpoint:** `/api/diamonds/1`

```json
{
  "diamond": {
    "id": 1,
    "shape": "Round",
    "carat": 1.0,
    "cut": "Excellent",
    "color": "D",
    "clarity": "VVS1",
    "price": 5000.00
  }
}
```

### Document Page

**Endpoint:** `/api/documents`

```json
{
  "documents": [
    { "title": "GIA Certificate Guide", "url": "/docs/gia-guide.pdf" }
  ]
}
```

### Learn About

**Endpoint:** `/api/learn`

```json
{
  "articles": [
    { "title": "The 4 Cs of Diamonds", "summary": "Learn about cut, color, clarity, and carat." }
  ]
}
```

### Product Details

**Endpoint:** `/api/products/1`

```json
{
  "product": {
    "id": 1,
    "name": "Diamond Ring",
    "description": "A beautiful diamond ring.",
    "price": 1200.00,
    "images": ["/images/ring1.jpg", "/images/ring2.jpg"]
  }
}
```

### Ring Guide

**Endpoint:** `/api/ring-guide`

```json
{
  "guides": [
    { "title": "How to Choose an Engagement Ring", "url": "/guides/engagement-ring" }
  ]
}
```

### Sale Jewelry Page

**Endpoint:** `/api/sale`

```json
{
  "saleItems": [
    { "id": 3, "name": "Emerald Earrings", "originalPrice": 500.00, "salePrice": 350.00 }
  ]
}
```

### Thank You Page

**Endpoint:** `/api/order/thank-you`

```json
{
  "order": {
    "id": 124,
    "customerName": "Jane Doe",
    "estimatedDelivery": "2023-06-01"
  }
}
```

### Wishlist

**Endpoint:** `/api/customer/wishlist`

```json
{
  "wishlist": [
    { "id": 2, "name": "Sapphire Necklace", "price": 1800.00 }
  ]
}
```

---

## Login Page

**Endpoint:** `/api/auth/login`

**Request:**
```json
{
  "email": "user@example.com",
  "password": "password123"
}
```

**Response:**
```json
{
  "token": "jwt-token",
  "user": { "name": "John Doe", "role": "customer" }
}
```

---

## Register Page

**Endpoint:** `/api/auth/register`

**Request:**
```json
{
  "name": "Jane Doe",
  "email": "jane.doe@example.com",
  "password": "password123"
}
```

**Response:**
```json
{
  "message": "Registration successful"
}
```

---

## Staff Pages

### Delivery Report

**Endpoint:** `/api/staff/delivery-report`

```json
{
  "deliveries": [
    { "orderId": 123, "status": "Delivered", "deliveryDate": "2023-05-25" }
  ]
}
```

### Sales Staff

**Endpoint:** `/api/staff/sales`

```json
{
  "sales": [
    { "staffId": 101, "staffName": "Bob", "totalSales": 5000.00 }
  ]
}
```
