using RimWorld;
using RimWorld.SquadAI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse.AI;
using Verse;

namespace Enhanced_Defence.PersonalShields
{
    public class ShieldedPawn : Verse.Pawn
    {
        public int max_shields = 100;
        public int currentShields = 100;
        public List<DamageMultiplier> AvalableShieldResistance;
        private bool baseShieldsActive = false;

        public ShieldedPawn()
            : base()
        {
            Log.Message("Creating ShieldedPawn");
        }


        public override void SpawnSetup()
        {
            Log.Message("SpawnSetup");
            base.SpawnSetup();

            //setupDamageMultiplier();
            setShieldsActive(true);
        }

        private void setupDamageMultiplier()
        {
            //Setup the list
            AvalableShieldResistance = new List<DamageMultiplier>();

            DamageMultiplier ArrowDamage = new DamageMultiplier();
            ArrowDamage.multiplier = 0.0f;
            ArrowDamage.damageDef = DamageTypeDefOf.Arrow;
            AvalableShieldResistance.Add(ArrowDamage);

            DamageMultiplier BiteDamage = new DamageMultiplier();
            BiteDamage.multiplier = 0.0f;
            BiteDamage.damageDef = DamageTypeDefOf.Bite;
            AvalableShieldResistance.Add(BiteDamage);

            DamageMultiplier BleedingDamage = new DamageMultiplier();
            BleedingDamage.multiplier = 0.0f;
            BleedingDamage.damageDef = DamageTypeDefOf.Bleeding;
            AvalableShieldResistance.Add(BleedingDamage);

            DamageMultiplier BluntDamage = new DamageMultiplier();
            BluntDamage.multiplier = 0.0f;
            BluntDamage.damageDef = DamageTypeDefOf.Blunt;
            AvalableShieldResistance.Add(BluntDamage);

            DamageMultiplier BombDamage = new DamageMultiplier();
            BombDamage.multiplier = 0.0f;
            BombDamage.damageDef = DamageTypeDefOf.Bomb;
            AvalableShieldResistance.Add(BombDamage);

            DamageMultiplier BreakdownDamage = new DamageMultiplier();
            BreakdownDamage.multiplier = 0.0f;
            BreakdownDamage.damageDef = DamageTypeDefOf.Breakdown;
            AvalableShieldResistance.Add(BreakdownDamage);

            DamageMultiplier BulletDamage = new DamageMultiplier();
            BulletDamage.multiplier = 0.0f;
            BulletDamage.damageDef = DamageTypeDefOf.Bullet;
            AvalableShieldResistance.Add(BulletDamage);

            DamageMultiplier CrushDamage = new DamageMultiplier();
            CrushDamage.multiplier = 0.0f;
            CrushDamage.damageDef = DamageTypeDefOf.Crush;
            AvalableShieldResistance.Add(CrushDamage);

            DamageMultiplier CutDamage = new DamageMultiplier();
            CutDamage.multiplier = 0.0f;
            CutDamage.damageDef = DamageTypeDefOf.Cut;
            AvalableShieldResistance.Add(CutDamage);

            DamageMultiplier EMPDamage = new DamageMultiplier();
            EMPDamage.multiplier = 0.0f;
            EMPDamage.damageDef = DamageTypeDefOf.EMP;
            AvalableShieldResistance.Add(EMPDamage);

            DamageMultiplier ExtinguishDamage = new DamageMultiplier();
            ExtinguishDamage.multiplier = 0.0f;
            ExtinguishDamage.damageDef = DamageTypeDefOf.Extinguish;
            AvalableShieldResistance.Add(ExtinguishDamage);

            DamageMultiplier FlameDamage = new DamageMultiplier();
            FlameDamage.multiplier = 0.0f;
            FlameDamage.damageDef = DamageTypeDefOf.Flame;
            AvalableShieldResistance.Add(FlameDamage);

            /*
            DamageMultiplier HealGlobalInjuryDamage = new DamageMultiplier();
            HealGlobalInjuryDamage.multiplier = 0.0f;
            HealGlobalInjuryDamage.damageDef = DamageTypeDefOf.HealGlobalInjury;
            AvalableShieldResistance.Add(HealGlobalInjuryDamage);

            DamageMultiplier HealLocalInjuryDamage = new DamageMultiplier();
            HealLocalInjuryDamage.multiplier = 0.0f;
            ArrowDamage.damageDef = DamageTypeDefOf.HealLocalInjury;
            AvalableShieldResistance.Add(HealLocalInjuryDamage);
             */

            DamageMultiplier MiningDamage = new DamageMultiplier();
            MiningDamage.multiplier = 0.0f;
            MiningDamage.damageDef = DamageTypeDefOf.Mining;
            AvalableShieldResistance.Add(MiningDamage);

            DamageMultiplier RottingDamage = new DamageMultiplier();
            RottingDamage.multiplier = 0.0f;
            RottingDamage.damageDef = DamageTypeDefOf.Rotting;
            AvalableShieldResistance.Add(RottingDamage);

            DamageMultiplier ScratchDamage = new DamageMultiplier();
            ScratchDamage.multiplier = 0.0f;
            ScratchDamage.damageDef = DamageTypeDefOf.Scratch;
            AvalableShieldResistance.Add(ScratchDamage);

            DamageMultiplier StabDamage = new DamageMultiplier();
            StabDamage.multiplier = 0.0f;
            StabDamage.damageDef = DamageTypeDefOf.Stab;
            AvalableShieldResistance.Add(StabDamage);

            DamageMultiplier StarvationDamage = new DamageMultiplier();
            StarvationDamage.multiplier = 0.0f;
            StarvationDamage.damageDef = DamageTypeDefOf.Starvation;
            AvalableShieldResistance.Add(StarvationDamage);

            DamageMultiplier StunDamage = new DamageMultiplier();
            StunDamage.multiplier = 0.0f;
            StunDamage.damageDef = DamageTypeDefOf.Stun;
            AvalableShieldResistance.Add(StunDamage);

            DamageMultiplier SuffocationDamage = new DamageMultiplier();
            SuffocationDamage.multiplier = 0.0f;
            SuffocationDamage.damageDef = DamageTypeDefOf.Suffocation;
            AvalableShieldResistance.Add(SuffocationDamage);

            /*
            DamageMultiplier SurgicalCutDamage = new DamageMultiplier();
            SurgicalCutDamage.multiplier = 0.0f;
            SurgicalCutDamage.damageDef = DamageTypeDefOf.SurgicalCut;
            AvalableShieldResistance.Add(SurgicalCutDamage);*/
        }

        public void setShieldsActive(bool newValue)
        {
            if (baseShieldsActive == newValue)
            {
                return;
            }

            if (newValue == true)
            {
                Log.Message("Change to True");
                baseShieldsActive = newValue;

                /*
                Log.Message("Check damageMultipliers start");
                if (this.def != null)
                {
                    if (this.def.damageMultipliers == null)
                    {
                        Log.Message("Check damageMultipliers");
                        this.def.damageMultipliers = new List<DamageMultiplier>();
                    }
                }

                Log.Message("Check AvalableShieldResistance start");
                if (this.AvalableShieldResistance != null)
                {
                    Log.Message("Check AvalableShieldResistance");
                    this.def.damageMultipliers.AddRange(AvalableShieldResistance);
                }*/

            }

            if (newValue == false)
            {
                Log.Message("Change to false");
                baseShieldsActive = newValue;

                /*if (this.AvalableShieldResistance != null)
                {
                    for (int i = 0; i < AvalableShieldResistance.Count; i++ )
                    {
                        this.def.damageMultipliers.Remove(AvalableShieldResistance[i]);
                    }
                }*/
            }
        }

        public bool getShieldActive()
        {
            return this.baseShieldsActive;
        }

        public bool getRequiresRecharge()
        {
            if (currentShields < max_shields)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void PreApplyDamage(DamageInfo dinfo)
        {
            //Log.Message("PreApplyDamage -> Resist:" + this.def.damageMultipliers.Count);
            Log.Message("dinfo: " + dinfo.Amount);
            //this.currentShieldResistance = temp;
            //this.def.damageMultipliers.Add(temp);
            //dinfo.ForceSetAmount(0);

            //Thing launcher = reflectionHelper.GetInstanceField(typeof(Projectile), pr, "launcher") as Thing;

            if (this.getShieldActive())
            {
                DebugSettings.playerDamageEnabled = false;

                currentShields -= dinfo.Amount;
                if (currentShields <= 0)
                {
                    currentShields = 0;
                    this.setShieldsActive(false);
                }
            }
            base.PreApplyDamage(dinfo);
        }

        public override void PostApplyDamage(DamageInfo dinfo)
        {
            //Log.Error("PostApplyDamage - Shield Took Hit" + dinfo.Amount);
            //this.healthTracker.bodyModel.healthDiffs.Clear();
            //this.health = this.MaxHealth;

            DebugSettings.playerDamageEnabled = true;

            /*
            if (this.Destroyed)
            {
                Log.Error("this.Destroyed");
            }

            if (this.getShieldActive())
            {
                Log.Message("AutoHealing");
                List<HealthDiff> temp = new List<HealthDiff>();
                temp.AddRange(this.healthTracker.bodyModel.healthDiffs);

                Log.Message("Removing: " + temp.Count);

                foreach (HealthDiff currentHealthDiff in temp)
                {
                    this.healthTracker.bodyModel.healthDiffs.Remove(currentHealthDiff);
                }
                this.health = this.MaxHealth;
            }
            else
            {
                base.PostApplyDamage(dinfo);
            }*/
            base.PostApplyDamage(dinfo);
        }

        public override void Tick()
        {
            /*IEnumerable<Thing> listOfThings = Find.

            Log.Message("Things:" + listOfThings.Count());

            foreach (Thing currentThing in listOfThings)
            {
                if (currentThing is Projectile)
                {
                    Log.Message("Found bullet");
                }
            }*/

            // Log.Message("Shielded Tick()");
            base.Tick();
        }

        public override string GetInspectString()
        {
            string status = "";

            if (this.baseShieldsActive)
            {
                status = "Online";
            }
            else
            {
                status = "Offline";
            }

            string output;

            output = "Shields " + status + ": " + currentShields + @" / " + max_shields + " - " + base.GetInspectString();
            return output;

        }

        public bool recharge(int chageAmmount)
        {
            if (this.currentShields < this.max_shields)
            {
                this.currentShields += chageAmmount;

                if (this.currentShields >= this.max_shields)
                {
                    this.setShieldsActive(true);
                    this.currentShields = this.max_shields;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void ExposeData()
        {
            Scribe_Values.LookValue<int>(ref currentShields, "currentShields");
            Scribe_Values.LookValue<int>(ref max_shields, "max_shields");
            Scribe_Values.LookValue<bool>(ref baseShieldsActive, "baseShieldsActive");

            Scribe_Collections.LookList<DamageMultiplier>(ref this.AvalableShieldResistance, "AvalableShieldResistance", LookMode.Deep, (object)null);

            base.ExposeData();
        }
    }
}
