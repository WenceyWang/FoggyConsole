using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	public class PageRenderer : ControlRenderer <Page>
	{

		public override void Draw ( ) { Control . Content ? . Draw ( ) ; }

	}

}
