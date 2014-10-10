using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace Enhanced_Defence.Power.LaserDrill
{

    public class Building_LaserFiller : Building
    {
        private int drillWork = 500;
        private CompPowerTrader powerComp;

        public override void SpawnSetup()
        {
            base.SpawnSetup();
            this.powerComp = this.GetComp<CompPowerTrader>();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.LookValue<int>(ref this.drillWork, "drillWork", 0, false);
        }



        public override void TickRare()
        {
            if (this.powerComp.PowerOn)
            {

                //Log.Message("Reducing count");
                this.drillWork = this.drillWork - 1;
            }
            else
            {
                //Log.Message("No Power for drill.");
            }

            if (this.drillWork <= 0)
            {
                
                
                Thing SteamGeyser = Find.ThingGrid.ThingAt(this.Position, ThingDef.Named("SteamGeyser"));
                //SteamGeyser.Destroy(DestroyMode.Vanish);

                SteamGeyser.DeSpawn();
                this.Destroy(DestroyMode.Vanish);
                //GenSpawn.Spawn(ThingDef.Named("SteamGeyser"), this.Position);
            }
            base.Tick();
        }

        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            string text;

            if (powerComp != null)
            {
                if (this.powerComp.PowerOn)
                {
                    text = "Fill Status: Online";

                }
                else
                {
                    text = "Fill Status: Low Power";
                }
            }
            else
            {
                text = "Fill Status: Low Power";
            }

            stringBuilder.AppendLine(text);

            text = "Fill Work Remaining: " + this.drillWork;
            stringBuilder.AppendLine(text);

            if (powerComp != null)
            {
                text = powerComp.CompInspectStringExtra();
                if (!text.NullOrEmpty())
                {
                    stringBuilder.AppendLine(text);
                }
            }


            return stringBuilder.ToString();
        }
    }
}
