using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace OfflinePhoton.Patches
{
    [HarmonyPatch(typeof(Singleplayer), nameof(Singleplayer.StartSolo))]
    public static class SingleplayerPatch
    {
        static bool Prefix()
        {
            if (Plugin.IsOnlineMode)
                return true;

            Helpers.Log.Info("Singleplayer.StartSolo()");
            PlayerPrefs.SetInt("Solo", 1);
            PhotonNetwork.OfflineMode = true;

            if (PhotonRoom.room != null)
                PhotonRoom.room.multiplayerScene = 2;

            PhotonNetwork.CreateRoom("Offline Server", new RoomOptions
            {
                MaxPlayers = 1,
                IsVisible = false,
                IsOpen = true,
                PublishUserId = true
            });

            return false;
        }
    }
}
