using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.SimpleSpellcheck {
    public class SpellCheck {
        private static SpellCheck _instance;
        private SpellCheckInternal _internal;

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
            var dictionary = new EnglishDictionary(new HashSet<string>(wordList));
            _internal = new SpellCheckInternal(dictionary);
        }

        private static TextAsset GetWordsTxt () {
            var guids = AssetDatabase.FindAssets("FluidWordsAlpha");
            var wordsPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<TextAsset>(wordsPath);
        }

        public void Validate (string text) {
            Debug.Log(text);
            SpellCheckResults.ShowWindow(_internal.Validate(text));
        }

        [MenuItem("Spelling/Test")]
        public static void TestSpellCheck () {
            Instance.Validate("This is a block of tidddxx with some spalkling arrars. This isn't how your mom's dog normally wants you to spell.");
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
