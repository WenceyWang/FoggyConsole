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

		public override Size Size
		{
			get => Application . Console . Size ;
			set => Application . Console . Size = value ;
		}

		public override bool CanFocusedOn => false ;

		public RootFrame ( IControlRenderer renderer = null ) : base ( renderer ?? new RootFrameRenderer ( ) )
			=> Enabled = false ;


		public Application Application { get ; set ; }

		public void PauseRedraw ( ) { RedrawPausedLevel++ ; }

		public void ResumeRedraw ( )
		{
			RedrawPausedLevel = Math . Max ( RedrawPausedLevel - 1 , 0 ) ;

			if ( RedrawPausedLevel == 0 )
			{
				RequestUpdateDisplay ( ) ;
			}
		}

		protected override void RequestMeasure ( )
		{
			if ( RedrawPausedLevel == 0 && Enabled )
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
			if ( RedrawPausedLevel == 0 && Enabled )
			{
				RedrawPausedLevel++ ;

				Draw ( ) ;

				RedrawPausedLevel = Math . Max ( RedrawPausedLevel - 1 , 0 ) ;
			}
		}

		public void RequestUpdateDisplay ( ) { RequestMeasure ( ) ; }

		private void Draw ( ) { Draw ( Application , new ConsoleArea ( Size ) ) ; }

	}

}
