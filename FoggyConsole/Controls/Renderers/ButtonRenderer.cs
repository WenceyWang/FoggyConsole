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
		public override void Draw ( )
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

			if ( Control . IsFocused )
			{
				foregroundColor = Control . ActualBackgroundColor ;
				backgroundColor = Control . ActualForegroundColor ;
			}
			else
			{
				foregroundColor = Control . ActualForegroundColor ;
				backgroundColor = Control . ActualBackgroundColor ;
			}


			ConsoleArea result = new ConsoleArea ( Control . ActualSize , backgroundColor ) ;
			if ( Control . ActualHeight == 1 )
			{
				int startPosition = ( Control . RenderArea . Width - Control . Text . Length ) ;
				startPosition = ( startPosition + startPosition % 2 ) / 2 ;

				for ( int x = 0 ; x < Control . ActualWidth && x < Control . Text . Length ; x++ )
				{
					result [ x + startPosition , 0 ] =
						new ConsoleChar ( Control . Text [ x ] , foregroundColor , backgroundColor ) ;
				}

				if ( Control . BoarderStyle != null )
				{
					result [ 0 , 0 ] = new ConsoleChar (
														Control . BoarderStyle . Value . SingleLineLeftEdge ,
														foregroundColor ,
														backgroundColor ) ;

					result [ Control . ActualWidth - 1 , 0 ] =
						new ConsoleChar ( ']' , foregroundColor , backgroundColor ) ;
				}
			}
			else
			{
				string [ ] lines = Control . Text . Split ( Environment . NewLine . ToCharArray ( ) ) ;

				int startLine = ( Control . ActualHeight - lines . Length ) / 2 ;

				for ( int y = 0 ; y < lines . Length && y + startLine < Control . ActualHeight ; y++ )
				{
					string currentLine = lines [ y ] ;

					int startPosition = ( Control . RenderArea . Width - currentLine . Length ) ;
					startPosition = ( startPosition + startPosition % 2 ) / 2 ;

					for ( int x = 0 ; x < Control . ActualWidth - 2 && x < currentLine . Length ; x++ )
					{
						result [ x + startPosition , startLine + y ] =
							new ConsoleChar ( currentLine [ x ] , foregroundColor , backgroundColor ) ;
					}
				}

				if ( Control . BoarderStyle != null )
				{
					result [ 0 , 0 ] = new ConsoleChar (
														Control . BoarderStyle . Value . TopLeftCorner ,
														foregroundColor ,
														backgroundColor ) ;
					result [ Control . ActualWidth - 1 , 0 ] = new ConsoleChar (
																				Control . BoarderStyle . Value .
																						TopRightCorner ,
																				foregroundColor ,
																				backgroundColor ) ;
					result [ 0 , Control . ActualHeight - 1 ] = new ConsoleChar (
																				Control . BoarderStyle . Value .
																						BottomLeftCorner ,
																				foregroundColor ,
																				backgroundColor ) ;
					result [ Control . ActualWidth - 1 , Control . ActualHeight - 1 ] =
						new ConsoleChar (
										Control . BoarderStyle . Value . BottomRightCorner ,
										foregroundColor ,
										backgroundColor ) ;

					for ( int x = 1 ; x < Control . RenderArea . Width - 1 ; x++ )
					{
						result [ x , 0 ] = new ConsoleChar (
															Control . BoarderStyle . Value . HorizontalEdge ,
															foregroundColor ,
															backgroundColor ) ;
						result [ x , Control . RenderArea . Height - 1 ] = new ConsoleChar (
																							Control . BoarderStyle .
																									Value .
																									HorizontalEdge ,
																							foregroundColor ,
																							backgroundColor ) ;
					}

					for ( int y = 1 ; y < Control . RenderArea . Height - 1 ; y++ )
					{
						result [ 0 , y ] = new ConsoleChar (
															Control . BoarderStyle . Value . VerticalEdge ,
															foregroundColor ,
															backgroundColor ) ;
						result [ Control . RenderArea . Width - 1 , y ] = new ConsoleChar (
																							Control . BoarderStyle .
																									Value .
																									VerticalEdge ,
																							foregroundColor ,
																							backgroundColor ) ;
					}
				}
			}


			FogConsole . Draw ( Control . RenderPoint , result ) ;

			//Todo:
		}

	}

}
