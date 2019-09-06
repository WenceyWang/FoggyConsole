using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Text . RegularExpressions ;

namespace DreamRecorder . FoggyConsole . XtermConsole . EscapeSequenceParsers
{

	public class KeyEscapeSequenceParser : IInputEscapeSequenceParser
	{

		public readonly Regex FullEscape = new Regex ( @"^\u001b\[(A|B|C|D|H|F|3~|2~|B|6~|D|E|C|A|5~)" ) ;

		public readonly Regex TryEscape = new Regex ( @"^\u001b(?:\[|$)(A|B|C|D|H|F|3~|2~|B|6~|D|E|C|A|5~|$)" ) ;

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
				switch ( match . Groups [ 1 ] . Value )
				{
					case "A" :
					{
						console . InvokeKeyPressed (
													new ConsoleKeyInfo (
																		default ,
																		ConsoleKey . UpArrow ,
																		false ,
																		false ,
																		false ) ) ;
						break ;
					}

					case "B" :
					{
						console . InvokeKeyPressed (
													new ConsoleKeyInfo (
																		default ,
																		ConsoleKey . DownArrow ,
																		false ,
																		false ,
																		false ) ) ;
						break ;
					}

					case "C" :
					{
						console . InvokeKeyPressed (
													new ConsoleKeyInfo (
																		default ,
																		ConsoleKey . RightArrow ,
																		false ,
																		false ,
																		false ) ) ;
						break ;
					}

					case "D" :
					{
						console . InvokeKeyPressed (
													new ConsoleKeyInfo (
																		default ,
																		ConsoleKey . LeftArrow ,
																		false ,
																		false ,
																		false ) ) ;
						break ;
					}

					case "H" :
					{
						console . InvokeKeyPressed (
													new ConsoleKeyInfo (
																		default ,
																		ConsoleKey . Home ,
																		false ,
																		false ,
																		false ) ) ;
						break ;
					}

					case "F" :
					{
						console . InvokeKeyPressed (
													new ConsoleKeyInfo (
																		default ,
																		ConsoleKey . End ,
																		false ,
																		false ,
																		false ) ) ;
						break ;
					}
				}

				content . RemoveRange ( 0 , match . Value . Length ) ;
			}
		}

	}

}
