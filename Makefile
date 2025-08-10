build:
	dotnet build
clean:
	dotnet clean
restore:
	dotnet restore
watch:
	dotnet watch run --project ./FreeEnterprise.Api/FreeEnterprise.Api.csproj
start:
	dotnet run --project ./FreeEnterprise.Api/FreeEnterprise.Api.csproj
watch-test:
	cd ./FreeEnterprise.Api.UnitTests && dotnet watch test
outdated:
	dotnet outdated -exc FluentAssertions
outdated-upgrade:
	dotnet outdated -exc FluentAssertions -u