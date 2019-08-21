using System;

namespace DreamRecorder.FoggyConsole
{
    public class ConsoleSizeChangedEvnetArgs : EventArgs
    {
        public Size NewSize { get; set; }

        public Size OldSize { get; set; }
    }
}