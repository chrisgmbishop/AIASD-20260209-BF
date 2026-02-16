---
ai_generated: true
model: "anthropic/claude-3.5-sonnet@2024-10-22"
operator: "johnmillerATcodemag-com"
chat_id: "web-tech-instructions-20260212"
prompt: |
  create instruction file for the following technologies containing the standards and practices
  - HTML5: Semantic markup for accessibility
  - CSS3: Modern styling with CSS Grid/Flexbox for layout
  - Vanilla JavaScript: No framework dependency for MVP (lightweight, fast)
started: "2026-02-12T14:00:00Z"
ended: "2026-02-12T14:30:00Z"
task_durations:
  - task: "requirements analysis and structure planning"
    duration: "00:05:00"
  - task: "HTML5 standards and accessibility section"
    duration: "00:08:00"
  - task: "CSS3 modern layout and styling section"
    duration: "00:07:00"
  - task: "Vanilla JavaScript best practices section"
    duration: "00:07:00"
  - task: "review and validation"
    duration: "00:03:00"
total_duration: "00:30:00"
ai_log: "ai-logs/2026/02/12/web-tech-instructions-20260212/conversation.md"
source: "johnmillerATcodemag-com"
name: web-technologies
description: Standards and best practices for HTML5, CSS3, and Vanilla JavaScript development
appliesTo: "**/*.{html,css,js}"
version: "1.0.0"
author: "johnmillerATcodemag-com"
tags:
  [
    "html5",
    "css3",
    "javascript",
    "web-standards",
    "accessibility",
    "performance",
  ]
owner: "Development Team"
reviewedDate: "2026-02-12"
nextReview: "2026-05-12"
---

# Web Technologies Standards and Best Practices

## Overview

**Purpose**: Define canonical standards and best practices for HTML5, CSS3, and Vanilla JavaScript development to ensure consistent, accessible, performant, and maintainable web applications.

**Target Audience**: AI assistants (primary), frontend developers, web developers

**Scope**: Comprehensive guidelines covering:

- Semantic HTML5 markup and accessibility
- Modern CSS3 layouts, styling, and best practices
- Vanilla JavaScript patterns and performance optimization
- Cross-browser compatibility and progressive enhancement
- Performance budgets and optimization strategies
- Security considerations and best practices

**Related Documentation**:

- [Web Calculator PRD](../requirements/web-calculator-prd.md) - Example application of these standards
- [AI-Assisted Output Instructions](ai-assisted-output.instructions.md) - Canonical post-creation requirements

## Table of Contents

- [HTML5 Standards](#html5-standards)
  - [Semantic Markup](#semantic-markup)
  - [Accessibility Requirements](#accessibility-requirements)
  - [Document Structure](#document-structure)
  - [Forms and Input Validation](#forms-and-input-validation)
  - [Meta Tags and SEO](#meta-tags-and-seo)
- [CSS3 Standards](#css3-standards)
  - [Modern Layout Systems](#modern-layout-systems)
  - [Responsive Design](#responsive-design)
  - [Naming Conventions](#naming-conventions)
  - [Performance Optimization](#performance-optimization)
  - [Browser Compatibility](#browser-compatibility)
- [Vanilla JavaScript Standards](#vanilla-javascript-standards)
  - [Code Organization](#code-organization)
  - [DOM Manipulation](#dom-manipulation)
  - [Event Handling](#event-handling)
  - [State Management](#state-management)
  - [Error Handling](#error-handling)
  - [Performance Patterns](#performance-patterns)
- [Cross-Cutting Concerns](#cross-cutting-concerns)
  - [Security Best Practices](#security-best-practices)
  - [Performance Budgets](#performance-budgets)
  - [Testing Standards](#testing-standards)
  - [Progressive Enhancement](#progressive-enhancement)
- [Validation Checklist](#validation-checklist)
- [Summary](#summary)

## HTML5 Standards

### Semantic Markup

**MUST Use Semantic Elements**: Always prefer semantic HTML5 elements over generic `<div>` and `<span>` tags.

#### Structural Elements

```html
<!-- ✅ CORRECT: Use semantic structural elements -->
<header>
  <nav>
    <ul>
      <li><a href="#home">Home</a></li>
    </ul>
  </nav>
</header>

<main>
  <article>
    <header>
      <h1>Article Title</h1>
      <time datetime="2026-02-12">February 12, 2026</time>
    </header>
    <section>
      <h2>Section Heading</h2>
      <p>Content here...</p>
    </section>
  </article>

  <aside>
    <h2>Related Content</h2>
  </aside>
</main>

<footer>
  <p>&copy; 2026 Company Name</p>
</footer>

<!-- ❌ INCORRECT: Generic divs without semantic meaning -->
<div class="header">
  <div class="nav">
    <div class="menu">...</div>
  </div>
</div>
```

#### Content Elements

```html
<!-- ✅ CORRECT: Semantic content elements -->
<figure>
  <img src="chart.png" alt="Sales data for Q1 2026 showing 25% growth" />
  <figcaption>Q1 2026 Sales Performance</figcaption>
</figure>

<blockquote cite="https://example.com/article">
  <p>Quoted text here...</p>
  <footer>— <cite>Author Name</cite></footer>
</blockquote>

<details>
  <summary>Click to expand</summary>
  <p>Hidden content until user expands</p>
</details>

<!-- ❌ INCORRECT: Missing semantic elements -->
<div class="quote">
  <p>Quoted text here...</p>
</div>
```

**Semantic Element Guidelines**:

- **`<header>`**: Page or section header (can have multiple per page)
- **`<nav>`**: Primary navigation blocks only
- **`<main>`**: Main content (only ONE per page)
- **`<article>`**: Self-contained, independently distributable content
- **`<section>`**: Thematic grouping with a heading
- **`<aside>`**: Tangentially related content, sidebars
- **`<footer>`**: Page or section footer
- **`<figure>` / `<figcaption>`**: Images, diagrams, code listings with captions
- **`<time>`**: Dates and times with machine-readable `datetime` attribute

### Accessibility Requirements

**MUST Meet WCAG 2.1 AA Standards**: All HTML must comply with Web Content Accessibility Guidelines Level AA.

#### ARIA Labels and Roles

```html
<!-- ✅ CORRECT: Proper ARIA usage -->
<button aria-label="Close dialog" aria-controls="modal-1">
  <svg aria-hidden="true">...</svg>
</button>

<nav aria-label="Main navigation">
  <ul>
    ...
  </ul>
</nav>

<div role="alert" aria-live="polite" aria-atomic="true">
  <p>Form submitted successfully</p>
</div>

<!-- Calculator example with proper ARIA -->
<div role="application" aria-label="Calculator">
  <div role="status" aria-live="polite" aria-atomic="true">
    <output id="display">0</output>
  </div>
  <button aria-label="Add" aria-describedby="display">+</button>
</div>

<!-- ❌ INCORRECT: Missing ARIA labels -->
<button><svg>...</svg></button>
<nav>
  <ul>
    ...
  </ul>
</nav>
```

#### Keyboard Accessibility

```html
<!-- ✅ CORRECT: Keyboard accessible -->
<button type="button" tabindex="0">Click Me</button>
<a href="#section" tabindex="0">Jump to Section</a>

<!-- Custom interactive element needs proper roles and keyboard handling -->
<div
  role="button"
  tabindex="0"
  onkeydown="if(event.key==='Enter'||event.key===' ') handleClick(event)"
>
  Custom Button
</div>

<!-- ❌ INCORRECT: Non-focusable interactive element -->
<div onclick="handleClick()">Click Me</div>
```

**Keyboard Accessibility Requirements**:

- **MUST**: All interactive elements focusable via Tab key
- **MUST**: Visible focus indicator (minimum 2px outline, 3:1 contrast ratio)
- **MUST**: Support Enter/Space for buttons, Enter for links
- **MUST**: Escape key closes modals/dialogs
- **MUST NOT**: Create keyboard traps

#### Focus Management

```css
/* ✅ CORRECT: Clear focus indicators */
:focus {
  outline: 2px solid #0066cc;
  outline-offset: 2px;
}

:focus-visible {
  outline: 2px solid #0066cc;
  outline-offset: 2px;
}

/* ❌ INCORRECT: Removing focus outline without replacement */
:focus {
  outline: none; /* Never do this without alternative styling */
}
```

#### Alt Text Requirements

```html
<!-- ✅ CORRECT: Descriptive alt text -->
<img
  src="sales-chart.png"
  alt="Bar chart showing 25% increase in Q1 sales compared to Q4 2025"
/>

<!-- Decorative images use empty alt -->
<img src="decorative-border.png" alt="" />

<!-- Icons with meaning -->
<svg role="img" aria-label="Warning icon">...</svg>

<!-- ❌ INCORRECT: Missing or poor alt text -->
<img src="sales-chart.png" alt="chart" />
<img src="logo.png" />
<!-- Missing alt attribute -->
```

#### Color Contrast Requirements

**MUST Meet Contrast Ratios**:

- **Normal text**: 4.5:1 minimum contrast ratio
- **Large text** (18pt+ or 14pt+ bold): 3:1 minimum contrast ratio
- **UI components**: 3:1 minimum contrast ratio for interactive elements

```css
/* ✅ CORRECT: High contrast text */
.primary-text {
  color: #000000; /* Black */
  background-color: #ffffff; /* White */
  /* Contrast ratio: 21:1 */
}

.button-primary {
  color: #ffffff;
  background-color: #0066cc; /* Sufficient contrast */
  /* Contrast ratio: 7.3:1 */
}

/* ❌ INCORRECT: Insufficient contrast */
.low-contrast {
  color: #999999;
  background-color: #ffffff;
  /* Contrast ratio: 2.8:1 - FAILS WCAG AA */
}
```

### Document Structure

**MUST Follow Proper Document Structure**:

```html
<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta
      name="description"
      content="Descriptive summary for SEO (150-160 chars)"
    />
    <title>Page Title - Site Name</title>

    <!-- Preconnect to external domains -->
    <link rel="preconnect" href="https://fonts.googleapis.com" />

    <!-- Critical CSS inline -->
    <style>
      /* Critical above-the-fold styles */
    </style>

    <!-- Deferred non-critical CSS -->
    <link
      rel="stylesheet"
      href="styles.css"
      media="print"
      onload="this.media='all'"
    />
  </head>
  <body>
    <!-- Skip link for keyboard users -->
    <a href="#main-content" class="skip-link">Skip to main content</a>

    <header>...</header>

    <main id="main-content">
      <!-- Page content -->
    </main>

    <footer>...</footer>

    <!-- Scripts at end of body for performance -->
    <script src="app.js" defer></script>
  </body>
</html>
```

**Document Structure Requirements**:

- **MUST**: Include `<!DOCTYPE html>`
- **MUST**: Specify language with `<html lang="en">`
- **MUST**: Include UTF-8 charset declaration
- **MUST**: Include viewport meta tag for responsive design
- **MUST**: Include descriptive page title (50-60 characters)
- **MUST**: Include meta description (150-160 characters)
- **SHOULD**: Include skip links for keyboard navigation
- **SHOULD**: Load scripts with `defer` or `async` attributes
- **SHOULD**: Inline critical CSS, defer non-critical

### Forms and Input Validation

```html
<!-- ✅ CORRECT: Accessible form with validation -->
<form action="/submit" method="post" novalidate>
  <div class="form-group">
    <label for="email">
      Email Address
      <span aria-label="required">*</span>
    </label>
    <input
      type="email"
      id="email"
      name="email"
      required
      aria-required="true"
      aria-describedby="email-error email-hint"
      autocomplete="email"
    />
    <span id="email-hint" class="hint">We'll never share your email</span>
    <span id="email-error" class="error" role="alert" aria-live="polite"></span>
  </div>

  <fieldset>
    <legend>Notification Preferences</legend>
    <div class="checkbox-group">
      <input
        type="checkbox"
        id="email-notif"
        name="notifications"
        value="email"
      />
      <label for="email-notif">Email notifications</label>
    </div>
  </fieldset>

  <button type="submit">Submit Form</button>
</form>

<!-- ❌ INCORRECT: Inaccessible form -->
<form>
  <div>Email: <input type="text" name="email" /></div>
  <input type="submit" value="Submit" />
</form>
```

**Form Requirements**:

- **MUST**: Associate labels with inputs using `for` and `id`
- **MUST**: Use appropriate input types (`email`, `tel`, `number`, `date`)
- **MUST**: Include `autocomplete` attributes for user data
- **MUST**: Use `<fieldset>` and `<legend>` for grouped inputs
- **MUST**: Indicate required fields with `required` and `aria-required`
- **MUST**: Provide error messages with `role="alert"` and `aria-live`
- **SHOULD**: Use `aria-describedby` to link hints and errors
- **SHOULD**: Implement client-side validation with `novalidate` for custom messages

### Meta Tags and SEO

```html
<!-- ✅ CORRECT: Comprehensive meta tags -->
<head>
  <meta charset="UTF-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <meta
    name="description"
    content="Free online calculator for quick arithmetic calculations"
  />
  <meta
    name="keywords"
    content="calculator, online calculator, arithmetic, math"
  />
  <meta name="author" content="Your Name" />
  <meta name="robots" content="index, follow" />

  <!-- Open Graph for social sharing -->
  <meta property="og:title" content="Web Calculator - Free Online Calculator" />
  <meta
    property="og:description"
    content="Fast, accessible calculator for everyday calculations"
  />
  <meta property="og:type" content="website" />
  <meta property="og:url" content="https://example.com/calculator" />
  <meta property="og:image" content="https://example.com/og-image.png" />

  <!-- Twitter Card -->
  <meta name="twitter:card" content="summary_large_image" />
  <meta name="twitter:title" content="Web Calculator" />
  <meta name="twitter:description" content="Free online calculator" />

  <!-- Favicon -->
  <link rel="icon" type="image/png" sizes="32x32" href="/favicon-32x32.png" />
  <link rel="icon" type="image/png" sizes="16x16" href="/favicon-16x16.png" />
  <link rel="apple-touch-icon" sizes="180x180" href="/apple-touch-icon.png" />

  <title>Web Calculator - Free Online Calculator</title>
</head>
```

## CSS3 Standards

### Modern Layout Systems

**MUST Use Modern Layout Methods**: Prefer CSS Grid and Flexbox over floats and positioning for layouts.

#### CSS Grid for 2D Layouts

```css
/* ✅ CORRECT: CSS Grid for calculator layout */
.calculator {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  grid-template-rows: auto repeat(5, 1fr);
  gap: 8px;
  max-width: 400px;
  padding: 16px;
}

.calculator__display {
  grid-column: 1 / -1; /* Span all columns */
  grid-row: 1;
  min-height: 80px;
}

.calculator__button--zero {
  grid-column: span 2; /* Span 2 columns */
}

.calculator__button--equals {
  grid-row: span 2; /* Span 2 rows */
}

/* ❌ INCORRECT: Using floats for grid layout */
.calculator-old {
  overflow: hidden;
}

.calculator-old button {
  float: left;
  width: 25%;
  /* Complex clearfix and margin hacks needed */
}
```

**CSS Grid Guidelines**:

- **USE FOR**: Two-dimensional layouts (rows AND columns)
- **USE FOR**: Overlapping elements
- **USE FOR**: Complex responsive layouts
- **PATTERNS**: `repeat()`, `minmax()`, `auto-fit`, `auto-fill`

#### Flexbox for 1D Layouts

```css
/* ✅ CORRECT: Flexbox for navigation */
.nav {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 16px;
  padding: 12px 24px;
}

.nav__links {
  display: flex;
  gap: 12px;
  list-style: none;
}

/* Responsive flex wrapping */
.card-container {
  display: flex;
  flex-wrap: wrap;
  gap: 20px;
}

.card {
  flex: 1 1 300px; /* Grow, shrink, base width */
  min-width: 0; /* Prevent flex item overflow */
}

/* ❌ INCORRECT: Inline-block with spacing hacks */
.nav-old {
  font-size: 0; /* Hack to remove whitespace */
}

.nav-old a {
  display: inline-block;
  font-size: 16px;
  margin-right: -4px; /* Spacing hack */
}
```

**Flexbox Guidelines**:

- **USE FOR**: One-dimensional layouts (row OR column)
- **USE FOR**: Navigation bars
- **USE FOR**: Centering content
- **USE FOR**: Equal-height cards
- **PATTERNS**: `gap` property (not margins), `flex` shorthand

### Responsive Design

**MUST Implement Mobile-First Responsive Design**:

```css
/* ✅ CORRECT: Mobile-first approach */
/* Base styles for mobile (320px+) */
.calculator {
  width: 100%;
  max-width: 100%;
  padding: 12px;
}

.calculator__button {
  min-height: 44px; /* Touch-friendly minimum */
  font-size: 18px;
}

/* Tablet (768px+) */
@media (min-width: 768px) {
  .calculator {
    max-width: 400px;
    padding: 16px;
  }

  .calculator__button {
    min-height: 56px;
    font-size: 20px;
  }
}

/* Desktop (1024px+) */
@media (min-width: 1024px) {
  .calculator {
    max-width: 450px;
    padding: 20px;
  }

  .calculator__button {
    min-height: 60px;
    font-size: 22px;
  }
}

/* ❌ INCORRECT: Desktop-first approach */
.calculator {
  width: 450px; /* Desktop width */
}

@media (max-width: 768px) {
  .calculator {
    width: 100%; /* Override for mobile */
  }
}
```

**Responsive Patterns**:

```css
/* Fluid typography */
.heading {
  font-size: clamp(1.5rem, 4vw, 3rem);
}

/* Responsive grid */
.grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 20px;
}

/* Container queries (when supported) */
@container (min-width: 400px) {
  .card {
    display: grid;
    grid-template-columns: 100px 1fr;
  }
}
```

**Breakpoint Guidelines**:

- **Mobile**: 320px - 767px (base styles)
- **Tablet**: 768px - 1023px (`min-width: 768px`)
- **Desktop**: 1024px+ (`min-width: 1024px`)
- **Large Desktop**: 1440px+ (`min-width: 1440px`)

**Touch Target Sizes**:

- **MUST**: Minimum 44×44px for interactive elements (WCAG 2.1)
- **RECOMMENDED**: 48×48px for better usability

### Naming Conventions

**MUST Use BEM (Block Element Modifier) Methodology**:

```css
/* ✅ CORRECT: BEM naming convention */
.calculator {
  /* Block */
  display: grid;
}

.calculator__display {
  /* Element */
  grid-column: 1 / -1;
}

.calculator__button {
  /* Element */
  padding: 12px;
}

.calculator__button--operator {
  /* Modifier */
  background-color: #ff9500;
}

.calculator__button--disabled {
  /* Modifier */
  opacity: 0.5;
  cursor: not-allowed;
}

/* ❌ INCORRECT: Inconsistent naming */
.calculator_display {
  /* Mixed separators */
}

.calculatorButton {
  /* CamelCase */
}

.calc-btn-op {
  /* Unclear abbreviations */
}
```

**BEM Guidelines**:

- **Block**: `.block` - Standalone entity (e.g., `.calculator`, `.modal`)
- **Element**: `.block__element` - Part of block (e.g., `.calculator__button`)
- **Modifier**: `.block--modifier` or `.block__element--modifier` - Variation (e.g., `.button--primary`)
- **AVOID**: Deep nesting (`.block__element__subelement` ❌)
- **AVOID**: Generic names (`.button`, `.title` ❌)
- **USE**: Descriptive, specific names (`.calculator__button`, `.modal__title` ✅)

### Performance Optimization

**MUST Optimize CSS for Performance**:

```css
/* ✅ CORRECT: Efficient selectors */
.button {
  /* Class selector - fast */
  padding: 12px;
}

.button--primary {
  /* Single class */
  background-color: #0066cc;
}

/* Use CSS custom properties for dynamic values */
:root {
  --color-primary: #0066cc;
  --spacing-unit: 8px;
  --transition-speed: 200ms;
}

.button {
  background-color: var(--color-primary);
  padding: var(--spacing-unit);
  transition: all var(--transition-speed);
}

/* ❌ INCORRECT: Inefficient selectors */
div.calculator div.row div.button {
  /* Too specific */
  padding: 12px;
}

[data-type="button"][data-style="primary"] {
  /* Slow attribute selectors */
  background-color: #0066cc;
}

* {
  /* Universal selector - expensive */
  margin: 0;
  padding: 0;
}
```

**Performance Guidelines**:

- **MUST**: Use class selectors over ID, attribute, or element selectors
- **MUST**: Minimize specificity (avoid deep nesting)
- **MUST**: Use CSS custom properties for theming
- **SHOULD**: Avoid `@import` (use build tools to concatenate)
- **SHOULD**: Use `will-change` sparingly for animations
- **SHOULD**: Minimize repaints/reflows

**CSS Loading Strategy**:

```html
<!-- Critical CSS inline -->
<style>
  /* Above-the-fold styles */
  .calculator {
    display: grid;
  }
</style>

<!-- Non-critical CSS deferred -->
<link
  rel="preload"
  href="styles.css"
  as="style"
  onload="this.onload=null;this.rel='stylesheet'"
/>
<noscript><link rel="stylesheet" href="styles.css" /></noscript>
```

### Browser Compatibility

**MUST Ensure Cross-Browser Compatibility**:

```css
/* ✅ CORRECT: Progressive enhancement with fallbacks */
.card {
  background-color: #ffffff; /* Fallback */
  background-color: rgba(255, 255, 255, 0.95); /* Modern browsers */
}

.button {
  display: inline-block; /* Fallback */
  display: inline-flex; /* Modern browsers */
  align-items: center;
}

/* Feature queries for new CSS */
@supports (display: grid) {
  .container {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
  }
}

@supports not (display: grid) {
  .container {
    display: flex;
    flex-wrap: wrap;
  }
}

/* ❌ INCORRECT: No fallbacks */
.card {
  background-color: rgba(255, 255, 255, 0.95); /* IE11 doesn't support rgba */
}
```

**Vendor Prefix Guidelines**:

```css
/* Use autoprefixer via build tools instead of manual prefixes */
.button {
  transform: translateX(10px);
  /* Autoprefixer adds: */
  /* -webkit-transform: translateX(10px); */
  /* -ms-transform: translateX(10px); */
}
```

**Browser Support Requirements**:

- **MUST**: Support last 2 versions of major browsers (Chrome, Firefox, Safari, Edge)
- **MUST**: Provide fallbacks for CSS features not universally supported
- **SHOULD**: Use feature queries (`@supports`) for progressive enhancement
- **SHOULD**: Use autoprefixer for vendor prefixes

## Vanilla JavaScript Standards

### Code Organization

**MUST Organize Code into Modules**:

```javascript
// ✅ CORRECT: Module pattern with clear separation
// calculator.js
const Calculator = (() => {
  "use strict";

  // Private state
  let currentValue = 0;
  let previousValue = 0;
  let operation = null;
  let displayValue = "0";

  // Private methods
  const updateDisplay = () => {
    const display = document.getElementById("display");
    if (display) {
      display.textContent = displayValue;
      display.setAttribute("aria-live", "polite");
    }
  };

  const calculate = () => {
    switch (operation) {
      case "+":
        return previousValue + currentValue;
      case "-":
        return previousValue - currentValue;
      case "*":
        return previousValue * currentValue;
      case "/":
        return currentValue !== 0 ? previousValue / currentValue : null;
      default:
        return currentValue;
    }
  };

  // Public API
  return {
    init() {
      this.attachEventListeners();
      updateDisplay();
    },

    inputNumber(number) {
      if (displayValue === "0" || displayValue === "Error") {
        displayValue = String(number);
      } else {
        displayValue += number;
      }
      currentValue = parseFloat(displayValue);
      updateDisplay();
    },

    inputOperator(op) {
      if (operation !== null) {
        this.calculate();
      }
      previousValue = currentValue;
      operation = op;
      displayValue = "0";
    },

    calculate() {
      const result = calculate();
      if (result === null) {
        displayValue = "Error";
        currentValue = 0;
      } else {
        displayValue = String(result);
        currentValue = result;
      }
      operation = null;
      previousValue = 0;
      updateDisplay();
    },

    clear() {
      currentValue = 0;
      previousValue = 0;
      operation = null;
      displayValue = "0";
      updateDisplay();
    },

    attachEventListeners() {
      // Event delegation for buttons
      const calculator = document.getElementById("calculator");
      if (calculator) {
        calculator.addEventListener("click", this.handleClick.bind(this));
        document.addEventListener("keydown", this.handleKeydown.bind(this));
      }
    },

    handleClick(event) {
      const button = event.target.closest("button");
      if (!button) return;

      const { action, value } = button.dataset;

      switch (action) {
        case "number":
          this.inputNumber(value);
          break;
        case "operator":
          this.inputOperator(value);
          break;
        case "equals":
          this.calculate();
          break;
        case "clear":
          this.clear();
          break;
      }
    },

    handleKeydown(event) {
      const keyMap = {
        0: () => this.inputNumber("0"),
        1: () => this.inputNumber("1"),
        2: () => this.inputNumber("2"),
        3: () => this.inputNumber("3"),
        4: () => this.inputNumber("4"),
        5: () => this.inputNumber("5"),
        6: () => this.inputNumber("6"),
        7: () => this.inputNumber("7"),
        8: () => this.inputNumber("8"),
        9: () => this.inputNumber("9"),
        "+": () => this.inputOperator("+"),
        "-": () => this.inputOperator("-"),
        "*": () => this.inputOperator("*"),
        "/": () => this.inputOperator("/"),
        Enter: () => this.calculate(),
        Escape: () => this.clear(),
      };

      const handler = keyMap[event.key];
      if (handler) {
        event.preventDefault();
        handler();
      }
    },
  };
})();

// Initialize on DOMContentLoaded
document.addEventListener("DOMContentLoaded", () => {
  Calculator.init();
});

// ❌ INCORRECT: Global variables and functions
var currentValue = 0;
var operation = null;

function inputNumber(num) {
  currentValue = num;
  document.getElementById("display").innerHTML = num; // Use textContent, not innerHTML
}

function calculate() {
  // No error handling, no encapsulation
}
```

**Code Organization Guidelines**:

- **MUST**: Use module pattern or ES6 modules for encapsulation
- **MUST**: Use `'use strict'` directive
- **MUST**: Keep related functionality together
- **MUST**: Separate concerns (display, calculation, state management)
- **SHOULD**: Use revealing module pattern for public/private separation
- **AVOID**: Global variables and functions

### DOM Manipulation

**MUST Use Efficient, Safe DOM Manipulation**:

```javascript
// ✅ CORRECT: Safe, efficient DOM manipulation
const Display = {
  element: null,

  init() {
    this.element = document.getElementById("display");
    if (!this.element) {
      console.error("Display element not found");
      return false;
    }
    return true;
  },

  setValue(value) {
    if (!this.element) return;

    // Use textContent for security (prevents XSS)
    this.element.textContent = value;

    // Update ARIA live region for screen readers
    this.element.setAttribute("aria-live", "polite");
  },

  addClass(className) {
    if (!this.element) return;
    this.element.classList.add(className);
  },

  removeClass(className) {
    if (!this.element) return;
    this.element.classList.remove(className);
  },

  toggleClass(className) {
    if (!this.element) return;
    this.element.classList.toggle(className);
  },
};

// Batch DOM updates
const updateCalculator = (state) => {
  // Use DocumentFragment for multiple insertions
  const fragment = document.createDocumentFragment();

  state.history.forEach((item) => {
    const li = document.createElement("li");
    li.textContent = `${item.expression} = ${item.result}`;
    fragment.appendChild(li);
  });

  const historyList = document.getElementById("history");
  historyList.innerHTML = ""; // Clear once
  historyList.appendChild(fragment); // Insert all at once
};

// ❌ INCORRECT: Unsafe, inefficient DOM manipulation
function updateDisplay(value) {
  // Security risk: innerHTML with user input
  document.getElementById("display").innerHTML = value;

  // Multiple reflows
  document.getElementById("display").style.color = "red";
  document.getElementById("display").style.fontSize = "24px";
  document.getElementById("display").style.fontWeight = "bold";
}

// Inefficient: Multiple DOM insertions
state.history.forEach((item) => {
  const historyList = document.getElementById("history");
  const li = document.createElement("li");
  li.textContent = item;
  historyList.appendChild(li); // Reflow for each item
});
```

**DOM Manipulation Guidelines**:

- **MUST**: Cache DOM element references
- **MUST**: Use `textContent` instead of `innerHTML` for text (security)
- **MUST**: Check if elements exist before manipulating
- **MUST**: Use `classList` API instead of `className` string manipulation
- **SHOULD**: Batch DOM updates to minimize reflows
- **SHOULD**: Use `DocumentFragment` for multiple insertions
- **SHOULD**: Use `dataset` API for data attributes

### Event Handling

**MUST Use Event Delegation and Proper Listeners**:

```javascript
// ✅ CORRECT: Event delegation pattern
const EventManager = {
  init() {
    // Single listener on parent element
    const calculator = document.getElementById("calculator");
    if (calculator) {
      calculator.addEventListener(
        "click",
        this.handleCalculatorClick.bind(this),
      );
    }

    // Keyboard events on document
    document.addEventListener("keydown", this.handleKeydown.bind(this));

    // Remove listeners on cleanup
    window.addEventListener("beforeunload", this.cleanup.bind(this));
  },

  handleCalculatorClick(event) {
    // Find the button that was clicked
    const button = event.target.closest("button");
    if (!button) return;

    // Prevent default if needed
    event.preventDefault();

    // Handle based on button type
    const action = button.dataset.action;
    const value = button.dataset.value;

    switch (action) {
      case "number":
        this.handleNumber(value);
        break;
      case "operator":
        this.handleOperator(value);
        break;
      case "equals":
        this.handleEquals();
        break;
      case "clear":
        this.handleClear();
        break;
    }
  },

  handleKeydown(event) {
    // Check for modifier keys
    if (event.ctrlKey || event.altKey || event.metaKey) {
      return;
    }

    // Map keys to actions
    const keyHandlers = {
      Enter: () => this.handleEquals(),
      Escape: () => this.handleClear(),
      Backspace: () => this.handleBackspace(),
    };

    const handler = keyHandlers[event.key];
    if (handler) {
      event.preventDefault();
      handler();
    }
  },

  cleanup() {
    // Remove event listeners to prevent memory leaks
    const calculator = document.getElementById("calculator");
    if (calculator) {
      calculator.removeEventListener("click", this.handleCalculatorClick);
    }
    document.removeEventListener("keydown", this.handleKeydown);
  },
};

// ❌ INCORRECT: Individual listeners on each button
document.querySelectorAll(".button").forEach((button) => {
  button.onclick = function () {
    // Memory leak: onclick vs addEventListener
    // No event.preventDefault()
    // Handler not bound to context
    handleClick(this.value);
  };
});
```

**Event Handling Guidelines**:

- **MUST**: Use event delegation for dynamically created elements
- **MUST**: Use `addEventListener` (not onclick properties)
- **MUST**: Remove event listeners when no longer needed
- **MUST**: Bind context properly (use arrow functions or `.bind()`)
- **SHOULD**: Use named functions for event handlers (easier debugging)
- **SHOULD**: Call `preventDefault()` or `stopPropagation()` when appropriate
- **AVOID**: Inline event handlers in HTML

### State Management

**MUST Implement Centralized State Management**:

```javascript
// ✅ CORRECT: Centralized state management
const StateManager = (() => {
  "use strict";

  // Private state
  let state = {
    currentValue: 0,
    previousValue: 0,
    operation: null,
    displayValue: "0",
    history: [],
    memory: 0,
  };

  // State observers
  const observers = [];

  // Private methods
  const notifyObservers = () => {
    observers.forEach((observer) => observer(state));
  };

  const validateState = (newState) => {
    return {
      currentValue: Number(newState.currentValue) || 0,
      previousValue: Number(newState.previousValue) || 0,
      operation:
        typeof newState.operation === "string" ? newState.operation : null,
      displayValue: String(newState.displayValue) || "0",
      history: Array.isArray(newState.history) ? newState.history : [],
      memory: Number(newState.memory) || 0,
    };
  };

  // Public API
  return {
    getState() {
      // Return copy to prevent direct mutation
      return { ...state };
    },

    setState(updates) {
      // Validate and merge updates
      const validatedUpdates = validateState({ ...state, ...updates });

      // Check if state actually changed
      const hasChanged = Object.keys(validatedUpdates).some(
        (key) => validatedUpdates[key] !== state[key],
      );

      if (hasChanged) {
        state = validatedUpdates;

        // Persist to localStorage
        this.saveToStorage();

        // Notify observers
        notifyObservers();
      }
    },

    subscribe(observer) {
      if (typeof observer === "function") {
        observers.push(observer);
        // Return unsubscribe function
        return () => {
          const index = observers.indexOf(observer);
          if (index > -1) {
            observers.splice(index, 1);
          }
        };
      }
    },

    addToHistory(expression, result) {
      const history = [
        ...state.history,
        { expression, result, timestamp: Date.now() },
      ];
      // Keep only last 50 entries
      if (history.length > 50) {
        history.shift();
      }
      this.setState({ history });
    },

    clearHistory() {
      this.setState({ history: [] });
    },

    saveToStorage() {
      try {
        localStorage.setItem("calculator-state", JSON.stringify(state));
      } catch (error) {
        console.error("Failed to save state:", error);
      }
    },

    loadFromStorage() {
      try {
        const saved = localStorage.getItem("calculator-state");
        if (saved) {
          const parsed = JSON.parse(saved);
          state = validateState(parsed);
          notifyObservers();
        }
      } catch (error) {
        console.error("Failed to load state:", error);
      }
    },

    reset() {
      this.setState({
        currentValue: 0,
        previousValue: 0,
        operation: null,
        displayValue: "0",
      });
    },
  };
})();

// Usage: Subscribe to state changes
const unsubscribe = StateManager.subscribe((state) => {
  Display.setValue(state.displayValue);
  HistoryView.render(state.history);
});

// ❌ INCORRECT: Scattered state management
let currentValue = 0; // Global variable
let previousValue = 0;

function updateValue(val) {
  currentValue = val;
  document.getElementById("display").textContent = val;
  localStorage.setItem("value", val); // Direct localStorage access
  // No validation, no observers, direct DOM manipulation
}
```

**State Management Guidelines**:

- **MUST**: Centralize state in single source of truth
- **MUST**: Validate all state updates
- **MUST**: Prevent direct state mutation (return copies)
- **SHOULD**: Implement observer pattern for state changes
- **SHOULD**: Separate state management from UI updates
- **SHOULD**: Persist critical state to localStorage
- **SHOULD**: Implement undo/redo if applicable

### Error Handling

**MUST Implement Comprehensive Error Handling**:

```javascript
// ✅ CORRECT: Comprehensive error handling
const ErrorHandler = {
  logError(error, context = {}) {
    // Log to console in development
    if (process.env.NODE_ENV === "development") {
      console.error("Error:", error);
      console.error("Context:", context);
      console.trace();
    }

    // Send to error tracking service in production
    if (process.env.NODE_ENV === "production") {
      // window.errorTracker.log(error, context);
    }
  },

  handleCalculationError(error, operation) {
    this.logError(error, { operation, type: "calculation" });

    // Show user-friendly error message
    Display.setValue("Error");
    Display.showMessage("Calculation error. Please try again.", "error");

    // Reset state
    StateManager.reset();
  },

  handleStorageError(error) {
    this.logError(error, { type: "storage" });

    // Notify user if storage is critical
    if (error.name === "QuotaExceededError") {
      Display.showMessage("Storage full. History may not be saved.", "warning");
    }
  },
};

// Calculation with error handling
const Calculator = {
  divide(a, b) {
    try {
      if (b === 0) {
        throw new Error("Division by zero");
      }

      if (!Number.isFinite(a) || !Number.isFinite(b)) {
        throw new Error("Invalid operands");
      }

      const result = a / b;

      if (!Number.isFinite(result)) {
        throw new Error("Result is not finite");
      }

      return result;
    } catch (error) {
      ErrorHandler.handleCalculationError(error, { operation: "divide", a, b });
      return null;
    }
  },

  calculate(expression) {
    try {
      // Validate expression
      if (typeof expression !== "string" || expression.trim() === "") {
        throw new Error("Invalid expression");
      }

      // Sanitize input
      const sanitized = expression.replace(/[^0-9+\-*/().\s]/g, "");

      // Perform calculation (using safe evaluation method)
      const result = this.evaluateExpression(sanitized);

      return result;
    } catch (error) {
      ErrorHandler.handleCalculationError(error, { expression });
      return null;
    }
  },

  evaluateExpression(expression) {
    // NEVER use eval() - security risk
    // Implement safe expression parser instead
    // For example, use a library like math.js or implement Shunting Yard algorithm

    // Placeholder for safe evaluation
    return 0;
  },
};

// Global error handler
window.addEventListener("error", (event) => {
  ErrorHandler.logError(event.error, {
    message: event.message,
    filename: event.filename,
    lineno: event.lineno,
    colno: event.colno,
  });
});

// Promise rejection handler
window.addEventListener("unhandledrejection", (event) => {
  ErrorHandler.logError(event.reason, { type: "unhandled-promise" });
});

// ❌ INCORRECT: No error handling
function divide(a, b) {
  return a / b; // No division by zero check
}

function calculate(expression) {
  return eval(expression); // Security risk!
}
```

**Error Handling Guidelines**:

- **MUST**: Validate all inputs before processing
- **MUST**: Use try-catch blocks for operations that can fail
- **MUST**: Provide user-friendly error messages
- **MUST**: Log errors with context for debugging
- **MUST NEVER**: Use `eval()` - security risk
- **SHOULD**: Implement global error handlers
- **SHOULD**: Gracefully degrade on errors
- **SHOULD**: Send errors to monitoring service in production

### Performance Patterns

**MUST Optimize JavaScript for Performance**:

```javascript
// ✅ CORRECT: Debouncing expensive operations
const debounce = (func, delay) => {
  let timeoutId;
  return function (...args) {
    clearTimeout(timeoutId);
    timeoutId = setTimeout(() => func.apply(this, args), delay);
  };
};

// Debounce search input
const searchInput = document.getElementById("search");
const performSearch = debounce((query) => {
  // Expensive search operation
  console.log("Searching for:", query);
}, 300);

searchInput.addEventListener("input", (e) => {
  performSearch(e.target.value);
});

// ✅ CORRECT: Throttling scroll events
const throttle = (func, limit) => {
  let inThrottle;
  return function (...args) {
    if (!inThrottle) {
      func.apply(this, args);
      inThrottle = true;
      setTimeout(() => (inThrottle = false), limit);
    }
  };
};

const handleScroll = throttle(() => {
  // Handle scroll
  console.log("Scrolled");
}, 100);

window.addEventListener("scroll", handleScroll);

// ✅ CORRECT: Lazy loading images
const lazyLoadImages = () => {
  const images = document.querySelectorAll("img[data-src]");

  const imageObserver = new IntersectionObserver((entries, observer) => {
    entries.forEach((entry) => {
      if (entry.isIntersecting) {
        const img = entry.target;
        img.src = img.dataset.src;
        img.removeAttribute("data-src");
        observer.unobserve(img);
      }
    });
  });

  images.forEach((img) => imageObserver.observe(img));
};

// ✅ CORRECT: Memoization for expensive calculations
const memoize = (fn) => {
  const cache = new Map();
  return (...args) => {
    const key = JSON.stringify(args);
    if (cache.has(key)) {
      return cache.get(key);
    }
    const result = fn(...args);
    cache.set(key, result);
    return result;
  };
};

const expensiveCalculation = memoize((n) => {
  // Expensive operation
  return n * n;
});

// ✅ CORRECT: RequestAnimationFrame for animations
const animate = () => {
  // Update animation
  const element = document.getElementById("animated");
  if (element) {
    element.style.transform = `translateX(${position}px)`;
  }

  requestAnimationFrame(animate);
};

requestAnimationFrame(animate);

// ❌ INCORRECT: No debouncing on expensive operations
searchInput.addEventListener("input", (e) => {
  performExpensiveSearch(e.target.value); // Runs on every keystroke
});

// ❌ INCORRECT: Using setTimeout for animations
setInterval(() => {
  element.style.left = position + "px"; // Causes layout thrashing
}, 16);
```

**Performance Guidelines**:

- **MUST**: Debounce rapid-fire events (input, resize)
- **MUST**: Throttle continuous events (scroll, mousemove)
- **MUST**: Use `requestAnimationFrame` for animations
- **SHOULD**: Lazy load images and resources
- **SHOULD**: Memoize expensive pure functions
- **SHOULD**: Use Web Workers for heavy computations
- **SHOULD**: Minimize DOM access and batch operations
- **AVOID**: `setInterval` for animations
- **AVOID**: Synchronous operations blocking the main thread

## Cross-Cutting Concerns

### Security Best Practices

**MUST Implement Security Measures**:

```javascript
// ✅ CORRECT: Input sanitization
const sanitizeInput = (input) => {
  if (typeof input !== "string") return "";

  // Remove potentially dangerous characters
  return input
    .replace(/[<>\"']/g, "") // Remove HTML/script injection characters
    .replace(/javascript:/gi, "") // Remove javascript: protocol
    .replace(/on\w+=/gi, "") // Remove event handlers
    .trim()
    .slice(0, 1000); // Limit length
};

// ✅ CORRECT: Safe DOM insertion
const displayUserInput = (input) => {
  const sanitized = sanitizeInput(input);
  const element = document.getElementById("output");

  // Use textContent, never innerHTML for user input
  element.textContent = sanitized;
};

// ✅ CORRECT: Content Security Policy
// Include in HTML <head>
// <meta http-equiv="Content-Security-Policy"
//       content="default-src 'self'; script-src 'self'; style-src 'self' 'unsafe-inline'; img-src 'self' data:;">

// ✅ CORRECT: Secure localStorage usage
const SecureStorage = {
  setItem(key, value) {
    try {
      // Validate key
      if (typeof key !== "string" || key.length === 0) {
        throw new Error("Invalid storage key");
      }

      // Don't store sensitive data in localStorage
      if (this.isSensitive(key)) {
        console.warn("Sensitive data should not be stored in localStorage");
        return false;
      }

      // Serialize and store
      const serialized = JSON.stringify(value);
      localStorage.setItem(key, serialized);
      return true;
    } catch (error) {
      console.error("Storage error:", error);
      return false;
    }
  },

  getItem(key) {
    try {
      const item = localStorage.getItem(key);
      return item ? JSON.parse(item) : null;
    } catch (error) {
      console.error("Storage retrieval error:", error);
      return null;
    }
  },

  isSensitive(key) {
    const sensitivePatterns = ["password", "token", "secret", "api_key"];
    return sensitivePatterns.some((pattern) =>
      key.toLowerCase().includes(pattern),
    );
  },
};

// ❌ INCORRECT: XSS vulnerability
function displayMessage(message) {
  document.getElementById("output").innerHTML = message; // XSS risk!
}

// ❌ INCORRECT: Using eval
function calculate(expression) {
  return eval(expression); // NEVER USE EVAL!
}

// ❌ INCORRECT: Storing sensitive data
localStorage.setItem("api_key", "secret123"); // Never store secrets in localStorage
```

**Security Guidelines**:

- **MUST**: Sanitize all user input
- **MUST**: Use `textContent` instead of `innerHTML` for user content
- **MUST**: Implement Content Security Policy (CSP)
- **MUST NEVER**: Use `eval()`, `Function()`, or `setTimeout(string)`
- **MUST NOT**: Store sensitive data in localStorage
- **SHOULD**: Validate and limit input length
- **SHOULD**: Use HTTPS for all external resources
- **SHOULD**: Implement rate limiting for API calls

### Performance Budgets

**MUST Meet Performance Budgets**:

```javascript
// Performance budget targets
const PERFORMANCE_BUDGETS = {
  // File sizes (compressed)
  maxHTMLSize: 15 * 1024, // 15 KB
  maxCSSSize: 50 * 1024, // 50 KB
  maxJSSize: 100 * 1024, // 100 KB
  maxImageSize: 200 * 1024, // 200 KB per image
  maxTotalSize: 500 * 1024, // 500 KB total

  // Timing metrics
  maxFirstContentfulPaint: 1500, // 1.5s
  maxTimeToInteractive: 3000, // 3s
  maxCalculationTime: 50, // 50ms for calculations
  maxRenderTime: 100, // 100ms for UI updates

  // Resource counts
  maxRequests: 20,
  maxDOMNodes: 1500,
};

// Performance monitoring
const PerformanceMonitor = {
  measureCalculation(fn, ...args) {
    const start = performance.now();
    const result = fn(...args);
    const duration = performance.now() - start;

    if (duration > PERFORMANCE_BUDGETS.maxCalculationTime) {
      console.warn(`Calculation exceeded budget: ${duration}ms`);
    }

    return result;
  },

  measureRender(fn) {
    const start = performance.now();
    fn();
    const duration = performance.now() - start;

    if (duration > PERFORMANCE_BUDGETS.maxRenderTime) {
      console.warn(`Render exceeded budget: ${duration}ms`);
    }
  },

  checkBudgets() {
    // Check timing budgets
    const perfData = performance.getEntriesByType("navigation")[0];
    const fcp = performance.getEntriesByName("first-contentful-paint")[0];

    if (fcp && fcp.startTime > PERFORMANCE_BUDGETS.maxFirstContentfulPaint) {
      console.warn(`FCP exceeded budget: ${fcp.startTime}ms`);
    }

    // Check DOM size
    const domSize = document.getElementsByTagName("*").length;
    if (domSize > PERFORMANCE_BUDGETS.maxDOMNodes) {
      console.warn(`DOM size exceeded budget: ${domSize} nodes`);
    }
  },
};

// Run check after load
window.addEventListener("load", () => {
  PerformanceMonitor.checkBudgets();
});
```

**Performance Budget Guidelines**:

- **MUST**: Total page weight < 500 KB (compressed)
- **MUST**: JavaScript bundle < 100 KB (compressed)
- **MUST**: CSS < 50 KB (compressed)
- **MUST**: First Contentful Paint < 1.5s
- **MUST**: Time to Interactive < 3s
- **MUST**: Calculation latency < 50ms
- **SHOULD**: Fewer than 20 HTTP requests
- **SHOULD**: DOM size < 1500 nodes

### Testing Standards

**MUST Write Comprehensive Tests**:

```javascript
// ✅ CORRECT: Unit tests for calculator logic
describe("Calculator", () => {
  let calculator;

  beforeEach(() => {
    calculator = new Calculator();
  });

  describe("addition", () => {
    it("should add two positive numbers", () => {
      expect(calculator.add(5, 3)).toBe(8);
    });

    it("should handle decimal numbers", () => {
      expect(calculator.add(0.1, 0.2)).toBeCloseTo(0.3, 10);
    });

    it("should handle negative numbers", () => {
      expect(calculator.add(-5, 3)).toBe(-2);
    });

    it("should handle zero", () => {
      expect(calculator.add(0, 5)).toBe(5);
      expect(calculator.add(5, 0)).toBe(5);
    });
  });

  describe("division", () => {
    it("should divide two numbers", () => {
      expect(calculator.divide(10, 2)).toBe(5);
    });

    it("should return null for division by zero", () => {
      expect(calculator.divide(10, 0)).toBeNull();
    });

    it("should handle decimal results", () => {
      expect(calculator.divide(1, 3)).toBeCloseTo(0.333333, 5);
    });
  });

  describe("error handling", () => {
    it("should throw error for invalid inputs", () => {
      expect(() => calculator.add("a", 5)).toThrow();
      expect(() => calculator.add(null, 5)).toThrow();
    });
  });
});

// Integration tests
describe("Calculator UI", () => {
  let container;

  beforeEach(() => {
    container = document.createElement("div");
    container.innerHTML = `
      <div id="calculator">
        <output id="display">0</output>
        <button data-action="number" data-value="5">5</button>
        <button data-action="operator" data-value="+">+</button>
        <button data-action="number" data-value="3">3</button>
        <button data-action="equals">=</button>
      </div>
    `;
    document.body.appendChild(container);
    Calculator.init();
  });

  afterEach(() => {
    document.body.removeChild(container);
  });

  it("should display result after calculation", () => {
    const display = document.getElementById("display");
    const buttons = {
      five: document.querySelector('[data-value="5"]'),
      plus: document.querySelector('[data-value="+"]'),
      three: document.querySelector('[data-value="3"]'),
      equals: document.querySelector('[data-action="equals"]'),
    };

    buttons.five.click();
    buttons.plus.click();
    buttons.three.click();
    buttons.equals.click();

    expect(display.textContent).toBe("8");
  });
});
```

**Testing Guidelines**:

- **MUST**: Write unit tests for all business logic
- **MUST**: Test edge cases and error conditions
- **MUST**: Maintain minimum 80% code coverage
- **SHOULD**: Write integration tests for user workflows
- **SHOULD**: Test accessibility with automated tools
- **SHOULD**: Test cross-browser compatibility
- **SHOULD**: Use test-driven development (TDD) when appropriate

### Progressive Enhancement

**MUST Implement Progressive Enhancement**:

```html
<!-- ✅ CORRECT: Progressive enhancement -->
<!-- Base HTML works without JavaScript -->
<form id="calculator-form" action="/calculate" method="post">
  <input type="number" name="value1" required />
  <select name="operation">
    <option value="add">+</option>
    <option value="subtract">-</option>
    <option value="multiply">×</option>
    <option value="divide">÷</option>
  </select>
  <input type="number" name="value2" required />
  <button type="submit">Calculate</button>
  <output id="result"></output>
</form>

<script>
  // JavaScript enhancement
  (function () {
    "use strict";

    // Check for required features
    if (
      !("querySelector" in document) ||
      !("addEventListener" in window) ||
      !("localStorage" in window)
    ) {
      // Graceful degradation - form still works
      return;
    }

    // Enhance with JavaScript
    const form = document.getElementById("calculator-form");
    if (form) {
      form.addEventListener("submit", (e) => {
        e.preventDefault();
        // Handle calculation client-side
        const formData = new FormData(form);
        const result = calculate(
          formData.get("value1"),
          formData.get("operation"),
          formData.get("value2"),
        );
        document.getElementById("result").textContent = result;
      });
    }
  })();
</script>
```

**Progressive Enhancement Guidelines**:

- **MUST**: Provide base functionality without JavaScript
- **MUST**: Feature-detect before using modern APIs
- **MUST**: Gracefully degrade for unsupported browsers
- **SHOULD**: Use `<noscript>` tags for critical functionality
- **SHOULD**: Test without JavaScript enabled
- **SHOULD**: Provide fallbacks for modern CSS features

## Validation Checklist

### HTML5 Validation

- [ ] All HTML5 documents include `<!DOCTYPE html>`
- [ ] Language specified with `<html lang="en">`
- [ ] Semantic HTML elements used appropriately (`<header>`, `<nav>`, `<main>`, `<article>`, `<section>`, `<aside>`, `<footer>`)
- [ ] All images have descriptive `alt` attributes
- [ ] Forms use proper labels associated with inputs
- [ ] ARIA labels and roles implemented correctly
- [ ] All interactive elements keyboard accessible
- [ ] Focus indicators visible and meet contrast requirements
- [ ] Color contrast meets WCAG 2.1 AA standards (4.5:1 for normal text, 3:1 for large text)
- [ ] Document passes W3C HTML validator
- [ ] Document passes axe-core accessibility audit
- [ ] Meta tags include viewport, description, and social sharing tags

### CSS3 Validation

- [ ] CSS Grid or Flexbox used for layouts (not floats)
- [ ] Mobile-first responsive design implemented
- [ ] BEM naming convention used consistently
- [ ] No inline styles in HTML
- [ ] CSS custom properties used for theming
- [ ] Vendor prefixes added via autoprefixer
- [ ] Feature queries (`@supports`) used for progressive enhancement
- [ ] Touch targets minimum 44×44px
- [ ] Print stylesheet provided (if applicable)
- [ ] Critical CSS inlined, non-critical CSS deferred
- [ ] CSS passes W3C CSS validator
- [ ] No CSS exceeds 50KB (compressed)

### JavaScript Validation

- [ ] Code organized into modules (module pattern or ES6 modules)
- [ ] `'use strict'` directive used
- [ ] No global variables (except module exports)
- [ ] Event delegation used for dynamic elements
- [ ] Event listeners properly removed when no longer needed
- [ ] All inputs validated and sanitized
- [ ] `textContent` used instead of `innerHTML` for user content
- [ ] `eval()` and `Function()` not used
- [ ] Try-catch blocks used for error-prone operations
- [ ] User-facing error messages provided
- [ ] State management centralized
- [ ] DOM element references cached
- [ ] Debouncing/throttling used for expensive operations
- [ ] No calculation exceeds 50ms
- [ ] No render operation exceeds 100ms
- [ ] Code passes ESLint validation
- [ ] Unit tests written with minimum 80% coverage

### Security Validation

- [ ] Content Security Policy implemented
- [ ] All user input sanitized
- [ ] No sensitive data stored in localStorage
- [ ] HTTPS used for all external resources
- [ ] No XSS vulnerabilities (verified with scanner)
- [ ] No `eval()` or `Function()` used
- [ ] Input length limits enforced

### Performance Validation

- [ ] Total page weight < 500 KB (compressed)
- [ ] JavaScript bundle < 100 KB (compressed)
- [ ] CSS < 50 KB (compressed)
- [ ] First Contentful Paint < 1.5s
- [ ] Time to Interactive < 3s
- [ ] Lighthouse performance score > 90
- [ ] Images optimized and lazy-loaded
- [ ] Scripts loaded with `defer` or `async`

### Browser Compatibility

- [ ] Tested in Chrome (last 2 versions)
- [ ] Tested in Firefox (last 2 versions)
- [ ] Tested in Safari (last 2 versions)
- [ ] Tested in Edge (last 2 versions)
- [ ] Tested on iOS Safari (iOS 13+)
- [ ] Tested on Chrome Android (latest)
- [ ] Graceful degradation for older browsers
- [ ] Feature detection used for modern APIs

## Summary

This instruction file defines comprehensive standards and best practices for HTML5, CSS3, and Vanilla JavaScript development. Key requirements include:

**HTML5 Requirements**:

- Use semantic markup for structure and meaning
- Implement WCAG 2.1 AA accessibility standards
- Provide proper ARIA labels and keyboard navigation
- Ensure all interactive elements are keyboard accessible
- Meet color contrast requirements (4.5:1 minimum)

**CSS3 Requirements**:

- Use CSS Grid and Flexbox for modern layouts
- Implement mobile-first responsive design
- Follow BEM naming convention
- Use CSS custom properties for theming
- Meet performance budgets (< 50 KB compressed)
- Provide feature detection and fallbacks

**JavaScript Requirements**:

- Organize code into modules with clear separation of concerns
- Implement centralized state management
- Use event delegation and proper listener management
- Validate and sanitize all user input
- Never use `eval()` or `innerHTML` with user content
- Implement comprehensive error handling
- Meet performance budgets (< 100 KB compressed, < 50ms calculations)
- Write unit tests with minimum 80% coverage

**Cross-Cutting Concerns**:

- Implement Content Security Policy
- Meet performance budgets (< 500 KB total, < 1.5s FCP, < 3s TTI)
- Ensure cross-browser compatibility
- Implement progressive enhancement
- Provide comprehensive testing

All code must pass validation tools (W3C validators, ESLint, axe-core, Lighthouse) and meet the specified performance budgets and accessibility standards.

---

**Document Version**: 1.0.0
**Last Updated**: 2026-02-12
**Maintainer**: Development Team
**Related Instructions**:

- [AI-Assisted Output Instructions](ai-assisted-output.instructions.md)
- [Web Calculator PRD](../requirements/web-calculator-prd.md)
