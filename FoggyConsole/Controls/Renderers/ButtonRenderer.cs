using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . Controls . Renderers
{

	/// <summary>
	///     Draws a
	///     <code>Button</code>
	///     -Control
	/// </summary>
	public class ButtonRenderer : ControlRenderer <Button>
	{

		/// <summary>
		///     Draws the
		///     <code>Button</code>
		///     given in the Control-Property.
		/// </summary>
		/// <exception cref="InvalidOperationException">Is thrown if the Control-Property isn't set.</exception>
		/// <exception cref="InvalidOperationException">Is thrown if the CalculateBoundary-Method hasn't been called.</exception>
		public override void DrawOverride ( Application application , ConsoleArea area )
		{
			if ( Control is null )
			{
				return ;
			}

			if ( Control . ActualSize . IsEmpty )
			{
				return ;
			}

			ConsoleColor foregroundColor = Control . ActualForegroundColor ;
			ConsoleColor backgroundColor = Control . ActualBackgroundColor ;

			area . Fill ( backgroundColor ) ;

			if ( Control . ContentHeight == 1 )
			{
				int startPosition = Control . ContentWidth - Control . Text . Length ;
				startPosition = ( startPosition            + startPosition % 2 ) / 2 ;
				startPosition = Math . Max ( startPosition , 0 ) ;

				for ( int x = 0 ; x < Control . ContentWidth && x < Control . Text . Length ; x++ )
				{
					area [ x + startPosition , 0 ] = new ConsoleChar (
																	  Control . Text [ x ] ,
																	  foregroundColor ,
																	  backgroundColor ) ;
				}
			}
			else
			{
				int startLine = ( Control . ContentHeight - Control . Lines . Count ) / 2 ;
				startLine = Math . Max ( startLine , 0 ) ;

				for ( int y = 0 ; y < Control . Lines . Count && y + startLine < Control . ContentHeight ; y++ )
				{
					string currentLine = Control . Lines [ y ] ;

					int startPosition = Control . ContentWidth - currentLine . Length ;
					startPosition = ( startPosition            + startPosition % 2 ) / 2 ;
					startPosition = Math . Max ( startPosition , 0 ) ;

					for ( int x = 0 ; x < Control . ContentWidth - 2 && x < currentLine . Length ; x++ )
					{
						area [ x + startPosition , startLine + y ] =
							new ConsoleChar ( currentLine [ x ] , foregroundColor , backgroundColor ) ;
					}
				}
			}

			//Todo:
		}

	}

}
