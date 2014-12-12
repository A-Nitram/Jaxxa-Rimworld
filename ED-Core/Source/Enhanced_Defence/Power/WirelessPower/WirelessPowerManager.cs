using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Enhanced_Defence.Power.WirelessPower
{
    static class WirelessPowerManager
    {

        private static int previousTickExecuted = 0;

        public static float CurrentAvalablePower = 0;
        static List<WirelessPowerNode> activeNodes = new List<WirelessPowerNode>();
        static List<WirelessPowerNode> nodesToRemove = new List<WirelessPowerNode>();

        static public bool suficentPower = false;

        static List<WirelessPowerNode> getCurrentNodes()
        {
            List<WirelessPowerNode> currentNodes = new List<WirelessPowerNode>();

            foreach (WirelessPowerNode currentNode in activeNodes)
            {
                if (currentNode != null)
                {
                    currentNodes.Add(currentNode);
                }
                else
                {
                    nodesToRemove.Add(currentNode);
                }
            }

            if (nodesToRemove.Count > 0)
            {
                foreach (WirelessPowerNode currentNode in nodesToRemove)
                {
                    activeNodes.Remove(currentNode);
                }
                nodesToRemove.Clear();
            }

            return currentNodes;
        }

        static void recalculatePowerDraw()
        {
            nodesToRemove.Clear();

            CurrentAvalablePower = 0;
            foreach (WirelessPowerNode currentNode in getCurrentNodes())
            {
                if (currentNode.WantsToTransmit())
                {
                    //Sending power
                    if (currentNode.power.PowerOn)
                    {
                        //CurrentAvalablePower -= currentNode.desiredPower;
                        CurrentAvalablePower -= currentNode.power.powerOutput;
                    }
                }
                else
                {
                    //Receiving power
                    if (currentNode.power.DesirePowerOn)
                    {
                        CurrentAvalablePower -= currentNode.desiredPower;
                    }
                }
            }

            if (CurrentAvalablePower >= 0)
            {
                WirelessPowerManager.suficentPower = true;
            }
            else
            {
                if (WirelessPowerManager.suficentPower)
                {
                    Messages.Message("Wireless Power Grid has collapsed.", MessageSound.Negative);
                }
                WirelessPowerManager.suficentPower = false;
            }

            //Log.Message("CurrentPower: " + CurrentAvalablePower);
        }

        public static bool registerToGrid(WirelessPowerNode powerNode)
        {
            if (getCurrentNodes().Contains(powerNode))
            {
                //Note already registered
                return false;
            }
            else
            {
                activeNodes.Add(powerNode);
                return true;
            }
        }

        //public static void TickRare()
        public static void Tick()
        {
            int currentTick = Find.TickManager.TicksGame;

            //Check to not have multiple ticks at the same time
            if (currentTick != previousTickExecuted)
            {
                //Record the previous tick
                previousTickExecuted = currentTick;

                WirelessPowerManager.recalculatePowerDraw();
            }


        }
    }
}
