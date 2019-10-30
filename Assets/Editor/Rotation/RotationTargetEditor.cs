using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(RotationTarget))]
public class RotationTargetEditor : PropertyDrawer
{
    Dictionary<string, bool> showComponent = new Dictionary<string, bool>();

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty targetX = property.FindPropertyRelative("targetX"),
            targetY = property.FindPropertyRelative("targetY"),
            targetZ = property.FindPropertyRelative("targetZ");

        // Get visibility for specific property.
        bool show = false;
        showComponent.TryGetValue(property.propertyPath, out show);

        // Calculate adjusted height.
        float height = 20;
        if (show)
        {
            height += 60 +
                (targetX.boolValue ? 60 : 0) +
                (targetY.boolValue ? 60 : 0) +
                (targetZ.boolValue ? 60 : 0);
        }

        return height;
    }

    /*
        UI Code is never pretty...
        still could have been broken down into multiple functions.
    */
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(rect, label, property);

        // Get visibility for specific property.
        bool show = false;
        showComponent.TryGetValue(property.propertyPath, out show);

        show = EditorGUI.Foldout(rect, show, label.text + ": Target Rotation");

        // Update visibility for property.
        showComponent[property.propertyPath] = show;

        rect.x += 10;
        rect.width -= 10;
        rect.y += 20;

        if (show)
        {
            SerializedProperty targetX = property.FindPropertyRelative("targetX"),
                        targetY = property.FindPropertyRelative("targetY"),
                        targetZ = property.FindPropertyRelative("targetZ");

            Rect xTargetRect = new Rect(rect.x, rect.y, rect.width, 20);
            targetX.boolValue = EditorGUI.Toggle(xTargetRect, "X: ", targetX.boolValue);
            if (targetX.boolValue)
            {
                xTargetRect.x += 10;
                xTargetRect.width -= 10;

                SerializedProperty minProperty = property.FindPropertyRelative("xMin"),
                    maxProperty = property.FindPropertyRelative("xMax");

                float minValue = minProperty.floatValue,
                    maxValue = maxProperty.floatValue;

                Rect minLabelRect = new Rect(xTargetRect.x, xTargetRect.y + 20, xTargetRect.width, 20),
                    maxLabelRect = new Rect(minLabelRect.x, minLabelRect.yMax, minLabelRect.width, 20),
                    sliderRect = new Rect(maxLabelRect.x, maxLabelRect.yMax, maxLabelRect.width, 20);

                xTargetRect.height += minLabelRect.height + maxLabelRect.height + sliderRect.height;

                EditorGUI.LabelField(minLabelRect, "Min: " + minProperty.floatValue.ToString());
                EditorGUI.LabelField(maxLabelRect, "Max: " + maxProperty.floatValue.ToString());
                EditorGUI.MinMaxSlider(sliderRect, ref minValue, ref maxValue, 0, 360);

                minProperty.floatValue = minValue;
                maxProperty.floatValue = maxValue;
            }

            Rect yTargetRect = new Rect(rect.x, xTargetRect.yMax, rect.width, 20);
            targetY.boolValue = EditorGUI.Toggle(yTargetRect, "Y: ", targetY.boolValue);
            if (targetY.boolValue)
            {
                yTargetRect.x += 10;
                yTargetRect.width -= 10;

                SerializedProperty minProperty = property.FindPropertyRelative("yMin"),
                    maxProperty = property.FindPropertyRelative("yMax");

                float minValue = minProperty.floatValue,
                    maxValue = maxProperty.floatValue;

                Rect minLabelRect = new Rect(yTargetRect.x, yTargetRect.y + 20, yTargetRect.width, 20),
                    maxLabelRect = new Rect(minLabelRect.x, minLabelRect.yMax, minLabelRect.width, 20),
                    sliderRect = new Rect(maxLabelRect.x, maxLabelRect.yMax, maxLabelRect.width, 20);

                yTargetRect.height += minLabelRect.height + maxLabelRect.height + sliderRect.height;

                EditorGUI.LabelField(minLabelRect, "Min: " + minProperty.floatValue.ToString());
                EditorGUI.LabelField(maxLabelRect, "Max: " + maxProperty.floatValue.ToString());
                EditorGUI.MinMaxSlider(sliderRect, ref minValue, ref maxValue, 0, 360);

                minProperty.floatValue = minValue;
                maxProperty.floatValue = maxValue;
            }

            Rect zTargetRect = new Rect(rect.x, yTargetRect.yMax, rect.width, 20);
            targetZ.boolValue = EditorGUI.Toggle(zTargetRect, "Z: ", targetZ.boolValue);
            if (targetZ.boolValue)
            {
                zTargetRect.x += 10;
                zTargetRect.width -= 10;

                SerializedProperty minProperty = property.FindPropertyRelative("zMin"),
                    maxProperty = property.FindPropertyRelative("zMax");

                float minValue = minProperty.floatValue,
                    maxValue = maxProperty.floatValue;

                Rect minLabelRect = new Rect(zTargetRect.x, zTargetRect.y + 20, zTargetRect.width, 20),
                    maxLabelRect = new Rect(minLabelRect.x, minLabelRect.yMax, minLabelRect.width, 20),
                    sliderRect = new Rect(maxLabelRect.x, maxLabelRect.yMax, maxLabelRect.width, 20);

                zTargetRect.height += minLabelRect.height + maxLabelRect.height + sliderRect.height;

                EditorGUI.LabelField(minLabelRect, "Min: " + minProperty.floatValue.ToString());
                EditorGUI.LabelField(maxLabelRect, "Max: " + maxProperty.floatValue.ToString());
                EditorGUI.MinMaxSlider(sliderRect, ref minValue, ref maxValue, 0, 360);

                minProperty.floatValue = minValue;
                maxProperty.floatValue = maxValue;
            }
        }

        EditorGUI.EndProperty();
    }
}
