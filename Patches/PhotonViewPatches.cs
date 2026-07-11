using HarmonyLib;
using Photon.Pun;

namespace OfflinePhoton.Patches
{
    [HarmonyPatch]
    public static class PhotonViewRpcPatch1
    {
        static System.Reflection.MethodBase TargetMethod() => AccessTools.Method(typeof(PhotonView), nameof(PhotonView.RPC), new[] { typeof(string), typeof(RpcTarget), typeof(object[]) });
        static bool Prefix(PhotonView __instance, string methodName, RpcTarget target, object[] parameters)
        {
            if (Plugin.IsOnlineMode)
                return true;

            Helpers.Log.Info("PhotonView.RPC(" + methodName + ", " + target + ")");
            Helpers.OfflineRpcDispatcher.InvokeLocal(__instance, methodName, parameters);
            return false;
        }
    }

    [HarmonyPatch]
    public static class PhotonViewRpcPatch2
    {
        static System.Reflection.MethodBase TargetMethod() => AccessTools.Method(typeof(PhotonView), nameof(PhotonView.RPC), new[] { typeof(string), typeof(Photon.Realtime.Player), typeof(object[]) });
        static bool Prefix(PhotonView __instance, string methodName, Photon.Realtime.Player targetPlayer, object[] parameters)
        {
            if (Plugin.IsOnlineMode)
                return true;

            Helpers.Log.Info("PhotonView.RPC(" + methodName + ", Player)");
            Helpers.OfflineRpcDispatcher.InvokeLocal(__instance, methodName, parameters);
            return false;
        }
    }

    [HarmonyPatch]
    public static class PhotonViewRpcSecurePatch1
    {
        static System.Reflection.MethodBase TargetMethod() => AccessTools.Method(typeof(PhotonView), nameof(PhotonView.RpcSecure), new[] { typeof(string), typeof(RpcTarget), typeof(bool), typeof(object[]) });
        static bool Prefix(PhotonView __instance, string methodName, RpcTarget target, bool encrypt, object[] parameters)
        {
            if (Plugin.IsOnlineMode)
                return true;

            Helpers.Log.Info("PhotonView.RpcSecure(" + methodName + ", " + target + ")");
            Helpers.OfflineRpcDispatcher.InvokeLocal(__instance, methodName, parameters);
            return false;
        }
    }

    [HarmonyPatch]
    public static class PhotonViewRpcSecurePatch2
    {
        static System.Reflection.MethodBase TargetMethod() => AccessTools.Method(typeof(PhotonView), nameof(PhotonView.RpcSecure), new[] { typeof(string), typeof(Photon.Realtime.Player), typeof(bool), typeof(object[]) });
        static bool Prefix(PhotonView __instance, string methodName, Photon.Realtime.Player targetPlayer, bool encrypt, object[] parameters)
        {
            if (Plugin.IsOnlineMode)
                return true;

            Helpers.Log.Info("PhotonView.RpcSecure(" + methodName + ", Player)");
            Helpers.OfflineRpcDispatcher.InvokeLocal(__instance, methodName, parameters);
            return false;
        }
    }
}
