<!-- PROJECT LOGO -->
<br />
<p align="center">

  <h3 align="center">Zadanie rekrutacyjne</h3>

  <p align="center">
    Stworzenie własnej implementacji struktury drzewiastej w ASP.Net Core MVC.

<!-- SPIS-TRESCI -->
## SPIS TREŚCI

* [O projekcie](#O PROJEKCIE)
* [Kontakt](#Kontakt)


<!-- O-PROJEKCIE -->
## O PROJEKCIE
<b>Mechanizm logowania wraz z zabezpieczeniem.</b><br />
Do jego stworzenia użyłem mechanizmu identity zalecanego przez firmę Microsoft.
<br />
<br />
  <a href="https://github.com/krzysztofrutana/zadanie-rekrutacyjne">
    <img src="https://i.ibb.co/b2HV0hx/Logowanie.png">
  </a>
  <br />
  <a href="https://github.com/krzysztofrutana/zadanie-rekrutacyjne">
    <img src="https://i.ibb.co/9W6YdX1/Logowanie-blad-1.png">
  </a>
  <br />
  <a href="https://github.com/krzysztofrutana/zadanie-rekrutacyjne">
    <img src="https://i.ibb.co/Y7QRKbV/Logowanie-blad-2.png">
  </a>
<br />
<br />
<br />
<br />
<br />
<b>Widok drzewa.</b><br />
Do jego stworzenia użyłem rekurencyjnego wywołania view component, który wyświetla tylko jedną gałąź z kategoriami. Do zmiany ikon (strzałek sygnalizujących wysunięcie gałęzi bądź nie)
napisałem proste skrypty w JS. Drzewo obsługuje sortowanie po nazwach głównych kategorii (przycisk nad drzewem po lewej) lub sortowanie po nazwach konkretnej gałęzi. W celu zapamiętania
stanu rozwinięcia konkretnej gałezi po odświerzeniu strony, po każdym zmianie stanu jakiejkolwiek gałęzi do kontrolera poprzez zapytanie AJAX wysyłana jest informacja o 
wszystkych rozwiniętych gałęziach, która po odświerzeniu jest wykorzystywana do odbudowania drzewa. Informacje o stanie są również zapisywane w pamięci lokalnej przeglądarki. 
<br />
<br />
  <a href="https://github.com/krzysztofrutana/zadanie-rekrutacyjne">
    <img src="https://i.ibb.co/tBR8K4R/Drzewo.png">
  </a>
  <br />
  <a href="https://github.com/krzysztofrutana/zadanie-rekrutacyjne">
    <img src="https://i.ibb.co/xXNCwNH/Drzewo-rozwiniete.png">
  </a>
  <br />
  <a href="https://github.com/krzysztofrutana/zadanie-rekrutacyjne">
    <img src="https://i.ibb.co/YQBzpBJ/Drzewo-g-wne-posortowane.png">
  </a>
  <br />
  <a href="https://github.com/krzysztofrutana/zadanie-rekrutacyjne">
    <img src="https://i.ibb.co/VDn8XXt/Ga-posortowana.png">
  </a>
  <br />
  <a href="https://github.com/krzysztofrutana/zadanie-rekrutacyjne">
    <img src="https://i.ibb.co/10CnJky/Drzewo-wyszukiwanie.png">
  </a>
<br />
<br />
<br />

<br />
<b>Widok tabeli.</b><br />
Jest to prosty widok tabeli. Umożliwia również wyszukiwanie kategorii po nazwie lub kategorii nadrzędnej oraz sortowanie po nazwach kategorii lub kategoriach nadrzędnych. 
<br />
<br />
  <a href="https://github.com/krzysztofrutana/zadanie-rekrutacyjne">
    <img src="https://i.ibb.co/LPv5FLH/Tabela.png">
  </a>
<br />
<br />

<br />
<b>Funkcje administratora.</b><br />
Administrator ma możliwość stworzyć nowe kategorie (i dopisać je do wszystkich istniejących jako liść), edytować daną kategorię (z zabezpieczeniem by nie przenieść jej do samej siebie
lub jej dzieci), zobaczyć szczegóły (oraz mini drzewo zawierające tylko dzieci danej kategorii z wyłączonymi opcjami administratora) oraz usunąć kategorię wraz z dziećmi.
<br />
Tworzenie oraz edycja są zabezpieczone przed nieprawidłowym wprowadzeniem nazwy (ostatni screen). Całość zrobiona jako okna modal dostępne w Bootstrap. Ich wywołanie, działanie oraz zamykanie jest oparte o skrypty JS oraz
zapytania AJAX.
<br />
<br />
  <a href="https://github.com/krzysztofrutana/zadanie-rekrutacyjne">
    <img src="https://i.ibb.co/fFh1K3Q/Utw-rz-kategori.png">
  </a>
  <br />
  <a href="https://github.com/krzysztofrutana/zadanie-rekrutacyjne">
    <img src="https://i.ibb.co/whk8g0T/Edytuj.png">
  </a>
  <br />
  <a href="https://github.com/krzysztofrutana/zadanie-rekrutacyjne">
    <img src="https://i.ibb.co/sg7L2sj/Szczeg-y.png">
  </a>
  <br />
  <a href="https://github.com/krzysztofrutana/zadanie-rekrutacyjne">
    <img src="https://i.ibb.co/717kJwJ/Usuwanie.png">
  </a>
  <br />
  <a href="https://github.com/krzysztofrutana/zadanie-rekrutacyjne">
    <img src="https://i.ibb.co/M9Lm61r/Utw-rz-kategori-zabezpieczenie.png">
  </a>
<br />
<br />

<!-- CONTACT -->
## Kontakt

Krzysztof Rutana - krzysztofrutana@wp.pl

Project Link: [https://github.com/krzysztofrutana/zadanie-rekrutacyjne](https://github.com/krzysztofrutana/zadanie-rekrutacyjne)

