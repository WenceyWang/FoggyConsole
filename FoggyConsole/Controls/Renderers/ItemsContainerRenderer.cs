using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . Controls . Renderers
{

	public class ItemsContainerRenderer : ControlRenderer <ItemsContainer>
	{

		public override void DrawOverride ( Application application , ConsoleArea area )
		{
			foreach ( Control control in Control . Items )
			{
				if ( control . RenderArea . IsNotEmpty ( ) )
				{
					control . Draw ( application , area . CreateSub ( control . RenderArea . GetValueOrDefault ( ) ) ) ;
				}
			}
		}

	}

	public class ItemsContainerRenderer <T> : ControlRenderer <T> where T : ItemsContainer
	{

		public override void DrawOverride ( Application application , ConsoleArea area )
		{
			foreach ( Control control in Control . Items )
			{
				if ( control . RenderArea . IsNotEmpty ( ) )
				{
					control . Draw ( application , area . CreateSub ( control . RenderArea . GetValueOrDefault ( ) ) ) ;
				}
			}
		}

	}

}
