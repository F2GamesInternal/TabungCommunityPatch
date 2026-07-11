using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace OfflinePhoton.Patches
{
    [HarmonyPatch(typeof(PhotonLobby))]
    public static class PhotonLobbyPatches
    {
        [HarmonyPatch("OnConnectedToMaster")]
        [HarmonyPrefix]
        private static bool OnConnectedToMasterPrefix(PhotonLobby __instance)
        {
            if (Plugin.IsOnlineMode)
                return true;

            Helpers.Log.Info("PhotonLobby.OnConnectedToMaster()");
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.NickName = PlayerPrefs.GetString("Nickname");
            __instance.Connected = true;

            if (__instance.RoomListScreen != null) __instance.RoomListScreen.SetActive(true);
            if (__instance.ConnectScreen != null) __instance.ConnectScreen.SetActive(false);
            return false;
        }

        [HarmonyPatch("OnDisconnected")]
        [HarmonyPrefix]
        private static bool OnDisconnectedPrefix(PhotonLobby __instance, DisconnectCause cause)
        {
            if (Plugin.IsOnlineMode)
                return true;

            Helpers.Log.Warn("PhotonLobby.OnDisconnected(" + cause + ") ignored");
            __instance.Connected = false;
            return false;
        }

        [HarmonyPatch("LoadRoomList")]
        [HarmonyPrefix]
        private static bool LoadRoomListPrefix(PhotonLobby __instance)
        {
            if (Plugin.IsOnlineMode)
                return true;

            Helpers.Log.Info("PhotonLobby.LoadRoomList()");
            PhotonNetwork.OfflineMode = true;
            if (__instance.ConnectScreen != null) __instance.ConnectScreen.SetActive(true);
            if (__instance.RoomListScreen != null) __instance.RoomListScreen.SetActive(false);
            return false;
        }

        [HarmonyPatch("JoinaRandomRoom")]
        [HarmonyPrefix]
        private static bool JoinaRandomRoomPrefix(PhotonLobby __instance)
        {
            if (Plugin.IsOnlineMode)
                return true;

            Helpers.Log.Info("PhotonLobby.JoinaRandomRoom()");
            PhotonNetwork.OfflineMode = true;
            if (PhotonRoom.room != null) PhotonRoom.room.multiplayerScene = 2;
            if (__instance.LoadingMultiplayer != null) __instance.LoadingMultiplayer.SetActive(true);
            PhotonNetwork.JoinRandomRoom();
            return false;
        }

        [HarmonyPatch("JoinGame")]
        [HarmonyPrefix]
        private static bool JoinGamePrefix(PhotonLobby __instance, string ServerName, byte MaxPlayers, int Map, int Servers, bool PrivateServer, bool AutomaticStart, bool PVP, bool Nightmare)
        {
            if (Plugin.IsOnlineMode)
                return true;

            Helpers.Log.Info("PhotonLobby.JoinGame(...)");
            PlayerPrefs.SetInt("Solo", 1);
            PhotonNetwork.OfflineMode = true;

            var tr = Traverse.Create(__instance);
            tr.Field("privateServer").SetValue(PrivateServer);
            tr.Field("automaticStart").SetValue(AutomaticStart);
            tr.Field("serverName").SetValue(string.IsNullOrEmpty(ServerName) ? "Offline Server" : ServerName);
            tr.Field("maxPlayers").SetValue(MaxPlayers);
            tr.Field("serverScene").SetValue(Map);
            tr.Field("maxTries").SetValue(Servers);
            tr.Field("nightmare").SetValue(Nightmare);
            tr.Field("pvp").SetValue(PVP);
            tr.Field("tries").SetValue(0);

            if (PhotonRoom.room != null) PhotonRoom.room.multiplayerScene = Map;
            if (__instance.LoadingMultiplayer != null) __instance.LoadingMultiplayer.SetActive(true);

            PhotonNetwork.JoinRoom(string.IsNullOrEmpty(ServerName) ? "Offline Server" : ServerName, null);
            return false;
        }

        [HarmonyPatch("JoinRoom")]
        [HarmonyPrefix]
        private static bool JoinRoomPrefix(PhotonLobby __instance, string name)
        {
            if (Plugin.IsOnlineMode)
                return true;

            Helpers.Log.Info("PhotonLobby.JoinRoom(" + name + ")");
            PhotonNetwork.OfflineMode = true;
            if (__instance.LoadingMultiplayer != null) __instance.LoadingMultiplayer.SetActive(true);
            PhotonNetwork.JoinRoom(name, null);
            return false;
        }
    }
}
