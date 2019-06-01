using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using WenceyWang . FoggyConsole . Controls . Renderers ;

namespace WenceyWang . FoggyConsole . Controls
{

	/// <summary>
	///     Base class for
	///     <code>Label</code>
	///     ,
	///     <code>Button</code>
	///     and
	///     <code>CheckBox</code>
	///     .
	///     A control which is able to display a single line of text.
	/// </summary>
	public abstract class TextualBase : Control
	{

		private string _text = string . Empty ;

		/// <summary>
		///     Gets or sets the text which is drawn onto this Control.
		/// </summary>
		public string Text
		{
			get => _text ;
			set
			{
				if ( value == null )
				{
					throw new ArgumentNullException ( nameof ( value ) ) ;
				}

				if ( _text != value )
				{
					_text = value ;
					TextChanged ? . Invoke ( this , EventArgs . Empty ) ;
					RequestMeasure ( ) ;
				}
			}
		}

		public override Size AutoDesiredSize
		{
			get
			{
				string [ ] lines = Text . Split (
												Environment . NewLine . ToCharArray ( ) ,
												StringSplitOptions . None ) ;
				return new Size ( lines . Max ( str => str . Length ) , lines . Length ) ;
			}
		}

		protected TextualBase ( IControlRenderer renderer ) : base ( renderer ) { }

		public event EventHandler TextChanged ;

	}

}
