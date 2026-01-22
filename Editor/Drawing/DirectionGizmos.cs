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
            if (UnityEditor.EditorTools.ToolManager.activeToolType == typeof(Tools.DirectionEditorTool))
                return;

            if (!component.enabled) return;

            bool isSelected = (gizmoType & GizmoType.Selected) != 0;
            float opacity = isSelected ? 1f : (Settings.DirectionPreferences.ShowGizmosAlways ? 0.5f : 0f);
            opacity *= Settings.DirectionPreferences.Opacity;

            if (opacity <= 0) return;

            var handles = DirectionReflector.GetHandles(component.gameObject);

            foreach (var handle in handles)
            {
                if(handle.Component != component) continue;

                if (handle.Type == DirectionHandleType.Angle)
                    DrawAngleVisuals(component.transform, handle, opacity);
                else
                    DrawLineVisuals(component.transform, handle, opacity);
            }
        }

        public static void DrawAngleVisuals(Transform t, DirectionState data, float opacity)
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

            Vector3 labelPos = t.position + direction * data.VisualLength;
            DirectionHandles.DrawShadowedLabel(labelPos, $"{data.CurrentValue:F1}Â°");
        }

        public static void DrawLineVisuals(Transform t, DirectionState data, float opacity)
        {
            var color = DirectionColors.ApplyGlobalOpacity(data.Color, opacity);

            Vector3 axis = data.Type == DirectionHandleType.Line ? t.TransformDirection(data.LocalAxis) : t.forward;
            Vector3 endPos = t.position + (axis * data.CurrentValue);

            Handles.color = DirectionColors.ApplyGlobalOpacity(DirectionColors.InfinityLine, opacity * 0.3f);
            Handles.DrawDottedLine(t.position - axis * 10f, t.position + axis * 10f, 2f);

            Handles.color = color;
            Handles.DrawDottedLine(t.position, endPos, 4f);

            DirectionHandles.DrawShadowedLabel(endPos, $"{data.CurrentValue:F1}m");
        }
    }
}
