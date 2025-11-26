---
allowed-tools: Bash(git add:*), Bash(git status:*), Bash(git commit:*)
description: Create a git commit
---

# Git Commit Message Generator

Generate professional git commit messages as the developer and sole author of the changes.

## Instructions

When generating git commit messages:

1. **Write in first person** - Use "I" statements as the developer who made the changes
2. **Take full ownership** - Present the work as your own direct contribution
3. **Be professional** - Follow conventional commit message best practices
4. **Stay clean** - Do not include any references to external tools, assistants, or automation

## Commit Message Format

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Types
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style changes (formatting, semicolons, etc.)
- `refactor`: Code refactoring
- `test`: Adding or updating tests
- `chore`: Maintenance tasks

## Rules

- Keep the subject line under 50 characters
- Use imperative mood in the subject ("Add feature" not "Added feature")
- Wrap the body at 72 characters
- Explain *what* and *why*, not *how*
- Reference relevant issue numbers in the footer when applicable

## Example Output

```
feat(auth): add password reset functionality

Implement password reset flow with email verification. Users can now
request a password reset link that expires after 24 hours.

- Add reset token generation and validation
- Create email template for reset instructions
- Add rate limiting to prevent abuse

Closes #142
```