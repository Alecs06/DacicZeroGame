## ui / preparation phase

* divided into 3 stages:
	* mission selector
	* character and dialog
	* weapons and upgrades

## 1. mission selector interface
* **title:** mission selector interface
* **description:** 
  implement the main interactive map workflow triggered from the central table, as well as the memory replay map for the robot philosopher.
* **components:**
  * **table interaction:** central table interactable object that opens fullscreen map.
  * **map navigation & precision:** 
    * wasd pinpoint navigation (or mouse-drag)
    * enter key confirms location selection.
    * proximity check: selection within tolerance threshold displays playable icon; invalid selection triggers warning feedback and returns player to table.
			**Q**: 'returns player to table' as in:
				*returns the player to the main interface where the table is located?* or
				*returns the player to within the table, where the mission selector is?*
  * **memory replay map (robot philosopher):**
    * eye projection effect displaying memory level selector.
    * allow replaying previously cleared levels (cannot gain resources from replays).
  * **mission launch screen:**
    * top-left overlay: mission details.
    * bottom overlay: selected weapon slots.
    * interactive play button to load the level/scene with the certain parameters.
	    * replay->bool
		* gear->?
		* upgrades->?

---

## 2. character & dialog system
* **title:** character & dialog system
* **description:** 
  abstract and create a modular npc system around the central table with custom dialog and interaction options, initialized with default dialog datasets.
* **components:**
  * **npc management & visibility:**
    * toggle states (`on` / `off`) to control world spawning and interactivity.
    * hover feedback effects (glow, border highlight)
  * **interaction framework:**
    * basic abstraction for character options and dialog flow.
	    * character:
		    * sprite
		    * options
			    * on hover: highlight the option. pressing enter chooses that option
			    * action (e.g start dialog, open interface)
			    * close?
    * ui options layout (when the player has to choose one of the options): 
	    * npc sprite on left
	    * options menu on right (left of player)
	    * player on right
  * **dialog engine:**
    * turn-based dialog view (left: npc, right: player, center: text message).
    * dynamic visibility: the speaking character is slightly brighter and highlighted
    * during dialog: the player can be prompted with selecting an *option* (use option for player's dialog lines so that actions can be done during the dialog -> like choosing a story path, opening a shop, receiving something, etc)
  * **data population:**
    * abstract character data structure to allow populating each npc with one default dialog setup.

---

## 3. weapon & upgrade interface
* **title:** weapon & upgrade interface
* **description:** 
  abstract weapon data and build the upgrade interface accessible via the tech guy npc, supporting loadout management for mission startup.
* **components:**
  * **weapon abstraction:**
    * data model for primary/secondary weapons and stats (default: bow).
    * loadout restrictions (2 primary slots, 2 secondary slots; max 2 equipped).
    * parameter passing to level creation pipeline.
  * **tech guy upgrade ui:**
    * **sidebar (left):**
	    * **header:** exit button (`x`) and npc/interface title and player resources
	    * contents: list of n available weapons in the current level.
    * **comparison view (center+right):** 
      * left panel: current weapon specs.
      * right panel: upgraded weapon specs (e.g., mk i / mk ii).
      * stats display sub-panels.
      * price tag overlay showing *( max(player resource / upgrade cost) / upgrade cost )*.
  * **navigation:* (implemented within the sidebar)
	  * ( x ): closes the upgrading interface
	  * interface/character title on the right of it
	  * player resources ( scrap )