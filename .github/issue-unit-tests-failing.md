# Issue: Unit tests failing when running `dotnet test`

Use this file as the body when creating the GitHub issue (e.g. `gh issue create --title "..." --body-file .github/issue-unit-tests-failing.md`).

---

## Summary

Running the unit tests in `PostHubAPI.Tests` produces one or more failures. Fixes are needed so that `dotnet test` passes and CI can enforce tests.

## Steps to reproduce

1. Open the repo and ensure dependencies are restored: `dotnet restore`
2. Build: `dotnet build`
3. Run tests: `dotnet test`

## Actual result

(Paste the full output of `dotnet test` here, including any failed test names and stack traces.)

```
Replace this block with your terminal output.
```

## Expected result

All tests in `PostHubAPI.Tests` should pass.

## Environment

- OS: macOS (as per project environment)
- .NET: 8.0.x
- Project: PostHubAPI.sln (PostHubAPI + PostHubAPI.Tests)

## Related

- [Test automation plan](.github/test-automation-plan.md)
- [Tests gap analysis](.github/tests-gap-analysis-20260211.md)

## Labels (suggested)

`bug`, `testing`, `good first issue` (optional)
