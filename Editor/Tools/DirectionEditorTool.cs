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
    [EditorTool("Direction Visualizer", typeof(GameObject))]
    public class DirectionEditorTool : EditorTool
    {
        private List<DirectionState> m_Handles = new List<DirectionState>();
        private GameObject m_CachedTarget;
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
            Undo.undoRedoPerformed += UpdateCache;
        }

        public override void OnWillBeDeactivated()
        {
            Undo.undoRedoPerformed -= UpdateCache;
            m_Handles.Clear();
        }

        private void UpdateCache()
        {
            if (target is GameObject go)
            {
                m_CachedTarget = go;
                m_Handles = new List<DirectionState>(DirectionReflector.GetHandles(go));
            }
        }

        public override void OnToolGUI(EditorWindow window)
        {
            if (!(target is GameObject go)) return;
            if (go != m_CachedTarget) UpdateCache();
            if (m_Handles.Count == 0) return;

            foreach (var handle in m_Handles)
            {
                if (!handle.IsValid) continue;

                var transform = ((MonoBehaviour)handle.Component).transform;
                var so = new SerializedObject(handle.Component);
                var prop = so.FindProperty(handle.PropertyPath);

                EditorGUI.BeginChangeCheck();

                Vector3 newHandlePos = handle.WorldPosition;
                float newValue = handle.CurrentValue;

                Handles.color = DirectionColors.HandleActive;

                switch (handle.Type)
                {
                    case DirectionHandleType.Angle:
                        DirectionGizmos.DrawAngleVisuals(transform, handle, DirectionPreferences.Opacity);

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
                            newValue = DirectionLogic.CalculateNewAngle(resultPos, transform);
                            if (Event.current.control) newValue = DirectionLogic.SnapValue(newValue, 15f);
                        }
                        break;

                    case DirectionHandleType.Line:
                    case DirectionHandleType.Radius:
                        DirectionGizmos.DrawLineVisuals(transform, handle, DirectionPreferences.Opacity);

                        Vector3 axis = handle.Type == DirectionHandleType.Line
                            ? transform.TransformDirection(handle.LocalAxis)
                            : transform.forward;

                        newHandlePos = Handles.Slider(
                            handle.WorldPosition,
                            axis,
                            HandleUtility.GetHandleSize(handle.WorldPosition) * 0.15f * DirectionPreferences.HandleSize,
                            Handles.SphereHandleCap,
                            0
                        );

                        if (EditorGUI.EndChangeCheck())
                        {
                            Vector3 localAxis = handle.Type == DirectionHandleType.Line ? handle.LocalAxis : Vector3.forward;
                            newValue = DirectionLogic.CalculateNewDistance(newHandlePos, transform, localAxis);
                            if (Event.current.control) newValue = DirectionLogic.SnapValue(newValue, 0.5f);
                        }
                        break;
                }

                if (GUI.changed)
                {
                    Undo.RecordObject(handle.Component, $"Change {handle.PropertyName}");

                    if (prop.propertyType == SerializedPropertyType.Float)
                        prop.floatValue = newValue;
                    else
                        prop.intValue = Mathf.RoundToInt(newValue);

                    so.ApplyModifiedProperties();

                    handle.CurrentValue = newValue;
                    if(handle.Type == DirectionHandleType.Angle)
                    {
                         handle.WorldRotation = transform.rotation * Quaternion.AngleAxis(newValue, Vector3.up);
                    }
                    else
                    {
                        Vector3 moveAxis = handle.Type == DirectionHandleType.Line ? transform.TransformDirection(handle.LocalAxis) : transform.forward;
                        handle.WorldPosition = transform.position + (moveAxis * newValue);
                    }
                }
            }
        }
    }
}
