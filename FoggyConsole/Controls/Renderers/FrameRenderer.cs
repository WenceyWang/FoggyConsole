using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using Microsoft . Extensions . Logging ;

namespace DreamRecorder . FoggyConsole . Controls . Renderers
{

	public class FrameRenderer : ControlRenderer <Frame>
	{

		public override void Draw ( ConsoleArea area )
		{
			FogConsole . WriteCount = 0 ;

			area . Fill ( Control . ActualBackgroundColor ) ;

			Control . Content ? . Draw ( area ) ;

			FogConsole . Draw ( Control ? . RenderPoint ?? Point . Zero , area ) ;

			Frame . Current . Logger . LogTrace ( "Redraw with {0} writes." , FogConsole . WriteCount ) ;
		}

	}

}
