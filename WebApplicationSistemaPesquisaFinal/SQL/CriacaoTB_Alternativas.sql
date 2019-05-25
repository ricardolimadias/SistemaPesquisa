USE [DEV_PESQUISA_SATISFACAO]
GO
BEGIN TRANSACTION;
/****** Object:  Table [dbo].[TB_Alternativas]    Script Date: 02/03/2019 03:22:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_Alternativas](
	[AlternativaId] [int] IDENTITY(1,1) NOT NULL,
	[QuestaoId] [int] NOT NULL,
	[Alternativa] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AlternativaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[TB_Alternativas]  WITH CHECK ADD  CONSTRAINT [FK_TB_Alternativas_TB_Questoes] FOREIGN KEY([QuestaoId])
REFERENCES [dbo].[TB_Questoes] ([QuestaoId])
GO

ALTER TABLE [dbo].[TB_Alternativas] CHECK CONSTRAINT [FK_TB_Alternativas_TB_Questoes]
GO

COMMIT;
