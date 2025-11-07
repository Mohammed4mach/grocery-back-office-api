all: watch

watch:
	dotnet watch run

build:
	dotnet build

run:
	dotnet run

migrate:
	dotnet fsi ./src/Infrastructure/Scripts/migrate.fsx

migrate-fresh:
	dotnet fsi ./src/Infrastructure/Scripts/migrate-fresh.fsx

migrate-rollback:
	dotnet fsi ./src/Infrastructure/Scripts/migrate-rollback.fsx

project: migrate-fresh seed

seed:
	dotnet fsi ./src/Infrastructure/Scripts/seed.fsx

