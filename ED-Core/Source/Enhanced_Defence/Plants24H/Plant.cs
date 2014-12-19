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

        private int ticksSinceLit;


        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.LookValue<int>(ref this.ticksSinceLit, "ticksSinceLit", 0, false);
        }

        public override void TickRare()
        {
            this.CheckTemperatureMakeLeafless();
            if (!GenPlant.GrowthSeasonNow(this.Position))
                return;
            if (!this.HasEnoughLightToGrow)
                this.ticksSinceLit += 250;
            else
                this.ticksSinceLit = 0;
            if (this.GrowingNow)
            {
                this.growthPercent += this.GrowthPerTickRare * this.TemperatureEfficiency;
                if (this.LifeStage == PlantLifeStage.Mature)
                {
                    Find.MapDrawer.MapChanged(this.Position, MapChangeType.Things);
                    if ((double)Find.Map.Biome.CommonalityOfPlant(this.def) == 0.0)
                        SoundStarter.PlayOneShot(Plant.SoundHarvestReady, (SoundInfo)this.Position);
                }
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

        private float GrowthPerTick
        {
            get
            {
                return (float)((double)this.LocalFertility * (double)this.def.plant.fertilityFactorGrowthRate + (1.0 - (double)this.def.plant.fertilityFactorGrowthRate)) * (this.def.plant.growthPer20kTicks / 20000f);
            }
        }

        private float GrowthPerTickRare
        {
            get
            {
                return this.GrowthPerTick * 250f;
            }
        }

        private void CheckTemperatureMakeLeafless()
        {
            float num = 8f;
            if ((double)GridsUtility.GetTemperature(this.Position) >= (double)Gen.HashOffset((Thing)this) * 0.00999999977648258 % (double)num - (double)num - 2.0)
                return;
            this.MakeLeafless();
        }

        private bool HasEnoughLightToGrow
        {
            get
            {
                return Find.GlowGrid.PsychGlowAt(this.Position) >= this.def.plant.growMinGlow;
            }
        }

        private float LocalFertility
        {
            get
            {
                return Find.FertilityGrid.FertilityAt(this.Position);
            }
        }


        public bool GrowingNow
        {
            get
            {
                Log.Message("Custom GrowingNow()");
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


        public override string GetInspectString()
        {
            StringBuilder stringBuilder1 = new StringBuilder();
            //stringBuilder1.Append(base.GetInspectString());
            //stringBuilder1.AppendLine();
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
                        stringBuilder1.AppendLine(Translator.Translate("NotGrowingNowResting"));
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
                    float temperatureEfficiency = this.TemperatureEfficiency;
                    if (!Mathf.Approximately(temperatureEfficiency, 1f))
                    {
                        if (Mathf.Approximately(temperatureEfficiency, 0.0f))
                        {
                            stringBuilder1.AppendLine(Translator.Translate("OutOfIdealTemperatureRangeNotGrowing"));
                        }
                        else
                        {
                            StringBuilder stringBuilder3 = stringBuilder1;
                            string key2 = "OutOfIdealTemperatureRange";
                            object[] objArray2 = new object[1];
                            int index2 = 0;
                            string str2 = Mathf.RoundToInt(temperatureEfficiency * 100f).ToString();
                            objArray2[index2] = (object)str2;
                            string str3 = Translator.Translate(key2, objArray2);
                            stringBuilder3.AppendLine(str3);
                        }
                    }
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
    }
}
