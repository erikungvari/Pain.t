Aplikace vytvoří kreslící plátno a ovládací panel s tlačítky. Po spuštění je zvolen brush s černou barvou, zvolený nástroj a barva jsou zvýrazněné.
Kliknutí myší na plátno se zvoleným nástrojem způsobí zapsání počátečního bodu tvaru a nastavení boolu isDrawing na true. Průběžně se ověřuje zda kurzor zůstává na plátně.
Dokud je isDrawing true, tvar se přepisuje podle počátečního bodu a pozice kurzoru, uloží se na plátno po puštění myši (isDrawing = false).
Pomocí tlačítka Clear můžeme smazat obsah canvasu.
Upload - otevře se okno windows exploreru s nabídkou obrázků, jenom .jpg a .png, zvolený obrázek se vloží do bitmapy která se použije jako zdroj pro Image, který se vloží na plátno.
Save - vytvoří se nová bitmapa s danými rozměry, bitmapa se předá do encoderu který to přeloží na obrázek podle formátu buď .png nebo .jpg a vytvoří soubor ve specifikovaném úložišti.