using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . Controls . Renderers
{

	public class ItemsContainerRenderer <T> : ControlRenderer <T> where T : ItemsContainer
	{

		public override void Draw ( ConsoleArea area )
		{
			foreach ( Control control in Control . Items )
			{
				if ( ! control . RenderArea . IsEmpty )
				{
					control . Draw ( area . CreateSub ( control . RenderArea ) ) ;
				}
			}
		}

	}

}