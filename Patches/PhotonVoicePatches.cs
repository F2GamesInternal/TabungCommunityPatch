using HarmonyLib;
using Photon.Voice.PUN;

namespace OfflinePhoton.Patches
{
    [HarmonyPatch(typeof(PhotonVoiceNetwork))]
    class PhotonVoicePatches
    {
        [HarmonyPatch("ConnectAndJoinRoom")]
        [HarmonyPrefix]
        static void Prefix()
        {
            Helpers.Log.Info("Preparing Photon Voice...");

            // We'll overwrite AppSettings here.
        }
    }
}