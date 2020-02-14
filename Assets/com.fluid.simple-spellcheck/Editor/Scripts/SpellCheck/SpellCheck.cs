using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.SimpleSpellcheck {
    public class SpellCheck {
        private static SpellCheck _instance;
        private readonly SpellCheckInternal _internal;

        public static SpellCheck Instance => _instance ?? (_instance = new SpellCheck());

        private SpellCheck () {
            TextAsset wordsTxt = null;
            try {
                wordsTxt = GetWordsTxt();
            } catch (Exception e) {
                Debug.LogError($"Could not find the dictionary text file. Aborting spell check setup {e}");
                return;
            }

            var wordList = wordsTxt.text.Split(new []{"\r\n"}, StringSplitOptions.None);
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
    }

    public class SpellCheckInternal {
        private readonly IEnglishDictionary _dic;

        public SpellCheckInternal (IEnglishDictionary dic) {
            _dic = dic;
        }

        public IWordSpelling[] Validate (string text) {
            return text
                .Split(' ')
                .Select(i => new WordSpelling(i, _dic.HasWord(i)))
                .ToArray<IWordSpelling>();
        }
    }
}
