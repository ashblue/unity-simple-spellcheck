using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CleverCrow.Fluid.SimpleSpellcheck {
    public class SpellCheckResults : EditorWindow {
        private VisualElement _text;

        public static void ShowWindow (IWordSpelling[] results) {
            var window = GetWindow<SpellCheckResults>();
            window.titleContent = new GUIContent("Spell Check");
            window.ShowText(results);
        }

        private void OnEnable () {
            var root = rootVisualElement;
            root.styleSheets.Add(Resources.Load<StyleSheet>("SpellCheckResults"));

            var tree = Resources.Load<VisualTreeAsset>("SpellCheckResults");
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
            AssetDatabase.CreateAsset(settings, "Assets/Editor/Resources/SpellCheckSettings.asset");

            Selection.activeObject = SpellCheckSettings.Instance;
        }

        private void ShowText (IWordSpelling[] results) {
            var root = rootVisualElement.Query("text").First();

            foreach (var child in root.Children().ToArray()) {
                root.Remove(child);
            }

            foreach (var word in results) {
                var text = new TextElement {text = $"{word.Text} "};

                if (!word.IsValid) {
                    text.AddToClassList("bad-spelling");
                }

                root.Add(text);
            }
        }
    }
}
