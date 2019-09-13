using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . Controls . Renderers
{

	/// <summary>
	///     Draws a
	///     <code>ProgressBar</code>
	///     -control
	/// </summary>
	public class ProgressBarRenderer : ControlRenderer <ProgressBar>
	{

		/// <summary>
		///     Draws the ProgressBar given in the Control-Property
		/// </summary>
		public override void Draw ( Application application , ConsoleArea area )
		{
			if ( Control is null )
			{
				return ;
			}

			if ( Control . ActualSize . IsEmpty )
			{
				return ;
			}

            ConsoleColor foregroundColor = Control.ActualForegroundColor;
            ConsoleColor backgroundColor = Control.ActualBackgroundColor;

            area . Fill ( backgroundColor ) ;

			int barStart ;
			int barMaxWidth ;

			if ( Control . BoarderStyle != null )
			{
				barMaxWidth = Control . ActualWidth - 2 ;
				barStart    = 1 ;

				area . DrawBoarder ( Control . BoarderStyle . Value , foregroundColor , backgroundColor ) ;
			}
			else
			{
				barMaxWidth = Control . ActualWidth ;
				barStart    = 0 ;
			}

            double valuePerCharacter = (Control.MaxValue - Control.MinValue) / (double)barMaxWidth;

            double CurrentValue = Control.Value/valuePerCharacter;

			for ( int y = 0 ; y < Control . ActualHeight ; y++ )
			{
				for ( int x = barStart ; x < barMaxWidth ; x++ )
				{
					area [ x , 0 ] = new ConsoleChar (
														  Control.CharProvider.GetChar(CurrentValue),
														  foregroundColor ,
														  backgroundColor ) ;

                    CurrentValue--;
				}
			}
		}

	}

}
