---
sidebar_position: 1
title: Axios API Client
---

# Utilisation d'Axios en React

**Problème** : Si chaque développeur utilise `fetch` manuellement, on oubliera d'injecter le Token d'authentification ou on gèrera mal les erreurs globales (ex: 401 Unauthorized -> Déconnexion).

**Solution** : Utiliser l'intercepteur pré-configuré.

```typescript
// frontend/src/api/apiClient.ts
import axios from 'axios';

export const apiClient = axios.create({
  baseURL: '/api', // Le proxy de Vite gère la redirection
  headers: {
    'Content-Type': 'application/json',
  },
});

// Injection automatique du Token
apiClient.interceptors.request.use((config) => {
    const token = localStorage.getItem('token');
    if (token) config.headers['Authorization'] = `Bearer ${token}`;
    return config;
});
```

**Exemple d'utilisation :**
```typescript
const { data } = await apiClient.get<Equipment[]>('/equipment');
console.log(data); // Données typées !
```
