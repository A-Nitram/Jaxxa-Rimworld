using RimWorld;
using UnityEngine;
using Verse;

namespace Enhanced_Development.Plants24H
{
    public class Plant : RimWorld.Plant
    {

        private int unlitTicks;

        private float GrowthPerTick
        {
            get
            {
                if (this.LifeStage != PlantLifeStage.Growing)
                    return 0.0f;
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

        private bool HasEnoughLightToGrow
        {
            get
            {
                return (double)this.GrowthRateFactor_Light > 1.0 / 1000.0;
            }
        }

        public override void TickLong()
        {
            this.CheckTemperatureMakeLeafless();
            if (!GenPlant.GrowthSeasonNow(this.Position))
                return;
            if (!this.HasEnoughLightToGrow)
                this.unlitTicks += 2000;
            else
                this.unlitTicks = 0;
            bool flag = this.LifeStage == PlantLifeStage.Mature;
            this.growth += this.GrowthPerTick * 2000f;
            if (!flag && this.LifeStage == PlantLifeStage.Mature)
                this.NewlyMatured();
            if (this.def.plant.LimitedLifespan)
            {
                this.age += 2000;
                if (this.Rotting)
                {
                    int amount = Mathf.CeilToInt(10f);
                    this.TakeDamage(new DamageInfo(DamageDefOf.Rotting, amount, (Thing)null, new BodyPartDamageInfo?(), (ThingDef)null));
                }
            }
            if (this.Destroyed || !this.def.plant.shootsSeeds || ((double)this.growth < 0.600000023841858 || !Rand.MTBEventOccurs(this.def.plant.seedEmitMTBDays, 30000f, 2000f)) || (!GenPlant.SnowAllowsPlanting(this.Position) || GridsUtility.Roofed(this.Position)))
                return;
            GenPlantReproduction.TrySpawnSeed(this.Position, this.def, SeedTargFindMode.ReproduceSeed, (Thing)this);
        }

        private void NewlyMatured()
        {
            if (!this.CurrentlyCultivated())
                return;
            Find.MapDrawer.MapMeshDirty(this.Position, MapMeshFlag.Things);
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


        public bool Rotting
        {
            get
            {
                return this.def.plant.LimitedLifespan && this.age > this.def.plant.LifespanTicks || this.unlitTicks > 450000;
            }
        }

        public override void ExposeData()
        {
            Scribe_Values.LookValue<int>(ref this.unlitTicks, "unlitTicks", 0, false);
            base.ExposeData();
        }
    }
}
