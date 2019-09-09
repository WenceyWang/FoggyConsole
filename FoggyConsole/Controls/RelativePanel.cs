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

		public override bool CanFocusedOn => false ;

		public RelativePanel ( IControlRenderer renderer ) : base ( renderer ?? new ItemsContainerRenderer ( ) ) { }

		public RelativePanel ( ) : this ( null ) { }

		public override void ArrangeOverride ( Rectangle finalRect ) { RenderArea = finalRect; }

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
