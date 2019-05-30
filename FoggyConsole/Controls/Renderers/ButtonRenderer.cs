using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace WenceyWang . FoggyConsole . Controls . Renderers
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
            if (Control is null)
            {
                return;
            }
            if (Control.Size.IsEmpty)
            {
                return;
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
				result [ 0 , 0 ] = new ConsoleChar ( '[' , foregroundColor , backgroundColor ) ;
				result [ Control . ActualWidth - 1 , 0 ] = new ConsoleChar ( ']' , foregroundColor , backgroundColor ) ;
				int startPosition = ( Control . RenderArea . Width - Control . Text . Length ) / 2 ;
				for ( int x = 0 ; x < Control . ActualWidth - 2 && x < Control . Text . Length ; x++ )
				{
					result [ x + startPosition , 0 ] = new ConsoleChar ( Control . Text [ x ] , foregroundColor , backgroundColor ) ;
				}
			}
			else
			{
				result [ 0 , 0 ] = new ConsoleChar ( '┌' , foregroundColor , backgroundColor ) ;
				result [ Control . ActualWidth - 1 , 0 ] = new ConsoleChar ( '┐' , foregroundColor , backgroundColor ) ;
				result [ 0 , Control . ActualHeight - 1 ] = new ConsoleChar ( '└' , foregroundColor , backgroundColor ) ;
				result [ Control . ActualWidth - 1 , Control . ActualHeight - 1 ] =
					new ConsoleChar ( '┘' , foregroundColor , backgroundColor ) ;

				string [ ] lines = Control . Text . Split ( Environment . NewLine . ToCharArray ( ) ) ;

				int startLine = ( Control . ActualHeight - lines . Length ) / 2 ;

				for ( int y = 0 ; y < lines . Length && y + startLine < Control . ActualHeight ; y++ )
				{
					int startPosition = ( Control . RenderArea . Width - lines [ y ] . Length ) / 2 ;

					for ( int x = 0 ; x < Control . ActualWidth - 2 && x < lines [ y ] . Length ; x++ )
					{
						result [ x + startPosition , startLine+y] = new ConsoleChar ( lines [ y ] [ x ] , foregroundColor , backgroundColor ) ;
					}
				}
			}


			FogConsole . Draw ( Control . RenderPoint , result ) ;

			//Todo:
		}

	}

}
