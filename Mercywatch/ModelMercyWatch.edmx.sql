
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/24/2017 23:14:00
-- Generated from EDMX file: C:\Users\Валентин\Documents\Visual Studio 2017\Projects\Mercywatch\Mercywatch\ModelMercyWatch.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MercyWatch];
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

-- Creating table 'PlayersSet'
CREATE TABLE [dbo].[PlayersSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Tag] nvarchar(max)  NOT NULL,
    [Rate] nvarchar(max)  NOT NULL,
    [WinRate] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'HeroesSet'
CREATE TABLE [dbo].[HeroesSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [WinRate] nvarchar(max)  NOT NULL,
    [Players_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'PlayersSet'
ALTER TABLE [dbo].[PlayersSet]
ADD CONSTRAINT [PK_PlayersSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HeroesSet'
ALTER TABLE [dbo].[HeroesSet]
ADD CONSTRAINT [PK_HeroesSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Players_Id] in table 'HeroesSet'
ALTER TABLE [dbo].[HeroesSet]
ADD CONSTRAINT [FK_PlayersHeroes]
    FOREIGN KEY ([Players_Id])
    REFERENCES [dbo].[PlayersSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayersHeroes'
CREATE INDEX [IX_FK_PlayersHeroes]
ON [dbo].[HeroesSet]
    ([Players_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------