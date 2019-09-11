using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . XtermConsole . EscapeSequenceParsers
{

	public interface IInputEscapeSequenceParser
	{

		ParseResult TryParse ( List <char> content , XtermConsole console ) ;

	}

}
