using HarmonyLib;
using Kingmaker.Inspect;

namespace InspectEverything
{
    [HarmonyPatch(typeof(InspectUnitsHelper), nameof(InspectUnitsHelper.IsInspectAllow))]
    public class InspectUnitsHelper_GetBody_Patch
    {
        public static bool Prefix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }
}
