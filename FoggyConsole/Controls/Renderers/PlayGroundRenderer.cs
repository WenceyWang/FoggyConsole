using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace WenceyWang . FoggyConsole . Controls . Renderers
{

	/// <summary>
	///     Draws an
	///     <code>Playground</code>
	/// </summary>
	public class PlayGroundRenderer : ControlRenderer <PlayGround>
	{

		/// <summary>
		/// </summary>
		/// <param name="control"></param>
		public PlayGroundRenderer ( PlayGround control ) { }

		/// <summary>
		///     Draws all characters within the given playground
		/// </summary>
		public override void Draw ( )
		{
			for ( int y = 0 ; y < Control . Height ; y++ )
			{
				for ( int x = 0 ; x < Control . Width ; x++ )
				{
					//FogConsole . Write ( Boundary . Left ,
					//				Boundary . Top + y ,
					//				Control [ y , x ] ,
					//				Boundary ,
					//				Control . ForegroundColor , Control . BackgroundColor );
				}
			}
		}

	}

}
