---
sidebar_position: 2
title: Architecture Multi-Tenant
---

# Le Concept Multi-Locataire (B2B)

## Théorie
Une architecture Multi-Tenant signifie qu'une seule instance de l'application dessert plusieurs clients (Tenants). 
C'est le modèle standard des SaaS modernes.

### Comment séparons-nous les données ?
Plutôt que d'avoir une base de données par client (trop coûteux à maintenir), nous utilisons une approche **Logique**. 
Chaque table sensible dans notre PostgreSQL possède une colonne `TenantId` (ou `CompanyId`).

### Sécurité
Lorsqu'un utilisateur fait une requête, le backend intercepte son JWT, lit son `TenantId`, et filtre automatiquement les requêtes Entity Framework avec un `Global Query Filter`. Il est donc impossible qu'un client "A" voie les factures d'un client "B", même s'il y a un bug dans le contrôleur.
