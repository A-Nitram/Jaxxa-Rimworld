using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using UnityEngine;
using Verse;
using RimWorld;

namespace Enhanced_Defence.TurretAmmo
{
    public class Building_TurretGun_Ammo : Building_TurretGun
    {
        CompPowerTrader power;
        public int previousburstCooldownTicksLeft = 0;

        public bool initialRun = false;

        /// <summary>
        /// This is the Type of Ammunition that this Turret Requires
        /// </summary>
        public string ammoType = "Shells";
        /// <summary>
        /// This is the amount of ammunition that this turret requires for each salvo
        /// </summary>
        public int ammoAmountUsedToFire = 1;
        /// <summary>
        /// Should the turret have an internal Ammunition supply
        /// </summary>
        public bool internalAmmoEnabled = true;
        /// <summary>
        /// The limit of the internal ammunition supply
        /// </summary>
        public int internalAmmoMAX = 100;
        /// <summary>
        /// Each time an ammoType object is taken from a hopper this is how much the internalAmmoCurrent should go up by, use this to allow 1 shell to fire the turret multiple times.
        /// </summary>
        public int internalAmmoMultiplier = 1;
        /// <summary>
        /// The starting amount of ammunition when it is built
        /// </summary>
        public int internalAmmoCurrent = 20;

        public int internalAmmoStartColonist = -1;

        //Only Run initially
        public override void PostMake()
        {
            //Log.Error("PostMake()");
            this.initialRun = true;
            base.PostMake();
        }

        //On spawn, get the power component reference
        public override void SpawnSetup()
        {
            //Log.Error("SpawnSetup()");
            //Load variables from XML
            if (def is TurretAmmoThingDef)
            {
                //Read in variables from the custom TurretAmmoThingDef
                this.ammoType = ((TurretAmmo.TurretAmmoThingDef)def).ammoType;
                this.ammoAmountUsedToFire = ((TurretAmmo.TurretAmmoThingDef)def).ammoAmountUsedToFire;
                this.internalAmmoEnabled = ((TurretAmmo.TurretAmmoThingDef)def).internalAmmoEnabled;

                this.internalAmmoMAX = ((TurretAmmo.TurretAmmoThingDef)def).internalAmmoMAX;
                this.internalAmmoMultiplier = ((TurretAmmo.TurretAmmoThingDef)def).internalAmmoMultiplier;
               //Log.Warning("Setting internalAmmoCurrent");

                this.internalAmmoStartColonist = ((TurretAmmo.TurretAmmoThingDef)def).internalAmmoStartColonist;

                if (this.initialRun)
                {
                    this.initialRun = false;


                    this.internalAmmoCurrent = ((TurretAmmo.TurretAmmoThingDef)def).internalAmmoCurrent;

                    if (this.Faction.Equals(Faction.OfColony))
                    {

                        //Log.Message("Setting to colonist custom ammo");
                        if (this.internalAmmoStartColonist != 0)
                        {
                            this.internalAmmoCurrent = this.internalAmmoStartColonist;
                            if (this.internalAmmoCurrent < 0)
                            {
                                this.internalAmmoCurrent = 0;
                            }
                        }
                    }
                }
            }
            base.SpawnSetup();
            this.power = base.GetComp<CompPowerTrader>();
        }

        public override void Tick()
        {
            //Log.Warning("Warn test");
            //Log.Error("Error test");
            //Log.Message("Message Test");

            if (this.internalAmmoEnabled)
            {
                if (this.internalAmmoMAX - this.internalAmmoCurrent >= (this.internalAmmoMultiplier))
                {
                    if (AmmoInHopper != null)
                    {
                        AmmoInHopper.SplitOff(this.ammoAmountUsedToFire);

                        this.internalAmmoCurrent += this.internalAmmoMultiplier;
                    }
                }
            }


            //If it just fired
            if (this.previousburstCooldownTicksLeft < this.burstCooldownTicksLeft)
            {
                if (!internalAmmoEnabled)
                {
                    if (AmmoInHopper != null)
                    {
                        AmmoInHopper.SplitOff(this.ammoAmountUsedToFire);
                        //FoodInHopper.stackCount -= 1;

                        // if (FoodInHopper.stackCount <= 0)
                        // {

                        //}
                    }
                }
                else
                {
                    this.internalAmmoCurrent -= this.ammoAmountUsedToFire;
                }
            }


            //Shutdown if no Ammo
            if (!HasAmmoToFire())
            {
                if (this.def.building.turretBurstCooldownTicks > 1)
                {
                    this.burstCooldownTicksLeft = this.def.building.turretBurstCooldownTicks;
                }
                else
                {
                    this.burstCooldownTicksLeft = 1;
                }

                if (this.power != null)
                {
                    power.DesirePowerOn = false;
                }
            }

            this.previousburstCooldownTicksLeft = this.burstCooldownTicksLeft;
            base.Tick();
        }
        //Saving game
        public override void ExposeData()
        {
            //Log.Error("ExposeData()");
            base.ExposeData();

            Scribe_Values.LookValue<string>(ref this.ammoType, "ammoType", "", false);
            Scribe_Values.LookValue<int>(ref this.ammoAmountUsedToFire, "ammoAmountUsedToFire", 0, false);
            Scribe_Values.LookValue<bool>(ref this.internalAmmoEnabled, "internalAmmoEnabled", false, false);

            Scribe_Values.LookValue<int>(ref this.internalAmmoMAX, "internalAmmoMAX", 0, false);
            Scribe_Values.LookValue<int>(ref this.internalAmmoMultiplier, "internalAmmoMultiplier", 0, false);
            Scribe_Values.LookValue<int>(ref this.internalAmmoCurrent, "internalAmmoCurrent", 0, false);

            // Scribe_Deep.LookDeep(ref shield, "shield");
            // shield.position = base.Position;

            // Scribe_Values.LookValue(ref ticksSinceExpand, "ticksSinceExpand");
            // Scribe_Values.LookValue(ref timesExpanded, "timesExpanded");
            //Scribe_Values.LookValue<int>(ref this.heat, "turretHeat", 0, false);
        }

        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            //Power info
            if (power != null)
            {
                //string str1 = (this.PowerNet.CurrentEnergyGainRate() / CompPower.WattsToWattDaysPerTick).ToString("F0");
                stringBuilder.AppendLine("Power: " + -power.powerOutput + " / " + (power.PowerNet.CurrentEnergyGainRate() / CompPower.WattsToWattDaysPerTick) + " - Stored: " + power.PowerNet.CurrentStoredEnergy().ToString("F0"));
            }

            stringBuilder.AppendLine("Ammo Type: " + this.ammoAmountUsedToFire + " " + this.ammoType);

            if (this.internalAmmoEnabled)
            {
                stringBuilder.Append("Internal Ammo: " + this.internalAmmoCurrent + @" / " + this.internalAmmoMAX + " - ");



                if (this.AmmoInHopper != null)
                {
                    if (AmmoInHopper.stackCount > 0)
                    {
                        stringBuilder.Append("Ammo Available");
                    }
                    else
                    {
                        if (this.internalAmmoCurrent >= this.ammoAmountUsedToFire)
                        {
                            stringBuilder.Append("Internal Ammo Only");
                        }
                        else
                        {
                            stringBuilder.Append("No Ammo");
                        }
                    }
                }
                else
                {
                    if (this.internalAmmoCurrent >= this.ammoAmountUsedToFire)
                    {
                        stringBuilder.Append("Internal Ammo Only");
                    }
                    else
                    {
                        stringBuilder.Append("No Ammo");
                    }
                }
            }
            else
            {
                if (this.AmmoInHopper != null)
                {
                    if (AmmoInHopper.stackCount > 0)
                    {
                        stringBuilder.Append("Ammo Available");
                    }
                    else
                    {
                        stringBuilder.Append("No Ammo");
                    }
                }
                else
                {
                    stringBuilder.Append("No Ammo");
                }
            }
            stringBuilder.AppendLine("");

            //stringBuilder.Append(base.GetInspectString());
            //if (this.AnyAdjacentHopper != null)


            //Range
            stringBuilder.AppendLine("Range:" + this.gun.PrimaryVerb.verbProps.minRange + " to " + this.gun.PrimaryVerb.verbProps.range);


            if (this.burstCooldownTicksLeft > 0)
            {
                stringBuilder.AppendLine(Translator.Translate("CanFireIn") + ": " + GenTime.TickstoSecondsString(this.burstCooldownTicksLeft));
            }

            //stringBuilder.Append(this.burstCooldownTicksLeft);

            //stringBuilder.Append(base.GetInspectString());

            return stringBuilder.ToString();
        }

        public bool HasAmmoToFire()
        {
            if (internalAmmoEnabled)
            {
                if (internalAmmoCurrent >= ammoAmountUsedToFire)
                {
                    //Log.Message("internalAmmoCurrent >= ammoAmountUsedToFire");
                    return true;
                }
            }
            else
            {
                if (AmmoInHopper != null)
                {
                    if (AmmoInHopper.stackCount >= this.ammoAmountUsedToFire)
                    {
                        //Log.Message("AmmoInHopper.stackCount >= this.ammoAmountUsedToFire");
                        return true;
                    }
                }
            }

            return false;
        }

        //TODO: Move this to Utils
        //Adapted from Building_NutrientPasteDispenser
        public Building AnyAdjacentHopper
        {
            get
            {
                ThingDef thingDef = ThingDef.Named("AutoLoader");
                foreach (IntVec3 loc in GenAdj.CellsAdjacentCardinal((Thing)this))
                {
                    Thing building = Find.ThingGrid.ThingAt(loc, thingDef);
                    if (building != null && building.def == thingDef)
                    {
                        return (Building)building;
                    }
                }
                return (Building)null;
            }
        }

        //TODO: Move this to Utils
        //Adapted from Building_NutrientPasteDispenser
        private Thing AmmoInHopper
        {
            get
            {
                ThingDef thingDefHopper = ThingDef.Named("AutoLoader");
                //ThingDef thingDefAmmoType = ThingDef.Named("Shells");
                ThingDef thingDefAmmoType = ThingDef.Named(this.ammoType);

                foreach (IntVec3 sq in GenAdj.CellsAdjacentCardinal((Thing)this))
                {
                    Thing thingAmmo = (Thing)null;
                    Thing thingContainer = (Thing)null;
                    foreach (Thing tempThing in Find.ThingGrid.ThingsAt(sq))
                    {
                        if (tempThing.def == thingDefAmmoType)
                        {
                            thingAmmo = tempThing;
                        }
                        if (tempThing.def == thingDefHopper)
                        {
                            thingContainer = tempThing;
                        }
                    }
                    if (thingAmmo != null && thingContainer != null && thingAmmo.stackCount >= this.ammoAmountUsedToFire)
                    {
                        return thingAmmo;
                    }
                }
                return (Thing)null;
            }
        }
    }
}
