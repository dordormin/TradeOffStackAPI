# Documentation Technique - TradeOffStack API

Ce document résume l'architecture technique, les choix de sécurité, et le fonctionnement interne du projet TradeOffStack API. Il sert de référence pour les développeurs, DevOps, et auditeurs.

## 1. Stack Technologique & Architecture
L'API est construite selon les normes modernes de développement Backend d'Entreprise :
- **Framework** : ASP.NET Core 10.0 (Minimal APIs et Controllers)
- **Base de données** : PostgreSQL 16
- **ORM** : Entity Framework Core avec migrations automatiques (Code-First)
- **Architecture** : "Repository Pattern" avec `IGenericRepository<T>` pour assurer une séparation stricte entre la logique métier (Services) et l'accès aux données.

## 2. DevSecOps & Sécurité (Best Practices)
La sécurité a été placée au cœur du développement :
- **Authentification JWT (JSON Web Tokens)** : Les utilisateurs reçoivent un Token sécurisé (exigeant une clé de signature de 256 bits minimum).
- **Gestion des Secrets** : Les mots de passe de production ne sont jamais hardcodés. L'API utilise un fichier `.env` non versionné sur Git, et Docker se charge d'injecter la variable `ConnectionStrings__DefaultConnection` de manière sécurisée.
- **Hachage des mots de passe** : L'algorithme standard **BCrypt** est utilisé avec salage dynamique pour empêcher les attaques par dictionnaire.
- **Rate Limiting** : Un middleware bloque les requêtes abusives par adresse IP (100 requêtes/minute globales, 10 requêtes/minute sur les routes de Login) pour prévenir les attaques DDoS et le Brute Force.
- **Seeding Automatique** : Sur une base vide, un compte Administrateur par défaut est généré dynamiquement à l'initialisation pour prévenir la faille de "l'œuf et la poule" (Chicken & Egg).

## 3. CI/CD & Déploiement Continu
Le projet intègre un pipeline GitHub Actions professionnel (`ci.yml`) :
- **Trigger** : Exécuté à chaque `push` et `pull_request` vers les branches `main` et `develop`.
- **Validation** : Compile le code source en mode "Release" strict.
- **Tests Isolés** : Exécute l'intégralité de la suite de tests (`TradeOffStackAPI.Tests`) pour garantir la non-régression avant tout déploiement.

## 4. Conteneurisation (Docker)
L'API est 100% Dockerisée, prête pour un hébergement Cloud / VPS :
- **Multi-Stage Build** : Le `Dockerfile` utilise le SDK lourd pour compiler, puis transfère uniquement l'exécutable sur une image Runtime Alpine ultra-légère.
- **Sécurité Docker** : L'image finale tourne avec l'utilisateur non-root `app` pour empêcher les fuites de privilèges kernel.
- **Orchestration locale** : Le fichier `docker-compose.yml` lie automatiquement le conteneur API au conteneur PostgreSQL via un réseau virtuel interne sécurisé, et vérifie que la base est prête (Healthchecks) avant de lancer l'API.

## 5. Qualité & Tests (QA)
La robustesse du code est assurée par deux couches de validation :
- **Tests Unitaires & d'Intégration (xUnit)** : Vérification du comportement des services et du Role-Based Access Control (RBAC).
- **Postman Automatisé** : Un fichier `TradeOffStackAPI_Tests_Automatises.postman_collection.json` est fourni. Il permet d'exécuter localement le cycle de vie complet (Authentification, Création, Lecture, Modification, Suppression d'entités) et stocke dynamiquement les tokens en mémoire locale.

---
*Ce document prouve que l'infrastructure répond aux plus hauts standards de résilience, de maintenabilité (code en anglais, documentation XML complète) et de sécurité informatique.*
