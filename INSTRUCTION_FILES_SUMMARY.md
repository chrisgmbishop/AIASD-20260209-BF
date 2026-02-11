# PostHubAPI Instruction Files - Summary of Changes

**Date**: February 11, 2026
**Project**: PostHubAPI (.NET 8+ Layered Architecture)

## Overview

I've revised and created instruction files specifically tailored to the PostHubAPI codebase. The PostHubAPI is a .NET layered architecture project (Controllers → Services → Data), not a vertical slice architecture project.

## New Files Created

### 1. **posthub-architecture.instructions.md** ⭐ START HERE
   - **Purpose**: Complete architecture guide for PostHubAPI
   - **Coverage**:
     - Three-layer architecture overview with diagrams
     - Project structure and folder organization
     - Layer responsibilities and patterns
     - Controller implementation guidelines
     - Service patterns and initialization
     - DTO standards and naming conventions
     - Entity model design rules
     - Data access patterns with LINQ
     - Error handling strategies
     - Code generation rules
     - Quality checklist for reviews

### 2. **dotnet-coding-standards.instructions.md** ⭐ CODE STYLE GUIDE
   - **Purpose**: C# coding standards for this project
   - **Coverage**:
     - Naming conventions (PascalCase, camelCase, UPPER_SNAKE_CASE)
     - Async/await patterns (critical for ASP.NET Core)
     - Type safety and nullability reference types
     - Exception handling patterns
     - LINQ guidelines with performance notes
     - Dependency injection patterns
     - XML documentation standards
     - Code style and formatting rules
     - Performance considerations
     - Security best practices

### 3. **posthub-dependency-guide.instructions.md**
   - **Purpose**: NuGet package management for PostHubAPI
   - **Coverage**:
     - Current production and dev dependencies
     - Dependency selection criteria (MUST/SHOULD have)
     - Version management strategy
     - Approved packages table
     - Procedures for adding new dependencies
     - Security scanning with `dotnet` CLI
     - Problematic packages to avoid
     - Update procedures (minor vs. major)
     - Conflict resolution
     - CI/CD integration examples
     - Emergency vulnerability procedures

### 4. **posthub-ai-guidelines.instructions.md** ⭐ FOR AI ASSISTANTS
   - **Purpose**: Guide for AI code generation (Copilot, Claude, ChatGPT)
   - **Coverage**:
     - 8 critical anti-patterns with examples
     - Quick reference CRUD patterns
     - Common coding mistakes
     - Authentication context patterns
     - Validation patterns
     - When-in-doubt checklist

## Files Modified

### 1. **vertical-slice.instructions.md**
   - **Change**: Added disclaimer at top clarifying this is NOT for PostHubAPI
   - **Links to**: PostHubAPI Architecture Guide and .NET Coding Standards
   - **Note**: Kept intact for other potential projects using vertical slices

### 2. **cqrs-architecture.instructions.md**
   - **Change**: Added disclaimer at top clarifying this is NOT for PostHubAPI
   - **Links to**: PostHubAPI Architecture Guide
   - **Note**: Kept intact for reference on when CQRS might be useful

### 3. **business-rules-to-slices.instructions.md**
   - **Change**: Added disclaimer at top clarifying vertical slices not used in PostHubAPI
   - **Links to**: PostHubAPI Architecture Guide
   - **Note**: Kept intact as reference material

### 4. **README.md** (in instructions folder)
   - **Change**: Reorganized to separate PostHubAPI-specific from generic instructions
   - **Added**: New section for PostHubAPI Project-Specific Instructions
   - **Updated**: Purpose section to explain two categories of guidance

## Key Architectural Decisions Documented

### PostHubAPI Architecture Pattern
✅ **Layered Architecture** (3 layers):
- Controllers (HTTP API layer)
- Services (Business logic)
- Data (Entity Framework + DbContext)

❌ **NOT Used**:
- Vertical Slices (kept docs as reference)
- CQRS (kept docs as reference)
- Microservices

### Critical Development Patterns

1. **Async-First**: ALL I/O operations must use `async/await`
2. **DTO Boundary**: Never return domain models to clients
3. **Interface Injection**: Services expose interfaces for testability
4. **Error Handling**: Map domain exceptions to HTTP status codes
5. **Input Validation**: Validate at DTO level and service level
6. **Type Safety**: Use nullable reference types (C# 8+)

## 8 Technical Debt Issues Documented

All documented in the instruction files with examples:

1. ❌ Async/await mismatches in controllers
2. ❌ Missing null checks
3. ❌ Configuration key typos
4. ❌ Inconsistent error handling
5. ❌ Missing validation attributes
6. ❌ Missing logging implementation
7. ❌ Incomplete input validation
8. ❌ N+1 query problems

## Quick Reference Card

### For Developers
1. Start with: [posthub-architecture.instructions.md](posthub-architecture.instructions.md)
2. Reference: [dotnet-coding-standards.instructions.md](dotnet-coding-standards.instructions.md)
3. For deps: [posthub-dependency-guide.instructions.md](posthub-dependency-guide.instructions.md)

### For AI Assistants
1. Start with: [posthub-ai-guidelines.instructions.md](posthub-ai-guidelines.instructions.md)
2. Reference: [posthub-architecture.instructions.md](posthub-architecture.instructions.md)
3. For code: [dotnet-coding-standards.instructions.md](dotnet-coding-standards.instructions.md)

## Implementation Guidelines

All new code for PostHubAPI should:

✅ **DO**:
- Use async/await consistently
- Create DTOs for all endpoints
- Implement service interfaces
- Include error handling in controllers
- Add validation attributes to DTOs
- Validate in services too
- Include null checks
- Use proper HTTP status codes
- Add UpdatedAt timestamps
- Inject dependencies via constructors

❌ **DON'T**:
- Use `.Result` or `.Wait()` on async methods
- Return domain models from endpoints
- Mix sync and async (choose one)
- Catch and ignore exceptions silently
- Write business logic in controllers
- Create large, slow database queries
- Forget to map entities to DTOs
- Use hardcoded dependencies
- Ignore null reference possibilities
- Call methods inside LINQ queries

## Documentation Standards

Each file includes:

- **Front Matter**: AI provenance metadata (model, operator, timestamps, chat log)
- **Clear Sections**: Organized with table of contents
- **Code Examples**: Both ✅ correct and ❌ wrong patterns
- **Patterns**: Quick-reference implementations
- **Checklists**: Verification lists for code reviews

## Relationship Between Files

```
README.md (in .github/instructions/)
├── PostHubAPI-Specific Guidance
│   ├── posthub-architecture.instructions.md (READ FIRST)
│   ├── dotnet-coding-standards.instructions.md (REF)
│   ├── posthub-dependency-guide.instructions.md (REF)
│   └── posthub-ai-guidelines.instructions.md (FOR AI)
│
└── Generic/Ecosystem Guidance
    ├── ai-assisted-output.instructions.md
    ├── dependency-management-policy.instructions.md
    ├── vertical-slice.instructions.md [NOT USED FOR PostHubAPI]
    ├── cqrs-architecture.instructions.md [NOT USED FOR PostHubAPI]
    ├── business-rules-to-slices.instructions.md [NOT USED]
    └── [Others...]
```

## Files Available in `.github/instructions/`

| File | Type | Purpose |
|------|------|---------|
| `posthub-architecture.instructions.md` | PostHubAPI-Specific | Architecture & patterns |
| `dotnet-coding-standards.instructions.md` | PostHubAPI-Specific | Code standards & style |
| `posthub-dependency-guide.instructions.md` | PostHubAPI-Specific | NuGet management |
| `posthub-ai-guidelines.instructions.md` | PostHubAPI-Specific | AI code generation |
| `README.md` | Updated | Navigation & guidance |
| `vertical-slice.instructions.md` | Updated | Marked as not for PostHubAPI |
| `cqrs-architecture.instructions.md` | Updated | Marked as not for PostHubAPI |
| `business-rules-to-slices.instructions.md` | Updated | Marked as not for PostHubAPI |

## Next Steps

1. **Commit these changes** to preserve the instruction updates
2. **Run code quality checks** on existing code to find technical debt
3. **Generate GitHub issues** for the 8 technical debt items identified
4. **Reference these files** in code reviews
5. **Link to guidelines** in PR templates

---

**This completes the instruction file revision for PostHubAPI.**

The codebase now has clear, specific guidance for:
- ✅ Developers writing new code
- ✅ AI assistants generating code
- ✅ Code reviewers checking quality
- ✅ Maintainers managing dependencies
