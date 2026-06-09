---
name: High-Efficiency Enterprise Dark
colors:
  surface: '#14121a'
  surface-dim: '#14121a'
  surface-bright: '#3a3841'
  surface-container-lowest: '#0f0d15'
  surface-container-low: '#1c1a23'
  surface-container: '#201e27'
  surface-container-high: '#2b2931'
  surface-container-highest: '#36333c'
  on-surface: '#e6e0ec'
  on-surface-variant: '#cac4d6'
  inverse-surface: '#e6e0ec'
  inverse-on-surface: '#312f38'
  outline: '#938e9f'
  outline-variant: '#484554'
  surface-tint: '#cbbeff'
  primary: '#cbbeff'
  on-primary: '#340098'
  primary-container: '#7a5de6'
  on-primary-container: '#fffcff'
  inverse-primary: '#6344ce'
  secondary: '#5ddac9'
  on-secondary: '#003731'
  secondary-container: '#00a393'
  on-secondary-container: '#00302a'
  tertiary: '#ffaedd'
  on-tertiary: '#60004a'
  tertiary-container: '#bc4898'
  on-tertiary-container: '#fffbff'
  error: '#ffb4ab'
  on-error: '#690005'
  error-container: '#93000a'
  on-error-container: '#ffdad6'
  primary-fixed: '#e7deff'
  primary-fixed-dim: '#cbbeff'
  on-primary-fixed: '#1e0061'
  on-primary-fixed-variant: '#4b26b5'
  secondary-fixed: '#7cf7e5'
  secondary-fixed-dim: '#5ddac9'
  on-secondary-fixed: '#00201c'
  on-secondary-fixed-variant: '#005048'
  tertiary-fixed: '#ffd8ec'
  tertiary-fixed-dim: '#ffaedd'
  on-tertiary-fixed: '#3b002d'
  on-tertiary-fixed-variant: '#831366'
  background: '#14121a'
  on-background: '#e6e0ec'
  surface-variant: '#36333c'
typography:
  headline-lg:
    fontFamily: Hanken Grotesk
    fontSize: 32px
    fontWeight: '600'
    lineHeight: '1.2'
    letterSpacing: -0.02em
  headline-md:
    fontFamily: Hanken Grotesk
    fontSize: 24px
    fontWeight: '600'
    lineHeight: '1.3'
  headline-sm:
    fontFamily: Hanken Grotesk
    fontSize: 20px
    fontWeight: '500'
    lineHeight: '1.4'
  body-lg:
    fontFamily: Inter
    fontSize: 16px
    fontWeight: '400'
    lineHeight: '1.6'
  body-md:
    fontFamily: Inter
    fontSize: 14px
    fontWeight: '400'
    lineHeight: '1.5'
  label-md:
    fontFamily: Geist
    fontSize: 12px
    fontWeight: '500'
    lineHeight: '1'
    letterSpacing: 0.05em
  mono-sm:
    fontFamily: Geist
    fontSize: 12px
    fontWeight: '400'
    lineHeight: '1.5'
rounded:
  sm: 0.125rem
  DEFAULT: 0.25rem
  md: 0.375rem
  lg: 0.5rem
  xl: 0.75rem
  full: 9999px
spacing:
  unit: 4px
  xs: 4px
  sm: 8px
  md: 16px
  lg: 24px
  xl: 48px
  gutter: 16px
  margin: 24px
---

## Brand & Style
The design system is engineered for high-density enterprise environments where technical precision and rapid data processing are paramount. The brand personality is authoritative, sophisticated, and hyper-functional. 

The design style is a blend of **Minimalism** and **Glassmorphism**, specifically optimized for a professional dark-themed aesthetic. It utilizes deep neutral backgrounds to reduce eye strain, punctuated by vibrant, high-contrast accents that signal state changes and interactive priority. The goal is to evoke an emotional response of absolute control and reliability within a complex software ecosystem.

## Colors
The palette is anchored in a deep charcoal neutral (`#0f1117`) to establish a professional enterprise foundation. 

- **Primary (#7a5de6):** Used for main action pathways, selection states, and brand presence.
- **Secondary (#1dac9c):** Reserved for data visualization highlights and secondary navigation cues.
- **Tertiary (#f275c7):** An accent color for specialized metadata, badges, and user-specific callouts.
- **Success (#17f748):** Utilized for positive status indicators and completion states.

To ensure WCAG 2.0 contrast compliance, text on surfaces uses a range of off-whites (90% opacity for headings, 70% for body). Interactive accents are paired with dark backgrounds to maintain a minimum 4.5:1 ratio, while critical alerts use high-luminance variants of the tertiary and success tokens.

## Typography
This design system utilizes a three-font strategy to balance readability with technical utility:
- **Hanken Grotesk** handles high-level headers, providing a modern, sharp edge to the interface.
- **Inter** is the workhorse for body copy and data tables, chosen for its exceptional legibility in dark mode and high-density layouts.
- **Geist** is reserved for labels, metadata, and code-like values, reinforcing the developer-friendly, technical nature of the platform.

Mobile typography scales primarily by reducing `headline-lg` to 24px and increasing line-heights slightly to improve touch-target readability.

## Layout & Spacing
The system employs a **Fluid Grid** model based on a 4px baseline shift. 

- **Desktop:** 12-column grid with 16px gutters. Content is housed in "Containers" that use `16px` padding for internal elements.
- **Tablet:** 8-column grid with 16px gutters. 
- **Mobile:** 4-column grid with 12px gutters and 16px side margins.

Spacing follows a strict geometric progression to ensure visual rhythm. Large sections (e.g., between card groups) use `xl` spacing, while related inputs use `sm` or `xs`.

## Elevation & Depth
Depth is created through **Tonal Layering** and **Subtle Glassmorphism** rather than traditional heavy shadows.

- **Level 0 (Base):** Deepest layer (`#0f1117`).
- **Level 1 (Cards/Containers):** Raised surface (`#1a1d26`) with a 1px solid border (`#2d313d`).
- **Level 2 (Modals/Popovers):** Elevated surface with a subtle 10% opacity white border and a large, soft ambient shadow (20px blur, 0.4 opacity).
- **Glass Effect:** Used for fixed navigation bars or headers—applying a `backdrop-filter: blur(12px)` with a 70% transparent surface color to maintain context of the content scrolling beneath.

## Shapes
The design system adopts a **Soft** shape language. 
- Standard components (buttons, inputs) use a `0.25rem` (4px) radius.
- Large containers and cards use `0.5rem` (8px).
- This restrained rounding maintains the professional, "square" enterprise feel while removing the harshness of perfectly sharp corners, ensuring the UI feels modern but disciplined.

## Components

### CRUD Actions
Standardized behaviors for core operations to ensure user predictability:
- **Add:** Primary button (`#7a5de6`). Displays a "Plus" icon. Opens a Level 2 Modal or an inline top-row expansion.
- **Edit:** Secondary ghost button with a "Pencil" icon. Changes the surface border of the editable element to Primary (`#7a5de6`) to indicate focus.
- **Delete:** High-contrast outline button. Requires a "Double-tap" or "Hold" interaction for destructive safety, shifting to a solid Tertiary (`#f275c7`) state on the final confirmation.
- **Retire:** A unique state using a "Archive" icon. Visual treatment involves desaturating the element to 50% opacity and applying a "Retired" label using the `label-md` typography.

### Common UI
- **Buttons:** Solid for Primary; Outlined with `#2d313d` for Secondary. 
- **Inputs:** Dark background (`#0f1117`) with a 1px border. On focus, the border transitions to Primary (`#7a5de6`).
- **Chips/Badges:** Small, Geist-font labels with 10% opacity background of the accent color (Primary, Secondary, or Success) and 100% opacity text for contrast compliance.
- **Cards:** Level 1 surfaces. Grouped data should use `md` (16px) internal padding.