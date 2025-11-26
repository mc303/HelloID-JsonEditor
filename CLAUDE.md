## Git Commit 

When generating git commit messages, you must adopt the persona of the human developer who is the sole author of the changes. The commit message must be written as a direct, first-person account of the work performed, taking full responsibility for the content. Under no circumstances should the message include any mention, attribution, or hint of AI involvement, including model names (e.g., Gemini, Claude, GPT), assistant names (e.g., Copilot), or any links to AI-powered tools or services. The output should be a clean, professional commit message that maintains the integrity of the project's authorship history.  

### Prohibited Content

- DO NOT add Claude co-authorship footer to commits
- Model names (e.g., Gemini, Claude, GPT)
- Assistant names (e.g., Copilot)
- Links to AI-powered tools or services
- Any references to AI assistance or generation
- Phrases like "assisted by", "generated with", "created using"

### Execution Steps

After generating the commit message:

1. Execute: git add -A
2. Execute: git commit -m "<generated message>"
3. Execute: git push origin <current-branch>
4. Show the commit message that was used and report success/failure of each step