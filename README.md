# SupplierCatalogue.DataExtract
The data extract for the Hitched Supplier Catalogue is a console application allowing extract of wedding suppliers details from the Hitched database in JSON format.

## Architecture
The Supplier Catalogue Data Extract is a Console Application written in C# using .NET Core.
To facilitate the posting of the extract data to an API the project also uses a javascript initiated by [Node.js](https://nodejs.org/en/)

For more information see:

* [The Microsoft .NET Core Guide](https://docs.microsoft.com/en-us/dotnet/articles/core/)
* [The Microsoft C# Guide](https://docs.microsoft.com/en-us/dotnet/articles/csharp/)
* [About Node.js](https://nodejs.org/en/about/)


## Data Extract
The supplier data extract is a simple command line application that can be initated with the stndard command `dotnet run` from the project folder. It will perform the extract detailing any status messages in the console window.

### Configuration
Configuration of the extract is held within AppSettings.json in the project folder:
```json
{
    "ApplicationOptions": {
        "Name": "Hitched Suppliers Extract",
        "Version": "1.0"
    },
    "ExtractOptions": {
        "RecordCount": 200,
        "SuppliersOutFilePath": "",
        "SuppliersOutFileName": "Suppliers.json"
    },
    "DBOptions": {
        "ConnectionString": "Data Source=127.0.0.1; Initial Catalog=hitched_local;    User ID=itmSolutions; Password=17m50Lu7!0n$;   Persist Security Info=false; Pooling=false"
    }
}
```

## Import
To facilitate upload of the data once extracted to the Supplier Catalogue API a script has been included in the project (import.js) that can be initiated through Node.js to post the data to the API. Before running the script for the first time you will need to run `npm install`.
To run the script, make sure you have a file named `Suppliers.json` in the same directory then run `npm start`.

## Docker Configuration
The import process is designed to be run inside a Docker container if required.  See the  [Docker Overview](https://docs.docker.com/engine/understanding-docker/)
for more information.

### Pre-Requisites
In order to use the Docker functionality please ensure you first [install Docker](https://docs.docker.com/engine/getstarted/step_one/) in your chosen development environment.

You can check everything is working correctly with the following command:
```cli
docker version
```
### Run

If you want to run this (and generate the Supplier.json) then use the following:

```cli
dotnet restore
dotnet run
```


### Build the docker image
Navigate to the project folder (containing Dockerfile) and issue the following command to build a Docker image of the application:
```cli
docker build -t suppliercatalogue_import .
```
The command takes several seconds to run and reports its outcome.

### Run the docker image
Once the image has been built you can then run it with the following command:
```cli
docker run -p suppliercatalogue_import
```
