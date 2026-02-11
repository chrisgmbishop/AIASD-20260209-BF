---
provenance:
  created: "2026-02-11"
  source: "Cursor IDE (AI-assisted)"
  purpose: "Gap analysis of non-test safety nets (CI, static analysis, security, error handling, validation, observability)."
  document_type: "report"
---

# PostHub API – Missing Safety Nets (Excluding Tests)

**Date:** 2026-02-11  
**Scope:** Brownfield PostHubAPI (.NET 8). This document identifies safety nets **other than tests** that are missing or weak. Test gaps are covered in `tests-gap-analysis-20260211.md`.

---

## Executive Summary

Besides the absence of tests, the codebase lacks **CI/CD**, **static analysis and code-style enforcement**, **global error handling**, **health checks**, **dependency and secret scanning**, **rate limiting**, **structured logging**, and **consistent validation**. Addressing these reduces production risk, improves debuggability, and enforces standards at build and deploy time.

---

## 1. CI/CD and Quality Gates

| Item | Status | Risk |
|------|--------|------|
| GitHub Actions (or other CI) | **Missing** – no `.github/workflows` | No automated build, test, or deploy; broken or insecure code can be merged. |
| Build on PR/push | **Missing** | No guarantee that the solution compiles before merge. |
| Test run in CI | **Missing** (and no tests yet) | No regression signal. |
| Quality gates (e.g. fail on warnings) | **Missing** | Warnings can accumulate; no enforcement. |

**Recommendation:** Add a workflow that restores, builds, and (once tests exist) runs tests on push/PR. Optionally run `dotnet format --verify-no-changes` and fail on warnings.

---

## 2. Static Analysis and Code Style

| Item | Status | Risk |
|------|--------|------|
| `.editorconfig` | **Missing** | No shared formatting, naming, or style rules across editors. |
| `TreatWarningsAsErrors` / `WarningsAsErrors` | **Not set** in `PostHubAPI.csproj` | Build succeeds with warnings; issues go unnoticed. |
| `EnforceCodeStyleInBuild` | **Not set** | Code-style analyzers are not run in CI. |
| Roslyn / StyleCop analyzers | **Not referenced** | No automated style or rule enforcement (e.g. naming, async suffix). |
| `Directory.Build.props` | **Missing** | No shared MSBuild defaults (e.g. nullable, warnings) across projects. |

**Recommendation:** Add `.editorconfig`, enable `TreatWarningsAsErrors` (or a curated `WarningsAsErrors` list), and consider adding Microsoft.CodeAnalysis.NetAnalyzers (or similar) and optionally StyleCop. Use `Directory.Build.props` if you add more projects.

---

## 3. Security

| Item | Status | Risk |
|------|--------|------|
| Dependabot (or similar) | **Missing** | No automated dependency vulnerability alerts or PRs. |
| CodeQL / SAST in CI | **Missing** | No automated security scanning of code. |
| Secret scanning | **Missing** | JWT secret is in `appsettings.json` (see audit-report); no pre-push or CI check for secrets. |
| Rate limiting | **Missing** | API is open to abuse (e.g. brute-force on Login, spam on Register/Post/Comment). |
| CORS | **Not explicitly configured** | `AllowedHosts: "*"` is permissive; CORS policy not defined for API. |

**Recommendation:** Enable Dependabot for the repo. Move secrets to environment or a secret manager; add secret scanning (e.g. GitHub secret scanning, or a pre-commit/CI step). Add rate limiting (e.g. ASP.NET Core rate limiting middleware). Define an explicit CORS policy. Consider CodeQL or another SAST tool in CI.

---

## 4. Error Handling and Resilience

| Item | Status | Risk |
|------|--------|------|
| Global exception handler / filter | **Missing** | Unhandled exceptions result in default 500 responses; no consistent error shape or logging. |
| Exception middleware or `UseExceptionHandler` | **Missing** | No central place to log and format errors. |
| Health checks | **Missing** | No `/health` (or similar) for load balancers, containers, or monitoring. |
| DbContext health check | **Missing** | No check that the app can reach the database (even if InMemory). |

**Recommendation:** Add a global exception handler (middleware or `IExceptionFilter`) that logs and returns a consistent error DTO (and optionally correlation ID). Add `AddHealthChecks` / `MapHealthChecks`; when moving to a real DB, add a DB health check.

---

## 5. Validation

| Item | Status | Risk |
|------|--------|------|
| DTO validation consistency | **Inconsistent** | `CreatePostDto` has `[Required]` and `[StringLength]`; `EditPostDto` has none – null or oversized values can reach services. |
| FluentValidation | **Not used** | Architecture instructions mention it; only data annotations are used, and only on some DTOs. |
| Automatic model validation | **Used** | Controllers check `ModelState.IsValid` (good), but DTOs must be fully annotated or validated elsewhere. |

**Recommendation:** Add validation to all input DTOs (e.g. `EditPostDto`, `EditCommentDto`) with `[Required]` and length constraints where appropriate. Optionally introduce FluentValidation for complex rules and consistency with architecture docs.

---

## 6. Observability

| Item | Status | Risk |
|------|--------|------|
| Structured logging | **Default only** | Standard ASP.NET Core logging; no structured (e.g. JSON) logs or correlation IDs. |
| Request/response logging middleware | **Missing** | Harder to debug and audit API usage. |
| Serilog / NLog | **Not used** | No enriched or file/sink-specific logging. |

**Recommendation:** Consider Serilog (or similar) with structured properties and a correlation ID per request. Add request logging middleware (with care to avoid logging sensitive bodies).

---

## 7. API Contract and Documentation

| Item | Status | Risk |
|------|--------|------|
| API versioning | **Missing** | Breaking changes are not explicit; clients cannot pin to a version. |
| OpenAPI security scheme (JWT) in Swagger | **Unverified** | Swagger is present; ensure JWT bearer is documented so UI can send auth. |

**Recommendation:** Add API versioning (e.g. URL or header) when you expect breaking changes. Ensure Swagger/OpenAPI documents the JWT security scheme so the generated UI supports authenticated calls.

---

## 8. Pre-commit / Local Quality

| Item | Status | Risk |
|------|--------|------|
| Pre-commit or similar hooks | **Missing** | No automated format/lint/secret check before commit. |
| `dotnet format` in CI or pre-push | **Missing** | Code style can drift. |

**Recommendation:** Optionally add a pre-commit config (e.g. run `dotnet format`, a secret scanner, or a small script that runs build + format). Run `dotnet format --verify-no-changes` in CI to enforce formatting.

---

## 9. Nullable and Null Safety

| Item | Status | Risk |
|------|--------|------|
| Nullable reference types | **Enabled** in csproj | Good. |
| DTOs with `string` properties | **Not initialized** (e.g. `public string Title { get; set; }`) | Without validation (e.g. on `EditPostDto`), null can reach services and cause runtime issues. |

**Recommendation:** Keep nullable enabled. Ensure every input DTO has `[Required]` (or equivalent) and/or non-nullable types where appropriate so the compiler and validation work together.

---

## Summary Table: Missing or Weak Safety Nets

| Category | Item | Present? |
|----------|------|----------|
| **CI/CD** | GitHub Actions workflow | No |
| | Build on PR/push | No |
| | Test run in CI | No (no tests yet) |
| **Static analysis** | .editorconfig | No |
| | TreatWarningsAsErrors / analyzers | No |
| **Security** | Dependabot / dependency scan | No |
| | Secret scanning | No |
| | Rate limiting | No |
| | Explicit CORS | No |
| **Resilience** | Global exception handler | No |
| | Health checks | No |
| **Validation** | Consistent DTO validation (all DTOs) | No |
| **Observability** | Structured logging / correlation ID | No |
| **API** | Versioning | No |
| **Local** | Pre-commit / format check | No |

### Code coverage (reference)

Code coverage is a **test-related** safety net; targets and current state are defined in the test automation plan and tests gap analysis. This document does not assess coverage.

| Scope | Target (from test plan) | In this document |
|-------|-------------------------|------------------|
| Overall (line) | ≥ 70% | N/A – out of scope |
| Services | High (e.g. ≥ 80%) | N/A – out of scope |
| Controllers | High (e.g. ≥ 75%) | N/A – out of scope |
| New code | ≥ 70% (if gate on) | N/A – out of scope |

See [tests-gap-analysis-20260211.md](tests-gap-analysis-20260211.md) and [test-automation-plan.md](test-automation-plan.md) for coverage goals and Phase 3.

---

## Prioritized Recommendations

1. **High:** Add a minimal CI workflow (build, then tests when they exist). Fix global error handling (exception middleware/filter) and add health checks.
2. **High:** Add dependency and secret scanning (Dependabot + move secrets out of appsettings; optional secret scan in CI). Add rate limiting for auth and write endpoints.
3. **Medium:** Add `.editorconfig` and `TreatWarningsAsErrors` (or selected warnings). Unify DTO validation (all DTOs covered, including `EditPostDto` / `EditCommentDto`).
4. **Medium:** Document JWT in Swagger; add API versioning when you need backward compatibility.
5. **Lower:** Structured logging and correlation ID; pre-commit or CI format check.

---

## Sources Scanned

- `PostHubAPI.csproj`, `Program.cs`, `appsettings.json`
- `.github/` (no `workflows` directory)
- Controllers, Dtos, Exceptions
- `.gitignore`, `.vscode/settings.json`
- `.github/audit-report-20260211.md`, `.github/tests-gap-analysis-20260211.md`
