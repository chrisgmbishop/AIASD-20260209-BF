---
provenance:
  created: "2026-02-11"
  created_by: "AI-assisted (Cursor)"
  source: "tests gap analysis for PostHub brownfield"
  version: "1.0"
  scope: "all production code, test coverage gaps"
---

# PostHub API – Tests Gap Analysis

**Date:** 2026-02-11  
**Scope:** Brownfield PostHubAPI (.NET 8) – all production code, no test projects present.

---

## Executive Summary

The PostHub brownfield system has **no test projects or test files**. The solution contains a single project (`PostHubAPI`) with three controllers, three services, EF Core + Identity, JWT auth, and AutoMapper. All of this is currently untested. The highest risk is **user auth (Register/Login)** where the controller does not await async service calls, which would return a serialized `Task` instead of a JWT (a bug tests would immediately catch). **Risk level: High** – no automated safety net for regressions, auth, or data flows.

---

## Detected Testing Setup

| Item | Status |
|------|--------|
| Test framework | **None** – no xUnit, NUnit, or MSTest references |
| Test projects | **None** – solution has only `PostHubAPI.csproj` |
| Test files | **None** – no `*Test*.cs` or `*Tests/**` found |
| CI test steps | **None** – no workflow running tests |
| Coverage | **N/A** |

**Conclusion:** No testing infrastructure exists; the following gaps assume introducing a unit-test project and optionally an integration/API test project.

---

## Test Inventory

| Type | Count | Notes |
|------|--------|------|
| Unit | 0 | No service or domain tests |
| Integration | 0 | No API or DbContext tests |
| E2E | 0 | No end-to-end flows |

**Notable production assets (all untested):**

- **Controllers:** `UserController`, `PostController`, `CommentController`
- **Services:** `UserService`, `PostService`, `CommentService`
- **Data:** `ApplicationDbContext`, EF configuration and relationships
- **Supporting:** `NotFoundException`, AutoMapper `PostProfile`, `CommentProfile`, `Program.cs` DI and pipeline

---

## Critical Functionality Mapped

| Area | Entry points | Behaviour to test |
|------|----------------|-------------------|
| **Auth** | `UserController.Register`, `UserController.Login` | Validation, duplicate user, wrong password, JWT shape; **controller must await async calls** |
| **Posts** | `PostController` (GetAll, GetById, Create, Edit, Delete) | CRUD, `NotFoundException` for missing id, model validation |
| **Comments** | `CommentController` (Get, Create, Edit, Delete) | CRUD, link to post, `NotFoundException`, `[Authorize]` |
| **Services** | `UserService`, `PostService`, `CommentService` | Business rules, Identity results, EF persistence, mapping |
| **Data** | `ApplicationDbContext` | Model config, cascade delete Post→Comments |
| **API contract** | All controller actions | Status codes, response bodies, location headers |

---

## Gaps / Risks Table

| Area / target | Missing type | Scenario | Risk / impact | Effort |
|---------------|--------------|----------|----------------|--------|
| **UserController.Register / Login** | Unit + integration | Async not awaited; returns `Task` instead of JWT | Clients get invalid response; auth broken | S (fix) + M (tests) |
| **UserService** | Unit | Register duplicate email, Login wrong password, null user, Identity errors | Regressions in auth and error messages | M |
| **PostService** | Unit | GetAll, GetById (found/not found), Create, Edit, Delete; NotFoundException | Data and API consistency | M |
| **CommentService** | Unit | Get, Create (valid/invalid postId), Edit, Delete; NotFoundException | Comment–post relationship and errors | M |
| **PostController** | Unit / integration | All actions, ModelState invalid, NotFound handling | Status codes and response shape | M |
| **CommentController** | Unit / integration | All actions, auth required, NotFound, invalid postId | Security and consistency | M |
| **UserController** | Unit / integration | Register/Login validation, ArgumentException→BadRequest | Validation and error contract | S–M |
| **ApplicationDbContext** | Integration | InMemory provider, Post–Comment relationship, cascade delete | Schema and behaviour in tests | M |
| **AutoMapper profiles** | Unit | PostProfile, CommentProfile mappings | Wrong DTOs or missing fields | S |
| **NotFoundException** | Unit | Thrown and mapped to 404 in controllers | Consistent error handling | S |
| **Program.cs** | Integration | DI, auth pipeline, Swagger in development | Startup and config regressions | L |

---

## Recommended Tests (Prioritized)

### High priority

1. **Fix and test UserController auth**
   - **Fix:** Make `Register` and `Login` `async Task<IActionResult>` and `await userService.Register(dto)` / `await userService.Login(dto)`.
   - **Test (integration):** POST Register with valid DTO → 200 and body is a JWT string (not a Task). POST Login with valid creds → 200 and JWT. Invalid model → 400. Duplicate email / wrong password → 400 with message.

2. **UserService unit tests**
   - Register: success returns token; duplicate email throws ArgumentException; invalid password triggers Identity errors and ArgumentException.
   - Login: valid creds return token; unknown user throws; wrong password throws. Use mocked `UserManager<User>` and `IConfiguration`.

3. **PostService unit tests**
   - GetAllPostsAsync returns mapped DTOs; GetPostByIdAsync returns post when exists, throws NotFoundException when not; CreateNewPostAsync returns new id; EditPostAsync updates and returns DTO, throws when id missing; DeletePostAsync removes or throws. Use in-memory EF or mocked `ApplicationDbContext`.

4. **CommentService unit tests**
   - GetCommentAsync found/not found; CreateNewCommnentAsync (fix typo in method name) for valid/invalid postId; EditCommentAsync and DeleteCommentAsync with NotFound. Use in-memory EF or mocks.

### Medium priority

5. **PostController tests (unit or integration)**
   - GetAll → 200 and list; GetById existing → 200, missing → 404; Create valid → 201 and Location; Create invalid model → 400; Edit/Delete existing → 200/204, missing → 404.

6. **CommentController tests**
   - All actions require auth (integration test with JWT or unit test with authorized context). Get/Create/Edit/Delete success and NotFound → 404.

7. **ApplicationDbContext / EF integration**
   - Create Post with Comments; delete Post and assert Comments removed (cascade). Use `UseInMemoryDatabase` with unique name per test.

8. **AutoMapper**
   - Assert `PostProfile` and `CommentProfile` map Create/Edit/Read DTOs to/from entities without missing or wrong properties.

### Lower priority

9. **NotFoundException**
   - Unit test that each controller maps `NotFoundException` to 404 with message (or shared filter tested once).

10. **Program / startup**
    - Integration: app builds, auth middleware and Swagger configured; optional health or minimal endpoint.

---

## Coverage Goals and Plan

### Targets

- **Overall:** Aim for ≥70% line coverage on `Services` and `Controllers` once tests exist.
- **Critical:** 100% of auth (UserController + UserService) and of NotFound paths in Post and Comment flows.

### Phased plan

| Phase | Focus | Deliverable |
|-------|--------|-------------|
| **1 – Quick wins** | Add test project (xUnit + Moq + in-memory EF). Fix UserController async bug. Add UserService unit tests and one integration test for Register/Login returning JWT. | Auth path safe and testable. |
| **2 – Core services** | PostService and CommentService unit tests (in-memory DbContext). PostController and CommentController tests for status codes and NotFound. | CRUD and error handling covered. |
| **3 – Integration and polish** | DbContext relationship/cascade tests. AutoMapper tests. Optional API integration tests with `WebApplicationFactory`. CI step to run tests. | Baseline coverage and CI in place. |

---

## Additional Finding: Bug in UserController

`UserController.Register` and `UserController.Login` call `IUserService.Register(dto)` and `IUserService.Login(dto)`, which return `Task<string>`. The controller does not await them and returns `Ok(token)` where `token` is a `Task<string>`. The API therefore returns a serialized Task object instead of the JWT string. This should be fixed regardless of tests:

- Change action signatures to `public async Task<IActionResult> Register(...)` and `public async Task<IActionResult> Login(...)`.
- Use `var token = await userService.Register(dto);` and `var token = await userService.Login(dto);`.

---

## Sources Scanned

- `PostHubAPI.sln` – single project, no test projects
- `PostHubAPI.csproj` – no test framework references
- `Controllers/` – UserController, PostController, CommentController
- `Services/Implementations/` and `Services/Interfaces/` – User, Post, Comment
- `Data/ApplicationDbContext.cs`, `Program.cs`
- `Models/`, `Dtos/`, `Profiles/`, `Exceptions/NotFoundException.cs`
- Repo-wide search for `*Test*.cs` and `*Tests/**/*.cs` – no matches

---

## Summary Table: Missing Tests by Component

| Component | Unit tests | Integration/API tests |
|-----------|------------|------------------------|
| UserController | ❌ | ❌ |
| PostController | ❌ | ❌ |
| CommentController | ❌ | ❌ |
| UserService | ❌ | — |
| PostService | ❌ | — |
| CommentService | ❌ | — |
| ApplicationDbContext | — | ❌ |
| AutoMapper profiles | ❌ | — |
| NotFoundException handling | ❌ | ❌ |
| Program / startup | — | ❌ |

**Total:** No tests exist today; the recommendations above define the minimal set to add.
