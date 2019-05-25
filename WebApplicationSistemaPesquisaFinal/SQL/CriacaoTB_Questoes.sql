USE [DEV_PESQUISA_SATISFACAO]
GO

/****** Object:  Table [dbo].[TB_Questoes]    Script Date: 5/21/2019 9:12:37 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_Questoes](
	[QuestaoId] [int] IDENTITY(1,1) NOT NULL,
	[PesquisaId] [int] NOT NULL,
	[Questao] [nvarchar](max) NOT NULL,
	[TipoRespostaId] [int] NOT NULL,
	[Obrigatorio] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[QuestaoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[TB_Questoes]  WITH CHECK ADD  CONSTRAINT [FK_TB_Questoes_TB_Pesquisa] FOREIGN KEY([PesquisaId])
REFERENCES [dbo].[TB_Pesquisa] ([PesquisaId])
GO

ALTER TABLE [dbo].[TB_Questoes] CHECK CONSTRAINT [FK_TB_Questoes_TB_Pesquisa]
GO

ALTER TABLE [dbo].[TB_Questoes]  WITH CHECK ADD  CONSTRAINT [FK_TB_Questoes_TB_TipoResposta] FOREIGN KEY([TipoRespostaId])
REFERENCES [dbo].[TB_TipoResposta] ([TipoRespostaId])
GO

ALTER TABLE [dbo].[TB_Questoes] CHECK CONSTRAINT [FK_TB_Questoes_TB_TipoResposta]
GO


