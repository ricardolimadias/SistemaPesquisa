USE [DEV_PESQUISA_SATISFACAO]
GO
BEGIN TRANSACTION;
/****** Object:  Table [dbo].[TB_TipoResposta]    Script Date: 02/03/2019 03:33:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_TipoResposta](
	[TipoRespostaId] [int] IDENTITY(1,1) NOT NULL,
	[TipoResposta] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TipoRespostaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

USE [DEV_PESQUISA_SATISFACAO]
GO

INSERT INTO [dbo].[TB_TipoResposta]
           ([TipoResposta])
     VALUES
           ('text')
GO

USE [DEV_PESQUISA_SATISFACAO]
GO

INSERT INTO [dbo].[TB_TipoResposta]
           ([TipoResposta])
     VALUES
           ('checkbox')
GO

USE [DEV_PESQUISA_SATISFACAO]
GO

INSERT INTO [dbo].[TB_TipoResposta]
           ([TipoResposta])
     VALUES
           ('radio')
GO

USE [DEV_PESQUISA_SATISFACAO]
GO

INSERT INTO [dbo].[TB_TipoResposta]
           ([TipoResposta])
     VALUES
           ('range')
GO

INSERT INTO [dbo].[TB_TipoResposta]
           ([TipoResposta])
     VALUES
           ('estrela')
GO

COMMIT;



