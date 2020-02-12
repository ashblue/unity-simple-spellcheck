using System.Linq;

namespace CleverCrow.Fluid.SimpleSpellcheck {
    public class SpellCheck {
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
