all: watch

watch:
	dotnet watch run

run:
	dotnet run

build:
	dotnet build

build-debug:
	dotnet build --configuration Debug

build-release:
	dotnet build --configuration release

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

