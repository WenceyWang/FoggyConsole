using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;
using DreamRecorder . ToolBox . General ;

using Microsoft . Extensions . DependencyInjection ;
using Microsoft . Extensions . Logging ;

namespace DreamRecorder . FoggyConsole . Controls
{

	public class Frame : ContentControl
	{

		private int _redrawPausedLevel = 1 ;

		public static Frame Current { get ; set ; }

		public int RedrawPausedLevel
		{
			get => _redrawPausedLevel ;
			private set
			{
				_redrawPausedLevel = value ;
				Logger . LogTrace ( $"{nameof ( RedrawPausedLevel )} : {value}" ) ;
			}
		}

		internal ILogger Logger { get ; } =
			StaticServiceProvider . Provider . GetService <ILoggerFactory> ( ) . CreateLogger <Frame> ( ) ;

		public override Size Size { get => Window . Size ; set => Window . Size = value ; }

		public override bool CanFocusedOn => false ;

		public Page CurrentPage { get ; private set ; }

		public override Control Content { get => CurrentPage ; set => throw new InvalidOperationException ( ) ; }

		public Frame ( IControlRenderer renderer = null ) : base ( renderer ?? new FrameRenderer ( ) )
		{
			Enabled = false ;
			Current = this ;
		}

		public Frame ( ) : this ( null ) { }

		public void PauseRedraw ( ) { RedrawPausedLevel++ ; }

		public void ResumeRedraw ( )
		{
			RedrawPausedLevel = Math . Max ( RedrawPausedLevel - 1 , 0 ) ;

			if ( RedrawPausedLevel == 0 )
			{
				RequestUpdateDisplay ( ) ;
			}
		}

		public override void Measure ( Size availableSize ) { CurrentPage ? . Measure ( availableSize ) ; }

		public override void Arrange ( Rectangle finalRect ) { CurrentPage ? . Arrange ( new Rectangle ( Size ) ) ; }

		public void RequestUpdateDisplay ( ) { RequestMeasure ( ) ; }

		protected override void RequestMeasure ( )
		{
			if ( ( RedrawPausedLevel == 0 ) && Enabled )
			{
				RedrawPausedLevel++ ;

				Measure ( Size ) ;
				Arrange ( new Rectangle ( Size ) ) ;
				Draw ( ) ;

				RedrawPausedLevel = Math . Max ( RedrawPausedLevel - 1 , 0 ) ;
			}
		}

		protected override void RequestRedraw ( )
		{
			if ( ( RedrawPausedLevel == 0 ) && Enabled )
			{
				RedrawPausedLevel++ ;

				Draw ( ) ;

				RedrawPausedLevel = Math . Max ( RedrawPausedLevel - 1 , 0 ) ;
			}
		}

		private void Draw ( ) { Draw ( new ConsoleArea ( Size ) ) ; }

		public void NavigateTo ( Page page )
		{
			if ( page == null )
			{
				throw new ArgumentNullException ( nameof ( page ) ) ;
			}

			if ( CurrentPage != page )
			{
				PauseRedraw ( ) ;
				CurrentPage             = page ;
				CurrentPage . Container = this ;
				CurrentPage . OnNavigateTo ( ) ;
				ResumeRedraw ( ) ;
			}
		}

	}

}
