# UK Parliament APIs - Complete Reference

This document provides a comprehensive reference to all publicly available UK Parliament APIs with their official swagger documentation links.

## ?? Official API Documentation

| API | Base URL | Swagger/OpenAPI | Status |
|-----|----------|-----------------|--------|
| **Petitions** | https://petition.parliament.uk/ | [Docs](https://petition.parliament.uk/help#api) | ? Production |
| **Members** | https://members-api.parliament.uk/ | [Swagger](https://members-api.parliament.uk/swagger/v1/swagger.json) | ? Production |
| **Bills** | https://bills-api.parliament.uk/ | [Swagger](https://bills-api.parliament.uk/swagger/v1/swagger.json) | ? Production |
| **Committees** | https://committees-api.parliament.uk/ | [Swagger](https://committees-api.parliament.uk/swagger/v1/swagger.json) | ?? Unstable |
| **Commons Votes** | https://commonsvotes-api.parliament.uk/ | [Swagger](https://commonsvotes-api.parliament.uk/swagger/docs/v1) | ?? 500 Errors |
| **Lords Votes** | https://lordsvotes-api.parliament.uk/ | [Swagger](https://lordsvotes-api.parliament.uk/swagger/v1/swagger.json) | ?? 500 Errors |
| **Interests (Register)** | https://interests-api.parliament.uk/ | [Swagger](https://interests-api.parliament.uk/swagger/v1/swagger.json) | ? Production |
| **Written Questions** | https://questions-statements-api.parliament.uk/ | [Swagger](https://questions-statements-api.parliament.uk/swagger/v1/swagger.json) | ? Production |
| **Oral Questions & Motions** | https://oralquestionsandmotions-api.parliament.uk/ | [Swagger](https://oralquestionsandmotions-api.parliament.uk/swagger/docs/v1) | ? Production |
| **Treaties** | https://treaties-api.parliament.uk/ | [Swagger](https://treaties-api.parliament.uk/swagger/v1/swagger.json) | ? Production |
| **Erskine May** | https://erskinemay-api.parliament.uk/ | [Swagger](https://erskinemay-api.parliament.uk/swagger/v1/swagger.json) | ? Production |
| **NOW (Annunciator)** | https://now-api.parliament.uk/ | [Swagger](https://now-api.parliament.uk/swagger/v1/swagger.json) | ? Production |

---

## 1. ?? Petitions API

**Purpose:** Access public petitions to UK Parliament

**Base URL:** `https://petition.parliament.uk/`

**Documentation:** https://petition.parliament.uk/help#api

### Key Endpoints

```
GET /petitions.json                    - List current petitions
GET /petitions/{id}.json               - Get specific petition
GET /archived/petitions.json           - List archived petitions
GET /archived/petitions/{id}.json      - Get archived petition
```

### Query Parameters

- `search` - Search term
- `state` - open, closed, rejected, awaiting_response, etc.
- `page` - Page number (1-based)
- `page_size` - Results per page (default: 50)

### Implementation Status
- ? **100% Complete** - 27/27 tests passing
- Full model support
- Extension methods for pagination

---

## 2. ?? Members API

**Purpose:** Information about MPs, Lords, constituencies, and political parties

**Base URL:** `https://members-api.parliament.uk/`

**Swagger:** https://members-api.parliament.uk/swagger/v1/swagger.json

### Key Endpoints

```
GET /api/Members/Search                          - Search members
GET /api/Members/{id}                            - Get member details
GET /api/Members/{id}/Biography                  - Get member biography
GET /api/Members/{id}/Contact                    - Get contact details
GET /api/Members/{id}/Voting                     - Get voting history
GET /api/Location/Constituency/Search            - Search constituencies
GET /api/Location/Constituency/{id}              - Get constituency
GET /api/Location/Constituency/{id}/Representations - Get representations
GET /api/Parties/GetActive/{house}               - Get active parties
GET /api/Parties/StateOfTheParties/{house}/{date} - Get party composition
```

### Implementation Status
- ? **100% Complete** - 17/17 tests passing
- Full model support with all properties
- Extension methods for pagination

---

## 3. ?? Bills API

**Purpose:** Parliamentary legislation information

**Base URL:** `https://bills-api.parliament.uk/`

**Swagger:** https://bills-api.parliament.uk/swagger/v1/swagger.json

### Key Endpoints

```
GET /api/v1/Bills                              - List all bills
GET /api/v1/Bills/{billId}                     - Get specific bill
GET /api/v1/Bills/{billId}/Stages              - Get bill stages
GET /api/v1/Bills/{billId}/Stages/{stageId}    - Get stage details
GET /api/v1/Bills/{billId}/Publications        - Get bill publications
GET /api/v1/Bills/{billId}/Stages/{stageId}/Amendments - Get amendments
GET /api/v1/BillTypes                          - Get bill types
GET /api/v1/Sittings                           - Get sitting days
GET /api/v1/Stages                             - Get all stage types
```

### Implementation Status
- ? **100% Complete** - 12/12 tests passing
- Full model support
- Extension methods for pagination

---

## 4. ??? Committees API

**Purpose:** Parliamentary committees, inquiries, and submissions

**Base URL:** `https://committees-api.parliament.uk/`

**Swagger:** https://committees-api.parliament.uk/swagger/v1/swagger.json

### Key Endpoints

```
GET /api/Committees                             - List committees
GET /api/Committees/{id}                        - Get committee details
GET /api/Committees/{id}/Members                - Get committee members
GET /api/Committees/{id}/Events                 - Get committee events
GET /api/CommitteeBusiness                      - List committee business
GET /api/CommitteeBusiness/{id}                 - Get specific inquiry
GET /api/Publications                           - List publications
GET /api/Publications/{id}                      - Get publication
GET /api/WrittenEvidence                        - List written evidence
GET /api/OralEvidence                           - List oral evidence
GET /api/Events                                 - List committee events
GET /api/Events/{id}                            - Get event details
```

### Implementation Status
- ?? **100% Complete (API Unstable)** - 7/13 tests pass
- Full model support
- API returns intermittent 500 errors (Parliament infrastructure issue)
- Extension methods implemented

---

## 5. ??? Commons Votes (Divisions) API

**Purpose:** Voting records from House of Commons

**Base URL:** `https://commonsvotes-api.parliament.uk/`

**Swagger:** https://commonsvotes-api.parliament.uk/swagger/docs/v1

### Key Endpoints

```
GET /data/division/{divisionId}.{format}               - Get specific division
GET /data/divisions.{format}/search                    - Search divisions
GET /data/divisions.{format}/groupedbyparty            - Get votes by party
GET /data/divisions.{format}/membervoting              - Get member voting records
GET /data/divisions.{format}/searchTotalResults        - Get total results count
```

### Implementation Status
- ?? **Interface Complete (API Non-functional)** - 2/8 tests pass
- API returns 500 errors on all endpoints
- Models cannot be created without API responses
- Unit tests pass (interface mocking works)

**Note:** Format parameter should be "json" or "xml"

---

## 6. ?? Lords Votes (Divisions) API

**Purpose:** Voting records from House of Lords

**Base URL:** `https://lordsvotes-api.parliament.uk/`

**Swagger:** https://lordsvotes-api.parliament.uk/swagger/v1/swagger.json

### Key Endpoints

```
GET /data/Divisions                                - List divisions
GET /data/Divisions/{divisionId}                   - Get specific division
GET /data/Divisions/groupedbyparty/{divisionId}    - Get votes by party
GET /data/Divisions/search                         - Search divisions
```

### Implementation Status
- ?? **Interface Complete (API Non-functional)** - 2/7 tests pass
- API returns 500 errors on all endpoints
- Models cannot be created without API responses
- Unit tests pass (interface mocking works)

---

## 7. ?? Member Interests (Register) API

**Purpose:** Register of Members' Financial Interests

**Base URL:** `https://interests-api.parliament.uk/`

**Swagger:** https://interests-api.parliament.uk/swagger/v1/swagger.json

### Key Endpoints

```
GET /api/Interests/Member/{memberId}           - Get member's interests
GET /api/Interests/Category                     - List interest categories
GET /api/Interests/Search                       - Search interests
```

### Implementation Status
- ?? **Not Implemented**
- Swagger spec available
- High priority for Phase 6

---

## 8. ? Written Questions & Statements API

**Purpose:** Written parliamentary questions and ministerial statements

**Base URL:** `https://questions-statements-api.parliament.uk/`

**Swagger:** https://questions-statements-api.parliament.uk/swagger/v1/swagger.json

### Key Endpoints

```
GET /api/writtenquestions/questions            - List written questions
GET /api/writtenquestions/questions/{id}       - Get specific question
GET /api/writtenquestions/questions/{date}/{uin} - Get by date and UIN
GET /api/writtenstatements/statements          - List written statements
GET /api/writtenstatements/statements/{id}     - Get specific statement
GET /api/dailyreports/dailyreports             - List daily reports
```

### Implementation Status
- ?? **Not Implemented**
- Swagger spec available
- Medium priority for Phase 6

---

## 9. ??? Oral Questions & Motions API

**Purpose:** Oral questions and motions in Parliament

**Base URL:** `https://oralquestionsandmotions-api.parliament.uk/`

**Swagger:** https://oralquestionsandmotions-api.parliament.uk/swagger/docs/v1

### Key Endpoints

```
GET /oralquestions/list                        - List oral questions
GET /oralquestions/{id}                        - Get specific question
GET /motions/list                              - List motions
GET /motions/{id}                              - Get specific motion
```

### Implementation Status
- ?? **Not Implemented**
- Swagger spec available
- Medium priority for Phase 7

---

## 10. ?? Treaties API

**Purpose:** International treaties laid before Parliament

**Base URL:** `https://treaties-api.parliament.uk/`

**Swagger:** https://treaties-api.parliament.uk/swagger/v1/swagger.json

### Key Endpoints

```
GET /api/Treaty                                - List treaties
GET /api/Treaty/{id}                           - Get specific treaty
GET /api/Treaty/{id}/BusinessItem              - Get treaty business items
GET /api/GovernmentOrganisation                - List government organizations
```

### Implementation Status
- ?? **Not Implemented**
- Swagger spec available
- Low priority for Phase 7

---

## 11. ?? Erskine May API

**Purpose:** Treatise on parliamentary procedure ("the bible of parliamentary procedure")

**Base URL:** `https://erskinemay-api.parliament.uk/`

**Swagger:** https://erskinemay-api.parliament.uk/swagger/v1/swagger.json

### Key Endpoints

```
GET /api/Part                                  - List all parts
GET /api/Part/{partNumber}                     - Get specific part
GET /api/Chapter/{chapterNumber}               - Get chapter
GET /api/Section/{sectionId}                   - Get section details
GET /api/Search/ParagraphSearchResults/{searchTerm} - Search paragraphs
GET /api/Search/SectionSearchResults/{searchTerm}   - Search sections
GET /api/Search/IndexTermSearchResults/{searchTerm} - Search index
GET /api/IndexTerm/browse                      - Browse index terms
```

### Implementation Status
- ?? **Not Implemented**
- Swagger spec available
- Low priority for Phase 8 (specialist use case)

---

## 12. ?? NOW (Annunciator) API

**Purpose:** Real-time annunciator system data (what's happening now in Parliament)

**Base URL:** `https://now-api.parliament.uk/`

**Swagger:** https://now-api.parliament.uk/swagger/v1/swagger.json

### Key Endpoints

```
GET /api/Message/message/{annunciator}/current     - Get current message
GET /api/Message/message/{annunciator}/{date}      - Get message after date
```

**Annunciator Types:**
- `CommonsMain` - House of Commons main chamber
- `LordsMain` - House of Lords main chamber

### Implementation Status
- ?? **Not Implemented**
- Swagger spec available
- Low priority for Phase 8 (real-time display systems)

---

## Implementation Roadmap

### ? Phase 5 (Current Release - v10.0.0)
- ? Petitions API - Complete
- ? Members API - Complete
- ? Bills API - Complete
- ?? Committees API - Complete (API unstable)
- ?? Commons/Lords Votes - Interfaces only (APIs broken)

### ?? Phase 6 (Next - When Parliament Fixes APIs)
- Fix Commons/Lords Divisions implementations
- Add full models for Divisions
- Unskip integration tests
- Member Interests API
- Written Questions & Statements API

### ?? Phase 7 (Future)
- Oral Questions & Motions API
- Treaties API

### ?? Phase 8 (Future)
- Erskine May API
- NOW (Annunciator) API

---

## API Contact Information

**UK Parliament Digital Service**
- Email: softwareengineering@parliament.uk
- Website: https://www.parliament.uk
- Developer Portal: https://developer.parliament.uk/

---

## Known API Issues

### Committees API
- ?? **Intermittent 500 errors** - Returns "error code: 500" randomly
- Server-side infrastructure problem
- Some requests work, others fail
- No diagnostic information provided

### Commons Votes API
- ?? **All endpoints return 500** - "error code: 500" on every request
- Consistent 100% failure rate
- Cannot retrieve sample responses
- Server-side infrastructure problem

### Lords Votes API
- ?? **All endpoints return 500** - "error code: 500" on every request
- Consistent 100% failure rate
- Cannot retrieve sample responses
- Server-side infrastructure problem

**Recommendation:** Contact softwareengineering@parliament.uk about Divisions API issues

---

## Additional Resources

- [UK Parliament Data Standards](https://www.parliament.uk/site-information/data-protection/open-data-standards/)
- [UK Parliament Open Data](https://www.parliament.uk/site-information/data-protection/open-data/)
- [UK Parliament APIs on GitHub](https://github.com/ukparliament)

---

**Last Updated:** January 2025  
**API Count:** 12 public APIs  
**Implementation Status:** 3/12 fully working, 1/12 partially working, 2/12 broken (Parliament issues), 6/12 planned
