-- Script para criar a base de dados e tabela de Reservas
-- Executar no SQL Server Management Studio ou Azure Data Studio

-- Criar base de dados (se não existir)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ReservationsDb')
BEGIN
    CREATE DATABASE ReservationsDb;
END
GO

USE ReservationsDb;
GO

-- Criar tabela Reservations (se não existir)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Reservations' AND xtype='U')
BEGIN
    CREATE TABLE Reservations (
        Id INT PRIMARY KEY IDENTITY(1,1),
        CustomerName NVARCHAR(100) NOT NULL,
        ReservationDate DATE NOT NULL,
        ReservationTime TIME NOT NULL,
        TableNumber INT NOT NULL,
        NumberOfPeople INT NOT NULL,
        CreatedAt DATETIME DEFAULT GETDATE()
    );
    
    PRINT 'Tabela Reservations criada com sucesso!';
END
ELSE
BEGIN
    PRINT 'Tabela Reservations já existe.';
END
GO

-- Criar índice para pesquisas por data (opcional, melhora performance)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Reservations_Date')
BEGIN
    CREATE INDEX IX_Reservations_Date ON Reservations(ReservationDate);
END
GO

-- Inserir dados de exemplo (opcional)
-- Descomenta as linhas abaixo se quiseres dados de teste

/*
INSERT INTO Reservations (CustomerName, ReservationDate, ReservationTime, TableNumber, NumberOfPeople)
VALUES 
    ('João Silva', '2026-01-15', '19:00', 1, 4),
    ('Maria Santos', '2026-01-15', '20:00', 2, 2),
    ('Pedro Costa', '2026-01-16', '19:30', 1, 6),
    ('Ana Rodrigues', '2026-01-16', '20:00', 3, 3);
*/

PRINT 'Script executado com sucesso!';
GO
