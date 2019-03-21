using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace HairDye
{
    public class ThingCompHairDye : ThingComp
    {
        public PawnHairData hairData;
        public EffectSpawnerDef effect;

        public override void PostExposeData()
        {
            Scribe_Deep.Look(ref hairData, "hairDyeData");
        }

        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn pawn) {
            string caption = "HairDyeDye".Translate();

            yield return new FloatMenuOption(caption, delegate ()
            {
                Find.WindowStack.Add(new WindowChooseHairColor(this, parent, pawn));
            });
            yield break;
        }
    }
}
