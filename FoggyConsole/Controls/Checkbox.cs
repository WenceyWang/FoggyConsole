using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	/// <summary>
	///     A control which displays a Text and has a State-State
	/// </summary>
	public class CheckBox : CheckableBase
	{

		public override bool CanFocus => Enabled ;

		/// <summary>
		///     Creates a new CheckBox
		/// </summary>
		/// <param name="renderer">
		///     The
		///     <code>ControlRenderer</code>
		///     to use. If null a new instance of
		///     <code>CheckboxRenderer</code>
		///     will be used.
		/// </param>
		/// <exception cref="ArgumentException">
		///     Thrown if the
		///     <code>ControlRenderer</code>
		///     which should be set already has an other
		///     Control assigned
		/// </exception>
		public CheckBox ( CheckboxRenderer renderer = null ) : base ( renderer ?? new CheckboxRenderer ( ) )
			=> State = CheckState . Indeterminate ;

		public CheckBox ( ) : this ( null ) { }

	}

}
