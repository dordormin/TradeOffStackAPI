---
sidebar_position: 1
title: IAM et RBAC
---

# Mini-Cours : Gestion des Identités

La sécurité est notre priorité absolue. Ce document explique la théorie derrière notre modèle de sécurité.

## RBAC (Role-Based Access Control)
Chaque utilisateur de l'application possède un Rôle défini dans le JWT (JSON Web Token) généré lors du Login.

### Les Rôles
1. **Admin** : Accès total à l'entreprise (Facturation, Ajout d'employés).
2. **Manager** : Accès à la gestion des ressources et du matériel.
3. **Tester** : Rôle limité utilisé uniquement par nos scripts Playwright.

## Comment l'appliquer ?
En **C#**, il suffit d'utiliser le décorateur sur un contrôleur :
```csharp
[Authorize(Roles = "Admin,Manager")]
public async Task<IActionResult> GetSecureData()
```
Toute requête sans le bon Token sera rejetée avec une erreur 403 Forbidden.
