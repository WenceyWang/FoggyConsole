using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . Controls . Renderers
{

	public class RootFrameRenderer : ControlRenderer <RootFrame>
	{

		public override void DrawOverride ( Application application , ConsoleArea area )
		{
			area . Fill ( Control . ActualBackgroundColor ) ;

			Control . Content ? . Draw ( application , area ) ;

			Rectangle ? focusedArea = application . FocusManager ? . FocusedControl ? . RenderArea ;

			if ( focusedArea . IsNotEmpty ( ) )
			{
				area . CreateSub ( focusedArea . Value ) . InvertColor ( ) ;
			}

			application . Console . Draw ( area ) ;
		}

	}

}
