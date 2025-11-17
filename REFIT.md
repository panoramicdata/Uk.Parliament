# Refit Migration Plan

## Overview

This document outlines the plan to refactor the UK Parliament API client from manual `HttpClient` implementation to use [Refit](https://github.com/reactiveui/refit), a type-safe REST library for .NET.

## Current Implementation Analysis

### Current Architecture
```
PetitionsClient (class)
??? HttpClient (instance field)
??? GetPayloadAsync<T> (private static)
??? GetSingleAsync<T> (private)
??? GetManyAsync<T> (private)
??? GetPetitionsAsync (public)
??? GetAllPetitionsAsync (public)
??? GetPetitionAsync (public)
```

### Current Features
- ? Manual HTTP calls with `HttpClient`
- ? Custom JSON deserialization with debug validation
- ? Result<T> pattern for error handling
- ? Automatic pagination in `GetAllPetitionsAsync`
- ? Query string building in `Query.ToString()`
- ? Endpoint building in `Resource.Endpoint`

### Issues with Current Approach
1. **Boilerplate Code**: Repetitive HTTP request/response handling
2. **Manual Serialization**: Custom JSON options management
3. **Error Handling**: Manual exception wrapping in Result<T>
4. **Testing**: Difficult to mock/test HTTP interactions
5. **Maintenance**: Changes require updates to multiple methods

## Benefits of Refit

### Advantages
1. **Type-Safe API Definitions**: Compile-time verification of endpoints
2. **Less Boilerplate**: Automatic request/response handling
3. **Built-in Serialization**: Native System.Text.Json support
4. **Easy Mocking**: Interface-based design perfect for testing
5. **Modern Features**: Support for async/await, cancellation tokens, etc.
6. **Community Support**: Well-maintained, widely-used library

### Compatibility with Current Features
- ? Custom JSON options (via `RefitSettings`)
- ? Debug validation (same `JsonSerializerOptions`)
- ? Result<T> pattern (can be wrapped)
- ? Query parameters (via attributes)
- ?? Automatic pagination (requires custom implementation)

## Migration Plan

### Phase 1: Setup and Dependencies

#### 1.1 Add NuGet Packages
```xml
<PackageReference Include="Refit" Version="7.2.1" />
<PackageReference Include="Refit.HttpClientFactory" Version="7.2.1" />
```

#### 1.2 Update Project Structure
```
Uk.Parliament/
??? Petitions/
?   ??? IPetitionsApi.cs (NEW - Refit interface)
?   ??? PetitionsClient.cs (REFACTOR - wrapper around Refit)
?   ??? Models/ (NEW - organize models)
?   ?   ??? Petition.cs
?   ?   ??? PetitionAttributes.cs
?   ?   ??? Query.cs
?   ?   ??? ... (all model classes)
?   ??? Infrastructure/ (NEW - configuration)
?       ??? RefitConfiguration.cs
?       ??? ResultHandler.cs
```

### Phase 2: Define Refit Interface

#### 2.1 Create IPetitionsApi Interface
```csharp
public interface IPetitionsApi
{
    [Get("/petitions.json")]
    Task<ApiResponse<List<Petition>>> GetPetitionsAsync(
        [Query] string search = null,
        [Query] string state = null,
        [Query] int? page = null,
        [Query(Name = "_pageSize")] int? pageSize = null,
        CancellationToken cancellationToken = default);

    [Get("/petitions/{id}.json")]
    Task<ApiResponse<Petition>> GetPetitionAsync(
        int id,
        CancellationToken cancellationToken = default);

    [Get("/archived/petitions.json")]
    Task<ApiResponse<List<Petition>>> GetArchivedPetitionsAsync(
        [Query] string search = null,
        [Query] string state = null,
        [Query] int? page = null,
        [Query(Name = "_pageSize")] int? pageSize = null,
        CancellationToken cancellationToken = default);

    [Get("/archived/petitions/{id}.json")]
    Task<ApiResponse<Petition>> GetArchivedPetitionAsync(
        int id,
        CancellationToken cancellationToken = default);
}
```

#### 2.2 Update Query Class
**Option A: Keep Query class for high-level API**
```csharp
// Query class remains for PetitionsClient wrapper
// Internally converted to individual parameters for Refit
```

**Option B: Use Query class directly with Refit**
```csharp
// Use [Query(CollectionFormat = CollectionFormat.Multi)]
// Requires Query class to be serializable
```

**Recommendation**: Option A - Keep Query class for backward compatibility

### Phase 3: Refactor PetitionsClient

#### 3.1 New PetitionsClient Implementation
```csharp
public class PetitionsClient : IDisposable
{
    private readonly IPetitionsApi _api;
    private readonly HttpClient _httpClient;

    // Constructor - Option 1: Create own HttpClient
    public PetitionsClient()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri("https://petition.parliament.uk/") };
        _api = RestService.For<IPetitionsApi>(_httpClient, GetRefitSettings());
    }

    // Constructor - Option 2: Accept HttpClient (recommended for DI)
    public PetitionsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _api = RestService.For<IPetitionsApi>(httpClient, GetRefitSettings());
    }

    // Constructor - Option 3: Accept Refit interface directly (best for testing)
    public PetitionsClient(IPetitionsApi api)
    {
        _api = api ?? throw new ArgumentNullException(nameof(api));
    }

    private static RefitSettings GetRefitSettings()
    {
        return new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
#if DEBUG
                    UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
#else
                    UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
#endif
                })
        };
    }

    public async Task<Result<List<Petition>>> GetPetitionsAsync(
        Query query,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _api.GetPetitionsAsync(
                search: query.Text,
                state: query.State?.ToString().ToLowerInvariant(),
                page: query.PageNumber,
                pageSize: query.PageSize,
                cancellationToken: cancellationToken);

            return new Result<List<Petition>>(response.Data);
        }
        catch (ApiException ex)
        {
            return new Result<List<Petition>>(new Exception($"API Error: {ex.StatusCode}", ex));
        }
        catch (Exception ex)
        {
            return new Result<List<Petition>>(ex);
        }
    }

    // ... other methods follow same pattern
}
```

#### 3.2 Automatic Pagination Support
```csharp
public async Task<Result<List<Petition>>> GetAllPetitionsAsync(
    Query query,
    CancellationToken cancellationToken)
{
    var allPetitions = new List<Petition>();
    var pageNumber = 1;

    try
    {
        while (true)
        {
            var pageQuery = new Query
            {
                Text = query.Text,
                State = query.State,
                PageSize = query.PageSize ?? 50,
                PageNumber = pageNumber
            };

            var pageResult = await GetPetitionsAsync(pageQuery, cancellationToken);

            if (!pageResult.Ok || pageResult.Data == null || pageResult.Data.Count == 0)
            {
                break;
            }

            allPetitions.AddRange(pageResult.Data);

            if (pageResult.Data.Count < (query.PageSize ?? 50))
            {
                break;
            }

            pageNumber++;
        }

        return new Result<List<Petition>>(allPetitions);
    }
    catch (Exception ex)
    {
        return new Result<List<Petition>>(ex);
    }
}
```

### Phase 4: Dependency Injection Support

#### 4.1 Add IServiceCollection Extensions
```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUkParliamentPetitionsClient(
        this IServiceCollection services,
        Action<RefitSettings>? configureSettings = null)
    {
        var settings = new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
#if DEBUG
                    UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
#else
                    UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
#endif
                })
        };

        configureSettings?.Invoke(settings);

        services.AddHttpClient<IPetitionsApi>("UkParliament", client =>
        {
            client.BaseAddress = new Uri("https://petition.parliament.uk/");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        })
        .AddTypedClient(c => RestService.For<IPetitionsApi>(c, settings));

        services.AddScoped<PetitionsClient>();

        return services;
    }
}
```

#### 4.2 Usage in ASP.NET Core
```csharp
// Startup.cs or Program.cs
services.AddUkParliamentPetitionsClient();

// Controller
public class PetitionsController : ControllerBase
{
    private readonly PetitionsClient _client;

    public PetitionsController(PetitionsClient client)
    {
        _client = client;
    }
}
```

### Phase 5: Update Tests

#### 5.1 Mock Refit Interface
```csharp
public class PetitionsTests
{
    private readonly Mock<IPetitionsApi> _mockApi;
    private readonly PetitionsClient _client;

    public PetitionsTests()
    {
        _mockApi = new Mock<IPetitionsApi>();
        _client = new PetitionsClient(_mockApi.Object);
    }

    [Fact]
    public async Task GetPetitionAsync_ReturnsSuccess()
    {
        // Arrange
        var expectedPetition = new Petition { Id = 123, Type = "petition" };
        var apiResponse = new ApiResponse<Petition>
        {
            Data = expectedPetition
        };

        _mockApi
            .Setup(x => x.GetPetitionAsync(123, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _client.GetPetitionAsync(123, default);

        // Assert
        result.Ok.Should().BeTrue();
        result.Data.Id.Should().Be(123);
    }
}
```

#### 5.2 Integration Tests with WireMock
```csharp
public class PetitionsIntegrationTests : IDisposable
{
    private readonly WireMockServer _server;
    private readonly PetitionsClient _client;

    public PetitionsIntegrationTests()
    {
        _server = WireMockServer.Start();
        var httpClient = new HttpClient { BaseAddress = new Uri(_server.Urls[0]) };
        _client = new PetitionsClient(httpClient);
    }

    [Fact]
    public async Task GetPetitionAsync_WithMockedApi_Succeeds()
    {
        // Arrange
        _server
            .Given(Request.Create().WithPath("/petitions/123.json").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody(/* JSON response */));

        // Act & Assert
        var result = await _client.GetPetitionAsync(123, default);
        result.Ok.Should().BeTrue();
    }
}
```

### Phase 6: Backward Compatibility

#### 6.1 Maintain Public API
- ? Keep all existing public methods
- ? Keep `Result<T>` pattern
- ? Keep `Query` class
- ? Keep all model classes unchanged
- ? No breaking changes to consumers

#### 6.2 Deprecation Strategy (if needed)
```csharp
[Obsolete("Use constructor with HttpClient for better DI support")]
public PetitionsClient()
{
    // Old implementation
}
```

### Phase 7: Remove Old Code

#### 7.1 Files to Remove
- None initially (keep for reference)

#### 7.2 Code to Remove
```csharp
// From PetitionsClient.cs
private readonly HttpClient _httpClient; // Remove if using DI
private static async Task<T> GetPayloadAsync<T>(...) // Remove
private async Task<Result<T>> GetSingleAsync<T>(...) // Remove
private async Task<Result<List<T>>> GetManyAsync<T>(...) // Remove
```

#### 7.3 Code to Simplify
```csharp
// Query.ToString() - may no longer be needed
// Resource.Endpoint - may no longer be needed
```

## Implementation Checklist

### Prerequisites
- [ ] Review Refit documentation
- [ ] Set up development branch
- [ ] Backup current working implementation

### Phase 1: Setup
- [ ] Add Refit NuGet packages
- [ ] Add Refit.HttpClientFactory package
- [ ] Create new folder structure

### Phase 2: Refit Interface
- [ ] Create `IPetitionsApi` interface
- [ ] Define all endpoints with attributes
- [ ] Test interface compiles correctly

### Phase 3: Refactor Client
- [ ] Create `RefitConfiguration` class
- [ ] Update `PetitionsClient` to use Refit
- [ ] Implement error handling wrapper
- [ ] Implement automatic pagination
- [ ] Test manually with real API

### Phase 4: Dependency Injection
- [ ] Create service collection extensions
- [ ] Add configuration options
- [ ] Document DI usage in README

### Phase 5: Testing
- [ ] Update existing unit tests to use mocks
- [ ] Add new tests for Refit-specific features
- [ ] Create integration test project (optional)
- [ ] Ensure all 10+ tests pass

### Phase 6: Documentation
- [ ] Update README with new usage examples
- [ ] Update XML documentation
- [ ] Add migration guide for existing users
- [ ] Update CHANGELOG

### Phase 7: Cleanup
- [ ] Remove old HttpClient code
- [ ] Remove unused helper methods
- [ ] Run code analysis
- [ ] Update version number

### Phase 8: Release
- [ ] Create release notes
- [ ] Tag version in Git
- [ ] Publish to NuGet
- [ ] Announce changes

## Risk Assessment

### High Risk
- ? **Breaking Changes**: Minimize by keeping wrapper API identical
- ? **Serialization Issues**: Test thoroughly with debug validation

### Medium Risk
- ?? **Performance**: Benchmark before/after (Refit is generally faster)
- ?? **Testing Complexity**: May need to learn new mocking patterns

### Low Risk
- ? **Refit Stability**: Well-established library
- ? **Community Support**: Active maintenance and documentation

## Success Criteria

### Must Have
- ? All existing tests pass
- ? No breaking changes to public API
- ? Debug validation still works
- ? Performance equal or better
- ? Improved testability

### Should Have
- ? Reduced lines of code
- ? Better error messages
- ? DI support out of the box
- ? Mock-friendly architecture

### Nice to Have
- ? Retry policies via Polly
- ? Request/response logging
- ? API versioning support
- ? Rate limiting support

## Timeline Estimate

### Optimistic: 4-6 hours
- 1 hour: Setup and interface definition
- 1 hour: Client refactoring
- 1 hour: Testing updates
- 1 hour: Documentation and cleanup

### Realistic: 8-12 hours
- 2 hours: Setup, planning, and interface definition
- 3 hours: Client refactoring and error handling
- 2 hours: Testing updates and verification
- 2 hours: Documentation, cleanup, and release prep

### Conservative: 16-20 hours
- Includes troubleshooting, edge cases, and comprehensive testing
- Additional integration tests
- Performance benchmarking
- Comprehensive documentation

## Open Questions

1. **Query Parameter Serialization**: Should we use Query class with Refit or convert to individual parameters?
   - **Recommendation**: Convert internally for maximum Refit compatibility

2. **Error Handling**: Should we keep Result<T> or switch to throwing exceptions?
   - **Recommendation**: Keep Result<T> for backward compatibility

3. **Constructor Overloads**: How many constructor variations should we support?
   - **Recommendation**: Support all three for maximum flexibility

4. **Archived Petitions**: Should this be a separate interface or same interface?
   - **Recommendation**: Same interface, different methods

5. **Testing Strategy**: Mock-based or integration tests or both?
   - **Recommendation**: Both - mocks for unit tests, WireMock for integration

## References

- [Refit GitHub Repository](https://github.com/reactiveui/refit)
- [Refit Documentation](https://reactiveui.github.io/refit/)
- [System.Text.Json with Refit](https://github.com/reactiveui/refit#systemtextjson)
- [HttpClient Factory](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests)

## Appendix A: Current vs. Refit Comparison

### Before (Current)
```csharp
// ~200 lines of boilerplate code in PetitionsClient
private static async Task<T> GetPayloadAsync<T>(HttpResponseMessage response)
{
    if (!response.IsSuccessStatusCode) { /* error handling */ }
    var @object = await response.Content.ReadFromJsonAsync<ApiResponse<T>>(JsonOptions);
    return @object.Data;
}

private async Task<Result<List<T>>> GetManyAsync<T>(Query query, CancellationToken ct)
{
    try
    {
        var path = $"{new T().Endpoint}{query}";
        var response = await _httpClient.GetAsync(path, ct).ConfigureAwait(false);
        var data = await GetPayloadAsync<List<T>>(response);
        return new Result<List<T>>(data);
    }
    catch (Exception ex) { return new Result<List<T>>(ex); }
}
```

### After (Refit)
```csharp
// Interface definition (~30 lines total)
[Get("/petitions.json")]
Task<ApiResponse<List<Petition>>> GetPetitionsAsync(...);

// Client wrapper (~50 lines per method)
public async Task<Result<List<Petition>>> GetPetitionsAsync(Query q, CancellationToken ct)
{
    try
    {
        var response = await _api.GetPetitionsAsync(q.Text, q.State?.ToString(), q.PageNumber, q.PageSize, ct);
        return new Result<List<Petition>>(response.Data);
    }
    catch (ApiException ex) { return new Result<List<Petition>>(new Exception($"API Error: {ex.StatusCode}", ex)); }
    catch (Exception ex) { return new Result<List<Petition>>(ex); }
}
```

### Lines of Code Reduction
- **Current**: ~300 lines (PetitionsClient + helpers)
- **Refit**: ~150 lines (Interface + wrapper)
- **Savings**: ~50% reduction in boilerplate

## Appendix B: Migration Checklist for Users

For existing users of the library, the migration should be seamless:

### No Changes Required
```csharp
// This code will continue to work exactly the same
var client = new PetitionsClient();
var result = await client.GetPetitionAsync(123456, default);
```

### Optional: Use New DI Features
```csharp
// New in version 2.0: Dependency Injection support
services.AddUkParliamentPetitionsClient();

// In controller/service
public MyService(PetitionsClient client)
{
    _client = client;
}
```

### Optional: Better Testing
```csharp
// New in version 2.0: Easy mocking
var mockApi = new Mock<IPetitionsApi>();
var client = new PetitionsClient(mockApi.Object);
```
