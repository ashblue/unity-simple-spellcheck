using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.SimpleSpellcheck {
    [CustomEditor(typeof(SpellCheckSettings))]
    public class SpellCheckSettingsInspector : Editor {
        private SerializedProperty _extraWords;
        private string _newWord;
        private int _deleteIndex = -1;

        private void OnEnable () {
            _extraWords = serializedObject.FindProperty("_extraWords");
        }

        public override void OnInspectorGUI () {
            serializedObject.Update();

            AddNewWord();
            PrintExtraWords();

            if (_deleteIndex != -1) {
                _extraWords.DeleteArrayElementAtIndex(_deleteIndex);
                _deleteIndex = -1;
                SpellCheck.Clear();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void PrintExtraWords () {
            EditorGUILayout.LabelField("Extra Words", EditorStyles.boldLabel);

            for (var i = 0; i < _extraWords.arraySize; i++) {
                var word = _extraWords.GetArrayElementAtIndex(i).stringValue;
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(word);
                if (GUILayout.Button("Delete")) {
                    _deleteIndex = i;
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private void AddNewWord () {
            _newWord = EditorGUILayout.TextField("New Word", _newWord);

            if (!GUILayout.Button("Add") || _newWord == string.Empty) return;

            if (((SpellCheckSettings)target).ExtraWords.Contains(_newWord)) {
                return;
            }

            _extraWords.InsertArrayElementAtIndex(_extraWords.arraySize == 0 ? 0 : _extraWords.arraySize - 1);
            var element = _extraWords.GetArrayElementAtIndex(_extraWords.arraySize - 1);
            element.stringValue = _newWord;

            _newWord = string.Empty;
            GUI.FocusControl(null);
            SpellCheck.Clear();
        }
    }
}
