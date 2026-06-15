# Wizard's Castle - Copilot Instructions

## Project Overview

This is a C# implementation of the classic 1980 text-based dungeon crawler game "Wizard's Castle". The project is a learning exercise and follows a detailed game specification (see `Spec.md`).

**Goal**: Navigate an 8x8x8 dungeon, find the Runestaff, acquire the Orb of Zot, and escape to victory.

## Build & Test Commands

```bash
# Build entire solution
dotnet build

# Run all tests
dotnet test

# Run the game (from WizardsCastle directory)
cd WizardsCastle && dotnet run

# Run a specific test class
dotnet test --filter ClassName=NavigationSituationTests

# Run a specific test method
dotnet test --filter FullyQualifiedName~NavigationSituationTests.NonExitMoveEntersDifferentRoom
```

## Project Structure

- **WizardsCastle.Logic** - Core game logic (internal visibility)
- **WizardsCastle.Logic.Tests** - NUnit tests with Moq for mocking
- **WizardsCastle** - Console UI entry point (Program.cs)

## Architecture

### Situation Pattern (Game State Machine)

The game uses a chain-of-responsibility pattern via `ISituation` implementations. Each situation represents a discrete game state:

```csharp
internal interface ISituation
{
    ISituation PlayThrough(GameData data, GameTools tools);
}
```

**Key points:**
- `Game.Play()` starts with `StartSituation` and follows the chain until a situation returns `null`
- Each situation returns the next situation or `null` to end the game
- Common situations: `NavigationSituation`, `EnterRoomSituation`, `EnterCombatSituation`, `GameOverSituation`
- Create situations via `SituationBuilder` (not direct construction)

### Dependency Injection via GameTools

`GameTools` is a manually-constructed service container holding all game services:
- No DI framework - dependencies are wired up in `GameTools.Create()`
- All services exposed as interfaces for testability
- Tests use `MockGameTools` which pre-creates `Mock<T>` instances for all services

### Internal Visibility

All logic classes use `internal` visibility - only entry point (`Program.cs`) is public.

### Key Services

- **IRandomizer** - All RNG (wraps `Random` for testability)
- **ISituationBuilder** - Factory for creating situation instances
- **IMoveInterpreter** - Translates move commands to dungeon navigation
- **ICombatService** - Handles combat mechanics
- **IGameDataBuilder** - Creates initial `GameData` (map, player, etc.)

## Testing Conventions

### Test Structure

- **Arrange**: Use `MockGameTools` and test helpers (`Any.*` for test data)
- **Act**: Call the method/situation under test
- **Assert**: Verify behavior using NUnit assertions and Moq verification

### Test Helpers

- **MockGameTools** - Pre-mocked GameTools with all services
  - Access mocks via properties (e.g., `_tools.RandomizerMock`)
  - Access implementations via base properties (e.g., `_tools.Randomizer`)
  
- **Any** class - Generates arbitrary test data
  - `Any.GameData()`, `Any.Location()`, `Any.RegularMove()`, etc.
  - Use when actual values don't matter for the test

- **MoqExtensions** - Fluent extensions for Moq setup
  - `SetupSequence()` for ordered mock returns

### Testing Situations

```csharp
// Setup
var situation = new SituationBuilder().SomeMethod();
var tools = new MockGameTools();
var data = Any.GameData();

// Configure mocks
tools.RandomizerMock.Setup(r => r.Next(1, 100)).Returns(50);

// Execute
var nextSituation = situation.PlayThrough(data, tools);

// Verify
Assert.That(nextSituation, Is.InstanceOf<ExpectedSituation>());
tools.RandomizerMock.Verify(r => r.Next(1, 100), Times.Once);
```

## Data Model

### GameData (Game State)
- `Map` - 8x8x8 grid of room contents
- `Player` - Stats, inventory, curses, position
- `CurrentLocation` - Where player is now
- `TurnCounter` - Game turn tracking

### Player
- Stats: Strength, Dexterity, Intelligence (death if any reach 0)
- Race determines starting stats
- Has equipment (`Weapon`, `Armor`), curses, special items (Runestaff, Orb of Zot)

## Game Implementation Status

**MVP Complete**: Basic gameplay loop, combat, navigation, Runestaff/Orb mechanics

**Not Yet Implemented** (see README.md To Do section):
- Trading with vendors
- Lamps/Flares
- Blindness mechanics
- Many room effect types (Books, Crystal Orb, Recipes, etc.)
- Some spells and curses

When implementing missing features, refer to `Spec.md` for detailed rules.
