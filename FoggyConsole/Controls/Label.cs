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

		private ContentAlign _align ;

		/// <summary>
		///     The align of the text
		/// </summary>
		public ContentAlign Align
		{
			get => _align ;
			set
			{
				if ( _align != value )
				{
					_align = value ;
					Draw ( ) ;
				}
			}
		}

		public override Size Size
		{
			get
			{
				if ( base . Size == new Size ( 0 , 0 ) )
				{
					return new Size ( Text . Length , 1 ) ;
				}

				return base . Size ;
			}
			set => base . Size = value ;
		}

		public override bool CanFocus => false ;

		/// <summary>
		///     Creates a new
		///     <code>Label</code>
		/// </summary>
		/// <param name="text">The text on the Label</param>
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
		public Label ( IControlRenderer renderer = null ) : base ( renderer ?? new LabelRenderer ( ) )
			=> BoarderStyle = LineStyle . Empty ;

	}

}
