using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using UnityEngine;
using RimWorld;
using Verse.Sound;

namespace Enhanced_Defence.Plants24H
{
    public class Plant : RimWorld.Plant
    {
        private static readonly Graphic GraphicSowing = GraphicDatabase.Get_Single("Things/Plant/Plant_Sowing", ShaderDatabase.Cutout, Color.white);
        protected static readonly SoundDef SoundHarvestReady = SoundDef.Named("HarvestReady");
        public float growthPercent = 0.05f;
        private List<int> posIndexList = new List<int>();
        private Color32[] workingColors = new Color32[4];
        public const float BaseGrowthPercent = 0.05f;
        private const float RotDamagePerTick = 0.005f;
        private const int MinPlantYield = 2;
        private const float MaxAirPressureForDOT = 0.6f;
        private const float SuffocationMaxDOTPerTick = 0.01f;
        private const float GridPosRandomnessFactor = 0.3f;
        private const int TicksWithoutLightBeforeRot = 100000;
        public int age;
        private int ticksSinceLit;

        public bool HarvestableNow
        {
            get
            {
                if (this.def.plant.Harvestable)
                    return (double)this.growthPercent > (double)this.def.plant.harvestMinGrowth;
                else
                    return false;
            }
        }

        public override bool EdibleNow
        {
            get
            {
                return (double)this.growthPercent > (double)this.def.plant.harvestMinGrowth;
            }
        }

        public bool Rotting
        {
            get
            {
                if (this.ticksSinceLit > 100000)
                    return true;
                if (this.def.plant.LimitedLifespan)
                    return this.age > this.def.plant.lifeSpan;
                else
                    return false;
            }
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

        public override string LabelMouseover
        {
            get
            {
                StringBuilder stringBuilder1 = new StringBuilder();
                stringBuilder1.Append(this.def.LabelCap);
                StringBuilder stringBuilder2 = stringBuilder1;
                string str1 = " (";
                string key = "PercentGrowth";
                object[] objArray = new object[1];
                int index = 0;
                string growthPercentString = this.GrowthPercentString;
                objArray[index] = (object)growthPercentString;
                string str2 = Translator.Translate(key, objArray);
                string str3 = str1 + str2;
                stringBuilder2.Append(str3);
                if (this.Rotting)
                    stringBuilder1.Append(", " + Translator.Translate("DyingLower"));
                stringBuilder1.Append(")");
                return stringBuilder1.ToString();
            }
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

        public PlantLifeStage LifeStage
        {
            get
            {
                if ((double)this.growthPercent < 1.0 / 1000.0)
                    return PlantLifeStage.Sowing;
                return (double)this.growthPercent > 0.999000012874603 ? PlantLifeStage.Mature : PlantLifeStage.Growing;
            }
        }

        public override Graphic Graphic
        {
            get
            {
                if (this.LifeStage == PlantLifeStage.Sowing)
                    return Plant.GraphicSowing;
                else
                    return base.Graphic;
            }
        }

        public override void SpawnSetup()
        {
            base.SpawnSetup();
            for (int index = 0; index < this.def.plant.maxMeshCount; ++index)
                this.posIndexList.Add(index);
            GenList.Shuffle<int>((IList<int>)this.posIndexList);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.LookValue<float>(ref this.growthPercent, "growthPercent", 0.0f, false);
            Scribe_Values.LookValue<int>(ref this.age, "age", 0, false);
            Scribe_Values.LookValue<int>(ref this.ticksSinceLit, "ticksSinceLit", 0, false);
        }

        public override float Eaten(float nutritionWanted, Pawn eater)
        {
            this.PlantCollected();
            return this.def.food.nutrition;
        }

        public void PlantCollected()
        {
            if (this.def.plant.harvestDestroys)
            {
                this.Destroy(DestroyMode.Vanish);
            }
            else
            {
                this.growthPercent = 0.08f;
                Find.MapDrawer.MapChanged(this.Position, MapChangeType.Things);
            }
        }

        public override void TickRare()
        {
            if (!this.HasEnoughLightToGrow)
                this.ticksSinceLit += 250;
            else
                this.ticksSinceLit = 0;
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

        public int YieldNow()
        {
            if (!this.HarvestableNow)
                return 0;
            if ((double)this.def.plant.harvestYieldRange.max <= 1.0)
                return Mathf.RoundToInt(this.def.plant.harvestYieldRange.max);
            int num = Gen.RandomRoundToInt(this.def.plant.harvestYieldRange.LerpThroughRange(Mathf.InverseLerp(this.def.plant.harvestMinGrowth, 1f, this.growthPercent)) * Mathf.Lerp(0.5f, 1f, (float)this.health / (float)this.MaxHealth));
            if (num < 2)
                num = 2;
            return num;
        }

        public override void PrintOnto(SectionLayer layer)
        {
            Vector3 vector3 = Gen.TrueCenter((Thing)this);
            Rand.Seed = this.Position.GetHashCode();
            float max1 = this.def.plant.maxMeshCount != 1 ? 0.5f : 0.05f;
            int num1 = Mathf.CeilToInt(this.growthPercent * (float)this.def.plant.maxMeshCount);
            if (num1 < 1)
                num1 = 1;
            int num2 = 1;
            int num3 = this.def.plant.maxMeshCount;
            switch (num3)
            {
                case 1:
                    num2 = 1;
                    break;
                case 4:
                    num2 = 2;
                    break;
                default:
                    if (num3 != 9)
                    {
                        if (num3 != 16)
                        {
                            if (num3 == 25)
                            {
                                num2 = 5;
                                break;
                            }
                            else
                            {
                                Log.Error((string)(object)this.def + (object)" must have plant.MaxMeshCount that is a perfect square.");
                                break;
                            }
                        }
                        else
                        {
                            num2 = 4;
                            break;
                        }
                    }
                    else
                    {
                        num2 = 3;
                        break;
                    }
            }
            float num4 = 1f / (float)num2;
            Vector3 center1 = Vector3.zero;
            Vector2 size = Vector2.zero;
            int num5 = 0;
            int count = this.posIndexList.Count;
            for (int index = 0; index < count; ++index)
            {
                int num6 = this.posIndexList[index];
                float num7 = this.def.plant.visualSizeRange.LerpThroughRange(this.growthPercent);
                if (this.def.plant.maxMeshCount == 1)
                {
                    center1 = vector3 + new Vector3(Rand.Range(-max1, max1), 0.0f, Rand.Range(-max1, max1));
                    float num8 = Mathf.Floor(vector3.z);
                    if ((double)center1.z - (double)num7 / 2.0 < (double)num8)
                        center1.z = num8 + num7 / 2f;
                }
                else
                {
                    center1 = this.Position.ToVector3();
                    center1.y = this.def.Altitude;
                    center1.x += 0.5f * num4;
                    center1.z += 0.5f * num4;
                    int num8 = num6 / num2;
                    int num9 = num6 % num2;
                    center1.x += (float)num8 * num4;
                    center1.z += (float)num9 * num4;
                    float max2 = num4 * 0.3f;
                    center1 += new Vector3(Rand.Range(-max2, max2), 0.0f, Rand.Range(-max2, max2));
                }
                bool flag = (double)Rand.Value < 0.5;
                Material matSingle = this.def.graphic.MatSingle;
                this.workingColors[1].a = this.workingColors[2].a = (byte)((double)byte.MaxValue * (double)this.def.plant.topWindExposure);
                this.workingColors[0].a = this.workingColors[3].a = (byte)0;
                if (this.def.overdraw)
                    num7 += 2f;
                size = new Vector2(num7, num7);
                bool flipUv = flag;
                Printer_Plane.PrintPlane(layer, center1, size, matSingle, 0.0f, flipUv, (Vector2[])null, this.workingColors);
                ++num5;
                if (num5 >= num1)
                    break;
            }
            if (this.def.sunShadowInfo == null)
                return;
            float num10 = (double)size.y >= 1.0 ? 0.81f : 0.6f;
            Vector3 center2 = center1;
            center2.z -= size.y / 2f * num10;
            center2.y -= 0.04f;
            Printer_Shadow.PrintShadow(layer, center2, this.def.sunShadowInfo);
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

        public void CropBlighted()
        {
            if ((double)Rand.Value >= 0.850000023841858)
                return;
            this.Destroy(DestroyMode.Vanish);
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            base.Destroy(mode);
            Find.DesignationManager.RemoveAllDesignationsOn((Thing)this);
        }
    }
}
