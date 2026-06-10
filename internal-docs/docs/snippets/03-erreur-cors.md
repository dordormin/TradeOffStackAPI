---
sidebar_position: 3
title: Erreur CORS (Backend)
---

# Résolution : Erreur de CORS

**Problème** : `Blocked by CORS policy: No 'Access-Control-Allow-Origin' header is present`

**Solution** : 
Vérifiez toujours `ProgramExtensions.cs` dans le backend .NET. Si Vite tourne sur un nouveau port, il faut l'ajouter à la Policy :

```csharp
options.AddPolicy("AllowFrontend", policy =>
{
    policy.WithOrigins("http://localhost:5173", "http://0.0.0.0:5173")
          .AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials();
});
```
