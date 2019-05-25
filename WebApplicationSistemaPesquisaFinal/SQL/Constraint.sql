Use DEV_PESQUISA_SATISFACAO2
GO

BEGIN TRANSACTION;

Alter Table [dbo].[TB_Questoes]
add constraint FK_TB_Questoes_TB_Pesquisa Foreign Key (PesquisaId) references TB_Pesquisa (PesquisaId),
constraint FK_TB_Questoes_TB_TipoResposta Foreign Key (TipoRespostaId) references TB_TipoResposta (TipoRespostaId)

Alter Table [dbo].[TB_Alternativas]
add constraint FK_TB_Alternativas_TB_Questoes Foreign Key (QuestaoId) references TB_Questoes (QuestaoId)

Alter Table [dbo].[TB_Respostas]
add constraint FK_TB_Respostas_TB_Questoes Foreign Key (QuestaoId) references TB_Questoes (QuestaoId),
constraint FK_TB_Respostas_TB_Participantes Foreign Key (ParticipanteId) references TB_Participantes (ParticipanteId)

Alter Table [dbo].[TB_Participantes]
add constraint FK_TB_Participantes_TB_Pesquisa Foreign Key (PesquisaId) references TB_Pesquisa (PesquisaId)

Alter Table [dbo].[TB_Acesso]
add constraint FK_TB_Acesso_TB_Perfil Foreign Key (PerfilId) references TB_Perfil (PerfilId)

Alter Table [dbo].[TB_PesquisaPerfil]
add constraint FK_TB_PesquisaPerfil_TB_Pesquisa Foreign Key (PesquisaId) references TB_Pesquisa (PesquisaId),
constraint FK_TB_PesquisaPerfil_TB_Perfil Foreign Key (PerfilId) references TB_Perfil (PerfilId)

Alter Table [dbo].[TB_MensagemEmail]
add constraint FK_TB_MensagemEmail_TB_Pesquisa Foreign Key (PesquisaId) references TB_Pesquisa (PesquisaId)

Alter Table [dbo].[TB_VigenciaPesquisa]
add constraint FK_TB_VigenciaPesquisa_TB_Pesquisa Foreign Key (PesquisaId) references TB_Pesquisa (PesquisaId)

Alter Table [dbo].[TB_VigenciaResposta]
add constraint FK_TB_VigenciaResposta_TB_Pesquisa Foreign Key (PesquisaId) references TB_Pesquisa (PesquisaId)
--Versão 2 -------------------------------------------------------------------------------------------------------------------
--Alter Table [dbo].[TB_VigenciaResposta]
--add constraint FK_TB_VigenciaResposta_TB_Pesquisa Foreign Key (PesquisaId) references TB_Pesquisa (PesquisaId)--,
--constraint FK_TB_VigenciaResposta_TB_Participantes Foreign Key (ParticipanteId) references TB_Participantes (ParticipanteId)
------------------------------------------------------------------------------------------------------------------------------
Alter Table [dbo].[TB_QuestaoEncadeada]
add constraint FK_TB_QuestaoEncadeada_TB_Pesquisa Foreign Key (PesquisaId) references TB_Pesquisa (PesquisaId),
constraint FK_TB_QuestaoEncadeada_TB_Questoes Foreign Key (QuestaoId) references TB_Questoes (QuestaoId),
constraint FK_TB_QuestaoEncadeada_TB_Alternativas Foreign Key (AlternativaId) references TB_Alternativas (AlternativaId),
constraint FK_TB_QuestaoEncadeada_TB_AcaoQuestaoEncadeada Foreign Key (AcaoId) references TB_AcaoQuestaoEncadeada (AcaoId)

Alter Table [dbo].[TB_Relatorio]
add constraint FK_TB_Relatorio_TB_Pesquisa Foreign Key (PesquisaId) references TB_Pesquisa (PesquisaId),
constraint FK_TB_Relatorio_TB_Questoes Foreign Key (QuestaoId) references TB_Questoes (QuestaoId),
constraint FK_TB_Relatorio_TB_Alternativas Foreign Key (AlternativaId) references TB_Alternativas (AlternativaId),
constraint FK_TB_Relatorio_TB_Participantes Foreign Key (ParticipanteId) references TB_Participantes (ParticipanteId),
constraint FK_TB_Relatorio_TB_VigenciaResposta Foreign Key (VigenciaRespostaId) references TB_VigenciaResposta (VigenciaRespostaId),
constraint FK_TB_Relatorio_TB_Respostas Foreign Key (RespostaId) references TB_Respostas (RespostaId)

--Deletar Constraint
--ALTER TABLE dbo.TB_PesquisaPerfil   
--DROP CONSTRAINT FK_TB_PesquisaPerfil_TB_Pesquisa;
--GO 
COMMIT;