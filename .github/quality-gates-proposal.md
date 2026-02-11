---
provenance:
  created: "2026-02-11"
  source: "Cursor IDE (AI-assisted)"
  purpose: "Proposed quality gates (build, tests, format, coverage, dependencies, branch protection) and phased rollout."
  document_type: "proposal"
---

# PostHub API – Quality Gates Proposal

**Date:** 2026-02-11  
**Purpose:** Define criteria that must pass before code is merged (and optionally before release) to enforce and sustain improvements identified in the test automation plan, safety nets gap analysis, and lint/architecture report.

---

## 1. What Are Quality Gates?

**Quality gates** are automated or manual checks that must succeed before:

- **Merge gate:** A pull request can be merged into `main` (e.g. CI status checks, required reviews).
- **Release gate (optional):** A build is considered releasable (e.g. no high-severity vulnerabilities, coverage above threshold).

This proposal focuses on **merge gates** implemented in CI and branch protection. Each gate is tied to a concrete improvement (tests, format, build, coverage, etc.).

---

## 2. Proposed Gates (Summary)

| Gate | What it enforces | Current state | Phase |
|------|-------------------|---------------|--------|
| **Build** | Solution compiles with no errors | Enforced (build step fails job) | Required now |
| **Tests** | All unit (and later integration) tests pass | `continue-on-error: true` | Required after test sync |
| **Format** | Code matches `dotnet format` | `continue-on-error: true` | Recommended |
| **No new warnings** | Build has zero warnings (or allowed list) | Not enforced | Optional |
| **Coverage** | Line coverage ≥ threshold or no drop | Not implemented | Optional |
| **Dependencies** | No known high/critical vulnerabilities | Dependabot only | Optional |
| **Branch protection** | PR requires passing CI + review | Manual setup | Recommended |

---

## 3. Gate Definitions

### 3.1 Build (Required – Already Effective)

- **Rule:** `dotnet build --no-restore --configuration Release` must exit 0.
- **Enforces:** Compilation, nullable and analyzer issues that are treated as errors.
- **Implementation:** Current workflow; no change. If build fails, the job fails and the status check does not pass.
- **Improvement sustained:** No broken code on `main`.

### 3.2 Tests (Required – After Test/Production Sync)

- **Rule:** `dotnet test --no-build --configuration Release` must exit 0 (all tests pass).
- **Enforces:** Unit tests (and later integration tests) pass; no regressions.
- **Implementation:** In `.github/workflows/build.yml`, remove `continue-on-error: true` from the Test step once tests are fixed and stable (see [lint-and-architecture-report](lint-and-architecture-report-20260211.md) for test/production sync).
- **Improvement sustained:** Test automation plan Phase 1 – failing tests block merge.

```yaml
# After test sync and verification:
- name: Test
  run: dotnet test --no-build --configuration Release --verbosity normal
```

### 3.3 Format (Recommended)

- **Rule:** `dotnet format --verify-no-changes` must exit 0 (no formatting changes required).
- **Enforces:** Consistent style per .editorconfig; no accidental format drift.
- **Implementation:** Remove `continue-on-error: true` from the Format check step when the team agrees. Optionally run `dotnet format` in a pre-commit hook or document “run format before pushing.”
- **Improvement sustained:** Safety nets / local quality; lint report recommendation.

```yaml
- name: Format check
  run: dotnet format --verify-no-changes --verbosity diagnostic
  # continue-on-error: false when enforced
```

### 3.4 No New Warnings (Optional)

- **Rule:** Build must complete with zero warnings (or only an agreed allow-list).
- **Enforces:** No new compiler or analyzer warnings; encourages fixing existing ones over time.
- **Implementation:**
  - Option A: In `PostHubAPI.csproj`, set `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` (or `WarningsAsErrors` for specific codes). Then build gate becomes “no warnings.”
  - Option B: Add a step that runs build, parses output for “warning,” and fails if any (fragile).
- **Recommendation:** Start with `TreatWarningsAsErrors` false; after cleaning warnings, enable it or use a curated `WarningsAsErrors` list.
- **Improvement sustained:** Static analysis and code style (safety nets, analyzers).

### 3.5 Coverage (Optional)

- **Rule:** Line coverage (overall or per assembly) must be ≥ X% and/or must not decrease from previous run.
- **Enforces:** Test automation plan coverage goals (e.g. ≥ 70% overall, high for Services/Controllers).
- **Implementation:**
  - Add coverlet (and optionally ReportGenerator) to the test project.
  - Run `dotnet test` with coverage (e.g. Cobertura/OpenCover); upload artifact or publish to Codecov/Coveralls.
  - Optional: fail the job if coverage &lt; threshold (e.g. 70%) or if coverage drops (e.g. vs. `main`).
- **Improvement sustained:** Test automation plan Phase 3; gap analysis coverage targets.

**Code coverage targets (for gate threshold):**

| Scope | Target % | Notes |
|-------|----------|--------|
| Overall (line) | ≥ 70 | Aspirational; adjust with team |
| Services | ≥ 80 | Core business logic |
| Controllers | ≥ 75 | Status codes and validation |
| New code (PR gate) | ≥ 70 | Optional; enforce in PR when gate is on |

### 3.6 Dependencies (Optional)

- **Rule:** No known high- or critical-severity vulnerabilities in dependencies (or only accepted exceptions).
- **Enforces:** Dependency hygiene; Dependabot alerts and/or CI security scan.
- **Implementation:** Use Dependabot (already configured); optionally add a CI step that runs `dotnet list package --vulnerable` or a security scanner and fails on high/critical. GitHub Advanced Security / CodeQL can also inform this.
- **Improvement sustained:** Safety nets (Dependabot, dependency scan).

### 3.7 Branch Protection (Recommended)

- **Rule:** Merges to `main` require (1) passing CI status check(s), (2) optional: PR review, (3) optional: up-to-date branch.
- **Enforces:** No merge when Build (or Build + Test + Format) has failed.
- **Implementation:** In GitHub: **Settings → Branches → Branch protection rules** for `main`: require status checks (e.g. “Build” or “build”), optionally “Require a pull request before merging” and “Require status checks to pass before merging.”
- **Improvement sustained:** Test automation plan “Branch Policy Suggestion”; all CI gates actually block merge.

---

## 4. Phased Rollout

### Phase 1 – Required (Now / Right After Test Fix)

| Gate | Action | Owner |
|------|--------|--------|
| Build | Keep as-is (already fails job on error) | — |
| Tests | Fix test/production sync ([lint-and-architecture-report](lint-and-architecture-report-20260211.md)); then set Test step `continue-on-error: false` | Dev |
| Branch protection | Enable for `main`: require “Build” (or the workflow name that runs build + test) | DevOps / maintainer |

**Exit criteria:** PRs cannot be merged if build or tests fail.

### Phase 2 – Recommended (Short Term)

| Gate | Action | Owner |
|------|--------|--------|
| Format | Run `dotnet format --verify-no-changes` locally; fix any differences; then set Format step `continue-on-error: false` | Dev |
| Branch protection | Add “Require pull request before merging” and “Require status checks to pass” if not already | DevOps |

**Exit criteria:** Format is enforced in CI; merge requires passing build + test + format.

### Phase 3 – Optional (When Team Agrees)

| Gate | Action | Owner |
|------|--------|--------|
| No new warnings | Clean existing warnings; set `TreatWarningsAsErrors` true or use `WarningsAsErrors` | Dev |
| Coverage | Add coverlet; publish coverage; optionally fail below threshold or on drop | Dev / DevOps |
| Dependencies | Add CI step or policy to fail on high/critical vulnerabilities | DevOps |

**Exit criteria:** Policy documented; gates enabled and visible in PRs.

---

## 5. Implementation Checklist

### 5.1 Workflow Changes (`.github/workflows/build.yml`)

- [ ] **Tests:** Remove `continue-on-error: true` from Test step once all tests pass.
- [ ] **Format:** Remove `continue-on-error: true` from Format check step when enforcing format.
- [ ] **Optional – Coverage:** Add coverlet to `PostHubAPI.Tests.csproj`; add step to run test with coverage and upload artifact (and optionally fail on threshold).
- [ ] **Optional – Warnings:** Ensure build uses Release; consider `TreatWarningsAsErrors` in csproj after cleaning warnings.

### 5.2 Branch Protection (GitHub Settings)

- [ ] Add or edit rule for branch `main`.
- [ ] Enable “Require status checks to pass before merging.”
- [ ] Select the status check that corresponds to the workflow job (e.g. “Build” or “build”).
- [ ] Optionally: “Require a pull request before merging,” “Require branches to be up to date before merging.”

### 5.3 Documentation

- [ ] Document in README or CONTRIBUTING: “Before pushing, run `dotnet build` and `dotnet test`; run `dotnet format` if format gate is enforced.”
- [ ] Document which gates are required vs optional and where they are configured (this file + workflow + branch protection).

---

## 6. Decision Table (When to Enforce)

| Gate | Enforce when |
|------|----------------|
| Build | Always (already) |
| Tests | As soon as test/production sync is done and `dotnet test` is green |
| Format | When repo is formatted once and team agrees to keep it that way |
| No new warnings | When current build has zero (or only allowed) warnings |
| Coverage | When coverage pipeline is in place and team agrees on threshold |
| Dependencies | When scanner is in place and team agrees to block on high/critical |
| Branch protection | When CI is stable; required for gates to actually block merge |

---

## 7. Related Documents

- [Test Automation Plan](test-automation-plan.md) – test strategy, CI, coverage goals, Phase 1–3.
- [Tests Gap Analysis](tests-gap-analysis-20260211.md) – scope of testable components.
- [Safety Nets Gap Analysis](safety-nets-gap-analysis-20260211.md) – CI, static analysis, format.
- [Lint and Architecture Report](lint-and-architecture-report-20260211.md) – test/production sync, XML docs, naming.
- [Safety Nets Implementation](safety-nets-implementation-20260211.md) – what was implemented (workflow, analyzers, etc.).

---

## 8. Summary

| Priority | Gate | Enforces | Implementation |
|----------|------|----------|----------------|
| **Required** | Build | Compiles | Already in workflow |
| **Required** | Tests | All tests pass | Remove `continue-on-error` on Test step after test sync |
| **Required** | Branch protection | CI must pass to merge | GitHub branch protection for `main` |
| **Recommended** | Format | Style per .editorconfig | Remove `continue-on-error` on Format step |
| **Optional** | No new warnings | Analyzer/compiler hygiene | `TreatWarningsAsErrors` or allow-list |
| **Optional** | Coverage | Coverage goal / no drop | Coverlet + optional fail on threshold |
| **Optional** | Dependencies | No high/critical vulns | Dependabot + optional CI scan |

Implementing **Required** and **Recommended** gates will enforce the main improvements (tests, format, CI) and make branch protection meaningful. **Optional** gates can be added when the team is ready and tooling is in place.
