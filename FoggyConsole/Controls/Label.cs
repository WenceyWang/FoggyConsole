using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	/// <summary>
	///     A control which displays one line of text
	/// </summary>
	public class Label : TextualBase
	{

		public override bool CanFocusedOn => false ;

		/// <summary>
		///     Creates a new
		///     <code>Label</code>
		/// </summary>
		/// <param name="renderer">
		///     The
		///     <code>ControlRenderer</code>
		///     to use. If null a new instance of
		///     <code>LabelRenderer</code>
		///     will be used.
		/// </param>
		/// <exception cref="ArgumentException">
		///     Thrown if the
		///     <code>ControlRenderer</code>
		///     which should be set already has an other
		///     Control assigned
		/// </exception>
		public Label ( IControlRenderer renderer = null ) : base ( renderer ?? new LabelRenderer ( ) ) { }

		public Label ( ) : this ( null ) { }

	}

}
