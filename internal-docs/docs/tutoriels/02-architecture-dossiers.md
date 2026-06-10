---
sidebar_position: 2
title: 2. Architecture des Dossiers
---

# L'Architecture de TradeOffStack

Où devez-vous placer votre code ?

## Le Backend (C# .NET)
- `/Controllers` : Vos endpoints API.
- `/Services` : La logique métier complexe.
- `/Repositories` : L'accès exclusif à Entity Framework et la base de données. Ne mettez jamais de requêtes Linq dans vos contrôleurs !
- `/Models` : Les entités de la base de données.

## Le Frontend (React / Vite)
- `/src/pages` : Les composants qui représentent une page entière (ex: `Inventory.tsx`).
- `/src/components` : Les composants réutilisables (Boutons, Modales Shadcn).
- `/src/api` : Le client Axios. Ne faites jamais de `fetch` directement dans un composant.
