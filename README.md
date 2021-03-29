API para salvar dados do país. Arquitetura:

Aplicação:
 - Framework .NET Core 3.1.
 - Habilitada autenticação 'Basic'.
 - Swagger habilitado via pacote Swashbuckle.AspNeCore (6.1.1). Para visualizar, acessar http://countryapplicationapi.azurewebsites.net.
 
Banco de dados:
 - Utiliza banco de dados Microsoft SQL Server (do Azure).
 - Acesso ao banco de dados via pacote Microsoft.Data.SqlClient (2.1.2).
 - Abstração para conectar ao banco e retornar os dados via pacote Dapper (2.0.78).
 - Script para criação das tabelas está em Database_Script.sql.

Publicação:
 - API está publicada no Azure em http://countryapplicationapi.azurewebsites.net.

Ambiente de desenvolvimento:
 - Foi utilizado o Microsoft Microsoft Visual Studio Community 2019 (16.8.4).

Executar a aplicação:
 - Para executar a aplicação, abrir o arquivo country.back.sln no Visual Studio.
 - Caso seja necessário modificar a conexão ao banco de dados, alterar em appsettings.json, chave 'ConnectionString'.
 - Executar a aplicação (Start Debugging).