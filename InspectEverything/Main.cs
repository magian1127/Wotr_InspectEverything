using HarmonyLib;
using System.Reflection;
using UnityModManagerNet;

namespace InspectEverything
{
    public class Main
    {
        public static bool Enabled;

        public static Harmony harmony;
        public static UnityModManager.ModEntry.ModLogger logger;
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            logger = modEntry.Logger;

            modEntry.OnToggle = OnToggle;

            harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            return true;
        }
    }
}
