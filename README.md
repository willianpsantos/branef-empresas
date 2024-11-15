# branef company api
API to manipulate companies information.

Dependencies:
    * Docker
    * SQL Server
    * MongoDB
    * RabbitMQ

If you want to run project dependencies as docker containers, please run the following commands in your docker instance:

# RabbitMQ
docker run --hostname=dc495ef6c38d --mac-address=02:42:ac:11:00:06 --env=PATH=/opt/rabbitmq/sbin:/opt/erlang/bin:/opt/openssl/bin:/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin --env=ERLANG_INSTALL_PATH_PREFIX=/opt/erlang --env=OPENSSL_INSTALL_PATH_PREFIX=/opt/openssl --env=RABBITMQ_DATA_DIR=/var/lib/rabbitmq --env=RABBITMQ_VERSION=4.0.3 --env=RABBITMQ_PGP_KEY_ID=0x0A9AF2115F4687BD29803A206B73A36E6026DFCA --env=RABBITMQ_HOME=/opt/rabbitmq --env=HOME=/var/lib/rabbitmq --env=LANG=C.UTF-8 --env=LANGUAGE=C.UTF-8 --env=LC_ALL=C.UTF-8 --volume=/var/lib/rabbitmq --network=bridge -p 15672:15672 -p 5672:5672 --restart=no --label='org.opencontainers.image.ref.name=ubuntu' --label='org.opencontainers.image.version=24.04' --runtime=runc -d rabbitmq:4.0-management

# MongoDB

docker run --hostname=65839cecda03 --mac-address=02:42:ac:11:00:04 --env=PATH=/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin --env=GOSU_VERSION=1.17 --env=JSYAML_VERSION=3.13.1 --env=MONGO_PACKAGE=mongodb-org --env=MONGO_REPO=repo.mongodb.org --env=MONGO_MAJOR=7.0 --env=MONGO_VERSION=7.0.12 --env=HOME=/data/db --volume=/data/configdb --volume=/data/db --network=bridge -p 27017:27017 --restart=no --label='org.opencontainers.image.ref.name=ubuntu' --label='org.opencontainers.image.version=22.04' --runtime=runc -d mongo:latest

# SQL Server

docker run --hostname=225cba5a3bb0 --user=mssql --mac-address=02:42:ac:11:00:03 --env=ACCEPT_EULA=Y --env=MSSQL_SA_PASSWORD=@Wips294802 --env=PATH=/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin --env=MSSQL_RPC_PORT=135 --env=CONFIG_EDGE_BUILD= --env=MSSQL_PID=developer --network=bridge -p 1433:1433 --restart=no --label='com.microsoft.product=Microsoft SQL Server' --label='com.microsoft.version=16.0.4125.3' --label='org.opencontainers.image.ref.name=ubuntu' --label='org.opencontainers.image.version=22.04' --label='vendor=Microsoft' --runtime=runc -t -d mcr.microsoft.com/mssql/server:2022-latest


[1] Create databases

a. In your SQL server instance, create a database called branef (feel free to choose some other name)
b. (Optional) Again in your SQL server instance, create a database called branef_logs. This db is going to be use by Serilog. You have the option to use the same application database (branef) for register application logs, in case you need it.


[2] Run the EF core migrations

a. The project has a solution file named "sharedsettings.json", put the proper connection string to your database, setting up WriteDb and ReadDb connectionstrings.
b. Go to project Branef.Empresas.DB.
c. In the appsettings.json file, find the "ConnectionStrings" section. 
d. Put the connection string to the database you've created for application.
e. Go to the terminal.
f. In terminal, navigate to Branef.Empresas.DB folder.

g. If the migrations are not present into Migrations folder, please run the following command:

    dotnet ef migrations add Initial -c Branef.Empresas.DB.BranefWriteDbContext -o .\Migrations\

h. Then, run the following command:

    dotnet ef database update -c Branef.Empresas.DB.BranefWriteDbContext



[3] Set up your connection string

a. Go to project Branef.Empresas.API and then find the file appsettings.Development.json. 
a. In the appsettings.Development.json file, find the "ConnectionStrings" section. 
b. Put the connection string to the database you've created for application.
c. Find the "Serilog" section.
d. Into "Serilog" section, find "WriteTo" section, and then a configuration with name "MSSqlServer".
e. Into "MSSqlServer" section, find "connectionString" key.
f. Put yor serilog database connection string in there.


[4] Set up your RabbitMQ settings

a. Go to project Branef.Empresas.API.
b. Go to appsettings.Development.json file.
c. In the appsettings.Development.json file, find the "MessageBroker" section. 
d. Put your RabbitMQ host into "Host" key, your rabbitmq username in "Username" key and you rabbitmq password in "Password" key