using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

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

        public override void SpawnSetup()
        {
            base.SpawnSetup();
            this.compTempControl = this.GetComp<CompTempControl>();
            this.compPowerTrader = this.GetComp<CompPowerTrader>();
        }

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
            IntVec3 intVec3_1 = this.Position + Gen.RotatedBy(IntVec3.south, this.Rotation);
            IntVec3 intVec3_2 = this.Position + Gen.RotatedBy(IntVec3.north, this.Rotation);

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

                energyLimit = Math.Abs (temperatureDiff *25.0f);

                float maxEnergyLimit = 1000.0f;

                if (energyLimit > maxEnergyLimit)
                {
                    energyLimit = maxEnergyLimit;
                }

                //Log.Message("Limit: " + energyLimit + " temperature1 " + temperature1 + " temperature2 " + temperature2);

                if (temperature1 > temperature2)
                {
                    GenTemperature.TryControlTemperature(intVec3_1, -energyLimit, temperature2);
                    GenTemperature.TryControlTemperature(intVec3_2, energyLimit, temperature1);
                }
                else
                {
                    GenTemperature.TryControlTemperature(intVec3_1, energyLimit, temperature2);
                    GenTemperature.TryControlTemperature(intVec3_2, -energyLimit, temperature1);
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

        public override IEnumerable<Command> GetCommands()
        {
            return compPowerTrader.CompGetCommandsExtra();
            //return base.GetCommands();
        }

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
                stringBuilder.AppendLine("Power: " + -this.compPowerTrader.powerOutput + " / " + (this.compPowerTrader.PowerNet.CurrentEnergyGainRate() / CompPower.WattsToWattDaysPerTick) + " - Stored: " + this.compPowerTrader.PowerNet.CurrentStoredEnergy().ToString("F0"));
            }

            return stringBuilder.ToString();
        }

    }
}
