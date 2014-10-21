using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace Enhanced_Defence.Utilities
{
    class Utilities
    {
        public static IEnumerable<Pawn> findPawns(IntVec3 position, float radius)
        {
            IEnumerable<Pawn> pawns = Find.ListerPawns.ColonistsAndPrisoners;
            IEnumerable<Pawn> closePawns;

            if (pawns != null)
            {
                closePawns = pawns.Where<Pawn>(t => t.Position.WithinHorizontalDistanceOf(position, radius));
                return closePawns;
            }
            return null;
        }

        static public Thing FindItemThingsInAutoLoader(Thing centerBuilding)
        {

            ThingDef thingDefHopper = ThingDef.Named("AutoLoader");
            //ThingDef thingDefAmmoType = ThingDef.Named("Shells");
            //ThingDef thingDefAmmoType = ThingDef.Named(this.ammoType);

            foreach (IntVec3 sq in GenAdj.AdjacentSquaresCardinal(centerBuilding))
            {
                Thing thingAmmo = (Thing)null;
                Thing thingContainer = (Thing)null;
                foreach (Thing tempThing in Find.ThingGrid.ThingsAt(sq))
                {
                    //if (tempThing is ThingWithComponents)
                    //{
                    //if (tempThing.def == ThingDefOf.Metal) ;
                    if (tempThing.def.category == EntityCategory.Item)
                    {
                        thingAmmo = tempThing;
                    }

                    if (tempThing.def == thingDefHopper)
                    {
                        thingContainer = tempThing;
                    }
                    //}
                }
                //if (thingAmmo != null && thingContainer != null && thingAmmo.stackCount >= this.ammoAmountUsedToFire)
                
                if (thingAmmo != null && thingContainer != null)
                {
                    return thingAmmo;
                }
            }
            return (Thing)null;
        }
    }
}

