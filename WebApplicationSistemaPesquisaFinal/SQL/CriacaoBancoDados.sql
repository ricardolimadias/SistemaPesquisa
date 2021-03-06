USE [master]
GO
BEGIN TRANSACTION;
/****** Object:  Database [DEV_PESQUISA_SATISFACAO]    Script Date: 3/1/2019 2:29:12 PM ******/
CREATE DATABASE [DEV_PESQUISA_SATISFACAO]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DEV_PESQUISA_SATISFACAO', FILENAME = N'H:\SQL_SERVICE\MSSQL11.SERVICE\MSSQL\DATA\DEV_PESQUISA_SATISFACAO.mdf' , SIZE = 3328KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'DEV_PESQUISA_SATISFACAO_log', FILENAME = N'H:\SQL_SERVICE\MSSQL11.SERVICE\MSSQL\DATA\DEV_PESQUISA_SATISFACAO.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET COMPATIBILITY_LEVEL = 110
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DEV_PESQUISA_SATISFACAO].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET ANSI_NULL_DEFAULT ON 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET ANSI_NULLS ON 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET ANSI_PADDING ON 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET ANSI_WARNINGS ON 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET ARITHABORT ON 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET CURSOR_DEFAULT  LOCAL 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET CONCAT_NULL_YIELDS_NULL ON 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET QUOTED_IDENTIFIER ON 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET  DISABLE_BROKER 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET RECOVERY FULL 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET  MULTI_USER 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET DB_CHAINING OFF 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [DEV_PESQUISA_SATISFACAO] SET  READ_WRITE 
GO


COMMIT;