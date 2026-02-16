---
applyTo: "**"
---

# Web Application Testing Instructions

## 1. Overview

This document defines the standards, tools, and processes for testing web applications within this repository. The goal is to ensure high-quality, reliable, and performant software through a comprehensive and structured testing strategy. Adhering to these guidelines helps catch bugs early, prevent regressions, and deliver a better user experience.

## 2. Scope

These instructions apply to all web application development, including frontend, backend, and API components. All new features and bug fixes must be accompanied by appropriate tests as defined in this document.

## 3. Core Testing Principles

- **Test Early, Test Often**: Integrate testing into every stage of the development lifecycle.
- **Automate Everything Possible**: Automate repetitive tests to improve efficiency and coverage. Manual testing should be reserved for exploratory and usability testing.
- **Focus on User Journeys**: Prioritize tests that simulate real user workflows and deliver the most value.
- **Write Clear and Maintainable Tests**: Tests are code. They must be readable, maintainable, and well-documented.
- **Own Your Quality**: Developers are responsible for writing tests for their code. QA is responsible for verifying quality and performing exploratory testing.

## 4. The Testing Pyramid

We follow the testing pyramid model to balance our testing efforts effectively. The pyramid illustrates the recommended proportion of tests at different levels.

```
      /\\
     /  \\  <-- End-to-End Tests (Few)
    /----\\
   /      \\ <-- Integration Tests (More)
  /--------\\
 /          \\<- Unit Tests (Lots)
/____________\\
```

- **Unit Tests (Base)**: Form the largest portion of our tests. They are fast, isolated, and verify small pieces of functionality (e.g., a single function or component).
- **Integration Tests (Middle)**: Verify that multiple components or services work together as expected. They are slower than unit tests but faster than E2E tests.
- **End-to-End (E2E) Tests (Top)**: Simulate complete user workflows from the user's perspective. They are the slowest and most brittle but provide the highest confidence that the system works as a whole.

## 5. Types of Testing

### 5.1. Unit Testing

- **Purpose**: To test individual functions, methods, or components in isolation.
- **Tools**:
  - **Frontend**: [Jest](https://jestjs.io/), [React Testing Library](https://testing-library.com/docs/react-testing-library/intro/)
  - **Backend (Node.js)**: [Jest](https://jestjs.io/), [Mocha](https://mochajs.org/)/[Chai](https://www.chaijs.com/)
- **Requirement**: All new business logic must have unit test coverage.

### 5.2. Integration Testing

- **Purpose**: To verify interactions between different parts of the application (e.g., API calls, database interactions, component compositions).
- **Tools**:
  - **Frontend/Backend**: [Jest](https://jestjs.io/), [Supertest](https://github.com/visionmedia/supertest) (for API testing)
- **Requirement**: Key user workflows and service integrations must be covered by integration tests.

### 5.3. End-to-End (E2E) Testing

- **Purpose**: To validate complete user flows in a production-like environment.
- **Tools**: [Cypress](https://www.cypress.io/), [Playwright](https://playwright.dev/)
- **Requirement**: Critical user journeys (e.g., user registration, checkout, core feature usage) must have E2E test coverage.

### 5.4. Component Testing

- **Purpose**: To test UI components in isolation, verifying their appearance and behavior across different states.
- **Tools**: [Storybook](https://storybook.js.org/), [Cypress Component Testing](https://docs.cypress.io/guides/component-testing/overview)
- **Requirement**: All reusable UI components should be developed and tested in a component library environment.

### 5.5. Visual Regression Testing

- **Purpose**: To catch unintended UI changes by comparing screenshots of components or pages over time.
- **Tools**: [Percy](https://percy.io/), [Applitools](https://applitools.com/)
- **Requirement**: Key pages and components should have visual regression tests to prevent visual bugs.

### 5.6. Accessibility (a11y) Testing

- **Purpose**: To ensure the application is usable by people with disabilities.
- **Tools**: [axe-core](https://github.com/dequelabs/axe-core), [Cypress-axe](https://github.com/component-driven/cypress-axe)
- **Requirement**: Automated accessibility checks must be integrated into the CI pipeline. Manual audits should be performed periodically.

### 5.7. Performance Testing

- **Purpose**: To measure and ensure the application's responsiveness, speed, and scalability.
- **Tools**: [Lighthouse](https://developers.google.com/web/tools/lighthouse), [k6](https://k6.io/), [JMeter](https://jmeter.apache.org/)
- **Requirement**: Performance budgets should be defined for key pages, and automated checks should run in CI.

### 5.8. Security Testing

- **Purpose**: To identify and fix security vulnerabilities.
- **Tools**: [OWASP ZAP](https://www.zaproxy.org/), [Snyk](https://snyk.io/), [npm audit](https://docs.npmjs.com/cli/v7/commands/npm-audit)
- **Requirement**: Automated security scans for dependencies and code must be part of the CI pipeline.

## 6. Test Planning and Strategy

For every new feature or significant change, a test plan must be created as part of the feature's specification or PRD.

The test plan should answer:

1.  **What are we testing?** (Scope of the feature)
2.  **What are the user stories and acceptance criteria?**
3.  **What types of tests are needed?** (Unit, Integration, E2E, etc.)
4.  **What are the critical user journeys to cover?**
5.  **What are the edge cases and negative scenarios?**
6.  **What data is needed for testing?**
7.  **What are the performance and accessibility considerations?**

## 7. Writing Good Tests

- **Arrange, Act, Assert (AAA)**: Structure your tests clearly.
  - **Arrange**: Set up the initial state and inputs.
  - **Act**: Execute the function or action being tested.
  - **Assert**: Verify the outcome is as expected.
- **Be Descriptive**: Test names should clearly state what they are testing and what the expected outcome is.
- **Avoid Logic in Tests**: Tests should be simple and straightforward. Avoid loops, conditional logic, and complex setup.
- **One Assertion Per Test**: Ideally, each test should verify a single outcome.
- **Use Mocks and Stubs**: Isolate the code under test by mocking external dependencies (e.g., APIs, databases).

## 8. Continuous Integration (CI)

- All tests must run automatically in the CI pipeline on every pull request.
- A pull request cannot be merged if any tests are failing.
- The CI pipeline should be configured to run:
  - Linting and static analysis.
  - Unit tests.
  - Integration tests.
  - E2E tests (on a staging environment).
  - Security and dependency scans.
  - Accessibility checks.

## 9. Bug Reporting and Triage

- When a bug is found, create a detailed bug report in the project's issue tracker.
- The report must include:
  - A clear, descriptive title.
  - Steps to reproduce the bug.
  - Expected behavior vs. actual behavior.
  - Screenshots or videos.
  - Environment details (browser, OS).
  - Severity and priority.
- The team will hold regular triage meetings to review, prioritize, and assign new bug reports.
