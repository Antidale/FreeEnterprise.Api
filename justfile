set default-list := true
oudatedParams := ''
folderPrefix := 'FreeEnterprise.Api'

# runs dotnet build
build:
	dotnet build

# run dotnet watch on the default csproj
watch:
	dotnet watch run --project ./{{folderPrefix}}/{{folderPrefix}}.csproj

# runs dotnet run on the default csproj
run:
	dotnet run --project ./{{folderPrefix}}/{{folderPrefix}}.csproj

# dotnet test -c Release
test:
	dotnet test -c Release

# cd to the unit tests directory and runs dotnet watch test
watch-test:
	cd ./{{folderPrefix}}.UnitTests && dotnet watch test

# runs dotnet outdated. excludes FluentAssertions by default. pass -u or -u:Prompt to upgrade, additinally pass other params
outdated params=oudatedParams:
	dotnet outdated -exc FluentAssertions {{params}}
