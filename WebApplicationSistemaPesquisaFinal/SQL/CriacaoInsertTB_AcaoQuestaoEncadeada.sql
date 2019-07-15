USE [DEV_PESQUISA_SATISFACAO]
GO

/****** Object:  Table [dbo].[TB_AcaoQuestaoEncadeada]    Script Date: 06/03/2019 13:55:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_AcaoQuestaoEncadeada](
	[AcaoId] [int] IDENTITY(1,1) NOT NULL,
	[Acao] [varchar](max) NULL,
 CONSTRAINT [PK_TB_AcaoQuestaoEncadeada] PRIMARY KEY CLUSTERED 
(
	[AcaoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO




INSERT INTO [dbo].[TB_AcaoQuestaoEncadeada]
           ([Acao])
     VALUES
           ('Parar Resposta Formulario')
GO

--INSERT INTO [dbo].[TB_AcaoQuestaoEncadeada]
--           ([Acao])
--     VALUES
--           ('Continuar Resposta Formulario')
--GO

--INSERT INTO [dbo].[TB_AcaoQuestaoEncadeada]
--           ([Acao])
--     VALUES
--           ('Abrir Nova Questão')
--GO

--INSERT INTO [dbo].[TB_AcaoQuestaoEncadeada]
--           ([Acao])
--     VALUES
--           ('Fechar Nova Questão')
--GO