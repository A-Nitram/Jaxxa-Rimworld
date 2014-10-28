using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using UnityEngine;

namespace Enhanced_Defence.Stargate
{
    class Building_Stargate : Building
    {
        //TODO: Saving the Building
        List<Thing> listOfOffworldThings = new List<Thing>();

        private static Texture2D UI_ADD_RESOURCES;
        private static Texture2D UI_ADD_COLONIST;
        private static Texture2D UI_DROPPOD;

        public bool StargateAddResources = true;
        public bool StargateAddUnits = true;
        public bool StargateRetreave = true;

        public override void SpawnSetup()
        {
            base.SpawnSetup();

            UI_ADD_RESOURCES = ContentFinder<Texture2D>.Get("UI/ADD_RESOURCES", true);
            UI_ADD_COLONIST = ContentFinder<Texture2D>.Get("UI/ADD_COLONIST", true);
            UI_DROPPOD = ContentFinder<Texture2D>.Get("UI/DEEP_STRIKE", true);

        }

        //Saving game
        public override void ExposeData()
        {

            Log.Message("Expose Data start");
            //base.ExposeData();


            //Scribe_Deep.LookDeep(ref listOfThingLists, "listOfThingLists");

            /*Scribe_Values.LookValue<int>(ref DropPodAddUnitRadius, "DropPodAddUnitRadius");
            Scribe_Values.LookValue<bool>(ref DropPodDeepStrike, "DropPodDeepStrike");
            Scribe_Values.LookValue<bool>(ref DropPodAddUnits, "DropPodAddUnits");
            Scribe_Values.LookValue<bool>(ref DropPodAddResources, "DropPodAddResources");*/

            Log.Message("Expose Data - look list");
            //Scribe_Collections.LookList<Thing>(ref listOfThingLists, "listOfThingLists", LookMode.Deep, (object)null);
            //Scribe_Collections.LookList<Thing>(ref listOfOffworldThings, "listOfOffworldThings", LookMode.Deep, (object)null);

            Log.Message("Expose Data about to start");

        }

        public override IEnumerable<Command> GetCommands()
        {
            IList<Command> CommandList = new List<Command>();
            IEnumerable<Command> baseCommands = base.GetCommands();

            if (baseCommands != null)
            {
                CommandList = baseCommands.ToList();
            }

            if (true)
            {
                //Upgrading
                Command_Action command_Action_AddResources = new Command_Action();

                command_Action_AddResources.defaultLabel = "Add Resources";

                command_Action_AddResources.icon = UI_ADD_RESOURCES;
                command_Action_AddResources.defaultDesc = "Add Resources";

                command_Action_AddResources.activateSound = SoundDef.Named("Click");
                command_Action_AddResources.action = new Action(this.AddResources);

                CommandList.Add(command_Action_AddResources);
            }

            if (true)
            {
                //Upgrading
                Command_Action command_Action_AddColonist = new Command_Action();

                command_Action_AddColonist.defaultLabel = "Add Colonist";

                command_Action_AddColonist.icon = UI_ADD_COLONIST;
                command_Action_AddColonist.defaultDesc = "Add Colonist";

                command_Action_AddColonist.activateSound = SoundDef.Named("Click");
                command_Action_AddColonist.action = new Action(this.AddColonist);

                CommandList.Add(command_Action_AddColonist);
            }
            /*
            if (this.StargateRetreave)
            {
                //Upgrading
                Command_Action command_Action_IncomingWormehole = new Command_Action();

                command_Action_IncomingWormehole.defaultLabel = "IncomingWormehole";

                command_Action_IncomingWormehole.icon = UI_DROPPOD;
                command_Action_IncomingWormehole.defaultDesc = "IncomingWormehole";

                command_Action_IncomingWormehole.activateSound = SoundDef.Named("Click");
                //command_Action_InstallShield.action = new Action(this.DeepStrike);

                CommandList.Add(command_Action_IncomingWormehole);
            }*/

            if (true)
            {
                //Upgrading
                Command_Action command_Action_DialOut = new Command_Action();

                command_Action_DialOut.defaultLabel = "Dial Out";

                command_Action_DialOut.icon = UI_DROPPOD;
                command_Action_DialOut.defaultDesc = "Dial Out";

                command_Action_DialOut.activateSound = SoundDef.Named("Click");
                command_Action_DialOut.action = new Action(this.StargateDialOut);

                CommandList.Add(command_Action_DialOut);
            }

            if (true)
            {
                //Upgrading
                Command_Action command_Action_IncomingWormhole = new Command_Action();

                command_Action_IncomingWormhole.defaultLabel = "Incoming WormHole";

                command_Action_IncomingWormhole.icon = UI_DROPPOD;
                command_Action_IncomingWormhole.defaultDesc = "Incoming WormHole";

                command_Action_IncomingWormhole.activateSound = SoundDef.Named("Click");
                command_Action_IncomingWormhole.action = new Action(this.StargateIncomingWormhole);

                CommandList.Add(command_Action_IncomingWormhole);
            }


            return CommandList.AsEnumerable<Command>();
        }

        public void AddResources()
        {

            //if (power.PowerOn)
            if (true)
            {

                Thing foundThing = Enhanced_Defence.Utilities.Utilities.FindItemThingsInAutoLoader(this);

                if (foundThing != null)
                {
                    if (foundThing.SpawnedInWorld)
                    {
                        List<Thing> thingList = new List<Thing>();
                        //thingList.Add(foundThing);
                        listOfOffworldThings.Add(foundThing);
                        foundThing.DeSpawn();

                        //Building_OrbitalRelay.listOfThingLists.Add(thingList);


                        //Recursively Call to get Everything
                        this.AddResources();
                    }
                }
            }
            else
            {
                Messages.Message("Insufficient Power to add Resources");
            }

        }

        public void StargateDialOut()
        {
            Enhanced_Defence.Stargate.Saving.SaveThings.save(listOfOffworldThings, @"C:\Stargate.txt", this);
        }

        public void StargateIncomingWormhole()
        {
            //listOfOffworldThings.Clear();

            Log.Message("start list contains: " + listOfOffworldThings.Count);
            Enhanced_Defence.Stargate.Saving.SaveThings.load(ref listOfOffworldThings, @"C:\Stargate.txt", this);
            Log.Message("end list contains: " + listOfOffworldThings.Count);

            foreach (Thing currentThing in listOfOffworldThings)
            {
                Log.Message("Placing Thing");
                GenPlace.TryPlaceThing(currentThing, this.Position, ThingPlaceMode.Near);
            }
            Log.Message("End of Placing");
        }

        public void AddColonist()
        {
            if (true)
                //if (power.PowerOn)
            {
                //Log.Message("CLick AddColonist");
                IEnumerable<Pawn> closePawns = Enhanced_Defence.Utilities.Utilities.findPawns(this.Position, 5);

                if (closePawns != null)
                {
                    foreach (Pawn currentPawn in closePawns)
                    {
                        if (currentPawn.SpawnedInWorld)
                        {

                            List<Thing> thingList = new List<Thing>();
                            //thingList.Add(foundThing);
                            listOfOffworldThings.Add(currentPawn);
                            currentPawn.DeSpawn();

                            //Building_OrbitalRelay.listOfThings.Add(currentPawn);
                            //currentPawn.DeSpawn();
                        }
                    }
                }
            }
            else
            {
                Messages.Message("Insufficient Power to add Colonist");
            }
        }
    }
}
