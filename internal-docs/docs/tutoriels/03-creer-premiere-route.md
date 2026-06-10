---
sidebar_position: 3
title: 3. Créer une API
---

# Tutoriel : Créer votre première route

## Étape 1 : Le Contrôleur C#
Allez dans `Controllers/EquipmentController.cs`.

```csharp
[HttpGet("ping")]
public IActionResult Ping() 
{
    return Ok(new { Message = "Pong" });
}
```

## Étape 2 : L'appel React
Dans votre composant, utilisez notre Snippet Axios :

```tsx
import { apiClient } from '@/api/apiClient';

const testApi = async () => {
    const res = await apiClient.get('/Equipment/ping');
    console.log(res.data.message); // Pong
};
```
