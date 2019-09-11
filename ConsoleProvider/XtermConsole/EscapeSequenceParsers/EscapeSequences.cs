using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . XtermConsole . EscapeSequenceParsers
{

	public static class EscapeSequences
	{

		public const char Esc = '\u001B' ;

		public static readonly char [ ] Csi = { Esc , '[' } ;

		public static readonly char [ ] Bel = { '\b' } ;

		public static readonly char [ ] Ss2 = { Esc , 'N' } ;

		public static readonly char [ ] Ss3 = { Esc , 'O' } ;

		public static readonly char [ ] St = { Esc , '\\' } ;

		public static readonly char [ ] Osc = { Esc , ']' } ;

	}

}
