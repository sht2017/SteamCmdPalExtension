<div align="center">

# <img src="../SteamCmdPalExtension/Assets/Designs/StoreLogo-Transparent.svg" width="96" height="96" />

</div>

# SteamPal for Command Palette <br> [![.NET](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/9.0) [![License](https://img.shields.io/github/license/sht2017/SteamCmdPalExtension)](../LICENSE) <br> [![Microsoft Store](https://get.microsoft.com/images/en-us%20light.svg)](https://apps.microsoft.com/detail/9ns40lt8g6v9?referrer=appbadge&mode=direct)

English | [简体中文](readmes/zh_CN.md) | [正體中文](readmes/zh_TW.md)

> [!NOTE]  
> Steam will be terminated once this extension is started - that means you had better not start or reload Command Palette when games are running.

> [!WARNING]  
> The SteamPal for Command Palette using Chrome DevTools Protocol over TCP to interact with Steam CEF and fetch necessary data. Please note that this extension may conflict with other programs that use the same technique. If there is a conflict, you may consider manually setting the debugging port. In addition, since Steam service has a high privilege level on Windows, using CEF debugging over TCP locally may be risky to your account from hacking if you cannot guarantee that your system is trustworthy (I've tried to implement debugging over pipe, but it seems like the steamwebhelper just won't take it. This might implement in further).

> [!CAUTION]
> Due to a bug from upstream ([microsoft/PowerToys#39837](https://github.com/microsoft/PowerToys/issues/39837)), you may observe that there is a memory leak when using this extension. I cannot do anything about it at this point.

SteamPal is a Powertoys Command Palette extension that allows you to quickly search, launch, and manage your Steam games and applications directly from the Command Palette. This extension leverages the Steam Client's debugging interface to retrieve real-time information about your installed games, playtimes, and even launch them with a single command.

![Preview](assets/preview.png)

## Table of Contents

- [Features](#features)
- [Installation](#installation)
  - [From Source (Development)](#from-source-development)
- [Usage](#usage)
  - [Searching for Games](#searching-for-games)
  - [Launching Games](#launching-games)
  - [Accessing Game Properties](#accessing-game-properties)
  - [Searching the Steam Store](#searching-the-steam-store)
- [Settings](#settings)
- [Contributing](#contributing)
- [License](#license)

## Features

- **Quick Search for Steam Games**: Rapidly find games in your Steam library by typing their names in the Command Palette.
- **Launch Games**: Easily launch installed Steam games directly from the search results.
- **View Game Details**: See important information about your games, including playtime, last played time, and game type.
- **Access Game Properties**: Open the Steam game properties dialog for a selected game.
- **Search Steam Store**: If a game isn't found in your library, quickly search for it on the Steam Store.
- **Real-time Data**: Utilizes the Steam Client's debugging interface to fetch up-to-date game information.

## Installation

### From Source (Development)

To set up SteamCmdPalExtension for development or to run it directly from source:

... some steps

## Usage

Once the extension is installed and running, open the Powertoys Command Palette (default: `Win` + `Alt` + `Space`).

### Searching for Games

Type "SteamPal" into the Command Palette and press `Enter` to open the SteamPal extension page.
Then, start typing the name of the game you're looking for. The list will filter in real-time.

### Launching Games

Select an installed game from the search results and press `Enter` (or double/click on it) to launch the game through Steam.

### Accessing Game Properties

When a game is selected in the results, you can access its properties.
Press `Ctrl + Enter` or look for additional commands associated with the item to open the Steam game properties.

### Searching the Steam Store

If your search query doesn't yield any results from your local library, a "Search in Steam" command will appear at the bottom of the list. Select this command to open the Steam Store in your default web browser and search for your query.

## Settings

The SteamCmdPalExtension provides the following settings, accessible through the Command Palette's settings interface:

-   **Max Results**: The maximum number of search results to display. (Suggested value: below 50 for optimal performance).
-   **(Optional) Debugger Port**: Allows you to specify the Steam CEF debugger port. Leave blank for dynamic port assignment.

## Contributing

Contributions are welcome! If you find a bug or have a feature request, please open an issue on the GitHub repository. I'm open to PRs.

## License

This project is licensed under MIT License.