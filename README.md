# TonyM
TonyM notifie la présence de cartes graphiques `Nvidia FE`. Si un GPU est en stock : 

**Version Console :** Un signal sonore vous avertira, et la page web s'ouvrira automatiquement.

**Version Discord :** Une notification sera envoyée dans le salon correspondant. Nécessite le paramétrage du fichier usersettings.json avec vos informations serveur.
```
!startrtx : commence la recherche
!stoprtx : met fin à la recherche
```

## Paramétrage usersettings.json :
- Gpu : La liste des cartes à rechercher
- Locale : Le pays de recherche
- Token : Le Token de votre bot Discord
- DropChannel : L'identifiant du channel ou les alertes sont envoyés
- DropGroup : Le groupe discord à notifier lors d'un drop

## Nécessite :
 * Runtime .NET (https://dotnet.microsoft.com/en-us/download/dotnet/6.0/runtime)


