FROM microsoft/dotnet:latest
WORKDIR /app
COPY . .

ENV ExtractOptions__ExtractMode 'object'
ENV ExtractOptions__RecordCount 100
ENV ExtractOptions__ApiDestination http://api:8080/api/v1/
ENV DBOptions__ConnectionString 'Data Source=ec2-54-171-141-64.eu-west-1.compute.amazonaws.com; Initial Catalog=hitched_gb;    User ID=hitchedweb; Password=d7hju85%T;   Persist Security Info=false; Pooling=false'

RUN dotnet restore
WORKDIR /app/SupplierCatalogue.DataExtract
RUN dotnet build --configuration Release
# CMD ["dotnet","run","--configuration","Release"]