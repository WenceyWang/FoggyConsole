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

			ConsoleColor foregroundColor ;
			ConsoleColor backgroundColor ;

			foregroundColor = Control . ActualForegroundColor ;
			backgroundColor = Control . ActualBackgroundColor ;

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

			int barWidth = barMaxWidth
						   * ( ( Control . Value      - Control . MinValue )
							   / ( Control . MaxValue - Control . MinValue ) ) ;


			for ( int y = 0 ; y < Control . ActualHeight ; y++ )
			{
				for ( int x = barStart ; x < barWidth ; x++ )
				{
					area [ x + 1 , 0 ] = new ConsoleChar (
														  Control . ProgressChar ,
														  foregroundColor ,
														  backgroundColor ) ;
				}
			}
		}

	}

}
