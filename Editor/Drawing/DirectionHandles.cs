using UnityEngine;
using UnityEditor;
using AV.Direction.Editor.Core;

namespace AV.Direction.Editor.Drawing
{
    public static class DirectionHandles
    {
        private static GUIStyle _labelStyle;
        private static GUIStyle LabelStyle
        {
            get
            {
                if (_labelStyle == null)
                {
                    _labelStyle = new GUIStyle(EditorStyles.boldLabel)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        normal =
                        {
                            textColor = Color.white
                        },
                        fontSize = 12,
                        richText = true
                    };
                }
                return _labelStyle;
            }
        }

        public static void DrawShadowedLabel(Vector3 position, string text)
        {
            // SAFETY: Check if point is behind the camera
            if (SceneView.currentDrawingSceneView?.camera != null)
            {
                Vector3 camPos = SceneView.currentDrawingSceneView.camera.transform.position;
                Vector3 camFwd = SceneView.currentDrawingSceneView.camera.transform.forward;
                Vector3 dirToPoint = position - camPos;

                // If dot product is negative, point is behind camera. Abort.
                if (Vector3.Dot(camFwd, dirToPoint) <= 0) return;
            }

            Handles.BeginGUI();
            // Project world point to screen
            Vector2 screenPoint = HandleUtility.WorldToGUIPoint(position);

            var content = new GUIContent(text);
            var size = LabelStyle.CalcSize(content);
            var rect = new Rect(screenPoint.x - size.x / 2, screenPoint.y - size.y / 2, size.x, size.y);

            // Draw Shadow
            var shadowRect = rect;
            shadowRect.position += new Vector2(1f, 1f);
            GUI.color = DirectionColors.TextShadow;
            GUI.Label(shadowRect, content, LabelStyle);

            // Draw Text
            GUI.color = Color.white;
            GUI.Label(rect, content, LabelStyle);

            Handles.EndGUI();
        }

        public static void DrawPieSlice(Vector3 center, Vector3 up, Vector3 forward, float angle, float radius, Color color)
        {
            Color fill = DirectionColors.GetFillColor(color);
            Color outline = DirectionColors.GetOutlineColor(color);

            Handles.color = fill;
            Handles.DrawSolidArc(center, up, Quaternion.AngleAxis(-angle / 2f, up) * forward, angle, radius);

            Handles.color = outline;
            Handles.DrawWireArc(center, up, Quaternion.AngleAxis(-angle / 2f, up) * forward, angle, radius);
            
            // Draw side lines
            Vector3 leftDir = Quaternion.AngleAxis(-angle / 2f, up) * forward;
            Vector3 rightDir = Quaternion.AngleAxis(angle / 2f, up) * forward;
            Handles.DrawLine(center, center + leftDir * radius);
            Handles.DrawLine(center, center + rightDir * radius);
        }

        public static void DrawArrow(Vector3 center, Vector3 direction, float length, Color color)
        {
            Handles.color = color;
            Vector3 endPoint = center + direction * length;
            Handles.DrawLine(center, endPoint);
            
            // Draw arrow head
            float arrowSize = length * 0.1f;
            Handles.ConeHandleCap(0, endPoint, Quaternion.LookRotation(direction), arrowSize, EventType.Repaint);
        }

        public static void DrawRangeCircle(Vector3 center, Vector3 up, float radius, Color color)
        {
            Handles.color = DirectionColors.GetOutlineColor(color);
            Handles.DrawWireDisc(center, up, radius);
            
            Handles.color = DirectionColors.GetFillColor(color);
            Handles.DrawSolidDisc(center, up, radius);
        }
    }
}