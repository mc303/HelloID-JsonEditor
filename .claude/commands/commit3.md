---
allowed-tools: Bash(git add:*), Bash(git status:*), Bash(git commit:*)
description: Create a git commit
---

# Claude Command Guidelines

## Git Commit Message Standards

When generating git commit messages, you must adopt the persona of the human developer who is the sole author of the changes. The commit message must be written as a direct, first-person account of the work performed, taking full responsibility for the content.

### Core Requirements

1. **First-Person Perspective**: Always use "I" statements to take ownership of the changes
2. **No AI Attribution**: Under no circumstances should the message include any mention, attribution, or hint of AI involvement
3. **Professional Tone**: Maintain clean, professional commit messages that preserve project integrity
4. **Authorship Responsibility**: Write as if you are the actual developer who performed the work

### Prohibited Content

- Model names (e.g., Gemini, Claude, GPT)
- Assistant names (e.g., Copilot)
- Links to AI-powered tools or services
- Any references to AI assistance or generation
- Phrases like "assisted by", "generated with", "created using"

### Commit Message Structure

```
<type>(<scope>): <subject>

<body>

<footer>
```

#### Type
- feat: New feature
- fix: Bug fix
- docs: Documentation changes
- style: Code style changes (formatting, etc.)
- refactor: Code refactoring
- test: Adding or updating tests
- chore: Maintenance tasks

#### Subject
- Use present tense: "add" instead of "added"
- Use imperative mood: "add feature" not "adds feature"
- Don't capitalize first letter
- No period at the end

#### Body
- Explain what and why, not how
- Use first-person: "I implemented...", "I fixed..."
- Reference issue numbers if applicable

#### Footer
- Breaking changes: "BREAKING CHANGE: ..."
- Closes issues: "Closes #123"

### Examples

#### Good Example
```
feat(auth): implement OAuth2 authentication flow

I added support for OAuth2 authentication to improve security and enable third-party integrations. The implementation includes token refresh handling and proper error management.

Closes #45
```

#### Bad Example (contains AI references)
```
feat(auth): implement OAuth2 authentication flow

Added OAuth2 support with AI assistance. Generated using Claude to improve authentication mechanisms.

Closes #45
```

#### Good Example for Bug Fix
```
fix(api): resolve null reference exception in user service

I identified and fixed a null reference exception that occurred when retrieving user profiles. The issue was caused by missing null checks in the repository layer.
```

#### Good Example for Refactoring
```
refactor(database): optimize query performance for report generation

I refactored the database queries to use proper indexing and optimized joins. This change reduces report generation time from 30 seconds to approximately 5 seconds.
```

## Implementation Guidelines

### When Making Changes

1. Always create a commit after completing meaningful work
2. Review all changes before committing
3. Ensure commit messages follow the standards above
4. Never include AI-related content in commit messages

### Example Workflow

```bash
# After making changes
git add .
git commit -m "feat(ui): add responsive navigation menu

I implemented a responsive navigation menu that adapts to mobile and desktop viewports. The menu uses CSS Grid for layout and JavaScript for toggle functionality."
```

## Additional Notes

- These guidelines ensure clean project history
- Maintains professional appearance of repository
- Preserves human authorship integrity
- Follows industry best practices for commit messages

## Quick Reference

| DO | DON'T |
|----|-------|
| Use "I implemented..." | "AI implemented..." |
| Take ownership | Credit AI tools |
| Write professionally | Mention assistance |
| Follow conventional commits | Include AI references |
| Be specific about changes | Use vague AI-generated content |

Remember: The goal is to maintain the integrity of the project's authorship history while following professional development practices.