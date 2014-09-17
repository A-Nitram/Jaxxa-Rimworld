using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Verse;
using Verse.AI;
using RimWorld;
using UnityEngine;

namespace Jaxxa_Shields
{
    public static class ShieldedPawnGenerator
    {
        public static ShieldedPawn GeneratePawn(string kindDefName, Faction faction)
        {
            return ShieldedPawnGenerator.GeneratePawn(PawnKindDef.Named(kindDefName), faction);
        }

        public static ShieldedPawn GeneratePawn(PawnKindDef kindDef, Faction faction)
        {
            //Does this work?
            return (ShieldedPawn)PawnGenerator.GeneratePawn(kindDef, faction);
        }

        public static ShieldedPawn GeneratePawn(string kindDefName, Faction faction, Pawn currentPawn)
        {
            return ShieldedPawnGenerator.GeneratePawn(PawnKindDef.Named(kindDefName), faction, currentPawn);
        }

        public static ShieldedPawn GeneratePawn(PawnKindDef kindDef, Faction faction, Pawn currentPawn)
        {
            Log.Message("Starting Save");
            //Save Pawn
            Scribe.InitWriting(@"C:\New.txt");
            Scribe_Deep.LookDeep<Pawn>(ref currentPawn, "currentPawn");

            Scribe.FinalizeWriting();
            Scribe.mode = LoadSaveMode.Inactive;
            Log.Message("End Save");


            ShieldedPawn newPawn = ShieldedPawnGenerator.GeneratePawn(kindDef, faction);


            Scribe.InitLoading(@"C:\New.txt");
            newPawn.ExposeData();

            LoadCrossRefHandler.ResolveAllCrossReferences();
            PostLoadInitter.DoAllPostLoadInits();

            newPawn.currentShields = 100;
            newPawn.max_shields = 100;

            return newPawn;
        }
    }
}
