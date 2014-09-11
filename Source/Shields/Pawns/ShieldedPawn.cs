using RimWorld;
using RimWorld.SquadAI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse.AI;
using Verse;

namespace Jaxxa_Shields
{
    public class ShieldedPawn : Verse.Pawn
    {
        public ShieldedPawn()
            : base()
        {
            Log.Error("Creating ShieldedPawn");
        }

        protected override void ApplyDamage(Verse.DamageInfo dinfo)
        {
            Log.Error("Shield Took Hit");
            //base.ApplyDamage(dinfo);
        }


        public override void Tick()
        {
            Log.Message("Shielded Tick()");
            base.Tick();
        }
    }
}
