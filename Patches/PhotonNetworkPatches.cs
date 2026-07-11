using System;
using HarmonyLib;
using Photon.Pun;
using UnityEngine.SceneManagement;

namespace OfflinePhoton.Patches
{
    [HarmonyPatch]
    public static class PhotonNetworkConnectUsingSettingsPatch
    {
        static System.Reflection.MethodBase TargetMethod() => AccessTools.Method(typeof(PhotonNetwork), nameof(PhotonNetwork.ConnectUsingSettings), Type.EmptyTypes);

        static bool Prefix(ref bool __result)
        {
            if (Plugin.IsOnlineMode)
                return true;

            Helpers.Log.Info("PhotonNetwork.ConnectUsingSettings() -> OfflineMode");
            PhotonNetwork.OfflineMode = true;
            __result = true;
            return false;
        }
    }

    [HarmonyPatch]
    public static class PhotonNetworkConnectUsingSettingsOverloadPatch
    {
        static System.Reflection.MethodBase TargetMethod() => AccessTools.Method(typeof(PhotonNetwork), nameof(PhotonNetwork.ConnectUsingSettings), new[] { typeof(Photon.Realtime.AppSettings), typeof(bool) });

        static bool Prefix(ref bool startInOfflineMode, ref bool __result)
        {
            if (Plugin.IsOnlineMode)
                return true;

            Helpers.Log.Info("PhotonNetwork.ConnectUsingSettings(AppSettings,bool) -> OfflineMode");
            startInOfflineMode = true;
            PhotonNetwork.OfflineMode = true;
            __result = true;
            return false;
        }
    }

    [HarmonyPatch]
    public static class PhotonNetworkJoinLobbyPatch
    {
        static System.Reflection.MethodBase TargetMethod() => AccessTools.Method(typeof(PhotonNetwork), nameof(PhotonNetwork.JoinLobby), Type.EmptyTypes);

        static bool Prefix(ref bool __result)
        {
            if (Plugin.IsOnlineMode)
                return true;

            Helpers.Log.Info("PhotonNetwork.JoinLobby() -> OfflineMode");
            PhotonNetwork.OfflineMode = true;
            __result = true;
            return false;
        }
    }

    [HarmonyPatch]
    public static class PhotonNetworkLoadLevelIntPatch
    {
        static System.Reflection.MethodBase TargetMethod() => AccessTools.Method(typeof(PhotonNetwork), nameof(PhotonNetwork.LoadLevel), new[] { typeof(int) });

        static bool Prefix(int levelNumber)
        {
            if (Plugin.IsOnlineMode)
                return true;

            Helpers.Log.Info("PhotonNetwork.LoadLevel(" + levelNumber + ")");
            SceneManager.LoadScene(levelNumber);
            return false;
        }
    }

    [HarmonyPatch]
    public static class PhotonNetworkLoadLevelStringPatch
    {
        static System.Reflection.MethodBase TargetMethod() => AccessTools.Method(typeof(PhotonNetwork), nameof(PhotonNetwork.LoadLevel), new[] { typeof(string) });

        static bool Prefix(string levelName)
        {
            if (Plugin.IsOnlineMode)
                return true;

            Helpers.Log.Info("PhotonNetwork.LoadLevel(" + levelName + ")");
            SceneManager.LoadScene(levelName);
            return false;
        }
    }
}
