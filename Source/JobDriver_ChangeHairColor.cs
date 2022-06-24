using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace HairDye
{
    public class JobDriver_ChangeHairColor : JobDriver
    {
        private Thing Dye => job.GetTarget(TargetIndex.A).Thing;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
//            if (BeamColorThing == null) return false;

            return pawn.Reserve(Dye, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {

            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);

            Toil dye = Toils_General.Wait(75, TargetIndex.None);
            dye.FailOnDespawnedOrNull(TargetIndex.A);
            dye.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            dye.WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
            dye.PlaySustainerOrSound(SoundDefOf.HairDyeBubbles);
            dye.tickAction = () =>
            {
               if (pawn.IsHashIntervalTick(5)) {
                    Vector3 vec = pawn.Position.ToVector3Shifted();
                    vec.z += 0.5f;

                    if (ticksLeftThisToil < 10)
                    {

                    }
                    else if (ticksLeftThisToil < 15)
                    {
                        EffectSpawnerDefOf.HairDyeEffectBubblesFinished.Spawn(pawn.Map, vec, 0);
                    }
                    else if (ticksLeftThisToil > 25)
                    {
                        EffectSpawnerDefOf.HairDyeEffectBubbles.Spawn(pawn.Map, vec, 0);
                    }
                }

            };
            yield return dye;
            yield return new Toil
            {
                initAction = delegate ()
                {
                    ThingCompHairDye comp = Dye.TryGetComp<ThingCompHairDye>();
                    if (comp == null) return;
                    if (comp.hairData == null) return;

                    comp.hairData.Write(pawn);

                    if (Dye.stackCount > 1) Dye.stackCount--;
                    else Dye.Destroy(DestroyMode.Vanish);

                    pawn.Drawer.renderer.graphics.SetAllGraphicsDirty();
                    PortraitsCache.SetDirty(pawn);
                }
            }.PlaySustainerOrSound(SoundDefOf.HairDyePop);
            yield break;
        }
    }
}
