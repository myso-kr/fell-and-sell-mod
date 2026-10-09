---
layout: default
title: "Exploration helper"
description: "Native map reveal, next-floor guidance, a moving guide light and optional nearby pickup."
lang: en
permalink: /features/
---

# Exploration helper — v0.4.0 preview

Use the game's M map to choose a destination or follow the default route to the next floor. A moving light travels ahead along the route and leaves a short glow trail. It fades with distance and when terrain blocks the view.

The owner confirmed the settings toggles, map display, destination selection and right-click return to next-floor guidance. The moving light and fade were also confirmed. Nearby pickup and route/map refresh after changing floors were confirmed. Automatic movement remains under review.

## Controls

| Control | Action |
|---|---|
| F8 | Open or close the Korean settings panel |
| F9 | Toggle unexplored-area display on the existing map |
| M | Open the game's map |
| Left-click the M map | Choose a destination while route guidance is enabled |
| Right-click the M map | Clear your chosen destination and return to next-floor guidance; stop movement |
| F10 | Start or stop movement along a complete route |
| Esc | Stop movement and close the settings panel |

All new features start disabled. Enable them individually in F8 and close the panel to interact with the M map. Guidance selects the next-floor exit by default. A selected destination takes priority until you right-click or change floors. When a new floor loads, the map refreshes and guidance returns to that floor’s exit.

## Guide light and map routes

The light moves along the route ahead of the player, including turns and stairs. Distance reduces its opacity, and walls or terrain gradually hide it. Complete routes use a warm gold light; partial routes use amber. The M map also shows the route: green for complete, orange for partial.

Clicking inside a room uses that room's elevation. A nearby chest, exit or boss marker can become the destination. A partial or unreachable route cannot start automatic movement.

## Automatic movement

Start each journey explicitly with F10 or the panel's start button. The map closes when movement starts. Manual movement, combat, damage, menus, death or loss of focus interrupts it. If no progress is made for 2.5 seconds, movement stops.

Closed doors require your interaction. This feature does not fight enemies or open doors for you.

## Pickup and map reveal

Nearby pickup uses a 3m default radius, configurable from 1–4m. It applies to eligible nearby loot and respects the game's weight, filters and pickup delay. It does not pursue distant items.

Map reveal applies to the existing map and minimap. Enemy markers show currently loaded enemies. Exploration records are preserved, and achievements retain their existing game behaviour.

## Report a problem

Include the affected control, the F8 status message and relevant lines from `MelonLoader/Latest.log` in an [issue](https://github.com/myso-kr/fell-and-sell-mod/issues).

[Installation](INSTALLATION.md) · [Troubleshooting](TROUBLESHOOTING.md) · [한국어 조작 안내](ko/EXPANSION.md)
