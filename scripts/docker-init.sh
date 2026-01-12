#!/bin/bash
# Script para inicializar o SQL Server no Docker
# Este script espera que o SQL Server esteja pronto e depois executa o init-database.sql

echo "Aguardando SQL Server iniciar..."
sleep 30

echo "Executando script de inicialização..."
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -C -i /docker-entrypoint-initdb.d/init-database.sql

echo "Base de dados inicializada!"
