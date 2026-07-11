using HarmonyLib;
using UnityEngine;

namespace OfflinePhoton.Patches
{
    [HarmonyPatch(typeof(GameSetup))]
    public static class GameSetupPatches
    {
        [HarmonyPatch("Awake")]
        [HarmonyPrefix]
        private static bool AwakePrefix(GameSetup __instance)
        {
            if (Plugin.IsOnlineMode)
                return true;

            GameSetup.GS = __instance;
            __instance.Name = PlayerPrefs.GetString("Nickname");
            __instance.ID = "Offline";
            __instance.FriendlyFire = false;
            __instance.GotInfo = true;
            __instance.RoundStarted = false;
            __instance.ActiveTimer = false;
            __instance.Chatting = false;
            __instance.Dead = false;
            __instance.Camper = true;
            __instance.IsTraitor = false;

            Traverse.Create(__instance).Field("roundOver").SetValue(false);

            if (__instance.ServerNameText != null) __instance.ServerNameText.text = "Offline";
            if (__instance.RoundStartScreen != null) __instance.RoundStartScreen.SetActive(false);
            if (__instance.Controls != null) __instance.Controls.SetActive(false);
            if (__instance.SpectatorControls != null) __instance.SpectatorControls.SetActive(false);
            if (__instance.Chat != null) __instance.Chat.SetActive(true);
            if (__instance.Observer != null) __instance.Observer.SetActive(false);

            return false;
        }
    }
}
