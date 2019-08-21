using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Logging;

namespace DreamRecorder.FoggyConsole.Controls.Renderers
{

    public class FrameRenderer : ControlRenderer<Frame>
    {

        public override void Draw(ApplicationBase application, ConsoleArea area)
        {
            area.Fill(Control.ActualBackgroundColor);

            Control.Content?.Draw(application, area);

            Rectangle? focusedArea = application.FocusManager?.FocusedControl?.RenderArea;

            if (focusedArea.IsNotEmpty())
            {
                area.CreateSub(focusedArea.Value).InvertColor();
            }

            application.Console.Draw(Control?.RenderPoint ?? Point.Zero, area);

        }

    }

}
