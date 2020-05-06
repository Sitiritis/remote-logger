# Description

AIR service in C# ASP.NetCore.

A small in-memory service to store logs in a hierarchical manner received from remote hosts. It is designed to receive many logs from many hosts simultaneously.

## Project structure

- `AIR` - backend of the service.
- `RemoteLogger` - implementation of the client.
- `LoggerTest` - a sample console application that demonstrates the usage of the client.

# How to run

```bash
# Start the service
cd ./AIR
dotnet run

# Run the sample console app to populate the service with some data
cd ../LoggerTest
dotnet run
```

The service will be available on port **51228**.

Open [http://localhost:51228/Log/](http://localhost:51228/Log/) in a browser to see the currently saved logs.

The documentation for the API is available at [http://localhost:51228/swagger/index.html](http://localhost:51228/swagger/index.html).
