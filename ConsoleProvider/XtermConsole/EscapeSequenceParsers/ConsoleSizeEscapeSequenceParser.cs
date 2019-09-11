using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Text . RegularExpressions ;

namespace DreamRecorder . FoggyConsole . XtermConsole . EscapeSequenceParsers
{

	public class ConsoleSizeEscapeSequenceParser : IInputEscapeSequenceParser
	{

		public readonly Regex FullEscape = new Regex ( @"^\u001b\[8;(\d+);(\d+)t" ) ;

		public readonly Regex TryEscape =
			new Regex ( @"^\u001b(?:\[|$)(?:8|$)(?:;|$)(?:(\d+)|$)(?:;|$)(?:(\d+)|$)(?:t|$)" ) ;

		public ParseResult TryParse ( List <char> content , XtermConsole console )
		{
			string contentString ;

			lock ( content )
			{
				contentString = new string ( content . ToArray ( ) ) ;
			}

			Match tryMatch = TryEscape . Match ( contentString ) ;

			if ( tryMatch . Success )
			{
				Match fullMatch = FullEscape . Match ( contentString ) ;

				if ( fullMatch . Success )
				{
					lock ( content )
					{
						content . RemoveRange ( 0 , fullMatch . Value . Length ) ;
					}

					int height = Convert . ToInt32 ( fullMatch . Groups [ 1 ] . Value ) ;
					int width  = Convert . ToInt32 ( fullMatch . Groups [ 2 ] . Value ) ;

					console . InternalSize = new Size ( width , height ) ;

					return ParseResult . Finished ;
				}
				else
				{
					return ParseResult . Established ;
				}
			}
			else
			{
				return ParseResult . CanNotEstablish ;
			}
		}

	}

}
