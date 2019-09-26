using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole
{

	public interface IConsole : IApplicationItem
	{

		Size Size { get ; set ; }

		void Start ( ) ;

		void Stop ( ) ;

		void Draw ( ConsoleArea area ) ;

		event EventHandler <ConsoleSizeChangedEvnetArgs> SizeChanged ;

		void Bell ( ) ;

		event EventHandler <KeyPressedEventArgs> KeyPressed ;

	}

}
