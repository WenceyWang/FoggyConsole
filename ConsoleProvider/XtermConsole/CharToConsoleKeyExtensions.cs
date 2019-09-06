using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Globalization ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . XtermConsole
{

	public static class CharToConsoleKeyExtensions
	{

		public static ConsoleKeyInfo ? ToConsoleKey ( this char c )
		{
			if ( Enum . TryParse (
								  c . ToString ( CultureInfo . InvariantCulture ) ,
								  true ,
								  out ConsoleKey consoleKey ) )
			{
				return new ConsoleKeyInfo ( c , consoleKey , char . IsUpper ( c ) , false , false ) ;
			}
			else
			{
				switch ( c )
				{
					case ' ' :
						return new ConsoleKeyInfo ( c , ConsoleKey . Spacebar , false , false , false ) ;
					case '\u0009' :
						return new ConsoleKeyInfo ( c , ConsoleKey . Tab , false , false , false ) ;
					case ';' :
						return new ConsoleKeyInfo ( c , ConsoleKey . Oem1 , false , false , false ) ;
					case ':' :
						return new ConsoleKeyInfo ( c , ConsoleKey . Oem1 , true , false , false ) ;
					case '/' :
						return new ConsoleKeyInfo ( c , ConsoleKey . Oem2 , false , false , false ) ;
					case '?' :
						return new ConsoleKeyInfo ( c , ConsoleKey . Oem2 , true , false , false ) ;
					case '`' :
						return new ConsoleKeyInfo ( c , ConsoleKey . Oem3 , false , false , false ) ;
					case '~' :
						return new ConsoleKeyInfo ( c , ConsoleKey . Oem3 , true , false , false ) ;
					case '[' :
						return new ConsoleKeyInfo ( c , ConsoleKey . Oem4 , false , false , false ) ;
					case '{' :
						return new ConsoleKeyInfo ( c , ConsoleKey . Oem4 , true , false , false ) ;
					case '\\' :
						return new ConsoleKeyInfo ( c , ConsoleKey . Oem5 , false , false , false ) ;
					case '|' :
						return new ConsoleKeyInfo ( c , ConsoleKey . Oem5 , true , false , false ) ;
					case ']' :
						return new ConsoleKeyInfo ( c , ConsoleKey . Oem6 , false , false , false ) ;
					case '}' :
						return new ConsoleKeyInfo ( c , ConsoleKey . Oem6 , true , false , false ) ;
					case ',' :
						return new ConsoleKeyInfo ( c , ConsoleKey . OemComma , false , false , false ) ;
					case '<' :
						return new ConsoleKeyInfo ( c , ConsoleKey . OemComma , true , false , false ) ;
					case '.' :
						return new ConsoleKeyInfo ( c , ConsoleKey . OemComma , false , false , false ) ;
					case '>' :
						return new ConsoleKeyInfo ( c , ConsoleKey . OemComma , true , false , false ) ;
					case '-' :
						return new ConsoleKeyInfo ( c , ConsoleKey . OemMinus , false , false , false ) ;
					case '_' :
						return new ConsoleKeyInfo ( c , ConsoleKey . OemMinus , true , false , false ) ;
					case '=' :
						return new ConsoleKeyInfo ( c , ConsoleKey . OemPlus , false , false , false ) ;
					case '+' :
						return new ConsoleKeyInfo ( c , ConsoleKey . OemPlus , true , false , false ) ;
					case '\r' :
						return new ConsoleKeyInfo ( c , ConsoleKey . Enter , false , false , false ) ;
					case '\u001b' :
						return new ConsoleKeyInfo ( c , ConsoleKey . Escape , false , false , false ) ;
					default :
						return null ;
				}
			}
		}

	}

}
