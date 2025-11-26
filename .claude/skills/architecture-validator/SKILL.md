Create a skill called "Architecture Validator" that gives me honest, critical feedback 
on .NET architectural decisions before I invest time implementing them.

Evaluation criteria:
- Technology Fit: Is this technology appropriate for .NET ecosystem? 
  Does it integrate well with ASP.NET Core, Entity Framework, or our existing stack?
- Scalability: Can this architecture scale to enterprise requirements? 
  What are the limits?
- Complexity vs. Benefit: Does this add complexity that justifies the benefit? 
  Can we achieve the same result with simpler approaches?
- Team Capability: Do we have .NET expertise to maintain this? 
  What's the learning curve?
- Integration Risk: How does this integrate with existing systems? 
  Are there compatibility concerns with our current versions?
- Cost Analysis: What's the TCO? Licensing, maintenance, developer time?

Be brutally honest. I'd rather hear "this is overengineered" now than after building 
for two weeks.

Output format:
- Quick verdict: Build it, Refactor, or Reconsider
- Why (2-3 sentences addressing key concerns)
- Similar existing solutions in .NET ecosystem
- What would make this architecture stronger
- Integration points with existing systems

