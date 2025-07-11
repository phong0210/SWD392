-- Test Data Generation Script for Diamond Shop System
-- This script generates fake data for testing purposes
-- Respects auto-generated UUIDs and uses correct role enum values

-- Clear existing test data (optional - comment out if you want to keep existing data)
-- DELETE FROM public."Warranties";
-- DELETE FROM public."Promotions";
-- DELETE FROM public."Products";
-- DELETE FROM public."Deliveries";
-- DELETE FROM public."Payments";
-- DELETE FROM public."OrderDetails";
-- DELETE FROM public."Orders";
-- DELETE FROM public."Vips";
-- DELETE FROM public."StaffProfiles";
-- DELETE FROM public."LoyaltyPoints";
-- DELETE FROM public."Users";
-- DELETE FROM public."Roles";
-- DELETE FROM public."Categories";

-- Insert Roles
INSERT INTO public."Roles" ("Id", "Name") VALUES
    (uuid_generate_v4(), 'HeadOfficeAdmin'),
    (uuid_generate_v4(), 'Manager'),
    (uuid_generate_v4(), 'SaleStaff'),
    (uuid_generate_v4(), 'DeliveryStaff');

-- Insert Categories
INSERT INTO public."Categories" ("Id", "Name", "Description") VALUES
    (uuid_generate_v4(), 'Diamond Rings', 'Engagement and wedding rings with diamonds'),
    (uuid_generate_v4(), 'Diamond Necklaces', 'Elegant diamond necklaces and pendants'),
    (uuid_generate_v4(), 'Diamond Earrings', 'Beautiful diamond earrings and studs'),
    (uuid_generate_v4(), 'Diamond Bracelets', 'Luxury diamond bracelets and bangles'),
    (uuid_generate_v4(), 'Loose Diamonds', 'Individual diamonds for custom jewelry');

-- Insert Users (mix of regular users and staff)
INSERT INTO public."Users" ("Id", "Name", "Email", "PasswordHash", "Phone", "Address", "GoogleId", "CreatedAt", "Status") VALUES
    -- Regular Customers
    (uuid_generate_v4(), 'John Smith', 'john.smith@email.com', '$2a$10$hashedpassword123', '+1234567890', '123 Main St, New York, NY', NULL, NOW(), true),
    (uuid_generate_v4(), 'Sarah Johnson', 'sarah.j@email.com', '$2a$10$hashedpassword456', '+1234567891', '456 Oak Ave, Los Angeles, CA', NULL, NOW(), true),
    (uuid_generate_v4(), 'Michael Brown', 'michael.b@email.com', '$2a$10$hashedpassword789', '+1234567892', '789 Pine Rd, Chicago, IL', NULL, NOW(), true),
    (uuid_generate_v4(), 'Emily Davis', 'emily.d@email.com', '$2a$10$hashedpassword012', '+1234567893', '321 Elm St, Miami, FL', NULL, NOW(), true),
    (uuid_generate_v4(), 'David Wilson', 'david.w@email.com', '$2a$10$hashedpassword345', '+1234567894', '654 Maple Dr, Seattle, WA', NULL, NOW(), true),
    
    -- Staff Users
    (uuid_generate_v4(), 'Admin User', 'admin@diamondshop.com', '$2a$10$hashedpassword678', '+1234567895', '100 Admin Blvd, HQ', NULL, NOW(), true),
    (uuid_generate_v4(), 'Manager User', 'manager@diamondshop.com', '$2a$10$hashedpassword901', '+1234567896', '200 Manager St, Store', NULL, NOW(), true),
    (uuid_generate_v4(), 'Sales Staff 1', 'sales1@diamondshop.com', '$2a$10$hashedpassword234', '+1234567897', '300 Sales Ave, Store', NULL, NOW(), true),
    (uuid_generate_v4(), 'Sales Staff 2', 'sales2@diamondshop.com', '$2a$10$hashedpassword567', '+1234567898', '301 Sales Ave, Store', NULL, NOW(), true),
    (uuid_generate_v4(), 'Delivery Staff 1', 'delivery1@diamondshop.com', '$2a$10$hashedpassword890', '+1234567899', '400 Delivery Rd, Warehouse', NULL, NOW(), true);

-- Insert Staff Profiles (using the staff users)
DO $$
DECLARE
    admin_role_id uuid;
    manager_role_id uuid;
    sales_role_id uuid;
    delivery_role_id uuid;
    admin_user_id uuid;
    manager_user_id uuid;
    sales1_user_id uuid;
    sales2_user_id uuid;
    delivery_user_id uuid;
BEGIN
    -- Get role IDs
    SELECT "Id" INTO admin_role_id FROM public."Roles" WHERE "Name" = 'HeadOfficeAdmin';
    SELECT "Id" INTO manager_role_id FROM public."Roles" WHERE "Name" = 'Manager';
    SELECT "Id" INTO sales_role_id FROM public."Roles" WHERE "Name" = 'SaleStaff';
    SELECT "Id" INTO delivery_role_id FROM public."Roles" WHERE "Name" = 'DeliveryStaff';
    
    -- Get user IDs
    SELECT "Id" INTO admin_user_id FROM public."Users" WHERE "Email" = 'admin@diamondshop.com';
    SELECT "Id" INTO manager_user_id FROM public."Users" WHERE "Email" = 'manager@diamondshop.com';
    SELECT "Id" INTO sales1_user_id FROM public."Users" WHERE "Email" = 'sales1@diamondshop.com';
    SELECT "Id" INTO sales2_user_id FROM public."Users" WHERE "Email" = 'sales2@diamondshop.com';
    SELECT "Id" INTO delivery_user_id FROM public."Users" WHERE "Email" = 'delivery1@diamondshop.com';
    
    -- Insert staff profiles
    INSERT INTO public."StaffProfiles" ("Id", "UserId", "RoleId", "Salary", "HireDate") VALUES
        (uuid_generate_v4(), admin_user_id, admin_role_id, 80000.00, NOW() - INTERVAL '2 years'),
        (uuid_generate_v4(), manager_user_id, manager_role_id, 65000.00, NOW() - INTERVAL '1 year'),
        (uuid_generate_v4(), sales1_user_id, sales_role_id, 45000.00, NOW() - INTERVAL '6 months'),
        (uuid_generate_v4(), sales2_user_id, sales_role_id, 42000.00, NOW() - INTERVAL '3 months'),
        (uuid_generate_v4(), delivery_user_id, delivery_role_id, 38000.00, NOW() - INTERVAL '1 year');
END $$;

-- Insert VIP memberships
DO $$
DECLARE
    user1_id uuid;
    user2_id uuid;
    user3_id uuid;
BEGIN
    SELECT "Id" INTO user1_id FROM public."Users" WHERE "Email" = 'john.smith@email.com';
    SELECT "Id" INTO user2_id FROM public."Users" WHERE "Email" = 'sarah.j@email.com';
    SELECT "Id" INTO user3_id FROM public."Users" WHERE "Email" = 'michael.b@email.com';
    
    INSERT INTO public."Vips" ("VipId", "UserId", "StartDate", "EndDate", "Tier") VALUES
        (uuid_generate_v4(), user1_id, NOW() - INTERVAL '6 months', NOW() + INTERVAL '6 months', 'Gold'),
        (uuid_generate_v4(), user2_id, NOW() - INTERVAL '3 months', NOW() + INTERVAL '9 months', 'Platinum'),
        (uuid_generate_v4(), user3_id, NOW() - INTERVAL '1 month', NOW() + INTERVAL '11 months', 'Silver');
END $$;

-- Insert Loyalty Points
DO $$
DECLARE
    user1_id uuid;
    user2_id uuid;
    user3_id uuid;
    user4_id uuid;
    user5_id uuid;
BEGIN
    SELECT "Id" INTO user1_id FROM public."Users" WHERE "Email" = 'john.smith@email.com';
    SELECT "Id" INTO user2_id FROM public."Users" WHERE "Email" = 'sarah.j@email.com';
    SELECT "Id" INTO user3_id FROM public."Users" WHERE "Email" = 'michael.b@email.com';
    SELECT "Id" INTO user4_id FROM public."Users" WHERE "Email" = 'emily.d@email.com';
    SELECT "Id" INTO user5_id FROM public."Users" WHERE "Email" = 'david.w@email.com';
    
    INSERT INTO public."LoyaltyPoints" ("Id", "UserId", "PointsEarned", "PointsRedeemed", "LastUpdated") VALUES
        (uuid_generate_v4(), user1_id, 2500, 500, NOW()),
        (uuid_generate_v4(), user2_id, 1800, 200, NOW()),
        (uuid_generate_v4(), user3_id, 3200, 800, NOW()),
        (uuid_generate_v4(), user4_id, 950, 150, NOW()),
        (uuid_generate_v4(), user5_id, 1200, 300, NOW());
END $$;

-- Insert Products
DO $$
DECLARE
    diamond_rings_id uuid;
    diamond_necklaces_id uuid;
    diamond_earrings_id uuid;
    diamond_bracelets_id uuid;
    loose_diamonds_id uuid;
BEGIN
    SELECT "Id" INTO diamond_rings_id FROM public."Categories" WHERE "Name" = 'Diamond Rings';
    SELECT "Id" INTO diamond_necklaces_id FROM public."Categories" WHERE "Name" = 'Diamond Necklaces';
    SELECT "Id" INTO diamond_earrings_id FROM public."Categories" WHERE "Name" = 'Diamond Earrings';
    SELECT "Id" INTO diamond_bracelets_id FROM public."Categories" WHERE "Name" = 'Diamond Bracelets';
    SELECT "Id" INTO loose_diamonds_id FROM public."Categories" WHERE "Name" = 'Loose Diamonds';
    
    INSERT INTO public."Products" ("Id", "Name", "SKU", "Description", "Price", "Carat", "Color", "Clarity", "Cut", "StockQuantity", "GIACertNumber", "IsHidden", "CategoryId", "OrderDetailId") VALUES
        -- Diamond Rings
        (uuid_generate_v4(), 'Classic Solitaire Ring', 'DR001', 'Beautiful 1.5 carat solitaire engagement ring', 8500.00, 15, 'D', 'VVS1', 'Excellent', 5, 'GIA123456', false, diamond_rings_id, NULL),
        (uuid_generate_v4(), 'Halo Diamond Ring', 'DR002', 'Stunning halo setting with 2.0 carat center stone', 12000.00, 20, 'E', 'VS1', 'Very Good', 3, 'GIA123457', false, diamond_rings_id, NULL),
        (uuid_generate_v4(), 'Three-Stone Ring', 'DR003', 'Elegant three-stone ring with 1.8 carat total', 9500.00, 18, 'F', 'VS2', 'Excellent', 4, 'GIA123458', false, diamond_rings_id, NULL),
        
        -- Diamond Necklaces
        (uuid_generate_v4(), 'Diamond Pendant', 'DN001', 'Delicate diamond pendant on 18k gold chain', 2800.00, 5, 'G', 'VS2', 'Very Good', 8, 'GIA123459', false, diamond_necklaces_id, NULL),
        (uuid_generate_v4(), 'Diamond Tennis Necklace', 'DN002', 'Stunning tennis necklace with 3.0 carat total', 15000.00, 30, 'D', 'VVS2', 'Excellent', 2, 'GIA123460', false, diamond_necklaces_id, NULL),
        
        -- Diamond Earrings
        (uuid_generate_v4(), 'Diamond Studs', 'DE001', 'Classic 1.0 carat diamond studs', 3500.00, 10, 'E', 'VS1', 'Excellent', 6, 'GIA123461', false, diamond_earrings_id, NULL),
        (uuid_generate_v4(), 'Diamond Drop Earrings', 'DE002', 'Elegant drop earrings with 1.5 carat total', 4200.00, 15, 'F', 'VS2', 'Very Good', 4, 'GIA123462', false, diamond_earrings_id, NULL),
        
        -- Diamond Bracelets
        (uuid_generate_v4(), 'Diamond Tennis Bracelet', 'DB001', 'Beautiful tennis bracelet with 2.5 carat total', 8500.00, 25, 'E', 'VS1', 'Excellent', 3, 'GIA123463', false, diamond_bracelets_id, NULL),
        
        -- Loose Diamonds
        (uuid_generate_v4(), 'Loose Diamond 2.0 Carat', 'LD001', 'Premium loose diamond for custom jewelry', 18000.00, 20, 'D', 'VVS1', 'Excellent', 2, 'GIA123464', false, loose_diamonds_id, NULL),
        (uuid_generate_v4(), 'Loose Diamond 1.5 Carat', 'LD002', 'High-quality loose diamond', 12000.00, 15, 'E', 'VS1', 'Very Good', 3, 'GIA123465', false, loose_diamonds_id, NULL);
END $$;

-- Insert Promotions
DO $$
DECLARE
    product1_id uuid;
    product2_id uuid;
BEGIN
    SELECT "Id" INTO product1_id FROM public."Products" WHERE "SKU" = 'DR001';
    SELECT "Id" INTO product2_id FROM public."Products" WHERE "SKU" = 'DE001';
    
    INSERT INTO public."Promotions" ("Id", "Name", "Description", "StartDate", "EndDate", "DiscountType", "DiscountValue", "AppliesToProductId") VALUES
        (uuid_generate_v4(), 'Summer Sale', '20% off all engagement rings', NOW() - INTERVAL '1 month', NOW() + INTERVAL '2 months', 'Percentage', '20', product1_id),
        (uuid_generate_v4(), 'Holiday Special', '15% off diamond earrings', NOW() - INTERVAL '2 weeks', NOW() + INTERVAL '1 month', 'Percentage', '15', product2_id),
        (uuid_generate_v4(), 'VIP Discount', '10% off for VIP members', NOW() - INTERVAL '1 week', NOW() + INTERVAL '3 months', 'Percentage', '10', NULL);
END $$;

-- Insert Orders
DO $$
DECLARE
    user1_id uuid;
    user2_id uuid;
    user3_id uuid;
    sales1_user_id uuid;
    sales2_user_id uuid;
BEGIN
    SELECT "Id" INTO user1_id FROM public."Users" WHERE "Email" = 'john.smith@email.com';
    SELECT "Id" INTO user2_id FROM public."Users" WHERE "Email" = 'sarah.j@email.com';
    SELECT "Id" INTO user3_id FROM public."Users" WHERE "Email" = 'michael.b@email.com';
    SELECT "Id" INTO sales1_user_id FROM public."Users" WHERE "Email" = 'sales1@diamondshop.com';
    SELECT "Id" INTO sales2_user_id FROM public."Users" WHERE "Email" = 'sales2@diamondshop.com';
    
    INSERT INTO public."Orders" ("Id", "UserId", "TotalPrice", "OrderDate", "VipApplied", "Status", "SaleStaff") VALUES
        (uuid_generate_v4(), user1_id, 8500.00, NOW() - INTERVAL '2 weeks', true, 1, 'Sales Staff 1'),
        (uuid_generate_v4(), user2_id, 15000.00, NOW() - INTERVAL '1 week', true, 2, 'Sales Staff 2'),
        (uuid_generate_v4(), user3_id, 4200.00, NOW() - INTERVAL '3 days', false, 1, 'Sales Staff 1'),
        (uuid_generate_v4(), user1_id, 2800.00, NOW() - INTERVAL '1 day', true, 3, 'Sales Staff 2');
END $$;

-- Insert Order Details
DO $$
DECLARE
    order1_id uuid;
    order2_id uuid;
    order3_id uuid;
    order4_id uuid;
    product1_id uuid;
    product2_id uuid;
    product3_id uuid;
    product4_id uuid;
BEGIN
    SELECT "Id" INTO order1_id FROM public."Orders" WHERE "TotalPrice" = 8500.00 AND "OrderDate" = (SELECT MAX("OrderDate") FROM public."Orders" WHERE "TotalPrice" = 8500.00);
    SELECT "Id" INTO order2_id FROM public."Orders" WHERE "TotalPrice" = 15000.00;
    SELECT "Id" INTO order3_id FROM public."Orders" WHERE "TotalPrice" = 4200.00;
    SELECT "Id" INTO order4_id FROM public."Orders" WHERE "TotalPrice" = 2800.00;
    
    SELECT "Id" INTO product1_id FROM public."Products" WHERE "SKU" = 'DR001';
    SELECT "Id" INTO product2_id FROM public."Products" WHERE "SKU" = 'DN002';
    SELECT "Id" INTO product3_id FROM public."Products" WHERE "SKU" = 'DE002';
    SELECT "Id" INTO product4_id FROM public."Products" WHERE "SKU" = 'DN001';
    
    INSERT INTO public."OrderDetails" ("Id", "OrderId", "ProductId", "UnitPrice", "Quantity") VALUES
    (uuid_generate_v4(), order1_id, product1_id, 8500.00, 1),
    (uuid_generate_v4(), order2_id, product2_id, 15000.00, 1),
    (uuid_generate_v4(), order3_id, product3_id, 4200.00, 1),
    (uuid_generate_v4(), order4_id, product4_id, 2800.00, 1);

END $$;

-- Insert Payments
DO $$
DECLARE
    order1_id uuid;
    order2_id uuid;
    order3_id uuid;
    order4_id uuid;
BEGIN
    SELECT "Id" INTO order1_id FROM public."Orders" WHERE "TotalPrice" = 8500.00 AND "OrderDate" = (SELECT MAX("OrderDate") FROM public."Orders" WHERE "TotalPrice" = 8500.00);
    SELECT "Id" INTO order2_id FROM public."Orders" WHERE "TotalPrice" = 15000.00;
    SELECT "Id" INTO order3_id FROM public."Orders" WHERE "TotalPrice" = 4200.00;
    SELECT "Id" INTO order4_id FROM public."Orders" WHERE "TotalPrice" = 2800.00;
    
    INSERT INTO public."Payments" ("Id", "OrderId", "Method", "Date", "Amount", "Status") VALUES
        (uuid_generate_v4(), order1_id, 'Credit Card', NOW() - INTERVAL '2 weeks', 8500.00, 2),
        (uuid_generate_v4(), order2_id, 'Bank Transfer', NOW() - INTERVAL '1 week', 15000.00, 2),
        (uuid_generate_v4(), order3_id, 'Credit Card', NOW() - INTERVAL '3 days', 4200.00, 1),
        (uuid_generate_v4(), order4_id, 'PayPal', NOW() - INTERVAL '1 day', 2800.00, 1);
END $$;

-- Insert Deliveries
DO $$
DECLARE
    order1_id uuid;
    order2_id uuid;
BEGIN
    SELECT "Id" INTO order1_id FROM public."Orders" WHERE "TotalPrice" = 8500.00 AND "OrderDate" = (SELECT MAX("OrderDate") FROM public."Orders" WHERE "TotalPrice" = 8500.00);
    SELECT "Id" INTO order2_id FROM public."Orders" WHERE "TotalPrice" = 15000.00;
    
    INSERT INTO public."Deliveries" ("Id", "OrderId", "DispatchTime", "DeliveryTime", "ShippingAddress", "Status") VALUES
        (uuid_generate_v4(), order1_id, NOW() - INTERVAL '10 days', NOW() - INTERVAL '5 days', '123 Main St, New York, NY', 3),
        (uuid_generate_v4(), order2_id, NOW() - INTERVAL '5 days', NULL, '456 Oak Ave, Los Angeles, CA', 2);
END $$;

-- Insert Warranties
DO $$
DECLARE
    product1_id uuid;
    product2_id uuid;
    product3_id uuid;
BEGIN
    SELECT "Id" INTO product1_id FROM public."Products" WHERE "SKU" = 'DR001';
    SELECT "Id" INTO product2_id FROM public."Products" WHERE "SKU" = 'DN002';
    SELECT "Id" INTO product3_id FROM public."Products" WHERE "SKU" = 'DE001';
    
    INSERT INTO public."Warranties" ("Id", "ProductId", "WarrantyStart", "WarrantyEnd", "Details", "IsActive") VALUES
        (uuid_generate_v4(), product1_id, NOW() - INTERVAL '2 weeks', NOW() + INTERVAL '2 years 50 weeks', 'Lifetime warranty on diamond and setting', true),
        (uuid_generate_v4(), product2_id, NOW() - INTERVAL '1 week', NOW() + INTERVAL '2 years 51 weeks', '5-year warranty on all components', true),
        (uuid_generate_v4(), product3_id, NOW() - INTERVAL '3 days', NOW() + INTERVAL '2 years 49 weeks', 'Lifetime warranty on diamond, 1-year on setting', true);
END $$;

-- Display summary of inserted data
SELECT 'Test Data Generation Complete!' as status;
SELECT COUNT(*) as total_users FROM public."Users";
SELECT COUNT(*) as total_products FROM public."Products";
SELECT COUNT(*) as total_orders FROM public."Orders";
SELECT COUNT(*) as total_staff FROM public."StaffProfiles"; 