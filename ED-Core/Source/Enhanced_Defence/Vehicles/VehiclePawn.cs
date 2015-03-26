using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Enhanced_Defence.Vehicles
{
    public class VehiclePawn : Pawn
    {
        public override void SpawnSetup()
        {
            base.SpawnSetup();
        }


        public override IEnumerable<Gizmo> GetGizmos()
        {
            return base.GetGizmos();
        }
    }

}
