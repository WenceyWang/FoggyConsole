using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . Controls . Renderers
{

	/// <summary>
	///     Draws a TextBox
	/// </summary>
	public class TextBoxRenderer : ControlRenderer <TextBox>
	{

		/// <summary>
		///     Draws the TextBox given in the Control-Property
		/// </summary>
		public override void Draw ( ConsoleArea area )
		{
			if ( Control . IsFocused )
			{
				area . Fill ( Control . ActualForegroundColor ) ;

				for ( int y = 0 ;
					y < Control . ActualHeight && y * Control . ActualHeight < Control . Text . Length ;
					y++ )
				{
					for ( int x = 0 ; x < Control . ActualWidth ; x++ )
					{
						if ( x + y * Control . ActualWidth < Control . Text . Length )
						{
							area [ x , y ] = new ConsoleChar (
															Control . Text [ x * y ] ,
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
				area . Fill ( Control . ActualBackgroundColor ) ;

				for ( int y = 0 ;
					y < Control . ActualHeight && y * Control . ActualHeight < Control . Text . Length ;
					y++ )
				{
					for ( int x = 0 ; x < Control . ActualWidth ; x++ )
					{
						if ( x + y * Control . ActualWidth < Control . Text . Length )
						{
							area [ x , y ] = new ConsoleChar (
															Control . Text [ x * y ] ,
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
		}

	}

}
