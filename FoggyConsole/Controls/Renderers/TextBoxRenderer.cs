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
		public override void DrawOverride ( Application application , ConsoleArea area )
		{
			
			ConsoleColor foregroundColor = Control . ActualForegroundColor ;
			ConsoleColor backgroundColor = Control . ActualBackgroundColor ;

			area . Fill ( backgroundColor ) ;

			int y = 0 ;

			for ( int lineIndex = 0 ; lineIndex < Control . Lines . Count ; lineIndex++ )
			{
				string line       = Control . Lines [ lineIndex ] ;
				string renderLine = $"{line} " ;
				int    x          = 0 ;

				for ( int position = 0 ; position < renderLine . Length && y < Control . ContentHeight ; position++ )
				{
					char t = renderLine [ position ] ;
					area [ x , y ] = new ConsoleChar ( t , foregroundColor , backgroundColor ) ;

					if ( Control . CursorPosition . X    == position
						 && Control . CursorPosition . Y == lineIndex )
					{
						area [ x , y ] = area [ x , y ] . InvertColor ( ) ;
					}

					x++ ;

					if ( x >= Control . ActualWidth )
					{
						x %= Control . ActualWidth ;
						y++ ;
					}
				}

				y++ ;
			}

		
		}

	}

}
