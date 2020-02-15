using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.SimpleSpellcheck {
    public class SpellCheck {
        private static SpellCheck _instance;
        private readonly SpellCheckInternal _internal;

        public static SpellCheck Instance => _instance ?? (_instance = new SpellCheck());

        private SpellCheck () {
            TextAsset wordsTxt;
            try {
                wordsTxt = GetWordsTxt();
            } catch (Exception e) {
                Debug.LogError($"Could not find the dictionary text file. Aborting spell check setup {e}");
                return;
            }

            var wordList = wordsTxt.text
                .ToLower()
                .Split(new []{"\r\n", "\n"}, StringSplitOptions.None);
            wordList = wordList.Concat(SpellCheckSettings.Instance.ExtraWords.ToArray()).ToArray();

            var dictionary = new EnglishDictionary(new HashSet<string>(wordList));
            _internal = new SpellCheckInternal(dictionary);
        }

        private static TextAsset GetWordsTxt () {
            var guids = AssetDatabase.FindAssets("FluidWordsAlpha");
            var wordsPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<TextAsset>(wordsPath);
        }

        public void Validate (string text) {
            SpellCheckResults.ShowWindow(_internal.Validate(text));
        }

        public static void Clear () {
            _instance = null;
        }

        public bool IsInvalid (string text) {
            return _internal.IsInvalid(text);
        }

        public void ClearValidation () {
            SpellCheckResults.GetWindow().ClearText();
        }

        public void AddValidation (string title, string text) {
            SpellCheckResults.GetWindow().ShowText(title, _internal.Validate(text));
        }

        public void ShowLogs (string title, List<LogEntry> logs) {
            SpellCheckLogs.ShowWindow(title, logs);
        }
    }

    public class SpellCheckInternal {
        private readonly IEnglishDictionary _dic;

        public SpellCheckInternal (IEnglishDictionary dic) {
            _dic = dic;
        }

        public IWordSpelling[] Validate (string text) {
            return GetWords(text)
                .Select(i => new WordSpelling(i, _dic.HasWord(i)))
                .ToArray<IWordSpelling>();
        }

        public bool IsInvalid (string text) {
            return GetWords(text)
                .ToList()
                .Find(i => _dic.HasWord(i) == false) != null;
        }

        private string[] GetWords (string text) {
            var cleanText = text
                .Replace("\n", " ")
                .Replace("\r", " ");

            return Regex
                .Replace(cleanText, "[ ]{2,}", " ")
                .Split(' ');
        }
    }
}
