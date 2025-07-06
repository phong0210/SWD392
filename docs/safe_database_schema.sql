-- Safe Database Schema Script
-- This script handles existing constraints and can be run multiple times safely

-- Create the required extension for UUID generation
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Drop existing foreign key constraints if they exist
DO $$ 
BEGIN
    -- Drop foreign key constraints
    IF EXISTS (SELECT 1 FROM information_schema.table_constraints WHERE constraint_name = 'FK_Deliveries_Orders_OrderId') THEN
        ALTER TABLE public."Deliveries" DROP CONSTRAINT IF EXISTS "FK_Deliveries_Orders_OrderId";
    END IF;
    
    IF EXISTS (SELECT 1 FROM information_schema.table_constraints WHERE constraint_name = 'FK_LoyaltyPoints_Users_UserId') THEN
        ALTER TABLE public."LoyaltyPoints" DROP CONSTRAINT IF EXISTS "FK_LoyaltyPoints_Users_UserId";
    END IF;
    
    IF EXISTS (SELECT 1 FROM information_schema.table_constraints WHERE constraint_name = 'FK_OrderDetails_Orders_OrderId') THEN
        ALTER TABLE public."OrderDetails" DROP CONSTRAINT IF EXISTS "FK_OrderDetails_Orders_OrderId";
    END IF;
    
    IF EXISTS (SELECT 1 FROM information_schema.table_constraints WHERE constraint_name = 'FK_Orders_Users_UserId') THEN
        ALTER TABLE public."Orders" DROP CONSTRAINT IF EXISTS "FK_Orders_Users_UserId";
    END IF;
    
    IF EXISTS (SELECT 1 FROM information_schema.table_constraints WHERE constraint_name = 'FK_Payments_Orders_OrderId') THEN
        ALTER TABLE public."Payments" DROP CONSTRAINT IF EXISTS "FK_Payments_Orders_OrderId";
    END IF;
    
    IF EXISTS (SELECT 1 FROM information_schema.table_constraints WHERE constraint_name = 'FK_Products_Categories_CategoryId') THEN
        ALTER TABLE public."Products" DROP CONSTRAINT IF EXISTS "FK_Products_Categories_CategoryId";
    END IF;
    
    IF EXISTS (SELECT 1 FROM information_schema.table_constraints WHERE constraint_name = 'FK_Products_OrderDetails_OrderDetailId') THEN
        ALTER TABLE public."Products" DROP CONSTRAINT IF EXISTS "FK_Products_OrderDetails_OrderDetailId";
    END IF;
    
    IF EXISTS (SELECT 1 FROM information_schema.table_constraints WHERE constraint_name = 'FK_Promotions_Products_AppliesToProductId') THEN
        ALTER TABLE public."Promotions" DROP CONSTRAINT IF EXISTS "FK_Promotions_Products_AppliesToProductId";
    END IF;
    
    IF EXISTS (SELECT 1 FROM information_schema.table_constraints WHERE constraint_name = 'FK_StaffProfiles_Roles_RoleId') THEN
        ALTER TABLE public."StaffProfiles" DROP CONSTRAINT IF EXISTS "FK_StaffProfiles_Roles_RoleId";
    END IF;
    
    IF EXISTS (SELECT 1 FROM information_schema.table_constraints WHERE constraint_name = 'FK_StaffProfiles_Users_UserId') THEN
        ALTER TABLE public."StaffProfiles" DROP CONSTRAINT IF EXISTS "FK_StaffProfiles_Users_UserId";
    END IF;
    
    IF EXISTS (SELECT 1 FROM information_schema.table_constraints WHERE constraint_name = 'FK_Vips_Users_UserId') THEN
        ALTER TABLE public."Vips" DROP CONSTRAINT IF EXISTS "FK_Vips_Users_UserId";
    END IF;
    
    IF EXISTS (SELECT 1 FROM information_schema.table_constraints WHERE constraint_name = 'FK_Warranties_Products_ProductId') THEN
        ALTER TABLE public."Warranties" DROP CONSTRAINT IF EXISTS "FK_Warranties_Products_ProductId";
    END IF;
    
    -- Drop unique constraints
    ALTER TABLE IF EXISTS public."Deliveries" DROP CONSTRAINT IF EXISTS "IX_Deliveries_OrderId_Unique";
    ALTER TABLE IF EXISTS public."LoyaltyPoints" DROP CONSTRAINT IF EXISTS "IX_LoyaltyPoints_UserId_Unique";
    ALTER TABLE IF EXISTS public."StaffProfiles" DROP CONSTRAINT IF EXISTS "IX_StaffProfiles_UserId_Unique";
    ALTER TABLE IF EXISTS public."Vips" DROP CONSTRAINT IF EXISTS "IX_Vips_UserId_Unique";
    ALTER TABLE IF EXISTS public."Warranties" DROP CONSTRAINT IF EXISTS "IX_Warranties_ProductId_Unique";
    
    -- Drop indexes
    DROP INDEX IF EXISTS "IX_Deliveries_OrderId";
    DROP INDEX IF EXISTS "IX_LoyaltyPoints_UserId";
    DROP INDEX IF EXISTS "IX_OrderDetails_OrderId";
    DROP INDEX IF EXISTS "IX_Orders_UserId";
    DROP INDEX IF EXISTS "IX_Payments_OrderId";
    DROP INDEX IF EXISTS "IX_Products_CategoryId";
    DROP INDEX IF EXISTS "IX_Products_OrderDetailId";
    DROP INDEX IF EXISTS "IX_Promotions_AppliesToProductId";
    DROP INDEX IF EXISTS "IX_StaffProfiles_RoleId";
    DROP INDEX IF EXISTS "IX_StaffProfiles_UserId";
    DROP INDEX IF EXISTS "IX_Vips_UserId";
    DROP INDEX IF EXISTS "IX_Warranties_ProductId";
    
END $$;

-- Drop existing tables if they exist (in reverse dependency order)
DROP TABLE IF EXISTS public."__EFMigrationsHistory" CASCADE;
DROP TABLE IF EXISTS public."Warranties" CASCADE;
DROP TABLE IF EXISTS public."Promotions" CASCADE;
DROP TABLE IF EXISTS public."Products" CASCADE;
DROP TABLE IF EXISTS public."Deliveries" CASCADE;
DROP TABLE IF EXISTS public."Payments" CASCADE;
DROP TABLE IF EXISTS public."OrderDetails" CASCADE;
DROP TABLE IF EXISTS public."Orders" CASCADE;
DROP TABLE IF EXISTS public."Vips" CASCADE;
DROP TABLE IF EXISTS public."StaffProfiles" CASCADE;
DROP TABLE IF EXISTS public."LoyaltyPoints" CASCADE;
DROP TABLE IF EXISTS public."Users" CASCADE;
DROP TABLE IF EXISTS public."Roles" CASCADE;
DROP TABLE IF EXISTS public."Categories" CASCADE;

-- Create tables
CREATE TABLE IF NOT EXISTS public."Categories"
(
    "Id" uuid NOT NULL DEFAULT uuid_generate_v4(),
    "Name" character varying(100) COLLATE pg_catalog."default" NOT NULL,
    "Description" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "PK_Categories" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS public."Deliveries"
(
    "Id" uuid NOT NULL DEFAULT uuid_generate_v4(),
    "OrderId" uuid NOT NULL,
    "DispatchTime" timestamp with time zone,
    "DeliveryTime" timestamp with time zone,
    "ShippingAddress" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    "Status" integer NOT NULL,
    CONSTRAINT "PK_Deliveries" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS public."LoyaltyPoints"
(
    "Id" uuid NOT NULL DEFAULT uuid_generate_v4(),
    "UserId" uuid NOT NULL,
    "PointsEarned" integer NOT NULL,
    "PointsRedeemed" integer NOT NULL,
    "LastUpdated" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_LoyaltyPoints" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS public."OrderDetails"
(
    "Id" uuid NOT NULL DEFAULT uuid_generate_v4(),
    "OrderId" uuid NOT NULL,
    "UnitPrice" double precision NOT NULL,
    "Quantity" integer NOT NULL,
    CONSTRAINT "PK_OrderDetails" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS public."Orders"
(
    "Id" uuid NOT NULL DEFAULT uuid_generate_v4(),
    "UserId" uuid NOT NULL,
    "TotalPrice" double precision NOT NULL,
    "OrderDate" timestamp with time zone NOT NULL,
    "VipApplied" boolean NOT NULL,
    "Status" integer NOT NULL,
    "SaleStaff" character varying(100) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "PK_Orders" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS public."Payments"
(
    "Id" uuid NOT NULL DEFAULT uuid_generate_v4(),
    "OrderId" uuid NOT NULL,
    "Method" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "Date" timestamp with time zone NOT NULL,
    "Amount" double precision NOT NULL,
    "Status" integer NOT NULL,
    CONSTRAINT "PK_Payments" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS public."Products"
(
    "Id" uuid NOT NULL DEFAULT uuid_generate_v4(),
    "Name" character varying(100) COLLATE pg_catalog."default" NOT NULL,
    "SKU" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "Description" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    "Price" double precision NOT NULL,
    "Carat" integer,
    "Color" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "Clarity" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "Cut" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "StockQuantity" integer NOT NULL,
    "GIACertNumber" character varying(100) COLLATE pg_catalog."default" NOT NULL,
    "IsHidden" boolean NOT NULL,
    "CategoryId" uuid NOT NULL,
    "OrderDetailId" uuid,
    CONSTRAINT "PK_Products" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS public."Promotions"
(
    "Id" uuid NOT NULL DEFAULT uuid_generate_v4(),
    "Name" character varying(100) COLLATE pg_catalog."default" NOT NULL,
    "Description" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    "StartDate" timestamp with time zone NOT NULL,
    "EndDate" timestamp with time zone,
    "DiscountType" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "DiscountValue" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "AppliesToProductId" uuid,
    CONSTRAINT "PK_Promotions" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS public."Roles"
(
    "Id" uuid NOT NULL DEFAULT uuid_generate_v4(),
    "Name" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "PK_Roles" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS public."StaffProfiles"
(
    "Id" uuid NOT NULL DEFAULT uuid_generate_v4(),
    "UserId" uuid,
    "RoleId" uuid NOT NULL,
    "Salary" double precision NOT NULL,
    "HireDate" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_StaffProfiles" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS public."Users"
(
    "Id" uuid NOT NULL DEFAULT uuid_generate_v4(),
    "Name" character varying(100) COLLATE pg_catalog."default" NOT NULL,
    "Email" character varying(100) COLLATE pg_catalog."default" NOT NULL,
    "PasswordHash" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    "Phone" character varying(20) COLLATE pg_catalog."default" NOT NULL,
    "Address" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    "GoogleId" uuid,
    "CreatedAt" timestamp with time zone NOT NULL,
    "Status" boolean NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS public."Vips"
(
    "VipId" uuid NOT NULL DEFAULT uuid_generate_v4(),
    "UserId" uuid,
    "StartDate" timestamp with time zone NOT NULL,
    "EndDate" timestamp with time zone,
    "Tier" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "PK_Vips" PRIMARY KEY ("VipId")
);

CREATE TABLE IF NOT EXISTS public."Warranties"
(
    "Id" uuid NOT NULL DEFAULT uuid_generate_v4(),
    "ProductId" uuid NOT NULL,
    "WarrantyStart" timestamp with time zone NOT NULL,
    "WarrantyEnd" timestamp with time zone NOT NULL,
    "Details" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    "IsActive" boolean NOT NULL,
    CONSTRAINT "PK_Warranties" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS public."__EFMigrationsHistory"
(
    "MigrationId" character varying(150) COLLATE pg_catalog."default" NOT NULL,
    "ProductVersion" character varying(32) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

-- Add foreign key constraints
ALTER TABLE public."Deliveries"
    ADD CONSTRAINT "FK_Deliveries_Orders_OrderId" FOREIGN KEY ("OrderId")
    REFERENCES public."Orders" ("Id") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE CASCADE;

ALTER TABLE public."LoyaltyPoints"
    ADD CONSTRAINT "FK_LoyaltyPoints_Users_UserId" FOREIGN KEY ("UserId")
    REFERENCES public."Users" ("Id") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE CASCADE;

ALTER TABLE public."OrderDetails"
    ADD CONSTRAINT "FK_OrderDetails_Orders_OrderId" FOREIGN KEY ("OrderId")
    REFERENCES public."Orders" ("Id") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE CASCADE;

ALTER TABLE public."Orders"
    ADD CONSTRAINT "FK_Orders_Users_UserId" FOREIGN KEY ("UserId")
    REFERENCES public."Users" ("Id") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE CASCADE;

ALTER TABLE public."Payments"
    ADD CONSTRAINT "FK_Payments_Orders_OrderId" FOREIGN KEY ("OrderId")
    REFERENCES public."Orders" ("Id") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE CASCADE;

ALTER TABLE public."Products"
    ADD CONSTRAINT "FK_Products_Categories_CategoryId" FOREIGN KEY ("CategoryId")
    REFERENCES public."Categories" ("Id") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE CASCADE;

ALTER TABLE public."Products"
    ADD CONSTRAINT "FK_Products_OrderDetails_OrderDetailId" FOREIGN KEY ("OrderDetailId")
    REFERENCES public."OrderDetails" ("Id") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;

ALTER TABLE public."Promotions"
    ADD CONSTRAINT "FK_Promotions_Products_AppliesToProductId" FOREIGN KEY ("AppliesToProductId")
    REFERENCES public."Products" ("Id") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;

ALTER TABLE public."StaffProfiles"
    ADD CONSTRAINT "FK_StaffProfiles_Roles_RoleId" FOREIGN KEY ("RoleId")
    REFERENCES public."Roles" ("Id") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE CASCADE;

ALTER TABLE public."StaffProfiles"
    ADD CONSTRAINT "FK_StaffProfiles_Users_UserId" FOREIGN KEY ("UserId")
    REFERENCES public."Users" ("Id") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;

ALTER TABLE public."Vips"
    ADD CONSTRAINT "FK_Vips_Users_UserId" FOREIGN KEY ("UserId")
    REFERENCES public."Users" ("Id") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;

ALTER TABLE public."Warranties"
    ADD CONSTRAINT "FK_Warranties_Products_ProductId" FOREIGN KEY ("ProductId")
    REFERENCES public."Products" ("Id") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE CASCADE;

-- Create indexes
CREATE INDEX "IX_Deliveries_OrderId"
    ON public."Deliveries"("OrderId");

CREATE INDEX "IX_LoyaltyPoints_UserId"
    ON public."LoyaltyPoints"("UserId");

CREATE INDEX "IX_OrderDetails_OrderId"
    ON public."OrderDetails"("OrderId");

CREATE INDEX "IX_Orders_UserId"
    ON public."Orders"("UserId");

CREATE INDEX "IX_Payments_OrderId"
    ON public."Payments"("OrderId");

CREATE INDEX "IX_Products_CategoryId"
    ON public."Products"("CategoryId");

CREATE INDEX "IX_Products_OrderDetailId"
    ON public."Products"("OrderDetailId");

CREATE INDEX "IX_Promotions_AppliesToProductId"
    ON public."Promotions"("AppliesToProductId");

CREATE INDEX "IX_StaffProfiles_RoleId"
    ON public."StaffProfiles"("RoleId");

CREATE INDEX "IX_StaffProfiles_UserId"
    ON public."StaffProfiles"("UserId");

CREATE INDEX "IX_Vips_UserId"
    ON public."Vips"("UserId");

CREATE INDEX "IX_Warranties_ProductId"
    ON public."Warranties"("ProductId");

-- Add unique constraints for 1:1 relationships
ALTER TABLE public."Deliveries"
    ADD CONSTRAINT "IX_Deliveries_OrderId_Unique" UNIQUE ("OrderId");

ALTER TABLE public."LoyaltyPoints"
    ADD CONSTRAINT "IX_LoyaltyPoints_UserId_Unique" UNIQUE ("UserId");

ALTER TABLE public."StaffProfiles"
    ADD CONSTRAINT "IX_StaffProfiles_UserId_Unique" UNIQUE ("UserId");

ALTER TABLE public."Vips"
    ADD CONSTRAINT "IX_Vips_UserId_Unique" UNIQUE ("UserId");

ALTER TABLE public."Warranties"
    ADD CONSTRAINT "IX_Warranties_ProductId_Unique" UNIQUE ("ProductId"); 