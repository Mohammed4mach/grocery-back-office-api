all: build run

watch:
	dotnet watch run

run:
	dotnet run

build:
	dotnet build

debug:
	dotnet build --configuration Debug

release:
	dotnet build --configuration Release

clean:
	dotnet clean

migrate:
	dotnet fsi ./src/Infrastructure/Scripts/migrate.fsx

migrate-fresh:
	dotnet fsi ./src/Infrastructure/Scripts/migrate-fresh.fsx

migrate-rollback:
	dotnet fsi ./src/Infrastructure/Scripts/migrate-rollback.fsx

seed:
	dotnet fsi ./src/Infrastructure/Scripts/seed.fsx

project: migrate-fresh seed

