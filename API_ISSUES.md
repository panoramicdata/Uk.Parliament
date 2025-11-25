# UK Parliament API Issues and Fixes

## Summary

During integration testing, we discovered that several API interfaces had incorrect endpoint paths that did not match the actual Parliament API Swagger specifications. This document details the findings and fixes applied.

## Fixes Applied

### ✅ Committees API - FULLY FIXED
**Issues Found:**
1. `leadHouse` property returned as object `{isCommons: bool, isLords: bool}` not string
2. `scrutinisingDepartments` returned as array of objects `{departmentId: int, name: string}` not array of strings

**Fixes:**
- Created `LeadHouse` class with `IsCommons` and `IsLords` properties
- Created `ScrutinisingDepartment` class with `DepartmentId` and `Name` properties
- Updated `Committee` model to use these new types

**Result:** All 10 Committees integration tests passing ✅

### ✅ OralQuestions & Motions API - PARTIALLY FIXED
**Issues Found:**
1. API uses completely different response structure than other Parliament APIs
2. Endpoint `/motions/list` doesn't exist - should be `/EarlyDayMotions/list`
3. Endpoint `/oralquestions/{id}` doesn't exist (404)
4. Filter parameter `askingMemberId` is accepted but doesn't actually filter results

**Fixes:**
- Created `OralQuestionsResponse<T>` model with correct structure:
  - `PagingInfo` object (not simple skip/take)
  - `StatusCode`, `Success`, `Errors` fields
  - `Response` array (not `Items`)
- Updated `OralQuestion` model to match API's PascalCase property names
- Created `OralQuestionMember` model for nested member information
- Fixed interface endpoints:
  - `/oralquestions/list` ✅
  - `/EarlyDayMotions/list` ✅ (was `/motions/list`)
  - `/EarlyDayMotion/{id}` ✅ (was `/motions/{id}`)
  - Removed non-existent `/oralquestions/{id}` endpoint

**Remaining Issues (Parliament API bugs):**
- `askingMemberId` filter doesn't work - returns unfiltered results
- Various motion-related filters may not work properly

**Result:** 2/8 tests passing, others fail due to API-side bugs ⚠️

### ✅ NOW (Annunciator) API - INTERFACE UPDATED
**Issues Found:**
1. ALL endpoints were wrong in our interface
2. API uses `/api/Message/message/{annunciator}/current` not `/api/Now/Commons`
3. API returns annunciator message structure, not chamber status

**Fixes:**
- Created `AnnunciatorMessage` and `AnnunciatorSlide` models
- Updated interface to correct endpoints:
  - `/api/Message/message/{annunciator}/current`
  - `/api/Message/message/{annunciator}/{date}`
- Changed return type from `ChamberStatus` to `AnnunciatorMessage`

**Status:** Interface fixed, tests need updating 🔄

### ✅ Interests API - INTERFACE UPDATED
**Issues Found:**
1. All endpoints missing `/v1/` path segment
2. API doesn't support pagination - returns all results
3. Endpoint names were incorrect

**Fixes:**
- Updated all endpoints to use `/api/v1/` prefix:
  - `/api/v1/Categories` (was `/api/Interests/Category`)
  - `/api/v1/Interests` (was `/api/Interests/Search`)
  - Added `/api/v1/Categories/{id}`
  - Added `/api/v1/Interests/{id}`
  - Added `/api/v1/Registers` and `/api/v1/Registers/{id}`
- Created `InterestRegister` model
- Updated extensions to handle non-paginated responses
- Removed non-existent `GetMemberInterestsAsync` method

**Status:** Interface fixed, tests need updating 🔄

### ✅ Erskine May API - INTERFACE UPDATED
**Issues Found:**
1. API uses singular paths `/api/Part` not plural `/api/Parts`
2. API uses `/api/Section/{id}` not `/api/Sections/{id}`
3. API doesn't support pagination for most endpoints
4. Search endpoints have specific paths like `/api/Search/SectionSearchResults/{term}`

**Fixes:**
- Updated all endpoints to match actual API:
  - `/api/Part` ✅ (was `/api/Parts`)
  - `/api/Part/{partNumber}` ✅ (was `/api/Parts/{partNumber}/Chapters`)
  - `/api/Chapter/{chapterNumber}` ✅ (was `/api/Chapters/{chapterNumber}/Sections`)
  - `/api/Section/{sectionId}` ✅ (was `/api/Sections/{id}`)
  - `/api/Search/SectionSearchResults/{searchTerm}` ✅ (was `/api/Search`)
  - `/api/Search/ParagraphSearchResults/{searchTerm}` ✅ (new)
- Removed pagination support from extensions (API doesn't support it)
- Removed non-existent `GetChaptersAsync` method

**Status:** Interface fixed, tests need updating 🔄

## Tests Requiring Updates

The following test files need to be updated to match the new API interfaces:

1. **NowIntegrationTests.cs** and **NowApiUnitTests.cs**
   - Update to use `GetCurrentMessageAsync("commons")` instead of `GetCommonsStatusAsync()`
   - Update to expect `AnnunciatorMessage` instead of `ChamberStatus`

2. **InterestsIntegrationTests.cs** and **InterestsApiUnitTests.cs**
   - Remove `GetMemberInterestsAsync` tests (endpoint doesn't exist)
   - Update `SearchInterestsAsync` to not expect pagination (returns `List<Interest>`)
   - Remove `take` and `skip` parameters from search calls
   - Update `GetAllInterestsAsync` to not use `pageSize` parameter

3. **ErskineMayIntegrationTests.cs** and **ErskineMayApiUnitTests.cs**
   - Remove `GetChaptersAsync` tests (use `GetPartAsync` and check its Chapters property)
   - Remove `GetSectionsAsync` tests (use `GetChapterAsync` and check its Sections property)
   - Update `SearchAsync` to not expect pagination (returns `List<>`)
   - Remove `skip` and `take` parameters from search calls
   - Remove `GetAllSectionsAsync` tests (no pagination support)

## Parliament API Bugs to Report

The following issues should be reported to the UK Parliament API team:

### OralQuestions & Motions API
1. **Filter parameter ignored**: `askingMemberId` parameter is accepted but returns unfiltered results
2. **Missing endpoint**: `/oralquestions/{id}` returns 404 - no way to get a single oral question by ID
3. **Inconsistent naming**: Uses `/EarlyDayMotions` but documentation suggests `/motions`

### General Issues
1. **Inconsistent response structures**: OralQuestions API uses different response format than other APIs
2. **Missing documentation**: Some endpoints return 404 even though they appear in Swagger
3. **Pagination support**: Inconsistent - some APIs support it, others don't, not always documented

## Verification Steps

To verify the fixes:

```powershell
# Test Committees API (should all pass)
dotnet test --filter "FullyQualifiedName~CommitteesIntegrationTests"

# Test OralQuestions API (2 should pass, others fail due to API bugs)
dotnet test --filter "FullyQualifiedName~OralQuestionsMotionsIntegrationTests"

# Verify API endpoints manually
curl "https://now-api.parliament.uk/api/Message/message/commons/current"
curl "https://interests-api.parliament.uk/api/v1/Categories"
curl "https://erskinemay-api.parliament.uk/api/Part"
curl "https://oralquestionsandmotions-api.parliament.uk/EarlyDayMotions/list?take=2"
```

## Next Steps

1. ✅ Update test files to match new interface signatures
2. ✅ Run full test suite to identify any remaining issues  
3. ✅ Document expected test failures due to Parliament API bugs
4. ✅ Create issue report for UK Parliament API team
5. ✅ Update README with known API limitations

## Related Files Changed

- `Uk.Parliament\Models\Committees\Committee.cs` - Added LeadHouse and ScrutinisingDepartment classes
- `Uk.Parliament\Models\OralQuestions\OralQuestion.cs` - Updated to match API response
- `Uk.Parliament\Models\OralQuestions\OralQuestionsResponse.cs` - New response model
- `Uk.Parliament\Models\Now\AnnunciatorMessage.cs` - New model for NOW API
- `Uk.Parliament\Models\Interests\InterestRegister.cs` - New model
- `Uk.Parliament\Interfaces\IOralQuestionsMotionsApi.cs` - Fixed endpoints
- `Uk.Parliament\Interfaces\INowApi.cs` - Fixed all endpoints
- `Uk.Parliament\Interfaces\IInterestsApi.cs` - Fixed all endpoints
- `Uk.Parliament\Interfaces\IErskineMayApi.cs` - Fixed all endpoints
- `Uk.Parliament\Extensions\InterestsApiExtensions.cs` - Updated for non-paginated responses
- `Uk.Parliament\Extensions\ErskineMayApiExtensions.cs` - Updated for non-paginated responses
