---
provenance:
  created: "2026-02-11"
  created_by: "AI-assisted (Cursor)"
  source: "GitHub CLI commands for issue/PR workflow"
  version: "1.0"
---

# Create issue and PR for unit test failures

Run these steps from the repo root **after** authenticating with GitHub CLI (`gh auth login` if needed).

---

## 1. Create the issue

```bash
gh issue create \
  --title "Unit tests failing when running dotnet test" \
  --body-file .github/issue-unit-tests-failing.md \
  --label "bug,testing"
```

If the repo has no `testing` label, use only `bug` or add the label first in the GitHub UI.

**Optional:** Before running, edit `.github/issue-unit-tests-failing.md` and paste your actual `dotnet test` output into the "Actual result" section so the issue contains the failure details.

---

## 2. Create branch for the fix

```bash
git checkout main
git pull origin main
git checkout -b fix/unit-tests
```

---

## 3. Push the branch (after you have commits to push)

When you have local commits (e.g. after applying fixes):

```bash
git push -u origin fix/unit-tests
```

---

## 4. Create the pull request

Replace `N` with the issue number from step 1 (e.g. `Fixes #3`). You can edit `.github/pr-fix-unit-tests.md` to add "Fixes #N" at the top of the body, then:

```bash
gh pr create \
  --base main \
  --head fix/unit-tests \
  --title "Fix failing unit tests" \
  --body-file .github/pr-fix-unit-tests.md
```

To link the PR to the issue when creating:

```bash
gh pr create \
  --base main \
  --head fix/unit-tests \
  --title "Fix failing unit tests" \
  --body "Fixes #N

$(cat .github/pr-fix-unit-tests.md)"
```

(Replace `N` with the actual issue number.)

---

## One-time: authenticate GitHub CLI

If `gh auth status` fails:

```bash
gh auth login -h github.com -p https -w
```

Follow the prompts to complete login.
