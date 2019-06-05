using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	public class ContentControlRenderer <T> : ControlRenderer <T> where T : ContentControl
	{

		public override void Draw ( ConsoleArea area ) { Control . Content ? . Draw ( area ) ; }

	}


	public class PageRenderer : ContentControlRenderer <Page>
	{

	}

}
