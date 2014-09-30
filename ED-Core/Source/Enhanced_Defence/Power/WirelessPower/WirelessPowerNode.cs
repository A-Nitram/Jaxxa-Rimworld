using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using UnityEngine;
using RimWorld;

namespace Enhanced_Defence.Power.WirelessPower
{
    class WirelessPowerNode : Building
    {
        #region Variables

        CompPowerTrader power;

        //NanoConnector nanoConnector;

        private static Texture2D UI_POWER_TRANSMIT;
        private static Texture2D UI_POWER_RECEIVE;

        public float desiredPower = 0;

        #endregion

        //Dummy override
        public override void PostMake()
        {
            UI_POWER_TRANSMIT = ContentFinder<Texture2D>.Get("UI/PowerTransmit", true);
            UI_POWER_RECEIVE = ContentFinder<Texture2D>.Get("UI/PowerReceive", true);

            base.PostMake();
        }

        //On spawn, get the power component reference
        public override void SpawnSetup()
        {
            base.SpawnSetup();
            this.power = base.GetComp<CompPowerTrader>();
            WirelessPowerManager.registerToGrid(this);
            //this.nanoConnector = new Jaxxa_Shields.Pawns.Nano.NanoConnector();
        }

        //public override void TickRare()
        public override void Tick()
        {
            //WirelessPowerManager.TickRare();
            WirelessPowerManager.Tick();
            base.TickRare();

            if (WirelessPowerManager.suficentPower)
            {
                this.power.PowerOn = true;
                this.power.DesirePowerOn = true;
            }
            else
            {
                if (this.desiredPower > 0)
                {
                    this.power.PowerOn = false;
                    this.power.DesirePowerOn = false;
                }
                else
                {
                   // this.power.PowerOn = true;
                    //this.power.DesirePowerOn = true;
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();

            // Scribe_Values.LookValue(ref flag_charge, "flag_charge");
            // Scribe_Values.LookValue(ref NanoManager.currentCharge, "currentCharge");
            /*
            Scribe_Deep.LookDeep(ref shieldField, "shieldField");

            shieldField.position = base.Position;

            Scribe_Values.LookValue(ref flag_direct, "flag_direct");
            Scribe_Values.LookValue(ref flag_indirect, "flag_indirect");
            Scribe_Values.LookValue(ref flag_fireSupression, "flag_fireSupression");
            Scribe_Values.LookValue(ref flag_shieldRepairMode, "flag_shieldRepairMode");*/
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
                //PowerUp
                Command_Action command_Action_PowerUp = new Command_Action();

                command_Action_PowerUp.defaultLabel = "Power Transmit";

                command_Action_PowerUp.icon = UI_POWER_TRANSMIT;
                command_Action_PowerUp.defaultDesc = "Power Transmit";

                command_Action_PowerUp.activateSound = SoundDef.Named("Click");
                command_Action_PowerUp.action = new Action(this.PowerTransmit);

                CommandList.Add(command_Action_PowerUp);
            }

            //Power Down
            if (true)
            {
                //PowerUp
                Command_Action command_Action_PowerDown = new Command_Action();

                command_Action_PowerDown.defaultLabel = "Power Receive";

                command_Action_PowerDown.icon = UI_POWER_RECEIVE;
                command_Action_PowerDown.defaultDesc = "Power Receive";

                command_Action_PowerDown.activateSound = SoundDef.Named("Click");
                command_Action_PowerDown.action = new Action(this.PowerReceive);

                CommandList.Add(command_Action_PowerDown);
            }
            return CommandList.AsEnumerable<Command>();
        }

        private void PowerTransmit()
        {
            this.desiredPower -= 100;
            this.power.powerOutput = desiredPower;
        }

        private void PowerReceive()
        {
            this.desiredPower += 100;
            this.power.powerOutput = desiredPower;
        }

        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            string text;

            if (WirelessPowerManager.suficentPower)
            {
                text = "Power Net Online:" + WirelessPowerManager.CurrentAvalablePower;
            }
            else
            {
                text = "Power Net Offline:" + WirelessPowerManager.CurrentAvalablePower;
            }

            stringBuilder.AppendLine(text);

            if (power != null)
            {
                text = power.CompInspectStringExtra();
                if (!text.NullOrEmpty())
                {
                    stringBuilder.AppendLine(text);
                }
            }


            return stringBuilder.ToString();
        }
    }
}
