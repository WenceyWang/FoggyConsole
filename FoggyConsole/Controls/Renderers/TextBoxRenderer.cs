using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace WenceyWang . FoggyConsole . Controls . Renderers
{

	/// <summary>
	///     Draws a textbox
	/// </summary>
	public class TextBoxRenderer : ControlRenderer <TextBox>
	{

		/// <summary>
		///     Draws the textbox given in the Control-Property
		/// </summary>
		public override void Draw ( )
		{
			ConsoleArea result ;

			if ( Control . IsFocused )
			{
				result = new ConsoleArea ( Control . ActualSize , Control . ActualForegroundColor ) ;

				for ( int y = 0 ; y < Control . ActualHeight && y * Control . ActualHeight < Control . Text . Length ; y++ )
				{
					for ( int x = 0 ; x < Control . ActualWidth ; x++ )
					{
						if ( x + y * Control . ActualWidth < Control . Text . Length )
						{
							result [ x , y ] = new ConsoleChar ( Control . Text [ x * y ] ,
																Control . ActualForegroundColor ,
																Control . ActualBackgroundColor ) ;
						}
						else
						{
							break ;
						}
					}
				}
			}
			else
			{
				result = new ConsoleArea ( Control . ActualSize , Control . ActualBackgroundColor ) ;

				for ( int y = 0 ; y < Control . ActualHeight && y * Control . ActualHeight < Control . Text . Length ; y++ )
				{
					for ( int x = 0 ; x < Control . ActualWidth ; x++ )
					{
						if ( x + y * Control . ActualWidth < Control . Text . Length )
						{
							result [ x , y ] = new ConsoleChar ( Control . Text [ x * y ] ,
																Control . ActualForegroundColor ,
																Control . ActualBackgroundColor ) ;
						}
						else
						{
							break ;
						}
					}
				}
			}

			FogConsole . Draw ( Control . RenderPoint , result ) ;
		}

	}

}
