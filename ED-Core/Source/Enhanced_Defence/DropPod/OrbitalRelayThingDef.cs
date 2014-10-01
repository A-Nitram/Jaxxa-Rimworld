using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enhanced_Defence.DropPod
{
    public class OrbitalRelayThingDef : Verse.ThingDef
    {
        public bool DropPodDeepStrike;
        public bool DropPodAddUnits;
        public bool DropPodAddResources;

        public int DropPodAddUnitRadius;
    }
}
