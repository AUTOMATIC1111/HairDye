using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace HairDye
{
    class GradientHairApi
    {
        public static bool GradientHairIsAvailable()
        {
            try
            {
                return GenTypes.GetTypeInAnyAssembly("GradientHair.PublicApi") != null;
            }
            catch (TypeLoadException)
            {
                return false;
            }
        }

        public static bool GetGradientHair(Pawn pawn, out bool enabled, out Color color)
        {
            try
            {
                return GradientHairApiInner.GetGradientHair(pawn, out enabled, out color);
            }
            catch (TypeLoadException)
            {
                enabled = false;
                color = Color.white;
                return false;
            }

        }

        public static void SetGradientHair(Pawn pawn, bool enabled, Color color)
        {
            try
            {
                GradientHairApiInner.SetGradientHair(pawn, enabled, color);
            }
            catch (TypeLoadException)
            {
            }
        }
    }

    class GradientHairApiInner
    {
        public static bool GetGradientHair(Pawn pawn, out bool enabled, out Color color)
        {
            return GradientHair.PublicApi.GetGradientHair(pawn, out enabled, out color);
        }

        public static void SetGradientHair(Pawn pawn, bool enabled, Color color)
        {
            GradientHair.PublicApi.SetGradientHair(pawn, enabled, color);
        }
    }
}
