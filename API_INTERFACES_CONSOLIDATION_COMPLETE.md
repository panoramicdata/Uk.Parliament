# API Interfaces Consolidated to Uk.Parliament.Interfaces ?

## Summary of Changes

### 1. **All API Interfaces Moved to `Uk.Parliament.Interfaces`**

All Refit API interfaces have been consolidated into a single namespace for better organization and consistency.

#### Files Moved:
- ? `Uk.Parliament/Petitions/IPetitionsApi.cs` ? namespace changed to `Uk.Parliament.Interfaces`
- ? `Uk.Parliament/Members/IMembersApi.cs` ? namespace changed to `Uk.Parliament.Interfaces`
- ? `Uk.Parliament/Bills/IBillsApi.cs` ? namespace changed to `Uk.Parliament.Interfaces`
- ? `Uk.Parliament/Committees/ICommitteesApi.cs` ? namespace changed to `Uk.Parliament.Interfaces`
- ? `Uk.Parliament/Divisions/IDivisionsApi.cs` ? namespace changed to `Uk.Parliament.Interfaces`

### 2. **ApiResponse Renamed to ParliamentApiResponse**

To avoid conflicts with Refit's `ApiResponse<T>` type, our custom API response wrapper has been renamed.

#### Changed Files:
- ? `Uk.Parliament/Petitions/ApiResponse.cs` - Class renamed to `ParliamentApiResponse<T>`
- ? `Uk.Parliament/Petitions/IPetitionsApi.cs` - Updated to use `ParliamentApiResponse<T>`

### 3. **Updated All References**

All files that reference the API interfaces or response types have been updated:

#### Core Files:
- ? `Uk.Parliament/ParliamentClient.cs` - Now uses `Uk.Parliament.Interfaces`
- ? `Uk.Parliament/Petitions/PetitionsApiExtensions.cs` - Now uses `Uk.Parliament.Interfaces`

#### Test Files:
- ? `Uk.Parliament.Test/Petitions.cs` - Added `using Uk.Parliament.Interfaces;`
- ? `Uk.Parliament.Test/ParliamentClientTests.cs` - Updated to use `ParliamentApiResponse<T>`

## File Organization

### Before:
```
Uk.Parliament/
??? Petitions/
?   ??? IPetitionsApi.cs (namespace: Uk.Parliament.Petitions)
??? Members/
?   ??? IMembersApi.cs (namespace: Uk.Parliament.Members)
??? Bills/
?   ??? IBillsApi.cs (namespace: Uk.Parliament.Bills)
??? Committees/
?   ??? ICommitteesApi.cs (namespace: Uk.Parliament.Committees)
??? Divisions/
    ??? IDivisionsApi.cs (namespace: Uk.Parliament.Divisions)
```

### After:
```
Uk.Parliament/
??? Interfaces/ (conceptual namespace)
?   ??? Resource.cs
?   ??? IPetitionsApi.cs ?
?   ??? IMembersApi.cs ?
?   ??? IBillsApi.cs ?
?   ??? ICommitteesApi.cs ?
?   ??? IDivisionsApi.cs (ICommonsDivisionsApi + ILordsDivisionsApi) ?
??? Petitions/
?   ??? ParliamentApiResponse.cs (renamed from ApiResponse.cs) ?
?   ??? Petition.cs
?   ??? PetitionAttributes.cs
?   ??? PetitionsApiExtensions.cs ?
??? Members/
??? Bills/
??? Committees/
??? Divisions/
```

## Benefits

### 1. **Consistency**
All API interfaces now live in the same namespace: `Uk.Parliament.Interfaces`

### 2. **No Ambiguous References**
Renamed `ApiResponse` to `ParliamentApiResponse` to avoid conflicts with `Refit.ApiResponse`

### 3. **Cleaner Imports**
Instead of:
```csharp
using Uk.Parliament.Petitions;
using Uk.Parliament.Members;
using Uk.Parliament.Bills;
using Uk.Parliament.Committees;
using Uk.Parliament.Divisions;
```

Now just:
```csharp
using Uk.Parliament.Interfaces;
```

### 4. **Better Organization**
- Interfaces are grouped logically
- Models stay in their domain-specific namespaces
- Extension methods reference the interfaces namespace

## Updated Usage

### Before:
```csharp
using Uk.Parliament.Petitions;

public class MyService
{
    private readonly IPetitionsApi _api;
    
    public MyService(IPetitionsApi api)
    {
        _api = api;
    }
}
```

### After:
```csharp
using Uk.Parliament.Interfaces;
using Uk.Parliament.Petitions; // For models like Petition, PetitionAttributes

public class MyService
{
    private readonly IPetitionsApi _api;
    
    public MyService(IPetitionsApi api)
    {
        _api = api;
    }
}
```

## Type Rename: ApiResponse ? ParliamentApiResponse

### Before:
```csharp
Task<ApiResponse<List<Petition>>> GetAsync(...);
```

### After:
```csharp
Task<ParliamentApiResponse<List<Petition>>> GetAsync(...);
```

### In Unit Tests:
```csharp
mockApi.Setup(x => x.GetAsync(...))
    .ReturnsAsync(new ParliamentApiResponse<List<Petition>>
    {
        Data = expectedPetitions,
        Links = new Links()
    });
```

## Compatibility

### Breaking Changes:
- ? Namespace change for all API interfaces
- ? Type rename: `ApiResponse<T>` ? `ParliamentApiResponse<T>`

### Migration Required:
- Update all `using` statements to include `Uk.Parliament.Interfaces`
- Replace any references to `ApiResponse<T>` with `ParliamentApiResponse<T>`

## Test Status

? All unit tests updated and passing
? All integration tests updated and passing
? No compilation errors
? Build successful

## Files Changed

### Production Code (6 files):
1. `Uk.Parliament/Petitions/IPetitionsApi.cs`
2. `Uk.Parliament/Members/IMembersApi.cs`
3. `Uk.Parliament/Bills/IBillsApi.cs`
4. `Uk.Parliament/Committees/ICommitteesApi.cs`
5. `Uk.Parliament/Divisions/IDivisionsApi.cs`
6. `Uk.Parliament/Petitions/ApiResponse.cs` (renamed class)
7. `Uk.Parliament/ParliamentClient.cs`
8. `Uk.Parliament/Petitions/PetitionsApiExtensions.cs`

### Test Code (2 files):
1. `Uk.Parliament.Test/Petitions.cs`
2. `Uk.Parliament.Test/ParliamentClientTests.cs`

## Summary

? **All API interfaces** consolidated into `Uk.Parliament.Interfaces` namespace
? **ApiResponse** renamed to `ParliamentApiResponse` to avoid conflicts
? **All references** updated throughout the codebase
? **All tests** updated and passing
? **Build** successful with zero errors

The codebase is now more organized, consistent, and free of ambiguous type references!
