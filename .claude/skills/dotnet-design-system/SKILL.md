Create a skill called ".NET Design System" that ensures every UI I build 
looks modern, professional, and follows enterprise design standards.

Design principles for .NET applications:
- Clean and minimal interface: Ample whitespace, clear hierarchy, no clutter
- Neutral color palette: Blues and grays as primary (#3B4252, #434C5E, #4C566A), 
  with one accent color (e.g., #5E81AC for interactions)
- Consistent spacing: 8px, 16px, 24px, 32px, 48px, 64px increments
- Typography hierarchy: Responsive font sizes, max 2 font families, 
  minimum 14px for body text on desktop
- Interactive states: Clear hover, active, focused, disabled, and loading states
- Component consistency: Buttons, forms, cards, modals follow unified patterns
- Accessibility first: WCAG 2.1 AA compliance, semantic HTML, proper ARIA labels
- Responsive design: Mobile-first approach, tested at 375px, 768px, and 1440px
- Error handling: Clear error messages in designated areas, not inline when possible
- Loading states: Skeleton screens or spinners for async operations

For Blazor applications specifically:
- Use CSS custom properties for theming
- Implement consistent EditContext styling
- Create reusable component libraries
- Maintain accessibility in interactive components

For ASP.NET Razor Pages:
- Use CSS frameworks like Tailwind or Bootstrap consistently
- Ensure form validation displays clearly
- Implement progressive enhancement

Bad examples to avoid:
- Rainbow gradients and excessive animations
- Tiny, unreadable text (< 14px)
- Inconsistent spacing and alignment
- Every element a different color
- Too many hover effects
- Poor form field labeling

Reference this skill whenever building any UI component with Blazor or Razor.