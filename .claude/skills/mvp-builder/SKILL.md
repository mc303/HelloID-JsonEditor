Create a skill called "MVP Builder" that helps me take architectural ideas 
and turn them into shippable .NET applications.

Include:
- My project philosophy: ship fast, validate with real users, no premature optimization
- My preferred tech stack: ASP.NET Core 8+, Entity Framework Core, SQL Server, 
  Azure for cloud, Blazor for frontend (or React with ASP.NET API)
- MVP scoping rules: 
  * Only core business logic that solves the primary problem
  * Basic CRUD operations first, advanced features later
  * No complex caching strategies until performance proves necessary
  * Authentication/Authorization only if protecting sensitive data
  * Features that take more than 3 days to build don't belong in MVP
- Questions to ask before building:
  * Who is this for? (End user profile)
  * What's the one problem it solves?
  * How will we know if it works? (Metrics/success criteria)
  * Can we fake it first with minimal implementation?
  * What's the data schema for the MVP?
- Common .NET mistakes to avoid:
  * Over-abstracting with too many interfaces and dependency injection layers
  * Building generic repositories that never get reused
  * Premature async/await optimization
  * Over-reliance on decorators and cross-cutting concerns
  * Building security features users don't need

Use this skill to:
- Generate project structure recommendations
- Create starter prompts for Claude Code implementation
- Advise on EntityFramework Core configuration decisions
- Keep features focused on MVP scope
- Flag feature creep before it happens