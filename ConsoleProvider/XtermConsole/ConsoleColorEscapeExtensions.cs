using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . XtermConsole
{

	public static class ConsoleColorEscapeExtensions
	{

		public static string ForegroundColorToCode ( this ConsoleColor color )
		{
			switch ( color )
			{
				/*
                LIGHT
				Ps = 9 0  -> Set foreground color to Black.
				Ps = 9 1  -> Set foreground color to Red.
				Ps = 9 2  -> Set foreground color to Green.
				Ps = 9 3  -> Set foreground color to Yellow.
				Ps = 9 4  -> Set foreground color to Blue.
				Ps = 9 5  -> Set foreground color to Magenta.
				Ps = 9 6  -> Set foreground color to Cyan.
				Ps = 9 7  -> Set foreground color to White.
				 */

				case ConsoleColor . Black :
					return "30" ;
				case ConsoleColor . Blue :
					return "94" ;
				case ConsoleColor . Cyan :
					return "96" ;
				case ConsoleColor . Gray :
					return "37" ;
				case ConsoleColor . Green :
					return "92" ;
				case ConsoleColor . Magenta :
					return "95" ;
				case ConsoleColor . Red :
					return "91" ;
				case ConsoleColor . White :
					return "97" ;
				case ConsoleColor . Yellow :
					return "93" ;


				/*
				 * DARK
				 * Ps = 3 0  -> Set foreground color to Black.
				 * Ps = 3 1  -> Set foreground color to Red.
				 * Ps = 3 2  -> Set foreground color to Green.
				 * Ps = 3 3  -> Set foreground color to Yellow.
				 * Ps = 3 4  -> Set foreground color to Blue.
				 * Ps = 3 5  -> Set foreground color to Magenta.
				 * Ps = 3 6  -> Set foreground color to Cyan.
				 * Ps = 3 7  -> Set foreground color to White.
				 */

				case ConsoleColor . DarkBlue :
					return "34" ;
				case ConsoleColor . DarkCyan :
					return "36" ;
				case ConsoleColor . DarkGray :
					return "90" ;
				case ConsoleColor . DarkGreen :
					return "32" ;
				case ConsoleColor . DarkMagenta :
					return "35" ;
				case ConsoleColor . DarkRed :
					return "31" ;
				case ConsoleColor . DarkYellow :
					return "33" ;
				default :
					throw new ArgumentOutOfRangeException ( nameof ( color ) , color , null ) ;
			}
		}

		public static string BackgroundColorToCode ( this ConsoleColor color )
		{
			switch ( color )
			{
				/*
                LIGHT
				Ps = 1 0 0  -> Set foreground color to Black.
				Ps = 1 0 1  -> Set foreground color to Red.
				Ps = 1 0 2  -> Set foreground color to Green.
				Ps = 1 0 3  -> Set foreground color to Yellow.
				Ps = 1 0 4  -> Set foreground color to Blue.
				Ps = 1 0 5  -> Set foreground color to Magenta.
				Ps = 1 0 6  -> Set foreground color to Cyan.
				Ps = 1 0 7  -> Set foreground color to White.
				 */

				case ConsoleColor . Black :
					return "40" ;
				case ConsoleColor . Blue :
					return "104" ;
				case ConsoleColor . Cyan :
					return "106" ;
				case ConsoleColor . Gray :
					return "47" ;
				case ConsoleColor . Green :
					return "102" ;
				case ConsoleColor . Magenta :
					return "105" ;
				case ConsoleColor . Red :
					return "101" ;
				case ConsoleColor . White :
					return "107" ;
				case ConsoleColor . Yellow :
					return "103" ;


				/*
				 * DARK
				 * Ps = 4 0  -> Set foreground color to Black.
				 * Ps = 4 1  -> Set foreground color to Red.
				 * Ps = 4 2  -> Set foreground color to Green.
				 * Ps = 4 3  -> Set foreground color to Yellow.
				 * Ps = 4 4  -> Set foreground color to Blue.
				 * Ps = 4 5  -> Set foreground color to Magenta.
				 * Ps = 4 6  -> Set foreground color to Cyan.
				 * Ps = 4 7  -> Set foreground color to White.
				 */

				case ConsoleColor . DarkBlue :
					return "44" ;
				case ConsoleColor . DarkCyan :
					return "46" ;
				case ConsoleColor . DarkGray :
					return "100" ;
				case ConsoleColor . DarkGreen :
					return "42" ;
				case ConsoleColor . DarkMagenta :
					return "45" ;
				case ConsoleColor . DarkRed :
					return "41" ;
				case ConsoleColor . DarkYellow :
					return "43" ;
				default :
					throw new ArgumentOutOfRangeException ( nameof ( color ) , color , null ) ;
			}
		}

	}

}
