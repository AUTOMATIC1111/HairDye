using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace HairDye
{
    [DefOf]
    public static class SoundDefOf
    {
        static SoundDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(SoundDefOf));
        }

        public static SoundDef HairDyeBubbles;
        public static SoundDef HairDyePop;
    }

}
