using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using WenceyWang . FoggyConsole . Controls . Renderers ;

namespace WenceyWang . FoggyConsole . Controls
{

	public class Grid : ItemsContainer
	{

		public override bool CanFocus => false ;

		public Grid ( IControlRenderer renderer ) : base ( renderer ?? new GridRenderer ( ) ) { }

	}

}
