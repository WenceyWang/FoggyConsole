using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	public class ConsolePresenter : PlayGround , IConsole
	{

		public override bool CanFocusedOn => true ;

		public ConsolePresenter ( IControlRenderer renderer ) : base ( renderer ) { }

		public Application Application { get ; set ; }

		public void Start ( ) { AutoRedraw = true ; }

		public void Stop ( ) { AutoRedraw = false ; }

		public void Draw ( ConsoleArea area )
		{
			Rectangle resultPosition = area . Position . Intersect ( Buffer . Position ) ;
			for ( int y = resultPosition . Top ; y <= resultPosition . Bottom ; y++ )
			{
				for ( int x = resultPosition . Left ; x <= resultPosition . Right ; x++ )
				{
					Buffer [ x , y ] = area [ x , y ] ;
				}
			}

			Redraw ( ) ;
		}

		public event EventHandler <ConsoleSizeChangedEvnetArgs> SizeChanged ;

		public void Bell ( ) { ViewRoot ? . Application ? . Console ? . Bell ( ) ; }

		public event EventHandler <KeyPressedEventArgs> KeyPressed ;

		public override void ArrangeOverride ( Rectangle finalRect )
		{
			if ( RenderArea != finalRect )
			{
				Size oldSize = RenderArea ? . Size ?? default ;

				RenderArea = finalRect ;
				SizeChanged ? . Invoke ( this , new ConsoleSizeChangedEvnetArgs ( finalRect . Size , oldSize ) ) ;
			}
		}

		public override void OnKeyPressed ( KeyPressedEventArgs args ) => KeyPressed ? . Invoke ( this , args ) ;

	}

}
