
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/13/2021 16:14:46
-- Generated from EDMX file: C:\Users\Tioh\source\repos\AdminSystem\AdminSystem\AdminSystemModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [AdminSystem];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_TeacherStudent_Student]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TeacherStudents] DROP CONSTRAINT [FK_TeacherStudent_Student];
GO
IF OBJECT_ID(N'[dbo].[FK_TeacherStudent_Teacher]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TeacherStudents] DROP CONSTRAINT [FK_TeacherStudent_Teacher];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Students]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Students];
GO
IF OBJECT_ID(N'[dbo].[Teachers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Teachers];
GO
IF OBJECT_ID(N'[dbo].[TeacherStudents]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TeacherStudents];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Students'
CREATE TABLE [dbo].[Students] (
    [StudentID] int IDENTITY(1,1) NOT NULL,
    [EmailAddress] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Teachers'
CREATE TABLE [dbo].[Teachers] (
    [TeacherID] int IDENTITY(1,1) NOT NULL,
    [EmailAddress] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'TeacherStudents'
CREATE TABLE [dbo].[TeacherStudents] (
    [TeacherStudentID] int IDENTITY(1,1) NOT NULL,
    [TeacherID] int  NOT NULL,
    [StudentID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [StudentID] in table 'Students'
ALTER TABLE [dbo].[Students]
ADD CONSTRAINT [PK_Students]
    PRIMARY KEY CLUSTERED ([StudentID] ASC);
GO

-- Creating primary key on [TeacherID] in table 'Teachers'
ALTER TABLE [dbo].[Teachers]
ADD CONSTRAINT [PK_Teachers]
    PRIMARY KEY CLUSTERED ([TeacherID] ASC);
GO

-- Creating primary key on [TeacherStudentID] in table 'TeacherStudents'
ALTER TABLE [dbo].[TeacherStudents]
ADD CONSTRAINT [PK_TeacherStudents]
    PRIMARY KEY CLUSTERED ([TeacherStudentID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [StudentID] in table 'TeacherStudents'
ALTER TABLE [dbo].[TeacherStudents]
ADD CONSTRAINT [FK_TeacherStudent_Student]
    FOREIGN KEY ([StudentID])
    REFERENCES [dbo].[Students]
        ([StudentID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TeacherStudent_Student'
CREATE INDEX [IX_FK_TeacherStudent_Student]
ON [dbo].[TeacherStudents]
    ([StudentID]);
GO

-- Creating foreign key on [TeacherID] in table 'TeacherStudents'
ALTER TABLE [dbo].[TeacherStudents]
ADD CONSTRAINT [FK_TeacherStudent_Teacher]
    FOREIGN KEY ([TeacherID])
    REFERENCES [dbo].[Teachers]
        ([TeacherID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TeacherStudent_Teacher'
CREATE INDEX [IX_FK_TeacherStudent_Teacher]
ON [dbo].[TeacherStudents]
    ([TeacherID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------