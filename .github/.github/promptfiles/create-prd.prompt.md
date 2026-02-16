---
name: Create Product Requirements Document (PRD)
description: Generate a comprehensive Product Requirements Document for a feature or initiative with problem definition, solution approach, success metrics, and implementation considerations.
author: product-manager-agent
version: 1.0.0
tags: [product-management, requirements, documentation, prd]
---

# Create Product Requirements Document (PRD)

## Purpose

Generate a comprehensive Product Requirements Document that clearly defines the problem, proposed solution, success metrics, user impact, and implementation considerations for a feature or product initiative.

## Inputs

- **Feature/Initiative Name**: The name or title of the feature/product being specified
- **Problem Statement**: Description of the user problem or business opportunity
- **Target Users**: Who will use this feature (personas, roles, segments)
- **Business Goals**: What business objectives this addresses
- **Current Context**: Existing functionality, technical constraints, or related features (optional)
- **Scope Boundaries**: What's in scope and out of scope (optional)

## Task

Create a structured Product Requirements Document that includes:

1. **Executive Summary**: 2-3 paragraph overview of the feature and its value
2. **Problem Definition**: Clear articulation of the problem being solved
3. **User Personas & Use Cases**: Who uses this and how
4. **Proposed Solution**: High-level approach and key capabilities
5. **User Stories**: Detailed user stories with acceptance criteria
6. **Success Metrics**: Measurable KPIs and success criteria
7. **Technical Considerations**: Architecture, dependencies, constraints
8. **Design Considerations**: UX/UI requirements and principles
9. **Risks & Mitigation**: Potential risks and how to address them
10. **Timeline & Milestones**: Phased approach and key dates
11. **Open Questions**: Unresolved items requiring further investigation

## Output Format

```markdown
# Product Requirements Document: [Feature Name]

**Version**: 1.0
**Created**: [Date]
**Owner**: [Product Manager/Team]
**Status**: [Draft | In Review | Approved]
**Last Updated**: [Date]

---

## Executive Summary

[2-3 paragraphs summarizing the feature, the problem it solves, and the expected business impact]

---

## 1. Problem Statement

### Background
[Context about why this problem exists and why it matters now]

### Current State
[Description of how users currently solve this problem or workaround limitations]

### Pain Points
- [Specific pain point 1]
- [Specific pain point 2]
- [Specific pain point 3]

### Opportunity
[What becomes possible if we solve this problem]

---

## 2. User Personas & Target Audience

### Primary Persona: [Persona Name]
- **Role**: [Job title/role]
- **Goals**: [What they're trying to achieve]
- **Pain Points**: [Current challenges]
- **Tech Savviness**: [Beginner | Intermediate | Advanced]

### Secondary Personas
[Brief description of other affected user types]

### User Segmentation
- **Priority 1**: [High-priority user segment]
- **Priority 2**: [Secondary user segment]

---

## 3. Use Cases

### Use Case 1: [Use Case Title]
**Actor**: [User persona]
**Goal**: [What they want to accomplish]
**Scenario**: [Step-by-step narrative of how they use the feature]

### Use Case 2: [Use Case Title]
[Additional use cases...]

---

## 4. Proposed Solution

### Overview
[High-level description of the solution approach]

### Key Capabilities
1. **[Capability 1]**: [Description and value]
2. **[Capability 2]**: [Description and value]
3. **[Capability 3]**: [Description and value]

### User Experience Flow
[Narrative or diagram of the end-to-end user flow]

### Information Architecture
[Key screens, components, or data structures]

---

## 5. User Stories & Acceptance Criteria

### Epic: [Epic Name]

#### Story 1: [User Story Title]
**As a** [user persona]
**I want** [capability]
**So that** [benefit]

**Acceptance Criteria**:
- [ ] [Testable criterion 1]
- [ ] [Testable criterion 2]
- [ ] [Testable criterion 3]

**Priority**: [Critical | High | Medium | Low]
**Estimate**: [Story points or time estimate]

#### Story 2: [User Story Title]
[Additional stories...]

---

## 6. Success Metrics & KPIs

### Primary Metrics
- **[Metric 1]**: [Definition] → **Target**: [Specific goal]
- **[Metric 2]**: [Definition] → **Target**: [Specific goal]

### Secondary Metrics
- **[Metric 3]**: [Definition] → **Target**: [Specific goal]

### Success Criteria
- [ ] [Measurable success criterion 1]
- [ ] [Measurable success criterion 2]
- [ ] [Measurable success criterion 3]

### Measurement Plan
- **How**: [Method of measurement - analytics, surveys, etc.]
- **When**: [When to measure - weekly, post-launch, etc.]
- **Baseline**: [Current baseline if applicable]

---

## 7. Technical Considerations

### Architecture & Infrastructure
[High-level technical approach, systems involved, data flows]

### Dependencies
- **Internal**: [Other features, services, or teams]
- **External**: [Third-party services, APIs, integrations]

### Technical Constraints
- [Constraint 1 and impact]
- [Constraint 2 and impact]

### Security & Privacy
- [Security requirements]
- [Privacy considerations]
- [Compliance requirements - GDPR, HIPAA, etc.]

### Performance Requirements
- **Response Time**: [Target latency]
- **Throughput**: [Expected load]
- **Scalability**: [Growth expectations]

### Data Requirements
- [Data sources needed]
- [Data storage requirements]
- [Data retention policies]

---

## 8. Design & UX Considerations

### Design Principles
- [Principle 1: e.g., "Minimize cognitive load"]
- [Principle 2: e.g., "Progressive disclosure"]

### Accessibility Requirements
- [WCAG compliance level]
- [Specific accessibility needs]

### Responsive Design
- [Desktop, mobile, tablet requirements]

### Internationalization
- [Language support requirements]
- [Localization considerations]

### Visual Design Notes
[Key visual design guidance or references]

---

## 9. Risks & Mitigation Strategies

| Risk | Likelihood | Impact | Mitigation Strategy |
|------|------------|--------|---------------------|
| [Risk 1] | [H/M/L] | [H/M/L] | [How to mitigate] |
| [Risk 2] | [H/M/L] | [H/M/L] | [How to mitigate] |
| [Risk 3] | [H/M/L] | [H/M/L] | [How to mitigate] |

---

## 10. Timeline & Milestones

### Phased Rollout Approach

**Phase 1: MVP** (Target: [Date])
- [Core capability 1]
- [Core capability 2]
- **Success Criteria**: [What defines MVP success]

**Phase 2: Enhanced** (Target: [Date])
- [Enhanced capability 1]
- [Enhanced capability 2]

**Phase 3: Complete** (Target: [Date])
- [Additional capability 1]
- [Polish and optimization]

### Key Milestones
- **[Milestone 1]**: [Date] - [Deliverable]
- **[Milestone 2]**: [Date] - [Deliverable]
- **[Milestone 3]**: [Date] - [Deliverable]

---

## 11. Scope

### In Scope
- [Item 1 clearly in scope]
- [Item 2 clearly in scope]

### Out of Scope
- [Item 1 explicitly excluded]
- [Item 2 explicitly excluded]

### Future Considerations
- [Enhancement 1 for future releases]
- [Enhancement 2 for future releases]

---

## 12. Open Questions

1. **[Question 1]**
   - **Owner**: [Who will resolve]
   - **Due Date**: [When answer is needed]
   - **Status**: [Open | In Progress | Resolved]

2. **[Question 2]**
   - **Owner**: [Who will resolve]
   - **Due Date**: [When answer is needed]
   - **Status**: [Open | In Progress | Resolved]

---

## 13. Stakeholders & Approvals

### Stakeholders
- **Product Owner**: [Name]
- **Engineering Lead**: [Name]
- **Design Lead**: [Name]
- **QA Lead**: [Name]
- **Business Sponsor**: [Name]

### Approval Status
- [ ] Product Management: [Name] - [Date]
- [ ] Engineering: [Name] - [Date]
- [ ] Design: [Name] - [Date]
- [ ] QA: [Name] - [Date]
- [ ] Business: [Name] - [Date]

---

## 14. Appendices

### A. Research & Validation
[Links to user research, competitive analysis, market data]

### B. Related Documents
- [Link to technical design doc]
- [Link to user research findings]
- [Link to competitive analysis]

### C. References
- [External references, industry standards, best practices]

---

## Document Change Log

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | [Date] | [Author] | Initial draft |
