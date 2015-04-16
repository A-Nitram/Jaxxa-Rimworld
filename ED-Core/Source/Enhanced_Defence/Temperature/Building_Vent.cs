using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;

namespace Enhanced_Defence.Temperature
{
    public class Building_Vent : Building
    {
        bool flag = false;

        public CompTempControl compTempControl;
        public CompPowerTrader compPowerTrader;
        float energyLimit = 0;

        float temperature1 = 0;
        float temperature2 = 0;
        float temperatureDiff = 0;

       //bool ventOpen = true;

        //private static Texture2D UI_CLOSE;
       // private static Texture2D UI_OPEN;

        public override void SpawnSetup()
        {
            base.SpawnSetup();
            this.compTempControl = this.GetComp<CompTempControl>();
            this.compPowerTrader = this.GetComp<CompPowerTrader>();

            //UI_CLOSE = ContentFinder<Texture2D>.Get("UI/RotRight", true);
            //UI_OPEN = ContentFinder<Texture2D>.Get("UI/RotRight", true);
        }


        //Saving game
        /*public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.LookValue<bool>(ref ventOpen, "ventOpen");

        }*/

        public override void TickRare()
        {
            energyLimit = 0;
            temperature1 = 0;
            temperature2 = 0;
            temperatureDiff = 0;

            if (!this.compPowerTrader.PowerOn)
            {
                return;
            }

            /*
            if (!this.ventOpen)
            {
                return;
            }*/

            
            IntVec3 intVec3_1 = this.Position + Gen.RotatedBy(IntVec3.South, this.Rotation);
            IntVec3 intVec3_2 = this.Position + Gen.RotatedBy(IntVec3.North, this.Rotation);

            bool flag = false;

            if (!GenGrid.Impassable(intVec3_2) && !GenGrid.Impassable(intVec3_1))
            {
                temperature1 = GridsUtility.GetTemperature(intVec3_1);
                temperature2 = GridsUtility.GetTemperature(intVec3_2);
                temperatureDiff = temperature1 - temperature2;
                /*
                //float energyLimit = (float)((double)this.compTempControl.props.energyPerSecond * temperatureDiff);

                float num2 = (float)(1.0 - (double)temperatureDiff * (1.0 / 130.0));
                float energyLimit = (float)((double)this.compTempControl.props.energyPerSecond * (double)num2 * 4.16666650772095);
                 * 
                12.0f * (double)num2 * 4.16666650772095);
                 * 
                energyLimit = Math.Abs(energyLimit);*/

                //float energyLimit = 100.0f;

                float halfTemp = Math.Abs(temperatureDiff) / 2;

                energyLimit = Math.Abs(temperatureDiff * 25.0f);

                float maxEnergyLimit = 500.0f;

                if (energyLimit > maxEnergyLimit)
                {
                    energyLimit = maxEnergyLimit;
                }

                //Log.Message("Limit: " + energyLimit + " temperature1 " + temperature1 + " temperature2 " + temperature2);

                if (temperature1 > temperature2)
                {
                    GenTemperature.ControlTemperatureTempChange(intVec3_1, -energyLimit, temperature2 + halfTemp);
                    GenTemperature.ControlTemperatureTempChange(intVec3_2, energyLimit, temperature1 - halfTemp);
                }
                else
                {
                    GenTemperature.ControlTemperatureTempChange(intVec3_1, energyLimit, temperature2 - halfTemp);
                    GenTemperature.ControlTemperatureTempChange(intVec3_2, -energyLimit, temperature1 + halfTemp);
                }


                /*
                if (temperatureDiff > 0)
                {
                    //flag = GenTemperature.TryControlTemperature(intVec3_1, energyLimit, this.compTempControl.targetTemperature);
                    flag = GenTemperature.TryControlTemperature(intVec3_1, energyLimit, temperature2);
                    if (flag)
                    {
                        GenTemperature.PushHeat(intVec3_2, (float)(-(double)energyLimit));
                    }
                }
                else
                {
                    //energyLimit = -energyLimit;
                    flag = GenTemperature.TryControlTemperature(intVec3_2, energyLimit, temperature1);
                    //flag = GenTemperature.TryControlTemperature(intVec3_2, energyLimit, this.compTempControl.targetTemperature);
                    if (flag)
                    {
                        GenTemperature.PushHeat(intVec3_1, (float)(-(double)energyLimit));
                    }
                }*/

            }

        }

        /* public override void TickRare()
        {
            if (!this.compPowerTrader.PowerOn)
            {
                return;
            }
            IntVec3 intVec3_1 = this.Position + Gen.RotatedBy(IntVec3.south, this.Rotation);
            IntVec3 intVec3_2 = this.Position + Gen.RotatedBy(IntVec3.north, this.Rotation);

            bool flag = false;

            if (!GenGrid.Impassable(intVec3_2) && !GenGrid.Impassable(intVec3_1))
            {
                float temperature1 = GridsUtility.GetTemperature(intVec3_1);
                float temperature2 = GridsUtility.GetTemperature(intVec3_2);
                float temperatureDiff = temperature1 - temperature2;

                float energyLimit = (float)((double)this.compTempControl.props.energyPerSecond * temperatureDiff);
                
                //energyLimit = Math.Abs(energyLimit);

                if (temperatureDiff > 0)
                {
                    flag = GenTemperature.TryControlTemperature(intVec3_1, energyLimit, this.compTempControl.targetTemperature);
                    if (flag)
                    {
                        GenTemperature.PushHeat(intVec3_2, (float)(-(double)energyLimit));
                    }
                }
                else
                {
                    energyLimit = -energyLimit;
                    flag = GenTemperature.TryControlTemperature(intVec3_2, energyLimit, this.compTempControl.targetTemperature);
                    if (flag)
                    {
                        GenTemperature.PushHeat(intVec3_1, (float)(-(double)energyLimit));
                    }
                }

            }

        }*/
        /*
        public override IEnumerable<Command> GetCommands()
        {
            IList<Command> CommandList = new List<Command>();
            IEnumerable<Command> baseCommands = base.GetCommands();

            if (baseCommands != null)
            {
                CommandList = baseCommands.ToList();
            }

            if (ventOpen)
            {
                //Upgrading
                Command_Action command_Action_AddResources = new Command_Action();

                command_Action_AddResources.defaultLabel = "Close Vent";

                command_Action_AddResources.icon = UI_CLOSE;
                command_Action_AddResources.defaultDesc = "Close Vent";

                command_Action_AddResources.activateSound = SoundDef.Named("Click");
                command_Action_AddResources.action = new Action(this.toggleVent);

                CommandList.Add(command_Action_AddResources);
            }
            else
            {
                //Upgrading
                Command_Action command_Action_AddResources = new Command_Action();

                command_Action_AddResources.defaultLabel = "Open Vent";

                command_Action_AddResources.icon = UI_OPEN;
                command_Action_AddResources.defaultDesc = "Open Vent";

                command_Action_AddResources.activateSound = SoundDef.Named("Click");
                command_Action_AddResources.action = new Action(this.toggleVent);

                CommandList.Add(command_Action_AddResources);
            }


            return CommandList.AsEnumerable<Command>();
            //return compPowerTrader.CompGetCommandsExtra();
            //return base.GetCommands();
        }

        public void toggleVent()
        {
            if (this.compPowerTrader.PowerOn)
            {
                this.ventOpen = !this.ventOpen;
            }
            else
            {
                if (this.ventOpen)
                {
                    Messages.Message("Insufficient Power to Close Vent", MessageSound.Reject);
                }
                else
                {
                    Messages.Message("Insufficient Power to Open Vent", MessageSound.Reject);
                }
            }
        }
        */
        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (this.compPowerTrader.PowerOn)
            {
                stringBuilder.AppendLine("Vent Open");
            }
            else
            {
                stringBuilder.AppendLine("Vent Closed");
            }

            stringBuilder.AppendLine("EnergyLimit: " + energyLimit + " temperature1: " + temperature1 + " temperature2: " + temperature2 + " temperatureDiff: " + temperatureDiff);

            //Power info
            if (this.compPowerTrader != null)
            {
                //string str1 = (this.PowerNet.CurrentEnergyGainRate() / CompPower.WattsToWattDaysPerTick).ToString("F0");
                stringBuilder.AppendLine("Power: " + -this.compPowerTrader.powerOutputInt + " / " + (this.compPowerTrader.PowerNet.CurrentEnergyGainRate() / CompPower.WattsToWattDaysPerTick) + " - Stored: " + this.compPowerTrader.PowerNet.CurrentStoredEnergy().ToString("F0"));
            }

            return stringBuilder.ToString();
        }

    }
}
