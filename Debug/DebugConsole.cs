using System;
using System.Collections.Generic;
using UnityEngine;

namespace OfflinePhoton.Debug
{
    public sealed class DebugConsole : MonoBehaviour
    {
        private struct LogEntry
        {
            public string Message;
            public string Stack;
            public LogType Type;
        }

        private readonly List<LogEntry> logs = new List<LogEntry>();
        private Vector2 scroll;
        private bool visible = false;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Application.logMessageReceived += HandleLog;
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
                visible = !visible;
        }

        private void HandleLog(string condition, string stackTrace, LogType type)
        {
            logs.Add(new LogEntry { Message = condition, Stack = stackTrace, Type = type });
            if (logs.Count > 600)
                logs.RemoveAt(0);
        }

        private void OnGUI()
        {
            if (!visible)
                return;

            GUI.depth = -1000;
            GUILayout.BeginArea(new Rect(10, 10, 920, 500), "OfflinePhoton", GUI.skin.window);
            scroll = GUILayout.BeginScrollView(scroll, GUILayout.ExpandHeight(true));

            for (int i = 0; i < logs.Count; i++)
            {
                var log = logs[i];
                GUI.contentColor = GetColor(log.Type);
                GUILayout.Label("[" + log.Type + "] " + log.Message);

                if (!string.IsNullOrEmpty(log.Stack))
                {
                    GUI.contentColor = Color.white;
                    GUILayout.Label(log.Stack);
                }

                GUILayout.Space(4);
            }

            GUILayout.EndScrollView();

            GUI.contentColor = Color.white;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear", GUILayout.Width(100)))
                logs.Clear();
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private static Color GetColor(LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                case LogType.Exception:
                    return Color.red;
                case LogType.Warning:
                    return Color.yellow;
                default:
                    return Color.white;
            }
        }
    }
}
