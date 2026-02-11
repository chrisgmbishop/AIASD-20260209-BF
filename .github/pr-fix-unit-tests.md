# PR: Fix failing unit tests

Use this file as the body when creating the pull request (e.g. `gh pr create --title "Fix failing unit tests" --body-file .github/pr-fix-unit-tests.md`).

---

## Summary

Fixes failing unit tests so that `dotnet test` passes. Addresses the issue created for test failures (link the issue number when creating the PR, e.g. "Fixes #N").

## Changes

- (To be filled when applying fixes: list of files/changes, e.g. test code fixes, dependency updates, or product code fixes required for tests to pass.)

## How to verify

1. `dotnet restore`
2. `dotnet build`
3. `dotnet test` — all tests should pass.

## Related

- Issue: (add issue number after creating it, e.g. "Fixes #123")
- [Test automation plan](.github/test-automation-plan.md)

## Checklist

- [ ] All unit tests pass locally.
- [ ] No new warnings introduced.
- [ ] CI workflow runs and test step passes (once `continue-on-error` is removed).
