using HarmonyLib;
using UnityEngine;

namespace OfflinePhoton.Patches
{
    [HarmonyPatch(typeof(RoundStartScreen))]
    public static class RoundStartScreenPatches
    {
        [HarmonyPatch("Awake")]
        [HarmonyPrefix]
        private static bool AwakePrefix(RoundStartScreen __instance)
        {
            if (Plugin.IsOnlineMode)
                return true;

            if (PlayerPrefs.GetInt("Solo", 0) != 1) return true;
            Helpers.Log.Info("RoundStartScreen.Awake()");
            if (__instance.StartNow != null) __instance.StartNow.SetActive(false);
            if (__instance.gameObject != null) __instance.gameObject.SetActive(false);
            return false;
        }

        [HarmonyPatch("Start")]
        [HarmonyPrefix]
        private static bool StartPrefix()
        {
            return Plugin.IsOnlineMode || PlayerPrefs.GetInt("Solo", 0) != 1;
        }

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        private static bool UpdatePrefix()
        {
            return Plugin.IsOnlineMode || PlayerPrefs.GetInt("Solo", 0) != 1;
        }
    }
}
