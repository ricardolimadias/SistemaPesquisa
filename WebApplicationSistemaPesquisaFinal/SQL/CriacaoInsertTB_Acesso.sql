USE [DEV_PESQUISA_SATISFACAO]
GO
BEGIN TRANSACTION;
/****** Object:  Table [dbo].[TB_Acesso]    Script Date: 3/1/2019 2:30:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TB_Acesso](
	[LoginId] [int] IDENTITY(1,1) NOT NULL,
	[Chave] [varchar](100) NULL,
	[Senha] [varchar](100) NULL,
	[Ativo] [char](1) NULL,
	[PerfilId] [int] NULL,
	[Nome] [varchar](30) NULL,
	[Sobrenome] [varchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[LoginId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING ON
GO

ALTER TABLE [dbo].[TB_Acesso]  WITH CHECK ADD  CONSTRAINT [FK_TB_Acesso_TB_Perfil] FOREIGN KEY([PerfilId])
REFERENCES [dbo].[TB_Perfil] ([PerfilId])
GO

ALTER TABLE [dbo].[TB_Acesso] CHECK CONSTRAINT [FK_TB_Acesso_TB_Perfil]
GO

USE [DEV_PESQUISA_SATISFACAO]
GO

INSERT INTO [dbo].[TB_Acesso]
           ([Chave]
           ,[Senha]
           ,[Ativo]
           ,[PerfilId]
           ,[Nome]
           ,[Sobrenome])
     VALUES
           ('AdmTI'
           ,'123456'
           ,'S'
           ,1
           ,'Admin'
           ,'Sistema')
GO

INSERT INTO [dbo].[TB_Acesso]
           ([Chave]
           ,[Senha]
           ,[Ativo]
           ,[PerfilId]
           ,[Nome]
           ,[Sobrenome])
     VALUES
           ('AdmGARTI'
           ,'123456'
           ,'S'
           ,2
           ,'Admin'
           ,'GARTI')
GO

INSERT INTO [dbo].[TB_Acesso]
           ([Chave]
           ,[Senha]
           ,[Ativo]
           ,[PerfilId]
           ,[Nome]
           ,[Sobrenome])
     VALUES
           ('AdmGPCO'
           ,'123456'
           ,'S'
           ,3
           ,'Admin'
           ,'GPCO')
GO
INSERT INTO [dbo].[TB_Acesso]
           ([Chave]
           ,[Senha]
           ,[Ativo]
           ,[PerfilId]
           ,[Nome]
           ,[Sobrenome])
     VALUES
           ('GARTI'
           ,'123456'
           ,'S'
           ,4
           ,'Usuario'
           ,'GARTI')
GO
INSERT INTO [dbo].[TB_Acesso]
           ([Chave]
           ,[Senha]
           ,[Ativo]
           ,[PerfilId]
           ,[Nome]
           ,[Sobrenome])
     VALUES
           ('GPCO'
           ,'123456'
           ,'S'
           ,5
           ,'Usuario'
           ,'GPCO')
GO

COMMIT;