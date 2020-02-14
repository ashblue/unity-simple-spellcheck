using System;
using UnityEngine;

namespace CleverCrow.Fluid.SimpleSpellcheck {
    [AttributeUsage(AttributeTargets.Field)]
    public class TextAreaSpellCheckAttribute : PropertyAttribute {
        public int Lines { get; }

        public TextAreaSpellCheckAttribute () {
            Lines = 3;
        }

        public TextAreaSpellCheckAttribute (int lines) {
            Lines = lines;
        }
    }
}
