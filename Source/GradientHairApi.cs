using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;

namespace HairDye
{
    class GradientHairApi
    {
        static private MethodInfo GradientHairApiMethod(string name)
        {
            Type type = Type.GetType("GradientHair.PublicApi, GradientHair");
            if (type == null) return null;

            return type.GetMethod(name);
        }
        static private MethodInfo GetGradientHairMethod = GradientHairApiMethod("GetGradientHair");
        static private MethodInfo SetGradientHairMethod = GradientHairApiMethod("SetGradientHair");

        public static bool GradientHairIsAvailable()
        {
            return GetGradientHairMethod != null;
        }

        public static bool GetGradientHair(Pawn pawn, out bool enabled, out Color color)
        {
            if (GetGradientHairMethod == null)
            {
                enabled = false;
                color = Color.white;
                return false;
            }

            try
            {
                object[] parameters = { pawn, null, null };
                bool res = (bool)GetGradientHairMethod.Invoke(null, parameters);

                enabled = (bool)parameters[1];
                color = (Color)parameters[2];

                return res;
            }
            catch (Exception)
            {
                enabled = false;
                color = Color.white;
                return false;
            }

        }

        public static void SetGradientHair(Pawn pawn, bool enabled, Color color)
        {
            if (SetGradientHairMethod == null) return;

            try
            {
                object[] parameters = { pawn, enabled, color };
                SetGradientHairMethod.Invoke(null, parameters);
            }
            catch (Exception)
            {
            }
        }
    }
}
