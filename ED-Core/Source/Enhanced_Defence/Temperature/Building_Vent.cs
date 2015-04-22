using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;

namespace Enhanced_Defence.Temperature
{
    public class Building_Vent : RimWorld.Building_Vent
    {

        CompPowerTrader power;

        private const float EqualizationPercentPerTickRare = 0.21f;

        public override void SpawnSetup()
        {
            base.SpawnSetup();

            this.power = base.GetComp<CompPowerTrader>();
        }

        public override void TickRare()
        {
            if (this.power.PowerOn)
            {

                IntVec3 loc1 = this.Position + Gen.RotatedBy(IntVec3.South, this.Rotation);
                IntVec3 loc2 = this.Position + Gen.RotatedBy(IntVec3.North, this.Rotation);
                Room room1 = GridsUtility.GetRoom(loc1);
                if (room1 == null)
                    return;
                Room room2 = GridsUtility.GetRoom(loc2);
                if (room2 == null || room1 == room2 || room1.UsesOutdoorTemperature && room2.UsesOutdoorTemperature)
                    return;
                float targetTemp = !room1.UsesOutdoorTemperature ? (!room2.UsesOutdoorTemperature ? (float)((double)room1.Temperature * (double)room1.CellCount + (double)room2.Temperature * (double)room2.CellCount) / (float)(room1.CellCount + room2.CellCount) : room2.Temperature) : room1.Temperature;
                if (!room1.UsesOutdoorTemperature)
                    this.Equalize(room1, targetTemp);
                if (room2.UsesOutdoorTemperature)
                    return;
                this.Equalize(room2, targetTemp);
            }
        }

        private void Equalize(Room r, float targetTemp)
        {
            float num = Mathf.Abs(r.Temperature - targetTemp) * 0.21f;
            if ((double)targetTemp < (double)r.Temperature)
            {
                r.Temperature = Mathf.Max(targetTemp, r.Temperature - num);
            }
            else
            {
                if ((double)targetTemp <= (double)r.Temperature)
                    return;
                r.Temperature = Mathf.Min(targetTemp, r.Temperature + num);
            }
        }
    }
}
