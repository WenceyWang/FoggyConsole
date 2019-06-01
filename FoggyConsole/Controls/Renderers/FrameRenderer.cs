using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using Microsoft . Extensions . Logging ;

namespace WenceyWang . FoggyConsole . Controls . Renderers
{

	public class FrameRenderer : ControlRenderer <Frame>
	{

		public override void Draw ( )
		{
			FogConsole . WriteCount = 0 ;

			ConsoleArea result = new ConsoleArea ( Control . Size , Control . ActualBackgroundColor ) ;
			FogConsole . Draw ( Control . RenderPoint , result ) ;
			Control . Content ? . Draw ( ) ;
			Frame . Current . Logger . LogTrace ( "Redraw with {0} writes." , FogConsole . WriteCount ) ;
		}

	}

}
