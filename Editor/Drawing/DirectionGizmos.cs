using UnityEngine;
using UnityEditor;
using AV.Direction.Editor.Core;
using AV.Direction.Editor.State;
using AV.Direction.Editor.Infrastructure;
using AV.Direction.Runtime.Enums;

namespace AV.Direction.Editor.Drawing
{
    public static class DirectionGizmos
    {
        // Passive Gizmo Drawing (When tool is NOT active)
        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        private static void DrawPassiveGizmos(MonoBehaviour component, GizmoType gizmoType)
        {
            // Don't draw if the custom tool is active to avoid Z-fighting/double drawing
            if (UnityEditor.EditorTools.ToolManager.activeToolType == typeof(Tools.DirectionEditorTool))
                return;

            if (!component.enabled) return;

            bool isSelected = (gizmoType & GizmoType.Selected) != 0;
            float opacity = isSelected ? 1f : (Settings.DirectionPreferences.ShowGizmosAlways ? 0.5f : 0f);
            opacity *= Settings.DirectionPreferences.Opacity;

            if (opacity <= 0) return;

            // We iterate strictly for drawing here.
            // In a pure system, we might cache this too, but for Gizmos,
            // recalculating small lists per object is acceptable if Reflector is fast.
            // Note: We create a temp list here. In high-performance scenarios, use a shared static list.
            var handles = DirectionReflector.GetHandles(component.gameObject);

            foreach (var handle in handles)
            {
                if(handle.Component != component) continue; // Filter to just this component's handles

                if (handle.Type == HandleType.Angle)
                    DrawAngleVisuals(component.transform, handle, opacity);
                else
                    DrawLineVisuals(component.transform, handle, opacity);
            }
        }

        public static void DrawAngleVisuals(Transform t, DirectionHandleData data, float opacity)
        {
            var color = DirectionColors.ApplyGlobalOpacity(data.Color, opacity);
            var direction = data.WorldRotation * Vector3.forward;

            switch (data.Style)
            {
                case DirectionStyle.Arrow:
                    DirectionHandles.DrawArrow(t.position, direction, data.VisualLength, color);
                    break;
                case DirectionStyle.PieSlice:
                case DirectionStyle.Cone:
                    DirectionHandles.DrawPieSlice(t.position, t.up, direction, data.ArcAngle, data.VisualLength, color);
                    break;
                case DirectionStyle.TargetLine:
                    Handles.color = color;
                    Handles.DrawDottedLine(t.position, t.position + direction * data.VisualLength, 4f);
                    break;
            }

            // Label
            Vector3 labelPos = t.position + direction * data.VisualLength;
            DirectionHandles.DrawShadowedLabel(labelPos, $"{data.CurrentValue:F1}°");
        }

        public static void DrawLineVisuals(Transform t, DirectionHandleData data, float opacity)
        {
            var color = DirectionColors.ApplyGlobalOpacity(data.Color, opacity);

            // Guide Line
            Vector3 axis = data.Type == HandleType.Line ? t.TransformDirection(data.LocalAxis) : t.forward;
            Vector3 endPos = t.position + (axis * data.CurrentValue);

            Handles.color = DirectionColors.ApplyGlobalOpacity(DirectionColors.InfinityLine, opacity * 0.3f);
            Handles.DrawDottedLine(t.position - axis * 10f, t.position + axis * 10f, 2f);

            // Actual Value Line
            Handles.color = color;
            Handles.DrawDottedLine(t.position, endPos, 4f);

            // Label
            DirectionHandles.DrawShadowedLabel(endPos, $"{data.CurrentValue:F1}m");
        }
    }
}
