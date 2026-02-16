# Plan: Remove Redundant Null Check in UserService.Login

**Date**: 2026-02-11  
**Scope**: `Services/Implementations/UserService.cs` — `Login` method  
**Related GitHub issues**: #5, #6 (Code Quality: Redundant null check in UserService.Login)  
**Instruction sources**: `.github/instructions/` (posthub-architecture, dotnet-coding-standards, posthub-ai-guidelines, README), `.github/create-issue-and-pr-commands.md`, `.github/audit-report-20260211.md`

---

## 1. Objective

- Remove the redundant null check on the `user` variable in `UserService.Login` so there is exactly one clear guard.
- Resolve the change in a structured way per the instruction files (branch, PR, issue linkage).
- Ensure all existing tests remain valid and are updated if behaviour or structure changes.

---

## 2. Current State

### 2.1 Code

- **File**: `Services/Implementations/UserService.cs`
- **Method**: `Login(LoginUserDto dto)`
- **Current flow**:
  1. `User? user = await userManager.FindByNameAsync(dto.Username);`
  2. `if (user == null) { throw new ArgumentException(...); }` — **first (and only explicit) null check**
  3. `if (!await userManager.CheckPasswordAsync(user, dto.Password)) { throw ... }`
  4. Build claims from `user.UserName`, `user.Email` and return JWT

**Note**: In the current code there is only one explicit `if (user == null)` check. The audit (issues #5/#6) refers to a "redundant" null check. The redundancy may be (a) a second explicit check that was removed in a prior edit, or (b) the combination of nullable type `User?` and the single explicit check, with the rest of the method still using `user` in a way that could be simplified. The plan below assumes we **keep exactly one null guard** and **remove any redundant check or redundant nullable flow** (e.g. after the guard, use a non-nullable variable so the remainder of the method does not carry redundant null semantics).

### 2.2 Tests

- **`PostHubAPI.Tests/Services/UserServiceTests.cs`**
  - `Login_WithUnregisteredUsername_ThrowsArgumentException` — asserts unregistered user → `ArgumentException` with "not registered".
  - `Login_WithWrongPassword_ThrowsArgumentException` — asserts wrong password → `ArgumentException` with "Unable to authenticate".
  - `Register_ThenLogin_WithSameCredentials_ReturnsToken` — registers then calls `Login` and asserts non-empty token.
- **`PostHubAPI.Tests/Controllers/UserControllerTests.cs`**
  - Controller tests mock `IUserService`; they do not depend on `UserService.Login` internals, only on the contract (return token or throw `ArgumentException`). No test changes expected unless we change that contract.

---

## 3. Instructions to Follow

Per `.github/instructions/README.md` and audit:

1. **Architecture**: `posthub-architecture.instructions.md` — service layer patterns, no business logic in controllers.
2. **Coding standards**: `dotnet-coding-standards.instructions.md` — one clear null check then use; avoid redundant checks; use nullable reference types correctly; no `!` (non-null assertion).
3. **AI guidelines**: `posthub-ai-guidelines.instructions.md` — include null checks for external inputs but avoid redundant ones.
4. **Resolution workflow**: Follow the pattern in `.github/create-issue-and-pr-commands.md` — branch from `main`, make changes, open PR linked to the issue(s).

---

## 4. Implementation Plan (No Code Changes Yet)

### Step 1: Confirm the exact redundancy

- **Action**: Re-read `UserService.Login` and confirm whether:
  - (A) There are two explicit `if (user == null)` (or equivalent) checks — if so, remove the second.
  - (B) There is only one explicit check — then the refactor is: keep that single guard, then assign to a non-nullable local (e.g. `User foundUser = user;`) and use `foundUser` for the rest of the method so there is no redundant nullable flow; optionally change the initial variable to non-nullable after the guard for clarity.
- **Output**: One-sentence description of which case applies and what will be removed/changed.

### Step 2: Create branch and link to issues

- **Action**: From repo root, create a branch for this fix (e.g. `fix/user-service-redundant-null-check` or `refactor/issue-5-6-login-null-check`).
- **Commands** (per create-issue-and-pr-commands pattern):
  - `git checkout main`
  - `git pull origin main`
  - `git checkout -b fix/user-service-redundant-null-check`
- **PR body**: Reference "Fixes #5" and "Fixes #6" (or the single issue that covers both) so the PR is the "place" where the issue is resolved.

### Step 3: Edit UserService.cs only as needed

- **File**: `Services/Implementations/UserService.cs`
- **Rules**:
  - Do not remove the guard that throws when the user is not found (we must keep one null check for `FindByNameAsync` result).
  - Remove the redundant check: either remove a second explicit check or, if only one exists, eliminate redundant nullable usage after the guard (one clear check, then use a non-nullable variable for the rest of the method).
  - Follow dotnet-coding-standards: no `!`, no suppressing nullability; use `ArgumentNullException` only for parameters, not for "user not found" (keep `ArgumentException` for business rule "not registered").
- **Scope**: Limit edits to the `Login` method (and possibly a single local variable); do not change `Register`, `GetToken`, or `GetErrorsText`.

### Step 4: Run tests and fix any failures

- **Action**: Run the test suite and ensure all tests pass.
  - `dotnet test` from solution root.
- **Focus**: `PostHubAPI.Tests/Services/UserServiceTests.cs` — all three Login-related tests must still pass with the same behaviour:
  - Unregistered username → `ArgumentException` with "not registered".
  - Wrong password → `ArgumentException` with "Unable to authenticate".
  - Valid login (after register) → non-empty token.
- **If tests fail**: Treat as "update tests to align with the changes":
  - If the refactor changes only structure (e.g. introducing `foundUser`) and not behaviour, tests should not need edits; fix any production code mistake.
  - If we inadvertently change exception type or message, restore the previous behaviour so existing tests remain valid; do not relax assertions unless we explicitly decide to change the contract.

### Step 5: Update tests only if behaviour or contract changes

- **When to change tests**:
  - Exception type or message for "user not found" or "wrong password" changes by design → update assertions in `UserServiceTests` to match the new contract.
  - New edge cases are added (e.g. explicit test for "null user path") → add tests per dotnet-coding-standards and posthub-architecture.
- **When not to change tests**:
  - If the refactor only removes redundancy and keeps the same public behaviour (same exceptions, same return), existing tests are the alignment check; no test code changes required.

### Step 6: Open PR and close issues

- **Action**: Push the branch, create a PR with title and body that reference the fix (e.g. "Remove redundant null check in UserService.Login (Fixes #5, #6)").
- **Body**: Include brief summary of what was redundant and what was done; link to this plan if desired.
- **Place to resolve**: The PR is the "place" where the issue is resolved; GitHub will close #5 and #6 when the PR is merged (if "Fixes #5" and "Fixes #6" are in the PR body).

---

## 5. Test Alignment Summary

| Test (UserServiceTests) | Current assertion / behaviour | After refactor |
|-------------------------|------------------------------|----------------|
| `Login_WithUnregisteredUsername_ThrowsArgumentException` | `ArgumentException`, message contains "not registered" | Same; no change unless we change contract |
| `Login_WithWrongPassword_ThrowsArgumentException` | `ArgumentException`, message contains "Unable to authenticate" | Same; no change unless we change contract |
| `Register_ThenLogin_WithSameCredentials_ReturnsToken` | Non-empty token returned | Same; no change unless we change contract |

| Test (UserControllerTests) | Dependency | After refactor |
|----------------------------|------------|----------------|
| `Login_WhenModelValid_ReturnsOkWithToken` | Mock `IUserService.Login` | No change; mock unchanged |
| `Login_WhenServiceThrowsArgumentException_ReturnsBadRequest` | Mock throws `ArgumentException` | No change |
| `Login_WhenModelInvalid_ReturnsBadRequest` | Mock not called | No change |

**Conclusion**: If the refactor only removes the redundant null check (or redundant nullable flow) and keeps the same exceptions and return behaviour, **no test updates are required**; the existing tests are the alignment check. If we intentionally change exception type or message, update the corresponding test assertions accordingly.

---

## 6. Checklist Before Making Changes

- [ ] Confirmed which redundancy exists (second explicit check vs. single check + nullable flow).
- [ ] Branch created from up-to-date `main`.
- [ ] Edits limited to `UserService.Login`; no unrelated changes.
- [ ] One null guard retained for "user not found"; redundant check or redundant nullable usage removed.
- [ ] `dotnet test` passes after the change.
- [ ] PR created with "Fixes #5" and "Fixes #6" (or the appropriate issue numbers).
- [ ] This plan document kept as the single place that describes how the issue is being tackled (and optionally linked from the PR).

---

**No code or test changes have been made yet; this document is the plan only.**
