using System.Collections;
using HarmonyLib;
using UnityEngine;

namespace OfflinePhoton.Patches
{
    [HarmonyPatch(typeof(JoinObject))]
    public static class JoinObjectPatches
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void StartPostfix(JoinObject __instance)
        {
            if (Plugin.IsOnlineMode)
                return;

            if (PlayerPrefs.GetInt("Solo", 0) != 1) return;
            __instance.StartCoroutine(OfflineAutoStart(__instance));
        }

        private static IEnumerator OfflineAutoStart(JoinObject inst)
        {
            yield return new WaitForEndOfFrame();
            if (GameSetup.GS == null || inst == null || GameSetup.GS.RoundStarted) yield break;
            Helpers.Log.Info("JoinObject -> offline RoundStart(0)");
            inst.RoundStart(0);
        }
    }
}
