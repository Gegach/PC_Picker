# PC_Picker

PC Picker je aplikacija za stolna računala koju sam izradio u sklopu kolegija "Programsko inženjerstvo" na drugoj godini prijediplomskog studija "Informacijski i poslovni sustavi". [Specifikacija korisničkih zahtjeva](https://github.com/Gegach/PC_Picker/wiki/Specifikacija-softverskih-zahtjeva) i [Specifikacija dizajna programskog proizvoda](https://github.com/Gegach/PC_Picker/wiki/Specifikacija-dizajna-programskog-proizvoda) su napisane kao wiki stranice. 

## Funkcionalnosti PC Picker-a

#### [Upravljanje komponentama](https://github.com/Gegach/PC_Picker/wiki/Specifikacija-dizajna-programskog-proizvoda#22-specifikacija-slu%C4%8Daja-kori%C5%A1tenja-upravljanje-komponentama)
Korisnik putem sučelja upravlja komponentama u bazi podataka (CRUD operacije). 
Sustav osigurava validaciju unosa i prikazuje poruke o uspjehu i greškama.

#### [Prikaz statističkih podataka](https://github.com/Gegach/PC_Picker/wiki/Specifikacija-dizajna-programskog-proizvoda#23-specifikacija-slu%C4%8Daja-kori%C5%A1tenja-prikaz-statisti%C4%8Dkih-podataka)
Korisnik može pregledati najčešće odabrane komponente kako bi analizirao trendove.
Sustav dohvaća i sortira podatke iz baze te prikazuje rezultate u vizualnom obliku.

## Arhitektura i dizajn
Sustav je organiziran u slojeve:
 - Korisničko sučelje (forme) - interakcija korisnika s sustavom
 - Repozitoriji/Servisi - komunikacija između UI sloja i baze podataka
 - Sloj za pristup bazi - rukovanje spremanjem i dohvatom podataka
 - Model podataka - entiteti

## Tehnologije i alati
 - UML dijagrami: slučajevi korištenja, slijeda, aktivnosti i klasa
 - Konceptualni model baze podataka
 - Programski jezik: C# (Windows Forms/.NET)
 - Baza podataka: MySQL (udaljeni server) kojem se pristupa preko **DBLayer.dll**

Autor: Dorijan Gegač
