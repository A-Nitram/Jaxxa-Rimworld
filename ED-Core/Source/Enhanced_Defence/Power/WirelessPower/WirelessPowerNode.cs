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

        public float MAX_DISTANCE = 2.0f;
        public bool flag_charge = false;
        CompPowerTrader power;
        //NanoConnector nanoConnector;

        private static Texture2D UI_POWER_UP;
        private static Texture2D UI_POWER_DOWN;

        #endregion
    }
}
