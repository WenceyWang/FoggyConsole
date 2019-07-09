using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . Controls . Renderers
{

	public class ContentControlRenderer <T> : ControlRenderer <T> where T : ContentControl
	{

		public override void Draw ( ConsoleArea area ) { Control . Content ? . Draw ( area ) ; }

	}

}