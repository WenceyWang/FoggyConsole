using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . Controls . Renderers
{

	/// <summary>
	///     Represents a class which is able to draw an specific type of control.
	/// </summary>
	public interface IControlRenderer
	{

		/// <summary>
		///     The Control which should be drawn
		/// </summary>
		Control Control { get ; set ; }

		/// <summary>
		///     Draws the Control stored in the Control-Property
		/// </summary>
		void Draw ( ) ;

	}

}
