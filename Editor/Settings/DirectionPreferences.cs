using UnityEditor;
using UnityEngine;

namespace AV.Direction.Editor.Settings
{
    public static class DirectionPreferences
    {
        private const string PREFIX = "AV_Direction_";

        public static float HandleSize
        {
            get => EditorPrefs.GetFloat(PREFIX + "HandleSize", 1.5f);
            set => EditorPrefs.SetFloat(PREFIX + "HandleSize", Mathf.Clamp(value, 0.1f, 3f));
        }

        public static float Opacity
        {
            get => EditorPrefs.GetFloat(PREFIX + "Opacity", 1.0f);
            set => EditorPrefs.SetFloat(PREFIX + "Opacity", Mathf.Clamp(value, 0.1f, 1f));
        }

        public static bool ShowGizmosAlways
        {
            get => EditorPrefs.GetBool(PREFIX + "ShowGizmosAlways", false);
            set => EditorPrefs.SetBool(PREFIX + "ShowGizmosAlways", value);
        }

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            var provider = new SettingsProvider("Preferences/AV Direction", SettingsScope.User)
            {
                label = "AV Direction",
                guiHandler = (searchContext) =>
                {
                    EditorGUILayout.LabelField("Visual Settings", EditorStyles.boldLabel);
                    HandleSize = EditorGUILayout.Slider(new GUIContent("Handle Scale", "Adjusts the size of interactive handles in the Scene View."), HandleSize, 0.1f, 3f);
                    Opacity = EditorGUILayout.Slider(new GUIContent("Global Opacity", "Controls the overall transparency of all direction visuals."), Opacity, 0.1f, 1f);
                    ShowGizmosAlways = EditorGUILayout.Toggle(new GUIContent("Show Gizmos Always", "If enabled, gizmos will be visible even when the object is not selected. Otherwise, they only appear when selected."), ShowGizmosAlways);
                    EditorGUILayout.Space();
                    EditorGUILayout.HelpBox("Hold Ctrl while dragging handles for snapping.", MessageType.Info);
                },
                keywords = new[] { "AV Direction", "Editor", "Tools", "Handle", "Gizmo", "Snap", "Opacity" }
            };
            return provider;
        }
    }
}