using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anomaly.Utils
{
    [System.Serializable]
    public class FlexibleValue
    {
        [SerializeField] private float defaultValue = default(float);

        [SerializeField] private float minValue = Mathf.NegativeInfinity, maxValue = Mathf.Infinity;
        [SerializeField] private bool hasRange = false;


        public float Addition { get; set; } = 0F;
        public float Multiplier { get; set; } = 1F;
        public float FinalAddition { get; set; } = 0F;
        public float FinalMultiplier { get; set; } = 1F;

        public float Value
        {
            get
            {
                float value = (defaultValue * Multiplier + Addition) * FinalMultiplier + FinalAddition;
                return hasRange ? Mathf.Clamp(value, minValue, maxValue) : value;
            }
        }
    }
}



#if UNITY_EDITOR
namespace Anomaly.Utils
{
    using UnityEditor;

    [CustomPropertyDrawer(typeof(FlexibleValue))]
    public class FlexibleValueDrawer : PropertyDrawer
    {
        private SerializedProperty defaultValue, hasRange, minValue, maxValue;

        private bool rangeFoldout = false;

        private void Initialize(SerializedProperty property)
        {
            defaultValue = property.FindPropertyRelative(nameof(defaultValue));
            hasRange = property.FindPropertyRelative(nameof(hasRange));
            minValue = property.FindPropertyRelative(nameof(minValue));
            maxValue = property.FindPropertyRelative(nameof(maxValue));

            rangeFoldout = hasRange.boolValue;
        }

        private void Display(Rect position, SerializedProperty property, GUIContent label)
        {
            var rect = position;

            rect.size = new Vector2(position.width * 0.55f, 18F);
            EditorGUI.PropertyField(rect, defaultValue, label);

            rect.x += rect.width + 20F;
            rect.width = position.width * 0.2f;
            EditorGUI.PropertyField(rect, hasRange, new GUIContent("Range?"));
            rangeFoldout = hasRange.boolValue;

            if (!rangeFoldout) return;

            rect.x = position.x + 72F;
            rect.y = position.y + 20F;
            rect.size = new Vector2(position.width * 0.25f, 18F);
            EditorGUI.PropertyField(rect, minValue, GUIContent.none);

            rect.x += rect.width;
            GUI.Label(rect, " ~ ");

            rect.x += 20F;
            EditorGUI.PropertyField(rect, maxValue, GUIContent.none);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Initialize(property);

            Display(EditorGUI.IndentedRect(position), property, label);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return rangeFoldout ? 40F : 18F;
        }
    }
}
#endif