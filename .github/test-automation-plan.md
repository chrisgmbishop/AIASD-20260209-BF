---
provenance:
  created: "2026-02-11"
  created_by: "AI-assisted (Cursor)"
  source: "PostHub brownfield test automation plan"
  version: "1.0"
  scope: "unit, integration, CI automation"
---

# PostHub API – Test Automation Plan

**Version:** 1.0  
**Date:** 2026-02-11  
**Scope:** PostHub brownfield codebase (.NET 8 API) – unit, integration, and CI automation.

---

## 1. Executive Summary

This plan defines how test automation is implemented, run, and maintained for the PostHub API. It aligns with the existing [tests gap analysis](.github/tests-gap-analysis-20260211.md) and establishes a test pyramid (unit → integration → optional API/E2E), CI integration, coverage goals, and ownership.

**Current state (as of plan date):**

- **Unit tests:** Present in `PostHubAPI.Tests` (xUnit, Moq, EF InMemory) for all three services and all three controllers (~46 tests).
- **Integration / API tests:** Not yet implemented.
- **CI:** `.github/workflows/build.yml` runs `dotnet test` with `continue-on-error: true`; should be enforced once tests are stable.
- **Coverage:** No coverage reporting or gates yet.

**Goals:**

- Automate all tests in CI and enforce pass/fail.
- Add integration/API tests for critical HTTP and auth flows.
- Introduce coverage reporting and optional quality gates.
- Keep tests fast, reliable, and maintainable.

---

## 2. Test Strategy

### 2.1 Test Pyramid

| Layer | Purpose | Scope | Tools | Target (relative) |
|-------|---------|--------|-------|-------------------|
| **Unit** | Business logic, controllers with mocks | Services, controllers, helpers | xUnit, Moq, EF InMemory | Majority (~70%) |
| **Integration** | Real HTTP, DB, auth pipeline | API endpoints, DbContext, Identity | xUnit, WebApplicationFactory, TestServer | Significant (~25%) |
| **E2E** (optional) | Full stack, external contracts | Smoke / critical paths | Same or Playwright/API client | Few (~5%) |

- **Unit:** Fast, no I/O, mock external dependencies; cover services and controller behaviour (status codes, validation, exceptions).
- **Integration:** Real app host (or TestServer), in-memory or test DB; cover HTTP status, auth, and persistence.
- **E2E:** Optional; only for high-value flows if needed later.

### 2.2 In-Scope vs Out-of-Scope

**In scope for automation:**

- All public API endpoints (User, Post, Comment).
- Service layer logic (UserService, PostService, CommentService).
- Controller behaviour (validation, NotFound, BadRequest, Created location).
- Auth: Register, Login, JWT shape; `[Authorize]` on CommentController.
- Data: CRUD, `NotFoundException`, Post–Comment relationship and cascade.
- CI: run tests on every push/PR to `main`; optionally block merge on failure.

**Out of scope (or manual):**

- Third-party services (no external HTTP in tests).
- UI (no front-end in this repo).
- Load/performance tests (separate plan if needed).
- Security penetration tests (separate process).

---

## 3. Tools and Frameworks

### 3.1 Current (Unit Tests)

| Concern | Choice | Version / notes |
|---------|--------|------------------|
| Test framework | xUnit | 2.6.x |
| Mocking | Moq | 4.20.x |
| In-memory DB | EF Core InMemory | 8.0.x |
| Runner | dotnet test | Built-in |
| Assertions | xUnit (Assert.*) | — |

### 3.2 Recommended Additions

| Concern | Choice | Purpose |
|---------|--------|---------|
| Integration host | `Microsoft.AspNetCore.Mvc.Testing` | `WebApplicationFactory<Program>` for in-process API tests |
| Coverage | coverlet + ReportGenerator | Line/code coverage; optional XML/HTML report |
| CI | GitHub Actions | Already present; enforce test step |

### 3.3 Project Layout

```
PostHubAPI.sln
├── PostHubAPI/                    # Production API
└── PostHubAPI.Tests/              # All automated tests
    ├── PostHubAPI.Tests.csproj
    ├── Helpers/                   # Shared test utilities
    ├── Services/                  # Service unit tests
    ├── Controllers/               # Controller unit tests (mocked services)
    └── Integration/               # (Phase 2) API integration tests
```

- Keep a single test project until test count or build time justifies splitting (e.g. `PostHubAPI.IntegrationTests`).
- Shared helpers (e.g. `InMemoryDbContextHelper`, `JwtConfigurationHelper`) stay in `Helpers/`.

---

## 4. CI/CD Integration

### 4.1 Current Workflow

- **File:** `.github/workflows/build.yml`
- **Triggers:** Push and pull_request to `main`
- **Steps:** Checkout → Setup .NET 8 → Restore → Build → **Test** → Format check
- **Test step:** `dotnet test --no-build --configuration Release --verbosity normal` with `continue-on-error: true`

### 4.2 Recommended Changes

| Change | Action |
|--------|--------|
| Enforce tests | Set `continue-on-error: false` for the Test job once all tests pass consistently. |
| Fail fast | Keep `--no-build`; run tests in same job as build for clear feedback. |
| Optional coverage | Add coverlet collector; publish coverage artifact (e.g. Codecov/Coveralls) in a follow-up. |
| Matrix (optional) | If multiple .NET versions are supported later, add a strategy matrix for `dotnet-version`. |

### 4.3 Example: Enforced Test Step

```yaml
- name: Test
  run: dotnet test --no-build --configuration Release --verbosity normal
  # continue-on-error removed so PRs must pass tests
```

### 4.4 Branch Policy Suggestion

- Require status checks for `main`: ensure the “Build” (or “Test”) workflow succeeds before merge.
- Optionally: require PR review; keep format check as advisory until team enforces it.

---

## 5. Phased Rollout

### Phase 1: Stabilise Unit Tests and CI (Current / Short Term)

| Task | Owner | Done when |
|------|--------|------------|
| Run `dotnet test` locally; fix any failures or environment issues | Dev | All 46+ unit tests pass |
| Set Test step `continue-on-error: false` in `build.yml` | Dev/DevOps | CI fails PRs when tests fail |
| Document “how to run tests” in README or CONTRIBUTING | Dev | New contributors can run tests |

**Exit criteria:** Every push/PR runs unit tests; failing tests block merge.

---

### Phase 2: Integration / API Tests

| Task | Owner | Done when |
|------|--------|------------|
| Add `Microsoft.AspNetCore.Mvc.Testing` to test project | Dev | Package ref added |
| Create `Integration/` (or equivalent) folder and first API test class | Dev | e.g. `AuthApiTests.cs` |
| Implement tests: POST Register → 200 + JWT; POST Login → 200 + JWT; invalid credentials → 400 | Dev | Auth contract covered |
| Add tests for Post CRUD (GET/POST/PUT/DELETE) and Comment CRUD with auth | Dev | Critical API paths covered |
| Use in-memory DB or test-specific config so integration tests do not hit real DB | Dev | No shared state; repeatable |

**Exit criteria:** At least one integration test per critical flow (auth, posts, comments); run in CI with unit tests.

---

### Phase 3: Coverage and Quality Gates (Optional)

| Task | Owner | Done when |
|------|--------|------------|
| Add coverlet MSBuild package to test project | Dev | Coverage data generated |
| Configure `dotnet test` to collect coverage (e.g. Cobertura/OpenCover) | Dev | Artifact available |
| Publish coverage report (e.g. GitHub Actions artifact or Codecov) | DevOps | Visible in PR/UI |
| Optional: fail if coverage drops below X% (e.g. 70%) or if uncovered lines increase | Team | Policy agreed and implemented |

**Exit criteria:** Coverage visible; optional gate in place if desired.

---

### Phase 4: E2E or Contract Tests (Optional, Later)

| Task | Owner | Done when |
|------|--------|------------|
| Decide if E2E is needed (e.g. deployed API + smoke tests) | Product/Dev | Decision documented |
| If yes: add minimal E2E project or suite (e.g. HTTP client against test environment) | Dev | Smoke tests run in release pipeline or nightly |

---

## 6. Coverage and Quality Gates

### 6.1 Coverage Goals

| Scope | Target | Notes |
|-------|--------|--------|
| Overall (line) | ≥ 70% | Aspirational; adjust with team |
| Services | High (e.g. ≥ 80%) | Core business logic |
| Controllers | High (e.g. ≥ 75%) | Status codes and validation |
| New code | ≥ 70% | Enforce in PR if gate is on |

- Use coverage to find gaps, not as the only quality measure.
- Prefer meaningful assertions over maximising line count.

### 6.2 Quality Practices

- **Naming:** `MethodName_Scenario_ExpectedResult` (e.g. `GetPostById_WhenNotFound_ReturnsNotFound`).
- **Arrange–Act–Assert:** One logical assertion per test where possible.
- **Isolation:** No shared mutable state; fresh in-memory DB or mocks per test.
- **Speed:** Unit tests &lt; a few seconds total; integration tests &lt; ~30 s total so CI stays fast.
- **Determinism:** No flaky tests; avoid time-dependent or random behaviour without seeding.

---

## 7. Maintenance and Ownership

### 7.1 Responsibilities

| Role | Responsibility |
|------|----------------|
| **Developers** | Write and update unit and integration tests with new features; fix failing tests. |
| **PR author** | Ensure tests pass locally and in CI before requesting review. |
| **Reviewers** | Check that new behaviour is tested and that tests are clear and stable. |
| **DevOps / maintainers** | Keep CI workflow and runners working; update test/coverage tooling if needed. |

### 7.2 When to Update This Plan

- New test type introduced (e.g. E2E, performance).
- New project or service added to the solution.
- Change of CI platform or branching model.
- Coverage or quality gate policy change.

### 7.3 Related Documents

- [Tests gap analysis](.github/tests-gap-analysis-20260211.md) – gaps and recommendations.
- [Tests gap analysis prompt](.github/prompts/tests-gap-analysis.prompt.md) – for re-running gap analysis.
- `.github/workflows/build.yml` – CI build and test definition.

---

## 8. Quick Reference

### Run Tests Locally

```bash
dotnet restore
dotnet build
dotnet test
# Or: dotnet test --filter "FullyQualifiedName~UserServiceTests"
```

### Add a Unit Test

1. Add a test class in `PostHubAPI.Tests/Services/` or `Controllers/` (or `Integration/` later).
2. Use xUnit `[Fact]` / `[Theory]` and Moq for dependencies.
3. Use `InMemoryDbContextHelper` for any test that needs a DbContext.
4. Run `dotnet test` and commit with the feature.

### CI Behaviour

- **On push/PR to `main`:** Checkout → .NET 8 → Restore → Build → Test → Format check.
- After Phase 1: Test step must pass for merge (no `continue-on-error`).

---

## 9. Summary

| Item | Status / Next step |
|------|---------------------|
| Unit tests | In place (~46 tests); stabilise and enforce in CI |
| Integration tests | Planned (Phase 2); WebApplicationFactory + in-memory DB |
| CI | Use existing workflow; turn off `continue-on-error` for Test |
| Coverage | Optional (Phase 3); coverlet + report |
| Ownership | Developers own tests; reviewers check coverage of new code |
| Plan updates | Revisit when adding E2E, new projects, or new gates |

This plan gives the team a single place to align on test automation scope, tools, CI, and rollout. Adjust phases and targets to match team capacity and risk tolerance.
