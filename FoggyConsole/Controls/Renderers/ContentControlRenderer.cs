using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . Controls . Renderers
{

	public class ContentControlRenderer : ControlRenderer <ContentControl>
	{

		public override void DrawOverride ( Application application , ConsoleArea area )
		{
			Control . Content ? . Draw ( application , area ) ;
		}

	}

	public class ContentControlRenderer <T> : ControlRenderer <T> where T : ContentControl
	{

		public override void DrawOverride( Application application , ConsoleArea area )
		{
			Control . Content ? . Draw ( application , area ) ;
		}

	}

}
