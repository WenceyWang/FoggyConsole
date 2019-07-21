using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . Controls . Renderers
{

	public class ItemsContainerRenderer : ControlRenderer <ItemsContainer>
	{

		public override void Draw ( ConsoleArea area )
		{
			foreach ( Control control in Control . Items )
			{
				if ( control . RenderArea . IsNotEmpty ( ) )
				{
					control . Draw ( area . CreateSub ( control . RenderArea . Value ) ) ;
				}
			}
		}

	}

	public class ItemsContainerRenderer <T> : ControlRenderer <T> where T : ItemsContainer
	{

		public override void Draw ( ConsoleArea area )
		{
			foreach ( Control control in Control . Items )
			{
				if ( control . RenderArea . IsNotEmpty ( ) )
				{
					control . Draw ( area . CreateSub ( control . RenderArea . Value ) ) ;
				}
			}
		}

	}

}
