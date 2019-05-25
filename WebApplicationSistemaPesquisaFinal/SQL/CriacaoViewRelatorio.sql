USE [DEV_PESQUISA_SATISFACAO]
GO

/****** Object:  View [dbo].[ViewRelatorio]    Script Date: 5/24/2019 5:46:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewRelatorio]
AS
SELECT DISTINCT 
                         dbo.TB_Participantes.RDM, dbo.TB_Pesquisa.Titulo, dbo.TB_Pesquisa.Descricao, dbo.TB_Questoes.Questao, dbo.TB_Alternativas.Alternativa, dbo.TB_Participantes.Nome, 
                         dbo.TB_DataEnvioDataResposta.DataEnvio, dbo.TB_DataEnvioDataResposta.DataResposta, dbo.TB_Respostas.Resposta, dbo.TB_VigenciaResposta.QuantidadeDias, 
                         dbo.TB_VigenciaPesquisa.DataInicialPesquisa, dbo.TB_VigenciaPesquisa.DataFinalPesquisa, dbo.TB_Pesquisa.PesquisaId
FROM            dbo.TB_Participantes INNER JOIN
                         dbo.TB_DataEnvioDataResposta ON dbo.TB_Participantes.ParticipanteId = dbo.TB_DataEnvioDataResposta.ParticipanteId INNER JOIN
                         dbo.TB_Pesquisa ON dbo.TB_Participantes.PesquisaId = dbo.TB_Pesquisa.PesquisaId INNER JOIN
                         dbo.TB_Questoes ON dbo.TB_Pesquisa.PesquisaId = dbo.TB_Questoes.PesquisaId INNER JOIN
                         dbo.TB_Alternativas ON dbo.TB_Questoes.QuestaoId = dbo.TB_Alternativas.QuestaoId INNER JOIN
                         dbo.TB_Respostas ON dbo.TB_Participantes.ParticipanteId = dbo.TB_Respostas.ParticipanteId AND dbo.TB_Questoes.QuestaoId = dbo.TB_Respostas.QuestaoId INNER JOIN
                         dbo.TB_VigenciaPesquisa ON dbo.TB_Pesquisa.PesquisaId = dbo.TB_VigenciaPesquisa.PesquisaId INNER JOIN
                         dbo.TB_VigenciaResposta ON dbo.TB_Pesquisa.PesquisaId = dbo.TB_VigenciaResposta.PesquisaId

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[15] 2[13] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "TB_Participantes"
            Begin Extent = 
               Top = 10
               Left = 607
               Bottom = 140
               Right = 777
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "TB_DataEnvioDataResposta"
            Begin Extent = 
               Top = 140
               Left = 462
               Bottom = 270
               Right = 632
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "TB_Pesquisa"
            Begin Extent = 
               Top = 1
               Left = 37
               Bottom = 114
               Right = 207
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TB_Questoes"
            Begin Extent = 
               Top = 0
               Left = 238
               Bottom = 130
               Right = 408
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TB_Alternativas"
            Begin Extent = 
               Top = 5
               Left = 433
               Bottom = 118
               Right = 603
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TB_Respostas"
            Begin Extent = 
               Top = 0
               Left = 780
               Bottom = 130
               Right = 950
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TB_VigenciaPesquisa"
            Begin Extent = 
               Top = 130
               Left = 36
               Bottom = 260
               Right ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewRelatorio'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'= 226
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TB_VigenciaResposta"
            Begin Extent = 
               Top = 136
               Left = 254
               Bottom = 249
               Right = 445
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 15
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewRelatorio'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewRelatorio'
GO


