using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Text ;

namespace WenceyWang . FoggyConsole
{

	/// <summary>
	///     Abstracts all calls on the
	///     <code>System.Console</code>
	///     class.
	///     Ensures that expensive operations like setting
	///     <code>Console.CursorLeft</code>
	///     or
	///     <code>Console.ForegroundColor</code>
	///     are only executed if neccessary.
	/// </summary>
	internal static class FogConsole
	{

		private static ConsoleColor CurrentForegroundColor { get ; set ; }

		private static ConsoleColor CurrentBackgroundColor { get ; set ; }

		public static void Draw ( Point position , ConsoleArea area )
		{
			Draw ( new Rectangle ( position , area . Size ) , area . Content ) ;
		}

		public static void Draw ( Rectangle position , ConsoleChar [ , ] content )
		{
			StringBuilder stringBuilder = new StringBuilder ( ) ;
			for ( int y = Math . Max ( - position . Y , 0 ) ; y < position . Height ; y++ )
			{
				Console . SetCursorPosition ( Math . Max ( position . Left , 0 ) , Math . Max ( position . Top + y , 0 ) ) ;
				for ( int x = Math . Max ( - position . X , 0 ) ; x < position . Width ; x++ )
				{
					ConsoleColor targetBackgroundColor = content [ x , y ] . BackgroundColor ;
					ConsoleColor targetForegroundColor = content [ x , y ] . ForegroundColor ;
					if ( CurrentBackgroundColor != targetBackgroundColor
						|| CurrentForegroundColor != targetForegroundColor )
					{
						Console . Write ( stringBuilder . ToString ( ) ) ;
						stringBuilder . Clear ( ) ;
						Console . BackgroundColor = CurrentBackgroundColor = targetBackgroundColor ;
						Console . ForegroundColor = CurrentForegroundColor = targetForegroundColor ;
					}
					stringBuilder . Append ( content [ x , y ] . Character ) ;
				}

				Console . Write ( stringBuilder . ToString ( ) ) ;
				stringBuilder . Clear ( ) ;
			}
		}

	}

}
