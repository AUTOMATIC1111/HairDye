using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse.Sound;
using Verse;
using RimWorld;

namespace HairDye
{
    public class EffectSpawnerDef : Def
    {
        public FloatRange offset;
        public List<SubEffect> subEffects;
        public List<EffectSpawnerDef> alsoSpawn;

        public static void Spawn(EffectSpawnerDef effect, Map map, Vector3 position, float angle = 0f)
        {
            if (effect != null)
                effect.Spawn(map, position, angle);
        }

        public static void Spawn(EffectSpawnerDef effect, Pawn pawn, float angle = 0f)
        {
            Spawn(effect, pawn.Map, pawn.TrueCenter(), angle);
        }

        public void Spawn(Map map, Vector3 position, float angle)
        {
            if (map == null) return;

            Vector3 origin = position + new Vector3(1f, 0f, 0f).RotatedBy(Rand.Range(0f, 360f)) * offset.RandomInRange;

            if (subEffects != null)
            {
                foreach (var subEffect in subEffects)
                {
                    subEffect.Spawn(map, origin, angle);
                }
            }

            if (alsoSpawn != null)
            {
                foreach (var spawner in alsoSpawn)
                {
                    spawner.Spawn(map, origin, angle);
                }
            }
        }
    }

    public abstract class SubEffect
    {
        public abstract void Spawn(Map map, Vector3 position, float angle);
    }

    public class SubEffectSound : SubEffect
    {
        public SoundDef sound;

        public override void Spawn(Map map, Vector3 position, float angle)
        {
            sound.PlayOneShot(new TargetInfo(position.ToIntVec3(), map, false));
        }
    }

    public class SubEffectRadial : SubEffect
    {
        public ThingDef mote;
        public int moteCount;
        public FloatRange radius;
        public FloatRange scale;
        public FloatRange rotationRate;
        public float speed;
        public float arc = 360f;

        public override void Spawn(Map map, Vector3 origin, float initialAngle)
        {
            if (map == null) return;

            for (int i = 0; i < moteCount; i++)
            {
                MoteThrown moteThrown = ThingMaker.MakeThing(mote, null) as MoteThrown;
                if (moteThrown == null) return;

                moteThrown.thingIDNumber = -1 - i;

                float angle = initialAngle + Rand.Range(- arc / 2, arc / 2);
                Vector3 dir = new Vector3(0f, 0f, 1f).RotatedBy(angle);

                float magnitude = Rand.Range(0f, 1f);
                float actualScale = scale.LerpThroughRange(magnitude);
                moteThrown.exactPosition = origin + dir * radius.LerpThroughRange(magnitude);
                moteThrown.exactRotation = angle;
                moteThrown.linearScale = new Vector3(actualScale, actualScale, actualScale);
                moteThrown.SetVelocity(angle, magnitude * speed);
                moteThrown.rotationRate = rotationRate.RandomInRange;
                GenSpawn.Spawn(moteThrown, origin.ToIntVec3(), map, WipeMode.Vanish);
                moteThrown.spawnTick -= (int)(magnitude * 0.75f * (mote.mote.fadeOutTime * 60));
            }
        }
    }

    [DefOf]
    public static class EffectSpawnerDefOf {
        static EffectSpawnerDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(EffectSpawnerDefOf));
        }

        public static EffectSpawnerDef HairDyeEffectBubbles;
        public static EffectSpawnerDef HairDyeEffectBubblesFinished;
    }
}
