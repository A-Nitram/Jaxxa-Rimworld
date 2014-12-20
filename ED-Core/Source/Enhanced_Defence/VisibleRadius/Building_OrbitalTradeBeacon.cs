using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace Enhanced_Defence.VisibleRadius
{
    class Building_OrbitalTradeBeacon : RimWorld.Building_OrbitalTradeBeacon
    {
        private static Texture2D UI_SHOW_ON;
        private static Texture2D UI_SHOW_OFF;

        public ShowStatus currentVisability = ShowStatus.Timer;
        public int showTimer = 10;

        public bool flag_showVisually = true;

        public override void SpawnSetup()
        {
            showTimer = 10;

            UI_SHOW_ON = ContentFinder<Texture2D>.Get("UI/ShieldShowOn", true);
            UI_SHOW_OFF = ContentFinder<Texture2D>.Get("UI/ShieldShowOff", true);

            base.SpawnSetup();
        }
        /*
        public override void Tick()
        {
            base.Tick();
        }*/

        public override void TickRare()
        {
            if (this.flag_showVisually)
            {
                if (this.currentVisability == ShowStatus.Timer)
                {
                    this.showTimer -= 1;
                    if (this.showTimer <= 0)
                    {
                        this.SwitchVisual();
                    }
                }

            }
            base.TickRare();
        }

        public override void Draw()
        {
            base.Draw();
            if (flag_showVisually)
            {

                Verse.GenDraw.DrawRadiusRing(this.Position, this.def.specialDisplayRadius);
            }
        }

        /// <summary>
        /// Remove the default selection
        /// </summary>
        public override void DrawExtraSelectionOverlays()
        {
            base.DrawExtraSelectionOverlays();
        }


        private void SwitchVisual()
        {
            if (this.currentVisability == ShowStatus.Hide)
            {
                this.currentVisability = ShowStatus.Show;
            }
            else if (this.currentVisability == ShowStatus.Show)
            {
                this.currentVisability = ShowStatus.Timer;
                this.showTimer = 10;
            }
            else
            {
                this.currentVisability = ShowStatus.Hide;
            }

            if (this.currentVisability == ShowStatus.Hide)
            {
                this.flag_showVisually = false;
            }
            else
            {
                this.flag_showVisually = true;
            }

            //flag_showVisually = !flag_showVisually;
        }



        public override IEnumerable<Command> GetCommands()
        {

            IList<Command> CommandList = new List<Command>();
            IEnumerable<Command> baseCommands = base.GetCommands();

            if (baseCommands != null)
            {
                //CommandList = CommandList.Concat<Command>(baseCommands).ToList();
                CommandList = baseCommands.ToList();
                //CommandList.AsEnumerable<Command>().Concat(baseCommands);
            }


            if (true)
            {
                //Switch Visuals
                Command_Action command_Action_switchShow = new Command_Action();
                command_Action_switchShow.defaultLabel = "Show Visually";
                if (this.currentVisability == ShowStatus.Show)
                {
                    command_Action_switchShow.icon = UI_SHOW_ON;
                    command_Action_switchShow.defaultDesc = "Show";
                    command_Action_switchShow.defaultLabel = "Show Visually";
                }
                else if (this.currentVisability == ShowStatus.Hide)
                {
                    command_Action_switchShow.icon = UI_SHOW_OFF;
                    command_Action_switchShow.defaultDesc = "Hide";
                    command_Action_switchShow.defaultLabel = "Hide Visually";
                }
                else if (this.currentVisability == ShowStatus.Timer)
                {
                    command_Action_switchShow.icon = UI_SHOW_ON;
                    command_Action_switchShow.defaultDesc = "Show Timer";
                    command_Action_switchShow.defaultLabel = "Show Timer: " + this.showTimer.ToString();

                }

                command_Action_switchShow.activateSound = SoundDef.Named("Click");
                command_Action_switchShow.action = new Action(this.SwitchVisual);

                CommandList.Add(command_Action_switchShow);
            }


            return CommandList.AsEnumerable<Command>();
        }

        public override void ExposeData()
        {
            Scribe_Values.LookValue(ref flag_showVisually, "flag_showVisually");
            Scribe_Values.LookValue(ref currentVisability, "currentVisability");

            base.ExposeData();
        }

    }

}
