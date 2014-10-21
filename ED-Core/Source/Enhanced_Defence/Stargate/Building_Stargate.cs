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


        public override IEnumerable<Command> GetCommands()
        {
            IList<Command> CommandList = new List<Command>();
            IEnumerable<Command> baseCommands = base.GetCommands();

            if (baseCommands != null)
            {
                CommandList = baseCommands.ToList();
            }

            if (this.StargateAddResources)
            {
                //Upgrading
                Command_Action command_Action_InstallShield = new Command_Action();

                command_Action_InstallShield.defaultLabel = "Add Resources";

                command_Action_InstallShield.icon = UI_ADD_RESOURCES;
                command_Action_InstallShield.defaultDesc = "Add Resources";

                command_Action_InstallShield.activateSound = SoundDef.Named("Click");
                command_Action_InstallShield.action = new Action(this.AddResources);

                CommandList.Add(command_Action_InstallShield);
            }

            if (this.StargateAddUnits)
            {
                //Upgrading
                Command_Action command_Action_InstallShield = new Command_Action();

                command_Action_InstallShield.defaultLabel = "Add Colonist";

                command_Action_InstallShield.icon = UI_ADD_COLONIST;
                command_Action_InstallShield.defaultDesc = "Add Colonist";

                command_Action_InstallShield.activateSound = SoundDef.Named("Click");
                //command_Action_InstallShield.action = new Action(this.AddColonist);

                CommandList.Add(command_Action_InstallShield);
            }

            if (this.StargateRetreave)
            {
                //Upgrading
                Command_Action command_Action_InstallShield = new Command_Action();

                command_Action_InstallShield.defaultLabel = "DeepStrike";

                command_Action_InstallShield.icon = UI_DROPPOD;
                command_Action_InstallShield.defaultDesc = "DeepStrike";

                command_Action_InstallShield.activateSound = SoundDef.Named("Click");
                //command_Action_InstallShield.action = new Action(this.DeepStrike);

                CommandList.Add(command_Action_InstallShield);
            }

            if (true)
            {
                //Upgrading
                Command_Action command_Action_InstallShield = new Command_Action();

                command_Action_InstallShield.defaultLabel = "Dial Out";

                command_Action_InstallShield.icon = UI_DROPPOD;
                command_Action_InstallShield.defaultDesc = "Dial Out";

                command_Action_InstallShield.activateSound = SoundDef.Named("Click");
                command_Action_InstallShield.action = new Action(this.StargateDialOut);

                CommandList.Add(command_Action_InstallShield);
            }

            if (true)
            {
                //Upgrading
                Command_Action command_Action_InstallShield = new Command_Action();

                command_Action_InstallShield.defaultLabel = "Incoming WormHole";

                command_Action_InstallShield.icon = UI_DROPPOD;
                command_Action_InstallShield.defaultDesc = "Incoming WormHole";

                command_Action_InstallShield.activateSound = SoundDef.Named("Click");
                command_Action_InstallShield.action = new Action(this.StargateIncomingWormHole);

                CommandList.Add(command_Action_InstallShield);
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

        public void StargateIncomingWormHole()
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
    }
}
