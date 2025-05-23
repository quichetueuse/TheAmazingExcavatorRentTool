# TheAmazingExcavatorRentTool - Application de gestion de locations de pelleteuses

## Présentation

Application lourde developpée en C# avec le framework WPF (Windows Presentation Foundation) permettant la gestion de location de pelleteuses. Ce projet a été réalisé dans le cadre de la validation de mon BTS SIO option SLAM.

## Conception

Les diagrammes suivants représentent la conception technique du projet :

### Diagramme des cas d'utilisation

![UseCase](https://github.com/quichetueuse/TheAmazingExcavatorRentTool/blob/master/ReadMeImages/USECASES.PNG)

### Modèle Conceptuel de Données

![MCD](https://github.com/quichetueuse/TheAmazingExcavatorRentTool/blob/master/ReadMeImages/MCD.PNG)

## Aperçu des interfaces

### Page de connexion

![Page de connexion](https://github.com/quichetueuse/TheAmazingExcavatorRentTool/blob/master/ReadMeImages/login.PNG)

### Page de gestion des pelleteuses

![Page de gestion des pelleteuses](https://github.com/quichetueuse/TheAmazingExcavatorRentTool/blob/master/ReadMeImages/pelleteuse-formulaire.PNG)

### Page de gestion des marques de pelleteuses

![Page de gestion des marques de pelleteuses](https://github.com/quichetueuse/TheAmazingExcavatorRentTool/blob/master/ReadMeImages/marque-formulaire.PNG)

### Page de gestion des clients

![Page de gestion des clients](https://github.com/quichetueuse/TheAmazingExcavatorRentTool/blob/master/ReadMeImages/customer-formulaire.PNG)

### Page de gestion des locations de pelleteuses

![Page de gestion des locations de pelleteuses](https://github.com/quichetueuse/TheAmazingExcavatorRentTool/blob/master/ReadMeImages/location-formulaire.PNG)

### Page de gestion des utilisateurs

![Page de gestion des utilisations de l'application](https://github.com/quichetueuse/TheAmazingExcavatorRentTool/blob/master/ReadMeImages/user-formulaire3.PNG)

## Fonctionnalités

### Employé utilisant l'application

- Ajout, Modification, Suppression, visualisation des différentes pelleteuses
- Ajout, Modification, Suppression, visualisation des différents clients
- Ajout, Modification, Suppression, visualisation des différentes locations de pelleteuses

### Administrateur du système

- Peut réaliser les actions d'un employé standard
- Ajout, Modification, Suppression, visualisation des différentes marques de pelleteuses
- Ajout, Modification, Suppression, visualisation des différents utilisateurs de l'application

## Identifiants de connexion

**Les mots de passe utilisés ne reflètent pas les règles utilisées pour la saisie des mots de passe dans l'application.**

### Compte Administrateur

- **identifiant**: ADMIN
- **Mot de passe**: ADMIN

### Compte Employé

- **identifiant**: ELIOTB
- **Mot de passe**: azef

## Configuration technique

### Prérequis

- Windows
- .NET Framework (Version 7.0 minimum)
- Serveur LAMP (Linux Apache MySQL PHP)
- IDE ou lignes de commande

### Installation

1. Clonez le dépot git sur lequel vous êtes
2. Importez `heavy_app_e5.sql` dans la base de données du serveur LAMP
3. Ouvrez le projet dans votre IDE ou via lignes de commande et lancez l'application
