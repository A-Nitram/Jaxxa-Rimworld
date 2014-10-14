using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enhanced_Defence.Shields
{
    public class ShieldThingDef : Verse.ThingDef
    {
        public int shieldMaxShieldStrength;
        public int shieldInitialShieldStrength;
        public int shieldShieldRadius;

        public int shieldPowerRequiredLoading;
        public int shieldPowerRequiredCharging;
        public int shieldPowerRequiredSustaining;

        public int shieldRechargeTickDelay;
        public int shieldRecoverWarmup;

        public bool shieldBlockIndirect;
        public bool shieldBlockDirect;
        public bool shieldFireSupression;
        public bool shieldInterceptDropPod;

        public bool shieldStructuralIntegrityMode;

        public float colourRed;
        public float colourGreen;
        public float colourBlue;
    }
}
