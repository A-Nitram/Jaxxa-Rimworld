using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enhanced_Defence.TurretAmmo
{
    public class TurretAmmoThingDef : Verse.ThingDef
    {
        public string ammoType;
        public int ammoAmountUsedToFire;
        public bool internalAmmoEnabled;

        public int internalAmmoMAX;
        public int internalAmmoMultiplier;
        public int internalAmmoCurrent;
    }
}
