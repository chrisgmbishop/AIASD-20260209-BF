# Codebase vs. Instructions Audit Report

**Date**: 2026-02-11  
**Scope**: PostHubAPI codebase vs. `.github/instructions` (dotnet-coding-standards, posthub-architecture, posthub-dependency-guide, vertical-slice note)

## Executive Summary

The codebase was audited against the instruction files in `.github/instructions`. Several deviations and bugs were identified. Existing GitHub issues (#3–#8) already cover async Register/Login, JWT key mismatch, redundant null check, error-handling consistency, and DTO validation. **Six new issues** were created for items not already tracked: typo in `CreateNewCommnentAsync`, POST response body standards, XML documentation, JWT `RequireHttpsMetadata`, constructor null validation, and unused BCrypt.Net package.

## Existing Issues (Not Duplicated)

| #   | Title                                                                 | Labels        |
|-----|-----------------------------------------------------------------------|---------------|
| 3   | Bug: UserController Register and Login methods are not async           | bug, high-priority |
| 4   | Critical: JWT configuration uses incorrect key 'Audience' instead of 'ValidAudience' | critical |
| 5–6 | Code Quality: Redundant null check in UserService.Login              | refactor      |
| 7   | Feature: Implement consistent error handling across controllers        | —             |
| 8   | Feature: Add validation consistency to DTO classes                    | —             |

## New Issues Created

| #   | Title                                                                 | Rationale |
|-----|-----------------------------------------------------------------------|-----------|
| 9   | Bug: Typo in method name CreateNewCommnentAsync (should be CreateNewCommentAsync) | ICommentService, CommentService, CommentController use misspelling. |
| 10  | Standards: POST create endpoints should return full resource in response body | PostController and CommentController return only ID; architecture expects CreatedAtAction with ReadDto. |
| 11  | Standards: Add XML documentation to public API (controllers and services) | dotnet-coding-standards require /// summary, param, returns, exception. |
| 12  | Security: JWT RequireHttpsMetadata set to false in Program.cs         | Production security concern; should be true or environment-conditional. |
| 13  | Standards: Service constructors should validate injected dependencies (ArgumentNullException) | Architecture and coding standards show null checks in constructors. |
| 14  | Code quality: Remove or justify unused NuGet package BCrypt.Net       | Identity handles hashing; dependency guide says minimize packages. |

## Deviations Summary

- **Naming**: Async methods should have `Async` suffix (UserService `Register`/`Login` — covered by #3). Typo `CreateNewCommnentAsync` → #9.
- **API contract**: POST create responses should return full resource (architecture) → #10.
- **Documentation**: XML docs missing on public API → #11.
- **Security**: JWT `RequireHttpsMetadata = false` and JWT key names (ValidIssuer/ValidAudience in appsettings vs Issuer/Audience in UserService) → #4, #12.
- **Reliability**: Constructor null validation and redundant null check → #5/#6, #13.
- **Dependencies**: Unused BCrypt.Net → #14.
- **Validation**: DTO validation consistency → #8.

## Sources Scanned

- `.github/instructions/dotnet-coding-standards.instructions.md`
- `.github/instructions/posthub-architecture.instructions.md`
- `.github/instructions/posthub-dependency-guide.instructions.md`
- `.github/instructions/vertical-slice.instructions.md` (noted: PostHubAPI uses layered, not vertical slice)
- `Program.cs`, Controllers, Services, Dtos, Models, Data, Profiles, `PostHubAPI.csproj`, `appsettings.json`

## GitHub Issue Creation

All new issues were created via `gh issue create`. Labels `security` and `code-quality` are not present in the repo, so issues #12–#14 were created without those labels; you can add labels in the repo if desired.
