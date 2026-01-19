using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;
using AV.Direction.Editor.State;
using AV.Direction.Editor.Infrastructure;
using AV.Direction.Editor.Core;
using AV.Direction.Editor.Drawing;
using AV.Direction.Editor.Settings;

namespace AV.Direction.Editor.Tools
{
    // Important: Target GameObject to catch selection properly
    [HelpURL("https://github.com/IAFahim/AV.Direction")]
    [AddComponentMenu("AV/Direction/DirectionEditorTool")]
    [EditorTool("Direction Visualizer", typeof(GameObject))]
    public class DirectionEditorTool : EditorTool
    {
        // Cache handles to avoid reflecting every GUI frame
        private List<DirectionHandleData> m_Handles = new List<DirectionHandleData>();
        private GameObject m_CachedTarget;

        // Robust Icon loading
        private GUIContent m_IconContent;
        public override GUIContent toolbarIcon
        {
            get
            {
                if (m_IconContent == null)
                {
                    var tex = EditorGUIUtility.IconContent("RotateTool").image;
                    m_IconContent = new GUIContent(tex, "Direction Visualizer");
                }
                return m_IconContent;
            }
        }

        public override void OnActivated()
        {
            UpdateCache();
            // Refresh cache on undo/redo to keep handles in sync
            Undo.undoRedoPerformed += UpdateCache;
        }

        public override void OnWillBeDeactivated()
        {
            Undo.undoRedoPerformed -= UpdateCache;
            m_Handles.Clear();
        }

        private void UpdateCache()
        {
            // If selection changes, EditorTool recreates/reactivates,
            // but we double check target matches.
            if (target is GameObject targetGameObject)
            {
                m_CachedTarget = targetGameObject;
                m_Handles = new List<DirectionHandleData>(DirectionReflector.GetHandles(targetGameObject));
            }
        }

        public override void OnToolGUI(EditorWindow window)
        {
            // 1. Validation
            if (!(target is GameObject targetGameObject)) return;

            // If target changed unexpectedly or is dirty, refresh
            if (targetGameObject != m_CachedTarget) UpdateCache();

            if (m_Handles.Count == 0) return;

            // 2. Iteration
            foreach (var handle in m_Handles)
            {
                if (!handle.IsValid) continue;

                var transform = ((MonoBehaviour)handle.Component).transform;
                var so = new SerializedObject(handle.Component);
                var prop = so.FindProperty(handle.PropertyPath);

                EditorGUI.BeginChangeCheck();

                Vector3 newHandlePos = handle.WorldPosition;
                float newValue = handle.CurrentValue;

                // 3. Drawing & Input (Polymorphic based on type)
                Handles.color = DirectionColors.HandleActive;

                switch (handle.Type)
                {
                    case HandleType.Angle:
                        // Draw visual aid
                        DirectionGizmos.DrawAngleVisuals(transform, handle, DirectionPreferences.Opacity);

                        // Interaction
                        Quaternion rot = UnityEditor.Tools.pivotRotation == PivotRotation.Local ? handle.WorldRotation : Quaternion.identity;
                        // Calculate position at the tip of the visual length
                        Vector3 tipPos = transform.position + (handle.WorldRotation * Vector3.forward * handle.VisualLength);

                        Vector3 resultPos = Handles.Slider2D(
                            tipPos,
                            transform.up,
                            transform.right,
                            transform.forward,
                            HandleUtility.GetHandleSize(tipPos) * 0.15f * DirectionPreferences.HandleSize,
                            Handles.SphereHandleCap,
                            0
                        );

                        if (EditorGUI.EndChangeCheck())
                        {
                            newValue = DirectionToolLogic.CalculateNewAngle(resultPos, transform);
                            if (Event.current.control) newValue = DirectionToolLogic.SnapValue(newValue, 15f); // Snap 15 deg
                        }
                        break;

                    case HandleType.Line:
                    case HandleType.Radius:
                        // Draw visual aid
                        DirectionGizmos.DrawLineVisuals(transform, handle, DirectionPreferences.Opacity);

                        // Interaction (Slider along axis)
                        Vector3 axis = handle.Type == HandleType.Line
                            ? transform.TransformDirection(handle.LocalAxis)
                            : transform.forward; // Radius usually forward for visual

                        newHandlePos = Handles.Slider(
                            handle.WorldPosition,
                            axis,
                            HandleUtility.GetHandleSize(handle.WorldPosition) * 0.15f * DirectionPreferences.HandleSize,
                            Handles.SphereHandleCap,
                            0
                        );

                        if (EditorGUI.EndChangeCheck())
                        {
                            newValue = DirectionToolLogic.CalculateNewDistance(newHandlePos, transform, handle.Type == HandleType.Line ? handle.LocalAxis : Vector3.forward);
                            if (Event.current.control) newValue = DirectionToolLogic.SnapValue(newValue, 0.5f); // Snap 0.5m
                        }
                        break;
                }

                // 4. Apply Changes
                if (GUI.changed)
                {
                    Undo.RecordObject(handle.Component, $"Change {handle.PropertyName}");

                    if (prop.propertyType == SerializedPropertyType.Float)
                        prop.floatValue = newValue;
                    else
                        prop.intValue = Mathf.RoundToInt(newValue);

                    so.ApplyModifiedProperties();

                    // Update cached data immediately for smooth dragging
                    handle.CurrentValue = newValue;
                    if(handle.Type == HandleType.Angle)
                    {
                         handle.WorldRotation = transform.rotation * Quaternion.AngleAxis(newValue, Vector3.up);
                    }
                    else
                    {
                        Vector3 moveAxis = handle.Type == HandleType.Line ? transform.TransformDirection(handle.LocalAxis) : transform.forward;
                        handle.WorldPosition = transform.position + (moveAxis * newValue);
                    }
                }
            }
        }
    }
}
