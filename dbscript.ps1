docker run -e "ACCEPT_EULA=Y" `
   -e "MSSQL_SA_PASSWORD=StrongP@ssw0rd!" `
   -p 1433:1433 `
   --name fagprove `
   -v fagprove_data:/var/opt/mssql `
   -d mcr.microsoft.com/mssql/server:2022-latest
