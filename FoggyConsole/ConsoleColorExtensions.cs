using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole
{

	public static class ConsoleColorExtensions
	{

		public static ConsoleColor Invert ( this ConsoleColor color )
		{
			switch ( color )
			{
				case ConsoleColor . Black :
					return ConsoleColor . White ;
				case ConsoleColor . Blue :
					return ConsoleColor . DarkYellow ;
				case ConsoleColor . Cyan :
					return ConsoleColor . DarkRed ;
				case ConsoleColor . DarkBlue :
					return ConsoleColor . Yellow ;
				case ConsoleColor . DarkCyan :
					return ConsoleColor . Red ;
				case ConsoleColor . DarkGray :
					return ConsoleColor . Gray ;
				case ConsoleColor . DarkGreen :
					return ConsoleColor . Magenta ;
				case ConsoleColor . DarkMagenta :
					return ConsoleColor . Green ;
				case ConsoleColor . DarkRed :
					return ConsoleColor . Cyan ;
				case ConsoleColor . DarkYellow :
					return ConsoleColor . Blue ;
				case ConsoleColor . Gray :
					return ConsoleColor . DarkGray ;
				case ConsoleColor . Green :
					return ConsoleColor . DarkMagenta ;
				case ConsoleColor . Magenta :
					return ConsoleColor . DarkGreen ;
				case ConsoleColor . Red :
					return ConsoleColor . DarkCyan ;
				case ConsoleColor . White :
					return ConsoleColor . Black ;
				case ConsoleColor . Yellow :
					return ConsoleColor . DarkBlue ;
				default :
					throw new ArgumentOutOfRangeException ( nameof ( color ) , color , null ) ;
			}
		}

	}

}