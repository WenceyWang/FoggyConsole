using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using WenceyWang . FoggyConsole . Controls . Renderers ;

namespace WenceyWang . FoggyConsole . Controls
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