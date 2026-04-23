---
name: openspec-check
description: Verify spec alignment before implementation. Use this when starting work on a feature to check OpenSpec specs and ensure implementation aligns with defined requirements.
---

## OpenSpec Spec Check

This project uses Spec-Driven Development (SDD). Before implementing code changes, verify alignment with existing specs.

### Workflow

1. **Check specs** in `/openspec/specs/` for relevant requirements:
   - `architecture/` - System design decisions
   - `data/` - Data models and storage
   - `export/` - Export functionality specs
   - `hardware/` - Hardware monitoring specs
   - `testing/` - Testing requirements
   - `features/` - Feature-specific specs

2. **Verify alignment**:
   - Does the proposed change conflict with existing specs?
   - Are there spec updates needed before implementing?
   - Is this covered by an existing spec or needs a new one?

3. **Report findings**:
   - List relevant specs found
   - Note any conflicts or gaps
   - Suggest spec updates if needed

### Commands

```bash
# List all spec files
find openspec/specs -name "*.md" -type f

# Search specs for keywords
grep -r "keyword" openspec/specs/
```

### Spec Format

Each spec follows this structure:
- **Status**: draft | approved | implemented | archived
- **Context**: Why this spec exists
- **Decision**: What was decided
- **Consequences**: Impact and tradeoffs
