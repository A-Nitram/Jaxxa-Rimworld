using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

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
    }
}
