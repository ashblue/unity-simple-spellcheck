using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.SimpleSpellcheck {
    [CustomPropertyDrawer(typeof(TextAreaSpellCheckAttribute))]
    public class TextAreaSpellCheckDrawer : PropertyDrawer {
        private Vector2 _scroll;

        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
            var options = attribute as TextAreaSpellCheckAttribute;

            PrintLabel(position, property);
            PrintTextArea(position, property, options.Lines);
            PrintSpellCheckButton(position, property, options);
        }

        private static void PrintSpellCheckButton (Rect position, SerializedProperty property,
            TextAreaSpellCheckAttribute options) {
            var btnPosition = position;
            btnPosition.height = EditorGUIUtility.singleLineHeight;
            btnPosition.y += EditorGUIUtility.singleLineHeight * (options.Lines + 1);
            if (GUI.Button(btnPosition, "Spell Check")) {
                SpellCheck.Instance.Validate(property.stringValue);
            }
        }

        private static void PrintTextArea (Rect position, SerializedProperty property, int lines) {
            EditorGUI.BeginChangeCheck();

            var textPosition = position;
            textPosition.y += EditorGUIUtility.singleLineHeight;
            textPosition.height = EditorGUIUtility.singleLineHeight * lines;
            var text = GUI.TextArea(textPosition, property.stringValue, -1, GUI.skin.textArea);

            if (EditorGUI.EndChangeCheck()) {
                property.stringValue = text;
            }
        }

        private static void PrintLabel (Rect position, SerializedProperty property) {
            var labelPosition = position;
            labelPosition.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(labelPosition, property.displayName);
        }

        public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
            var options = attribute as TextAreaSpellCheckAttribute;

            return EditorGUIUtility.singleLineHeight * (options.Lines + 2);
        }
    }
}
