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

	public class RootFrame : ContentControl , IApplicationItem
	{

		private int _redrawPausedLevel = 1 ;

		public bool UpdateDisplayRequested ;

		public int RedrawPausedLevel
		{
			get => _redrawPausedLevel ;
			private set
			{
				_redrawPausedLevel = Math . Max ( value , 0 ) ;
				Logger . LogTrace ( $"{nameof ( RedrawPausedLevel )} : {_redrawPausedLevel}" ) ;
			}
		}

		internal ILogger Logger { get ; } =
			StaticServiceProvider . Provider . GetService <ILoggerFactory> ( ) . CreateLogger <Frame> ( ) ;

		public override Size Size
		{
			get => Application . Console . Size ;
			set => Application . Console . Size = value ;
		}

		public override bool CanFocusedOn => false ;

		public override RootFrame ViewRoot => this ;

		public RootFrame ( IControlRenderer renderer = null ) : base ( renderer ?? new RootFrameRenderer ( ) )
			=> Enabled = false ;

		public Application Application { get ; set ; }

		public void PauseRedraw ( ) { RedrawPausedLevel++ ; }

		public void ResumeRedraw ( )
		{
			RedrawPausedLevel-- ;
			RequestUpdateDisplay ( ) ;
		}

		protected override void RequestMeasure ( )
		{
			if ( RedrawPausedLevel == 0 && Enabled )
			{
				RedrawPausedLevel++ ;

				Measure ( Size ) ;
				Arrange ( new Rectangle ( Size ) ) ;
				Draw ( ) ;

				RedrawPausedLevel-- ;

				if ( UpdateDisplayRequested )
				{
					UpdateDisplayRequested = false ;
					RequestUpdateDisplay ( ) ;
				}
			}
			else
			{
				UpdateDisplayRequested = true ;
			}
		}

		protected override void RequestRedraw ( )
		{
			if ( RedrawPausedLevel == 0 && Enabled )
			{
				RedrawPausedLevel++ ;

				Draw ( ) ;

				RedrawPausedLevel-- ;
				if ( UpdateDisplayRequested )
				{
					UpdateDisplayRequested = false ;
					RequestUpdateDisplay ( ) ;
				}
			}
			else
			{
				UpdateDisplayRequested = true ;
			}
		}

		public void RequestUpdateDisplay ( ) { RequestMeasure ( ) ; }

		private void Draw ( ) { Draw ( Application , new ConsoleArea ( Size ) ) ; }

	}

}
