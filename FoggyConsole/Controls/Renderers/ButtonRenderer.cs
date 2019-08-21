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
		public override void Draw(ApplicationBase application, ConsoleArea area)
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

			if ( Control . ActualHeight == 1 )
			{
				int startPosition = Control . ActualWidth - Control . Text . Length ;
				startPosition = ( startPosition           + startPosition % 2 ) / 2 ;
				startPosition = Math . Max ( startPosition , 0 ) ;

				for ( int x = 0 ; x < Control . ActualWidth && x < Control . Text . Length ; x++ )
				{
					area [ x + startPosition , 0 ] = new ConsoleChar (
																	Control . Text [ x ] ,
																	foregroundColor ,
																	backgroundColor ) ;
				}
			}
			else
			{
				string [ ] lines =
					Control . Text . Split ( Environment . NewLine . ToCharArray ( ) ) ;

				int startLine = ( Control . ActualHeight - lines . Length ) / 2 ;
				startLine = Math . Max ( startLine , 0 ) ;

				for ( int y = 0 ;
					y < lines . Length && y + startLine < Control . ActualHeight ;
					y++ )
				{
					string currentLine = lines [ y ] ;

					int startPosition = Control . ActualWidth - currentLine . Length ;
					startPosition = ( startPosition           + startPosition % 2 ) / 2 ;
					startPosition = Math . Max ( startPosition , 0 ) ;

					for ( int x = 0 ;
						x < Control . ActualWidth - 2 && x < currentLine . Length ;
						x++ )
					{
						area [ x + startPosition , startLine + y ] =
							new ConsoleChar (
											currentLine [ x ] ,
											foregroundColor ,
											backgroundColor ) ;
					}
				}
			}

			if ( Control . BoarderStyle != null )
			{
				area . DrawBoarder (
									Control . BoarderStyle . Value ,
									foregroundColor ,
									backgroundColor ) ;
			}

			//Todo:
		}

	}

}
