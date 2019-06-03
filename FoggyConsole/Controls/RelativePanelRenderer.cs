using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
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
