# Refit Migration Plan

## Overview

This document outlines the plan to **completely refactor** the UK Parliament API client from manual `HttpClient` implementation to use [Refit](https://github.com/reactiveui/refit), a type-safe REST library for .NET.

**⚠️ BREAKING CHANGE: This is a major version bump (v2.0) with NO backward compatibility.**

**🎯 SCOPE EXPANSION: This migration will also add support for ALL publicly available UK Parliament APIs.**

## Current Implementation Analysis

### Current Architecture
```
PetitionsClient (class)
├── HttpClient (instance field)
├── GetPayloadAsync<T> (private static)
├── GetSingleAsync<T> (private)
├── GetManyAsync<T> (private)
├── GetPetitionsAsync (public)
├── GetAllPetitionsAsync (public)
└── GetPetitionAsync (public)
```

### Current Features to REMOVE
- ❌ Manual HTTP calls with `HttpClient` → **Refit interface**
- ❌ Custom JSON deserialization helpers → **Refit with System.Text.Json**
- ❌ Result<T> pattern → **Direct exceptions (ApiException)**
- ❌ Query wrapper class → **Direct query parameters**
- ❌ PetitionsClient wrapper class → **IPetitionsApi interface only**
- ❌ Resource.Endpoint helper → **Refit attributes**
- ❌ Query.ToString() helper → **Refit query serialization**

### Issues with Current Approach
1. **Boilerplate Code**: Repetitive HTTP request/response handling
2. **Manual Serialization**: Custom JSON options management
3. **Result<T> Wrapper**: Hides exceptions, makes debugging harder
4. **Testing**: Difficult to mock/test HTTP interactions
5. **Maintenance**: Changes require updates to multiple methods
6. **Abstraction Overhead**: Query and PetitionsClient wrappers add unnecessary complexity

## Benefits of Refit

### Advantages
1. **Type-Safe API Definitions**: Compile-time verification of endpoints
2. **Less Boilerplate**: Automatic request/response handling
3. **Built-in Serialization**: Native System.Text.Json support
4. **Easy Mocking**: Interface-based design perfect for testing
5. **Modern Features**: Support for async/await, cancellation tokens, etc.
6. **Community Support**: Well-maintained, widely-used library
7. **Standard Error Handling**: Uses exceptions like standard HTTP libraries
8. **Direct Parameter Mapping**: No need for query wrapper classes
9. **Cleaner API Surface**: Single interface, no wrapper classes needed

## UK Parliament APIs Coverage

### Currently Implemented APIs

#### 1. ✅ Petitions API
**Base URL:** `https://petition.parliament.uk/`
- GET /petitions.json
- GET /petitions/{id}.json
- GET /archived/petitions.json
- GET /archived/petitions/{id}.json

**Status:** Fully implemented (to be migrated to Refit)

---

### APIs to Add in v2.0

#### 2. 🆕 Members API
**Base URL:** `https://members-api.parliament.uk/`
**Documentation:** https://members-api.parliament.uk/index.html

Provides comprehensive information about Members of Parliament (MPs and Lords).

**Key Endpoints:**
- GET /api/Members/Search - Search for members
- GET /api/Members/{id} - Get specific member details
- GET /api/Members/{id}/Biography - Get member biography
- GET /api/Members/{id}/Contact - Get member contact details
- GET /api/Members/{id}/Focus - Get member focus areas
- GET /api/Members/{id}/Voting - Get member voting history
- GET /api/Location/Constituency/Search - Search constituencies
- GET /api/Location/Constituency/{id} - Get constituency details
- GET /api/Location/Constituency/{id}/Representations - Get constituency representations
- GET /api/Location/Constituency/{id}/Synopsis - Get constituency synopsis
- GET /api/Parties/GetActive/{house} - Get active parties
- GET /api/Parties/StateOfTheParties/{house}/{forDate} - Get party composition
- GET /api/Parties/LordsByType/{forDate} - Get Lords by type

**Priority:** HIGH - Core parliamentary data

---

#### 3. 🆕 Bills API
**Base URL:** `https://bills-api.parliament.uk/`
**Documentation:** https://bills-api.parliament.uk/index.html

Provides information about Parliamentary Bills and legislation.

**Key Endpoints:**
- GET /api/v1/Bills - List all bills
- GET /api/v1/Bills/{billId} - Get specific bill
- GET /api/v1/Bills/{billId}/Stages - Get bill stages
- GET /api/v1/Bills/{billId}/Publications - Get bill publications
- GET /api/v1/Bills/Search - Search bills
- GET /api/v1/Sessions - Get parliamentary sessions
- GET /api/v1/BillTypes - Get bill types
- GET /api/v1/Publications/Search - Search publications
- GET /api/v1/SittingDays - Get sitting days

**Priority:** HIGH - Legislation tracking

---

#### 4. 🆕 Committees API
**Base URL:** `https://committees-api.parliament.uk/`
**Documentation:** https://committees-api.parliament.uk/index.html

Provides information about Parliamentary Committees and their work.

**Key Endpoints:**
- GET /api/Committees - List all committees
- GET /api/Committee/{id} - Get specific committee
- GET /api/Committee/{id}/Members - Get committee members
- GET /api/Committee/{id}/Inquiries - Get committee inquiries
- GET /api/Inquiry/{id} - Get specific inquiry
- GET /api/Inquiry/{id}/Contributions - Get inquiry contributions
- GET /api/Inquiry/{id}/Submissions - Get inquiry submissions
- GET /api/Inquiry/{id}/Publications - Get inquiry publications

**Priority:** MEDIUM - Committee oversight and inquiries

---

#### 5. 📋 Lords Divisions API
**Base URL:** `https://lordsvotes-api.parliament.uk/`

Provides information about voting in the House of Lords.

**Key Endpoints:**
- GET /data/Divisions - List divisions
- GET /data/Divisions/{divisionId} - Get specific division
- GET /data/Divisions/groupedbyparty/{divisionId} - Get division votes grouped by party
- GET /data/Divisions/search - Search divisions

**Priority:** MEDIUM - Voting records

---

#### 6. 📋 Commons Divisions API
**Base URL:** `https://commonsvotes-api.parliament.uk/`

Provides information about voting in the House of Commons.

**Key Endpoints:**
- GET /data/divisions.json - List divisions
- GET /data/division/{divisionId}.json - Get specific division
- GET /data/divisions.json/groupedbyparty/{divisionId} - Get division votes grouped by party
- GET /data/divisions.json/search - Search divisions
- GET /data/divisions.json/membervoting - Get member voting records

**Priority:** MEDIUM - Voting records

---

### Future Consideration APIs

#### 7. 🔮 Early Day Motions API
**Base URL:** TBD - May be part of another API or not yet public

**Priority:** LOW - Awaiting public API availability

#### 8. 🔮 Parliamentary Questions API
**Base URL:** TBD - May be part of another API

**Priority:** LOW - Awaiting public API availability

#### 9. 🔮 Hansard API
**Base URL:** `https://hansard.parliament.uk/`

Official record of parliamentary debates. Currently only has web interface.

**Priority:** LOW - No structured API currently documented

---

## Migration Plan (EXPANDED)

### Phase 1: Setup and Dependencies

#### 1.1 Add NuGet Packages
```xml
<PackageReference Include="Refit" Version="8.0.0" />
<PackageReference Include="Refit.HttpClientFactory" Version="8.0.0" />
```

#### 1.2 New Project Structure
```
Uk.Parliament/
├── Petitions/
│   ├── IPetitionsApi.cs (NEW - Refit interface)
│   ├── Models/
│   │   ├── Petition.cs
│   │   ├── PetitionAttributes.cs
│   │   └── ... (all petition models)
│   └── Extensions/
│       └── PetitionsApiExtensions.cs
├── Members/ (NEW)
│   ├── IMembersApi.cs (NEW - Refit interface)
│   ├── Models/
│   │   ├── Member.cs
│   │   ├── Constituency.cs
│   │   ├── Party.cs
│   │   └── ... (all member models)
│   └── Extensions/
│       └── MembersApiExtensions.cs
├── Bills/ (NEW)
│   ├── IBillsApi.cs (NEW - Refit interface)
│   ├── Models/
│   │   ├── Bill.cs
│   │   ├── BillStage.cs
│   │   ├── Publication.cs
│   │   └── ... (all bill models)
│   └── Extensions/
│       └── BillsApiExtensions.cs
├── Committees/ (NEW)
│   ├── ICommitteesApi.cs (NEW - Refit interface)
│   ├── Models/
│   │   ├── Committee.cs
│   │   ├── Inquiry.cs
│   │   ├── Submission.cs
│   │   └── ... (all committee models)
│   └── Extensions/
│       └── CommitteesApiExtensions.cs
├── Divisions/ (NEW)
│   ├── ILordsDivisionsApi.cs (NEW)
│   ├── ICommonsDivisionsApi.cs (NEW)
│   ├── Models/
│   │   ├── Division.cs
│   │   ├── Vote.cs
│   │   └── ... (all division models)
│   └── Extensions/
│       └── DivisionsApiExtensions.cs
├── Common/ (NEW - Shared types)
│   ├── Models/
│   │   ├── ApiResponse.cs
│   │   ├── PaginatedResponse.cs
│   │   ├── Links.cs
│   │   └── Meta.cs
└── Extensions/ (NEW - Root level)
    └── ServiceCollectionExtensions.cs (All DI registration)
```

#### 1.3 Files to DELETE Completely
- ❌ `PetitionsClient.cs` (replaced by `IPetitionsApi` interface)
- ❌ `Query.cs` (replaced by direct parameters)
- ❌ `Resource.cs` (replaced by Refit attributes)
- ❌ `Result.cs` (replaced by exceptions)
- ❌ All HTTP helper methods

### Phase 2: Define Refit Interfaces for ALL APIs

#### 2.1 IPetitionsApi Interface
```csharp
namespace Uk.Parliament.Petitions;

/// <summary>
/// UK Parliament Petitions API client
/// </summary>
[Headers("User-Agent: Uk.Parliament.NET/2.0")]
public interface IPetitionsApi
{
    /// <summary>
    /// Get petitions with optional filtering and pagination
    /// </summary>
    /// <param name="search">Optional search term to filter petitions</param>
    /// <param name="state">Optional state filter (e.g., "open", "closed", "rejected")</param>
    /// <param name="page">Optional page number for pagination (1-based)</param>
    /// <param name="pageSize">Optional number of results per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>API response containing list of petitions</returns>
    /// <exception cref="ApiException">Thrown when the API returns an error response</exception>
    [Get("/petitions.json")]
    Task<ApiResponse<PetitionsResponse>> GetPetitionsAsync(
        [Query] string? search = null,
        [Query] string? state = null,
        [Query] int? page = null,
        [Query(Name = "page_size")] int? pageSize = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a single petition by ID
    /// </summary>
    /// <param name="id">Petition ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>API response containing the petition</returns>
    /// <exception cref="ApiException">Thrown when the API returns an error response</exception>
    [Get("/petitions/{id}.json")]
    Task<ApiResponse<PetitionResponse>> GetPetitionAsync(
        int id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get archived petitions with optional filtering and pagination
    /// </summary>
    [Get("/archived/petitions.json")]
    Task<ApiResponse<PetitionsResponse>> GetArchivedPetitionsAsync(
        [Query] string? search = null,
        [Query] string? state = null,
        [Query] int? page = null,
        [Query(Name = "page_size")] int? pageSize = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a single archived petition by ID
    /// </summary>
    [Get("/archived/petitions/{id}.json")]
    Task<ApiResponse<PetitionResponse>> GetArchivedPetitionAsync(
        int id,
        CancellationToken cancellationToken = default);
}
```

#### 2.2 IMembersApi Interface (NEW)
```csharp
namespace Uk.Parliament.Members;

/// <summary>
/// UK Parliament Members API client
/// </summary>
[Headers("User-Agent: Uk.Parliament.NET/2.0")]
public interface IMembersApi
{
    /// <summary>
    /// Search for members of parliament
    /// </summary>
    [Get("/api/Members/Search")]
    Task<ApiResponse<PaginatedResponse<Member>>> SearchMembersAsync(
        [Query] string? name = null,
        [Query] int? skip = null,
        [Query] int? take = null,
        [Query] string? house = null,
        [Query] bool? isCurrentMember = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a specific member by ID
    /// </summary>
    [Get("/api/Members/{id}")]
    Task<ApiResponse<MemberResponse>> GetMemberAsync(
        int id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get member biography
    /// </summary>
    [Get("/api/Members/{id}/Biography")]
    Task<ApiResponse<BiographyResponse>> GetMemberBiographyAsync(
        int id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get member contact details
    /// </summary>
    [Get("/api/Members/{id}/Contact")]
    Task<ApiResponse<ContactResponse>> GetMemberContactAsync(
        int id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Search for constituencies
    /// </summary>
    [Get("/api/Location/Constituency/Search")]
    Task<ApiResponse<PaginatedResponse<Constituency>>> SearchConstituenciesAsync(
        [Query] string? searchText = null,
        [Query] int? skip = null,
        [Query] int? take = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get constituency details
    /// </summary>
    [Get("/api/Location/Constituency/{id}")]
    Task<ApiResponse<ConstituencyResponse>> GetConstituencyAsync(
        int id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get active parties for a house
    /// </summary>
    [Get("/api/Parties/GetActive/{house}")]
    Task<ApiResponse<PartyListResponse>> GetActivePartiesAsync(
        int house,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get state of the parties on a specific date
    /// </summary>
    [Get("/api/Parties/StateOfTheParties/{house}/{forDate}")]
    Task<ApiResponse<StateOfPartiesResponse>> GetStateOfPartiesAsync(
        int house,
        DateTime forDate,
        CancellationToken cancellationToken = default);
}
```

#### 2.3 IBillsApi Interface (NEW)
```csharp
namespace Uk.Parliament.Bills;

/// <summary>
/// UK Parliament Bills API client
/// </summary>
[Headers("User-Agent: Uk.Parliament.NET/2.0")]
public interface IBillsApi
{
    /// <summary>
    /// List all bills with optional filtering
    /// </summary>
    [Get("/api/v1/Bills")]
    Task<ApiResponse<PaginatedResponse<Bill>>> GetBillsAsync(
        [Query] int? searchTerm = null,
        [Query] int? session = null,
        [Query] int? currentHouse = null,
        [Query] string? originatingHouse = null,
        [Query] bool? includeBillsWithdrawn = null,
        [Query] bool? includeBillsDefeated = null,
        [Query] int? skip = null,
        [Query] int? take = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a specific bill by ID
    /// </summary>
    [Get("/api/v1/Bills/{billId}")]
    Task<ApiResponse<BillResponse>> GetBillAsync(
        int billId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get stages for a bill
    /// </summary>
    [Get("/api/v1/Bills/{billId}/Stages")]
    Task<ApiResponse<List<BillStage>>> GetBillStagesAsync(
        int billId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get publications for a bill
    /// </summary>
    [Get("/api/v1/Bills/{billId}/Publications")]
    Task<ApiResponse<List<Publication>>> GetBillPublicationsAsync(
        int billId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Search bills
    /// </summary>
    [Get("/api/v1/Bills/Search")]
    Task<ApiResponse<PaginatedResponse<Bill>>> SearchBillsAsync(
        [Query] string searchTerm,
        [Query] int? session = null,
        [Query] int? skip = null,
        [Query] int? take = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get parliamentary sessions
    /// </summary>
    [Get("/api/v1/Sessions")]
    Task<ApiResponse<List<Session>>> GetSessionsAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get bill types
    /// </summary>
    [Get("/api/v1/BillTypes")]
    Task<ApiResponse<List<BillType>>> GetBillTypesAsync(
        CancellationToken cancellationToken = default);
}
```

#### 2.4 ICommitteesApi Interface (NEW)
```csharp
namespace Uk.Parliament.Committees;

/// <summary>
/// UK Parliament Committees API client
/// </summary>
[Headers("User-Agent: Uk.Parliament.NET/2.0")]
public interface ICommitteesApi
{
    /// <summary>
    /// List all committees
    /// </summary>
    [Get("/api/Committees")]
    Task<ApiResponse<List<Committee>>> GetCommitteesAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a specific committee
    /// </summary>
    [Get("/api/Committee/{id}")]
    Task<ApiResponse<CommitteeResponse>> GetCommitteeAsync(
        int id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get committee members
    /// </summary>
    [Get("/api/Committee/{id}/Members")]
    Task<ApiResponse<List<CommitteeMember>>> GetCommitteeMembersAsync(
        int id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get committee inquiries
    /// </summary>
    [Get("/api/Committee/{id}/Inquiries")]
    Task<ApiResponse<List<Inquiry>>> GetCommitteeInquiriesAsync(
        int id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a specific inquiry
    /// </summary>
    [Get("/api/Inquiry/{id}")]
    Task<ApiResponse<InquiryResponse>> GetInquiryAsync(
        int id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get inquiry contributions
    /// </summary>
    [Get("/api/Inquiry/{id}/Contributions")]
    Task<ApiResponse<List<Contribution>>> GetInquiryContributionsAsync(
        int id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get inquiry submissions
    /// </summary>
    [Get("/api/Inquiry/{id}/Submissions")]
    Task<ApiResponse<PaginatedResponse<Submission>>> GetInquirySubmissionsAsync(
        int id,
        [Query] int? skip = null,
        [Query] int? take = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get inquiry publications
    /// </summary>
    [Get("/api/Inquiry/{id}/Publications")]
    Task<ApiResponse<List<InquiryPublication>>> GetInquiryPublicationsAsync(
        int id,
        CancellationToken cancellationToken = default);
}
```

#### 2.5 ICommonsDivisionsApi Interface (NEW)
```csharp
namespace Uk.Parliament.Divisions;

/// <summary>
/// UK Parliament Commons Divisions (Voting) API client
/// </summary>
[Headers("User-Agent: Uk.Parliament.NET/2.0")]
public interface ICommonsDivisionsApi
{
    /// <summary>
    /// List all Commons divisions
    /// </summary>
    [Get("/data/divisions.json")]
    Task<ApiResponse<List<Division>>> GetDivisionsAsync(
        [Query] string? queryParameters = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a specific division
    /// </summary>
    [Get("/data/division/{divisionId}.json")]
    Task<ApiResponse<DivisionResponse>> GetDivisionAsync(
        int divisionId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get division results grouped by party
    /// </summary>
    [Get("/data/divisions.json/groupedbyparty/{divisionId}")]
    Task<ApiResponse<GroupedVotesResponse>> GetDivisionGroupedByPartyAsync(
        int divisionId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Search divisions
    /// </summary>
    [Get("/data/divisions.json/search")]
    Task<ApiResponse<List<Division>>> SearchDivisionsAsync(
        [Query] string searchTerm,
        [Query] int? skip = null,
        [Query] int? take = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get member voting records
    /// </summary>
    [Get("/data/divisions.json/membervoting")]
    Task<ApiResponse<MemberVotingResponse>> GetMemberVotingAsync(
        [Query] int memberId,
        [Query] int? skip = null,
        [Query] int? take = null,
        CancellationToken cancellationToken = default);
}
```

#### 2.6 ILordsDivisionsApi Interface (NEW)
```csharp
namespace Uk.Parliament.Divisions;

/// <summary>
/// UK Parliament Lords Divisions (Voting) API client
/// </summary>
[Headers("User-Agent: Uk.Parliament.NET/2.0")]
public interface ILordsDivisionsApi
{
    /// <summary>
    /// List all Lords divisions
    /// </summary>
    [Get("/data/Divisions")]
    Task<ApiResponse<List<Division>>> GetDivisionsAsync(
        [Query] int? skip = null,
        [Query] int? take = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a specific Lords division
    /// </summary>
    [Get("/data/Divisions/{divisionId}")]
    Task<ApiResponse<DivisionResponse>> GetDivisionAsync(
        int divisionId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get division results grouped by party
    /// </summary>
    [Get("/data/Divisions/groupedbyparty/{divisionId}")]
    Task<ApiResponse<GroupedVotesResponse>> GetDivisionGroupedByPartyAsync(
        int divisionId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Search divisions
    /// </summary>
    [Get("/data/Divisions/search")]
    Task<ApiResponse<List<Division>>> SearchDivisionsAsync(
        [Query] string searchTerm,
        [Query] int? skip = null,
        [Query] int? take = null,
        CancellationToken cancellationToken = default);
}
```

### Phase 3: Unified Dependency Injection Support

#### 3.1 Service Collection Extensions (ALL APIs)
```csharp
namespace Uk.Parliament.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds ALL UK Parliament API clients to the service collection
    /// </summary>
    public static IServiceCollection AddUkParliamentApis(
        this IServiceCollection services,
        Action<RefitSettings>? configureSettings = null)
    {
        services.AddUkParliamentPetitionsApi(configureSettings);
        services.AddUkParliamentMembersApi(configureSettings);
        services.AddUkParliamentBillsApi(configureSettings);
        services.AddUkParliamentCommitteesApi(configureSettings);
        services.AddUkParliamentDivisionsApis(configureSettings);
        
        return services;
    }

    /// <summary>
    /// Adds the Petitions API client
    /// </summary>
    public static IServiceCollection AddUkParliamentPetitionsApi(
        this IServiceCollection services,
        Action<RefitSettings>? configureSettings = null)
    {
        var settings = GetDefaultRefitSettings();
        configureSettings?.Invoke(settings);

        services
            .AddRefitClient<IPetitionsApi>(settings)
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri("https://petition.parliament.uk/");
                ConfigureDefaultHeaders(client);
            });

        return services;
    }

    /// <summary>
    /// Adds the Members API client
    /// </summary>
    public static IServiceCollection AddUkParliamentMembersApi(
        this IServiceCollection services,
        Action<RefitSettings>? configureSettings = null)
    {
        var settings = GetDefaultRefitSettings();
        configureSettings?.Invoke(settings);

        services
            .AddRefitClient<IMembersApi>(settings)
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri("https://members-api.parliament.uk/");
                ConfigureDefaultHeaders(client);
            });

        return services;
    }

    /// <summary>
    /// Adds the Bills API client
    /// </summary>
    public static IServiceCollection AddUkParliamentBillsApi(
        this IServiceCollection services,
        Action<RefitSettings>? configureSettings = null)
    {
        var settings = GetDefaultRefitSettings();
        configureSettings?.Invoke(settings);

        services
            .AddRefitClient<IBillsApi>(settings)
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri("https://bills-api.parliament.uk/");
                ConfigureDefaultHeaders(client);
            });

        return services;
    }

    /// <summary>
    /// Adds the Committees API client
    /// </summary>
    public static IServiceCollection AddUkParliamentCommitteesApi(
        this IServiceCollection services,
        Action<RefitSettings>? configureSettings = null)
    {
        var settings = GetDefaultRefitSettings();
        configureSettings?.Invoke(settings);

        services
            .AddRefitClient<ICommitteesApi>(settings)
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri("https://committees-api.parliament.uk/");
                ConfigureDefaultHeaders(client);
            });

        return services;
    }

    /// <summary>
    /// Adds both Commons and Lords Divisions API clients
    /// </summary>
    public static IServiceCollection AddUkParliamentDivisionsApis(
        this IServiceCollection services,
        Action<RefitSettings>? configureSettings = null)
    {
        var settings = GetDefaultRefitSettings();
        configureSettings?.Invoke(settings);

        // Commons Divisions
        services
            .AddRefitClient<ICommonsDivisionsApi>(settings)
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri("https://commonsvotes-api.parliament.uk/");
                ConfigureDefaultHeaders(client);
            });

        // Lords Divisions
        services
            .AddRefitClient<ILordsDivisionsApi>(settings)
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri("https://lordsvotes-api.parliament.uk/");
                ConfigureDefaultHeaders(client);
            });

        return services;
    }

    /// <summary>
    /// Adds all APIs with Polly resilience policies
    /// </summary>
    public static IServiceCollection AddUkParliamentApisWithResilience(
        this IServiceCollection services,
        Action<RefitSettings>? configureSettings = null)
    {
        services.AddUkParliamentApis(configureSettings);

        // Apply policies to all registered Refit clients
        services.ConfigureAll<HttpClientFactoryOptions>(options =>
        {
            options.HttpMessageHandlerBuilderActions.Add(builder =>
            {
                builder.AdditionalHandlers.Add(new PolicyHttpMessageHandler(GetRetryPolicy()));
                builder.AddAdditionalHandlers.Add(new PolicyHttpMessageHandler(GetCircuitBreakerPolicy()));
            });
        });

        return services;
    }

    private static RefitSettings GetDefaultRefitSettings()
    {
        return new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
#if DEBUG
                    UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
#else
                    UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
#endif
                })
        };
    }

    private static void ConfigureDefaultHeaders(HttpClient client)
    {
        client.DefaultRequestHeaders.UserAgent.ParseAdd("Uk.Parliament.NET/2.0");
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}
```

#### 3.2 Usage Examples

**Register ALL APIs:**
```csharp
// Program.cs
builder.Services.AddUkParliamentApis();

// Or with resilience
builder.Services.AddUkParliamentApisWithResilience();
```

**Register SPECIFIC APIs:**
```csharp
// Only register what you need
builder.Services.AddUkParliamentPetitionsApi();
builder.Services.AddUkParliamentMembersApi();
```

**Usage in Services:**
```csharp
public class ParliamentDataService
{
    private readonly IPetitionsApi _petitionsApi;
    private readonly IMembersApi _membersApi;
    private readonly IBillsApi _billsApi;
    private readonly ICommitteesApi _committeesApi;
    private readonly ICommonsDivisionsApi _commonsDivisionsApi;

    public ParliamentDataService(
        IPetitionsApi petitionsApi,
        IMembersApi membersApi,
        IBillsApi billsApi,
        ICommitteesApi committeesApi,
        ICommonsDivisionsApi commonsDivisionsApi)
    {
        _petitionsApi = petitionsApi;
        _membersApi = membersApi;
        _billsApi = billsApi;
        _committeesApi = committeesApi;
        _commonsDivisionsApi = commonsDivisionsApi;
    }

    public async Task<CompleteParliamentaryOverview> GetOverviewAsync()
    {
        // Get data from multiple APIs
        var petitions = await _petitionsApi.GetPetitionsAsync(state: "open", pageSize: 10);
        var members = await _membersApi.SearchMembersAsync(isCurrentMember: true, take: 10);
        var bills = await _billsApi.GetBillsAsync(take: 10);
        var committees = await _committeesApi.GetCommitteesAsync();
        
        return new CompleteParliamentaryOverview
        {
            RecentPetitions = petitions.Content.Data,
            ActiveMembers = members.Content.Items,
            CurrentBills = bills.Content.Items,
            ActiveCommittees = committees.Content
        };
    }
}
```

### Phase 4: Extension Methods for Common Patterns

#### 4.1 Pagination Helper Extension
```csharp
namespace Uk.Parliament.Petitions.Extensions;

/// <summary>
/// Extension methods for IPetitionsApi
/// </summary>
public static class PetitionsApiExtensions
{
    /// <summary>
    /// Get all petitions by automatically paginating through results
    /// </summary>
    /// <param name="api">The petitions API</param>
    /// <param name="search">Optional search term</param>
    /// <param name="state">Optional state filter</param>
    /// <param name="pageSize">Items per page (default: 50)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async enumerable of all petitions</returns>
    public static async IAsyncEnumerable<Petition> GetAllPetitionsAsync(
        this IPetitionsApi api,
        string? search = null,
        string? state = null,
        int pageSize = 50,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        int page = 1;
        
        while (true)
        {
            var response = await api.GetPetitionsAsync(search, state, page, pageSize, cancellationToken);
            
            if (!response.IsSuccessStatusCode || 
                response.Content?.Data == null || 
                response.Content.Data.Count == 0)
            {
                yield break;
            }

            foreach (var petition in response.Content.Data)
            {
                yield return petition;
            }

            // Stop if this was the last page
            if (response.Content.Data.Count < pageSize || 
                response.Content.Links?.Next == null)
            {
                yield break;
            }

            page++;
        }
    }

    /// <summary>
    /// Get all petitions as a materialized list
    /// </summary>
    public static async Task<List<Petition>> GetAllPetitionsListAsync(
        this IPetitionsApi api,
        string? search = null,
        string? state = null,
        int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        var allPetitions = new List<Petition>();
        
        await foreach (var petition in api.GetAllPetitionsAsync(search, state, pageSize, cancellationToken))
        {
            allPetitions.Add(petition);
        }
        
        return allPetitions;
    }
}
```

**Usage:**
```csharp
// Streaming approach (memory efficient)
await foreach (var petition in _api.GetAllPetitionsAsync(state: "open"))
{
    Console.WriteLine($"{petition.Attributes.Action}: {petition.Attributes.SignatureCount}");
}

// Materialized list approach
var allPetitions = await _api.GetAllPetitionsListAsync(state: "open");

```

### Phase 5: Testing Strategy for ALL APIs

#### 5.1 Unit Tests per API
```csharp
// Petitions API Tests (existing - to be updated)
public class PetitionsApiTests { /* ... */ }

// NEW: Members API Tests
public class MembersApiTests
{
    private readonly Mock<IMembersApi> _mockApi;

    [Fact]
    public async Task SearchMembers_ReturnsResults()
    {
        var response = new ApiResponse<PaginatedResponse<Member>>(
            new HttpResponseMessage(HttpStatusCode.OK),
            new PaginatedResponse<Member>
            {
                Items = new List<Member> { /* test data */ },
                TotalResults = 650
            },
            new RefitSettings());

        _mockApi.Setup(x => x.SearchMembersAsync(It.IsAny<string>(), null, null, null, null, default))
                .ReturnsAsync(response);

        var result = await _mockApi.Object.SearchMembersAsync("Smith");
        result.Content.Items.Should().NotBeEmpty();
    }
}

// NEW: Bills API Tests
public class BillsApiTests { /* ... */ }

// NEW: Committees API Tests
public class CommitteesApiTests { /* ... */ }

// NEW: Divisions API Tests
public class DivisionsApiTests { /* ... */ }
