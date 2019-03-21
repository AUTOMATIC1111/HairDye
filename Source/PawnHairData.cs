using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace HairDye
{
    public class PawnHairData : IExposable
    {
        public Color colorA;
        public Color colorB;
        public bool gradientHairEnabled;

        public void ExposeData()
        {
            Scribe_Values.Look(ref colorA, "dyeHairColorA");
            Scribe_Values.Look(ref colorB, "dyeHairColorB");
            Scribe_Values.Look(ref gradientHairEnabled, "dyeHairGradientHairEnabled");
        }

        public void Read(Pawn pawn) {
            colorA = pawn.story.hairColor;
            GradientHairApi.GetGradientHair(pawn, out gradientHairEnabled, out colorB);
        }

        public void Write(Pawn pawn) {
            pawn.story.hairColor = colorA;
            GradientHairApi.SetGradientHair(pawn, gradientHairEnabled, colorB);
        }
    }
}
