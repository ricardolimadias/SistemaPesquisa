USE [DEV_PESQUISA_SATISFACAO]
GO
BEGIN TRANSACTION;
/****** Object:  Table [dbo].[TB_VigenciaPesquisa]    Script Date: 02/03/2019 03:35:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_VigenciaPesquisa](
	[VigenciaPesquisaId] [int] IDENTITY(1,1) NOT NULL,
	[PesquisaId] [int] NULL,
	[DataInicialPesquisa] [date] NULL,
	[DataFinalPesquisa] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[VigenciaPesquisaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[TB_VigenciaPesquisa]  WITH CHECK ADD  CONSTRAINT [FK_TB_VigenciaPesquisa_TB_Pesquisa] FOREIGN KEY([PesquisaId])
REFERENCES [dbo].[TB_Pesquisa] ([PesquisaId])
GO

ALTER TABLE [dbo].[TB_VigenciaPesquisa] CHECK CONSTRAINT [FK_TB_VigenciaPesquisa_TB_Pesquisa]
GO

COMMIT;
