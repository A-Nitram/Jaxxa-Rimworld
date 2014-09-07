Jaxxa Shields

###About:
This mod allows you to place shield generators. They are expensive and power hungry but can really strengthen your defences. 
The shields will stop projectiles that try to enter it, but allow weapons to be fired out.

###How to use:
To install place the directory called "Jaxxa_Shields" (that contains the folders About,Assemblies,Defs,Sounds,Source,Textures) into your Mods folder.
Then activate from the mods option on the main menu screen.
After activating the mod and starting to play the first thing you will have to do is the research "Basic shield generator" this will unlock the "Basic shield" building.

After placing a Shield and providing it with enough power it will enter a warmup state. During this time there will be a countdown timer showing you how long until the shield is activated (if you select the shield).

After this time has elapsed the shield will activate showing a circle on the screen that shows where is covered by the shield.
This shield will stop all projectiles that enter it, but allow you to fire out from within the shield. The shield will start with a certain amount of health and will increase until it reaches it's maximum health. This will be reduced whenever the shield is used to block a projectile. If the shield is reduced to zero it will have to compleate the warmup phase again before it can block anything.

###Details:
There are 4 Shields in this version of the Mod.

Basic shield generator:
This the standard shield generator and is almost identical to the only shield in Darker's original version of the mod.

Small shield:
This shield required additional research.
It is smaller in size, lower strength, uses less power and allows power to stand on it.

Phalanx CRAM shield:
This shield has been modified to only block indirect fire weapons. It will protect a large area from Mortars attack, but will allow bullets to pass through in any direction.

SIF Generator:
Rather than creating a large circular shield this generator will create an small individual shield over each wall/door object.

###Statistics / Modding:

One of the main modifications that I made was making information about the shield to be read in from the XLM files rather than hard coded into the C# code. Because of this it should be easy to make your own shield generator of any size or strength.

The new XML variables that I have added that will be needed by shields are:

shieldMaxShieldStrength: Maximum shield strength
shieldInitialShieldStrength:Shield strength when going online
shieldShieldRadius:Radius of the Shield
shieldPowerRequiredLoading:The power used during Warmup
shieldPowerRequiredCharging:The power when the shield is up but not at full strength
shieldPowerRequiredSustaining:The power used when the shield is up and at full strength
shieldBlockIndirect:Should it block projectiles that fly overhead
shieldBlockDirect:Should it block projectiles that fly direct
shieldFireSupression:Should it put out fires (in squares that it is blocking)
shieldStructuralIntegrityMode:Should it only work on walls
shieldRechargeTickDelay:How many ticks between recharging the shield by 1 (lower number will recharge faster)
shieldRecoverWarmup:How long to wait before starting to charge
colourRed:Float value, Colour between 0 and 1
colourGreen:Float value, Colour between 0 and 1
colourBlue:Float value, Colour between 0 and 1

###ChangeLog:
0.01
	- Updating Darker's mod to fix a bug in Alpha 4
0.02
	- Updating Darker's mod to Alpha 5
0.03
	- Main rewrite and move to my own mod, many changes
0.04
	- Increasing Strength of CRAM shield
	- Separate variables for shieldBlockIndirect and shieldBlockDirect
	- Fire suppression ability.
	- Added a shield that will protect squares that have walls / doors on them rather than a circular area
0.05
	- Fix for not leaving debris when destroyed
0.06
	- Alpha 6 Update 
	- Renaming MyThingDef to ShieldThingDef
	- Shields will now leaveResourcesWhenKilled
	- Fixed Typo in research for Shields SIF Generator
	- Switchable Shield Options
		- Enable/Disable Direct / Indirect / Fire from ingame
		- Only if the shield supports it.
	- Enhanced fire suppression
		- Will drain shields and put fires out over time
		- Will work in the shield radius, not just the edge anymore
		- SIF shields will put out fires on the wall / doors
	- XML based colour
		- Shield colour is now defined in the XML files rather than hard coded
0.07
	- Added Repair mods to SIF shield
		- A counter to walls being damaged before the shot gets intercepted.
	- Coding enhancements to work more reliably and predictably
0.08
	- Fix for shield strength not being charged on loading a save game
	
Planned:
	-Early Warning?
	-Kenetic CRAM?
	-Check / deal with missing values better
	
###Background:
This is based of the Shields mod by "Darker" http://ludeon.com/forums/index.php?topic=2677.msg24772#msg24772 that is no longer being supported by him. I did a fix for Alpha4 and updated it to Alpha5 and have now developed to the point where I consider it a separate spin-off mod.

###Credit/Legal:
Credit to "Darker" for making the original mod, and thanks for allowing anything to be done with it.
http://ludeon.com/forums/index.php?topic=2677.msg24772#msg24772

Thanks to mrofa for providing code in "Clutter Mod" that was helpful to look at for figuring out better ways to do Fire Suppression and Command Switches

This work is licensed under http://creativecommons.org/licenses/by/4.0/

--Jaxxa