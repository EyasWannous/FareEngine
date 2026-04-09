# FareEngine

A backend demo for a ticketing and fare-calculation scenario built with ASP.NET Core 9, Entity Framework Core, and SQL Server 2019.

---

## How to Run

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

### Steps

**1. Start SQL Server via Docker Compose**

```bash
docker-compose up -d
```

This starts a SQL Server 2025 container and exposes it on port `1433`.

On startup the application will:
- Apply EF Core migrations automatically
- Seed the database with sample sold products, fare policies, and modifications

**2. Open the API explorer**

Navigate to `https://localhost:{port}/scalar/v1` in your browser 
or `https://localhost:{port}/swagger/index.html`.

### Docker Compose

```yaml
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2025-latest
    container_name: fareengine-sqlserver
    environment:
      SA_PASSWORD: "FareEngine_2025!"
      ACCEPT_EULA: "Y"
      MSSQL_PID: "Developer"
    ports:
      - "1433:1433"
    volumes:
      - sqlserver_data:/var/opt/mssql
    healthcheck:
      test: ["CMD", "/opt/mssql-tools/bin/sqlcmd", "-S", "localhost", "-U", "sa", "-P", "FareEngine_2025!", "-Q", "SELECT 1"]
      interval: 10s
      timeout: 5s
      retries: 5

volumes:
  sqlserver_data:
```

### Connection String

In `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=FareEngine;User Id=sa;Password=FareEngine_2025!;TrustServerCertificate=True"
  }
}
```

---

## Seeded Data Reference

The following IDs can be used directly to test the API without creating data first.

### Fare Policies

| ID suffix | Name | Type | Details |
|---|---|---|---|
| `...0001` | Standard Daily Pass | FlatRate | €8.00 flat |
| `...0002` | Standard Distance Rate | DistanceBased | €0.10 / km |
| `...0003` | Marine Zone 1 | ZoneBased | €5.00 |
| `...0004` | Marine Zone 2 | ZoneBased | €10.00 |
| `...0005` | Marine Zone 3 | ZoneBased | €15.00 |

### Modifications

| ID suffix | Name | Type | Details |
|---|---|---|---|
| `...0010` | First Class | Surcharge | +€15.00 |
| `...0011` | Senior Discount | Discount | −20% |

### Sold Products

| ID | Type | Policies | Modifications | Expected Final Fare |
|---|---|---|---|---|
| `...0001-...0001` | Daily Pass | Flat €8 | none | €8.00 |
| `...0001-...0002` | Daily Pass | Flat €8 | First Class | €23.00 |
| `...0001-...0003` | Daily Pass | Flat €8 | Senior −20% | €6.40 |
| `...0001-...0004` | Daily Pass | Flat €8 | First Class + Senior | €18.40 |
| `...0002-...0001` | Hybrid Trip | Distance 50km + Zone 1 | none | €10.00 |
| `...0002-...0002` | Hybrid Trip | Distance 80km + Zone 2 | First Class | €33.00 |
| `...0002-...0003` | Hybrid Trip | Distance 120km + Zone 3 | Senior −20% | €27.00 |
| `...0002-...0004` | Hybrid Trip | Distance 200km + Zone 2 | First Class + Senior | €41.00 |

---

## Design Decisions and Thinking

This section documents the full thought process — not just what was built, but why each decision was made and what alternatives were considered.

---

### Phase 1 — Understanding the Problem

Before writing any code, three core concepts needed to be clearly understood.

**Product Type** defines what kind of ticket was sold. The key insight is that different product types do not just have different prices — they have fundamentally different *logic* for computing their base price. A daily pass always costs a flat amount. A hybrid trip costs based on a combination of distance and zone. These are not just different numbers, they are different algorithms.

**Base Fare Policy** is the rule that computes the starting price for a given product type. It answers: *"before any extras, how much does this ticket cost, and how is that computed?"*

**Modification** is an optional add-on that adjusts the base fare up or down. Modifications do not change how the base price was computed — they only adjust the final number. A sold ticket can have zero, one, or many modifications stacked on top.

A critical clarification from the task: a **sold product** is a specific issued ticket instance, not a catalog offer. This means the entity must carry instance-specific data (like how many kilometers were traveled, or which zone was used) — not just a reference to a product definition.

---

### Phase 2 — Initial Design Proposal and Problems

The initial proposal was:
- `Product`, `FarePolicy`, `Modification` entities (abstract classes)
- `ProductFarePolicies` and `ProductModification` junction entities
- A `BillService` that calculates the total

**Problem 1 — Abstract classes and EF Core**
EF Core supports inheritance but forces a choice between TPH (one table, discriminator column), TPT (one table per type), or TPC. The concern was whether the schema would end up clean or full of nullable columns.

**Problem 2 — BillService becoming a god class**
A single `BillService` that knows about all product types and all modifications tends to grow unboundedly. The question was: *who is responsible for knowing how to calculate the fare for a specific product type?*

**Problem 3 — Extensibility**
The concern was: *what happens when we add a new product type or modification in the future?* The answer: adding a new type will always require new code — that is unavoidable and correct (Open/Closed Principle). The real question is whether adding a new type requires *modifying existing classes*. If yes, the design is wrong. If no, the design is extensible enough.

---

### Phase 3 — Introducing IFarePolicyCalculator

Extract an `IFarePolicyCalculator` interface. Each policy type and modification type gets its own focused calculator class. `BillService` works through the interface and never needs to know what concrete type it is dealing with.

**Two interfaces, not one**

A base fare calculator answers: *"what is the starting price, computed from scratch?"*
A modification calculator answers: *"given a price that already exists, how do I adjust it?"*

These are fundamentally different operations. One *produces* a fare, the other *transforms* a fare. Their inputs are different:
- `IFarePolicyCalculator.Calculate(FarePolicy, SoldProduct)` — needs the product for instance-specific data like distance
- `IModificationCalculator.Calculate(Modification, decimal currentFare)` — needs the running total for percentage-based discounts

**Delta, not new price**

The modification calculator returns a delta (the amount to add, which can be negative for discounts), not the new price. This is more honest about what a modification is, and it makes the fare breakdown trivial — each modification's contribution is already isolated. `BillService` simply accumulates deltas without caring whether they are surcharges or discounts.

---

### Phase 4 — The Factory Pattern

The question was: *how does BillService know which calculator to use for which policy?* The entity should not implement the calculator interface — that would mix data/domain concerns with calculation logic.

The Factory Pattern solves this cleanly. The factory looks at the concrete type of the policy or modification and returns the correct calculator. `BillService` asks the factory, the factory decides, and the calculator executes.

The factory receives concrete calculator implementations via constructor injection rather than resolving them internally. This keeps the factory testable and lets the DI container manage lifetimes. The factory uses pattern matching on the concrete type rather than switching on an enum, because the concrete type already carries the identity and pattern matching is more idiomatic in modern C#.

---

### Phase 5 — Result Objects and the Fare Breakdown

Result objects (`FareCalculationResult`, `ModificationResult`) were designed from the start to carry both a decimal amount and a string label. Without labels, the final output can only be a total number. With labels, the output becomes a full itemized breakdown:

```
Distance-based fare (80km × €0.10/km):   €8.00
Zone-based fare (zone 2):                €10.00
+ First class surcharge:                 €15.00
+ Senior discount (20%):                 -€6.60
─────────────────────────────────────────────────
Total:                                   €26.40
```

This resolves the bonus requirement for free, simply by designing the result objects correctly before writing the first calculator.

---

### Phase 6 — Domain Modeling

**Choosing product types**

Two product types were chosen to maximize design contrast:
- **Daily Pass** — a single flat rate policy, no instance-specific data needed. Simple.
- **Hybrid Trip** — a combination of fare policies chosen freely by the user at creation time.

The Hybrid Trip was deliberately kept simple. One alternative would have been to model it as a composite of other product types — essentially a product-to-products relationship. That was rejected as over-engineering. Instead, the Hybrid Trip is treated as a single product that can hold *any combination* of fare policies the user assigns to it. The many-to-many between `SoldProduct` and `FarePolicy` already supports this naturally — no special casing needed. The user picks a distance-based policy for the land segment and a zone-based policy for the marine segment, and the engine just iterates over whatever policies are attached. This keeps the model flat, flexible, and easy to extend without adding a recursive product hierarchy.

**SoldProduct: abstract**

`SoldProduct` is abstract because the two product types carry structurally different data:
- `SoldDailyPass` needs no extra fields — the flat rate policy needs nothing instance-specific
- `SoldHybridTrip` needs `DistanceInKm` and `ZoneNumber` so its calculators can do their job

A concern was raised during design: *"if we add a new product type, would we need to modify SoldHybridTrip?"* The answer is no — adding a new type means adding a new subclass, never touching an existing one. That is the Open/Closed Principle working correctly.

**Value objects as junction**

`SoldProductFarePolicy` and `SoldProductModification` are simple records holding only a foreign key ID. They are value objects inside the `SoldProduct` aggregate — not full entities with their own lifecycle. This enforces that `SoldProduct` owns and controls its own relationships. Nothing can add a fare policy to a sold product from outside; it always goes through `AddFarePolicy()`.

**FarePolicy hierarchy**

Three concrete policy types were implemented:
- `FlatRateFarePolicy` — carries `FlatAmount`
- `DistanceBasedFarePolicy` — carries `RatePerKm`
- `ZoneBasedFarePolicy` — carries `ZoneNumber` and `ZonePrice`

---

### Phase 7 — Architecture

**Clean Architecture with feature-first folders**

Classic Clean Architecture layers were kept (Domain → Application → Infrastructure → API), but the organization inside each layer was changed from "group by type" to "group by feature":

```
Domain/SoldProducts/    ← entity, value objects, repository interface
Domain/FarePolicies/    ← entity, calculator interface, factory interface
Domain/Modifications/   ← entity, calculator interface, factory interface
```

The layer rules and dependency directions stay exactly the same — only the folder organization changes, making each concept self-contained and easy to navigate.

**Where each piece lives and why**

| Piece | Layer | Reasoning |
|---|---|---|
| `IFarePolicyCalculator` | Domain | Contract the domain defines |
| `IFarePolicyCalculatorFactory` | Domain | Application depends on it; must not depend on Infrastructure |
| `FlatRateFarePolicyCalculator` | Infrastructure | Knows about a concrete domain type — implementation detail |
| `FarePolicyCalculatorFactory` | Infrastructure | Resolves concrete calculators — implementation detail |
| `BillService` | Application | Orchestrates domain interfaces; implements nothing |
| Repository interfaces | Domain | Defined by the domain, implemented by Infrastructure |
| Repository implementations | Infrastructure | Touch EF Core — an Infrastructure concern |
| `SoldProductManager` (domain service) | Domain | Enforces domain rules about policy assignment |
| `SoldProductAppService` | Application | Orchestrates domain service + persistence |

**Domain service vs application service**

`SoldProductManager` enforces domain rules — a `SoldDailyPass` can only have a `FlatRateFarePolicy`, a `SoldHybridTrip` can have any combination of policies. `SoldProductAppService` handles the workflow — validating that referenced IDs exist in the database, calling the domain service, persisting the result. These are different concerns at different levels of abstraction and are deliberately kept separate.

---

### Phase 8 — Read Repositories, ViewModels, and DTOs

**The problem with using domain entities for reads**

The write side of the application uses domain entities (`SoldProduct`, `FarePolicy`, `Modification`) with EF Core change tracking. These entities are designed for enforcing invariants and executing business logic — not for querying. Using them for reads has several problems:
- Change tracking overhead on queries that will never write anything
- Domain entities expose only what the aggregate needs — not a flat, convenient shape for the API response
- Inheritance means EF Core returns a `FarePolicy` reference; the caller has to cast or pattern-match to get subtype-specific fields

**Read repositories**

Separate read repositories (`FarePolicyReadRepository`, `ModificationReadRepository`, `SoldProductReadRepository`) were introduced that:
- Use `AsNoTracking()` on all queries — no change tracking overhead
- Project directly into flat view models using LINQ `Select()` — EF Core translates this to a single SQL query with only the needed columns
- Handle subtype-specific fields inline using `is` type checks inside the projection, which EF Core translates correctly to SQL `CASE` expressions
- Use `AsSplitQuery()` on the sold product read to avoid Cartesian explosion when loading both fare policies and modifications in the same query

**ViewModels**

`FarePolicyViewModel`, `ModificationViewModel`, and `SoldProductViewModel` live in the Domain layer. They are flat, read-optimized projections — not domain entities. They carry all subtype-specific nullable fields in one object so the caller does not need to know about the inheritance hierarchy. They are the output contract of the read repositories.

**DTOs**

`FarePolicyDto`, `ModificationDto`, and `SoldProductDto` live in the Application layer. They map from ViewModels and are the objects the API controllers return. The separation between ViewModel and DTO exists because the ViewModel is a persistence-level read projection while the DTO is an API-level response contract — these can evolve independently. Each DTO includes a `TypeString` computed property so the enum value is always returned as a readable string alongside the integer.

The flow is:

```
Read Repository (Infrastructure)
    → ViewModel (Domain)
        → DTO (Application)
            → API response (API)
```

Each step has a clear responsibility and the layers do not bleed into each other.

---

### Phase 9 — EF Core Configuration

**TPH inheritance strategy**

Table Per Hierarchy (TPH) was chosen for `FarePolicy` and `Modification`. All subtypes are stored in one table with a discriminator column. This was chosen over TPT (table per type) because it requires no joins on reads, performs better, and the number of nullable columns per type is small and acceptable.

**Separate configuration classes per subtype**

Instead of one large configuration class, separate `IEntityTypeConfiguration<T>` classes were created per subtype. Adding a new policy type means adding a new configuration class — existing ones are never touched.

**Where casting happens**

EF Core materializes the correct concrete subtype automatically based on the discriminator column. The cast happens inside the calculator, which is the correct place — the factory guarantees the pairing is correct, so the cast is always safe. On the read side, no casting is needed at all because the read repositories project directly into flat ViewModels using inline type checks.

---

### Phase 10 — BillService

`BillService` orchestrates the fare calculation flow without knowing anything about concrete product types, policy types, or modification types. Its dependencies are:
- `ISoldProductRepository` — to load the sold product with its attached policy and modification IDs
- `IFarePolicyRepository` — to load the actual policy entities by ID
- `IModificationRepository` — to load the actual modification entities by ID
- `IFarePolicyCalculatorFactory` — to get the right calculator for each policy
- `IModificationCalculatorFactory` — to get the right calculator for each modification

The calculation flow is:

```
1. Fetch sold product (with fare policy and modification IDs)
2. Fetch all fare policies by ID in one query
3. For each policy → factory picks calculator → calculator returns (Amount, Label)
4. Sum amounts → baseFare
5. Fetch all modifications by ID in one query
6. For each modification → factory picks calculator → calculator returns (Delta, Label)
7. Apply deltas to running total
8. Enforce price floor (Math.Max(currentFare, 0))
9. Return full BillResultDto with breakdown
```

**Price floor** is enforced in `BillService`, not in the modification calculators. The calculator's job is to compute a delta honestly. Enforcing the business rule that a fare cannot go below zero belongs to the orchestrator.

**Modification order matters** — percentage-based discounts apply to the running total after previous modifications, not always to the base fare. A senior discount applied after a first-class surcharge discounts the surcharge too. This is an explicit business rule documented here so it can be changed if needed.

**Batch fetching** — instead of fetching one policy or modification at a time inside the calculation loop, all IDs are collected first and fetched in a single `WHERE id IN (...)` query. This avoids N+1 query problems.

---

### Phase 11 — Things Deliberately Not Over-Engineered

The task explicitly asked for a small, clean, thoughtful solution. Several things were consciously kept simple:

- No plugin system or rules engine for loading calculators dynamically
- No MediatR or CQRS — application services are simple and direct
- No FluentValidation — guard clauses in domain constructors are sufficient
- No AutoMapper — manual mapping with static `MapFromViewModel()` methods is explicit and readable
- `InvalidOperationException` and `KeyNotFoundException` instead of a custom exception hierarchy — the global exception handler maps these to the correct HTTP status codes
- No recursive product hierarchy for Hybrid Trip — the existing many-to-many between sold products and fare policies already handles any combination the user wants

---

### Summary of Key Decisions

| Decision | Chosen | Alternative | Why |
|---|---|---|---|
| Calculator per type | `IFarePolicyCalculator` per concrete type | One calculator with big switch | Extensible, focused, testable |
| Modification result | Returns delta | Returns new price | Honest about what a modification is; breakdown is free |
| Factory implementation | Pattern match on concrete type | Switch on enum | More idiomatic, type-safe |
| Hybrid Trip modeling | Free combination of any policies | Product-to-products hierarchy | Simpler, avoids over-engineering, the many-to-many already supports it |
| Read side | Separate read repositories + ViewModels + DTOs | Return domain entities directly | No change tracking overhead, flat projections, clean API contracts |
| EF Core inheritance | TPH | TPT, TPC | Simpler queries, acceptable nullable columns |
| EF config | Separate class per subtype | All in base config | Extensible, each class focused |
| Casting location | Inside calculator (write side) / not needed (read side) | Inside repository | Calculator knows its type; read side projects to flat ViewModels |
| Fetch strategy | Batch `WHERE id IN (...)` | One query per ID | Avoids N+1 |
| Price floor | In BillService | In modification calculator | Business rule belongs to orchestrator |
| SoldProduct abstract | Yes | Single class with nullables | Cleaner schema, each subclass self-contained |
| Folder organization | Feature-first within layers | Type-first (Entities/, Interfaces/) | Each concept is self-contained and navigable |
