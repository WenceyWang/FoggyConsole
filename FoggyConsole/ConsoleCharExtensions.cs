using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole
{

	public static class ConsoleCharExtensions
	{

		public static ConsoleChar InvertColor ( this ConsoleChar character )
			=> new ConsoleChar (
								character . Character ,
								character . ForegroundColor . Invert ( ) ,
								character . BackgroundColor . Invert ( ) ) ;

	}

}