using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using WenceyWang . FoggyConsole . Controls . Renderers ;

namespace WenceyWang . FoggyConsole . Controls
{

	public class PageRenderer : ControlRenderer <Page>
	{

		public override void Draw ( ) { Control . Content ? . Draw ( ) ; }

	}

}
