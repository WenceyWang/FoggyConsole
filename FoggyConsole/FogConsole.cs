using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Text ;

namespace DreamRecorder . FoggyConsole
{

	/// <summary>
	///     Abstracts all calls on the
	///     <code>System.Console</code>
	///     class.
	///     Ensures that expensive operations like setting
	///     <code>Console.CursorLeft</code>
	///     or
	///     <code>Console.ForegroundColor</code>
	///     are only executed if necessary.
	/// </summary>
	internal static class FogConsole
	{

		public static int WriteCount { get ; set ; }

		private static ConsoleColor CurrentForegroundColor { get ; set ; }

		private static ConsoleColor CurrentBackgroundColor { get ; set ; }

		public static void Draw ( Point position , ConsoleArea area )
		{
			Draw ( new Rectangle ( position , area . Size ) , area . Content ) ;
		}

		public static void Draw ( Rectangle position , ConsoleChar [ , ] content )
		{
			try
			{
				CurrentBackgroundColor = Console . BackgroundColor ;
				CurrentForegroundColor = Console . ForegroundColor ;

				Rectangle consoleArea = new Rectangle ( new Point ( ) , Window . Size ) ;

				position = Rectangle . Intersect ( position , consoleArea ) ;

				bool changeLine = position . Right != consoleArea . Right || position . Left != consoleArea . Left ;

				StringBuilder stringBuilder = new StringBuilder ( content . Length ) ;

				if ( ! changeLine )
				{
					Console . SetCursorPosition ( position . Left , position . Top ) ;
				}

				for ( int y = 0 ; y < position . Height ; y++ )
				{
					if ( changeLine )
					{
						Console . SetCursorPosition ( position . Left , position . Top + y ) ;
					}

					for ( int x = Math . Max ( - position . X , 0 ) ; x < position . Width ; x++ )
					{
						ConsoleChar currentPosition = content [ x , y ] ;

						ConsoleColor targetBackgroundColor = currentPosition . BackgroundColor ;
						ConsoleColor targetForegroundColor = currentPosition . ForegroundColor ;

						if ( CurrentBackgroundColor != targetBackgroundColor
							|| CurrentForegroundColor != targetForegroundColor
							&& ! char . IsWhiteSpace ( currentPosition . Character ) )
						{
							Write ( stringBuilder ) ;

							Console . BackgroundColor = CurrentBackgroundColor = targetBackgroundColor ;
							Console . ForegroundColor = CurrentForegroundColor = targetForegroundColor ;
						}

						stringBuilder . Append ( currentPosition . Character ) ;
					}

					if ( changeLine )
					{
						Write ( stringBuilder ) ;
					}
				}

				if ( ! changeLine )
				{
					Write ( stringBuilder ) ;
				}

				Console . SetCursorPosition ( position . Left , position . Top ) ;
			}
			catch ( Exception e )
			{
			}
		}

		private static void Write ( StringBuilder stringBuilder )
		{
			if ( stringBuilder . Length > 0 )
			{
				Console . Write ( stringBuilder . ToString ( ) ) ;
				stringBuilder . Clear ( ) ;

				WriteCount++ ;
			}
		}

	}

}
