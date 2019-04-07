
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/07/2019 21:46:10
-- Generated from EDMX file: C:\Users\Jennifer\Desktop\IEproject\gosafe-back\gosafe-back\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [gosafe-back20190407071339_db];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_EmergencyContactUserEmergency]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserEmergency] DROP CONSTRAINT [FK_EmergencyContactUserEmergency];
GO
IF OBJECT_ID(N'[dbo].[FK_UserProfileUserEmergency]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserEmergency] DROP CONSTRAINT [FK_UserProfileUserEmergency];
GO
IF OBJECT_ID(N'[dbo].[FK_SuburbCrimeRate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CrimeRate] DROP CONSTRAINT [FK_SuburbCrimeRate];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserProfile]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserProfile];
GO
IF OBJECT_ID(N'[dbo].[UserEmergency]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserEmergency];
GO
IF OBJECT_ID(N'[dbo].[EmergencyContact]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmergencyContact];
GO
IF OBJECT_ID(N'[dbo].[Suburb]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Suburb];
GO
IF OBJECT_ID(N'[dbo].[CrimeRate]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CrimeRate];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserProfile'
CREATE TABLE [dbo].[UserProfile] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [address] nvarchar(max)  NOT NULL,
    [gender] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UserEmergency'
CREATE TABLE [dbo].[UserEmergency] (
    [EmergencyContactPhone] int IDENTITY(1,1) NOT NULL,
    [UserProfileId] int  NOT NULL
);
GO

-- Creating table 'EmergencyContact'
CREATE TABLE [dbo].[EmergencyContact] (
    [Phone] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Suburb'
CREATE TABLE [dbo].[Suburb] (
    [SuburbName] nvarchar(max)  NOT NULL,
    [Postcode] nvarchar(max)  NOT NULL,
    [Boundary1] nvarchar(max)  NOT NULL,
    [Boundary2] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CrimeRate'
CREATE TABLE [dbo].[CrimeRate] (
    [SuburbSuburbName] nvarchar(max)  NOT NULL,
    [Rate] nchar(4000)  NOT NULL,
    [OffenceCount] nvarchar(max)  NOT NULL,
    [Totpopulation] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'UserProfile'
ALTER TABLE [dbo].[UserProfile]
ADD CONSTRAINT [PK_UserProfile]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [EmergencyContactPhone], [UserProfileId] in table 'UserEmergency'
ALTER TABLE [dbo].[UserEmergency]
ADD CONSTRAINT [PK_UserEmergency]
    PRIMARY KEY CLUSTERED ([EmergencyContactPhone], [UserProfileId] ASC);
GO

-- Creating primary key on [Phone] in table 'EmergencyContact'
ALTER TABLE [dbo].[EmergencyContact]
ADD CONSTRAINT [PK_EmergencyContact]
    PRIMARY KEY CLUSTERED ([Phone] ASC);
GO

-- Creating primary key on [SuburbName] in table 'Suburb'
ALTER TABLE [dbo].[Suburb]
ADD CONSTRAINT [PK_Suburb]
    PRIMARY KEY CLUSTERED ([SuburbName] ASC);
GO

-- Creating primary key on [SuburbSuburbName] in table 'CrimeRate'
ALTER TABLE [dbo].[CrimeRate]
ADD CONSTRAINT [PK_CrimeRate]
    PRIMARY KEY CLUSTERED ([SuburbSuburbName] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [EmergencyContactPhone] in table 'UserEmergency'
ALTER TABLE [dbo].[UserEmergency]
ADD CONSTRAINT [FK_EmergencyContactUserEmergency]
    FOREIGN KEY ([EmergencyContactPhone])
    REFERENCES [dbo].[EmergencyContact]
        ([Phone])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserProfileId] in table 'UserEmergency'
ALTER TABLE [dbo].[UserEmergency]
ADD CONSTRAINT [FK_UserProfileUserEmergency]
    FOREIGN KEY ([UserProfileId])
    REFERENCES [dbo].[UserProfile]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserProfileUserEmergency'
CREATE INDEX [IX_FK_UserProfileUserEmergency]
ON [dbo].[UserEmergency]
    ([UserProfileId]);
GO

-- Creating foreign key on [SuburbSuburbName] in table 'CrimeRate'
ALTER TABLE [dbo].[CrimeRate]
ADD CONSTRAINT [FK_SuburbCrimeRate]
    FOREIGN KEY ([SuburbSuburbName])
    REFERENCES [dbo].[Suburb]
        ([SuburbName])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------