# Wizards Castle

## Background
In early 2019, @beejjorgensen gave a tech talk for the [Bend Hackers Guild](http://bend.hackersguild.us) introducing the Rust language.  The talk and language were very interesting, but a tangential detail stuck out.  Beej had taught himself Rust by using it to implementing a 40 year old game called [Wizard's Castle](https://github.com/beejjorgensen/Wizards-Castle-Rust). In support of that, he researched the origins of the game, got it's original source code, and reverse engineered a technical spec.  All that info is [here](https://github.com/beejjorgensen/Wizards-Castle-Info), and I recommend giving it a look through, it's pretty awesome.

Inspired by this, I decided to use the same game to learn F#.

### Wait a second...

You may now be noticing this repository is C#.  I decided to start by creating the game (or at least a solid start) in C# a language I know well.  Lacking Beej's lifelong knowledge of the game, this will give me a chance to learn it's ins and outs without learning something else at the same time.  Eventually, I'll link to the F# project from here.

## How To

This project is written in .NET Standard, so given a Visual Studio or VS Code environment and Nuget, you can run it in the usual ways.  

For Example:
- `dotnet build`
- `dotnet test`
- `dotnet run` (from the WizardsCastle project folder)

# To Do
These things are all pretty easy to find on the [spec page](https://github.com/beejjorgensen/Wizards-Castle-Info/blob/master/doc/wizards_castle_spec.md).

MVP of the game
* Combat, monsters on the map
* Warps
* Runestaff
* Orb of Zot

Additional work to complete the game
* Complete player creation impl
  * Allocate bonus stat points
  * Buy Weapon and armor on player creation
  * Option to Buy lamp
  * Choose Gender
* Populate map and handle room effects for the following
  * Pool
  * Chest
  * Gold Pieces
  * Sinkhole
  * Crystal Orb
  * Books
  * Treasure
* Lamp effects
* Flare effects, place them on the map
* Vendors
  * Trade
  * Combat
* Curses
* Recipes
* Random messages

