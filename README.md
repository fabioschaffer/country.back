API para salvar dados do país. Arquitetura:

Aplicação:
 - Framework .NET Core 3.1
 - Habilitada autenticação 'Basic'.
 - Swagger habilitado via pacote Swashbuckle.AspNeCore (6.1.1). Para visualizar, acessar http://countryapplicationapi.azurewebsites.net.
 
Banco de dados:
 - Utiliza banco de dados Microsoft SQL Server (do Azure).
 - Acesso ao banco de dados via pacote Microsoft.Data.SqlClient (2.1.2)
 - Abstração de dados de dados via pacote Dapper (2.0.78).

Publicação:
 - API está publicada no Azure em http://countryapplicationapi.azurewebsites.net.