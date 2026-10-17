-- Script CadEtec (Material 03 - Criando o Banco de Dados)
-- Execute no SQL Server Management Studio (F5)

IF DB_ID('CadEtec') IS NULL
    CREATE DATABASE CadEtec;
GO

USE CadEtec;
GO

IF OBJECT_ID('dbo.cliente', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[cliente](
        [idCliente] [numeric](18, 0) IDENTITY(1,1) PRIMARY KEY NOT NULL,
        [nome]      [varchar](30) NOT NULL,
        [endereco]  [varchar](50) NOT NULL,
        [telefone]  [varchar](30) NOT NULL,
        [cpf]       [numeric](11, 0) NOT NULL
    );
END
GO
