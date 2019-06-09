using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	//Todo
	public class RelativePanel : ItemsContainer
	{

		public override bool CanFocus => false ;

		public RelativePanel ( IControlRenderer renderer ) : base ( renderer ?? new RelativePanelRenderer ( ) ) { }

		public RelativePanel ( ) : this ( null ) { }

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
