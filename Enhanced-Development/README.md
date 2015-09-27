Enhanced Development
======================

##Overview:
This mod is designed with a number of distinct modules, that I think improve upon or add new features to Rimworld. Each of these are self-contained as much as is practical, ideally only implementing a single feature or system. These are designed to allow you to customise your experience by only enabling the parts of the mod that fit with your play style. 

Please view the included manual for information on how to set these up and what each one does.

##Links:
Discussion and Latest Download available on the Official Rimworld forums at:  
https://ludeon.com/forums/index.php?topic=6636.0

With a Download Mirror on the Nexus:  
http://www.nexusmods.com/rimworld/mods/62/?

##ChangeLog:  
~~~
00.01.00
- Initial re-release as Enhanced Development.
00.02.00
- Update to Alpha 12D.
- Removed unneeded vent images.
- Initial version of Shielded Animals.
- Fix for Drop-pod Intercept.
- Increased Shield buildings max Shield strength.
- Unpowered Shields buildings will now discharge slowly.
~~~

##Old ChangeLog:  
This is the old change log from when this was Enhanced-Defence.
~~~
0.01  
- Initial Release  
0.02  
- Reduced Research Cost  
0.03  
- Changed Personal Shields to be Completely Standalone  
	- No research requirements for base shields  
	- Separate copies of the Textures  
- Added Embrasures  
- Update description of OmniGel Research
- Added Wireless power increase / decrease by 1000 options
0.04
- Fixed Wireless Power Node not loading icons correctly after the game has been reset.
- Fix to the starting Ammo of some types of Mortars
- Fixed what walls / doors can be covered by SIF Shields
0.05
- Fix for Memory leak relating to the Shield Display
- Added option to Hide Shield visual display
- Only one Laser Drill Can be active at a time
- Power carrying Embrasures.
0.06
- Fix for SIF Shield not restoring correctly when loading a saved game.
- Fix for potential null pointer errors relating to projectiles without a faction
- Changed OmniGel textures to use the ones provided by Omni
0.07
-Changes to Shields
--Standard / Small / Fortress / SIF Shields
-Allow Artillery to FlyOver Shields
-DropPod Intercept Mode
-Fix to Toggle of Visually Showing Shields not being restored on Save Load
-Shield Texture Updates
0.08
-Added ED-VisibleRadius module
--Allows you to keep the radius up for Trade beacons and SunLamps after deselecting them
-Rebalanced (Reduced) various research costs
-Added ED-WallConnect module
--Visual update to allow walls and natural rock to connect with each other
--Also applied to Embrasures (even without this module)
-Added ED-Plants24H module
--Plants will grow 24 Hours a day if they have sufficient lighting
--Effects PlantPotato, PlantStrawberry, PlantCotton, PlantDevilstrand, PlantRose, PlantDaylily
--Also changes OmniGel (even without this module)
-Fixed bug that allowed EMRG to spawn on mechanoids
-Fortress shield is now 2x2 in size
-SIF Shield should now function for any Wall and Door
-Droppod will not activate undercover
0.09
-Updated to Alpha8
-Removed ED-WallConnect
--Now implemented in new Alpha of base Game
0.10
-Removed VisibleRadius for TradeBeacons until it can work properly
-Fixed Omnigel not being processed into anything
-Added more things that can be made from Omnigel
--Balancing ideas welcome
0.11
-Added Vent System
--Shares graphics with cooler
--Power it on to equalise the temperature between two rooms
--Made out of Stuff, same health as a wall from that stuff
--Omnidirectional
--All the buttons except the power button do nothing
0.12
-Updated Graphic for Vent
-More specific install instructions
0.13
-Initial Stargate Release
-Moved Common Textures into Core Mod
--Mainly UI in modules that already require Core
0.14
-Fixes to Mortar turrets to bring them in line with Alpha 8 changes
-Increased Cost of Stargate
-Increased Stargate Power Consumption
-Added charge options
--Increase the power consumption of the Stargate for a reduction in charge time
-Removed Autoloader Research Requirement
--Would have been problematic to be used with offworld gates
-Stargate Dynamic Save location
--Change where the stargate .xml files are saved and read from
--Most people will want to leave this alone, but it might be helpful for trying more complex stuff or bug fixing.
-Updating / Balancing OmniGel recipes based on resource cost
0.15
-Redone vent logic
-increased stack amount for OmniGel
-Adds reverse cycle Cooler
--Identical to the standard cooler except that it can be freely rotated after being placed.
0.16
-Update to Alpha 9
-Removed "ED-MortarAmmo"
--Similar feature now implemented in the base game
-Removed "ED-EMRG turret"
--Was mainly a demo for turrets using Ammo and that is now in the base game
--Might come back later in some form.
-Increased size of Lazer Drill, brought in line with geothermal plant
-Reduced size of Laser Filler to better fill in tight spaces to remove useless geothermal vents.
-Reworking Personal Shields
--Will not require custom Pawns anymore 
--I got part way through updating this when Latta uploaded an awesome mod with a bunch of assets and code I could use.
---"Shooter's shield / Customizable personal shield" https://ludeon.com/forums/index.php?topic=10994.0
0.16.1
-Changes To personal Shields
--Renamed Personal shields to be "Nanite Shield Modulator"
---To better differentiate between stock shields / shields added in other mods.
--Removed tags from personal shields to stop pawns from spawning with them initially
---Not that it did them much good because they did not start charged.
--Changed layers Overhead, should now fit over armour
--Require Personal shields to be fully charged before being brought on-line
0.17.0
-Updating to Alpha 10
-Fixed Repair mode display bug
-Removed ED-VisibleRadius
--Now Sunlamps can create grow zones and Trade beacons can create stockpiles
---This removes the main use for this module
-Personal Shields Ignore Certain damage types:
--HealGlobal
--HealInjury
--Repair
--RestoreBodyPart
--SurgicalCut
0.17.1
-Fix to OmniGel giving potatoes
0.18.0
-Initial Alpha of Vehicle module
-Fix for Personal Shields
--Not recognising when charger is disabled
--Attempting to charge full shields
-Laser Filler now works on Area of Effect
-Removed EmbrasureConduit 
-SIF Shielded buildings are now read in from XML to be easily edited.
--Included by default are:
---Sandbags
---Wall
---Door
---Autodoor
---Embrasure
0.18.1
-Fix for error in loading / saving SIF shields
0.18.2
-Fix for Show Visually / Repair mode
0.19.0
-Update to Alpha 11
--Minus StarGate and Vehicles
0.19.1
-Changes to Visual studio output location
-Now include .dll in github repository
-Now building against Alpha 11B Binaries
-Removed some conflicting hotkeys
-Updating Stargate to Alpha11
1.00.00
-Refactoring code and renaming mod to Enhanced Development
--Code for individual modules will now be split up where logical
---This reduces the dependency of the ED-Core / EPE-Core module
--Documentation primarilly provided as  included manual in the download rather than on the github wiki
---Manual provided in .docx and .pdf format
-Increased work to build Embrasures
~~~

##License:
This work is licensed under http://creativecommons.org/licenses/by/4.0/

##Credit / Thanks:

Thanks to Alphathon for:
* Creating the Stargate graphic that I edited slightly
 * http://commons.wikimedia.org/wiki/File:Stargate.svg

Thanks to Architect for:
* Allowing the Code from BetterPower+ to be used.
 * Currently using the LaserDrill

Thanks to Darker for:
* making the original Shield mod, and thanks for allowing anything to be done with it.
 * http://ludeon.com/forums/index.php?topic=2677.msg24772#msg24772

Thanks to EdB for:
* Allowing people to look through his UI code.

Thanks to Haplo for:
* Providing a tutorial on animating images.
* Providing the MAI source code, was invaluable in figuring out how to work with Pawns for the initial personal shields and vehicles.

Thanks to Kulverstukass for:
* Awesome bug reporting and feedback.

Thanks to Latta for:
* Allowing people to use his personal shields mod.
 * https://ludeon.com/forums/index.php?topic=10994.0
 
Thanks to mrofa for:
* Providing code in "Clutter Mod" that was helpful to look at for figuring out better ways to do Fire Suppression and Command Switches
* Allowing me to use his art from the BetterPower+ Mod
 * Laser Drill
 * NuclearLamp used for Power Node  

Thanks to obuw for:
* Providing the base of some new icons

Thanks to Omni for:
* Providing new textures for the OmniGel system
 
Thanks to PunisheR007 for:
* Creating the Cannons and Turrets mod that is the basis for the Cannon Ammo and Turrets module (Unreleased)
 * http://ludeon.com/forums/index.php?topic=3878.0
* Creating the Embrasures  mod that is the basis for the Embrasures module  
 * http://ludeon.com/forums/index.php?topic=3961.0

Thanks to skullywag for:
* The use of his StunGun Graphic (unreleased)

--Jaxxa
