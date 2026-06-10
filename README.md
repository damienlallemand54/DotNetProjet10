# MédiLabo Solutions

Application de détection du diabète de type 2, construite en architecture microservices avec ASP.NET Core.

## Architecture

| Service | Rôle |

| PatientService | Gestion des dossiers patients (CRUD) |
| GatewayService | Point d'entrée unique via Ocelot |
| AuthService | Authentification et génération de JWT |
| Frontend | Interface utilisateur |

## Stack technique

- ASP.NET Core .NET 10
- Entity Framework Core + SQL Server
- Ocelot API Gateway
- ASP.NET Core Identity + JWT
- Docker

## Lancer le projet

```bash
docker-compose up
```

## Green Code

Actions mises en place pour réduire l'impact environnemental :

- Architecture microservices : chaque service est déployé et scalé indépendamment, évitant la surconsommation de ressources
- Requêtes SQL optimisées via EF Core (pas de SELECT *)
- Pas de dépendances inutiles dans chaque microservice
- Images Docker légères basées sur `mcr.microsoft.com/dotnet/aspnet`

## Sprints

- **Sprint 1** : PatientService, GatewayService, AuthService, Frontend (liste + détail patient)
- **Sprint 2** : NoteService (MongoDB), mise à jour Frontend
- **Sprint 3** : RiskService (évaluation du risque diabète), mise à jour Frontend
