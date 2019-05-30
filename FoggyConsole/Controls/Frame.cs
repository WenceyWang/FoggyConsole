using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using WenceyWang . FoggyConsole . Controls . Renderers ;

namespace WenceyWang . FoggyConsole . Controls
{

	public class Frame : ContentControl
	{

		public override Size Size
		{
			get =>  Window.Size ;
			set =>  Window.Size = value ;
		}

		public override bool CanFocus => false ;

		public Page CurrentPage { get ; private set ; }

		public override Control Content { get => CurrentPage ; set => throw new InvalidOperationException ( ) ; }

		public Frame ( IControlRenderer renderer = null ) : base ( renderer ?? new FrameRenderer ( ) ) { }

		public override void Measure ( Size availableSize ) { CurrentPage ? . Measure ( availableSize ) ; }

		public override void Arrange ( Rectangle finalRect ) { CurrentPage ? . Arrange ( new Rectangle ( Size ) ) ; }

		protected override void RequestMeasure ( )
		{
			Measure ( Size ) ;
			Arrange ( new Rectangle ( Size ) ) ;
			Draw ( ) ;
		}

		protected override void RequestRedraw ( ) { Draw ( ) ; }

		public void NavigateTo ( Page page )
		{
			if ( page == null )
			{
				throw new ArgumentNullException ( nameof(page) ) ;
			}

			if ( CurrentPage != page )
			{
				CurrentPage = page ;
				CurrentPage . Container = this ;
				CurrentPage . OnNavigateTo ( ) ;
				Measure ( Size ) ;
				Arrange ( new Rectangle ( Size ) ) ;
				Draw ( ) ;
			}
		}

	}

}
