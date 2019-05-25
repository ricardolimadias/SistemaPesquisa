USE [DEV_PESQUISA_SATISFACAO]
GO
BEGIN TRANSACTION;
/****** Object:  Table [dbo].[TB_Participantes]    Script Date: 02/03/2019 03:25:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_Participantes](
	[ParticipanteId] [int] IDENTITY(1,1) NOT NULL,
	[PesquisaId] [int] NOT NULL,
	[Nome] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ParticipanteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[TB_Participantes]  WITH CHECK ADD  CONSTRAINT [FK_TB_Participantes_TB_Pesquisa] FOREIGN KEY([PesquisaId])
REFERENCES [dbo].[TB_Pesquisa] ([PesquisaId])
GO

ALTER TABLE [dbo].[TB_Participantes] CHECK CONSTRAINT [FK_TB_Participantes_TB_Pesquisa]
GO

COMMIT;
-----Versão 2 Usar essa-----
USE [DEV_PESQUISA_SATISFACAO]
GO

/****** Object:  Table [dbo].[TB_Participantes]    Script Date: 5/6/2019 10:45:28 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TB_Participantes](
	[ParticipanteId] [int] IDENTITY(1,1) NOT NULL,
	[PesquisaId] [int] NOT NULL,
	[Nome] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[RDM] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ParticipanteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING ON
GO

ALTER TABLE [dbo].[TB_Participantes]  WITH CHECK ADD  CONSTRAINT [FK_TB_Participantes_TB_Pesquisa] FOREIGN KEY([PesquisaId])
REFERENCES [dbo].[TB_Pesquisa] ([PesquisaId])
GO

ALTER TABLE [dbo].[TB_Participantes] CHECK CONSTRAINT [FK_TB_Participantes_TB_Pesquisa]
GO


