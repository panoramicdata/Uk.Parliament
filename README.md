"# My project's README" 

````````

# Uk.Parliament

A .NET library for accessing the UK Parliament Petitions API.

[![NuGet](https://img.shields.io/nuget/v/Uk.Parliament.svg)](https://www.nuget.org/packages/Uk.Parliament/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Overview

This library provides a simple and intuitive way to interact with the UK Parliament Petitions API. It allows you to retrieve information about petitions submitted to the UK Parliament.

## Installation

Install the package via NuGet:

```bash
dotnet add package Uk.Parliament
```

Or via the Package Manager Console:

```powershell
Install-Package Uk.Parliament
```

## Usage

### Basic Example

```csharp
using Uk.Parliament.Petitions;

// Create a client
var client = new PetitionsClient();

// Get a single petition by ID
var result = await client.GetPetitionAsync(petitionId: 123456, CancellationToken.None);
if (result.Ok)
{
    var petition = result.Data;
    Console.WriteLine($"Title: {petition.Attributes.Action}");
    Console.WriteLine($"Signatures: {petition.Attributes.SignatureCount}");
}

// Get multiple petitions with a query
var query = new Query
{
    State = PetitionState.Open,
    PageSize = 50
};

var petitionsResult = await client.GetPetitionsAsync(query, CancellationToken.None);
if (petitionsResult.Ok)
{
    foreach (var petition in petitionsResult.Data)
    {
        Console.WriteLine($"{petition.Attributes.Action} - {petition.Attributes.SignatureCount} signatures");
    }
}
```

### Query Options

You can filter petitions using the `Query` class:

```csharp
var query = new Query
{
    State = PetitionState.Open,  // Open, Closed, Rejected, etc.
    PageSize = 50,
    Page = 1
};
```

### Working with Results

The library uses a `Result<T>` pattern for error handling:

```csharp
var result = await client.GetPetitionAsync(123456, CancellationToken.None);

if (result.Ok)
{
    // Access the petition data
    var petition = result.Data;
}
else
{
    // Handle the error
    Console.WriteLine($"Error: {result.Exception?.Message}");
}
```

## Features

- ? Retrieve individual petitions by ID
- ? Query multiple petitions with filters
- ? Support for petition states (Open, Closed, Rejected, etc.)
- ? Access to petition signatures, government responses, and debate information
- ? Country-level signature data
- ? Async/await support with cancellation tokens
- ? Built on .NET 10.0
- ? Full debugging support with SourceLink
- ? Symbol packages (snupkg) published to NuGet.org

## Debugging

This package includes full source debugging support through SourceLink and symbol packages (.snupkg).

### Enable Source Link in Visual Studio

1. Go to **Tools ? Options ? Debugging ? General**
2. Uncheck **Enable Just My Code**
3. Check **Enable Source Link support**
4. Check **Enable source server support** (if needed)

### Enable Source Link in Visual Studio Code

Add to your `launch.json`:

```json
{
    "justMyCode": false,
    "suppressJITOptimizations": true,
    "symbolOptions": {
        "searchPaths": [],
        "searchMicrosoftSymbolServer": true,
        "searchNuGetOrgSymbolServer": true
    }
}
```

### Enable Source Link in JetBrains Rider

1. Go to **Settings ? Build, Execution, Deployment ? Debugger**
2. Enable **Enable external sources debug**
3. Enable **Enable NuGet symbol servers**

Once configured, you can step into the library code during debugging, and the debugger will automatically download the source code from GitHub.

## API Reference

### PetitionsClient

The main client for interacting with the UK Parliament Petitions API.

#### Methods

- `GetPetitionAsync(int petitionId, CancellationToken cancellationToken)` - Get a single petition by its ID
- `GetPetitionsAsync(Query query, CancellationToken cancellationToken)` - Get multiple petitions based on query parameters

### Models

- `Petition` - Represents a petition with all its attributes
- `PetitionAttributes` - Contains petition details (action, background, signatures, etc.)
- `PetitionState` - Enumeration of petition states (Open, Closed, Rejected, etc.)
- `PetitionGovernmentResponse` - Government response to a petition
- `PetitionDebate` - Debate information for a petition
- `CountrySignatures` - Signature counts by country
- `Query` - Query parameters for filtering petitions

## Requirements

- .NET 10.0 or later

## Building from Source

```bash
git clone https://github.com/panoramicdata/Uk.Parliament.git
cd Uk.Parliament
dotnet build
dotnet test
```

## Publishing

The package is configured to automatically generate:
- NuGet package (.nupkg)
- Symbol package (.snupkg) with embedded source code for debugging
- XML documentation for IntelliSense support

Symbol packages are automatically published to NuGet.org's symbol server, enabling source debugging.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Copyright

Copyright © 2025 Panoramic Data Limited

## Related Links

- [UK Parliament Petitions Website](https://petition.parliament.uk/)
- [UK Parliament Petitions API](https://petition.parliament.uk/petitions.json)

## Support

For issues, questions, or contributions, please visit the [GitHub repository](https://github.com/panoramicdata/Uk.Parliament).
