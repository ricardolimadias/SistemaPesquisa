USE [DEV_PESQUISA_SATISFACAO]
GO

/****** Object:  Table [dbo].[TB_QuestaoEncadeada]    Script Date: 06/03/2019 03:12:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_QuestaoEncadeada](
	[QuestaoEncadeadaId] [int] IDENTITY(1,1) NOT NULL,
	[PesquisaId] [int] NULL,
	[QuestaoId] [int] NULL,
	[AlternativaId] [int] NULL,
	[AcaoId] [int] NULL
 CONSTRAINT [PK_TB_QuestaoEncadeada] PRIMARY KEY CLUSTERED 
(
	[QuestaoEncadeadaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO