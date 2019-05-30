using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace WenceyWang . FoggyConsole . Controls . Renderers
{

	public class FrameRenderer : ControlRenderer <Frame>
	{

		public override void Draw ( )
		{
			ConsoleArea result = new ConsoleArea ( Control . Size , Control . ActualBackgroundColor ) ;
			FogConsole . Draw ( Control . RenderPoint , result ) ;
			Control . Content ? . Draw ( ) ;
		}

	}

}
