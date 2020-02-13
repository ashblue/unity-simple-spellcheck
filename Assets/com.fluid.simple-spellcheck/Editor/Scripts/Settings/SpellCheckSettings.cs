using System.Collections.Generic;
using UnityEngine;

namespace CleverCrow.Fluid.SimpleSpellcheck {
    [CreateAssetMenu(fileName = "SpellCheckSettings", menuName = "Fluid/Spell Check/Settings")]
    public class SpellCheckSettings : ScriptableObject {
        private static SpellCheckSettings _instance;

        public static SpellCheckSettings Instance {
            get {
                if (_instance != null) return _instance;

                _instance = Resources.Load<SpellCheckSettings>("SpellCheckSettings");
                if (_instance == null) {
                    return CreateInstance<SpellCheckSettings>();
                }

                return _instance;
            }
        }

        [SerializeField]
        private List<string> _extraWords = new List<string>();

        public List<string> ExtraWords => _extraWords;
    }
}
