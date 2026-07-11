using System.Collections;
using HarmonyLib;

namespace OfflinePhoton.Patches
{
    [HarmonyPatch(typeof(UpdateChecker))]
    public static class UpdateCheckerPatches
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void AwakePostfix(UpdateChecker __instance)
        {
            UpdateChecker.UC = __instance;
            __instance.isLatest = true;
            Helpers.Log.Info("UpdateChecker forced to latest");
        }

        [HarmonyPatch("Check")]
        [HarmonyPrefix]
        private static bool CheckPrefix(UpdateChecker __instance, ref IEnumerator __result)
        {
            __instance.isLatest = true;
            __result = NoOp();
            return false;
        }

        [HarmonyPatch("UpdateLink")]
        [HarmonyPrefix]
        private static bool UpdateLinkPrefix()
        {
            Helpers.Log.Info("UpdateLink ignored");
            return false;
        }

        private static IEnumerator NoOp()
        {
            yield break;
        }
    }
}
