using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using AV.Direction.Editor.State;
using AV.Direction.Runtime.Attributes;

namespace AV.Direction.Editor.Infrastructure
{
    public static class DirectionReflector
    {
        private static readonly List<DirectionState> s_Buffer = new List<DirectionState>();

        public static List<DirectionState> GetHandles(GameObject target)
        {
            s_Buffer.Clear();
            if (target == null) return s_Buffer;

            var components = target.GetComponents<MonoBehaviour>();
            foreach (var comp in components)
            {
                if (comp == null) continue;
                ExtractHandlesFromComponent(comp, s_Buffer);
            }
            return s_Buffer;
        }

        private static void ExtractHandlesFromComponent(MonoBehaviour comp, List<DirectionState> list)
        {
            var type = comp.GetType();
            var transform = comp.transform;
            var so = new SerializedObject(comp);

            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                // 1. Direction Attribute
                var dirAttr = field.GetCustomAttribute<DirectionAttribute>();
                if (dirAttr != null)
                {
                    var prop = so.FindProperty(field.Name);
                    if(prop == null) continue;

                    float angle = prop.propertyType == SerializedPropertyType.Float ? prop.floatValue : prop.intValue;
                    Quaternion angleRot = Quaternion.AngleAxis(angle, transform.up);
                    Quaternion worldRot = transform.rotation * angleRot;

                    list.Add(new DirectionState
                    {
                        Component = comp,
                        PropertyPath = field.Name,
                        PropertyName = ObjectNames.NicifyVariableName(field.Name),
                        Type = DirectionHandleType.Angle,
                        Style = dirAttr.Style,
                        Color = dirAttr.UseCustomColor ? dirAttr.CustomColor : Core.DirectionColors.DefaultDirection,
                        VisualLength = dirAttr.Length,
                        ArcAngle = dirAttr.ArcAngle,
                        CurrentValue = angle,
                        WorldPosition = transform.position,
                        WorldRotation = worldRot
                    });
                    continue;
                }

                // 2. LineRange Attribute
                var lineAttr = field.GetCustomAttribute<LineRangeAttribute>();
                if (lineAttr != null)
                {
                    var prop = so.FindProperty(field.Name);
                    if(prop == null) continue;

                    float dist = prop.propertyType == SerializedPropertyType.Float ? prop.floatValue : prop.intValue;
                    Vector3 worldAxis = transform.TransformDirection(lineAttr.Axis);
                    Vector3 handlePos = transform.position + (worldAxis * dist);

                    list.Add(new DirectionState
                    {
                        Component = comp,
                        PropertyPath = field.Name,
                        PropertyName = ObjectNames.NicifyVariableName(field.Name),
                        Type = DirectionHandleType.Line,
                        Color = lineAttr.UseCustomColor ? lineAttr.CustomColor : Core.DirectionColors.DefaultLineRange,
                        LocalAxis = lineAttr.Axis,
                        CurrentValue = dist,
                        WorldPosition = handlePos,
                        WorldRotation = transform.rotation
                    });
                    continue;
                }

                // 3. RangeCircle Attribute
                var rangeAttr = field.GetCustomAttribute<RangeCircleAttribute>();
                if (rangeAttr != null)
                {
                    var prop = so.FindProperty(field.Name);
                    if(prop == null) continue;

                    float radius = prop.propertyType == SerializedPropertyType.Float ? prop.floatValue : prop.intValue;
                    Vector3 handlePos = transform.position + (transform.forward * radius);

                    list.Add(new DirectionState
                    {
                        Component = comp,
                        PropertyPath = field.Name,
                        PropertyName = ObjectNames.NicifyVariableName(field.Name),
                        Type = DirectionHandleType.Radius,
                        Color = rangeAttr.UseCustomColor ? rangeAttr.CustomColor : Core.DirectionColors.DefaultRange,
                        CurrentValue = radius,
                        WorldPosition = handlePos,
                        WorldRotation = transform.rotation
                    });
                }
            }
        }
    }
}
