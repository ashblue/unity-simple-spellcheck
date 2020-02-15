using UnityEngine;

namespace CleverCrow.Fluid.SimpleSpellcheck.Examples {
    [CreateAssetMenu(menuName = "Fluid/Spell Check/Example Dialogue", fileName = "ExampleDialogue")]
    public class ExampleDialogue : ScriptableObject {
        [SerializeField]
        private string _title = null;

        [SerializeField]
        [TextAreaSpellCheck]
        private string _text = null;

        public string Title => _title;
        public string Text => _text;
    }
}
