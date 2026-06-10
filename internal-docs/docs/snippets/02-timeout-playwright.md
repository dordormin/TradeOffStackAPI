---
sidebar_position: 2
title: Timeout Playwright (Shadcn)
---

# Résolution : Clics bloqués dans Playwright

**Problème** : Lors du test de la création d'un équipement, Playwright reste figé sur un clic d'un bouton situé dans une Modale (Dialog) Shadcn.

**Cause** : Les modales Shadcn (Radix UI) utilisent des "Pointer Events" invisibles lors des animations, ce qui bloque le strict contrôle `Actionability` de Playwright.

**Solution (Standard de code validé)** :
Toujours utiliser l'option `force: true` pour interagir avec des boutons dans les Modales Shadcn.

```typescript
// ❌ Ne pas faire (risque de timeout)
await page.locator('button[type="submit"]').click();

// ✅ Validé par l'équipe QA
await page.locator('button[type="submit"]').click({ force: true });
```
