//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
////using System.Threading.Tasks;
//using UnityEngine;
//using Verse;
//using RimWorld;

//namespace Enhanced_Defence.TurretAmmo
//{
//    public class Building_TurretGun : RimWorld.Building_TurretGun
//    {
//        public override string GetInspectString()
//        {
//            StringBuilder stringBuilder = new StringBuilder();

//            //Power info
//            if (base.powerComp != null)
//            {
//                //string str1 = (this.PowerNet.CurrentEnergyGainRate() / CompPower.WattsToWattDaysPerTick).ToString("F0");
//                stringBuilder.AppendLine("Power: " + -base.powerComp.powerOutputInt + " / " + (base.powerComp.PowerNet.CurrentEnergyGainRate() / CompPower.WattsToWattDaysPerTick) + " - Stored: " + base.powerComp.PowerNet.CurrentStoredEnergy().ToString("F0"));
//            }

//            //stringBuilder.Append(base.GetInspectString());
//            //if (this.AnyAdjacentHopper != null)


//            //Range
//            stringBuilder.AppendLine("Range:" + this.gun.PrimaryVerb.verbProps.minRange + " to " + this.gun.PrimaryVerb.verbProps.range);


//            if (this.burstCooldownTicksLeft > 0)
//            {
//                stringBuilder.AppendLine(Translator.Translate("CanFireIn") + ": " + GenTime.TickstoSecondsString(this.burstCooldownTicksLeft));
//            }

//            //stringBuilder.Append(this.burstCooldownTicksLeft);

//            //stringBuilder.Append(base.GetInspectString());

//            return stringBuilder.ToString();
//        }


//    }
//}
