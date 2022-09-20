using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.SimpleSpellcheck {
    [CustomEditor(typeof(SpellCheckSettings))]
    public class SpellCheckSettingsInspector : Editor {
        private SerializedProperty _extraWords;
        private string _newWord;
        private int _deleteIndex = -1;
        private int _editIndex = -1;
        private string _newStringValue;

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
                Save();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void PrintExtraWords () {
            EditorGUILayout.LabelField("Extra Words", EditorStyles.boldLabel);

            for (var i = 0; i < _extraWords.arraySize; i++) {
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

                if (_editIndex == i) {
                    EditWord(i);
                } else {
                    PrintWord(i);
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private void EditWord (int i) {
            var word = _extraWords.GetArrayElementAtIndex(i).stringValue;

            if (_newStringValue == null) {
                _newStringValue = word;
            }

            _newStringValue = EditorGUILayout.TextField(_newStringValue);

            if (GUILayout.Button("Save")) {
                _extraWords.GetArrayElementAtIndex(i).stringValue = _newStringValue;
                ClearEditForm();
                Save();
            }
        }

        private void ClearEditForm () {
            _editIndex = -1;
            _newStringValue = null;
        }

        private void PrintWord (int i) {
            var word = _extraWords.GetArrayElementAtIndex(i).stringValue;
            EditorGUILayout.LabelField(word);

            if (GUILayout.Button("Delete")) {
                _deleteIndex = i;
            }

            if (GUILayout.Button("Edit")) {
                ClearEditForm();
                _editIndex = i;
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
            Save();
        }

        private void Save () {
            serializedObject.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();
            SpellCheck.Clear();
        }
    }
}
