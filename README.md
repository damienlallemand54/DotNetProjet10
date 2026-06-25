# MédiLabo Solutions

Application de détection du diabète de type 2, construite en architecture microservices avec ASP.NET Core.

## 🏗️ Architecture

| Service | Rôle | Base de données |
| :--- | :--- | :--- |
| **PatientService** | Gestion des dossiers patients (CRUD) | SQL Server |
| **NoteService** | Gestion de l'historique et des notes médicales | MongoDB |
| **RiskService** | Évaluation algorithmique du risque de diabète | *Aucune* |
| **GatewayService** | Point d'entrée unique et routage sécurisé via Ocelot | *Aucune* |
| **AuthService** | Authentification et génération de tokens JWT | SQL Server |
| **Frontend** | Interface utilisateur web (**ASP.NET Core MVC**) | *Aucune* |

## 🛠️ Stack technique

* **Frameworks :** ASP.NET Core .NET 10 (**Web API & MVC**)
* **Bases de données :** SQL Server, MongoDB
* **Infrastructure & Réseau :** Ocelot API Gateway, Docker & Docker Compose
* **Sécurité :** ASP.NET Core Identity, JWT (JSON Web Tokens)

## 🚀 Lancer le projet

Pour construire les images et démarrer l'ensemble des conteneurs, exécutez la commande suivante à la racine du projet :

```bash
docker-compose up --build
```

## 🍃 Green Code

Actions mises en place pour réduire l'impact environnemental et optimiser les ressources :

- **Architecture microservices :** Chaque service est déployé et scalé indépendamment, évitant la surconsommation globale de ressources.
- **Mutualisation du réseau :** L'API Gateway centralise les flux, ce qui évite la multiplication des connexions point-à-point redondantes entre chaque microservice.
- **Dépendances minimalistes :** Chaque microservice est isolé et ne charge que les bibliothèques dont il a strictement besoin.
- **Images Docker légères :** Utilisation des images de runtime optimisées `mcr.microsoft.com/dotnet/aspnet`.

Suggestion d'évolution future : 

- **Compilation Native AOT (Ahead-Of-Time) :** Passer la compilation des microservices .NET en mode Native AOT.

## 📅 Sprints & Évolution

- **Sprint 1** : Création de PatientService, GatewayService, AuthService et du Frontend MVC (liste + détail patient).
- **Sprint 2** : Implémentation de NoteService avec intégration MongoDB pour la gestion de l'historique et mise à jour du Frontend.
- **Sprint 3** : Implémentation de RiskService (moteur de calcul de risque), mise à jour du Frontend MVC et consolidation du routage interne via l'API Gateway Ocelot.
