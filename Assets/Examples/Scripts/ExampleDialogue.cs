using UnityEngine;

namespace CleverCrow.Fluid.SimpleSpellcheck.Examples {
    [CreateAssetMenu(menuName = "Fluid/Spell Check/Example Dialogue", fileName = "ExampleDialogue")]
    public class ExampleDialogue : ScriptableObject {
        [SerializeField]
        private string _title;

        [SerializeField]
        [TextAreaSpellCheck]
        private string _text;

        public string Title => _title;
        public string Text => _text;
    }
}
