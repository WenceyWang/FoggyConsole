using System;
using DreamRecorder.ToolBox.General;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DreamRecorder.FoggyConsole.Controls
{
    public class RootFrame:ContentControl,IApplicationItem
    {
        private int _redrawPausedLevel = 1;

        public int RedrawPausedLevel
        {
            get => _redrawPausedLevel;
            private set
            {
                _redrawPausedLevel = value;
                Logger.LogTrace($"{nameof(RedrawPausedLevel)} : {value}");
            }
        }

        internal ILogger Logger { get; } =
            StaticServiceProvider.Provider.GetService<ILoggerFactory>().
                CreateLogger<Frame>();



        public void PauseRedraw() { RedrawPausedLevel++; }

        public void ResumeRedraw()
        {
            RedrawPausedLevel = Math.Max(RedrawPausedLevel - 1, 0);

            if (RedrawPausedLevel == 0)
            {
                RequestUpdateDisplay();
            }
        }

        public override bool CanFocusedOn => false;

        protected override void RequestMeasure()
        {
            if (RedrawPausedLevel == 0 && Enabled)
            {
                RedrawPausedLevel++;

                Measure(Size);
                Arrange(new Rectangle(Size));
                Draw();

                RedrawPausedLevel = Math.Max(RedrawPausedLevel - 1, 0);
            }
        }

        protected override void RequestRedraw()
        {
            if (RedrawPausedLevel == 0 && Enabled)
            {
                RedrawPausedLevel++;

                Draw();

                RedrawPausedLevel = Math.Max(RedrawPausedLevel - 1, 0);
            }
        }

        public void RequestUpdateDisplay() { RequestMeasure(); }

        private void Draw() { Draw(Application, new ConsoleArea(Size)); }

        public ApplicationBase Application { get; set; }
    }
}