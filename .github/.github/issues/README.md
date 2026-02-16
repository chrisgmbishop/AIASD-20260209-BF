# Web Calculator - GitHub Issues

This directory contains individual GitHub issue files for all 30 vertical slices of the Web Calculator project.

## Directory Structure

```
.github/issues/
├── README.md                          # This file
├── web-calculator-slices.md          # Consolidated issue document
├── create-issues.ps1                 # PowerShell automation script
└── slices/                           # Individual slice issue files
    ├── VS-01.md through VS-08.md     # Foundation slices (MVP)
    ├── VS-09.md through VS-16.md     # Enhancement slices (V1.1-V2.0)
    ├── VS-17.md through VS-26.md     # Progressive slices (V2.0-V3.0)
    └── VS-27.md through VS-30.md     # Advanced slices (V3.0)
```

## Quick Start

### Option 1: Automated Creation (Recommended)

Use the provided PowerShell script to create all issues at once:

```powershell
# Navigate to repository root
cd c:\git\AIASD\AIASD-20260209

# Run dry-run first to preview
.\.github\issues\create-issues.ps1 -DryRun

# Create all issues
.\.github\issues\create-issues.ps1

# Create specific range (e.g., MVP slices only)
.\.github\issues\create-issues.ps1 -StartSlice 1 -EndSlice 8
```

**Prerequisites:**

1. Install [GitHub CLI](https://cli.github.com/)
2. Authenticate: `gh auth login`
3. Verify access to repository

### Option 2: Manual Creation

For each slice file in `slices/`:

1. Open the file (e.g., `VS-01.md`)
2. Copy the content after the `---` frontmatter
3. Create new issue in GitHub UI
4. Paste content as issue body
5. Add labels and milestone from frontmatter

### Option 3: GitHub CLI (One at a time)

```bash
# Example for VS-01
gh issue create \
  --repo johnmillerATcodemag-com/AIASD-20260209 \
  --title "VS-01: Display Current Value (Query Slice - Foundation)" \
  --body-file .github/issues/slices/VS-01.md \
  --label "slice:foundation,priority:P0,type:query,phase:mvp" \
  --milestone "MVP - Foundation"
```

## Issue Metadata

Each issue file contains YAML frontmatter with:

- `title`: Issue title
- `labels`: Array of labels to apply
- `assignees`: Team members to assign (optional)
- `milestone`: Project milestone

### Label Taxonomy

**Slice Categories:**

- `slice:foundation` - Core MVP functionality (VS-01 to VS-08)
- `slice:enhancement` - Post-MVP improvements (VS-09 to VS-16)
- `slice:progressive` - Advanced features (VS-17 to VS-26)
- `slice:advanced` - Specialized calculators (VS-27 to VS-30)

**Priority Levels:**

- `priority:P0` - Critical path
- `priority:high` - Important for MVP
- `priority:medium` - Nice to have
- `priority:low` - Future enhancements

**Type Indicators:**

- `type:query` - Read-only operations
- `type:command` - State-modifying operations
- `type:feature` - New capabilities
- `type:cross-cutting` - Affects multiple slices

**Risk Assessment:**

- `risk:low` - Straightforward implementation
- `risk:medium` - Some complexity
- `risk:high` - Significant technical challenges

**Development Phases:**

- `phase:mvp` - Initial release
- `phase:v1.1` - First update
- `phase:v2.0` - Major update
- `phase:v3.0` - Advanced features

### Milestones

Create these milestones in GitHub before importing issues:

- **MVP - Foundation** - Core calculator functionality (VS-01 to VS-08)
- **V1.1 - Enhancements** - History, memory, clipboard (VS-09 to VS-16)
- **V2.0 - Advanced Features** - PWA, themes, converters (VS-17 to VS-24)
- **V3.0 - Specialized Calculators** - Scientific, statistics, etc. (VS-25 to VS-30)

## Issue Content Structure

Each issue includes:

1. **Overview Section**
   - Slice type, priority, effort estimate, risk level
   - Dependencies on other slices

2. **User Story** (where applicable)
   - Clear description of user need

3. **Acceptance Criteria**
   - Checklist of requirements for completion

4. **Technical Requirements**
   - Code examples and specifications
   - State management details
   - UI component details

5. **Test Requirements**
   - Unit test requirements
   - Integration test scenarios
   - Accessibility testing needs

6. **Implementation Checklist**
   - Step-by-step tasks for completion

## PowerShell Script Options

```powershell
# Script parameters
-Owner          # GitHub repository owner (default: johnmillerATcodemag-com)
-Repo           # Repository name (default: AIASD-20260209)
-SlicesDir      # Path to slices directory (default: .github/issues/slices)
-StartSlice     # First slice number to create (default: 1)
-EndSlice       # Last slice number to create (default: 30)
-DryRun         # Preview without creating (default: false)

# Examples
.\.github\issues\create-issues.ps1 -DryRun                    # Preview all
.\.github\issues\create-issues.ps1 -StartSlice 1 -EndSlice 8  # MVP only
.\.github\issues\create-issues.ps1 -StartSlice 9 -EndSlice 16 # Enhancements
```

## Customization

### Before Creating Issues

1. **Review Labels**: Ensure labels exist in your repository
2. **Create Milestones**: Set up all milestones as listed above
3. **Adjust Estimates**: Modify effort estimates based on team capacity
4. **Update Dependencies**: Adjust slice dependencies if needed
5. **Assign Team Members**: Add assignees in frontmatter if desired

### Modifying Issue Files

Each `.md` file follows this structure:

```markdown
---
title: "Issue Title"
labels: ["label1", "label2"]
assignees: []
milestone: "Milestone Name"
---

## Overview

[content...]

## Acceptance Criteria

[content...]
```

## Troubleshooting

### GitHub CLI Issues

**Error: `gh: command not found`**

- Install from: https://cli.github.com/
- Restart terminal after installation

**Error: `authentication required`**

- Run: `gh auth login`
- Follow authentication prompts

**Error: `milestone not found`**

- Create milestones in GitHub first
- Or remove milestone from command

### Script Issues

**Error: Slices directory not found**

- Ensure you're in repository root
- Check SlicesDir parameter path

**Rate Limiting**

- Script includes 500ms pause between issues
- For large batches, consider creating in smaller groups

## Project Management

### Recommended Workflow

1. **Week 1**: Create MVP foundation issues (VS-01 to VS-08)
2. **Weekly**: Track progress in GitHub Projects
3. **Sprint Planning**: Use slice dependencies for sprint ordering
4. **Code Review**: Reference issue in PR description

### GitHub Projects Setup

1. Create Project: "Web Calculator Development"
2. Add Views:
   - **Kanban**: Group by Status (Backlog, In Progress, Done)
   - **Priority**: Sort by priority label
   - **Phase**: Group by milestone
   - **Dependencies**: Custom view showing blocked issues

### Linking Issues

In code and PRs, reference issues:

```
Implements #123 (VS-01: Display Current Value)
Depends on #124 (VS-02: Input Digit)
```

## Support

For issues with:

- **Issue Content**: Review vertical slice plan in `requirements/`
- **Script Problems**: Check PowerShell execution policy
- **GitHub API**: Consult GitHub CLI documentation

## License

These issue templates are part of the Web Calculator project and follow the same license as the repository.

---

**Last Updated**: 2026-02-12
**Version**: 1.0.0
**Total Slices**: 30
**Estimated Total Effort**: 280+ hours
