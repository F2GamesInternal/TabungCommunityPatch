using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Photon.Pun;
using Photon.Voice.PUN;
using System;
using UnityEngine;

namespace OfflinePhoton
{
    [BepInPlugin("tabung.offline", "The Tabung Offline", "1.0.0")]
    public sealed class Plugin : BaseUnityPlugin
    {
        internal static BepInEx.Logging.ManualLogSource Log;
        internal static ConfigEntry<string> Mode;
        internal static ConfigEntry<string> AppIdRealtime;
        internal static ConfigEntry<string> AppIdVoice;
        internal static ConfigEntry<string> Region;
        internal static ConfigEntry<string> Nickname;

        internal static bool IsOnlineMode => (Mode?.Value ?? "Offline").Trim().Equals("Online", StringComparison.OrdinalIgnoreCase);
        internal static bool IsOfflineMode => !IsOnlineMode;

        private void Awake()
        {
            Log = Logger;
            Mode = Config.Bind("General", "Mode", "Offline", "Offline, Online, or Original. Use Online to connect to your own Photon App ID.");
            AppIdRealtime = Config.Bind("Photon", "AppIdRealtime", "", "Photon App ID Realtime to use when Mode = Online.");
            AppIdVoice = Config.Bind("Photon", "AppIdVoice", "", "Photon Voice App ID to use when Mode = Online.");
            Region = Config.Bind("Photon", "Region", "", "Fixed Photon region to use when Mode = Online. Leave blank for auto.");
            Nickname = Config.Bind("General", "Nickname", "Player", "Nickname used by the game and Photon.");

            Logger.LogInfo("OfflinePhoton v1.0 loaded");
            ApplyPhotonConfig();
            new Harmony("tabung.offline").PatchAll(typeof(Plugin).Assembly);
            CreateDebugConsole();
        }

        private static void ApplyPhotonConfig()
        {
            if (PhotonNetwork.PhotonServerSettings == null || PhotonNetwork.PhotonServerSettings.AppSettings == null)
            {
                Log?.LogWarning("PhotonServerSettings/AppSettings not found.");
                return;
            }

            var appIdRealtime = (AppIdRealtime?.Value ?? "").Trim();
            var appIdVoice = (AppIdVoice?.Value ?? "").Trim();
            var region = (Region?.Value ?? "").Trim();
            var nickname = (Nickname?.Value ?? "Player").Trim();


            PlayerPrefs.SetString("Nickname", nickname);
            PhotonNetwork.NickName = nickname;

            if (IsOnlineMode && !string.IsNullOrWhiteSpace(appIdRealtime))
            {
                var settings = PhotonNetwork.PhotonServerSettings.AppSettings;

                settings.AppIdRealtime = appIdRealtime;

                // If no separate Voice App ID was supplied,
                // reuse the Realtime App ID.
                settings.AppIdVoice = string.IsNullOrWhiteSpace(appIdVoice)
                    ? appIdRealtime
                    : appIdVoice;

                settings.FixedRegion = string.IsNullOrWhiteSpace(region)
                    ? null
                    : region;

                PhotonNetwork.OfflineMode = false;

                Log?.LogInfo($"Realtime AppId : {settings.AppIdRealtime}");
                Log?.LogInfo($"Voice AppId    : {settings.AppIdVoice}");
                Log?.LogInfo($"Region         : {(string.IsNullOrEmpty(settings.FixedRegion) ? "Auto" : settings.FixedRegion)}");
                Log?.LogInfo("Photon configured for Online mode.");
            }
            else
            {
                PhotonNetwork.OfflineMode = true;
                Log?.LogInfo("Photon configured for Offline mode.");
            }
        }

        private void CreateDebugConsole()
        {
            var go = new GameObject("OfflinePhoton Debug Console");
            DontDestroyOnLoad(go);
            go.hideFlags = HideFlags.HideAndDontSave;
            go.AddComponent<OfflinePhoton.Debug.DebugConsole>();
        }
    }
}
