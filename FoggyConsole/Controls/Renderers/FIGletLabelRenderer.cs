using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . Controls . Renderers
{

	// ReSharper disable once InconsistentNaming
	// ReSharper disable once IdentifierTypo
	public class FIGletLabelRenderer : ControlRenderer <FIGletLabel>
	{

		public override void Draw ( Application application , ConsoleArea area )
		{
			area . Fill ( Control . ActualBackgroundColor ) ;

			for ( int y = 0 ; y < area . Size . Height && y < Control . AsciiArt . Height ; y++ )
			{
				for ( int x = 0 ; x < area . Size . Width && x < Control . ActualText [ y ] . Length ; x++ )
				{
					area [ x , y ] = new ConsoleChar (
													  Control . ActualText [ y ] [ x ] ,
													  Control . ActualForegroundColor ,
													  Control . ActualBackgroundColor ) ;
				}
			}
		}

	}

}
