using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace WenceyWang . FoggyConsole . Controls . Renderers
{

	/// <summary>
	///     Draws a
	///     <code>Canvas</code>
	///     , which has no own appearance.
	///     All controls within the panel are drawn.
	/// </summary>
	public class CanvasRenderer : ControlRenderer <Canvas>
	{

		public override void Draw ( )
		{
			foreach ( Control control in Control . Items )
			{
				control . Draw ( ) ;
			}
		}

	}

}
