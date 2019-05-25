USE [DEV_PESQUISA_SATISFACAO]
GO
BEGIN TRANSACTION;
/****** Object:  Table [dbo].[TB_Respostas]    Script Date: 02/03/2019 03:32:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_Respostas](
	[RespostaId] [int] IDENTITY(1,1) NOT NULL,
	[QuestaoId] [int] NOT NULL,
	[AlternativaId] [int] NOT NULL,
	[Resposta] [nvarchar](max) NOT NULL,
	[ParticipanteId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RespostaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[TB_Respostas]  WITH CHECK ADD  CONSTRAINT [FK_TB_Respostas_TB_Participantes] FOREIGN KEY([ParticipanteId])
REFERENCES [dbo].[TB_Participantes] ([ParticipanteId])
GO

ALTER TABLE [dbo].[TB_Respostas] CHECK CONSTRAINT [FK_TB_Respostas_TB_Participantes]
GO

ALTER TABLE [dbo].[TB_Respostas]  WITH CHECK ADD  CONSTRAINT [FK_TB_Respostas_TB_Questoes] FOREIGN KEY([QuestaoId])
REFERENCES [dbo].[TB_Questoes] ([QuestaoId])
GO

ALTER TABLE [dbo].[TB_Respostas] CHECK CONSTRAINT [FK_TB_Respostas_TB_Questoes]
GO

COMMIT;
