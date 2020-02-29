using HarmonyLib;
using System.Reflection;
using Verse;

namespace HairDye
{
    [StaticConstructorOnStartup]
    public class HairDye
    {

        static HairDye()
        {
            var harmony = new Harmony("com.github.automatic1111.hairdye");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
