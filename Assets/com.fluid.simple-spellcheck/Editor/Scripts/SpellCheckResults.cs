using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CleverCrow.Fluid.SimpleSpellcheck {
    public class SpellCheckResults : EditorWindow {
        public static void ShowWindow (IWordSpelling[] results) {
            var window = GetWindow<SpellCheckResults>();
            window.titleContent = new GUIContent("Spell Check");
            window.ShowText(results);
        }

        public void ShowText (IWordSpelling[] results) {
            var root = rootVisualElement;
            root.style.paddingLeft = root.style.paddingTop = root.style.paddingRight = root.style.paddingBottom = 10;
            root.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);

            foreach (var child in root.Children()) {
                root.Remove(child);
            }

            foreach (var word in results) {
                var text = new TextElement();
                text.text = $"{word.Text} ";

                if (!word.IsValid) {
                    text.style.color = new StyleColor(Color.red);
                    text.style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);
                }

                root.Add(text);
            }
        }
    }
}
