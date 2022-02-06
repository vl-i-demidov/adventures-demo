dotnet publish ../Demo.Adventures.sln -c Release
docker compose -f ./docker-compose.yml build
docker compose -f ./docker-compose.yml --profile full up -d
Start-Process "http://localhost:5000/swagger"