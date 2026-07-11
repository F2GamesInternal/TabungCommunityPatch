using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OfflinePhoton.Patches
{
    [HarmonyPatch(typeof(PhotonRoom))]
    public static class PhotonRoomPatches
    {
        [HarmonyPatch("Awake")]
        [HarmonyPrefix]
        private static bool AwakePrefix(PhotonRoom __instance)
        {
            if (__instance.PV == null)
                __instance.PV = __instance.GetComponent<PhotonView>();
            return true;
        }

        [HarmonyPatch("OnDisconnected")]
        [HarmonyPrefix]
        private static bool OnDisconnectedPrefix(PhotonRoom __instance, DisconnectCause cause)
        {
            if (Plugin.IsOnlineMode)
                return true;

            Helpers.Log.Warn("PhotonRoom.OnDisconnected(" + cause + ") ignored");
            return false;
        }

        [HarmonyPatch("StartGame")]
        [HarmonyPrefix]
        private static bool StartGamePrefix(PhotonRoom __instance)
        {
            if (Plugin.IsOnlineMode)
                return true;

            Helpers.Log.Info("PhotonRoom.StartGame() -> SceneManager.LoadScene(" + __instance.multiplayerScene + ")");
            SceneManager.LoadScene(__instance.multiplayerScene);
            return false;
        }

        [HarmonyPatch("OnJoinedRoom")]
        [HarmonyPrefix]
        private static bool OnJoinedRoomPrefix(PhotonRoom __instance)
        {
            if (Plugin.IsOnlineMode)
                return true;

            Helpers.Log.Info("PhotonRoom.OnJoinedRoom()");
            __instance.playersInRoom = PhotonNetwork.PlayerList.Length;
            __instance.myNumberInRoom = __instance.playersInRoom;
            __instance.isGameLoaded = true;
            return true;
        }
    }
}
