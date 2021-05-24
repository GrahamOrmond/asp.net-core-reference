# ASP.NET Web API with MongoDB
personal reference for asp.net web api using MongoDB

ReadMe copied from  microsft documentation *[Create a web API with ASP.NET Core and MongoDB](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-5.0&tabs=visual-studio)* and [MongoDB documentation](https://docs.mongodb.com/manual/introduction/)

## MongoDB Windows Installation
[MongoDB install guide here](https://docs.mongodb.com/manual/tutorial/install-mongodb-on-windows/)

1. Download MongoDB [here](https://www.mongodb.com/try/download/community?tck=docs_server)
2. Run the installer and select default setting
a. *Install MongoD as a service*
b. *Run service as Network Service user*
c. *Install MongoDB Compass (optional)*

### Installed MongoDB as a Windows Service
The MongoDB service is started upon successful installation 

To begin using MongoDB, connect a [mongo.exe](https://docs.mongodb.com/manual/reference/program/mongo/#mongodb-binary-bin.mongo) shell to the running MongoDB instance. Either:

From Windows Explorer/File Explorer, go to `C:\Program Files\MongoDB\Server\4.4\bin\` directory and double-click on [mongo.exe](https://docs.mongodb.com/manual/reference/program/mongo/#mongodb-binary-bin.mongo).
Or, open a Command Interpreter with Administrative privileges and run:
```
"C:\Program Files\MongoDB\Server\4.4\bin\mongo.exe"
```

### Start MongoDB Service

To start/restart the MongoDB service, use the Services console:
1. From the Services console, locate the MongoDB service.
2. Right-click on the MongoDB service and click **Start**.

To begin using MongoDB, connect a [mongo.exe](https://docs.mongodb.com/manual/reference/program/mongo/#mongodb-binary-bin.mongo) shell to the running MongoDB instance. To connect, open a **Command Interpreter** with Administrative privileges and run:
```
"C:\Program Files\MongoDB\Server\4.4\bin\mongo.exe"
```

### Stop MongoDB Service
To stop/pause the MongoDB service, use the Services console:

1. From the Services console, locate the MongoDB service.
2. Right-click on the MongoDB service and click **Stop** (or **Pause**).

### Remove MongoDB Service
To remove the MongoDB service, first use the Services console to stop the service. Then open a Windows [command prompt/interpreter](https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/cmd) (*cmd.exe*) as an **Administrator**, and run the following command:
```
sc.exe delete MongoDB
```

### Run MongoDB from the Command Interpreter

You can run MongoDB Community Edition from the [Windows command prompt/interpreter](https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/cmd) (*cmd.exe*) instead of as a service.

Open a Windows command prompt/interpreter (*cmd.exe*) as an *Administrator*.

#### 1. Create database directory.
Create the data directory where MongoDB stores data. MongoDB's default data directory path is the absolute path \data\db on the drive from which you start MongoDB.

From the Command Interpreter, create the data directories:
```
cd C:\
md "\data\db"
```

#### 2. Start your MongoDB database.
To start MongoDB, run [exe](https://docs.mongodb.com/manual/reference/program/mongod.exe/#mongodb-binary-bin.mongod.exe).
```
"C:\Program Files\MongoDB\Server\4.4\bin\mongod.exe" --dbpath="c:\data\db"
```
The [--dbpath](https://docs.mongodb.com/manual/reference/program/mongod/#std-option-mongod.--dbpath) option points to your database directory.

If the MongoDB database server is running correctly, the *Command Interpreter* displays:
```
[initandlisten] waiting for connections
```
#### 3. Connect to MongoDB.

To connect a [mongo.exe](https://docs.mongodb.com/manual/reference/program/mongo/#mongodb-binary-bin.mongo) shell to the MongoDB instance, open another **Command Interpreter** with Administrative privileges and run:

```
"C:\Program Files\MongoDB\Server\4.4\bin\mongo.exe"
```

### Additional Considerations
#### Localhost Binding by Default

By default, MongoDB launches with [bindIp](https://docs.mongodb.com/manual/reference/configuration-options/#mongodb-setting-net.bindIp) set to 127.0.0.1, which binds to the localhost network interface.
This means that the mongod.exe can only accept connections from clients that are running on the same machine.
Remote clients will not be able to connect to the mongod.exe, and the mongod.exe will not be able to initialize a [replica](https://docs.mongodb.com/manual/reference/glossary/#std-term-replica-set) set unless this value is set to a valid network interface.


This value can be configured either:
- in the MongoDB configuration file with bindIp, or
- via the command-line argument --bind_ip

#### WARNING
Before binding to a non-localhost (e.g. publicly accessible) IP address, ensure you have secured your cluster from unauthorized access. For a complete list of security recommendations, see [Security Checklist](https://docs.mongodb.com/manual/administration/security-checklist/). At minimum, consider [enabling authentication](https://docs.mongodb.com/manual/administration/security-checklist/#std-label-checklist-auth) and [hardening network infrastructure](https://docs.mongodb.com/manual/core/security-hardening/).

For more information on configuring [bindIp](https://docs.mongodb.com/manual/reference/configuration-options/#mongodb-setting-net.bindIp), see [IP Binding](https://docs.mongodb.com/manual/core/security-mongodb-configuration/).

### Point Releases and .msi

If you installed MongoDB with the Windows installer (*.msi*), the *.msi* automatically upgrades within its [release series](https://docs.mongodb.com/manual/reference/versioning/#std-label-release-version-numbers) (e.g. 4.2.1 to 4.2.2).

### Add MongoDB binaries to the System PATH

All command-line examples in this tutorial are provided as absolute paths to the MongoDB binaries. You can add **C:\Program Files\MongoDB\Server\4.4\bin** to your System **PATH** and then omit the full path to the MongoDB binaries.