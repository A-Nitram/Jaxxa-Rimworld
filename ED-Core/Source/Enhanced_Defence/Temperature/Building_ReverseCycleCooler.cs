using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;

namespace Enhanced_Defence.Temperature
{
    public class Building_ReverseCycleCooler : Building_Cooler
    {
        private static Texture2D UI_ADD_RESOURCES;
        
        public override void SpawnSetup()
        {
            base.SpawnSetup();
            UI_ADD_RESOURCES = ContentFinder<Texture2D>.Get("UI/ADD_RESOURCES", true);
        }

       /* public override void TickRare()
        {
            if (!this.compPowerTrader.PowerOn)
            {
                return;
            }
            IntVec3 intVec3_1 = this.Position + Gen.RotatedBy(IntVec3.south, this.Rotation);
            IntVec3 intVec3_2 = this.Position + Gen.RotatedBy(IntVec3.north, this.Rotation);

        }*/

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

                command_Action_AddResources.defaultLabel = "Rotate";

                command_Action_AddResources.icon = UI_ADD_RESOURCES;
                command_Action_AddResources.defaultDesc = "Rotate";

                command_Action_AddResources.activateSound = SoundDef.Named("Click");
                command_Action_AddResources.action = new Action(this.ChangeRotation);

                CommandList.Add(command_Action_AddResources);
            }


            return CommandList.AsEnumerable<Command>();
            //return compPowerTrader.CompGetCommandsExtra();
            //return base.GetCommands();
        }

        public void ChangeRotation()
        {
            //Log.Error("Rotation");

            if (this.Rotation.AsInt == 3)
            {
                this.Rotation = new IntRot(0);
            }
            else
            {

                this.Rotation = new IntRot(this.Rotation.AsInt + 1);
            }


            // Tell the MapDrawer that here is something thats changed
            Find.MapDrawer.MapChanged(Position, MapChangeType.Things, true, false);
            
            //this.Rotation.Rotate(RotationDirection.Clockwise);
            //this.Rotation.Rotate(RotationDirection.Clockwise);

        }

    }
}
