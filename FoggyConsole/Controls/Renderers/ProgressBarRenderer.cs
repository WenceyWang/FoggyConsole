using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DreamRecorder.FoggyConsole.Controls.Renderers
{

    /// <summary>
    ///     Draws a
    ///     <code>ProgressBar</code>
    ///     -control
    /// </summary>
    public class ProgressBarRenderer : ControlRenderer<ProgressBar>
    {

        /// <summary>
        ///     Draws the ProgressBar given in the Control-Property
        /// </summary>
        public override void DrawOverride(Application application, ConsoleArea area)
        {
            if (Control is null)
            {
                return;
            }

            if (Control.ActualSize.IsEmpty)
            {
                return;
            }

            ConsoleColor foregroundColor = Control.ActualForegroundColor;
            ConsoleColor backgroundColor = Control.ActualBackgroundColor;


			int barMaxWidth = Control.ActualWidth ;
            int barXStart = 0 ;
            int barHeight = Control.ActualHeight ;
            int barYStart = 0 ;

            double valuePerCharacter = (Control.MaxValue - Control.MinValue) / (double)barMaxWidth;

            for (int y = barYStart; y < barHeight; y++)
            {
                double currentValue = Control.Value / valuePerCharacter;

                for (int x = barXStart; x < barMaxWidth; x++)
                {
                    area[x, y] = new ConsoleChar(
                                                      Control.CharProvider.GetChar(currentValue),
                                                      foregroundColor,
                                                      backgroundColor);

                    currentValue--;
                }
            }
        }

    }

}
