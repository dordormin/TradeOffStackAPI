---
name: TradeOffStack Design System
colors:
  surface: '#1a120d'
  surface-dim: '#1a120d'
  surface-bright: '#423731'
  surface-container-lowest: '#140c08'
  surface-container-low: '#231a15'
  surface-container: '#271e19'
  surface-container-high: '#322823'
  surface-container-highest: '#3d332d'
  on-surface: '#f1dfd7'
  on-surface-variant: '#dbc1b5'
  inverse-surface: '#f1dfd7'
  inverse-on-surface: '#392e29'
  outline: '#a38c80'
  outline-variant: '#554339'
  surface-tint: '#ffb68e'
  primary: '#ffb68e'
  on-primary: '#542200'
  primary-container: '#d9773a'
  on-primary-container: '#491c00'
  inverse-primary: '#99460a'
  secondary: '#bfc7d8'
  on-secondary: '#29313e'
  secondary-container: '#3f4755'
  on-secondary-container: '#adb5c6'
  tertiary: '#f6adfc'
  on-tertiary: '#50155a'
  tertiary-container: '#bc78c3'
  on-tertiary-container: '#480c53'
  error: '#ffb4ab'
  on-error: '#690005'
  error-container: '#93000a'
  on-error-container: '#ffdad6'
  primary-fixed: '#ffdbca'
  primary-fixed-dim: '#ffb68e'
  on-primary-fixed: '#331200'
  on-primary-fixed-variant: '#773300'
  secondary-fixed: '#dbe3f4'
  secondary-fixed-dim: '#bfc7d8'
  on-secondary-fixed: '#141c28'
  on-secondary-fixed-variant: '#3f4755'
  tertiary-fixed: '#ffd6fe'
  tertiary-fixed-dim: '#f6adfc'
  on-tertiary-fixed: '#35003f'
  on-tertiary-fixed-variant: '#6a2e73'
  background: '#1a120d'
  on-background: '#f1dfd7'
  surface-variant: '#3d332d'
  status-available: '#10B981'
  status-reserved: '#3B82F6'
  status-repair: '#F59E0B'
  status-critical: '#EF4444'
  surface-charcoal: '#0F1115'
  surface-slate: '#1E293B'
  border-subtle: '#334155'
typography:
  headline-xl:
    fontFamily: Manrope
    fontSize: 36px
    fontWeight: '700'
    lineHeight: 44px
    letterSpacing: -0.02em
  headline-lg:
    fontFamily: Manrope
    fontSize: 24px
    fontWeight: '600'
    lineHeight: 32px
    letterSpacing: -0.01em
  headline-md:
    fontFamily: Manrope
    fontSize: 20px
    fontWeight: '600'
    lineHeight: 28px
  body-lg:
    fontFamily: Manrope
    fontSize: 16px
    fontWeight: '400'
    lineHeight: 24px
  body-md:
    fontFamily: Manrope
    fontSize: 14px
    fontWeight: '400'
    lineHeight: 20px
  body-sm:
    fontFamily: Manrope
    fontSize: 13px
    fontWeight: '400'
    lineHeight: 18px
  label-md:
    fontFamily: Manrope
    fontSize: 12px
    fontWeight: '600'
    lineHeight: 16px
    letterSpacing: 0.02em
  mono-sm:
    fontFamily: Courier Prime
    fontSize: 12px
    fontWeight: '400'
    lineHeight: 16px
rounded:
  sm: 0.125rem
  DEFAULT: 0.25rem
  md: 0.375rem
  lg: 0.5rem
  xl: 0.75rem
  full: 9999px
spacing:
  sidebar-width: 260px
  header-height: 64px
  gutter: 1rem
  container-max: 1440px
  stack-sm: 0.5rem
  stack-md: 1rem
  stack-lg: 1.5rem
---

## Brand & Style
The design system for this internal IT Asset Management platform is built on the **Corporate / Modern** aesthetic, prioritizing data density and functional clarity. It targets a professional audience of IT Administrators and internal employees, evoking a sense of high-performance reliability and enterprise-grade security.

Drawing inspiration from high-fidelity B2B SaaS platforms like Linear and Stripe, the visual language utilizes:
- **Functional Density:** Maximizing screen real estate for asset tracking without sacrificing legibility.
- **Precision Detailing:** Crisp 1px borders, subtle monochromatic shifts, and refined typography to indicate hierarchy.
- **Sophisticated Dark/Light Transitions:** Deep charcoals and slate grays for the dark mode provide a developer-friendly environment, while the light mode uses "Paper" white surfaces with cool-gray accents to maintain focus.
- **Action-Oriented Accents:** The "Sahara" orange is used surgically for primary actions and brand presence, ensuring it remains a premium highlight rather than an overwhelming theme.

## Colors
This design system uses a primary **Dark Mode** by default to cater to IT environments, though it supports a clean light mode implementation. 

### Palette Logic
- **Primary (Sahara #C2652A):** Reserved strictly for primary call-to-actions (CTAs), active navigation states, and brand-identifying icons.
- **Neutrals:** A range of deep charcoals (#0F1115) for backgrounds and slate grays for containers. This provides the "SaaS" depth seen in modern engineering tools.
- **Semantic Status (Critical):**
    - **Available (Emerald):** Positive status, equipment ready for deployment.
    - **Reserved (Blue):** In-process or staged equipment.
    - **OutForRepair (Amber):** Warning state, requires attention but not immediate panic.
    - **Retired/Critical (Rose):** Terminal state or urgent system failure.
- **Borders:** Instead of shadows, use 1px solid borders in `#334155` (dark) or `#E2E8F0` (light) to define component boundaries and maintain a "Technical" feel.

## Typography
**Manrope** is the workhorse of the design system. It provides a contemporary, geometric feel that bridges the gap between approachable and technical.

- **Data Density:** `body-sm` (13px) is the standard for data tables and list items to maximize information visibility without compromising legibility.
- **Code/IDs:** Use `mono-sm` (Courier Prime) for Asset Tags, Serial Numbers, and JWT Token snippets to distinguish technical strings from human-readable text.
- **Hierarchy:** Headlines use tighter letter-spacing for a "Stripe-like" premium finish. 
- **Labels:** Use uppercase for `label-md` when used as table headers or category tags to create a distinct visual rhythm.

## Layout & Spacing
The layout follows a **Fixed-Fluid Hybrid** model designed for widescreen monitors typical in IT management workflows.

- **Navigation:** A persistent left-sidebar (260px) houses the primary modules: IAM, Assets, Reservations, Maintenance, Departments, and Audit Logs.
- **Grid:** A 12-column grid is used for dashboard layouts, while asset tables use a fluid-width container with a horizontal scroll overflow for high-column counts.
- **Breakpoints:**
    - **Desktop (1280px+):** Sidebar fully expanded.
    - **Tablet (768px - 1279px):** Sidebar collapses to icons; 2-column card layouts reflow to 1.
    - **Mobile (<767px):** Sidebar moves to a bottom navigation bar or a hamburger overlay; tables transition to "List Cards."
- **Data Density:** Use a 4px baseline grid. Padding within data cells is strictly 8px (sm) or 12px (md) to maintain the high-density requirement.

## Elevation & Depth
In line with the sophisticated SaaS aesthetic, depth is communicated through **Tonal Layering** and **Low-contrast Outlines** rather than heavy shadows.

- **The Layering Model:**
    - **Level 0 (Background):** Base surface (#0F1115).
    - **Level 1 (Cards/Sidebar):** Slightly lighter slate (#1E293B) with a 1px solid border.
    - **Level 2 (Modals/Sheets):** Elevated surface with a 12% opacity ambient shadow and a lighter border color to simulate physical protrusion.
- **Glassmorphism:** Reserved exclusively for the global search header and Slide-over sheets to maintain context of the underlying data while performing tasks.
- **Interactions:** Hover states on table rows should use a subtle background tint change (e.g., adding 5% white overlay) rather than an elevation lift.

## Shapes
The shape language is "Soft" (`0.25rem` or `4px`), reflecting a disciplined and engineered environment.

- **Input Fields & Buttons:** Use the standard 4px radius. 
- **Tags/Chips:** Status indicators for "Available" or "Reserved" use a slightly more rounded 12px radius to differentiate them from interactive buttons.
- **Cards:** Dashboard widgets use a larger `rounded-lg` (8px) to frame content sections softly against the background.
- **Icons:** Use linear, 2px stroke icons with slightly rounded caps to match the Manrope typeface.

## Components
- **High-Density DataTables:** Feature "Sticky" ID columns and horizontal scrolling. Header rows use `label-md` with a subtle bottom border. Row height should be capped at 40px for "Compact" and 52px for "Standard" view.
- **Slide-over Sheets:** Used for asset details and quick-edits. These slide in from the right, covering 40% of the screen width, utilizing a backdrop blur on the main content area.
- **Kanban Boards:** For Maintenance Requests. Cards are simplified versions of asset records, draggable between "Pending," "InProgress," and "Resolved" columns.
- **Request Wizards:** Multi-step forms for reservations. Use a horizontal stepper at the top with a "Locked" interaction for future steps.
- **Buttons:** 
    - *Primary:* Sahara background, white text.
    - *Secondary:* Ghost style with 1px slate border.
    - *Destructive:* Rose/Red text with ghost styling until hover.
- **Status Badges:** Small, dot-indicator paired with text (e.g., a green dot next to "Available"). Use low-saturation background tints with high-saturation text for the badge container.