using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CleverCrow.Fluid.SimpleSpellcheck {
    public interface IEnglishDictionary {
        bool HasWord (string word);
    }

    public class EnglishDictionary : IEnglishDictionary {
        private readonly HashSet<string> _words;

        public EnglishDictionary (HashSet<string> words) {
            _words = words;
        }

        public bool HasWord (string word) {
            if (word.Contains("-")) {
                var words = word.Split('-');
                var badWord = words.ToList().Find(w => !_words.Contains(CleanedWord(w)));
                return badWord == null;
            }

            return _words.Contains(CleanedWord(word));
        }

        private string CleanedWord (string word) {
            var wordFilter = word.Replace("'s", "");
            wordFilter = Regex.Replace(wordFilter, "[^a-zA-Z']", string.Empty);

            return wordFilter.ToLower();
        }
    }
}
