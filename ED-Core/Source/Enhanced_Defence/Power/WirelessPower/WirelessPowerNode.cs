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

        public CompPowerTrader power;

        //NanoConnector nanoConnector;

        private static Texture2D UI_POWER_TRANSMIT;
        private static Texture2D UI_POWER_RECEIVE;

        public int desiredPower = 0;

        #endregion

        //Dummy override
        public override void PostMake()
        {
            base.PostMake();
        }

        //On spawn, get the power component reference
        public override void SpawnSetup()
        {
            base.SpawnSetup();
            this.power = base.GetComp<CompPowerTrader>();

            this.power.powerOutputInt = desiredPower;

            WirelessPowerManager.registerToGrid(this);
            //this.nanoConnector = new Jaxxa_Shields.Pawns.Nano.NanoConnector();


            UI_POWER_TRANSMIT = ContentFinder<Texture2D>.Get("UI/PowerUp", true);
            UI_POWER_RECEIVE = ContentFinder<Texture2D>.Get("UI/PowerDown", true);

            //UI_POWER_TRANSMIT = ContentFinder<Texture2D>.Get("UI/PowerTransmit", true);
            //UI_POWER_RECEIVE = ContentFinder<Texture2D>.Get("UI/PowerReceive", true);
        }

        //public override void TickRare()
        public override void Tick()
        {
            //WirelessPowerManager.TickRare();
            WirelessPowerManager.Tick();
            base.TickRare();

            if (!this.WantsToTransmit())
            {
                //Receiving power
                if (!WirelessPowerManager.suficentPower)
                {
                    //this.power.PowerOn = true;
                    this.power.DesirePowerOn = false;
                }
            }
            else
            {
                //Transmitting power

                // this.power.PowerOn = true;
                //this.power.DesirePowerOn = true;
            }


        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.LookValue(ref desiredPower, "desiredPower");


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


        public override IEnumerable<Gizmo> GetGizmos()
        {
            //Add the stock Gizmoes
            foreach (var g in base.GetGizmos())
            {
                yield return g;
            }

            if (true)
            {
                Command_Action act = new Command_Action();
                //act.action = () => Designator_Deconstruct.DesignateDeconstruct(this);
                act.action = () => this.PowerReceive1000();
                act.icon = UI_POWER_RECEIVE;
                act.defaultLabel = "Power Receive 1000";
                act.defaultDesc = "Power Receive 1000";
                act.activateSound = SoundDef.Named("Click");
                //act.hotKey = KeyBindingDefOf.DesignatorDeconstruct;
                //act.groupKey = 689736;
                yield return act;
            }


            if (true)
            {
                Command_Action act = new Command_Action();
                //act.action = () => Designator_Deconstruct.DesignateDeconstruct(this);
                act.action = () => this.PowerReceive100();
                act.icon = UI_POWER_RECEIVE;
                act.defaultLabel = "Power Receive 100";
                act.defaultDesc = "Power Receive 100";
                act.activateSound = SoundDef.Named("Click");
                //act.hotKey = KeyBindingDefOf.DesignatorDeconstruct;
                //act.groupKey = 689736;
                yield return act;
            }


            if (true)
            {
                Command_Action act = new Command_Action();
                //act.action = () => Designator_Deconstruct.DesignateDeconstruct(this);
                act.action = () => this.PowerTransmit100();
                act.icon = UI_POWER_TRANSMIT;
                act.defaultLabel = "Power Transmit 100";
                act.defaultDesc = "Power Transmit 100";
                act.activateSound = SoundDef.Named("Click");
                //act.hotKey = KeyBindingDefOf.DesignatorDeconstruct;
                //act.groupKey = 689736;
                yield return act;
            }

            if (true)
            {
                Command_Action act = new Command_Action();
                //act.action = () => Designator_Deconstruct.DesignateDeconstruct(this);
                act.action = () => this.PowerTransmit1000();
                act.icon = UI_POWER_TRANSMIT;
                act.defaultLabel = "Power Transmit 1000";
                act.defaultDesc = "Power Transmit 1000";
                act.activateSound = SoundDef.Named("Click");
                //act.hotKey = KeyBindingDefOf.DesignatorDeconstruct;
                //act.groupKey = 689736;
                yield return act;
            }
            
        }

        private void PowerTransmit100()
        {
            this.desiredPower -= 100;
            this.power.powerOutputInt = desiredPower;
        }
        private void PowerTransmit1000()
        {
            this.desiredPower -= 1000;
            this.power.powerOutputInt = desiredPower;
        }

        private void PowerReceive100()
        {
            this.desiredPower += 100;
            this.power.powerOutputInt = desiredPower;
        }
        private void PowerReceive1000()
        {
            this.desiredPower += 1000;
            this.power.powerOutputInt = desiredPower;
        }

        public bool WantsToTransmit()
        {
            if (power.powerOutputInt <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
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
