using System.Collections.Generic;
using UnityEditor;

namespace CleverCrow.Fluid.SimpleSpellcheck.Examples {
    public class SpellCheckAllDialogue {
        [MenuItem("Spell Check/All Dialogue")]
        public static void CheckAllDialogue () {
            var logList = new List<LogEntry>();

            var guids = AssetDatabase.FindAssets($"t:{typeof(ExampleDialogue).Name}");
            foreach (var guid in guids) {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<ExampleDialogue>(path);

                if (!SpellCheck.Instance.IsInvalid(asset.Title) && !SpellCheck.Instance.IsInvalid(asset.Text)) continue;

                var log = new LogEntry($"{asset.Title} {asset.Text}", () => {
                    SpellCheck.Instance.ClearValidation();
                    SpellCheck.Instance.AddValidation("Title", asset.Title);
                    SpellCheck.Instance.AddValidation("Text", asset.Text);
                    Selection.activeObject = asset;
                });

                logList.Add(log);
            }

            SpellCheck.Instance.ShowLogs("All Example Dialogue Errors", logList);
        }
    }
}
