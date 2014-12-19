using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using UnityEngine;
using RimWorld;
using Verse.Sound;
using System.Reflection;

namespace Enhanced_Defence.Plants24H
{
    public class Plant : RimWorld.Plant
    {
        //int local_ticksSinceLit;

        FieldInfo field_ticksSinceLit;

        public override void SpawnSetup()
        {
            /*Plant tempPlant = this;
            Log.Message("Setting up");
            object returnValue = Enhanced_Defence.ShieldUtils.ReflectionHelper.GetInstanceField(typeof(Plant), tempPlant, "ticksSinceLit") as int;
            Log.Message("Got Object");
            local_ticksSinceLit = (int)returnValue;
            Log.Message("Assigned Object");*/

            //Log.Message("Setting up");
            field_ticksSinceLit = typeof(RimWorld.Plant).GetField("ticksSinceLit", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

            //Log.Message("SetUp");
            //local_ticksSinceLit = (int)fieldInfo.GetValue(this);
            
            base.SpawnSetup();
        }

        public override void TickRare()
        {
            //Log.Error("local_ticksSinceLit:" + (int)this.field_ticksSinceLit.GetValue(this));
            //base.TickRare();
            
            if (!this.HasEnoughLightToGrow)
            {
                //this.ticksSinceLit += 250;
                this.incrimentTicksSinceLit();
            }
            else
            {
                //this.ticksSinceLit = 0;
                this.clearTicksSinceLit();
            }
            if (this.GrowingNow)
            {
                this.growthPercent += this.GrowthPerTickRare;
                if (this.LifeStage == PlantLifeStage.Mature && (double)Find.Map.Biome.CommonalityOfPlant(this.def) == 0.0)
                    SoundStarter.PlayOneShot(Plant.SoundHarvestReady, (SoundInfo)this.Position);
            }
            if (this.def.plant.LimitedLifespan)
            {
                this.age += 250;
                if (this.Rotting)
                {
                    int amount = Mathf.CeilToInt(1.25f);
                    this.TakeDamage(new DamageInfo(DamageTypeDefOf.Rotting, amount, (Thing)null, new BodyPartDamageInfo?(), (ThingDef)null));
                }
            }
            if (this.Destroyed)
                return;
            GenPlantReproduction.TickReproduceFrom(this);
        }

        private void incrimentTicksSinceLit()
        {
            int temp = (int)this.field_ticksSinceLit.GetValue(this);
            temp += 250;
            this.field_ticksSinceLit.SetValue(this, temp);
        }
        private void clearTicksSinceLit()
        {
            this.field_ticksSinceLit.SetValue(this, 0);
        }
        
        public override string GetInspectString()
        {
            StringBuilder stringBuilder1 = new StringBuilder();
            //stringBuilder1.Append(base.GetInspectString());
            stringBuilder1.AppendLine();
            StringBuilder stringBuilder2 = stringBuilder1;
            string key1 = "PercentGrowth";
            object[] objArray1 = new object[1];
            int index1 = 0;
            string growthPercentString = this.GrowthPercentString;
            objArray1[index1] = (object)growthPercentString;
            string str1 = Translator.Translate(key1, objArray1);
            stringBuilder2.AppendLine(str1);
            if (this.LifeStage != PlantLifeStage.Sowing)
            {
                if (this.LifeStage == PlantLifeStage.Growing)
                {
                    if (!this.HasEnoughLightToGrow)
                    {
                        StringBuilder stringBuilder3 = stringBuilder1;
                        string key2 = "NotGrowingNow";
                        object[] objArray2 = new object[1];
                        int index2 = 0;
                        string str2 = GlowUtility.HumanName(this.def.plant.growMinGlow).ToLower();
                        objArray2[index2] = (object)str2;
                        string str3 = Translator.Translate(key2, objArray2);
                        stringBuilder3.AppendLine(str3);
                    }
                    else if (!this.GrowingNow)
                        stringBuilder1.AppendLine(Translator.Translate("NotGrowingNowNight"));
                    else
                        stringBuilder1.AppendLine(Translator.Translate("Growing"));
                    StringBuilder stringBuilder4 = stringBuilder1;
                    string key3 = "FullyGrownIn";
                    object[] objArray3 = new object[1];
                    int index3 = 0;
                    string str4 = GenTime.TickstoDaysString(this.TicksUntilFullyGrown);
                    objArray3[index3] = (object)str4;
                    string str5 = Translator.Translate(key3, objArray3);
                    stringBuilder4.AppendLine(str5);
                }
                else if (this.LifeStage == PlantLifeStage.Mature)
                {
                    if (this.def.plant.Harvestable)
                        stringBuilder1.AppendLine(Translator.Translate("ReadyToHarvest"));
                    else
                        stringBuilder1.AppendLine(Translator.Translate("Mature"));
                }
            }
            return stringBuilder1.ToString();
        }

        public bool GrowingNow
        {
            get
            {
                //if (this.LifeStage == PlantLifeStage.Growing && this.HasEnoughLightToGrow && (double)GenDate.CurFullDayPercent > 0.25)
                if (this.LifeStage == PlantLifeStage.Growing && this.HasEnoughLightToGrow)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool HasEnoughLightToGrow
        {
            get
            {
                return Find.GlowGrid.PsychGlowAt(this.Position) >= this.def.plant.growMinGlow;
            }
        }

        private float GrowthPerTickRare
        {
            get
            {
                return this.GrowthPerTick * 250f;
            }
        }

        private int TicksUntilFullyGrown
        {
            get
            {
                if ((double)this.growthPercent > 0.999899983406067)
                    return 0;
                else
                    return (int)((1.0 - (double)this.growthPercent) / (double)this.GrowthPerTick);
            }
        }

        private float GrowthPerTick
        {
            get
            {
                return (float)((double)this.LocalFertility * (double)this.def.plant.fertilityFactorGrowthRate + (1.0 - (double)this.def.plant.fertilityFactorGrowthRate)) * (this.def.plant.growthPer20kTicks / 20000f);
            }
        }

        private float LocalFertility
        {
            get
            {
                return Find.FertilityGrid.FertilityAt(this.Position);
            }
        }

        private string GrowthPercentString
        {
            get
            {
                float num = this.growthPercent * 100f;
                if ((double)num > 100.0)
                    num = 100.1f;
                return num.ToString("##0");
            }
        }
    }
}
