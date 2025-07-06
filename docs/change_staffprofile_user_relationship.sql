-- Script to change StaffProfiles.UserId relationship from 1:1 to 1:optional
-- This makes the UserId column nullable while keeping the unique constraint
-- This allows users to optionally have a staff profile (1:optional relationship)

-- First, drop the foreign key constraint temporarily
ALTER TABLE public."StaffProfiles" 
    DROP CONSTRAINT IF EXISTS "FK_StaffProfiles_Users_UserId";

-- Make the UserId column nullable
ALTER TABLE public."StaffProfiles" 
    ALTER COLUMN "UserId" DROP NOT NULL;

-- Re-add the foreign key constraint (now nullable)
ALTER TABLE public."StaffProfiles"
    ADD CONSTRAINT "FK_StaffProfiles_Users_UserId" FOREIGN KEY ("UserId")
    REFERENCES public."Users" ("Id") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;

-- Verify the changes
DO $$
BEGIN
    -- Check if UserId is nullable
    IF EXISTS (
        SELECT 1 FROM information_schema.columns 
        WHERE table_name = 'StaffProfiles' 
        AND column_name = 'UserId' 
        AND is_nullable = 'YES'
    ) THEN
        RAISE NOTICE 'Successfully made StaffProfiles.UserId nullable';
    ELSE
        RAISE EXCEPTION 'Failed to make StaffProfiles.UserId nullable';
    END IF;
    
    -- Check if unique constraint still exists
    IF EXISTS (
        SELECT 1 FROM information_schema.table_constraints 
        WHERE constraint_name = 'IX_StaffProfiles_UserId_Unique' 
        AND table_name = 'StaffProfiles'
    ) THEN
        RAISE NOTICE 'Unique constraint on StaffProfiles.UserId is maintained';
    ELSE
        RAISE EXCEPTION 'Unique constraint on StaffProfiles.UserId was lost';
    END IF;
END $$;

-- Optional: Add a comment to document the change
COMMENT ON COLUMN public."StaffProfiles"."UserId" IS 'Foreign key to Users table - optional staff profile per user (1:optional relationship)'; 