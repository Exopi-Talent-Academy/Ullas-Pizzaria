# Entity Relationships

- 1:1 → FK i én af tabellerne (vælg den der giver mening)
- 1:N → FK i "mange"-siden  
- N:M → junction-tabel med eget PK (ofte composite) bestående af 2 FK'er

---

# Use Cases

## Use Case: Bibliotekar udlåner bog

**Primær aktør:** Bibliotekar  
**Sekundær aktør:** System

**Precondition:** Bibliotekar er logget ind

**Postcondition:** Kvittering printet og udlånsdata gemt

**Flow:**
1. Bibliotekar aktiverer "Udlån"
2. System spørger efter emne
3. Bibliotekar scanner bogens stregkode
4. System viser bogens oplysninger
5. Bibliotekar vælger "Låneroplysninger"
6. System beder om CPR-nr.
7. Bibliotekar taster låners CPR-nr.
8. System viser låneroplysninger og "Udlån bog"
9. Bibliotekar trykker "Udlån bog"
10. System registrerer udlån
11. System spørger "Ønskes kvittering?"
12. Bibliotekar svarer "Ja"
13. System printer kvittering
14. System gemmer udlånsdata

---

## Use Case: Rykkermail ved for sent return

**Primær aktør:** System  
**Sekundær aktør:** — (ingen)

**Precondition:** Låner er registreret i systemet og låner har lånt bøger, hvis udlånsperiode er overskredet

**Postcondition:** Rykkermail blev sendt

**Flow:**
1. System lytter til event om overskredet udlånsdato
2. Når event udløses med info om låner og overskredne bøger
3. System sender rykkermail til låner

---

## Use Case Diagram

- Use cases skrives i midten af diagrammet (som ellipser/ovale)
- Aktører skrives på ydersiden med streger ind til de use cases, de er involveret i
- Ingen pile - kun simple streger

---

## Use Case Diagram - Primær aktør

- Forbindelsen mellem use case og aktør trækkes kun til den primære aktør for den specifikke use case
- Hver use case har sin egen primære aktør (kan variere mellem use cases)
- Eksempel (Webshop):

```
         [Kunde]────────────────────┐
                                    │
                  ┌─────────────────┼─────────────────┐
                  │                 │                 │
             1. Opret         2. Læg i        3. Betal
                  konto            kurv
                  └─────────────────┼─────────────────┘
                                    │
                  ┌─────────────────┼─────────────────┐
                  │                 │                 │
            4. Send            5. Levér         6. Send
            ordrebekræftelse   varer           leveringsstatus
                  │                 │                 │
                  └─────────────────┼─────────────────┘
                                    │
         [System]────────────────────┤
         [Leverandør]────────────────┘
```

---

# API Design (Webshop)

## HTTP Metoder

- **POST** = oprette nye ressourcer
- **GET** = hente data
- **PUT/PATCH** = opdatere eksisterende
- **DELETE** = slette

### Eksempler:
- Opret konto → POST `/kunde`
- Betal → PUT `/ordre/{id}/paymentstatus`
- Send ordrebekræftelse → PUT `/ordre/{id}/ordrebekræftelseSendt`
- Lever varer → PUT `/levering/{id}`
- Send leveringsstatus → PUT `/levering/{id}/status`

## Authentication

- Token-baseret authentication (JWT)
- Token gemmes i middleware (både frontend og API)
- Token indeholder: bruger-ID og roller
- Roller giver adgang til bestemte endpoints

## Datamodeller

- **Kunde** → attributter (navn, email, etc.)
- **Ordre** → betalingsstatus, leverstatus, ordre­bekræftelse­Sendt
- **Vare** → navn, pris
- **Kunde → Ordre**: 1:N (én kunde, mange ordrer)
- **Ordre → Vare**: N:M (kræver junction-tabel)

---

# UML til datamodellering

## 1. UML Klassediagram (med metoder)

Viser klasser med attributter OG metoder - ikke nøgler.

```
┌──────────┐       ┌──────────┐       ┌──────────┐
│  Kunde   │       │  Ordre   │       │   Vare   │
├──────────┤       ├──────────┤       ├──────────┤
│-Navn     │       │-Dato     │       │-Navn     │
│-Email    │  1:N  │-Status   │  N:M  │-Pris     │
├──────────┤───────│-Betalt   │◄──────│          │
│+Opret()  │       ├──────────┤       ├──────────┤
│+Login()  │       │+Betaling()│      │+Update() │
└──────────┘       └──────────┘       └──────────┘
```

## 2. Database ER-diagram (med nøgler)

Viser tabeller med PK og FK - ikke metoder.

```
┌──────────┐       ┌──────────┐       ┌──────────┐
│  Kunde   │       │  Ordre   │       │   Vare   │
├──────────┤       ├──────────┤       ├──────────┤
│PK KundeId│───1:N─│PK OrdreId│       │PK VareId │
│-Navn     │       │FK KundeId│  N:M  │-Navn     │
│-Email    │       │-Dato     │◄──────│-Pris     │
└──────────┘       │-Status   │       └──────────┘
                    │-Betalt   │
                    └──────────┘
                   
          OrdreVare (junction)
          ├────────────────┤
          │PK OrdreVareId │
          │FK OrdreId     │
          │FK VareId      │
          │-Antal         │
          └────────────────┘
```

---

# Lagdelt Arkitektur (Dependencies)

Regel: Dependencies peger kun indad - ydre lag kender til indre lag, men ikke omvendt.

```
┌─────────────────────────────────────┐
│           CONTROLLER                 │  ← Yderst
├─────────────────────────────────────┤
│              SERVICE                 │  
├─────────────────────────────────────┤
│       REPOSITORY / DB CONTEXT        │  ← Tæt på data
├─────────────────────────────────────┤
│              ENTITY                   │  ← Inderst (domain)
└─────────────────────────────────────┘

Pil: Controller → Service → Repository → Entity
```

**Lagdelt arkitektur** er det overordnede begreb - at opdele koden i lag.

Eksempler på lag (i .NET):
- Presentation/API lag = Controller
- Business/Service lag = Services
- Data lag = Repository / DB Context
- Domain/Model lag = Entities

---

# Teknologier og System

Typiske spørgsmål:
1. Hvilken tech stack ville du bruge? (frontend, backend, database)
2. Hvorfor har du valgt den stack?
3. Hvilken database? (SQL vs NoSQL, og hvilken type)
4. Hvilken arkitektur? (monolit, microservices, lagdelt)
5. Hvordan ville du deploye? (cloud, Docker, CI/CD)
6. Hvordan ville du sikre API'erne?

For webshop case kunne svarene være:
- Frontend: React/Blazor
- Backend: .NET
- Database: SQL (PostgreSQL/SQL Server)
- Arkitektur: Lagdelt (Controller → Service → Repository)
- Deployment: Azure/App Service
```
