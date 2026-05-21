# NinjaOne.Client

A generated .NET client for the NinjaOne/NinjaRMM API v2.

## Installation

dotnet add package NinjaOne.Client

## Authentication

NinjaOne uses OAuth2 client credentials. Valid scopes: monitoring, management, control

## Regional Endpoints

US: https://app.ninjarmm.com
US2: https://us2.ninjarmm.com
EU: https://eu.ninjarmm.com
CA: https://ca.ninjarmm.com
AUS: https://app.ninjarmm.com.au

## Running Tests

Unit tests: dotnet test

Functional tests require credentials:
dotnet user-secrets --id ninjaone-client-tests set NinjaOne:ClientId your-id
dotnet user-secrets --id ninjaone-client-tests set NinjaOne:ClientSecret your-secret

## License

MIT
