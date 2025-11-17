# UK Parliament APIs

This document provides an overview of the various APIs available from the UK Parliament.

## ??? Current Implementation: Petitions API

**Base URL:** `https://petition.parliament.uk/`

### Endpoints
- `GET /petitions.json` - List current petitions
- `GET /petitions/{id}.json` - Get specific petition
- `GET /archived/petitions.json` - List archived petitions
- `GET /archived/petitions/{id}.json` - Get specific archived petition

### Status
? **Fully Implemented** in this library

---

## ??? Other UK Parliament APIs

### 1. Members API
**Base URL:** `https://members-api.parliament.uk/`

Provides information about Members of Parliament (MPs) in the House of Commons.

#### Key Endpoints
- `GET /api/Members/Search` - Search for MPs
- `GET /api/Members/{id}` - Get specific MP details
- `GET /api/Location/Constituency/Search` - Search constituencies
- `GET /api/Parties/StateOfTheParties/{house}/{forDate}` - Get party composition

#### Example Response
```json
{
  "totalResults": 5194,
  "skip": 0,
  "take": 20,
  "items": [{
    "value": {
      "id": 172,
      "nameListAs": "Abbott, Ms Diane",
      "nameDisplayAs": "Ms Diane Abbott",
      "nameFullTitle": "Ms Diane Abbott MP",
      "gender": "F"
    }
  }]
}
```

#### Status
? **Not Implemented** - Potential future addition

---

### 2. Bills API
**Base URL:** `https://bills-api.parliament.uk/`

Provides information about Parliamentary Bills.

#### Key Endpoints
- `GET /api/v1/Bills` - List bills
- `GET /api/v1/Bills/{billId}` - Get specific bill
- `GET /api/v1/Bills/{billId}/Stages` - Get bill stages
- `GET /api/v1/Bills/{billId}/Publications` - Get bill publications

#### Status
? **Not Implemented** - Potential future addition

---

### 3. Committees API  
**Base URL:** `https://committees-api.parliament.uk/`

Provides information about Parliamentary Committees.

#### Key Endpoints
- `GET /api/Committees` - List committees
- `GET /api/Committee/{id}` - Get specific committee
- `GET /api/Committees/{id}/Members` - Get committee members
- `GET /api/Committees/{id}/Inquiries` - Get committee inquiries

#### Status
? **Not Implemented** - Potential future addition

---

### 4. Hansard (Parliamentary Debates)
**Base URL:** `https://hansard.parliament.uk/`

Official record of what is said in Parliament. While there is a website, the API access details are not publicly documented in the same way as the others.

#### Access
- Web interface available at https://hansard.parliament.uk/
- May require special access or different authentication
- Consider using the UK Parliament's official data services

#### Status
? **Not Implemented** - Would require further investigation

---

## ?? Additional Resources

### UK Parliament Open Data Services
- **Parliament Website:** https://www.parliament.uk/
- **Data Homepage:** https://www.parliament.uk/site-information/glossary/open-data/
- **Developer Resources:** https://developer.parliament.uk/ (if available)

### API Documentation
- **Members API Documentation:** https://members-api.parliament.uk/index.html
- **Bills API Documentation:** https://bills-api.parliament.uk/index.html  
- **Committees API Documentation:** https://committees-api.parliament.uk/index.html

---

## ?? Implementation Roadmap

### Priority 1: Current (? Complete)
- [x] Petitions API
- [x] Archived Petitions support
- [x] Complete model coverage with debug validation

### Priority 2: Next Steps (Potential)
- [ ] Members API - MP information and constituencies
- [ ] Bills API - Parliamentary legislation tracking
- [ ] Committees API - Committee information and inquiries

### Priority 3: Future Consideration
- [ ] Hansard integration (if API becomes available)
- [ ] Lords Members API (if it becomes available)
- [ ] Early Day Motions
- [ ] Parliamentary Questions

---

## ?? Contributing

If you're interested in adding support for additional Parliament APIs to this library, please:

1. Check the official API documentation
2. Ensure the API is publicly accessible
3. Follow the existing patterns in the codebase
4. Add comprehensive tests
5. Update this documentation

---

## ?? Legal & Terms

All UK Parliament APIs are subject to their respective terms of use and licensing. Most UK Parliament data is available under the [Open Parliament Licence](https://www.parliament.uk/site-information/copyright-parliament/open-parliament-licence/).

Always check the specific terms for each API before implementation.
