"# My project's README" 

````````

# Uk.Parliament

A comprehensive .NET library for accessing UK Parliament APIs including Petitions, Members, Bills, Committees, and Divisions.

[![NuGet](https://img.shields.io/nuget/v/Uk.Parliament.svg)](https://www.nuget.org/packages/Uk.Parliament/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Overview

**Version 2.0** - Complete rewrite using Refit for type-safe REST API access to all publicly available UK Parliament APIs.

This library provides a unified, intuitive interface to interact with multiple UK Parliament data sources:
- ? **Petitions API** - Public petitions to Parliament (COMPLETE)
- ?? **Members API** - MPs, Lords, Constituencies, Parties (IN PROGRESS)
- ? **Bills API** - Parliamentary legislation (PLANNED)
- ? **Committees API** - Committee inquiries and submissions (PLANNED)
- ? **Divisions APIs** - Voting records (Commons & Lords) (PLANNED)

## Installation

Install the package via NuGet:

```bash
dotnet add package Uk.Parliament
```

Or via the Package Manager Console:

```powershell
Install-Package Uk.Parliament
```

## Quick Start

### Using the Unified Client

```csharp
using Uk.Parliament;
using Uk.Parliament.Extensions;

// Create a unified client for all Parliament APIs
var client = new ParliamentClient();

// Access Petitions API
var petitions = await client.Petitions.GetAsync(state: "open", pageSize: 10);
foreach (var petition in petitions.Data)
{
    Console.WriteLine($"{petition.Attributes.Action}: {petition.Attributes.SignatureCount} signatures");
}

// Get a specific petition
var petition = await client.Petitions.GetByIdAsync(700143);
Console.WriteLine($"Petition: {petition.Data.Attributes.Action}");

// Access Members API (when complete)
// var members = await client.Members.SearchAsync(name: "Smith", take: 10);
```

### Using Extension Methods for Pagination

```csharp
// Stream all petitions (memory efficient)
await foreach (var petition in client.Petitions.GetAllAsync(state: "open", pageSize: 20))
{
    Console.WriteLine($"{petition.Attributes.Action}");
    // Process one at a time
}

// Or get all as a materialized list
var allPetitions = await client.Petitions.GetAllListAsync(state: "closed", pageSize: 50);
Console.WriteLine($"Total closed petitions: {allPetitions.Count}");
```

### Dependency Injection (ASP.NET Core)

```csharp
// In Program.cs or Startup.cs
services.AddSingleton<ParliamentClient>();

// Or with custom options
services.AddSingleton(sp => new ParliamentClient(new ParliamentClientOptions
{
    UserAgent = "MyApp/1.0",
    Timeout = TimeSpan.FromSeconds(30),
    EnableDebugValidation = false
}));

// Then inject into your services
public class MyService
{
    private readonly ParliamentClient _parliament;

    public MyService(ParliamentClient parliament)
    {
        _parliament = parliament;
    }

    public async Task GetPetitionsAsync()
    {
        var petitions = await _parliament.Petitions.GetAsync(state: "open");
        // Use petitions...
    }
}
```

## API Coverage

### ? Petitions API (COMPLETE)

Access public petitions to UK Parliament.

```csharp
// Search petitions
var response = await client.Petitions.GetAsync(
    search: "climate",
    state: "open",
    page: 1,
    pageSize: 20);

// Get specific petition
var petition = await client.Petitions.GetByIdAsync(123456);

// Access petition details
Console.WriteLine($"Action: {petition.Data.Attributes.Action}");
Console.WriteLine($"Signatures: {petition.Data.Attributes.SignatureCount}");
Console.WriteLine($"State: {petition.Data.Attributes.State}");

// Get signatures by country
foreach (var country in petition.Data.Attributes.SignaturesByCountry)
{
    Console.WriteLine($"{country.Name}: {country.SignatureCount}");
}

// Archived petitions
var archived = await client.Petitions.GetArchivedAsync(state: "closed", pageSize: 10);
```

**Endpoints:**
- `GET /petitions.json` - List current petitions
- `GET /petitions/{id}.json` - Get specific petition
- `GET /archived/petitions.json` - List archived petitions
- `GET /archived/petitions/{id}.json` - Get specific archived petition

**Status:** 27/27 tests passing ?

### ?? Members API (IN PROGRESS)

Access information about MPs, Lords, constituencies, and political parties.

```csharp
// Search for members
var members = await client.Members.SearchAsync(
    name: "Smith",
    house: 1, // 1 = Commons, 2 = Lords
    isCurrentMember: true,
    take: 10);

// Get specific member
var member = await client.Members.GetByIdAsync(172);
Console.WriteLine($"Name: {member.NameFullTitle}");
Console.WriteLine($"Party: {member.LatestParty?.Name}");

// Search constituencies
var constituencies = await client.Members.SearchConstituenciesAsync(
    searchText: "London",
    take: 10);

// Get specific constituency
var constituency = await client.Members.GetConstituencyByIdAsync(4074);
```

**Endpoints:**
- `GET /api/Members/Search` - Search members
- `GET /api/Members/{id}` - Get member details
- `GET /api/Location/Constituency/Search` - Search constituencies
- `GET /api/Location/Constituency/{id}` - Get constituency details

**Status:** Models created, interface implemented, 4/17 tests passing (models need minor fixes) ??

### ? Bills API (PLANNED)

Access parliamentary legislation information.

```csharp
// When implemented:
// var bills = await client.Bills.GetBillsAsync(take: 10);
// var bill = await client.Bills.GetBillByIdAsync(123);
// var stages = await client.Bills.GetBillStagesAsync(123);
// var sessions = await client.Bills.GetSessionsAsync();
```

**Status:** Interface defined, test templates ready (12 tests) ?

### ? Committees API (PLANNED)

Access parliamentary committee information and inquiries.

```csharp
// When implemented:
// var committees = await client.Committees.GetCommitteesAsync();
// var committee = await client.Committees.GetCommitteeByIdAsync(1);
// var inquiries = await client.Committees.GetCommitteeInquiriesAsync(1);
```

**Status:** Interface defined, test templates ready (11 tests) ?

### ? Divisions APIs (PLANNED)

Access voting records from House of Commons and House of Lords.

```csharp
// When implemented:
// var divisions = await client.CommonsDivisions.GetDivisionsAsync();
// var division = await client.CommonsDivisions.GetDivisionByIdAsync(123);
// var lordsDivisions = await client.LordsDivisions.GetDivisionsAsync();
```

**Status:** Interfaces defined, test templates ready (15 tests) ?

## Configuration Options

```csharp
var options = new ParliamentClientOptions
{
    // Custom user agent (default: "Uk.Parliament.NET/2.0")
    UserAgent = "MyApp/1.0",
    
    // Request timeout (default: 30 seconds)
    Timeout = TimeSpan.FromSeconds(60),
    
    // Enable strict JSON validation (default: true in DEBUG, false in RELEASE)
    EnableDebugValidation = true,
    
    // Base URLs for each API (defaults provided)
    PetitionsBaseUrl = "https://petition.parliament.uk/",
    MembersBaseUrl = "https://members-api.parliament.uk/",
    // ... etc
};

var client = new ParliamentClient(options);
```

## Extension Methods

All APIs support pagination helpers:

```csharp
// Streaming (memory efficient)
await foreach (var item in api.GetAllAsync(pageSize: 50))
{
    // Process one item at a time
}

// Materialized list
var allItems = await api.GetAllListAsync(pageSize: 50);
```

## Error Handling

The library uses Refit's exception handling:

```csharp
using Refit;

try
{
    var petition = await client.Petitions.GetByIdAsync(123456);
}
catch (ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
{
    Console.WriteLine("Petition not found");
}
catch (ApiException ex)
{
    Console.WriteLine($"API Error: {ex.StatusCode} - {ex.Content}");
}
```

## Requirements

- .NET 10.0 or later
- Refit 8.0.0 or later

## Building from Source

```bash
git clone https://github.com/panoramicdata/Uk.Parliament.git
cd Uk.Parliament
dotnet build
dotnet test
```

## Test Suite

The library includes comprehensive tests:

- **81 total tests** across all APIs
- **39 tests passing** (Petitions + unit tests)
- **12 tests failing** (Members - models being fixed)
- **30 tests skipped** (APIs not yet implemented)

```bash
# Run all tests
dotnet test

# Run specific API tests
dotnet test --filter "FullyQualifiedName~Petitions"
```

## Development Status

| API | Status | Tests | Completion |
|-----|--------|-------|------------|
| Petitions | ? Complete | 27/27 passing | 100% |
| Members | ?? In Progress | 4/17 passing | 95% |
| Bills | ? Planned | 2/12 passing | 20% |
| Committees | ? Planned | 2/11 passing | 20% |
| Commons Divisions | ? Planned | 2/8 passing | 20% |
| Lords Divisions | ? Planned | 2/7 passing | 20% |

See [MASTER_PLAN.md](MASTER_PLAN.md) for detailed implementation roadmap.

## Documentation

- [MASTER_PLAN.md](MASTER_PLAN.md) - Complete project roadmap
- [PARLIAMENT_APIS.md](PARLIAMENT_APIS.md) - Overview of all UK Parliament APIs
- [REFIT.md](REFIT.md) - Migration to Refit architecture
- [TEST_SUITE_OVERVIEW.md](TEST_SUITE_OVERVIEW.md) - Comprehensive test documentation
- [FINAL_TEST_SUMMARY.md](FINAL_TEST_SUMMARY.md) - Current test results

## Migration from v1.x

Version 2.0 is a complete rewrite with breaking changes:

**v1.x:**
```csharp
var client = new PetitionsClient();
var result = await client.GetPetitionAsync(123, CancellationToken.None);
if (result.Ok) { /* use result.Data */ }
```

**v2.0:**
```csharp
var client = new ParliamentClient();
var petition = await client.Petitions.GetByIdAsync(123);
// Direct access, throws ApiException on error
```

Key changes:
- ? Removed `Result<T>` wrapper - use exceptions instead
- ? Removed `Query` class - use method parameters
- ? Added `ParliamentClient` unified client
- ? Added Refit-based type-safe interfaces
- ? Added support for multiple Parliament APIs
- ? Improved extension methods
- ? Better async/await patterns

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

### Priority Areas:
1. Fix Members API models (30 minutes)
2. Implement Bills API (~2 hours)
3. Implement Committees API (~2 hours)
4. Implement Divisions APIs (~3 hours)

See [MASTER_PLAN.md](MASTER_PLAN.md) for details.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Copyright

Copyright © 2025 Panoramic Data Limited

## Related Links

- [UK Parliament Petitions](https://petition.parliament.uk/)
- [UK Parliament Members API](https://members-api.parliament.uk/)
- [UK Parliament Bills API](https://bills-api.parliament.uk/)
- [UK Parliament Committees API](https://committees-api.parliament.uk/)
- [e-Petitions GitHub](https://github.com/alphagov/e-petitions)
- [Other UK Parliament APIs](PARLIAMENT_APIS.md)

## Support

For issues, questions, or suggestions, please [open an issue](https://github.com/panoramicdata/Uk.Parliament/issues) on GitHub.
