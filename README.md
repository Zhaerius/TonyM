# TonyM
TonyM notifie la présence de cartes graphiques `Nvidia FE`. Si un GPU est en stock : 

**Version Console :** Un signal sonore vous avertira, et la page web s'ouvrira automatiquement.

**Version Discord :** Une notification sera envoyé dans le salon correspondant. Nécésite le paramètrage du fichier usersettings.json avec vos informations serveur.
```
!startrtx : commence la recherche
!stoprtx : met fin à la recherche
```

## Paramètres usersettings.json :
- Gpu : La liste des cartes a rechercher, séparé par une virgule.
- Token : Le Token de votre bot Discord
- DropChannel : L'identifiant du channel ou les messages d'alertes sont envoyés
- DropGroup : Le groupe discord à notifier lors d'un drop
- Locale : A modifier si vous voulez rechercher les drop d'autres pays


## Nécessite :
 * Runtime .NET (https://dotnet.microsoft.com/en-us/download/dotnet/6.0/runtime)


