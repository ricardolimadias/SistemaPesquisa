USE [DEV_PESQUISA_SATISFACAO]
GO

/****** Object:  Table [dbo].[TB_Relatorio]    Script Date: 06/03/2019 22:40:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_Relatorio](
	[RelatorioId] [int] IDENTITY(1,1) NOT NULL,
	[PesquisaId] [int] NULL,
	[AlternativaId] [int] NULL,
	[QuestaoId] [int] NULL,
	[ParticipanteId] [int] NULL,
	[VigenciaRespostaId] [int] NULL,
	[RespostaId] [int] NULL,
 CONSTRAINT [PK_TB_Relatorio] PRIMARY KEY CLUSTERED 
(
	[RelatorioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO