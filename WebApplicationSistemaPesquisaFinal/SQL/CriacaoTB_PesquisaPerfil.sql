USE [DEV_PESQUISA_SATISFACAO]
GO
BEGIN TRANSACTION;
/****** Object:  Table [dbo].[TB_PesquisaPerfil]    Script Date: 02/03/2019 03:30:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_PesquisaPerfil](
	[PerfilPesquisaId] [int] IDENTITY(1,1) NOT NULL,
	[PesquisaId] [int] NULL,
	[PerfilId] [int] NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[TB_PesquisaPerfil]  WITH CHECK ADD  CONSTRAINT [FK_TB_PesquisaPerfil_TB_Perfil] FOREIGN KEY([PerfilId])
REFERENCES [dbo].[TB_Perfil] ([PerfilId])
GO

ALTER TABLE [dbo].[TB_PesquisaPerfil] CHECK CONSTRAINT [FK_TB_PesquisaPerfil_TB_Perfil]
GO

ALTER TABLE [dbo].[TB_PesquisaPerfil]  WITH CHECK ADD  CONSTRAINT [FK_TB_PesquisaPerfil_TB_Pesquisa] FOREIGN KEY([PesquisaId])
REFERENCES [dbo].[TB_Pesquisa] ([PesquisaId])
GO

ALTER TABLE [dbo].[TB_PesquisaPerfil] CHECK CONSTRAINT [FK_TB_PesquisaPerfil_TB_Pesquisa]
GO

COMMIT;
