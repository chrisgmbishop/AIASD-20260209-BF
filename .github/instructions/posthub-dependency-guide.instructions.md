---
ai_generated: true
model: "anthropic/claude-3.5-sonnet@2024-10-22"
operator: "GitHub Copilot"
chat_id: "posthub-dependency-guide-20260211"
prompt: |
  Create a PostHubAPI-specific dependency management guide covering NuGet packages,
  version management, and dependency security specific to this project.
started: "2026-02-11T00:00:00Z"
ended: "2026-02-11T00:30:00Z"
task_durations:
  - task: "dependency analysis and documentation"
    duration: "00:30:00"
total_duration: "00:30:00"
ai_log: "ai-logs/2026/02/11/posthub-dependency-guide-20260211/conversation.md"
source: "GitHub Copilot"
applyTo: "**/*"
---

# PostHubAPI Dependency Management Guide

## Overview

This guide covers NuGet package management, dependency security, and version control for the PostHubAPI project.

**Target Audience**: Developers managing PostHubAPI dependencies
**Scope**: NuGet package selection, version pinning, security scanning, and update procedures
**Framework**: .NET 8+

## Current Dependencies

### Production Dependencies

```xml
<!-- Microsoft.AspNetCore.App -->
App hosting and identity framework - included with .NET SDK

<!-- Microsoft.EntityFrameworkCore -->
Latest stable version - ORM for database access

<!-- Microsoft.EntityFrameworkCore.SqlServer -->
SQL Server provider for EF Core

<!-- AutoMapper -->
Object-to-object mapping for DTOs
```

### Development Dependencies

```xml
<!-- xUnit -->
Unit testing framework

<!-- xUnit.runner.visualstudio -->
Visual Studio test runner integration

<!-- Moq -->
Mocking library for unit tests
```

## Dependency Selection Criteria

### MUST HAVE Requirements

- ✅ Active maintenance (commits within last 3 months)
- ✅ NuGet.org availability with 100+ downloads/month minimum
- ✅ MIT, Apache 2.0, or BSD license
- ✅ No known critical CVEs
- ✅ .NET 8+ compatibility

### SHOULD HAVE Requirements

- Strong community adoption (10k+ downloads/month)
- Responsive issue resolution
- Good documentation
- Stable API (backward compatible)

## Version Management Strategy

### Version Pinning Policy

```xml
<!-- appsettings.json configuration-driven, but project file versions: -->

<!-- Critical Infrastructure: Exact versions -->
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />

<!-- Framework Libraries: Minor version floating -->
<PackageReference Include="AutoMapper" Version="13.0.*" />

<!-- Testing: Can be more flexible -->
<PackageReference Include="xUnit" Version="2.6.*" />
```

### Update Schedule

**Security Patches**: Apply within 24-48 hours
**Minor Updates**: Apply monthly during maintenance window
**Major Updates**: Plan quarterly with extended testing

## Approved NuGet Packages

### Core Framework (Do NOT change)

| Package                                 | Version | Purpose              | License      |
| --------------------------------------- | ------- | -------------------- | ------------ |
| Microsoft.EntityFrameworkCore           | 8.0+    | ORM                  | Apache 2.0   |
| Microsoft.EntityFrameworkCore.SqlServer | 8.0+    | SQL Server Provider  | Apache 2.0   |
| Microsoft.AspNetCore.Identity           | 8.0+    | Authentication       | Apache 2.0   |
| AutoMapper                              | 13.0+   | DTO Mapping          | MIT          |

### Testing (Can update)

| Package    | Version | License |
| ---------- | ------- | ------- |
| xUnit      | 2.6+    | Apache  |
| Moq        | 4.20+   | BSD     |

## Adding New Dependencies

### Before Adding

1. **✅ Check if functionality exists** in current packages
2. **✅ Evaluate alternatives** against selection criteria
3. **✅ Request code review** before adding
4. **✅ Run security scan** after adding

### New Package Submission

```markdown
## Add New Package Proposal

**Package Name**: [name]
**Current Version**: [version]
**Purpose**: [why needed]
**Alternatives Considered**: [list others]
**Size Impact**: [bytes added to build]
**Security**: [any known issues]
**License**: [license type]
**Adoption**: [downloads/month]

Proceed? (y/n)
```

## Security Scanning

### Local Vulnerability Check

```bash
# Scan current project
dotnet package search --outdated

# Check for vulnerabilities
dotnet list package --vulnerable

# Generate SBOM
dotnet sbom create --output sbom.json
```

### GitHub Integration

- Enable Dependabot alerts
- Configure pull request updates
- Review security warnings weekly

## Problematic Packages to Avoid

### ❌ Deprecated/Unmaintained

- `Newtonsoft.Json` (use `System.Text.Json` instead)
- `AutoFac` (use built-in DI)
- Old Entity Framework (< v6)

### ❌ Licensing Issues

- GPL-licensed packages
- Proprietary/commercial without approval

### ❌ Security Risks

- Packages with unpatched CVEs
- Packages from untrusted sources
- Packages with irregular update patterns

## Update Procedures

### Minor Version Update

```bash
# Check for updates
dotnet package list --outdated

# Update specific package
dotnet package update AutoMapper

# Run all tests
dotnet test

# Commit change
git commit -m "chore(deps): update AutoMapper to 13.0.1"
```

### Major Version Update

**Requires extended testing**:

1. Update in development environment
2. Run complete test suite (no failures)
3. Integration testing
4. Deploy to staging
5. Regression testing (48 hours)
6. Code review
7. Production deployment

## Conflict Resolution

### Version Conflicts

**Common**: Transitive dependencies requiring different versions

**Resolution**:
1. Update parent packages first
2. Check for pre-release versions
3. Consider package alternatives
4. Document workaround if needed

### License Conflicts

**If discovered**: Stop immediately and escalate to team lead

## CI/CD Integration

### Automated Security Checks

```yaml
# .github/workflows/security.yml
- name: Check Nuget Vulnerabilities
  run: dotnet list package --vulnerable --include-transitive

- name: Check License Compliance
  run: dotnet package verify licenses
```

## Best Practices

### ✅ DO

- Keep dependencies to minimum necessary
- Update regularly but deliberately
- Document why each dependency is needed
- Pin major versions in production
- Run security scans regularly
- Review transitive dependencies

### ❌ DON'T

- Add dependencies without evaluation
- Use experimental packages in production
- Ignore security warnings
- Pin exact versions unnecessarily
- Use deprecated packages
- Add conflicting dependencies

## Emergency Procedures

### Critical Security Vulnerability

1. **Immediate**: Identify affected package and version
2. **Assess**: Determine if project is vulnerable
3. **Plan**: Check if patch available
4. **Implement**: Apply patch or workaround
5. **Test**: Verify fix effectiveness
6. **Deploy**: Fast-track to production
7. **Document**: Post-incident review

---

**Document Version**: 1.0.0
**Last Updated**: 2026-02-11
**Maintained By**: Development Team
