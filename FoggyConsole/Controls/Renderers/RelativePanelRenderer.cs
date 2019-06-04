using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . Controls . Renderers
{

	public class RelativePanelRenderer : ControlRenderer <RelativePanel>
	{

		public override void Draw ( )
		{
			foreach ( Control control in Control . Items )
			{
				Control . Draw ( ) ;
			}
		}

	}

}
