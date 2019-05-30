using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using WenceyWang . FoggyConsole . Controls . Renderers ;

namespace WenceyWang . FoggyConsole . Controls
{

	public abstract class ContentControl : Container
	{

		public abstract Control Content { get ; set ; }

		public override IList <Control> Children
		{
			get
			{
				List <Control> children = new List <Control> ( ) ;
				if ( Content != null )
				{
					children . Add ( Content ) ;
				}
				return children ;
			}
		}


		protected ContentControl ( IControlRenderer renderer ) : base ( renderer ) { }

	}

}