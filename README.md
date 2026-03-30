# Uk.Parliament

![UK Parliament .NET Library](Uk.Parliament/Icon.png)

**The most comprehensive .NET library for UK Parliament APIs**  
All 12 public Parliament APIs supported with core endpoint coverage

[![NuGet](https://img.shields.io/nuget/v/Uk.Parliament.svg)](https://www.nuget.org/packages/Uk.Parliament/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET 10](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/download)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/c1099bcad8314dca94fd6215de4a7862)](https://app.codacy.com/gh/panoramicdata/Uk.Parliament/dashboard)

---

This .NET library provides coverage of all 12 UK Parliament public APIs:

✅ **Petitions API** - Public petitions to Parliament (100% endpoints)  
✅ **Members API** - MPs, Lords, constituencies, parties (core endpoints)  
✅ **Bills API** - Parliamentary legislation (core endpoints)  
✅ **Committees API** - Committee inquiries and submissions (core endpoints)  
✅ **Commons Divisions API** - House of Commons voting records  
✅ **Lords Divisions API** - House of Lords voting records  
✅ **Member Interests API** - Register of Members' Financial Interests (core endpoints)  
✅ **Written Questions & Statements API** - Parliamentary questions and statements (100% endpoints)  
✅ **Oral Questions & Motions API** - Oral questions and motions  
✅ **Treaties API** - International treaties laid before Parliament (core endpoints)  
✅ **Erskine May API** - Parliamentary procedure reference (core endpoints)  
✅ **NOW (Annunciator) API** - Real-time chamber status (100% endpoints)  

**Status:** All 12 APIs functional. Some APIs have additional sub-endpoints not yet implemented (see coverage details below).

---

## ⚠️ Breaking Change in v10.1 — Request Model Migration

Starting with **v10.1**, all API methods that previously accepted individual parameters have been replaced by strongly-typed **request model** objects. The old overloads are marked `[Obsolete("…", true)]` and will produce **compile errors** if referenced.

### Why?

| Benefit | Details |
|---------|---------|
| **Extensibility** | New query parameters can be added to a request record without changing method signatures, preventing future breaking changes. |
| **Consistency** | Every API call follows the same pattern: one request object in, one response out. |
| **Discoverability** | IntelliSense on the request type shows every available filter in one place. |
| **Testability** | Request objects are simple records that can be constructed, inspected, and asserted against. |

### Migration at a glance

**Before (v10.0.x) — loose parameters:**

```csharp
// Interface methods
var bills = await client.Bills.GetBillsAsync(searchTerm: "Budget", take: 10);
var members = await client.Members.SearchAsync(name: "Smith", isCurrentMember: true);
var petitions = await client.Petitions.GetAsync(state: "open", pageSize: 20);
var treaties = await client.Treaties.GetTreatiesAsync(status: "In Force", take: 10);

// Extension / pagination methods
await foreach (var bill in client.Bills.GetAllBillsAsync(currentHouse: "Commons", pageSize: 10)) { }
var allMembers = await client.Members.GetAllListAsync(house: 1, isCurrentMember: true, pageSize: 20);
```

**After (v10.1) — request models:**

```csharp
// Preferred: use GetAllAsync for paginated endpoints (handles paging automatically)
await foreach (var bill in client.GetAllAsync(new GetBillsRequest { SearchTerm = "Budget" }, cancellationToken)) { }
await foreach (var member in client.GetAllAsync(new SearchMembersRequest { Name = "Smith", IsCurrentMember = true }, cancellationToken)) { }
await foreach (var petition in client.GetAllAsync(new GetPetitionsRequest { State = "open" }, cancellationToken)) { }
var allMembers = await client.GetAllListAsync(new SearchMembersRequest { House = 1, IsCurrentMember = true }, cancellationToken);

// Raw interface methods are still available when you need manual paging control
var page = await client.Bills.GetBillsAsync(new GetBillsRequest { Skip = 0, Take = 20 }, cancellationToken);
```

> **Tip:** Each obsolete method's compiler error message includes a ready-to-use code example showing the equivalent request model call.

All request types live in the `Uk.Parliament.Requests` namespace. Pagination request types also implement `IPaginatedRequest<TItem>`, enabling the generic `GetAllAsync<TItem>()` method. The raw Refit interface methods (e.g. `client.Bills.GetBillsAsync(...)`) should only be used when you need to implement paging yourself.

---

## Overview

**Version 10.1** - Complete Refit-based implementation with type-safe REST API access and request models.

### Features

- 🎯 **All 12 APIs** - Every public Parliament API supported
- 🚀 **Type-Safe** - Refit interfaces with full IntelliSense
- ⚡ **Async/Await** - Modern async patterns throughout
- 📄 **Pagination** - Generic `GetAllAsync` for easy data streaming
- 🧪 **Well-Tested** - 139 tests (44 unit + 95 integration)
- 📖 **Documented** - Complete XML documentation
- 🔧 **DI-Ready** - Works with dependency injection
- 🔍 **Logging** - Built-in HTTP request/response logging
- ✅ **Production Ready** - All 12 APIs functional

---

## Installation

Install via NuGet:

```bash
dotnet add package Uk.Parliament
```

Or via Package Manager Console:

```powershell
Install-Package Uk.Parliament
```

---

## Quick Start

### Basic Usage

```csharp
using Uk.Parliament;
using Uk.Parliament.Requests;

// Create unified client for all Parliament APIs
using var client = new ParliamentClient();
using var cts = new CancellationTokenSource();
var cancellationToken = cts.Token;

// Stream all open petitions (automatic pagination)
await foreach (var petition in client.GetAllAsync(new GetPetitionsRequest { State = "open" }, cancellationToken))
{
    Console.WriteLine($"{petition.Attributes.Action}: {petition.Attributes.SignatureCount:N0} signatures");
}

// Stream all current Commons members
await foreach (var member in client.GetAllAsync(new SearchMembersRequest { House = 1, IsCurrentMember = true }, cancellationToken))
{
    Console.WriteLine($"{member.NameFullTitle} - {member.LatestParty?.Name}");
}

// Get all bills as a materialized list
var allBills = await client.GetAllListAsync(new GetBillsRequest { CurrentHouse = "Commons" }, cancellationToken);
Console.WriteLine($"Total Commons bills: {allBills.Count}");

// Non-paginated / by-ID lookups go through the raw API
var petition = await client.Petitions.GetByIdAsync(700143, cancellationToken);
var commonsMessage = await client.Now.GetCurrentMessageAsync("commons", cancellationToken);
Console.WriteLine($"Commons: {commonsMessage.Title}");
```

### Pagination with `GetAllAsync` (recommended)

For any endpoint that returns paginated results, prefer `client.GetAllAsync(...)` or
`client.GetAllListAsync(...)`. They handle all paging automatically, require a
`CancellationToken`, and work with every request type that implements
`IPaginatedRequest<TItem>`.

```csharp
// Memory-efficient streaming — processes one item at a time
await foreach (var bill in client.GetAllAsync(new GetBillsRequest { SearchTerm = "Budget" }, cancellationToken))
{
    Console.WriteLine(bill.ShortTitle);
}

// Or materialise into a list when you need random access
var allTreaties = await client.GetAllListAsync(new GetTreatiesRequest { Status = "In Force" }, cancellationToken);
```

### Manual Paging (advanced)

If you need to control paging yourself (e.g. to show page numbers in a UI),
you can call the raw Refit interface methods directly. These are exposed on
the typed API properties (`client.Bills`, `client.Members`, etc.):

```csharp
// Fetch a single page
var page = await client.Bills.GetBillsAsync(
    new GetBillsRequest { Skip = 0, Take = 20 }, cancellationToken);
Console.WriteLine($"Page has {page.Items.Count} of {page.TotalResults} bills");
```

### Dependency Injection

```csharp
// In Program.cs
services.AddSingleton<ParliamentClient>();

// Or with custom options
services.AddSingleton(sp => new ParliamentClient(new ParliamentClientOptions
{
    UserAgent = "MyApp/1.0",
    Timeout = TimeSpan.FromSeconds(30)
}));

// Inject into your services
public class MyService
{
    private readonly ParliamentClient _parliament;

    public MyService(ParliamentClient parliament)
    {
        _parliament = parliament;
    }
}
```

---

## 📚 Complete API Coverage

### ✅ Petitions API

Access public petitions to UK Parliament.

```csharp
// Stream all matching petitions (recommended)
await foreach (var petition in client.GetAllAsync(
    new GetPetitionsRequest { Search = "climate", State = "open" }, cancellationToken))
{
    Console.WriteLine($"{petition.Attributes.Action}: {petition.Attributes.SignatureCount:N0}");
}

// Get specific petition with full details
var petition = await client.Petitions.GetByIdAsync(700143, cancellationToken);
Console.WriteLine($"Signatures: {petition.Data.Attributes.SignatureCount:N0}");
Console.WriteLine($"State: {petition.Data.Attributes.State}");

// Signatures by country
foreach (var country in petition.Data.Attributes.SignaturesByCountry)
{
    Console.WriteLine($"{country.Name}: {country.SignatureCount:N0}");
}
```

**Status:** ✅ All tests passing

### ✅ Members API

Information about MPs, Lords, constituencies, and political parties.

```csharp
// Stream all matching members (recommended)
await foreach (var member in client.GetAllAsync(
    new SearchMembersRequest
    {
        Name = "Johnson",
        House = 1, // 1=Commons, 2=Lords
        IsCurrentMember = true
    }, cancellationToken))
{
    Console.WriteLine($"{member.NameFullTitle} - {member.LatestParty?.Name}");
}

// Stream all constituencies matching a search
await foreach (var constituency in client.GetAllAsync(
    new SearchConstituenciesRequest { SearchText = "London" }, cancellationToken))
{
    Console.WriteLine(constituency.Name);
}

// Get a specific member by ID
var member = await client.Members.GetByIdAsync(172, cancellationToken);
```

**Status:** ✅ All tests passing (core endpoints)

### ✅ Bills API

Parliamentary legislation information.

```csharp
// Stream all matching bills (recommended)
await foreach (var bill in client.GetAllAsync(
    new GetBillsRequest { SearchTerm = "Budget", CurrentHouse = "Commons" }, cancellationToken))
{
    Console.WriteLine($"{bill.ShortTitle} ({bill.BillTypeId})");
}

// Get a specific bill by ID
var bill = await client.Bills.GetBillByIdAsync(123, cancellationToken);

// Get bill types (non-paginated)
var billTypes = await client.Bills.GetBillTypesAsync(cancellationToken);
```

**Status:** ✅ All tests passing (core endpoints)

### ✅ Committees API

Parliamentary committee information.

```csharp
// Stream all committees (recommended)
await foreach (var committee in client.GetAllAsync(
    new GetCommitteesRequest(), cancellationToken))
{
    Console.WriteLine($"{committee.Name} ({committee.House})");
}

// Get a specific committee by ID
var committee = await client.Committees.GetCommitteeByIdAsync(1, cancellationToken);
```

**Status:** ✅ Core endpoints functional

### ✅ Divisions APIs

Voting records for Commons and Lords.

```csharp
// Commons divisions — search and get by ID
var commonsDivisions = await client.CommonsDivisions.SearchDivisionsAsync(
    new SearchCommonsDivisionsRequest { SearchTerm = "Budget" }, cancellationToken);
foreach (var div in commonsDivisions)
{
    Console.WriteLine($"{div.Title}: Ayes {div.AyeCount}, Noes {div.NoCount}");
}

var division = await client.CommonsDivisions.GetDivisionByIdAsync(12345, cancellationToken);

// Member voting history
var votingHistory = await client.CommonsDivisions.GetMemberVotingAsync(
    new GetCommonsMemberVotingRequest { MemberId = 172 }, cancellationToken);
foreach (var record in votingHistory)
{
    Console.WriteLine($"{record.PublishedDivision?.Title}: Aye={record.MemberVotedAye}");
}

// Lords divisions
var lordsDivisions = await client.LordsDivisions.SearchDivisionsAsync(
    new SearchLordsDivisionsRequest { SearchTerm = "Education" }, cancellationToken);
foreach (var div in lordsDivisions)
{
    Console.WriteLine($"{div.Title}: Content {div.AuthoritativeContentCount}, Not-Content {div.AuthoritativeNotContentCount}");
}

var lordsDivision = await client.LordsDivisions.GetDivisionByIdAsync(6789, cancellationToken);
```

**Status:** ✅ Fully typed

### ✅ Member Interests API

Register of Members' Financial Interests.

```csharp
// Stream all matching interests (recommended)
await foreach (var interest in client.GetAllAsync(
    new SearchInterestsRequest { SearchTerm = "employment" }, cancellationToken))
{
    Console.WriteLine($"{interest.MemberId}: {interest.Category.Name}");
}

// Get interest categories (non-paginated)
var categories = await client.Interests.GetCategoriesAsync(cancellationToken);
foreach (var category in categories.Items)
{
    Console.WriteLine($"Category: {category.Name}");
}

// Get a specific interest by ID
var interest = await client.Interests.GetInterestByIdAsync(123, cancellationToken);

// Get all registers (non-paginated)
var registers = await client.Interests.GetRegistersAsync(cancellationToken);
```

**Status:** ✅ All tests passing

### ✅ Written Questions & Statements API

Parliamentary questions and ministerial statements.

```csharp
// Stream all answered written questions from the last month (recommended)
await foreach (var question in client.GetAllAsync(
    new GetWrittenQuestionsRequest
    {
        TabledWhenFrom = DateTime.Now.AddMonths(-1),
        IsAnswered = true
    }, cancellationToken))
{
    Console.WriteLine(question.Value.QuestionText);
}

// Stream all written statements from the last week
await foreach (var statement in client.GetAllAsync(
    new GetWrittenStatementsRequest
    {
        MadeWhenFrom = DateTime.Now.AddDays(-7)
    }, cancellationToken))
{
    Console.WriteLine(statement.Value.Title);
}

// Stream all daily reports from the last week
await foreach (var report in client.GetAllAsync(
    new GetDailyReportsRequest
    {
        DateFrom = DateTime.Now.AddDays(-7)
    }, cancellationToken))
{
    Console.WriteLine(report.Value.Title);
}
```

**Status:** ✅ All tests passing

### ✅ Oral Questions & Motions API

Oral questions and parliamentary motions.

```csharp
// Stream all oral questions from the last month (recommended)
await foreach (var question in client.GetAllAsync(
    new GetOralQuestionsRequest
    {
        House = "Commons",
        DateFrom = DateTime.Now.AddMonths(-1)
    }, cancellationToken))
{
    Console.WriteLine(question.QuestionText);
}

// Stream all active Early Day Motions
await foreach (var motion in client.GetAllAsync(
    new GetMotionsRequest
    {
        MotionType = "Early Day Motion",
        IsActive = true
    }, cancellationToken))
{
    Console.WriteLine(motion.Title);
}
```

**Status:** ✅ All tests passing

### ✅ Treaties API

International treaties laid before Parliament.

```csharp
// Stream all treaties in force (recommended)
await foreach (var treaty in client.GetAllAsync(
    new GetTreatiesRequest
    {
        Status = "In Force",
        DateLaidFrom = DateTime.Now.AddYears(-1)
    }, cancellationToken))
{
    Console.WriteLine($"{treaty.CommandPaperNumber}: {treaty.Title}");
}

// Get a specific treaty by ID
var treaty = await client.Treaties.GetTreatyByIdAsync("123", cancellationToken);

// Get treaty business items (non-paginated)
var businessItems = await client.Treaties.GetTreatyBusinessItemsAsync(123, cancellationToken);

// Get government organizations (non-paginated)
var orgs = await client.Treaties.GetGovernmentOrganisationsAsync(cancellationToken);
```

**Status:** ✅ All tests passing

### ✅ Erskine May API

The authoritative guide to parliamentary procedure.

```csharp
// Search parliamentary procedure by section
var results = await client.ErskineMay.SearchAsync("voting procedure", cancellationToken);

// Search by paragraph
var paragraphs = await client.ErskineMay.SearchParagraphsAsync("division bells", cancellationToken);

// Get all parts
var parts = await client.ErskineMay.GetPartsAsync(cancellationToken);

// Get a specific part
var part = await client.ErskineMay.GetPartAsync(partNumber: 1, cancellationToken);

// Get a specific chapter
var chapter = await client.ErskineMay.GetChapterAsync(chapterNumber: 1, cancellationToken);

// Get a section by ID
var section = await client.ErskineMay.GetSectionByIdAsync(sectionId: 100, cancellationToken);

// Browse index terms
var indexTerms = await client.ErskineMay.BrowseIndexTermsAsync(cancellationToken);
```

**Status:** ✅ All tests passing

### ✅ NOW (Annunciator) API

Real-time chamber status and business information.

```csharp
// Get current Commons annunciator message
var commonsMessage = await client.Now.GetCurrentMessageAsync("commons", cancellationToken);
Console.WriteLine($"Title: {commonsMessage.Title}");

// Get current Lords annunciator message
var lordsMessage = await client.Now.GetCurrentMessageAsync("lords", cancellationToken);

// Get message for a specific date
var historicMessage = await client.Now.GetMessageByDateAsync("commons", "2024-01-15", cancellationToken);
```

**Status:** ✅ All tests passing

---

## Configuration

```csharp
var options = new ParliamentClientOptions
{
    UserAgent = "MyApp/1.0",
    Timeout = TimeSpan.FromSeconds(60),
    EnableDebugValidation = false, // Strict JSON validation
    EnableVerboseLogging = true,   // Full HTTP logs
    Logger = loggerFactory.CreateLogger("ParliamentClient")
};

var client = new ParliamentClient(options);
```

---

## Test Coverage

| API | Unit Tests | Integration Tests | Total | Status |
|-----|------------|-------------------|-------|--------|
| Petitions | 3 | 10 | 13 | ✅ |
| Members | 3 | 11 | 14 | ✅ |
| Bills | 2 | 9 | 11 | ✅ |
| Committees | 2 | 10 | 12 | ✅ |
| Commons Divisions | 2 | 5 | 7 | ✅ |
| Lords Divisions | 2 | 5 | 7 | ✅ |
| Interests | 3 | 8 | 11 | ✅ |
| Questions/Statements | 4 | 10 | 14 | ✅ |
| Oral Questions | 4 | 6 | 10 | ✅ |
| Treaties | 4 | 6 | 10 | ✅ |
| Erskine May | 3 | 6 | 9 | ✅ |
| NOW | 3 | 3 | 6 | ✅ |
| Client/Misc | 2 | 10 | 12 | ✅ |
| **TOTAL** | **40** | **99** | **139** | **44 unit passing** |

---

## API Endpoint Coverage

The library covers all core/primary endpoints for each API. Some APIs have additional sub-endpoints not yet implemented:

| API | Implemented | Available | Coverage | Notes |
|-----|------------|-----------|----------|-------|
| Petitions | 4 | ~4 | ✅ 100% | Full coverage |
| NOW (Annunciator) | 2 | 2 | ✅ 100% | Full coverage |
| Written Q&A | 7 | 7 | ✅ 100% | Full coverage |
| Interests | 8 | 8 | ✅ 100% | Including CSV export and document download |
| Treaties | 6 | 6 | ✅ 100% | Full coverage |
| Erskine May | 11 | 11 | ✅ 100% | Full coverage |
| Divisions (Commons) | 5 | ~5 | ✅ ~100% | Core voting endpoints |
| Divisions (Lords) | 4 | ~4 | ✅ ~100% | Core voting endpoints |
| Oral Questions | 3 | ~3+ | ✅ Core | No Swagger spec available |
| Members | 4 | 40 | ⚠️ Core | Missing: biography, voting, portraits, parties, posts, etc. |
| Bills | 3 | 21 | ⚠️ Core | Missing: stages, amendments, publications, RSS, sittings |
| Committees | 2 | 37 | ⚠️ Core | Missing: events, evidence, publications, members |

---

## Documentation

- [MASTER_PLAN.md](MASTER_PLAN.md) - Complete implementation roadmap
- [API_ISSUES.md](API_ISSUES.md) - Known Parliament API issues

---

## Known Issues

- **Committees API**: Occasional intermittent 500 errors from Parliament servers
- **Members API**: Many sub-endpoints (biography, voting history, portraits) not yet implemented
- **Bills API**: Stage/amendment/publication sub-endpoints not yet implemented
- **Committees API**: Evidence, events, and publication sub-endpoints not yet implemented

---

## Development

```bash
git clone https://github.com/panoramicdata/Uk.Parliament.git
cd Uk.Parliament
dotnet build
dotnet test
```

---

## Migration from v1.x

**v1.x:**
```csharp
var client = new PetitionsClient();
var result = await client.GetPetitionAsync(123, CancellationToken.None);
if (result.Ok) { /* use result.Data */ }
```

**v2.0+:**
```csharp
var client = new ParliamentClient();
var petition = await client.Petitions.GetByIdAsync(123, cancellationToken);
// Direct access, throws ApiException on error
```

## Migration from v10.0.x to v10.1

All query-parameter overloads have been replaced by request model overloads, and
pagination is now handled by `client.GetAllAsync(...)` / `client.GetAllListAsync(...)`.
See [Breaking Change in v10.1](#️-breaking-change-in-v101--request-model-migration) at the top of this page for a full before/after guide.

---

## Contributing

Contributions welcome! Priority areas:
1. Expand Members API (biography, voting history, portraits, etc.)
2. Expand Bills API (stages, amendments, publications)
3. Add more integration tests
4. Improve documentation
5. Report issues

---

## License

MIT License - see [LICENSE](LICENSE) file

---

## Copyright

Copyright © 2025 Panoramic Data Limited

---

## Links

- [UK Parliament Petitions](https://petition.parliament.uk/)
- [UK Parliament APIs](https://developer.parliament.uk/)
- [NuGet Package](https://www.nuget.org/packages/Uk.Parliament/)
- [GitHub Repository](https://github.com/panoramicdata/Uk.Parliament)
- [Report Issues](https://github.com/panoramicdata/Uk.Parliament/issues)

---

## Acknowledgments

Thanks to the UK Parliament Digital Service for providing these public APIs.

For API issues, contact: softwareengineering@parliament.uk
