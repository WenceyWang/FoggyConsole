using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace WenceyWang . FoggyConsole . Controls . Renderers
{

	public class StackPanelRenderer : ControlRenderer <StackPanel>
	{

		public override void Draw ( )
		{
			foreach ( Control control in Control . Items )
			{
				control . Draw ( ) ;
			}
		}

	}

}
