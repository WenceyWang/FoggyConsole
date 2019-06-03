using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	public class Grid : ItemsContainer
	{

		public override bool CanFocus => false ;

		public Grid ( IControlRenderer renderer ) : base ( renderer ?? new GridRenderer ( ) ) { }

	}

}
