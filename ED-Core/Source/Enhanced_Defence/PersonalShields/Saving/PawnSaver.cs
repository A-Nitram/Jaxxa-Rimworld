using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Enhanced_Defence.PersonalShields.Saving
{
    static class PawnSaver
    {

        public static void save(Pawn currentPawn, string fileLocation)
        {
            Log.Message("Starting Save");
            //Save Pawn
            Scribe.InitWriting(fileLocation);
            Scribe_Deep.LookDeep<Pawn>(ref currentPawn, "currentPawn");

            Scribe.FinalizeWriting();
            Scribe.mode = LoadSaveMode.Inactive;
            Log.Message("End Save");

        }

        public static void load(Pawn newPawn, string fileLocation)
        {
            Scribe.InitLoading(fileLocation);
            newPawn.ExposeData();
            LoadCrossRefHandler.ResolveAllCrossReferences();
            PostLoadInitter.DoAllPostLoadInits();
        }
    }
}
