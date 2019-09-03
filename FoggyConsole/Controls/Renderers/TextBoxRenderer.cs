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

			{
				area . Fill ( backgroundColor ) ;

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
															  foregroundColor ,
															  backgroundColor ) ;
						}
						else
						{
							break ;
						}
					}
				}
			}

			if ( Control . BoarderStyle != null )
			{
				area . DrawBoarder ( Control . BoarderStyle . Value , foregroundColor , backgroundColor ) ;
			}
		}

	}

}
