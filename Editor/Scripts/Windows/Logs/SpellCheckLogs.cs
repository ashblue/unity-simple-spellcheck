using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CleverCrow.Fluid.SimpleSpellcheck {
    public class SpellCheckLogs : EditorWindow {
        public static void ShowWindow (string title, List<LogEntry> logs) {
            var window = GetWindow<SpellCheckLogs>();
            window.titleContent = new GUIContent("Spell Check Logs");
            window.Show(title, logs);
        }

        private void OnEnable () {
            var root = rootVisualElement;
            root.styleSheets.Add(Resources.Load<StyleSheet>("SpellCheck/Logs"));

            var tree = Resources.Load<VisualTreeAsset>("SpellCheck/Logs");
            tree.CloneTree(root);
        }

        private void Show (string titleText, List<LogEntry> logs) {
            ClearLogs();
            SetTitle(titleText);

            var root = rootVisualElement;

            if (logs.Count == 0) {
                var noResults = root.Query<VisualElement>("no-results").First();
                noResults.style.display = DisplayStyle.Flex;
            }

            var logContainer = root.Query<VisualElement>("log-container").First();
            var logTemplate = Resources.Load<VisualTreeAsset>("SpellCheck/LogEntry");

            foreach (var logEntry in logs) {
                logTemplate.CloneTree(logContainer);
                var elLog = logContainer.Children().Last();

                var text = elLog.Query<TextElement>(null, "log-entry__text").First();
                text.text = logEntry.Preview;

                var hideBtn = elLog.Query<Button>(null, "log-entry__button-hide").First();
                hideBtn.clickable.clicked += () => logContainer.Remove(elLog);

                var viewBtn = elLog.Query<Button>(null, "log-entry__button-view").First();
                viewBtn.clickable.clicked += logEntry.ViewCallback;
            }
        }

        private void SetTitle (string titleText) {
            var root = rootVisualElement;
            var elTitle = root.Query<TextElement>("title").First();
            elTitle.text = titleText;
        }


        private void ClearLogs () {
            rootVisualElement.Query<VisualElement>("no-results").First().style.display = DisplayStyle.None;
            var logContainer = rootVisualElement
                .Query<VisualElement>("log-container")
                .First();

            foreach (var child in logContainer.Children().ToArray()) {
                logContainer.Remove(child);
            }
        }
    }
}
