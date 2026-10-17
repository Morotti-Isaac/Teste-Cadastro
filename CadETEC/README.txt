CadETEC - Programação WEB com Banco de Dados (Prof. Valnei H. Reis)
====================================================================

COMO EXECUTAR
1. No SQL Server Management Studio abra "Script CadEtec.sql" e execute (F5).
   Ele cria o banco CadEtec e a tabela cliente.
2. Abra CadETEC.sln no Visual Studio 2022 (precisa do .NET Framework 4.8.1
   e da carga de trabalho "ASP.NET e desenvolvimento para a Web").
3. Edite a string de conexão "CadEtec" em CadETEC\Web.config e ajuste o
   Data Source para o seu servidor (ex.: MARTE-PC\Marte).
4. Build > Restore NuGet Packages (automático ao compilar) e pressione F5.
   O site abre em Inicio/Index; use o menu "Clientes".

ESTRUTURA (Materiais 01 a 09)
 Model.Entity  -> Cliente.cs (entidade, [Display]/[Required], construtores)
 Model.Dao     -> ConexaoDB.cs, Intermediario.cs (interface),
                  ClienteDao.cs (create, delete, update, find, findAll)
 Model.Neg     -> ClienteNeg.cs (validações e códigos de "estado")
 CadETEC (Web) -> RouteConfig (rota padrão Inicio/Index), InicioController,
                  ClienteController, Views (Inicio, Cliente), _Layout

CÓDIGOS DE ESTADO (camada Neg)
 99 sucesso | 1 erro de banco | 2/20 nome | 6/60 endereço | 7/70 telefone
 5/50 CPF | 8 duplicado | 33 não encontrado | 10 sem código | 100 erro geral

ATIVIDADE PRÁTICA (Material 09)
 Views\Cliente\Index.cshtml usa Bootstrap 5 (cards, tabela responsiva,
 badges, alertas, ícones e campo de pesquisa).
