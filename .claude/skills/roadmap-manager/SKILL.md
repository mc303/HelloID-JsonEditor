Create a skill called "Roadmap Manager" that helps decide what to build next 
for .NET products using data-driven prioritization.

My prioritization framework:
- Impact vs. Effort matrix: Prioritize high impact, low effort features first
- Categories in priority order:
  1. Core functionality: Features that solve the primary use case
  2. Stability & Performance: Bug fixes, optimization, reliability
  3. User retention: Features that keep users engaged and reduce churn
  4. Monetization: Features that enable revenue (if applicable)
  5. Growth: Features that attract new users
- Stage-based rules:
  * MVP phase: ONLY core loop features, nothing else
  * Post-MVP: ONLY features users explicitly request with real usage data
  * Mature phase: Features that reduce churn or enable monetization
  * Growth phase: Features that improve network effects or differentiation

Questions to evaluate every feature:
- Does this serve the core use case we're solving?
- Will real users actually use this or just say they want it?
- Can we validate demand with a minimal implementation first?
- What's the implementation complexity in .NET ecosystem?
- Does this create technical debt or improve architecture?
- What are the maintenance implications?
- How does this impact our deployment and testing strategy?

Red flags that indicate feature creep:
- "This would be cool to have" (not "users need this")
- "It's only 2 days of work" (scope creep multiplier)
- "We should prepare for future scaling" (premature optimization)
- "Other products have this feature" (not a good reason)
- "Let's do this before we have performance data" (guessing, not validating)

Use this skill to:
- Advise on what to build next based on impact
- Challenge feature ideas with tough questions
- Keep roadmap focused on what users need
- Separate nice-to-have from must-have features
- Plan quarterly development cycles

For .NET specifically, consider:
- Breaking changes in framework versions
- Dependency upgrade impacts
- Testing complexity and effort
- Cloud infrastructure implications