using System;
using System.Linq;
using System.Reflection;
using Photon.Pun;
using UnityEngine;

namespace OfflinePhoton.Helpers
{
    public static class OfflineRpcDispatcher
    {
        public static bool InvokeLocal(PhotonView view, string methodName, object[] parameters)
        {
            if (view == null || string.IsNullOrEmpty(methodName))
                return false;

            var args = parameters ?? Array.Empty<object>();
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var invoked = false;

            foreach (var comp in view.GetComponents<MonoBehaviour>())
            {
                if (comp == null) continue;

                var methods = comp.GetType().GetMethods(flags).Where(m => m.Name == methodName).ToArray();
                foreach (var m in methods)
                {
                    try
                    {
                        if (m.GetParameters().Length != args.Length) continue;
                        m.Invoke(comp, args);
                        invoked = true;
                    }
                    catch (Exception ex)
                    {
                        Log.Error("RPC invoke failed: " + comp.GetType().Name + "." + methodName + " -> " + ex.Message);
                    }
                }
            }

            if (!invoked)
                Log.Warn("RPC '" + methodName + "' not found on " + view.name);

            return invoked;
        }
    }
}
