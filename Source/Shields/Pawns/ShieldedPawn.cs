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
        public int max_shields = 100;
        public int currentShields = 100;

        public ShieldedPawn()
            : base()
        {
            Log.Message("Creating ShieldedPawn");
        }

        protected override void ApplyDamage(Verse.DamageInfo dinfo)
        {
            Log.Message("Shield Took Hit");

            if (currentShields > 0)
            {
                currentShields -= dinfo.Amount;
            }
            else
            {
                base.ApplyDamage(dinfo);
            }
        }


        public override void Tick()
        {
            //Log.Message("Shielded Tick()");
            base.Tick();
        }

        public override string GetInspectString()
        {
            string output;

            output = "Shields " + currentShields + @" / " + max_shields + " - " + base.GetInspectString();
            return output; 
            
        }

        public override void ExposeData()
        {
            Scribe_Values.LookValue<int>(ref currentShields, "currentShields");
            Scribe_Values.LookValue<int>(ref max_shields, "max_shields");
            base.ExposeData();
        }
    }
}
