# Zombify Plugin for SCP SL

The **Zombify** plugin enables SCP-049 to transform human players (such as Class D, Scientists, MTF, etc.) into SCP-049-2 (Zombies) upon death. The plugin automatically adjusts the player's health, max health, and Hume Shield when they are zombified.

## Features

- **Automatic Zombification**: When a human player is killed by SCP-049, their role is automatically changed to SCP-049-2 (Zombie).
- **Health Restoration**: Zombified players receive a full health restoration, max health, and max Hume Shield.
- **Role Eligibility**: The plugin only zombifies players in specific roles, including Class D, Scientists, MTF (Specialists, Sergeants), and Chaos Insurgents.

## Installation

1. Download the plugin file.
2. Place it in the `Plugins` folder of your **EXILED** server.
3. Restart the server to load the plugin.

## Configuration

The plugin includes a simple configuration that allows you to enable or disable it:

- **IsEnabled**: Set to `true` to enable the plugin, or `false` to disable it.
- **Debug**: Set to `true` for debug mode.

The default configuration will be automatically generated on first startup.

## Usage

- SCP-049 will automatically zombify eligible players upon killing them.
- The plugin will not require any manual commands to activate; it runs automatically when SCP-049 kills a human player.

## Example:

- SCP-049 kills a player from an eligible role (e.g., Class D).
- The player is instantly zombified into SCP-049-2 with, max health, and max Hume Shield.

## Compatibility

This plugin is compatible with EXILED version 2.0 or higher.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

