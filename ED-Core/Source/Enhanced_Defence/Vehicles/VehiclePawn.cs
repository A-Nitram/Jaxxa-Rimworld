using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace Enhanced_Defence.Vehicles
{
    public class VehiclePawn : Pawn
    {
        public VehiclePawn()
        {
            Log.Message("Ping");
            this.verbTracker = new VerbTracker((VerbOwner)this);
            this.drawer = new Pawn_DrawTracker(this);
            this.stances = new Pawn_StanceTracker(this);
            this.natives = new Pawn_NativeVerbs(this);
            this.meleeVerbs = new Pawn_MeleeVerbs(this);
            this.thinker = new Pawn_Thinker(this);
        }

        /*
        public override void SpawnSetup()
        {
            base.SpawnSetup();
        }


        public override IEnumerable<Gizmo> GetGizmos()
        {
            return base.GetGizmos();
        }*/
    }

}
