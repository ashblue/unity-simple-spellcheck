using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CleverCrow.Fluid.SimpleSpellcheck {
    public class SpellCheckResults : EditorWindow {
        private VisualElement _text;

        public static SpellCheckResults GetWindow () {
            var window = GetWindow<SpellCheckResults>();
            window.titleContent = new GUIContent("Spell Check");

            return window;
        }

        public static void ShowWindow (IWordSpelling[] results) {
            var window = GetWindow();
            window.titleContent = new GUIContent("Spell Check");

            window.ClearText();
            window.ShowText(results);
        }

        private void OnEnable () {
            var root = rootVisualElement;
            root.styleSheets.Add(Resources.Load<StyleSheet>("SpellCheck/Results"));

            var tree = Resources.Load<VisualTreeAsset>("SpellCheck/Results");
            tree.CloneTree(root);

            var button = root.Query<Button>("open-settings").First();
            button.clickable.clicked += OpenSettings;
        }

        private void OpenSettings () {
            if (SpellCheckSettings.DoesExist()) {
                Selection.activeObject = SpellCheckSettings.Instance;
                return;
            }

            if (AssetDatabase.LoadAssetAtPath<Object>("Assets/Editor") == null) {
                AssetDatabase.CreateFolder("Assets", "Editor");
            }

            if (AssetDatabase.LoadAssetAtPath<Object>("Assets/Editor/Resources") == null) {
                AssetDatabase.CreateFolder("Assets/Editor", "Resources");
            }

            var settings = CreateInstance<SpellCheckSettings>();
            AssetDatabase.CreateAsset(settings, "Assets/Editor/Resources/SpellCheck/SpellCheckSettings.asset");

            Selection.activeObject = SpellCheckSettings.Instance;
        }

        private void ShowText (IWordSpelling[] results) {
            var root = rootVisualElement.Query("text").First();

            var textBlock = new VisualElement();
            textBlock.AddToClassList("text-block");
            root.Add(textBlock);

            foreach (var word in results) {
                var text = new TextElement {text = $"{word.Text} "};

                if (!word.IsValid) {
                    text.AddToClassList("bad-spelling");
                }

                textBlock.Add(text);
            }

            root.Add(textBlock);
        }

        public void ClearText () {
            var root = rootVisualElement.Query("text").First();

            foreach (var child in root.Children().ToArray()) {
                root.Remove(child);
            }
        }

        public void ShowText (string title, IWordSpelling[] text) {
            var root = rootVisualElement.Query("text").First();

            var elTitle = new TextElement { text = title };
            elTitle.AddToClassList("text-title");
            root.Add(elTitle);

            ShowText(text);
        }
    }
}
