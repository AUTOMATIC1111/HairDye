using Harmony;
using System.Reflection;
using Verse;

namespace HairDye
{
    [StaticConstructorOnStartup]
    public class HairDye
    {

        static HairDye()
        {
            var harmony = HarmonyInstance.Create("com.github.automatic1111.hairdye");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Log.Message("Available: " + GradientHairApi.GradientHairIsAvailable());
        }
    }
}
