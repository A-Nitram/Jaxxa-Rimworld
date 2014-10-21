using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using System.Xml;

namespace Enhanced_Defence.Stargate.Saving
{
    class SaveThings
    {
        public static void save(List<Thing> thingsToSave, string fileLocation, Thing source)
        {
            Scribe.InitWriting(fileLocation);

            //Log.Message("Starting Save");
            //Save Pawn

            //Scribe_Collections.LookList<Thing>(ref thingsToSave, "things", LookMode.Deep, (object)null);

            Scribe.EnterNode("things");
            source.ExposeData();
            Scribe.ExitNode();
            /*
            for (int i = 0; i < thingsToSave.Count; i++)
            {
                Scribe_Deep.LookDeep<Thing>(ref thingsToSave[i], thingsToSave[i].ThingID);
            }*/

            Scribe.FinalizeWriting();
            Scribe.mode = LoadSaveMode.Inactive;
            //Log.Message("End Save");
        }


        public static void load(ref List<Thing> thingsToLoad, string fileLocation, Thing currentSource)
        {
            Log.Message("Loading from " + fileLocation);
            Log.Message("ScribeINIT");

            Scribe.InitLoading(fileLocation);
            //Log.Message("list: " + thingsToLoad.Count.ToString());
            List<Thing> list1 = new List<Thing>(10000);

            //Scribe_Collections.LookList<Thing>(ref list1, "things", LookMode.Deep, (object)null);

            Scribe.EnterNode("things");

            Log.Message("Expose Data about to start");
            currentSource.ExposeData();

            Scribe.ExitNode();

            Log.Message("Expose Data ended");



            Scribe.ExitNode();
            Scribe.mode = LoadSaveMode.Inactive;

            Log.Message("list: " + list1.Count.ToString());

            //foreach (Thing thing in Enumerable.Concat<Thing>((IEnumerable<Thing>)list3, (IEnumerable<Thing>)list2))
            //    list1.Add(thing);

            Log.Message("Exit Node");
            Scribe.ExitNode();


            Log.Message("ResolveAllCrossReferences");
            LoadCrossRefHandler.ResolveAllCrossReferences();


            Log.Message("DoAllPostLoadInits");
            PostLoadInitter.DoAllPostLoadInits();

            Log.Message("Return");

        }

    }
}
