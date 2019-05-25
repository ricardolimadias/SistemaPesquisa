USE [DEV_PESQUISA_SATISFACAO]
GO
BEGIN TRANSACTION;
/****** Object:  Table [dbo].[TB_VigenciaResposta]    Script Date: 02/03/2019 03:36:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_VigenciaResposta](
	[VigenciaRespostaId] [int] IDENTITY(1,1) NOT NULL,
	[PesquisaId] [int] NULL,
	[DataInicialResposta] [date] NULL,
	[DataFinalResposta] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[VigenciaRespostaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[TB_VigenciaResposta]  WITH CHECK ADD  CONSTRAINT [FK_TB_VigenciaResposta_TB_Pesquisa] FOREIGN KEY([PesquisaId])
REFERENCES [dbo].[TB_Pesquisa] ([PesquisaId])
GO

ALTER TABLE [dbo].[TB_VigenciaResposta] CHECK CONSTRAINT [FK_TB_VigenciaResposta_TB_Pesquisa]
GO

COMMIT;
--Versão 2 Usar essa-------------------------------------------------------------------------------------------------------------------
USE [DEV_PESQUISA_SATISFACAO]
GO

/****** Object:  Table [dbo].[TB_VigenciaResposta]    Script Date: 3/18/2019 10:21:25 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_VigenciaResposta](
	[VigenciaRespostaId] [int] IDENTITY(1,1) NOT NULL,
	[PesquisaId] [int] NULL,
	--[DataEnvioPesquisa] [date] NULL,
	--[ParticipanteId] [int] NULL,
	[QuantidadeDias] [int] NULL,
 CONSTRAINT [PK_TB_VigenciaResposta] PRIMARY KEY CLUSTERED 
(
	[VigenciaRespostaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[TB_VigenciaResposta]  WITH CHECK ADD  CONSTRAINT [FK_TB_VigenciaResposta_TB_Pesquisa] FOREIGN KEY([PesquisaId])
REFERENCES [dbo].[TB_Pesquisa] ([PesquisaId])
GO

ALTER TABLE [dbo].[TB_VigenciaResposta] CHECK CONSTRAINT [FK_TB_VigenciaResposta_TB_Pesquisa]
GO
------------------------------------------------------------------------------------------------------------------------------
