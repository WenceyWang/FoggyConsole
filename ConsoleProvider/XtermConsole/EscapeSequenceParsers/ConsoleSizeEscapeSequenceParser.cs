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

		public ParseResult TryParse ( List <char> content )
		{
			string contentString = new string ( content . ToArray ( ) ) ;

			Match tryMatch = TryEscape . Match ( contentString ) ;

			if ( tryMatch . Success )
			{
				Match fullMatch = FullEscape . Match ( contentString ) ;

				if ( fullMatch . Success )
				{
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

		public void Apply ( List <char> content , XtermConsole console )
		{
			string contentString = new string ( content . ToArray ( ) ) ;

			Match match = FullEscape . Match ( contentString ) ;

			if ( match . Success )
			{
				int height = Convert . ToInt32 ( match . Groups [ 1 ] . Value ) ;
				int width  = Convert . ToInt32 ( match . Groups [ 2 ] . Value ) ;

				console . InternalSize = new Size ( width , height ) ;

				content . RemoveRange ( 0 , match . Value . Length ) ;
			}
		}

	}

}
