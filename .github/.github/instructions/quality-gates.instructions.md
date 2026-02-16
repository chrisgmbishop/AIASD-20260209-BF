---
applyTo: "**"
---

# Quality Gates and Release Criteria

## 1. Overview

This document outlines the quality gates that must be passed before code can be merged into the main branch and deployed to production. These gates ensure that all changes meet our standards for quality, stability, security, and performance. Adherence to these criteria is mandatory for all contributions.

## 2. Purpose

The purpose of these quality gates is to:

- Prevent regressions and reduce the number of bugs in production.
- Ensure a consistent level of quality across the codebase.
- Automate quality checks to provide fast feedback to developers.
- Clarify the definition of "done" for a feature or bug fix.
- Mitigate risks associated with deploying new code.

## 3. Pull Request (PR) Quality Gates

Before a PR can be merged, it must meet the following criteria. These checks are automated via our Continuous Integration (CI) pipeline.

### 3.1. Code Quality and Style

- **[ ] Linting and Static Analysis**: The code must pass all linting rules without any errors. This ensures consistency and helps catch common mistakes.
  - **Tools**: ESLint, Prettier, Stylelint.
- **[ ] Code Complexity**: The complexity of the code should not exceed a predefined threshold.
  - **Tools**: SonarCloud, Code Climate.

### 3.2. Testing and Coverage

- **[ ] All Tests Pass**: All unit, integration, and component tests must pass.
  - **Tools**: Jest, Cypress, React Testing Library.
- **[ ] Code Coverage Threshold**: The code coverage must meet or exceed the project's defined threshold (e.g., 80% for new code). A decrease in overall coverage is not permitted.
  - **Tools**: Jest Coverage, Coveralls.
- **[ ] E2E Tests Pass**: All end-to-end tests must pass on a staging environment that mirrors production.
  - **Tools**: Cypress, Playwright.

### 3.3. Security

- **[ ] No Vulnerable Dependencies**: The project's dependencies must be scanned for known vulnerabilities. No new high or critical severity vulnerabilities are allowed.
  - **Tools**: Snyk, `npm audit`.
- **[ ] No Hardcoded Secrets**: The code must be scanned for hardcoded secrets (API keys, passwords, etc.).
  - **Tools**: TruffleHog, Git-secrets.
- **[ ] Static Application Security Testing (SAST)**: The code must be scanned for common security vulnerabilities (e.g., SQL injection, XSS).
  - **Tools**: SonarQube, Snyk Code.

### 3.4. Accessibility

- **[ ] Automated Accessibility Checks Pass**: All pages and components must pass automated accessibility checks. No new `critical` or `serious` violations are allowed.
  - **Tools**: axe-core, Cypress-axe.

### 3.5. Performance

- **[ ] Performance Budgets Met**: Key pages must be tested against performance budgets (e.g., Lighthouse scores, load times, bundle sizes).
  - **Tools**: Lighthouse CI, WebPageTest.
- **[ ] No Performance Regressions**: Automated tests should detect and flag any significant performance regressions.

### 3.6. Peer Review

- **[ ] At Least One Approving Review**: Every PR must be reviewed and approved by at least one other team member. For critical changes, two approvals may be required.
- **[ ] All Comments Resolved**: All comments and feedback from the review must be addressed and resolved before merging.

## 4. Release Criteria

Before a new version of the application can be deployed to production, the following criteria must be met.

### 4.1. Staging Environment Verification

- **[ ] Successful Deployment to Staging**: The release candidate must be successfully deployed to a staging environment that is a clone of production.
- **[ ] Manual QA Sign-off**: A QA engineer must perform exploratory testing on the staging environment and sign off on the release. This includes:
  - Verifying the new features work as expected.
  - Running through a regression test suite to ensure existing functionality is not broken.
  - Validating that all critical user journeys are functional.
- **[ ] No Blocking Bugs**: There must be no open bugs marked as `blocker` or `critical` for the release.

### 4.2. Documentation and Communication

- **[ ] Release Notes Prepared**: Clear and concise release notes must be written, detailing the changes, new features, and bug fixes in the release.
- **[ ] Stakeholders Notified**: All relevant stakeholders (Product, Marketing, Support) must be notified of the upcoming release and any impact it may have.

### 4.3. Rollback Plan

- **[ ] A Rollback Plan is in Place**: A clear and tested plan must exist to quickly roll back the deployment in case of a critical failure. This could involve redeploying the previous version or using feature flags to disable the problematic feature.

## 5. Definition of "Done"

A user story, feature, or bug fix is considered "done" only when it meets all the following criteria:

- The code is complete and implements the required functionality.
- All relevant tests (unit, integration, E2E) have been written and are passing.
- The code has been peer-reviewed and approved.
- All CI checks and quality gates have passed.
- The changes have been deployed to a staging environment and verified by QA.
- Any necessary documentation has been updated.

By enforcing these quality gates and release criteria, we can maintain a high standard of quality, reduce risks, and deliver a reliable and robust product to our users.
