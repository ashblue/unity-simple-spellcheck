using System;

namespace CleverCrow.Fluid.SimpleSpellcheck {
    public class LogEntry {
        public string Preview { get; }
        public Action ViewCallback { get; }

        public LogEntry (string preview, Action viewCallback) {
            Preview = preview;
            ViewCallback = viewCallback;
        }
    }
}
