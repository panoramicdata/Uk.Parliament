# Phase 6 Implementation Plan - Detailed Breakdown

## Overview

**Goal:** Implement Member Interests API and Written Questions & Statements API

**Duration:** ~5 hours (excluding 6.1 Divisions which is blocked)

**Status:** Ready to Execute

---

## 6.2 Member Interests API (~2 hours)

### API Overview
- **Base URL:** `https://interests-api.parliament.uk/`
- **Swagger:** https://interests-api.parliament.uk/swagger/v1/swagger.json
- **Purpose:** Register of Members' Financial Interests

### Task Breakdown

#### Step 1: Research API (15 min)
- [ ] Fetch swagger specification
- [ ] Test sample endpoints with `curl`
- [ ] Document response structures
- [ ] Identify required models

#### Step 2: Create Models (30 min)
**Files to create:**
- `Uk.Parliament/Models/Interests/Interest.cs`
- `Uk.Parliament/Models/Interests/InterestCategory.cs`
- `Uk.Parliament/Models/Interests/MemberInterest.cs`
- `Uk.Parliament/Models/Interests/InterestsResponse.cs`

**Estimated models:** 4-6 classes

#### Step 3: Create Interface (15 min)
**File:** `Uk.Parliament/Interfaces/IInterestsApi.cs`

**Expected endpoints:**
```csharp
[Get("/api/Interests/Member/{memberId}")]
Task<InterestsResponse> GetMemberInterestsAsync(int memberId, CancellationToken cancellationToken = default);

[Get("/api/Interests/Category")]
Task<List<InterestCategory>> GetCategoriesAsync(CancellationToken cancellationToken = default);

[Get("/api/Interests/Search")]
Task<PaginatedResponse<Interest>> SearchInterestsAsync(
    [Query] string? searchTerm = null,
    [Query] int? skip = null,
    [Query] int? take = null,
    CancellationToken cancellationToken = default);
```

#### Step 4: Create Extension Methods (15 min)
**File:** `Uk.Parliament/Extensions/InterestsApiExtensions.cs`

**Methods:**
- `GetAllInterestsAsync()` - Streaming pagination
- `GetAllInterestsListAsync()` - Materialized list

#### Step 5: Integrate into ParliamentClient (10 min)
**Updates needed:**
- Add `IInterestsApi Interests { get; }` property
- Initialize in both constructors
- Add base URL to `ParliamentClientOptions`

#### Step 6: Write Tests (35 min)
**File:** `Uk.Parliament.Test/InterestsIntegrationTests.cs`

**Tests to write:**
- GetMemberInterestsAsync_WithValidId_ReturnsInterests
- GetMemberInterestsAsync_WithInvalidId_ThrowsApiException
- GetCategoriesAsync_ReturnsCategories
- SearchInterestsAsync_WithSearchTerm_ReturnsResults
- GetAllInterestsAsync_RetrievesMultiplePages
- InterestsApi_CanBeMocked (unit test)

**Expected:** 8-10 tests

---

## 6.3 Written Questions & Statements API (~3 hours)

### API Overview
- **Base URL:** `https://questions-statements-api.parliament.uk/`
- **Swagger:** https://questions-statements-api.parliament.uk/swagger/v1/swagger.json
- **Purpose:** Written parliamentary questions and ministerial statements

### Task Breakdown

#### Step 1: Research API (20 min)
- [ ] Fetch swagger specification
- [ ] Test sample endpoints with `curl`
- [ ] Document response structures
- [ ] Identify required models

#### Step 2: Create Models (45 min)
**Files to create:**
- `Uk.Parliament/Models/Questions/WrittenQuestion.cs`
- `Uk.Parliament/Models/Questions/QuestionAttributes.cs`
- `Uk.Parliament/Models/Questions/Answer.cs`
- `Uk.Parliament/Models/Questions/WrittenStatement.cs`
- `Uk.Parliament/Models/Questions/StatementAttributes.cs`
- `Uk.Parliament/Models/Questions/DailyReport.cs`
- `Uk.Parliament/Models/Questions/QuestionsResponse.cs`
- `Uk.Parliament/Models/Questions/StatementsResponse.cs`

**Estimated models:** 8-10 classes

#### Step 3: Create Interface (20 min)
**File:** `Uk.Parliament/Interfaces/IQuestionsStatementsApi.cs`

**Expected endpoints:**
```csharp
// Written Questions
[Get("/api/writtenquestions/questions")]
Task<PaginatedResponse<WrittenQuestion>> GetWrittenQuestionsAsync(
    [Query] int? askingMemberId = null,
    [Query] int? answeringMemberId = null,
    [Query] DateTime? tabledWhenFrom = null,
    [Query] DateTime? tabledWhenTo = null,
    [Query] string? answered = null,
    [Query] int? skip = null,
    [Query] int? take = null,
    CancellationToken cancellationToken = default);

[Get("/api/writtenquestions/questions/{id}")]
Task<WrittenQuestion> GetWrittenQuestionByIdAsync(int id, CancellationToken cancellationToken = default);

// Written Statements
[Get("/api/writtenstatements/statements")]
Task<PaginatedResponse<WrittenStatement>> GetWrittenStatementsAsync(
    [Query] DateTime? madeWhenFrom = null,
    [Query] DateTime? madeWhenTo = null,
    [Query] int? skip = null,
    [Query] int? take = null,
    CancellationToken cancellationToken = default);

[Get("/api/writtenstatements/statements/{id}")]
Task<WrittenStatement> GetWrittenStatementByIdAsync(int id, CancellationToken cancellationToken = default);

// Daily Reports
[Get("/api/dailyreports/dailyreports")]
Task<PaginatedResponse<DailyReport>> GetDailyReportsAsync(
    [Query] DateTime? dateFrom = null,
    [Query] DateTime? dateTo = null,
    [Query] string? house = null,
    [Query] int? skip = null,
    [Query] int? take = null,
    CancellationToken cancellationToken = default);
```

#### Step 4: Create Extension Methods (20 min)
**File:** `Uk.Parliament/Extensions/QuestionsStatementsApiExtensions.cs`

**Methods:**
- `GetAllWrittenQuestionsAsync()` - Streaming pagination
- `GetAllWrittenQuestionsListAsync()` - Materialized list
- `GetAllWrittenStatementsAsync()` - Streaming pagination
- `GetAllWrittenStatementsListAsync()` - Materialized list
- `GetAllDailyReportsAsync()` - Streaming pagination

#### Step 5: Integrate into ParliamentClient (10 min)
**Updates needed:**
- Add `IQuestionsStatementsApi QuestionsStatements { get; }` property
- Initialize in both constructors
- Add base URL to `ParliamentClientOptions`

#### Step 6: Write Tests (45 min)
**File:** `Uk.Parliament.Test/QuestionsStatementsIntegrationTests.cs`

**Tests to write:**
- GetWrittenQuestionsAsync_WithNoFilters_Succeeds
- GetWrittenQuestionsAsync_FilterByMember_ReturnsQuestions
- GetWrittenQuestionsAsync_FilterByDateRange_ReturnsQuestions
- GetWrittenQuestionByIdAsync_WithValidId_ReturnsQuestion
- GetWrittenStatementsAsync_WithNoFilters_Succeeds
- GetWrittenStatementsAsync_FilterByDateRange_ReturnsStatements
- GetWrittenStatementByIdAsync_WithValidId_ReturnsStatement
- GetDailyReportsAsync_WithDateRange_ReturnsReports
- GetAllWrittenQuestionsAsync_StreamingResults_Works
- GetAllWrittenStatementsAsync_StreamingResults_Works
- QuestionsStatementsApi_CanBeMocked (unit test)

**Expected:** 12-15 tests

---

## Phase 6 Completion Checklist

### 6.2 Member Interests API
- [ ] Swagger specification fetched and analyzed
- [ ] All models created (4-6 classes)
- [ ] Interface implemented (`IInterestsApi`)
- [ ] Extension methods created
- [ ] Integrated into `ParliamentClient`
- [ ] Tests written (8-10 tests)
- [ ] Tests passing
- [ ] XML documentation complete
- [ ] No compiler warnings

### 6.3 Written Questions & Statements API
- [ ] Swagger specification fetched and analyzed
- [ ] All models created (8-10 classes)
- [ ] Interface implemented (`IQuestionsStatementsApi`)
- [ ] Extension methods created
- [ ] Integrated into `ParliamentClient`
- [ ] Tests written (12-15 tests)
- [ ] Tests passing
- [ ] XML documentation complete
- [ ] No compiler warnings

### Final Phase 6 Tasks
- [ ] Run full test suite
- [ ] Verify no regressions in existing tests
- [ ] Update README.md with new API examples
- [ ] Update MASTER_PLAN.md to mark Phase 6 complete
- [ ] Update version.json (if needed)
- [ ] Commit all changes
- [ ] Create Phase 6 completion summary document

---

## Success Metrics

**Phase 6.2 (Interests):**
- ? 8-10 new tests passing
- ? API fully integrated
- ? Documentation complete

**Phase 6.3 (Questions/Statements):**
- ? 12-15 new tests passing
- ? API fully integrated
- ? Documentation complete

**Overall Phase 6:**
- ? 20-25 new tests added
- ? 2 new APIs fully functional
- ? Test count: 82 ? 102-107 tests
- ? API count: 6 ? 8 APIs implemented
- ? Fully functional APIs: 3 ? 5

---

## Implementation Order

1. **Member Interests API** - Simpler, good warm-up
2. **Written Questions & Statements API** - More complex, multiple endpoints

---

## Time Estimates

| Task | Interests | Questions/Statements | Total |
|------|-----------|----------------------|-------|
| Research | 15 min | 20 min | 35 min |
| Models | 30 min | 45 min | 75 min |
| Interface | 15 min | 20 min | 35 min |
| Extensions | 15 min | 20 min | 35 min |
| Integration | 10 min | 10 min | 20 min |
| Tests | 35 min | 45 min | 80 min |
| **Total** | **2 hours** | **3 hours** | **5 hours** |

---

## Next Steps

**Ready to begin:** Start with Step 1 of 6.2 (Research Member Interests API)

**Command to start:**
```bash
# Fetch swagger spec
curl https://interests-api.parliament.uk/swagger/v1/swagger.json > interests-swagger.json

# Test a sample endpoint
curl "https://interests-api.parliament.uk/api/Interests/Category"
```

---

**Document Created:** January 2025  
**Phase:** 6  
**Status:** Ready to Execute
