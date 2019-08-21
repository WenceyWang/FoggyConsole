using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{
    public class Frame : ContentControl
	{
		
		public override bool CanFocusedOn => false ;

		public Page CurrentPage { get ; private set ; }

		public override Control Content
		{
			get => CurrentPage ;
			set => throw new InvalidOperationException ( ) ;
		}

		public Frame ( IControlRenderer renderer = null ) : base (
																renderer ?? new FrameRenderer ( ) )
		{
			Enabled = false ;
		}

		public Frame ( ) : this ( null ) { }

		public override void Measure ( Size availableSize )
		{
			CurrentPage ? . Measure ( availableSize ) ;
		}

		public override void ArrangeOverride ( Rectangle finalRect )
		{
			CurrentPage ? . Arrange ( new Rectangle ( Size ) ) ;
		}



		public void NavigateTo ( Page page )
		{
			if ( page == null )
			{
				throw new ArgumentNullException ( nameof ( page ) ) ;
			}

			if ( CurrentPage != page )
			{
				CurrentPage             = page ;
				CurrentPage . Container = this ;
				CurrentPage . OnNavigateTo ( ) ;
			}
		}

	}

}
