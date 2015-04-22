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
        private static readonly Graphic GraphicSowing = GraphicDatabase.Get<Graphic_Single>("Things/Plant/Plant_Sowing", ShaderDatabase.Cutout, IntVec2.One, Color.white);
        private int madeLeaflessTick = -99999;
        private List<int> posIndexList = new List<int>();
        private Color32[] workingColors = new Color32[4];
        private const float RotDamagePerTick = 0.005f;
        private const int MinPlantYield = 2;
        private const float GridPosRandomnessFactor = 0.3f;
        private const int TicksWithoutLightBeforeRot = 450000;
        private const int LeaflessMinRecoveryTicks = 60000;
        private const float MinLeaflessTemperature = -10f;
        private const float MinAnimalEatPlantsTemperature = 0.0f;
        private int unlitTicks;

        private bool Resting
        {
            get
            {
                return false;
            }
            /*
            get
            {
                if ((double)GenDate.CurrentDayPercent >= 0.25)
                    return (double)GenDate.CurrentDayPercent > 0.800000011920929;
                else
                    return true;
            }*/
        }

        private float GrowthPerTick
        {
            get
            {
                if (this.LifeStage != PlantLifeStage.Growing || this.Resting)
                    return 0.0f;
                else
                    return (float)(1.0 / (30000.0 * (double)this.def.plant.growDays)) * this.GrowthRate;
            }
        }

        private void CheckTemperatureMakeLeafless()
        {
            float num = 8f;
            if ((double)GridsUtility.GetTemperature(this.Position) >= (double)Gen.HashOffset((Thing)this) * 0.00999999977648258 % (double)num - (double)num - 2.0)
                return;
            this.MakeLeafless();
        }

        new public void MakeLeafless()
        {
            bool flag = !this.LeaflessNow;
            this.madeLeaflessTick = Find.TickManager.TicksGame;
            if (this.def.plant.dieIfLeafless)
                this.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 99999, (Thing)null, new BodyPartDamageInfo?(), (ThingDef)null));
            if (!flag)
                return;
            Find.MapDrawer.MapChanged(this.Position, MapChangeType.Things);
        }

        private bool HasEnoughLightToGrow
        {
            get
            {
                return (double)this.GrowthRateFactor_Light > 1.0 / 1000.0;
            }
        }

        private void NewlyMatured()
        {
            if (!this.CurrentlyCultivated())
                return;
            Find.MapDrawer.MapChanged(this.Position, MapChangeType.Things);
        }

        private bool CurrentlyCultivated()
        {
            if (!this.def.plant.Sowable)
                return false;
            Zone zone = Find.ZoneManager.ZoneAt(this.Position);
            if (zone != null && zone is Zone_Growing)
                return true;
            Building edifice = GridsUtility.GetEdifice(this.Position);
            return edifice != null && edifice.def.building.SupportsPlants;
        }

        public override void TickRare()
        {
            this.CheckTemperatureMakeLeafless();
            if (!GenPlant.GrowthSeasonNow(this.Position))
                return;
            if (!this.HasEnoughLightToGrow)
                this.unlitTicks += 250;
            else
                this.unlitTicks = 0;
            bool flag = this.LifeStage == PlantLifeStage.Mature;
            this.growth += this.GrowthPerTick * 250f;
            if (!flag && this.LifeStage == PlantLifeStage.Mature)
                this.NewlyMatured();
            if (this.def.plant.LimitedLifespan)
            {
                this.age += 250;
                if (this.Rotting)
                {
                    int amount = Mathf.CeilToInt(1.25f);
                    this.TakeDamage(new DamageInfo(DamageDefOf.Rotting, amount, (Thing)null, new BodyPartDamageInfo?(), (ThingDef)null));
                }
            }
            if (this.Destroyed)
                return;
            GenPlantReproduction.TickReproduceFrom(this);
        }

    }
}
