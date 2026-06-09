---
name: High-Efficiency Enterprise
colors:
  surface: '#f8f9ff'
  surface-dim: '#cbdbf5'
  surface-bright: '#f8f9ff'
  surface-container-lowest: '#ffffff'
  surface-container-low: '#eff4ff'
  surface-container: '#e5eeff'
  surface-container-high: '#dce9ff'
  surface-container-highest: '#d3e4fe'
  on-surface: '#0b1c30'
  on-surface-variant: '#5a4138'
  inverse-surface: '#213145'
  inverse-on-surface: '#eaf1ff'
  outline: '#8f7066'
  outline-variant: '#e3bfb2'
  surface-tint: '#a83900'
  primary: '#a43700'
  on-primary: '#ffffff'
  primary-container: '#cd4700'
  on-primary-container: '#fffbff'
  inverse-primary: '#ffb59a'
  secondary: '#565e74'
  on-secondary: '#ffffff'
  secondary-container: '#dae2fd'
  on-secondary-container: '#5c647a'
  tertiary: '#006194'
  on-tertiary: '#ffffff'
  tertiary-container: '#007bb9'
  on-tertiary-container: '#fdfcff'
  error: '#ba1a1a'
  on-error: '#ffffff'
  error-container: '#ffdad6'
  on-error-container: '#93000a'
  primary-fixed: '#ffdbcf'
  primary-fixed-dim: '#ffb59a'
  on-primary-fixed: '#380d00'
  on-primary-fixed-variant: '#802a00'
  secondary-fixed: '#dae2fd'
  secondary-fixed-dim: '#bec6e0'
  on-secondary-fixed: '#131b2e'
  on-secondary-fixed-variant: '#3f465c'
  tertiary-fixed: '#cce5ff'
  tertiary-fixed-dim: '#93ccff'
  on-tertiary-fixed: '#001d31'
  on-tertiary-fixed-variant: '#004b73'
  background: '#f8f9ff'
  on-background: '#0b1c30'
  surface-variant: '#d3e4fe'
typography:
  display-lg:
    fontFamily: Manrope
    fontSize: 48px
    fontWeight: '800'
    lineHeight: 56px
    letterSpacing: -0.02em
  headline-lg:
    fontFamily: Manrope
    fontSize: 32px
    fontWeight: '700'
    lineHeight: 40px
    letterSpacing: -0.01em
  headline-lg-mobile:
    fontFamily: Manrope
    fontSize: 24px
    fontWeight: '700'
    lineHeight: 32px
  headline-md:
    fontFamily: Manrope
    fontSize: 24px
    fontWeight: '600'
    lineHeight: 32px
  title-lg:
    fontFamily: Manrope
    fontSize: 20px
    fontWeight: '600'
    lineHeight: 28px
  body-lg:
    fontFamily: Manrope
    fontSize: 18px
    fontWeight: '400'
    lineHeight: 28px
  body-md:
    fontFamily: Manrope
    fontSize: 16px
    fontWeight: '400'
    lineHeight: 24px
  body-sm:
    fontFamily: Manrope
    fontSize: 14px
    fontWeight: '400'
    lineHeight: 20px
  label-md:
    fontFamily: Manrope
    fontSize: 14px
    fontWeight: '600'
    lineHeight: 20px
    letterSpacing: 0.01em
  label-sm:
    fontFamily: Manrope
    fontSize: 12px
    fontWeight: '700'
    lineHeight: 16px
    letterSpacing: 0.03em
rounded:
  sm: 0.125rem
  DEFAULT: 0.25rem
  md: 0.375rem
  lg: 0.5rem
  xl: 0.75rem
  full: 9999px
spacing:
  base: 4px
  xs: 4px
  sm: 8px
  md: 16px
  lg: 24px
  xl: 40px
  container-max: 1280px
  gutter: 24px
  margin-mobile: 16px
  margin-desktop: 32px
---

## Brand & Style
The design system focuses on a high-performance B2B SaaS aesthetic tailored for decision-makers and analysts. The brand personality is authoritative yet approachable, emphasizing clarity and precision. By blending a **Corporate Modern** foundation with subtle **Tonal Layering**, the UI evokes a sense of reliability and speed. The visual language prioritizes data density without sacrificing legibility, ensuring that professional users can navigate complex trade-offs with confidence and focus.

## Colors
This color palette is engineered for WCAG 2.0 AA compliance. The primary brand orange has been intensified to **#E65100** to ensure a 4.5:1 contrast ratio against the slightly darkened background surfaces. 

- **Primary:** A vibrant, high-contrast orange used for primary actions and brand emphasis.
- **Secondary:** A deep Navy used for navigation and high-level headers to provide a grounded, professional structure.
- **Neutral:** A slate-based neutral scale that favors legibility and subtle UI separation.
- **Surfaces:** The background is darkened to **#F1F5F9** (Slate 100) to reduce glare and improve the "pop" of foreground elements.
- **Status Indicators:** Success, Warning, and Error colors are calibrated to their 700-level shades to ensure text-on-color or color-on-white legibility meets the 4.5:1 threshold.

## Typography
The design system utilizes **Manrope** across all roles to leverage its modern, balanced, and highly legible geometric traits. 

- **Hierarchy:** Use bold and extra-bold weights for display and headlines to create a clear information architecture.
- **Readability:** Body text is set at 16px (md) or 14px (sm) to maintain high data density while remaining accessible.
- **Alignment:** Tighten letter-spacing on larger headings to maintain a professional, "locked-in" appearance.
- **Contrast:** Always use the Secondary Navy (#0F172A) for headings and Neutral Slate (#334155 / Slate 700 or darker) for body text to ensure AA compliance.

## Layout & Spacing
The layout follows a **Fluid Grid** logic with fixed maximum constraints for desktop readability. 

- **Grid:** A 12-column system is used for desktop (breakpoint 1024px+), shifting to a 4-column system for mobile.
- **Rhythm:** An 8px linear scale (with a 4px half-step for tight components) governs all padding and margins.
- **Consistency:** Use `md` (16px) for internal component padding and `lg` (24px) for spacing between distinct sections or cards. 
- **Adaptation:** On mobile, margins reduce to 16px and gutters to 16px to maximize horizontal real estate.

## Elevation & Depth
Depth is communicated through **Tonal Layers** and precise **Low-Contrast Outlines**. This minimizes visual noise in data-heavy environments.

- **Level 0 (Background):** #F1F5F9. The base canvas.
- **Level 1 (Cards/Surface):** #FFFFFF. White surfaces sit on the background with a 1px border (#E2E8F0) and a very soft, subtle ambient shadow (4px blur, 2% opacity).
- **Level 2 (Modals/Popovers):** #FFFFFF. These use a more pronounced ambient shadow (12px blur, 8% opacity) to signify interaction priority.
- **Interactive States:** Use a 2px Primary Orange outline for keyboard focus states to ensure maximum accessibility and visibility.

## Shapes
The design system adopts a **Soft** shape language. 

- **Standard:** 0.25rem (4px) radius for buttons, inputs, and small components.
- **Containers:** 0.5rem (8px) for cards and modals.
- **Selection:** Use the soft radius for checkboxes and radio button containers to maintain a consistent professional look. 
- **Buttons:** Avoid pill-shapes; stick to the standard 4px radius to reinforce the structural, SaaS-oriented aesthetic.

## Components
- **Buttons:** Primary buttons use the Primary Orange (#E65100) with White text. Secondary buttons use a Slate 700 outline with Slate 700 text.
- **Inputs:** Text fields use a 1px #CBD5E1 border, shifting to #E65100 on focus. Labels must be Slate 700 or darker for contrast.
- **Chips:** Status chips use a background tint (10% opacity of the status color) with the full-strength status color for the text (e.g., Success text at #15803D).
- **Cards:** Cards are white with a 1px #E2E8F0 border. Headers within cards should have a subtle bottom border to separate metadata from content.
- **Lists:** Use 16px vertical padding for list items with a 1px separator. Hover states should use #F8FAFC.
- **Progress Indicators:** Use the Tertiary Blue (#0284C7) for neutral progress and Status colors for specific outcomes.