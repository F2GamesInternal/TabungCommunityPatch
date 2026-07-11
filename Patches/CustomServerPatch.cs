using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace OfflinePhoton.Patches
{
    [HarmonyPatch(typeof(CustomServer), nameof(CustomServer.JoinServer))]
    public static class CustomServerPatch
    {
        static bool Prefix(CustomServer __instance)
        {
            if (Plugin.IsOnlineMode)
                return true;

            Helpers.Log.Info("CustomServer.JoinServer()");
            if (__instance.Price > PlayerPrefs.GetInt("Tokens", 0))
                return false;

            PlayerPrefs.SetInt("Solo", 1);
            PhotonNetwork.OfflineMode = true;

            if (PhotonRoom.room != null)
                PhotonRoom.room.multiplayerScene = __instance.MapSceneIndex;

            var roomName = string.IsNullOrEmpty(__instance.ServerName) ? "Offline Server" : __instance.ServerName;
            PhotonNetwork.CreateRoom(roomName, new RoomOptions
            {
                MaxPlayers = __instance.MaxPlayers,
                IsVisible = !__instance.HiddenServerToggle.isOn,
                IsOpen = true,
                PublishUserId = true
            });

            return false;
        }
    }
}
