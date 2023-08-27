# Clears Tracker
(formerly Raid Clears)

Add a small grid to the screen showing raid encounter weekly clear status.

Requires an active API token for account+progression.


Key Features
- Configurable size
- Label options for the wings (number, abbriv, none)
- Horizontal or Vertical layouts (New -Single Row layout)
- Option to hide wings from the display.
- Optional mouse over to read boss/wing names
- Opacity/Transparency settings
- Optional Corner Icon next to Blish to hide/show the clears screen
- Keybind support to hide/show the clears screen
- Selectable API polling rate


## Change Log

### 1.1.2 (2022-07-22)
* Fixed Opacity settings not being applied at module load
* Fixed FontSize scaling by changing label widths and limiting available sizes
* Added ApiPollPeriod setting to increase or decrease polling rate

### 1.1.1 (2022-07-21)
* Added IsInGame and MapOpen filter checks to hide the panel properly

### 1.1.0 (2022-07-21)
* Rewrote using Eksofa's SDK style repo.

### 1.2.0 (2022-07-31)
* Added Dunegon tracking

### 1.3.0 (2022-10-23)
* Added Call of the Mists and Emboldened

### 1.4.0 (2022-12-31)
* Added color customization

### 1.4.1 (2023-01-11)
* Settings are expanded by default

### 2.0.0 (2023-02-01)
* Rewrite
* Added strike functionality
### 2.0.1 (2023-02-12)
* version bump for 2.0.0 full release
### 2.0.2 (2023-02-13)
* Fixed keybinds not activating
### 2.0.3 (2023-02-26)
* Fixed race condition on 
### 2.0.4 (2023-03-01)
* Fixed daily strike index using Anets day of year index calculation
### 2.0.5 (2023-03-19)
* Added Wing 8 for april fools joke
### 2.1.0 (2023-04-20)

### 2.2.0 (2023-06-28)
* Added Silent Surf support
### 2.2.1 (2023-07-12)
* Finalized Silent Surf fractal changes
* Added strike and fractal clear management setting page.
### 2.2.2 (2023-07-30)
* Added Dragonstorm to IBS strikes list
### 2.2.3 (2023-08-22)
* Added Soto fix patch for blish corner icon offset
### 2.3.0 (2023-08-24)
* Added SotO Strike missions
---
Heavily inspired by the same feature bundled with Gw2TaCO.
