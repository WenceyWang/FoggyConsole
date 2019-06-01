using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using WenceyWang . FoggyConsole . Controls . Renderers ;

namespace WenceyWang . FoggyConsole . Controls
{

	//Todo
	public class RelativePanel : ItemsContainer
	{

		public override bool CanFocus => false ;

		public RelativePanel ( IControlRenderer renderer ) : base ( renderer ?? new RelativePanelRenderer ( ) ) { }

		public override void Arrange ( Rectangle finalRect ) { base . Arrange ( finalRect ) ; }

		public override void Measure ( Size availableSize )
		{
			foreach ( Control control in Items )
			{
				control . Measure ( availableSize ) ;
			}

			base . Measure ( availableSize ) ;
		}

	}

}
