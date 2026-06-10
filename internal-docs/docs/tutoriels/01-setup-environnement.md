---
sidebar_position: 1
title: 1. Setup de l'Environnement
---

# Comment builder une application TradeOffStack

Bienvenue dans ce cours d'onboarding. Nous allons voir les étapes pour lancer l'application sur votre machine.

## Étape 1 : Prérequis
- Docker Desktop installé
- Node.js v22+
- IDE (Rider pour C#, VS Code pour React)

## Étape 2 : Lancer les conteneurs
Notre architecture repose sur Docker et le proxy web Caddy.
Ouvrez le terminal à la racine et tapez :
```bash
docker-compose -f docker-compose.yml up -d
```
Cela lancera la base de données PostgreSQL, le Backend API (port 5000) et le Reverse Proxy Caddy.

## Étape 3 : Lancer le Frontend
```bash
cd frontend
npm install
npm run dev
```

Bravo ! L'application est disponible sur `http://localhost:5173`. Procédez au chapitre suivant.
