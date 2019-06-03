using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . Controls . Renderers
{

	// ReSharper disable once InconsistentNaming
	public class FIGletLabelRenderer : ControlRenderer <FIGletLabel>
	{

		public override void Draw ( )
		{
			ConsoleArea result = new ConsoleArea ( Control . ActualSize , Control . ActualBackgroundColor ) ;

			for ( int y = 0 ; y < result . Size . Height && y < Control . AsciiArt . Height ; y++ )
			{
				for ( int x = 0 ; x < result . Size . Width && x < Control . ActualText [ y ] . Length ; x++ )
				{
					result [ x , y ] = new ConsoleChar (
														Control . ActualText [ y ] [ x ] ,
														Control . ActualForegroundColor ,
														Control . ActualBackgroundColor ) ;
				}
			}

			FogConsole . Draw ( Control . RenderPoint , result ) ;
		}

	}

}
