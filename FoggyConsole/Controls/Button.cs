using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	/// <summary>
	///     A
	///     <code>Control</code>
	///     that can be triggered when focused.
	///     The standard look is:
	///     <example>[ Button Name ]</example>
	///     (drawn by
	///     <code>ButtonRenderer</code>
	///     ).
	///     If Width is zero, the button will use as much space as required.
	/// </summary>
	public class Button : TextualBase
	{

		private bool _allowSingleLine = true ;

		public bool AllowSingleLine
		{
			get => _allowSingleLine ;
			set
			{
				if ( _allowSingleLine != value )
				{
					_allowSingleLine = value ;
					RequestMeasure ( ) ;
				}
			}
		}

		public override Size AutoDesiredSize
		{
			get
			{
				Size baseSize = base . AutoDesiredSize ;

				if ( BoarderStyle is null )
				{
					return baseSize ;
				}
				else
				{
					if ( baseSize . Height == 1 && AllowSingleLine )
					{
						return new Size ( baseSize . Width + 2 , baseSize . Height ) ;
					}
					else
					{
						return baseSize + new Size ( 2 , 2 ) ;
					}
				}
			}
		}

		public override bool CanFocus => Enabled ;

		/// <summary>
		///     Creates a new
		///     <code>Button</code>
		/// </summary>
		/// <param name="renderer">
		///     The
		///     <code>ControlRenderer</code>
		///     to use. If null a new instance of
		///     <code>ButtonRenderer</code>
		///     will be used.
		/// </param>
		/// <exception cref="ArgumentException">
		///     Thrown if the
		///     <code>ControlRenderer</code>
		///     which should be set already has an other
		///     Control assigned
		/// </exception>
		public Button ( ControlRenderer <Button> renderer = null ) : base ( renderer ?? new ButtonRenderer ( ) )
			=> BoarderStyle = LineStyle . CornerOnlySingleLineSet ;

		/// <summary>
		///     Fired if the button is focused and the user presses the space bar
		/// </summary>
		public event EventHandler Pressed ;

		public override void KeyPressed ( KeyPressedEventArgs args )
		{
			if ( args . KeyInfo . Key == ConsoleKey . Spacebar )
			{
				args . Handled = true ;
				Pressed ? . Invoke ( this , EventArgs . Empty ) ;
			}
		}

	}

}
