using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Verse;
using Verse.AI;
using RimWorld;
using UnityEngine;

namespace Enhanced_Defence.PersonalShields
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

            //string fileLocation = @"C:\New.txt";
            string fileLocation = @"TempPawn.txt";

            Enhanced_Defence.PersonalShields.Saving.PawnSaver.save(currentPawn, fileLocation);

            ShieldedPawn newPawn = ShieldedPawnGenerator.GeneratePawn(kindDef, faction);

            Enhanced_Defence.PersonalShields.Saving.PawnSaver.load(newPawn, fileLocation);

            newPawn.currentShields = 100;
            newPawn.max_shields = 100;

            return newPawn;
        }
    }
}
