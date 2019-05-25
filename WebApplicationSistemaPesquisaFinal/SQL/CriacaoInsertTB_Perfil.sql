USE [DEV_PESQUISA_SATISFACAO]
GO
BEGIN TRANSACTION;
/****** Object:  Table [dbo].[TB_Perfil]    Script Date: 02/03/2019 03:26:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_Perfil](
	[PerfilId] [int] IDENTITY(1,1) NOT NULL,
	[Perfil] [varchar](15) NULL,
PRIMARY KEY CLUSTERED 
(
	[PerfilId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [DEV_PESQUISA_SATISFACAO]
GO

INSERT INTO [dbo].[TB_Perfil]
           ([Perfil])
     VALUES
           ('AdmTI')
GO

USE [DEV_PESQUISA_SATISFACAO]
GO

INSERT INTO [dbo].[TB_Perfil]
           ([Perfil])
     VALUES
           ('AdmGARTI')
GO

USE [DEV_PESQUISA_SATISFACAO]
GO

INSERT INTO [dbo].[TB_Perfil]
           ([Perfil])
     VALUES
           ('AdmGPCO')
GO

USE [DEV_PESQUISA_SATISFACAO]
GO

INSERT INTO [dbo].[TB_Perfil]
           ([Perfil])
     VALUES
           ('GARTI')
GO

USE [DEV_PESQUISA_SATISFACAO]
GO

INSERT INTO [dbo].[TB_Perfil]
           ([Perfil])
     VALUES
           ('GPCO')
GO

COMMIT;


