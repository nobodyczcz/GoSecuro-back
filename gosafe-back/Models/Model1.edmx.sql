
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/27/2019 23:02:48
-- Generated from EDMX file: C:\Users\czcz2\IEProject\gosafe-back\gosafe-back\Models\Model1.edmx
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

IF OBJECT_ID(N'[dbo].[FK_SuburbCrimeRate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CrimeRate] DROP CONSTRAINT [FK_SuburbCrimeRate];
GO
IF OBJECT_ID(N'[dbo].[FK_TempLinkJourney]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TempLink] DROP CONSTRAINT [FK_TempLinkJourney];
GO
IF OBJECT_ID(N'[dbo].[FK_UserProfileTempLink]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TempLink] DROP CONSTRAINT [FK_UserProfileTempLink];
GO
IF OBJECT_ID(N'[dbo].[FK_JTrackingJourney]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JTracking] DROP CONSTRAINT [FK_JTrackingJourney];
GO
IF OBJECT_ID(N'[dbo].[FK_UserProfileUserEmergency]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserEmergency] DROP CONSTRAINT [FK_UserProfileUserEmergency];
GO
IF OBJECT_ID(N'[dbo].[FK_UserProfileJourney]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Journey] DROP CONSTRAINT [FK_UserProfileJourney];
GO
IF OBJECT_ID(N'[dbo].[FK_EmergencyContactUserEmergency]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserEmergency] DROP CONSTRAINT [FK_EmergencyContactUserEmergency];
GO
IF OBJECT_ID(N'[dbo].[FK_EmergencyContactUserProfile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmergencyContact] DROP CONSTRAINT [FK_EmergencyContactUserProfile];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Suburb]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Suburb];
GO
IF OBJECT_ID(N'[dbo].[CrimeRate]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CrimeRate];
GO
IF OBJECT_ID(N'[dbo].[TempLink]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TempLink];
GO
IF OBJECT_ID(N'[dbo].[JTracking]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JTracking];
GO
IF OBJECT_ID(N'[dbo].[UserProfile]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserProfile];
GO
IF OBJECT_ID(N'[dbo].[UserEmergency]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserEmergency];
GO
IF OBJECT_ID(N'[dbo].[EmergencyContact]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmergencyContact];
GO
IF OBJECT_ID(N'[dbo].[Journey]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Journey];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Suburb'
CREATE TABLE [dbo].[Suburb] (
    [SuburbName] nvarchar(128)  NOT NULL,
    [Postcode] int  NULL,
    [Boundary1] nvarchar(max)  NOT NULL,
    [Boundary2] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CrimeRate'
CREATE TABLE [dbo].[CrimeRate] (
    [SuburbSuburbName] nvarchar(128)  NOT NULL,
    [Rate] real  NULL,
    [OffenceCount] int  NULL,
    [Totpopulation] int  NULL
);
GO

-- Creating table 'TempLink'
CREATE TABLE [dbo].[TempLink] (
    [TempLinkId] nvarchar(20)  NOT NULL,
    [JourneyJourneyId] int  NOT NULL,
    [UserProfileId] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'JTracking'
CREATE TABLE [dbo].[JTracking] (
    [JourneyJourneyId] int  NOT NULL,
    [Time] datetime  NOT NULL,
    [CoordLat] float  NOT NULL,
    [CoordLog] float  NOT NULL
);
GO

-- Creating table 'UserProfile'
CREATE TABLE [dbo].[UserProfile] (
    [Id] nvarchar(128)  NOT NULL,
    [Address] nvarchar(max)  NULL,
    [Gender] nvarchar(max)  NULL,
    [FirstName] nvarchar(max)  NULL,
    [LastName] nvarchar(max)  NULL
);
GO

-- Creating table 'UserEmergency'
CREATE TABLE [dbo].[UserEmergency] (
    [EmergencyContactPhone] nvarchar(128)  NOT NULL,
    [UserProfileId] nvarchar(128)  NOT NULL,
    [ECname] nvarchar(max)  NULL
);
GO

-- Creating table 'EmergencyContact'
CREATE TABLE [dbo].[EmergencyContact] (
    [Phone] nvarchar(128)  NOT NULL,
    [UserProfile_Id] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'Journey'
CREATE TABLE [dbo].[Journey] (
    [JourneyId] int IDENTITY(1,1) NOT NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NULL,
    [NavigateRoute] nvarchar(max)  NULL,
    [SCoordLat] float  NOT NULL,
    [SCoordLog] float  NOT NULL,
    [ECoordLat] float  NULL,
    [ECoordLog] float  NULL,
    [Status] nvarchar(max)  NOT NULL,
    [UserProfileId] nvarchar(128)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

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

-- Creating primary key on [TempLinkId] in table 'TempLink'
ALTER TABLE [dbo].[TempLink]
ADD CONSTRAINT [PK_TempLink]
    PRIMARY KEY CLUSTERED ([TempLinkId] ASC);
GO

-- Creating primary key on [JourneyJourneyId], [Time] in table 'JTracking'
ALTER TABLE [dbo].[JTracking]
ADD CONSTRAINT [PK_JTracking]
    PRIMARY KEY CLUSTERED ([JourneyJourneyId], [Time] ASC);
GO

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

-- Creating primary key on [JourneyId] in table 'Journey'
ALTER TABLE [dbo].[Journey]
ADD CONSTRAINT [PK_Journey]
    PRIMARY KEY CLUSTERED ([JourneyId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [SuburbSuburbName] in table 'CrimeRate'
ALTER TABLE [dbo].[CrimeRate]
ADD CONSTRAINT [FK_SuburbCrimeRate]
    FOREIGN KEY ([SuburbSuburbName])
    REFERENCES [dbo].[Suburb]
        ([SuburbName])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [JourneyJourneyId] in table 'TempLink'
ALTER TABLE [dbo].[TempLink]
ADD CONSTRAINT [FK_TempLinkJourney]
    FOREIGN KEY ([JourneyJourneyId])
    REFERENCES [dbo].[Journey]
        ([JourneyId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TempLinkJourney'
CREATE INDEX [IX_FK_TempLinkJourney]
ON [dbo].[TempLink]
    ([JourneyJourneyId]);
GO

-- Creating foreign key on [UserProfileId] in table 'TempLink'
ALTER TABLE [dbo].[TempLink]
ADD CONSTRAINT [FK_UserProfileTempLink]
    FOREIGN KEY ([UserProfileId])
    REFERENCES [dbo].[UserProfile]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserProfileTempLink'
CREATE INDEX [IX_FK_UserProfileTempLink]
ON [dbo].[TempLink]
    ([UserProfileId]);
GO

-- Creating foreign key on [JourneyJourneyId] in table 'JTracking'
ALTER TABLE [dbo].[JTracking]
ADD CONSTRAINT [FK_JTrackingJourney]
    FOREIGN KEY ([JourneyJourneyId])
    REFERENCES [dbo].[Journey]
        ([JourneyId])
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

-- Creating foreign key on [UserProfileId] in table 'Journey'
ALTER TABLE [dbo].[Journey]
ADD CONSTRAINT [FK_UserProfileJourney]
    FOREIGN KEY ([UserProfileId])
    REFERENCES [dbo].[UserProfile]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserProfileJourney'
CREATE INDEX [IX_FK_UserProfileJourney]
ON [dbo].[Journey]
    ([UserProfileId]);
GO

-- Creating foreign key on [EmergencyContactPhone] in table 'UserEmergency'
ALTER TABLE [dbo].[UserEmergency]
ADD CONSTRAINT [FK_EmergencyContactUserEmergency]
    FOREIGN KEY ([EmergencyContactPhone])
    REFERENCES [dbo].[EmergencyContact]
        ([Phone])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserProfile_Id] in table 'EmergencyContact'
ALTER TABLE [dbo].[EmergencyContact]
ADD CONSTRAINT [FK_EmergencyContactUserProfile]
    FOREIGN KEY ([UserProfile_Id])
    REFERENCES [dbo].[UserProfile]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmergencyContactUserProfile'
CREATE INDEX [IX_FK_EmergencyContactUserProfile]
ON [dbo].[EmergencyContact]
    ([UserProfile_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------