using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using UnityEngine;
using RimWorld;
using VerseBase;

namespace Enhanced_Defence.Stargate
{
    class Building_Stargate : Building
    {
        //TODO: Saving the Building
        List<Thing> listOfBufferThings = new List<Thing>();

        private static Texture2D UI_ADD_RESOURCES;
        private static Texture2D UI_ADD_COLONIST;
        private static Texture2D UI_DROPPOD;

        private static Texture2D UI_GATE_IN;
        private static Texture2D UI_GATE_OUT;

        public bool StargateAddResources = true;
        public bool StargateAddUnits = true;
        public bool StargateRetreave = true;

        private string FileLocationPrimary = @"Stargate.xml";
        private string FileLocationSecondary = @"StargateBackup.xml";

        Graphic graphicActive;
        Graphic graphicInactive;

        CompPowerTrader power;

        public override void SpawnSetup()
        {
            base.SpawnSetup();

            this.power = base.GetComp<CompPowerTrader>();

            UI_ADD_RESOURCES = ContentFinder<Texture2D>.Get("UI/ADD_RESOURCES", true);
            UI_ADD_COLONIST = ContentFinder<Texture2D>.Get("UI/ADD_COLONIST", true);
            UI_DROPPOD = ContentFinder<Texture2D>.Get("UI/DEEP_STRIKE", true);

            UI_GATE_IN = ContentFinder<Texture2D>.Get("UI/StargateGUI-In", true);
            UI_GATE_OUT = ContentFinder<Texture2D>.Get("UI/StargateGUI-Out", true);

            // Set the graphic
            Material matActive = MaterialPool.MatFrom("Things/Buildings/Stargate-Active", def.shader);
            graphicActive = new Graphic_Single(matActive, false);

            Material matInactive = MaterialPool.MatFrom("Things/Buildings/Stargate", def.shader);
            graphicInactive = new Graphic_Single(matInactive, false);
        }

        //Saving game
        public override void ExposeData()
        {

            Log.Message("Expose Data start");
            base.ExposeData();

            //Scribe_Deep.LookDeep(ref listOfThingLists, "listOfThingLists");

            /*Scribe_Values.LookValue<int>(ref DropPodAddUnitRadius, "DropPodAddUnitRadius");
            Scribe_Values.LookValue<bool>(ref DropPodDeepStrike, "DropPodDeepStrike");
            Scribe_Values.LookValue<bool>(ref DropPodAddUnits, "DropPodAddUnits");
            Scribe_Values.LookValue<bool>(ref DropPodAddResources, "DropPodAddResources");*/

            Log.Message("Expose Data - look list");
            Scribe_Collections.LookList<Thing>(ref listOfBufferThings, "listOfBufferThings", LookMode.Deep, (object)null);
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
                Command_Action command_Action_AddColonist = new Command_Action();

                command_Action_AddColonist.defaultLabel = "Add Colonist";

                command_Action_AddColonist.icon = UI_ADD_COLONIST;
                command_Action_AddColonist.defaultDesc = "Add Colonist";

                command_Action_AddColonist.activateSound = SoundDef.Named("Click");
                command_Action_AddColonist.action = new Action(this.AddColonist);

                CommandList.Add(command_Action_AddColonist);
            }

            if (true)
            {
                //Upgrading
                Command_Action command_Action_DialOut = new Command_Action();

                command_Action_DialOut.defaultLabel = "Dial Out";

                command_Action_DialOut.icon = UI_GATE_OUT;
                command_Action_DialOut.defaultDesc = "Dial Out";

                command_Action_DialOut.activateSound = SoundDef.Named("Click");
                command_Action_DialOut.action = new Action(this.StargateDialOut);

                CommandList.Add(command_Action_DialOut);
            }

            if (true)
            {
                //Upgrading
                Command_Action command_Action_IncomingWormhole = new Command_Action();

                command_Action_IncomingWormhole.defaultLabel = "Inbound WormHole";

                command_Action_IncomingWormhole.icon = UI_GATE_IN;
                command_Action_IncomingWormhole.defaultDesc = "Inbound Wormhole";

                command_Action_IncomingWormhole.activateSound = SoundDef.Named("Click");
                command_Action_IncomingWormhole.action = new Action(this.StargateIncomingWormhole);

                CommandList.Add(command_Action_IncomingWormhole);
            }


            return CommandList.AsEnumerable<Command>();
        }

        public void AddResources()
        {

            if (power.PowerOn)
            {

                Thing foundThing = Enhanced_Defence.Utilities.Utilities.FindItemThingsInAutoLoader(this);

                if (foundThing != null)
                {
                    if (foundThing.SpawnedInWorld)
                    {
                        List<Thing> thingList = new List<Thing>();
                        //thingList.Add(foundThing);
                        listOfBufferThings.Add(foundThing);
                        foundThing.DeSpawn();

                        //Building_OrbitalRelay.listOfThingLists.Add(thingList);


                        //Recursively Call to get Everything
                        this.AddResources();
                    }
                }

                // Tell the MapDrawer that here is something thats changed
                Find.MapDrawer.MapChanged(Position, MapChangeType.Things, true, false);
            }
            else
            {
                Messages.Message("Insufficient Power to add Resources");
            }

        }

        private void MoveToBackup()
        {

            if (System.IO.File.Exists(this.FileLocationSecondary))
            {
                System.IO.File.Delete(this.FileLocationSecondary);
            }

            if (System.IO.File.Exists(this.FileLocationPrimary))
            {
                System.IO.File.Move(this.FileLocationPrimary, this.FileLocationSecondary);
            }
            else
            {
                Log.Warning("Building_Stargate.MoveToBackup(), file at FileLocationPrimary not found.");
            }
        }

        public void StargateDialOut()
        {
            if (System.IO.File.Exists(this.FileLocationPrimary))
            {
                Messages.Message("Please Recall Offworld Teams First", MessageSound.Reject);
            }
            else
            {
                Enhanced_Defence.Stargate.Saving.SaveThings.save(listOfBufferThings, this.FileLocationPrimary, this);

                this.listOfBufferThings.Clear();

                // Tell the MapDrawer that here is something thats changed
                Find.MapDrawer.MapChanged(Position, MapChangeType.Things, true, false);
            }

        }

        public void StargateIncomingWormhole()
        {
            if (System.IO.File.Exists(this.FileLocationPrimary))
            {
                Messages.Message("Recalling Offworld Teams", MessageSound.Benefit);

                List<Thing> inboundBuffer = new List<Thing>();

                Log.Message("start list contains: " + inboundBuffer.Count);
                Enhanced_Defence.Stargate.Saving.SaveThings.load(ref inboundBuffer, this.FileLocationPrimary, this);
                Log.Message("end list contains: " + inboundBuffer.Count);

                foreach (Thing currentThing in inboundBuffer)
                {
                    Log.Message("Placing Thing");
                    GenPlace.TryPlaceThing(currentThing, this.Position + new IntVec3(0,0,-2), ThingPlaceMode.Near);
                }
                Log.Message("End of Placing");
                inboundBuffer.Clear();

                // Tell the MapDrawer that here is something thats changed
                Find.MapDrawer.MapChanged(Position, MapChangeType.Things, true, false);

                this.MoveToBackup();

            }
            else
            {
                Messages.Message("No Offworld Teams Found", MessageSound.Reject);
                Log.Message("Building_Stargate.StargateIncomingWormhole() unable to find file at FileLocationPrimary");
            }
        }

        public void AddColonist()
        {
            if (power.PowerOn)
            {
                //Log.Message("CLick AddColonist");
                IEnumerable<Pawn> closePawns = Enhanced_Defence.Utilities.Utilities.findPawns(this.Position, 5);

                if (closePawns != null)
                {
                    foreach (Pawn currentPawn in closePawns.ToList())
                    {
                        if (currentPawn.SpawnedInWorld)
                        {
                            List<Thing> thingList = new List<Thing>();
                            listOfBufferThings.Add(currentPawn);
                            //currentPawn.DeSpawn();
                            currentPawn.Destroy(DestroyMode.Vanish);
                        }
                    }
                }

                // Tell the MapDrawer that here is something thats changed
                Find.MapDrawer.MapChanged(Position, MapChangeType.Things, true, false);
            }
            else
            {
                Messages.Message("Insufficient Power to add Colonist");
            }
        }

        public override Graphic Graphic
        {
            get
            {
                if (this.listOfBufferThings.Count > 0)
                {
                    return this.graphicActive;
                }
                else
                {
                    return this.graphicInactive;

                }
                return base.Graphic;
            }
        }

        public override string GetInspectString()
        {

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(base.GetInspectString());
            stringBuilder.AppendLine("Buffer Items: " + this.listOfBufferThings.Count);

            return stringBuilder.ToString();
        }
    }
}
