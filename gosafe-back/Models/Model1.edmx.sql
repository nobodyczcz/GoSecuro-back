
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/01/2019 21:14:02
-- Generated from EDMX file: C:\Users\czcz2\IEProject\gosafe-back\gosafe-back\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Gosafe-Database];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserProfileSet'
CREATE TABLE [dbo].[UserProfileSet] (
    [UserID] int IDENTITY(1,1) NOT NULL,
    [address] nvarchar(max)  NOT NULL,
    [gender] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UserEmergencySet'
CREATE TABLE [dbo].[UserEmergencySet] (
    [UserProfileUserID] int  NOT NULL,
    [EmergencyContactContactPhone] int  NOT NULL
);
GO

-- Creating table 'EmergencyContactSet'
CREATE TABLE [dbo].[EmergencyContactSet] (
    [ContactPhone] int IDENTITY(1,1) NOT NULL,
    [ContactName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SuburbSet'
CREATE TABLE [dbo].[SuburbSet] (
    [SuburbName] int IDENTITY(1,1) NOT NULL,
    [Postcode] nvarchar(max)  NOT NULL,
    [Boundary] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CrimeRateSet'
CREATE TABLE [dbo].[CrimeRateSet] (
    [SuburbName] int  NOT NULL,
    [Rate] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [UserID] in table 'UserProfileSet'
ALTER TABLE [dbo].[UserProfileSet]
ADD CONSTRAINT [PK_UserProfileSet]
    PRIMARY KEY CLUSTERED ([UserID] ASC);
GO

-- Creating primary key on [UserProfileUserID], [EmergencyContactContactPhone] in table 'UserEmergencySet'
ALTER TABLE [dbo].[UserEmergencySet]
ADD CONSTRAINT [PK_UserEmergencySet]
    PRIMARY KEY CLUSTERED ([UserProfileUserID], [EmergencyContactContactPhone] ASC);
GO

-- Creating primary key on [ContactPhone] in table 'EmergencyContactSet'
ALTER TABLE [dbo].[EmergencyContactSet]
ADD CONSTRAINT [PK_EmergencyContactSet]
    PRIMARY KEY CLUSTERED ([ContactPhone] ASC);
GO

-- Creating primary key on [SuburbName] in table 'SuburbSet'
ALTER TABLE [dbo].[SuburbSet]
ADD CONSTRAINT [PK_SuburbSet]
    PRIMARY KEY CLUSTERED ([SuburbName] ASC);
GO

-- Creating primary key on [SuburbName] in table 'CrimeRateSet'
ALTER TABLE [dbo].[CrimeRateSet]
ADD CONSTRAINT [PK_CrimeRateSet]
    PRIMARY KEY CLUSTERED ([SuburbName] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserProfileUserID] in table 'UserEmergencySet'
ALTER TABLE [dbo].[UserEmergencySet]
ADD CONSTRAINT [FK_UserProfileUserEmergency]
    FOREIGN KEY ([UserProfileUserID])
    REFERENCES [dbo].[UserProfileSet]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [EmergencyContactContactPhone] in table 'UserEmergencySet'
ALTER TABLE [dbo].[UserEmergencySet]
ADD CONSTRAINT [FK_EmergencyContactUserEmergency]
    FOREIGN KEY ([EmergencyContactContactPhone])
    REFERENCES [dbo].[EmergencyContactSet]
        ([ContactPhone])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmergencyContactUserEmergency'
CREATE INDEX [IX_FK_EmergencyContactUserEmergency]
ON [dbo].[UserEmergencySet]
    ([EmergencyContactContactPhone]);
GO

-- Creating foreign key on [SuburbName] in table 'CrimeRateSet'
ALTER TABLE [dbo].[CrimeRateSet]
ADD CONSTRAINT [FK_SuburbCrimeRate]
    FOREIGN KEY ([SuburbName])
    REFERENCES [dbo].[SuburbSet]
        ([SuburbName])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------