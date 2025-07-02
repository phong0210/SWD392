-- Fake Data Import Script for Diamond Shop System
-- This script creates comprehensive fake data for testing and development

-- Clear existing data (optional - uncomment if needed)
-- DELETE FROM LoyaltyPointTransactions;
-- DELETE FROM Payments;
-- DELETE FROM OrderDetails;
-- DELETE FROM Deliveries;
-- DELETE FROM Orders;
-- DELETE FROM CustomerProfiles;
-- DELETE FROM StaffProfiles;
-- DELETE FROM Users;
-- DELETE FROM Promotions;
-- DELETE FROM Warranties;
-- DELETE FROM Products;
-- DELETE FROM Categories;
-- DELETE FROM VipStatuses;
-- DELETE FROM Roles;

-- Insert Roles
INSERT INTO Roles (Id, Name) VALUES
('11111111-1111-1111-1111-111111111111', 'Customer'),
('22222222-2222-2222-2222-222222222222', 'SalesStaff'),
('33333333-3333-3333-3333-333333333333', 'StoreManager'),
('44444444-4444-4444-4444-444444444444', 'HeadOfficeAdmin'),
('55555555-5555-5555-5555-555555555555', 'DeliveryStaff');

-- Insert VIP Statuses
INSERT INTO VipStatuses (Id, Name, RequiredPoints, DiscountMultiplier) VALUES
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'Bronze', 0, 1.0),
('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'Silver', 1000, 0.95),
('cccccccc-cccc-cccc-cccc-cccccccccccc', 'Gold', 5000, 0.90),
('dddddddd-dddd-dddd-dddd-dddddddddddd', 'Platinum', 10000, 0.85),
('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee', 'Diamond', 25000, 0.80);

-- Insert Categories
INSERT INTO Categories (Id, Name) VALUES
('c9d4a0c1-5b0d-4e44-8c8a-0f8b0f8b0f8b', 'Rings'),
('d1e5b1d2-6c1e-4f55-9d9b-1c9c1c9c1c9c', 'Necklaces'),
('f2f6c2e3-7d2f-5g66-0e0c-2d0d2d0d2d0d', 'Earrings'),
('g3g7d3f4-8e3g-6h77-1f1d-3e1e3e1e3e1e', 'Bracelets'),
('h4h8e4g5-9f4h-7i88-2g2e-4f2f4f2f4f2f', 'Pendants');

-- Insert Users (Customers, Staff, etc.)
INSERT INTO Users (Id, FullName, Email, Phone, PasswordHash, RoleId, IsActive) VALUES
-- Customers
('10000001-0000-0000-0000-000000000001', 'Nguyen Van An', 'an.nguyen@email.com', '0901234567', '$2a$11$hashedpassword1', '11111111-1111-1111-1111-111111111111', 1),
('10000002-0000-0000-0000-000000000002', 'Tran Thi Binh', 'binh.tran@email.com', '0901234568', '$2a$11$hashedpassword2', '11111111-1111-1111-1111-111111111111', 1),
('10000003-0000-0000-0000-000000000003', 'Le Van Cuong', 'cuong.le@email.com', '0901234569', '$2a$11$hashedpassword3', '11111111-1111-1111-1111-111111111111', 1),
('10000004-0000-0000-0000-000000000004', 'Pham Thi Dung', 'dung.pham@email.com', '0901234570', '$2a$11$hashedpassword4', '11111111-1111-1111-1111-111111111111', 1),
('10000005-0000-0000-0000-000000000005', 'Hoang Van Em', 'em.hoang@email.com', '0901234571', '$2a$11$hashedpassword5', '11111111-1111-1111-1111-111111111111', 1),

-- Sales Staff
('20000001-0000-0000-0000-000000000001', 'Nguyen Thi Sales', 'sales1@diamondshop.com', '0901234572', '$2a$11$hashedpassword6', '22222222-2222-2222-2222-222222222222', 1),
('20000002-0000-0000-0000-000000000002', 'Tran Van Sales', 'sales2@diamondshop.com', '0901234573', '$2a$11$hashedpassword7', '22222222-2222-2222-2222-222222222222', 1),

-- Store Managers
('30000001-0000-0000-0000-000000000001', 'Le Thi Manager', 'manager1@diamondshop.com', '0901234574', '$2a$11$hashedpassword8', '33333333-3333-3333-3333-333333333333', 1),
('30000002-0000-0000-0000-000000000002', 'Pham Van Manager', 'manager2@diamondshop.com', '0901234575', '$2a$11$hashedpassword9', '33333333-3333-3333-3333-333333333333', 1),

-- Head Office Admin
('40000001-0000-0000-0000-000000000001', 'Hoang Thi Admin', 'admin@diamondshop.com', '0901234576', '$2a$11$hashedpassword10', '44444444-4444-4444-4444-444444444444', 1),

-- Delivery Staff
('50000001-0000-0000-0000-000000000001', 'Nguyen Van Delivery', 'delivery1@diamondshop.com', '0901234577', '$2a$11$hashedpassword11', '55555555-5555-5555-5555-555555555555', 1),
('50000002-0000-0000-0000-000000000002', 'Tran Thi Delivery', 'delivery2@diamondshop.com', '0901234578', '$2a$11$hashedpassword12', '55555555-5555-5555-5555-555555555555', 1);

-- Insert Customer Profiles
INSERT INTO CustomerProfiles (UserId, Address_Street, Address_City, Address_State, Address_PostalCode, Address_Country, VipStatusId, LoyaltyPoints) VALUES
('10000001-0000-0000-0000-000000000001', '123 Nguyen Trai', 'Ho Chi Minh City', 'Ho Chi Minh', '70000', 'Vietnam', 'dddddddd-dddd-dddd-dddd-dddddddddddd', 15000),
('10000002-0000-0000-0000-000000000002', '456 Le Loi', 'Ho Chi Minh City', 'Ho Chi Minh', '70000', 'Vietnam', 'cccccccc-cccc-cccc-cccc-cccccccccccc', 7500),
('10000003-0000-0000-0000-000000000003', '789 Tran Phu', 'Hanoi', 'Hanoi', '10000', 'Vietnam', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 2500),
('10000004-0000-0000-0000-000000000004', '321 Vo Van Tan', 'Ho Chi Minh City', 'Ho Chi Minh', '70000', 'Vietnam', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 500),
('10000005-0000-0000-0000-000000000005', '654 Dien Bien Phu', 'Hanoi', 'Hanoi', '10000', 'Vietnam', 'eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee', 30000);

-- Insert Staff Profiles
INSERT INTO StaffProfiles (UserId, HireDate, Salary) VALUES
('20000001-0000-0000-0000-000000000001', '2023-01-15', 15000000),
('20000002-0000-0000-0000-000000000002', '2023-03-20', 15000000),
('30000001-0000-0000-0000-000000000001', '2022-06-10', 25000000),
('30000002-0000-0000-0000-000000000002', '2022-08-15', 25000000),
('40000001-0000-0000-0000-000000000001', '2022-01-01', 35000000),
('50000001-0000-0000-0000-000000000001', '2023-02-01', 12000000),
('50000002-0000-0000-0000-000000000002', '2023-04-10', 12000000);

-- Insert Products
INSERT INTO Products (Id, SKU, Name, Description, BasePrice, CategoryId, IsActive, Quantity) VALUES
('a1b2c3d4-e5f6-7890-1234-567890abcdef', 'RING001', 'Classic Diamond Ring', 'A beautiful classic diamond ring with excellent cut and clarity.', 15000000, 'c9d4a0c1-5b0d-4e44-8c8a-0f8b0f8b0f8b', 1, 10),
('b2c3d4e5-f6a7-8901-2345-67890abcdef0', 'NECK001', 'Elegant Diamond Necklace', 'A stunning diamond necklace perfect for special occasions.', 25000000, 'd1e5b1d2-6c1e-4f55-9d9b-1c9c1c9c1c9c', 1, 8),
('c3d4e5f6-a7b8-9012-3456-7890abcdef01', 'RING002', 'Princess Cut Diamond Ring', 'Modern princess cut diamond ring with platinum setting.', 18000000, 'c9d4a0c1-5b0d-4e44-8c8a-0f8b0f8b0f8b', 1, 12),
('d4e5f6a7-b8c9-0123-4567-8901abcdef02', 'EARR001', 'Diamond Stud Earrings', 'Classic diamond stud earrings with brilliant cut.', 8000000, 'f2f6c2e3-7d2f-5g66-0e0c-2d0d2d0d2d0d', 1, 15),
('e5f6a7b8-c9d0-1234-5678-9012abcdef03', 'BRAC001', 'Diamond Tennis Bracelet', 'Elegant tennis bracelet with multiple diamonds.', 12000000, 'g3g7d3f4-8e3g-6h77-1f1d-3e1e3e1e3e1e', 1, 6),
('f6a7b8c9-d0e1-2345-6789-0123abcdef04', 'PEND001', 'Diamond Pendant', 'Beautiful diamond pendant with white gold chain.', 6000000, 'h4h8e4g5-9f4h-7i88-2g2e-4f2f4f2f4f2f', 1, 20),
('a7b8c9d0-e1f2-3456-7890-1234abcdef05', 'RING003', 'Oval Diamond Ring', 'Stunning oval cut diamond ring with rose gold setting.', 22000000, 'c9d4a0c1-5b0d-4e44-8c8a-0f8b0f8b0f8b', 1, 7),
('b8c9d0e1-f2a3-4567-8901-2345abcdef06', 'NECK002', 'Diamond Choker', 'Luxurious diamond choker with multiple stone arrangement.', 35000000, 'd1e5b1d2-6c1e-4f55-9d9b-1c9c1c9c1c9c', 1, 4),
('c9d0e1f2-a3b4-5678-9012-3456abcdef07', 'EARR002', 'Diamond Drop Earrings', 'Elegant drop earrings with pear-shaped diamonds.', 15000000, 'f2f6c2e3-7d2f-5g66-0e0c-2d0d2d0d2d0d', 1, 9),
('d0e1f2a3-b4c5-6789-0123-4567abcdef08', 'BRAC002', 'Diamond Bangle', 'Modern diamond bangle with geometric design.', 18000000, 'g3g7d3f4-8e3g-6h77-1f1d-3e1e3e1e3e1e', 1, 5);

-- Insert Diamond Properties (Owned Entity)
INSERT INTO Products_DiamondProperties (ProductId, Carat, Color, Clarity, Cut) VALUES
('a1b2c3d4-e5f6-7890-1234-567890abcdef', 1.0, 'D', 'VS1', 'Excellent'),
('b2c3d4e5-f6a7-8901-2345-67890abcdef0', 1.5, 'E', 'VVS2', 'Very Good'),
('c3d4e5f6-a7b8-9012-3456-7890abcdef01', 1.2, 'F', 'VS1', 'Excellent'),
('d4e5f6a7-b8c9-0123-4567-8901abcdef02', 0.5, 'G', 'VS2', 'Very Good'),
('e5f6a7b8-c9d0-1234-5678-9012abcdef03', 2.0, 'D', 'VVS1', 'Excellent'),
('f6a7b8c9-d0e1-2345-6789-0123abcdef04', 0.3, 'H', 'SI1', 'Good'),
('a7b8c9d0-e1f2-3456-7890-1234abcdef05', 1.8, 'E', 'VS1', 'Excellent'),
('b8c9d0e1-f2a3-4567-8901-2345abcdef06', 3.5, 'D', 'VVS1', 'Excellent'),
('c9d0e1f2-a3b4-5678-9012-3456abcdef07', 1.1, 'F', 'VS2', 'Very Good'),
('d0e1f2a3-b4c5-6789-0123-4567abcdef08', 2.2, 'E', 'VS1', 'Excellent');

-- Insert Warranties
INSERT INTO Warranties (Id, ProductId, Duration, Terms) VALUES
('w1111111-1111-1111-1111-111111111111', 'a1b2c3d4-e5f6-7890-1234-567890abcdef', 24, 'Full warranty covering defects in materials and workmanship'),
('w2222222-2222-2222-2222-222222222222', 'b2c3d4e5-f6a7-8901-2345-67890abcdef0', 36, 'Extended warranty with lifetime diamond replacement'),
('w3333333-3333-3333-3333-333333333333', 'c3d4e5f6-a7b8-9012-3456-7890abcdef01', 24, 'Standard warranty covering manufacturing defects'),
('w4444444-4444-4444-4444-444444444444', 'd4e5f6a7-b8c9-0123-4567-8901abcdef02', 18, 'Basic warranty for earrings'),
('w5555555-5555-5555-5555-555555555555', 'e5f6a7b8-c9d0-1234-5678-9012abcdef03', 24, 'Comprehensive warranty for bracelet'),
('w6666666-6666-6666-6666-666666666666', 'f6a7b8c9-d0e1-2345-6789-0123abcdef04', 12, 'Standard warranty for pendant'),
('w7777777-7777-7777-7777-777777777777', 'a7b8c9d0-e1f2-3456-7890-1234abcdef05', 36, 'Premium warranty with annual inspection'),
('w8888888-8888-8888-8888-888888888888', 'b8c9d0e1-f2a3-4567-8901-2345abcdef06', 60, 'Lifetime warranty for luxury items'),
('w9999999-9999-9999-9999-999999999999', 'c9d0e1f2-a3b4-5678-9012-3456abcdef07', 24, 'Standard warranty for earrings'),
('w0000000-0000-0000-0000-000000000000', 'd0e1f2a3-b4c5-6789-0123-4567abcdef08', 24, 'Comprehensive warranty for bangle');

-- Insert Promotions
INSERT INTO Promotions (Id, Code, Description, DiscountPercentage, StartDate, EndDate, ProductId) VALUES
('p1111111-1111-1111-1111-111111111111', 'SUMMER20', 'Summer Sale - 20% off all rings', 20.0, '2024-06-01', '2024-08-31', 'a1b2c3d4-e5f6-7890-1234-567890abcdef'),
('p2222222-2222-2222-2222-222222222222', 'VIP15', 'VIP Customer Discount - 15% off necklaces', 15.0, '2024-01-01', '2024-12-31', 'b2c3d4e5-f6a7-8901-2345-67890abcdef0'),
('p3333333-3333-3333-3333-333333333333', 'NEWCUST10', 'New Customer Discount - 10% off earrings', 10.0, '2024-01-01', '2024-12-31', 'd4e5f6a7-b8c9-0123-4567-8901abcdef02'),
('p4444444-4444-4444-4444-444444444444', 'HOLIDAY25', 'Holiday Special - 25% off bracelets', 25.0, '2024-12-01', '2024-12-31', 'e5f6a7b8-c9d0-1234-5678-9012abcdef03'),
('p5555555-5555-5555-5555-555555555555', 'FLASH30', 'Flash Sale - 30% off pendants', 30.0, '2024-07-15', '2024-07-20', 'f6a7b8c9-d0e1-2345-6789-0123abcdef04');

-- Insert Orders
INSERT INTO Orders (Id, CustomerId, OrderDate, Status, TotalAmount, ShippingAddress_Street, ShippingAddress_City, ShippingAddress_State, ShippingAddress_PostalCode, ShippingAddress_Country) VALUES
('o1111111-1111-1111-1111-111111111111', '10000001-0000-0000-0000-000000000001', '2024-06-15', 4, 15000000, '123 Nguyen Trai', 'Ho Chi Minh City', 'Ho Chi Minh', '70000', 'Vietnam'),
('o2222222-2222-2222-2222-222222222222', '10000002-0000-0000-0000-000000000002', '2024-06-20', 3, 25000000, '456 Le Loi', 'Ho Chi Minh City', 'Ho Chi Minh', '70000', 'Vietnam'),
('o3333333-3333-3333-3333-333333333333', '10000003-0000-0000-0000-000000000003', '2024-06-25', 2, 8000000, '789 Tran Phu', 'Hanoi', 'Hanoi', '10000', 'Vietnam'),
('o4444444-4444-4444-4444-444444444444', '10000004-0000-0000-0000-000000000004', '2024-07-01', 1, 12000000, '321 Vo Van Tan', 'Ho Chi Minh City', 'Ho Chi Minh', '70000', 'Vietnam'),
('o5555555-5555-5555-5555-555555555555', '10000005-0000-0000-0000-000000000005', '2024-07-05', 0, 18000000, '654 Dien Bien Phu', 'Hanoi', 'Hanoi', '10000', 'Vietnam'),
('o6666666-6666-6666-6666-666666666666', '10000001-0000-0000-0000-000000000001', '2024-07-10', 4, 6000000, '123 Nguyen Trai', 'Ho Chi Minh City', 'Ho Chi Minh', '70000', 'Vietnam'),
('o7777777-7777-7777-7777-777777777777', '10000002-0000-0000-0000-000000000002', '2024-07-12', 3, 22000000, '456 Le Loi', 'Ho Chi Minh City', 'Ho Chi Minh', '70000', 'Vietnam'),
('o8888888-8888-8888-8888-888888888888', '10000003-0000-0000-0000-000000000003', '2024-07-15', 2, 15000000, '789 Tran Phu', 'Hanoi', 'Hanoi', '10000', 'Vietnam');

-- Insert Order Details
INSERT INTO OrderDetails (Id, OrderId, ProductId, Quantity, PriceAtTimeOfPurchase) VALUES
('od111111-1111-1111-1111-111111111111', 'o1111111-1111-1111-1111-111111111111', 'a1b2c3d4-e5f6-7890-1234-567890abcdef', 1, 15000000),
('od222222-2222-2222-2222-222222222222', 'o2222222-2222-2222-2222-222222222222', 'b2c3d4e5-f6a7-8901-2345-67890abcdef0', 1, 25000000),
('od333333-3333-3333-3333-333333333333', 'o3333333-3333-3333-3333-333333333333', 'd4e5f6a7-b8c9-0123-4567-8901abcdef02', 1, 8000000),
('od444444-4444-4444-4444-444444444444', 'o4444444-4444-4444-4444-444444444444', 'e5f6a7b8-c9d0-1234-5678-9012abcdef03', 1, 12000000),
('od555555-5555-5555-5555-555555555555', 'o5555555-5555-5555-5555-555555555555', 'c3d4e5f6-a7b8-9012-3456-7890abcdef01', 1, 18000000),
('od666666-6666-6666-6666-666666666666', 'o6666666-6666-6666-6666-666666666666', 'f6a7b8c9-d0e1-2345-6789-0123abcdef04', 1, 6000000),
('od777777-7777-7777-7777-777777777777', 'o7777777-7777-7777-7777-777777777777', 'a7b8c9d0-e1f2-3456-7890-1234abcdef05', 1, 22000000),
('od888888-8888-8888-8888-888888888888', 'o8888888-8888-8888-8888-888888888888', 'c9d0e1f2-a3b4-5678-9012-3456abcdef07', 1, 15000000);

-- Insert Payments
INSERT INTO Payments (Id, OrderId, Amount, PaymentMethod, Status, TransactionDate) VALUES
('pay11111-1111-1111-1111-111111111111', 'o1111111-1111-1111-1111-111111111111', 15000000, 'VNPay', 2, '2024-06-15 10:30:00'),
('pay22222-2222-2222-2222-222222222222', 'o2222222-2222-2222-2222-222222222222', 25000000, 'VNPay', 2, '2024-06-20 14:15:00'),
('pay33333-3333-3333-3333-333333333333', 'o3333333-3333-3333-3333-333333333333', 8000000, 'VNPay', 2, '2024-06-25 09:45:00'),
('pay44444-4444-4444-4444-444444444444', 'o4444444-4444-4444-4444-444444444444', 12000000, 'VNPay', 1, '2024-07-01 16:20:00'),
('pay55555-5555-5555-5555-555555555555', 'o5555555-5555-5555-5555-555555555555', 18000000, 'VNPay', 0, '2024-07-05 11:10:00'),
('pay66666-6666-6666-6666-666666666666', 'o6666666-6666-6666-6666-666666666666', 6000000, 'VNPay', 2, '2024-07-10 13:25:00'),
('pay77777-7777-7777-7777-777777777777', 'o7777777-7777-7777-7777-777777777777', 22000000, 'VNPay', 2, '2024-07-12 15:40:00'),
('pay88888-8888-8888-8888-888888888888', 'o8888888-8888-8888-8888-888888888888', 15000000, 'VNPay', 1, '2024-07-15 10:55:00');

-- Insert Deliveries
INSERT INTO Deliveries (Id, OrderId, DeliveryStaffId, Status, DispatchedDate, DeliveredDate) VALUES
('del11111-1111-1111-1111-111111111111', 'o1111111-1111-1111-1111-111111111111', '50000001-0000-0000-0000-000000000001', 4, '2024-06-16 08:00:00', '2024-06-17 14:30:00'),
('del22222-2222-2222-2222-222222222222', 'o2222222-2222-2222-2222-222222222222', '50000002-0000-0000-0000-000000000002', 3, '2024-06-21 09:00:00', NULL),
('del33333-3333-3333-3333-333333333333', 'o3333333-3333-3333-3333-333333333333', '50000001-0000-0000-0000-000000000001', 2, '2024-06-26 10:00:00', NULL),
('del44444-4444-4444-4444-444444444444', 'o4444444-4444-4444-4444-444444444444', '50000002-0000-0000-0000-000000000002', 1, NULL, NULL),
('del55555-5555-5555-5555-555555555555', 'o5555555-5555-5555-5555-555555555555', '50000001-0000-0000-0000-000000000001', 0, NULL, NULL),
('del66666-6666-6666-6666-666666666666', 'o6666666-6666-6666-6666-666666666666', '50000002-0000-0000-0000-000000000002', 4, '2024-07-11 08:30:00', '2024-07-12 15:45:00'),
('del77777-7777-7777-7777-777777777777', 'o7777777-7777-7777-7777-777777777777', '50000001-0000-0000-0000-000000000001', 3, '2024-07-13 09:15:00', NULL),
('del88888-8888-8888-8888-888888888888', 'o8888888-8888-8888-8888-888888888888', '50000002-0000-0000-0000-000000000002', 2, '2024-07-16 10:30:00', NULL);

-- Insert Loyalty Point Transactions
INSERT INTO LoyaltyPointTransactions (Id, CustomerId, Points, TransactionType, Date) VALUES
('lpt11111-1111-1111-1111-111111111111', '10000001-0000-0000-0000-000000000001', 1500, 0, '2024-06-17 14:30:00'),
('lpt22222-2222-2222-2222-222222222222', '10000002-0000-0000-0000-000000000002', 2500, 0, '2024-06-20 14:15:00'),
('lpt33333-3333-3333-3333-333333333333', '10000003-0000-0000-0000-000000000003', 800, 0, '2024-06-25 09:45:00'),
('lpt44444-4444-4444-4444-444444444444', '10000004-0000-0000-0000-000000000004', 1200, 0, '2024-07-01 16:20:00'),
('lpt55555-5555-5555-5555-555555555555', '10000005-0000-0000-0000-000000000005', 1800, 0, '2024-07-05 11:10:00'),
('lpt66666-6666-6666-6666-666666666666', '10000001-0000-0000-0000-000000000001', 600, 0, '2024-07-12 15:45:00'),
('lpt77777-7777-7777-7777-777777777777', '10000002-0000-0000-0000-000000000002', 2200, 0, '2024-07-12 15:40:00'),
('lpt88888-8888-8888-8888-888888888888', '10000003-0000-0000-0000-000000000003', 1500, 0, '2024-07-15 10:55:00'),
('lpt99999-9999-9999-9999-999999999999', '10000001-0000-0000-0000-000000000001', -500, 1, '2024-06-20 10:00:00'),
('lpt00000-0000-0000-0000-000000000000', '10000002-0000-0000-0000-000000000002', -1000, 1, '2024-06-25 14:00:00');

-- Update product quantities after orders
UPDATE Products SET Quantity = Quantity - 1 WHERE Id = 'a1b2c3d4-e5f6-7890-1234-567890abcdef';
UPDATE Products SET Quantity = Quantity - 1 WHERE Id = 'b2c3d4e5-f6a7-8901-2345-67890abcdef0';
UPDATE Products SET Quantity = Quantity - 1 WHERE Id = 'd4e5f6a7-b8c9-0123-4567-8901abcdef02';
UPDATE Products SET Quantity = Quantity - 1 WHERE Id = 'e5f6a7b8-c9d0-1234-5678-9012abcdef03';
UPDATE Products SET Quantity = Quantity - 1 WHERE Id = 'c3d4e5f6-a7b8-9012-3456-7890abcdef01';
UPDATE Products SET Quantity = Quantity - 1 WHERE Id = 'f6a7b8c9-d0e1-2345-6789-0123abcdef04';
UPDATE Products SET Quantity = Quantity - 1 WHERE Id = 'a7b8c9d0-e1f2-3456-7890-1234abcdef05';
UPDATE Products SET Quantity = Quantity - 1 WHERE Id = 'c9d0e1f2-a3b4-5678-9012-3456abcdef07';

PRINT 'Fake data import completed successfully!';
PRINT 'Total records inserted:';
PRINT '- Roles: 5';
PRINT '- VIP Statuses: 5';
PRINT '- Categories: 5';
PRINT '- Users: 11';
PRINT '- Customer Profiles: 5';
PRINT '- Staff Profiles: 7';
PRINT '- Products: 10';
PRINT '- Diamond Properties: 10';
PRINT '- Warranties: 10';
PRINT '- Promotions: 5';
PRINT '- Orders: 8';
PRINT '- Order Details: 8';
PRINT '- Payments: 8';
PRINT '- Deliveries: 8';
PRINT '- Loyalty Point Transactions: 10'; 