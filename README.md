# LeagueConvert

Easily convert champion models from League of Legends to glTF, complete with
textures and animations.

## What is 'vnext'

On this branch I'll be working towards the next version of LeagueConvert
(formerly LeagueBulkConvert, might still be changed).

## Status and roadmap

Initially I wanted to rewrite all of LeagueBulkConvert into a library, a command
line version and a GUI version. I also wanted to add support for map geometry
files, but I ran into two issues: I can't easily texture map geometry with
LeagueToolkit and the library LeagueToolkit uses for creating glTF files
(SharpGLTF) is, in my opinion, very hard to use. This is why I'm writing my own,
somewhat low-level glTF writing library called SimpleGltf (might publish it as
a standalone library at some point.)

### SimpleGltf

Currently implemented:
* Primitives
* Skins
* Materials
* Animations

To-do:

* Optimisation
* Check adherence to glTF spec

### LeagueConvert

Currently, LeagueConvert can do everything LeagueBulkConvert can do, but with
code that isn't terrible.

Planned:

* Rewrite hash table loading code
* Support for map geometry
* Support for texture masks

### LeagueConvert.CommandLine

Postponed until I am done with LeagueConvert.

### LeagueConvert.WPF

I might make a WPF version so I can replace LeagueBulkConvert as quickly as
possible. Currently on hold until I am done with LeagueConvert.CommandLine.

### LeagueConvert.MAUI

Postponed until .NET 6 releases. Will replace LeagueConvert.WPF.

## Biggest changes compared to LeagueBulkConvert

* No more temporary 'assets' directories
* Much more granular control over skin parsing, loading and saving
* Cross-platform (the command line version should be cross-platform out of the
  box, cross-platform GUI requires .NET MAUI or Xamarin)

## How to use

1. Download the latest source code .zip archive
2. Install the .NET 5.0 SDK (optionally you can use an IDE like Rider or
Visual Studio)
3. Execute `dotnet run` in the 'LeagueConvert/LeagueConvert.CommandLine' folder
