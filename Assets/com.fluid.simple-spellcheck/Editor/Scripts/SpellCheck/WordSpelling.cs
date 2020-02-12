namespace CleverCrow.Fluid.SimpleSpellcheck {
    public interface IWordSpelling {
        string Text { get; }
        bool IsValid { get; }
    }

    public class WordSpelling : IWordSpelling {
        public string Text { get; }
        public bool IsValid { get; }

        public WordSpelling (string text, bool isValid) {
            Text = text;
            IsValid = isValid;
        }
    }
}
