USE [DEV_PESQUISA_SATISFACAO]
GO
BEGIN TRANSACTION;
/****** Object:  Table [dbo].[TB_MensagemEmail]    Script Date: 02/03/2019 03:24:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_MensagemEmail](
	[TextoEmailId] [int] IDENTITY(1,1) NOT NULL,
	[PesquisaId] [int] NULL,
	[Mensagem] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[TextoEmailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[TB_MensagemEmail]  WITH CHECK ADD  CONSTRAINT [FK_TB_MensagemEmail_TB_Pesquisa] FOREIGN KEY([PesquisaId])
REFERENCES [dbo].[TB_Pesquisa] ([PesquisaId])
GO

ALTER TABLE [dbo].[TB_MensagemEmail] CHECK CONSTRAINT [FK_TB_MensagemEmail_TB_Pesquisa]
GO

COMMIT;

