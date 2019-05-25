USE [DEV_PESQUISA_SATISFACAO]
GO

/****** Object:  Table [dbo].[TB_DataEnvioDataResposta]    Script Date: 4/18/2019 2:01:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_DataEnvioDataResposta](
	[EnvioId] [int] IDENTITY(1,1) NOT NULL,
	[PesquisaId] [int] NULL,
	[ParticipanteId] [int] NULL,
	[DataEnvio] [date] NULL,
	[DataResposta] [date] NULL,
 CONSTRAINT [PK_TB_DataEnvioDataResposta] PRIMARY KEY CLUSTERED 
(
	[EnvioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


