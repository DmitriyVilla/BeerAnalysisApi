# BeerAnalysisApi

Eine kleine ASP.NET-Core-Web-API, die Produktdaten (Bier) aus einer JSON-Quelle lädt und ein paar Auswertungen bereitstellt.  
Die API liefert zum Beispiel:

- günstigster und teuerster Preis pro Liter  
- Produkte mit einem bestimmten Preis (z. B. 17,99 €)  
- welches Produkt die meisten Flaschen hat  
- eine Gesamtauswertung aller Werte  

Die Daten werden standardmäßig von flaschenpost (öffentliche Test-URL) geladen.

Die Struktur ist bewusst einfach gehalten (Controller, Service, Client, Utils, DTOs).  
Ein paar Unit-Tests sind ebenfalls enthalten.

## Architektur

Das Projekt ist bewusst einfach und übersichtlich aufgebaut:

- Controllers – API-Endpoints
- Services – Business-Logik
- Clients – Laden der JSON-Daten
- Utils – kleine Parser (Preis pro Liter, Flaschenanzahl)
- Models / DTOs – Datenstrukturen
- Tests – Unit-Tests mit xUnit und Moq


## Endpoints:

- /api/products/cheapest-per-litre
- /api/products/most-expensive-per-litre
- /api/products/price-exact?price=17.99
- /api/products/most-bottles
- /api/products/analysis
