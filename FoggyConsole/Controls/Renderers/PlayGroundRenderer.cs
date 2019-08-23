using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . Controls . Renderers
{

	/// <summary>
	///     Draws an
	///     <code>Playground</code>
	/// </summary>
	public class PlayGroundRenderer : ControlRenderer <PlayGround>
	{

		/// <summary>
		///     Draws all characters within the given playground
		/// </summary>
		public override void Draw ( Application application , ConsoleArea area )
		{
			for ( int y = 0 ; y < Control . Height ; y++ )
			{
				for ( int x = 0 ; x < Control . Width ; x++ )
				{
					area [ x , y ] = Control [ x , y ] ;
				}
			}
		}

	}

}
